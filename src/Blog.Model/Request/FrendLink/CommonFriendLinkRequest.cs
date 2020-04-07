using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.FrendLink
{
    public class CommonFriendLinkRequest
    {
        [Required]
        public string LinkName { get; set; }

        public string Description { get; set; }
    }
}