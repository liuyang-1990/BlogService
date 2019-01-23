using Newtonsoft.Json;
using SqlSugar;
using System;

namespace Blog.Model.Db
{
    [SugarTable("sys_user_info")]
    public class UserInfo : UserInfoBase
    {
      

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

    }

    public class UserInfoBase : BaseEntity
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户角色  1系统管理员  0 普通用户
        /// </summary>
        public int Role { get; set; }

        /// <summary>
        /// 状态 1 启用  0 禁用
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 上次登录时间
        /// </summary>
        [SugarColumn(ColumnName = "last_login_time")]
        public DateTime? LastLoginTime { get; set; }

    }
}