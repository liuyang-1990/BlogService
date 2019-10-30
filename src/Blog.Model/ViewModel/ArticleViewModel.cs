using System;
using Blog.Model.Request.Article;

namespace Blog.Model.ViewModel
{
    public class ArticleViewModel : ArticleCell
    {
        public string Id { get; set; }
       
        public DateTime CreateTime { get; set; }

    }
}