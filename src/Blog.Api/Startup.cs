using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using AutoMapper;
using Blog.Api.Filters;
using Blog.Api.Interceptors;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using SqlSugar;
using StackExchange.Profiling.Storage;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

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
            this.Name = configuration["Name"] ?? "Blog Service";
            this.Version = configuration["Version"] ?? "v0";
            this.Description = Configuration["Description"] ?? string.Empty;
        }

        /// <summary>
        /// Configuration属性
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Hosting Environment属性
        /// </summary>
        public IHostingEnvironment Env { get; }
        public string Name { get; set; }

        public string Version { get; set; }

        public string Description { get; set; }
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

            //services.Configure<DbSetting>(Configuration.GetSection("ConnectionStrings"));

            services.AddMiniProfiler(options =>
            {
                options.RouteBasePath = "/profiler";
                if (options.Storage is MemoryCacheStorage memoryCacheStorage)
                {
                    memoryCacheStorage.CacheDuration = TimeSpan.FromMinutes(10);
                }
            });
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

            services.AddSwaggerGen(option =>
             {
                 option.CustomSchemaIds(x => x.FullName);
                 option.DescribeAllEnumsAsStrings();
                 option.SwaggerDoc(Version, new Info()
                 {
                     Title = Name,
                     Version = Version,
                     Description = Description
                 });

                 var basePath = Microsoft.DotNet.PlatformAbstractions.ApplicationEnvironment.ApplicationBasePath;
                 var xmlPath = Path.Combine(basePath, "Blog.Api.xml");
                 option.IncludeXmlComments(xmlPath, true);
                 //添加header验证信息
                 var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                 option.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                 option.AddSecurityDefinition("Bearer", new ApiKeyScheme
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

            services.AddHttpClient();
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.ConfigureDynamicProxy(config =>
            {
                config.Interceptors.AddTyped<MiniProfilerInterceptor>();
            });

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
        public void Configure(IApplicationBuilder app)
        {

            if (Env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.DocumentTitle = Name;
                option.SwaggerEndpoint($"/swagger/{Version}/swagger.json", $"{Name} {Version}");
                option.RoutePrefix = "swagger";
                option.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("Blog.Api.index.html");
            });
            #endregion

            //跨域
            app.UseCors("LimitRequests");

            //自定义认证
            //app.UseMiddleware<AuthenticationMiddleware>();
            //认证
            app.UseAuthentication();

            // 返回错误码
            app.UseStatusCodePages();
            //miniProfiler
            app.UseMiniProfiler();

            app.UseStaticFiles();
            app.UseCookiePolicy();
            app.UseMvc();

        }
    }



}
