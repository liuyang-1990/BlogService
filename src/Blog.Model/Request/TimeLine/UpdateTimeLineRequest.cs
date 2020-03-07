using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.TimeLine
{
    public class UpdateTimeLineRequest : CommonTimeLineRequest, IEntity<string>
    {
        [Required]
        public string Id { get; set; }
    }
}