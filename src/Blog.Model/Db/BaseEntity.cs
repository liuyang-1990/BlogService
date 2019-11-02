using Newtonsoft.Json;
using SqlSugar;
using System;

namespace Blog.Model
{

    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>        
        public string Id { get; set; }

        /// <summary>
        /// 是否删除 0 未删除 1 已删除
        /// </summary>
        [JsonIgnore]
        [SugarColumn(ColumnName = "is_deleted")]
        public byte IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; } = DateTime.Now;
        ///<summary>
        /// 修改时间
        /// </summary>
        [JsonIgnore]
        public DateTime? ModifyTime { get; set; }

    }
}