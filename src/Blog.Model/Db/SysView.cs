using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 浏览量表
    /// </summary>
    [SugarTable("sys_view")]
    public class SysView : BaseEntity
    {
        /// <summary>
        /// 访问ip
        /// </summary>
        public  string IP { get; set; }

    }
}