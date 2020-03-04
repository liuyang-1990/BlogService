using Blog.Model.Entities;
using Newtonsoft.Json;
using SqlSugar;

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
    }

}