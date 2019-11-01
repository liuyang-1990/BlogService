using System;

namespace Blog.Model.ViewModel
{
    public class ArticleViewModel : ArticleCell, IEntity
    {
        public string Id { get; set; }

        public DateTime CreateTime { get; set; }

    }
}