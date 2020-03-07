using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.Article
{
    public class UpdateArticleRequest : AddArticleRequest, IEntity<string>
    {
        [Required]
        public string Id { get; set; }
    }
}