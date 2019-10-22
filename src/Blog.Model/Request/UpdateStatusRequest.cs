using System.Collections.Generic;

namespace Blog.Model.Request
{
    public class UpdateStatusRequest
    {
        public List<string> Ids { get; set; }

        public int Status { get; set; }

    }
}