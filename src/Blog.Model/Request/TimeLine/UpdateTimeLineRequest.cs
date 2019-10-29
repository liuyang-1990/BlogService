using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.TimeLine
{
    public class UpdateTimeLineRequest : CommonTimeLineRequest
    {
        [Required]
        public string Id { get; set; }
    }
}