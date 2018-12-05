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
        /// 是否是管理员
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// 是否可用
        /// </summary>
        public int Status { get; set; }

    }
}