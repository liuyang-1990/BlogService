using Newtonsoft.Json;
using SqlSugar;
using System;
using Blog.Model.Entities;

namespace Blog.Model.Db
{
    [SugarTable("sys_user_info")]
    public class UserInfo : BaseEntity, IPassivable
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = false)]
        public string UserName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonIgnore]
        [SugarColumn(Length = 32, ColumnDataType = "char", IsNullable = false)]
        public string Password { get; set; }

        /// <summary>
        /// 用户角色  1系统管理员  0 普通用户
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// 状态 1 启用  0 禁用
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public string Avatar { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [SugarColumn(Length = 11, ColumnDataType = "char", IsNullable = true)]
        public string Phone { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [SugarColumn(Length = 100, IsNullable = true)]
        public string Email { get; set; }

        /// <summary>
        /// 国家
        /// </summary>
        [SugarColumn(Length = 10, IsNullable = true)]
        public string Country { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string Province { get; set; }

        [SugarColumn(Length = 20, IsNullable = true)]
        public string City { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string Profile { get; set; }

        [SugarColumn(Length = 500, IsNullable = true)]
        public string Address { get; set; }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        [SugarColumn(ColumnName = "last_login_time", IsNullable = true)]
        public DateTime? LastLoginTime { get; set; }
    }

}