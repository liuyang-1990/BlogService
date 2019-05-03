namespace Blog.Model.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

    }
}