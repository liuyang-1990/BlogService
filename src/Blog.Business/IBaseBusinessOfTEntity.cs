using Blog.Model.Entities;

namespace Blog.Business
{
    public interface IBaseBusiness<TEntity> : IBaseBusiness<TEntity, int> where TEntity : class, IEntity<int>, new()
    {

    }
}