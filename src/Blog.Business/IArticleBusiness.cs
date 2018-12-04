using Blog.Model;

namespace Blog.Business
{
    public interface IArticleBusiness
    {
        string GetPageList(int pageIndex, int pageSize);

        string GetDetailInfo(int id);

        bool Insert(ArticleDto articleDto);

        bool Delete(int id);

        bool Update(ArticleDto articleDto);
    }
}