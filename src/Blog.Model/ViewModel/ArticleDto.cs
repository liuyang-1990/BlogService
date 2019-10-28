

using System.Collections.Generic;

namespace Blog.Model.ViewModel
{
    public class ArticleDto : ArticleViewModel
    {

        /// <summary>
        /// 标签
        /// </summary>
        public List<string> TagIds { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public List<string> CategoryIds { get; set; }

    }
}