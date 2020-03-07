using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.Category
{
    public class UpdateCategoryRequest : CommonCategoryRequest, IEntity<string>
    {
        [Required]
        public string Id { get; set; }

    }
}