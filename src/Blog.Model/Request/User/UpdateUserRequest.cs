using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.User
{
    public class UpdateUserRequest : AddUserRequest, IEntity<string>
    {
        [Required]
        public string Id { get; set; }

    }
}