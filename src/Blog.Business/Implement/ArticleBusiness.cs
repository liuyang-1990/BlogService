using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class ArticleBusiness : IArticleBusiness
    {

        private readonly IArticleRepository _articleRepository;
        public ArticleBusiness(IArticleRepository articleRepository)
        {
            _articleRepository = articleRepository;
        }

        public string GetPageList(int pageIndex, int pageSize)
        {
            return _articleRepository.GetPageList(pageIndex, pageSize);
        }

        public string GetDetailInfo(int id)
        {
            return _articleRepository.GetDetailInfo(id);
        }

        public bool Insert(ArticleDto articleDto)
        {
            var article = new Article()
            {
                Abstract = articleDto.Abstract,
                Categories = articleDto.Categories,
                Tags = articleDto.Tags,
                Title = articleDto.Title
            };

            var content = new ArticleContent()
            {
                Content = articleDto.Content
            };
            return _articleRepository.Insert(article, content);
        }

        public bool Delete(int id)
        {
            return _articleRepository.Delete(id);
        }

        public bool Update(ArticleDto articleDto)
        {
            var article = new Article()
            {
                Abstract = articleDto.Abstract,
                Categories = articleDto.Categories,
                Tags = articleDto.Tags,
                Title = articleDto.Title,
                Id = articleDto.Id
            };

            var content = new ArticleContent()
            {
                Content = articleDto.Content,
                Article_Id = article.Id
            };

            return _articleRepository.Update(article, content);
        }


    }
}