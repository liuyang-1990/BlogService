namespace Blog.Infrastructure
{
    public interface IBlogUrlHelper
    {
        string GetQueryString(string url, string para);
    }
}
