namespace Blog.Model.Db
{
    public class UserOAuthMapping : BaseEntity
    {
        public int UserId { get; set; }

        public string OpenId { get; set; }

        public string Type { get; set; }
    }
}