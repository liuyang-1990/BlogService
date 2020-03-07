using System.Collections.Generic;
using Blog.Model.Entities;

namespace Blog.Model.Request.User
{
    public class UpdateStatusRequest : IPassivable
    {
        public List<string> Ids { get; set; }
        public bool IsActive { get; set; }
    }
}