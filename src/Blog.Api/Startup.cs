using System;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
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
            services.AddSwaggerGen(option =>
            {
                option.CustomSchemaIds(x => x.FullName);
                option.DescribeAllEnumsAsStrings();
                option.SwaggerDoc("v1", new Info() { Title = "BlogService", Version = "V1" });
            });
            services.Configure<DBSetting>(Configuration.GetSection("ConnectionStrings"));
            var builder = new ContainerBuilder();
            builder.Populate(services);
            //新模块组件注册    
            builder.RegisterModule<AutofacModuleRegister>();
            //创建容器
            var container = builder.Build();
            //第三方IOC接管 core内置DI容器 
            return new AutofacServiceProvider(container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
            app.UseSwagger();
            loggerFactory.AddNLog();
            env.ConfigureNLog("nlog.config");
            app.UseSwaggerUI(option =>
            {
                option.SwaggerEndpoint("/swagger/v1/swagger.json", "blog");
                option.RoutePrefix = "swagger";
            });
        }
    }


    public class AutofacModuleRegister : Autofac.Module
    {
        //重写Autofac管道Load方法，在这里注册注入
        protected override void Load(ContainerBuilder builder)
        {
            //注册Service中的对象,Service中的类要以Business结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Business")).Where(a => a.Name.EndsWith("Business")).AsImplementedInterfaces();
            //注册Repository中的对象,Repository中的类要以Repository结尾，否则注册失败
            builder.RegisterAssemblyTypes(GetAssemblyByName("Blog.Repository")).Where(a => a.Name.EndsWith("Repository")).AsImplementedInterfaces();
        }
        /// <summary>
        /// 根据程序集名称获取程序集
        /// </summary>
        /// <param name="assemblyName">程序集名称</param>
        /// <returns></returns>
        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }
    }
}
