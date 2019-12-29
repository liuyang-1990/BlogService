using Blog.Model.Entities;

namespace Blog.Model.Request.User
{
    public class UserSearchRequest : GridParams, IPassivable
    {
        public UserSearchRequest()
        {
            IsActive = true;
        }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
    }
}