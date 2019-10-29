using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Category
{
    public class CommonCategoryRequest
    {
        [Required]
        public string CategoryName { get; set; }

        public string Description { get; set; }
    }
}