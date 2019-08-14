using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AutoMapper;
using Blog.Api.Filters;
using Blog.Api.Interceptors;
using Blog.Api.SwaggerExtensions;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using StackExchange.Profiling.Storage;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Blog.Infrastructure.Extensions;
using Prometheus;
using StackExchange.Profiling;

namespace Blog.Api
{
    /// <summary>
    /// 启动配置类
    /// </summary>
    public class Startup
    {

        public Startup(IConfiguration configuration, IHostingEnvironment env)
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
        public IHostingEnvironment Env { get; }
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc(options =>
            {
                if (!Env.IsDevelopment())
                {
                    options.Filters.Add(new ServiceExceptionFilterAttribute());
                }

            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
              .AddJsonOptions(options =>
            {
                //asp.net core default use CamelCaseNamingStrategy, we disable it.
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
                // options.SerializerSettings.Formatting= Formatting.None;
            });

            #region  URL 地址转换成小写的形式
            services.AddRouting(options =>
              {
                  options.LowercaseUrls = true;
              });
            #endregion
            //services.Configure<DbSetting>(Configuration.GetSection("ConnectionStrings"));

            #region MiniProfiler
            services.AddMiniProfiler(options =>
             {
                 options.RouteBasePath = "/profiler";
                 if (options.Storage is MemoryCacheStorage memoryCacheStorage)
                 {
                     memoryCacheStorage.CacheDuration = TimeSpan.FromMinutes(10);
                 }
             });


            #endregion

            #region 跨域
            services.AddCors(options =>
            {
                options.AddPolicy("LimitRequests", policy =>
                {
                    policy
                        .WithOrigins("https://www.nayoung515.top", "https://admin.nayoung515.top", Configuration["Origins"])
                        .AllowAnyMethod() //允许任何方式
                        .AllowAnyHeader() //允许任何头
                        .WithExposedHeaders("Authorization") //返回自定义Header
                        .AllowCredentials();
                });
            });

            #endregion

            #region Swagger

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(options =>
             {
                 options.CustomSchemaIds(x => x.FullName);
                 options.DescribeAllEnumsAsStrings();
                 options.OperationFilter<SwaggerDefaultValues>();
                 var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                 var xmlPath = Path.Combine(basePath, "Blog.Api.xml");
                 options.IncludeXmlComments(xmlPath, true);
                 //添加header验证信息
                 var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                 options.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                 options.AddSecurityDefinition("Bearer", new ApiKeyScheme
                 {
                     Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                     Name = "Authorization",//jwt默认的参数名称
                     In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                     Type = "apiKey"
                 });

             });

            #endregion

            #region 认证  
            // 认证，就是根据登录的时候，生成的令牌，检查其是否合法，这个主要是证明没有被篡改
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = Configuration["JwtAuth:Issuer"],
                    ValidAudience = Configuration["JwtAuth:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtAuth:SecurityKey"])),
                    RequireSignedTokens = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
            #endregion

            #region 授权
            //  授权，就是根据令牌反向去解析出的用户身份，回应当前http请求的许可，表示可以使用当前接口，或者拒绝访问
            services.AddAuthorization(options =>
              {
                  options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
              });
            #endregion

            #region AutoMapper
            services.AddAutoMapper(typeof(Startup));
            #endregion

            //使用微软的分布式缓存
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration["RedisCaching:ConnectionString"];
                options.InstanceName = "blog";
            });

            services.AddSqlSugarDbContext(options =>
            {
                options.DbType = DbType.MySql;
                options.ConnectionString = Configuration["ConnectionStrings:ConnectionString"];
                options.IsAutoCloseConnection = true;
                options.InitKeyType = InitKeyType.SystemTable;
                options.AopEvents = new AopEvents()
                {
                    OnLogExecuting = (sql, pars) =>
                    {
                        var sqlP = sql + "\r\n" + JsonConvert.SerializeObject(pars.ToDictionary(it => it.ParameterName, it => it.Value));
                        MiniProfiler.Current.CustomTiming("[SQL]:", sqlP);
                    }
                };
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            services.AddMemoryCache();
            #region AOP

            services.ConfigureDynamicProxy(config =>
              {
                  config.Interceptors.AddTyped<BlogRedisCacheInterceptor>();
                  config.Interceptors.AddTyped<MiniProfilerInterceptor>();
              });
            #endregion

            #region HealthCheck

            var drivers = DriveInfo.GetDrives();
            var targetDriver = drivers.FirstOrDefault(x => x.DriveType == DriveType.Fixed);
            services.AddHealthChecks()
                   .AddPrivateMemoryHealthCheck(1000_000_000L)
                   .AddVirtualMemorySizeHealthCheck(1000_000_000L)
                   .AddWorkingSetHealthCheck(1000_000_000L)
                   .AddRedis(Configuration["RedisCaching:ConnectionString"])
                   .AddDiskStorageHealthCheck(x => x.AddDrive(targetDriver?.Name, 1000))
                   .AddMySql(Configuration["ConnectionStrings:ConnectionString"]);

            #endregion

            #region API版本控制
            services.AddApiVersioning(options =>
               {
                   // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                   options.ReportApiVersions = true;
               });

            services.AddVersionedApiExplorer(
                options =>
                {
                    // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                    // note: the specified format code will format the version as "'v'major[.minor][-status]"
                    options.GroupNameFormat = "'v'VVV";

                    // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                    // can also be used to control the format of the API version in route templates
                    options.SubstituteApiVersionInUrl = true;
                });
            #endregion

            #region Ioc
            //var builder = new ContainerBuilder();
            //builder.Populate(services);
            //builder.RegisterType<BlogRedisCacheAOP>();
            //builder.RegisterType<MiniProfilerAOP>();
            ////新模块组件注册    
            //builder.RegisterModule<AutofacModuleRegister>();

            ////创建容器
            //var container = builder.Build();
            ////第三方IOC接管 core内置DI容器 
            //return new AutofacServiceProvider(container);
            // return CoreContainer.Init(services);
            return AspectCoreContainer.BuildServiceProvider(services);
            #endregion


        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app, IApiVersionDescriptionProvider provider)
        {

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Blog.Api.index.html");
            });
            #endregion

            //跨域
            app.UseCors("LimitRequests");

            //自定义认证
            app.UseMiddleware<AuthenticationMiddleware>();
            //认证
            app.UseAuthentication();

            // 返回错误码
            app.UseStatusCodePages();
            //miniProfiler
            app.UseMiniProfiler();

            #region Metrics
            app.UseMetricServer();
            app.UseHttpMetrics(); 
            #endregion

            #region HealthCheck
            app.UseHealthChecks("/healthz", new HealthCheckOptions()
            {
                ResponseWriter = async (context, report) =>
                {
                    var result = JsonConvert.SerializeObject(
                        new
                        {
                            status = report.Status.ToString(),
                            errors = report.Entries.Select(e => new { key = e.Key, value = Enum.GetName(typeof(HealthStatus), e.Value.Status) })
                        });
                    context.Response.ContentType = MediaTypeNames.Application.Json;
                    await context.Response.WriteAsync(result);
                }
            });
            #endregion
            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();

        }
    }



}
