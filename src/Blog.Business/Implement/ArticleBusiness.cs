using Blog.Model.Db;
using Blog.Model.ViewModel;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class ArticleBusiness : BaseBusiness<ArticleInfo>, IArticleBusiness
    {

        private readonly IArticleRepository _articleRepository;

        public ArticleBusiness(IArticleRepository articleRepository)
        {
            BaseRepository = articleRepository;
            _articleRepository = articleRepository;
        }


        public bool Insert(ArticleDto articleDto)
        {
            var article = new ArticleInfo()
            {
                Abstract = articleDto.Abstract,
                Title = articleDto.Title
            };

            var content = new ArticleContent()
            {
                Content = articleDto.Content
            };
            return _articleRepository.Insert(article, content);
        }

        public bool Update(ArticleDto articleDto)
        {
            var article = new ArticleInfo()
            {
                Abstract = articleDto.Abstract,
                //Categories = articleDto.Categories,
                //Tags = articleDto.Tags,
                Title = articleDto.Title,
                Id = articleDto.Id
            };

            var content = new ArticleContent()
            {
                Content = articleDto.Content,
                ArticleId = article.Id
            };

            return _articleRepository.Update(article, content);
        }


    }
}