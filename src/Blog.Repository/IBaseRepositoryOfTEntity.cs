using Blog.Model.Entities;

namespace Blog.Repository
{
    public interface IBaseRepository<TEntity> : IBaseRepository<TEntity, string> where TEntity : class, IEntity<string>, new()
    {

    }
}