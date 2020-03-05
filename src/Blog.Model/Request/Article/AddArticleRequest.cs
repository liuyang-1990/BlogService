using Blog.Model.Response;
using System.Collections.Generic;

namespace Blog.Model.Request.Article
{
    public class AddArticleRequest : ArticleCell
    {
        /// <summary>
        /// 标签
        /// </summary>
        public List<int> TagIds { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        public List<int> CategoryIds { get; set; }
    }
}