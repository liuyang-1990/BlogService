using AspectCore.Configuration;
using AspectCore.Extensions.Hosting;
using Blog.Api.Interceptors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Web;

namespace Blog.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            //Use  AOP
            .UseServiceContext()
            .ConfigureDynamicProxy((context, config) =>
            {
                //only intercept services end with business
                config.Interceptors.AddTyped<BlogRedisCacheInterceptor>(Predicates.ForService("*Business"));
                config.Interceptors.AddTyped<MiniProfilerInterceptor>(Predicates.ForService("*Business"));
                //Global Intercept
                //config.Interceptors.AddTyped<MiniProfilerInterceptor>();
            })
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureLogging((context, logging) =>
            {
                logging.ClearProviders();
                logging.AddConfiguration(context.Configuration.GetSection("Logging"));
                logging.AddConsole();
            })
            .UseNLog();

    }
}
