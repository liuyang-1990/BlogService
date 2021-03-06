﻿using SqlSugar;

namespace Blog.Model.Db
{
    [SugarTable("tbl_article_comment")]
    public class ArticleComment : BaseEntity
    {
        /// <summary>
        /// 对应文章ID
        /// </summary>
        [SugarColumn(ColumnName = "article_id", IsNullable = false)]
        public int ArticleId { get; set; }

        [SugarColumn(ColumnName = "comment_id", IsNullable = false)]
        public int CommentId { get; set; }


    }
}