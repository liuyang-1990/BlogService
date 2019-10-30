using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Article
{
    public class UpdateArticleRequest : AddArticleRequest
    {
        [Required]
        public string Id { get; set; }
    }
}