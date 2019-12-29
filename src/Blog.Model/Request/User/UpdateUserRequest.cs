using System.ComponentModel.DataAnnotations;
using Blog.Model.Entities;

namespace Blog.Model.Request.User
{
    public class UpdateUserRequest : AddUserRequest, IEntity<int>
    {
        [Required]
        public int Id { get; set; }

    }
}