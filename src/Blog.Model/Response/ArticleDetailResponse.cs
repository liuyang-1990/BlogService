using Blog.Model.ViewModel;
using System.Collections.Generic;

namespace Blog.Model.Response
{
    public class ArticleDetailResponse
    {

        public ArticleViewModel ArticleInfo { get; set; }

        public List<Property> Tags { get; set; }

        public List<Property> Categories { get; set; }

    }

    public class Property
    {
        public string Id { get; set; }

        public string Value { get; set; }
    }
}