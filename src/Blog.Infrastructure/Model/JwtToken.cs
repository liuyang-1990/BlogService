namespace Blog.Infrastructure
{
    public class JwtToken
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Uid { get; set; }
        /// <summary>
        /// 角色
        /// </summary>
        public string Role { get; set; }

        public string UserName { get; set; }

        public string Avatar { get; set; }
    }
}