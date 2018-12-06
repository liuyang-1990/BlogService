using Blog.Model.Db;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class CategoryBusiness : BaseBusiness<Category>, ICategoryBusiness
    {
        public CategoryBusiness(ICategoryRepository respoitory)
        {
            base.BaseRepository = respoitory;
        }
    }
}