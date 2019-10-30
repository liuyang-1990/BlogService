using System.Collections.Generic;

namespace Blog.Model.Request.User
{
    public class UpdateStatusRequest
    {
        public List<string> Ids { get; set; }

        public int Status { get; set; }

    }
}