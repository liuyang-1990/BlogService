using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.Category
{
    public class UpdateCategoryRequest : CommonCategoryRequest, IEntity<int>
    {
        [Required]
        public int Id { get; set; }

    }
}