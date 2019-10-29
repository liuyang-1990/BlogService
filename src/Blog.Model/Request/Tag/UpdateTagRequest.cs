using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Tag
{
    public class UpdateTagRequest : CommonTagRequest
    {
        [Required]
        public string Id { get; set; }
    }
}