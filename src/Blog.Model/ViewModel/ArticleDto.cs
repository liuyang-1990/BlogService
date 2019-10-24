

using System.Collections.Generic;

namespace Blog.Model.ViewModel
{
    public class ArticleDto : V_Article_Info
    {

        /// <summary>
        /// 标签
        /// </summary>
        public List<string> Tags { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public List<string> Categories { get; set; }

    }
}