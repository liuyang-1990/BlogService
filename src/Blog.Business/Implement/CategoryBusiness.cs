using Blog.Model.Db;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class CategoryBusiness : BaseBusiness<CategoryInfo>, ICategoryBusiness
    {
        public CategoryBusiness(ICategoryRepository respoitory)
        {
            base.BaseRepository = respoitory;
        }
    }
}