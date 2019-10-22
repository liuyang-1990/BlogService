using Blog.Model.ViewModel;
using System.Collections.Generic;

namespace Blog.Model.Response
{
    public class ArticleDetailResponse
    {

        public V_Article_Info ArticleInfo { get; set; }

        public IEnumerable<Property> Tags { get; set; }

        public IEnumerable<Property> Categories { get; set; }

    }

    public class Property
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}