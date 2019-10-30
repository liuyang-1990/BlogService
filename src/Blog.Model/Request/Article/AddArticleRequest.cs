using Blog.Model.ViewModel;
using System.Collections.Generic;

namespace Blog.Model.Request.Article
{
    public class AddArticleRequest : ArticleCell
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