using System.Collections.Generic;
using Blog.Model.Entities;

namespace Blog.Model.Response
{
    public class ArticleDetailResponse
    {

        public ArticleViewModel ArticleInfo { get; set; }

        public List<Property> Tags { get; set; }

        public List<Property> Categories { get; set; }

    }

    public class Property : Entity
    {
        public string Value { get; set; }
    }
}