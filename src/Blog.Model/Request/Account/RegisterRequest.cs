using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.Account
{
    public class RegisterRequest : LoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Captcha { get; set; }
    }
}