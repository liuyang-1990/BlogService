using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.Tag
{
    public class UpdateTagRequest : CommonTagRequest, IEntity<string>
    {
        [Required]
        public string Id { get; set; }
    }
}