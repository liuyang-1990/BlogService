using System;

namespace Blog.Model
{

    public class BaseEntity
    {
        /// <summary>
        /// 主键
        /// </summary>
        
        public int Id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;
        ///<summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

    }
}