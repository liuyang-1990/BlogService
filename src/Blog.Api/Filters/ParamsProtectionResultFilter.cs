﻿using Blog.Infrastructure.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Blog.Api.Filters
{
    public class ParamsProtectionResultFilter : IAsyncResultFilter
    {
        private static readonly string MatchJsonIdExpression = "\"[a-zA-Z0-9]*id[s]?\"";
        private static readonly string MatchJsonIdValueExpression = "[a-zA-Z0-9_\\-]+";
        private static readonly Regex MatchJsonIdKeyValue = new Regex($"{MatchJsonIdExpression}:{MatchJsonIdValueExpression}", RegexOptions.IgnoreCase);

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            var originalBodyStream = context.HttpContext.Response.Body;
            using (var ms = new MemoryStream())
            {
                context.HttpContext.Response.Body = ms;
                await next();
                var response = context.HttpContext.Response;
                response.Body.Seek(0, SeekOrigin.Begin);
                var text = await new StreamReader(response.Body).ReadToEndAsync();
                // 筛选以Id结尾的字段，并将ID加密
                var matchedIdCollection = MatchJsonIdKeyValue.Matches(text);
                foreach (Match match in matchedIdCollection)
                {
                    var unprotectId = Regex.Match(match.Value, $"{MatchJsonIdValueExpression}$").Value;
                    var protectId = Regex.Replace(match.Value,
                        $"{MatchJsonIdValueExpression}$",
                        $"\"{unprotectId.ToEncrypted()}\"");

                    text = text.Replace(match.Value, protectId);
                }
                var buffer = Encoding.UTF8.GetBytes(text);
                ms.Seek(0, SeekOrigin.Begin);
                await ms.WriteAsync(buffer, 0, buffer.Length);
                await ms.FlushAsync();
                response.Body.Seek(0, SeekOrigin.Begin);
                context.HttpContext.Response.ContentLength = buffer.Length;
                await ms.CopyToAsync(originalBodyStream);
            }
        }
    }
}