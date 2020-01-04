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
        [SugarColumn(ColumnName = "category_name", Length = 20, IsNullable = false)]
        public string CategoryName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string Description { get; set; }
    }
}