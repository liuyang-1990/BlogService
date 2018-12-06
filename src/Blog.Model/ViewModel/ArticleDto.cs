using System;

namespace Blog.Model.ViewModel
{
    public class ArticleDto
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
        /// 标签
        /// </summary>
        public string Tags { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }


        public DateTime CreateTime { get; set; }
    }
}