using System;

namespace Blog.Model.Request
{
    public class TimeLineRequest
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public int Status { get; set; }

        public DateTime Start_Time { get; set; }

        public DateTime End_Time { get; set; }
    }
}
