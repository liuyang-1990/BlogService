using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.Tag
{
    public class UpdateTagRequest : CommonTagRequest, IEntity<int>
    {
        [Required]
        public int Id { get; set; }
    }
}