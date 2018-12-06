namespace Blog.Model.Db
{
    /// <summary>
    /// 文章内容
    /// </summary>
    public class ArticleContent:BaseEntity
    {

        public int Article_Id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }

    }
}