using Blog.Model.Entities;
using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.FrendLink
{
    public class UpdateFriendLinkRequest : CommonFriendLinkRequest, IEntity<string>
    {
        [Required]
        public string Id { get; set; }
    }
}