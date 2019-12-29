using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.TimeLine
{
    public class UpdateTimeLineRequest : CommonTimeLineRequest, IEntity<int>
    {
        [Required]
        public int Id { get; set; }
    }
}