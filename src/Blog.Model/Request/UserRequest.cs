namespace Blog.Model.Request
{
    public class UserRequest: LoginRequest
    {

        public int Id { get; set; }

        public int Role { get; set; }

        public int Status { get; set; }

    }
}