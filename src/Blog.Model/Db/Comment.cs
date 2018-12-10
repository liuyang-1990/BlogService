﻿using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 评论/回复表
    /// </summary>
    [SugarTable("tbl_comment")]
    public class Comment:BaseEntity
    {
        [SugarColumn(ColumnName = "parent_id")]
        public int? ParentId { get; set; }

        /// <summary>
        /// 留言内容
        /// </summary>
        public string Content  { get; set; }

        /// <summary>
        /// 留言人
        /// </summary>
        [SugarColumn(ColumnName = "create_by")]
        public int CreateBy { get; set; }


    }
}