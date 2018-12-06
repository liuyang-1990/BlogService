﻿using SqlSugar;

namespace Blog.Model.Db
{
    /// <summary>
    /// 标签表
    /// </summary>
    [SugarTable("tbl_tag_info")]
    public class TagInfo:BaseEntity
    {
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName { get; set; }

        
    }
}