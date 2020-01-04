using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 标签表
    /// </summary>
    [SugarTable("tbl_tag_info")]
    public class TagInfo : BaseEntity
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        [SugarColumn(ColumnName = "tag_name", Length = 20, IsNullable = false)]
        public string TagName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string Description { get; set; }
    }
}