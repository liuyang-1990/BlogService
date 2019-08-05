namespace Blog.Model.Response
{
    public class LoginResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public Profile Profile { get; set; }

    }

    public class Profile
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }
    }
}