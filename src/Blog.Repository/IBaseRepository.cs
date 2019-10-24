using Blog.Model;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        Task<bool> QueryIsExist(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<T>> QueryAll();

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="strWhere">查询条件</param>
        /// <returns></returns>
        Task<List<T>> QueryAll(string strWhere);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="whereExpression">查询条件</param>
        /// <returns></returns>
        Task<List<T>> QueryAll(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        Task<JsonResultModel<T>> QueryByPage(GridParams param, Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 根据where条件查询一条数据
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <returns></returns>
        Task<T> QueryByWhere(Expression<Func<T, bool>> whereExpression);

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<T> QueryById(string id);

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<T>> QueryByIds(List<string> ids);

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回自增量</returns>
        Task<int> Insert(T entity);

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="columns">指定只插入列</param>
        /// <returns>返回自增量</returns>
        Task<int> Insert(T entity, Expression<Func<T, object>> columns);

        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="columns">指定只插入列</param>
        /// <returns>返回自增量</returns>
        Task<int> Insert(T entity, params string[] columns);

        /// <summary>
        /// 批量插入实体(性能很快不用操心）
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns>受影响行数</returns>
        Task<int> Insert(List<T> listEntity);


        /// <summary>
        ///  更新实体数据(以主键为条件)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> Update(T entity);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreAllDefaultAndNullValue">是NULL的列和默认值的列不更新</param>
        /// <returns></returns>
        Task<bool> Update(T entity, bool ignoreAllDefaultAndNullValue);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新的列</param>
        /// <returns></returns>
        Task<bool> Update(T entity, Expression<Func<T, object>> updateColumns);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="ignoreColumns">不更新的列</param>
        /// <returns></returns>
        Task<bool> Update(T entity, params string[] ignoreColumns);

        /// <summary>
        /// 批量更新(主键要有值，主键是更新条件)
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns></returns>
        Task<bool> Update(List<T> listEntity);

        /// <summary>
        /// 根据主键批量更新部分列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">部分列</param>
        /// <returns></returns>
        Task<bool> UpdateByIds(List<string> ids, Expression<Func<T, T>> updateExpression);

        /// <summary>
        /// 根据主键批量更新某一列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">某一列</param>
        /// <returns></returns>
        Task<bool> UpdateByIds(List<string> ids, Expression<Func<T, bool>> updateExpression);
        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<bool> DeleteById(string id);

        /// <summary>
        /// 删除指定ID集合的数据(批量假删除)
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteByIds(List<string> ids);

    }
}