namespace Blog.Model.Request
{
    public class UserRequest : LoginRequest
    {

        public string Id { get; set; }

        public int Role { get; set; }

        public int Status { get; set; }

    }


    public class ChangePasswordRequest : LoginRequest
    {

        public string OldPassword { get; set; }

    }
}