using Blog.Model.Entities;

namespace Blog.Repository
{
    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, int> where TEntity : class, IEntity<int>, new()
    {

    }
}