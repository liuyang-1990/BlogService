using Blog.Model.Entities;

namespace Blog.Repository.Implement
{
    public class BaseRepository<TEntity> : BaseRepository<TEntity, int> where TEntity : class, IEntity<int>, new()
    {

    }
}