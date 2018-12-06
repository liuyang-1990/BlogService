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
        public string CategoryName { get; set; }
    }
}