using System;

namespace Blog.Model.Entities
{
    public interface IHasModificationTime
    {
        /// <summary>
        /// The last modified time for this entity.
        /// </summary>
        DateTime? ModifyTime { get; set; }
    }
}