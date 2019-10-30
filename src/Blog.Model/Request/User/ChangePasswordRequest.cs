using Blog.Model.Request.Account;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.User
{
    public class ChangePasswordRequest : LoginRequest
    {

        [Required]
        public string OldPassword { get; set; }
    }
}