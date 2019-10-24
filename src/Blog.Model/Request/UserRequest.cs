using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request
{
    public class UserRequest
    {
        public string UserName { get; set; }
        public int Status { get; set; } = 1;

    }


    public class ChangePasswordRequest : LoginRequest
    {
        [Required]
        public string OldPassword { get; set; }

    }
}