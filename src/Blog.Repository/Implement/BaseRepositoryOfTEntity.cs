using Blog.Model.Entities;

namespace Blog.Repository.Implement
{
    public class BaseRepository<TEntity> : BaseRepository<TEntity, string> where TEntity : class, IEntity<string>, new()
    {

    }
}