using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Article
{
    public class UpdateArticleRequest : AddArticleRequest
    {
        [Required]
        public int Id { get; set; }
    }
}