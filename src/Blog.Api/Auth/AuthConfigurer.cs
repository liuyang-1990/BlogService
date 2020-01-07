using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Blog.Api
{
    public static class AuthConfigurer
    {
        public static void Configure(IServiceCollection services, IConfiguration configuration)
        {
            if (!bool.Parse(configuration["Authentication:JwtBearer:IsEnabled"]))
            {
                return;
            }
            // 认证，就是根据登录的时候，生成的令牌，检查其是否合法，这个主要是证明没有被篡改
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            }).AddJwtBearer("JwtBearer", o =>
            {
                o.Audience = configuration["Authentication:JwtBearer:Audience"];
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:JwtBearer:SecurityKey"])),

                    ValidateIssuer = true,
                    ValidIssuer = configuration["Authentication:JwtBearer:Issuer"],

                    ValidateAudience = true,
                    ValidAudience = configuration["Authentication:JwtBearer:Audience"],

                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                o.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = QueryStringTokenResolver
                };
            });
        }

        /// <summary>
        /// This method is needed to authorize SignalR javascript client.
        ///SignalR can not send authorization header.So, we are getting it from query string as an encrypted text.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private static Task QueryStringTokenResolver(MessageReceivedContext context)
        {
            if (!context.HttpContext.Request.Path.HasValue ||
                !context.HttpContext.Request.Path.Value.StartsWith("/chatHub"))
            {
                // We are just looking for signalr clients
                return Task.CompletedTask;
            }
            var qsAuthToken = context.HttpContext.Request.Query["enc_auth_token"].FirstOrDefault();
            if (qsAuthToken == null)
            {
                // Cookie value does not matches to querystring value
                return Task.CompletedTask;
            }
            // Set auth token from cookie
            context.Token = qsAuthToken;
            return Task.CompletedTask;
        }
    }
}