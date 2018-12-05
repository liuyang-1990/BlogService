using Autofac;
using Autofac.Extensions.DependencyInjection;
using Blog.Api.AutoFac;
using Blog.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using NLog.Web;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;

namespace Blog.Api
{
    /// <summary>
    /// 启动配置类
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration属性
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region 跨域

            services.AddCors(options =>
            {
                options.AddPolicy("allowAll", policy =>
                    {
                        policy
                            .AllowAnyOrigin()//允许任何源
                            .AllowAnyMethod()//允许任何方式
                            .AllowAnyHeader()//允许任何头
                            .AllowCredentials();//允许cookie
                    });
            });

            #endregion


            #region Swagger

            services.AddSwaggerGen(option =>
             {
                 option.CustomSchemaIds(x => x.FullName);
                 option.DescribeAllEnumsAsStrings();
                 option.SwaggerDoc("v1", new Info()
                 {
                     Title = "BlogService",
                     Version = "V1",
                     Description = "框架说明文档",
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

            services.Configure<DBSetting>(Configuration.GetSection("ConnectionStrings"));

            #region Ioc
            var builder = new ContainerBuilder();
            builder.Populate(services);
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            var container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(container);
            #endregion
        }

        /// <summary>
        ///  This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="loggerFactory"></param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                #region Swagger
                app.UseSwagger();
                app.UseSwaggerUI(option =>
                {
                    option.SwaggerEndpoint("/swagger/v1/swagger.json", "blog");
                    option.RoutePrefix = "swagger";
                });
                #endregion
            }
            else
            {
                app.UseHsts();
            }
            app.UseMiddleware<ExceptionFilter>();//自定义异常处理
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseCors("allowAll");
            app.UseMvc();

        }
    }



}
