using Blog.Model;

namespace Blog.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        // Task<IEnumerable<T>> GetEntities(Expression<Func<T, bool>> lambda);
        //Task<IEnumerable<T>> GetEntiyByPage<S>(int pageInex, int pageSize, out int total, Expression<Func<T, bool>> lambda,
        //    Expression<Func<T, S>> orderBy, bool isAsc);

        bool Insert(T entity);

        //  bool Update(T entity);

        bool Delete(dynamic id);

    }
}