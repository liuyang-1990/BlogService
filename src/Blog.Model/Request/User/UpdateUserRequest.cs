using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.User
{
    public class UpdateUserRequest : AddUserRequest
    {
        [Required]
        public string Id { get; set; }

    }
}