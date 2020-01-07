using Blog.Model.Entities;

namespace Blog.Business.Implement
{
    public class BaseBusiness<TEntity> : BaseBusiness<TEntity, int> where TEntity : class, IEntity<int>, IHasModificationTime, ISoftDelete, new()
    {
    }
}