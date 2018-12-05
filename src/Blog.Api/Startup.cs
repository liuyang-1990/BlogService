using System;
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

namespace Blog.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            #region Swagger

            services.AddSwaggerGen(option =>
             {
                 option.CustomSchemaIds(x => x.FullName);
                 option.DescribeAllEnumsAsStrings();
                 option.SwaggerDoc("v1", new Info() { Title = "BlogService", Version = "V1" });
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseMvc();
            #region Swagger
            app.UseSwagger();
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "blog");
                option.RoutePrefix = "swagger";
            }); 
            #endregion
        }
    }


   
}
