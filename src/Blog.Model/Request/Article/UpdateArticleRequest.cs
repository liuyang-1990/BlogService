using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.Article
{
    public class UpdateArticleRequest : AddArticleRequest, IEntity<int>
    {
        [Required]
        public int Id { get; set; }
    }
}