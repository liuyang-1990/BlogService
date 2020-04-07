using SqlSugar;

namespace Blog.Model.Db
{
    [SugarTable("tbl_friend_link")]
    public class FriendLink : BaseEntity
    {
        /// <summary>
        /// 友链名称
        /// </summary>
        [SugarColumn(ColumnName = "link_name", Length = 20, IsNullable = false)]
        public string LinkName { get; set; }

        [SugarColumn(Length = 50, IsNullable = true)]
        public string Description { get; set; }
    }
}
