using System;
using Blog.Model.Entities;

namespace Blog.Model.ViewModel
{
    public class ArticleViewModel : ArticleCell, IEntity<string>
    {
        public string Id { get; set; }

        public DateTime CreateTime { get; set; }

    }
}