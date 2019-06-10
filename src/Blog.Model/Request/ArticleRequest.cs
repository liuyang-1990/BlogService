namespace Blog.Model.Request
{
    public class ArticleRequest
    {
        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int? Status { get; set; }
    }
}