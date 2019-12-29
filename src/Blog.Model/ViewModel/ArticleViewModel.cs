using System;
using Blog.Model.Entities;

namespace Blog.Model.ViewModel
{
    public class ArticleViewModel : ArticleCell, IEntity<int>
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

    }
}