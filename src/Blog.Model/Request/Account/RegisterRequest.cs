namespace Blog.Model.Request.Account
{
    public class RegisterRequest:LoginRequest
    {
        public string Email { get; set; }
    }
}