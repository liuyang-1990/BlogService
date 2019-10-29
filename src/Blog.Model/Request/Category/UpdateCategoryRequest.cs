using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Category
{
    public class UpdateCategoryRequest : CommonCategoryRequest
    {
        [Required]
        public string Id { get; set; }

    }
}