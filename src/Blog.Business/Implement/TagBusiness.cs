using Blog.Model;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class TagBusiness : BaseBusiness<Tag>, ITagBusiness
    {
        public TagBusiness(IBaseRepository<Tag> baseRepository) : base(baseRepository)
        {
        }
    }
}