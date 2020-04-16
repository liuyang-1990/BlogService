using AspNetCoreRateLimit;
using Blog.Api.Filters;
using Blog.Api.Hubs;
using Blog.Api.MiddleWares;
using Blog.Infrastructure.DI;
using Blog.Infrastructure.ServiceCollectionExtension;
using Blog.Model.Seed;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Prometheus;
using SqlSugar;
using StackExchange.Profiling;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Blog.Api
{
    /// <summary>
    /// 启动配置类
    /// </summary>
    public class Startup
    {

        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        /// <summary>
        /// Configuration属性
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Hosting Environment属性
        /// </summary>
        public IWebHostEnvironment Env { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public void ConfigureServices(IServiceCollection services)
        {

            // needed to load configuration from appsettings.json
            services.AddOptions();
            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();
            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));
            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.AddControllers(options =>
            {
                if (!Env.IsDevelopment())
                {
                    options.Filters.Add<ServiceExceptionFilterAttribute>();
                }
                options.Filters.Add<ParamsProtectionResultFilter>();
            }).AddNewtonsoftJson(option =>
            {
                option.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            #region  URL 地址转换成小写的形式
            services.AddRouting(options =>
            {
                options.LowercaseUrls = true;
            });
            #endregion

            #region MiniProfiler
            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                options.PopupRenderPosition = RenderPosition.Left;
                options.PopupShowTimeWithChildren = true;
            });


            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("LimitRequests", policy =>
                {
                    policy
                        .WithOrigins(Configuration["Origins"].Split(",", StringSplitOptions.RemoveEmptyEntries))
                        .AllowAnyMethod() //允许任何方式
                        .AllowAnyHeader() //允许任何头
                        .WithExposedHeaders("Authorization") //返回自定义Header
                        .AllowCredentials();
                });
            });

            #endregion

            #region Swagger

            //services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "Blog Service",
                    Contact = new OpenApiContact() { Name = "Liu Yang", Email = "xy_liuy0305@163.com" },
                    TermsOfService = new Uri("https://api.nayoung515.top/swagger"),
                    License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
                });

                options.CustomSchemaIds(x => x.FullName);
                options.OperationFilter<SwaggerDefaultValues>();
                var basePath = AppDomain.CurrentDomain.BaseDirectory;
                var xmlPath = Path.Combine(basePath, "Blog.Api.xml");
                options.IncludeXmlComments(xmlPath, true);
                //添加header验证信息

                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();

                options.OperationFilter<SecurityRequirementsOperationFilter>();
                //var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                //options.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = ParameterLocation.Header,//jwt默认存放Authorization信息的位置(请求头中)
                    Type = SecuritySchemeType.ApiKey
                });

            });

            #endregion

            //认证  
            AuthConfigurer.Configure(services, Configuration);

            #region 授权
            //  授权，就是根据令牌反向去解析出的用户身份，回应当前http请求的许可，表示可以使用当前接口，或者拒绝访问
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
            });
            #endregion

            #region TinyMapper
            Mapper.InitMapping();
            #endregion

            //使用微软的分布式缓存
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisCaching:ConnectionString"];
                options.InstanceName = "blog";
            });

            #region SqlSugarDbContext
            services.AddSqlSugarDbContext(options =>
            {
                options.DbType = (DbType)Enum.Parse(typeof(DbType), Configuration["ConnectionStrings:DbType"]);
                options.ConnectionString = Configuration["ConnectionStrings:ConnectionString"];
                options.IsAutoCloseConnection = true;
                options.InitKeyType = InitKeyType.Attribute;
                options.IsShardSameThread = true;
                options.AopEvents = new AopEvents()
                {
                    OnLogExecuting = (sql, pars) =>
                    {
                        var sqlP = sql + "\r\n" + JsonConvert.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                        MiniProfiler.Current.CustomTiming("[SQL]:", sqlP);
                    }
                };
            });
            #endregion
            services.AddHttpClient();

            // the IHttpContextAccessor service is not registered by default.
            // the clientId/clientIp resolvers use it.
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            // configuration (resolvers, counter key builders)
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

            services.AddResponseCompression();
            services.AddSignalR();
            #region Seed
            services.AddScoped(typeof(SeedHelper));
            #endregion

            #region API版本控制
            //services.AddApiVersioning(options =>
            //{
            //    // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
            //    options.ReportApiVersions = true;
            //});

            //services.AddVersionedApiExplorer(options =>
            //{
            //    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
            //    // note: the specified format code will format the version as "'v'major[.minor][-status]"
            //    options.GroupNameFormat = "'v'VVV";

            //    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
            //    // can also be used to control the format of the API version in route templates
            //    options.SubstituteApiVersionInUrl = true;
            //});
            #endregion

            #region Ioc
            CoreContainer.Current.BuildServiceProvider(services);
            #endregion

            var seed = CoreContainer.Current.GetService<SeedHelper>();
            seed.SeedAsync().ConfigureAwait(false);
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app)
        {
            //Ip限流
            app.UseIpRateLimiting();
            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.DefaultModelRendering(ModelRendering.Example);
                //hide the expansion models
                option.DefaultModelsExpandDepth(-1);
                option.DefaultModelExpandDepth(2);
                option.DocExpansion(DocExpansion.List);
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "Blog.Api");
                //foreach (var description in provider.ApiVersionDescriptions)
                //{
                //    option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                //}
                option.IndexStream = () => Assembly.GetExecutingAssembly().GetManifestResourceStream("Blog.Api.wwwroot.swagger.ui.index.html");
            });
            #endregion

            // 返回错误码
            app.UseStatusCodePages();
            //miniProfiler
            app.UseMiniProfiler();

            #region Metrics
            app.UseMetricServer();
            app.UseHttpMetrics();
            #endregion
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            //跨域
            app.UseCors("LimitRequests");


            //自定义认证
            //app.UseMiddleware<AuthenticationMiddleware>();
            //认证
            app.UseAuthentication();

            app.UseAuthorization();

            app.UseDataProtectMiddleware();
            app.UseResponseCompression();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chatHub");
            });
        }
    }



}
