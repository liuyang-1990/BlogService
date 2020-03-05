using System;
using Blog.Model.Entities;

namespace Blog.Model.Response
{
    public class ArticleViewModel : ArticleCell, IEntity
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }

    }
}