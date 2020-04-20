using System;

namespace Blog.Infrastructure.OAuth
{
    public class OAuth2Account
    {
        public int Id { get; set; }


        /// <summary>
        /// 保存对应的ID
        /// </summary>
        public string OpenID { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public DateTime ExpireTime { get; set; }

        /// <summary>
        /// 返回的第三方昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 返回的第三方账号对应的头像地址。
        /// </summary>
        public string Avatar { get; set; }

        public string Email { get; set; }

    }
}