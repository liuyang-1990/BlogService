using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Tag
{
    public class CommonTagRequest
    {
        [Required]
        public string TagName { get; set; }

        public string Description { get; set; }
    }
}