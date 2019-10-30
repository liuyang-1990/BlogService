using System.ComponentModel.DataAnnotations;

namespace Blog.Model.Request.User
{
    public class AddUserRequest
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 用户角色  1系统管理员  0 普通用户
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// 状态 1 启用  0 禁用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        public string Country { get; set; }

        public string Province { get; set; }

        public string City { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        public string Profile { get; set; }

        public string Address { get; set; }
    }
}