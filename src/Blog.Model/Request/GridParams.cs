namespace Blog.Model.Request
{
    public class GridParams
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string SortField { get; set; }
        public string SortOrder { get; set; }
    }
}