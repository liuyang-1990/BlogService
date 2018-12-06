using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 类别表
    /// </summary>
    [SugarTable("tbl_category_info")]
    public class CategoryInfo : BaseEntity
    {
        /// <summary>
        /// 分类名称
        /// </summary>
        [SugarColumn(ColumnName = "category_name")]
        public string CategoryName { get; set; }
    }
}