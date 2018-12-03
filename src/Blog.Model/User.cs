using System;

namespace Blog.Model
{
    public class User:BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 注册时间
        /// </summary>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 是否是管理员
        /// </summary>
        public bool IsSuperUser { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        public DateTime LastLoginTime { get; set; }


    }
}