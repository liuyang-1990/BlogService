using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.TimeLine
{
    public class CommonTimeLineRequest
    {
        [Required]
        public string Title { get; set; }
        [Required]
        public string Content { get; set; }
        public int Status { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }
    }
}