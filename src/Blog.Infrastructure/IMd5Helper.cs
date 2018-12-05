namespace Blog.Infrastructure
{
    public interface IMd5Helper
    {
        string Encrypt32(string originStr);
    }
}