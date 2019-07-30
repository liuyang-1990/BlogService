using System;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Blog.Api
{
    public class BlogApiControllerAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "api/v{version:apiVersion}/[controller]";

        public int? Order { get; set; }

        public string Name { get; set; }
    }
}