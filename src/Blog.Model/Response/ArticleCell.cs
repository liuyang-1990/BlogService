namespace Blog.Model.Response
{
    public class ArticleCell
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Abstract { get; set; }

        /// <summary>
        /// 访问量
        /// </summary>
        public int Views { get; set; }

        /// <summary>
        /// 评论量
        /// </summary>
        public int Comments { get; set; }

        /// <summary>
        /// 点赞量
        /// </summary>
        public int Likes { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 文章状态 1发布  0草稿
        /// </summary>
        public bool IsPublished { get; set; }

        public string ImageUrl { get; set; }
    }
}