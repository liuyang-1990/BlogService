using System;
using SqlSugar;

namespace Blog.Model.ViewModel
{
    public class V_Article_Info
    {
        public int Id { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        public DateTime CreateTime { get; set; }
        [SugarColumn(ColumnName = "is_deleted")]
        public int IsDeleted { get; set; }

    }
}