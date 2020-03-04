using Newtonsoft.Json;
using SqlSugar;
using System;
using Blog.Model.Entities;

namespace Blog.Model
{

    public class BaseEntity : Entity, ISoftDelete, IHasModificationTime
    {
        public BaseEntity()
        {
            CreateTime = DateTime.Now;
        }

        /// <summary>
        /// 是否删除 0 未删除 1 已删除
        /// </summary>
        [JsonIgnore]
        [SugarColumn(ColumnName = "is_deleted", DefaultValue = "0")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn(IsOnlyIgnoreUpdate = true)]
        public DateTime CreateTime { get; set; }
        ///<summary>
        /// 修改时间
        /// </summary>
        [JsonIgnore]
        [SugarColumn(IsOnlyIgnoreInsert = true, IsNullable = true)]
        public DateTime? ModifyTime { get; set; }
    }
}