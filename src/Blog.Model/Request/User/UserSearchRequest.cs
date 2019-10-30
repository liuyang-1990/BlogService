namespace Blog.Model.Request.User
{
    public class UserSearchRequest : GridParams
    {
        public string UserName { get; set; }

        public int Status { get; set; } = 1;
    }
}