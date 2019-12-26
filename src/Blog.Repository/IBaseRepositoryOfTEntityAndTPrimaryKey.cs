using Blog.Model.Entities;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository
{
    /// <summary>
    /// This interface is implemented by all repositories to ensure implementation of fixed methods.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IBaseRepository<TEntity, TPrimaryKey> where TEntity : class, IEntity<TPrimaryKey>, new()
    {

        /// <summary>
        /// 查询是否存在
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        Task<bool> QueryIsExist(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> QueryAll();

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <returns></returns>
        Task<JsonResultModel<TEntity>> QueryByPage(GridParams param, Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        ///  分页查询
        /// </summary>
        /// <param name="param">分页以及排序参数</param>
        /// <param name="whereExpression">条件</param>
        /// <param name="groupByExpression">groupBy</param>
        /// <param name="selectExpression">select</param>
        /// <param name="totalCount">返回总条数</param>
        /// <returns></returns>
        Task<List<TResult>> QueryByPage<TResult>(GridParams param,
            Expression<Func<TEntity, bool>> whereExpression,
            Expression<Func<TEntity, object>> groupByExpression,
            Expression<Func<TEntity, TResult>> selectExpression,
            RefAsync<int> totalCount);

        /// <summary>
        /// 根据where条件查询一条数据
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <returns></returns>
        Task<TEntity> QueryByWhere(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<TEntity> QueryById(TPrimaryKey id);


        /// <summary>
        ///  根据ID查询数据列表
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<List<TEntity>> QueryByIds(List<TPrimaryKey> ids);

        /// <summary>
        /// 根据ID查询数据列表
        /// </summary>
        /// <typeparam name="T1">返回的对象</typeparam>
        /// <param name="ids">主键</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        Task<List<T1>> QueryByIds<T1>(List<TPrimaryKey> ids, Expression<Func<TEntity, T1>> selectExpression) where T1 : Property;
        /// <summary>
        ///  根据where条件查询某几列
        /// </summary>
        /// <param name="whereExpression">where条件</param>
        /// <param name="selectExpression">查询某几列</param>
        /// <returns></returns>
        Task<List<TResult>> QueryByWhere<TResult>(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, TResult>> selectExpression);

        /// <summary>
        /// 多表联合查询
        /// </summary>
        /// <typeparam name="T1">一表</typeparam>
        /// <typeparam name="T2">二表</typeparam>
        /// <typeparam name="T3">返回实体</typeparam>
        /// <param name="joinExpression"></param>
        /// <param name="selectExpression"></param>
        /// <param name="whereLambda"></param>
        /// <returns></returns>
        Task<T3> JoinQuery<T1, T2, T3>(
            Expression<Func<T1, T2, object[]>> joinExpression,
            Expression<Func<T1, T2, T3>> selectExpression,
            Expression<Func<T1, T2, bool>> whereLambda) where T3 : IEntity<TPrimaryKey>;


        /// <summary>
        /// 新增实体数据
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>返回自增量</returns>
        Task<int> Insert(TEntity entity);

        /// <summary>
        /// 批量插入实体(性能很快不用操心）
        /// </summary>
        /// <param name="listEntity"></param>
        /// <returns>受影响行数</returns>
        Task<int> Insert(List<TEntity> listEntity);


        /// <summary>
        ///  更新实体数据(以主键为条件)
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="updateColumns">更新的列</param>
        /// <returns></returns>
        Task<bool> Update(TEntity entity, Expression<Func<TEntity, object>> updateColumns);

        /// <summary>
        /// 更新实体数据
        /// </summary>
        /// <param name="updateObj">要更新的实体</param>
        /// <param name="whereColumns">更新的条件</param>
        /// <returns></returns>
        Task<bool> UpdateByWhere(TEntity updateObj, Expression<Func<TEntity, object>> whereColumns);

        /// <summary>
        /// 根据主键批量更新某一列
        /// </summary>
        /// <param name="ids">主键</param>
        /// <param name="updateExpression">某一列</param>
        /// <returns></returns>
        Task<bool> UpdateByIds(List<TPrimaryKey> ids, Expression<Func<TEntity, bool>> updateExpression);
        /// <summary>
        /// 根据主键删除(假删除)
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns></returns>
        Task<bool> DeleteById(TPrimaryKey id);

        /// <summary>
        /// 真删除
        /// </summary>
        /// <param name="whereExpression"></param>
        /// <returns></returns>
        Task<bool> DeleteByWhere(Expression<Func<TEntity, bool>> whereExpression);

        /// <summary>
        /// 使用事务
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        Task<DbResult<bool>> UseTranAsync(Action action);
    }
}