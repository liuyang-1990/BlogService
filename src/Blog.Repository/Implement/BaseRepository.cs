using Blog.Model;
using Blog.Model.Common;
using Blog.Model.Request;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    /// <summary>
    /// Base class for custom repositories of the application.
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    public class BaseRepository<TEntity> where TEntity : BaseEntity, new()
    {
        protected readonly ISqlSugarClient Db;
        public BaseRepository(ISqlSugarClient sqlSugarClient)
        {
            Db = sqlSugarClient;
        }

        #region Query

        /// <summary>
        /// Query for existence with given predicate.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Queryable<TEntity>().AnyAsync(predicate);
        }

        /// <summary>
        /// Get All Entities
        /// </summary>
        /// <returns></returns>
        public async Task<List<TEntity>> QueryAll()
        {
            return await Db.Queryable<TEntity>().ToListAsync();
        }

        /// <summary>
        ///  Gets entities with given predicate,page & sort params.
        /// </summary>
        /// <param name="param">page & sort</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        public async Task<JsonResultModel<TEntity>> Query(GridParams param, Expression<Func<TEntity, bool>> predicate)
        {
            RefAsync<int> totalCount = 0;
            var queryable = Db.Queryable<TEntity>().WhereIF(predicate != null, predicate)
                .OrderByIF(!string.IsNullOrEmpty(param.SortField) && !string.IsNullOrEmpty(param.SortOrder),
                    param.SortField + " " + param.SortOrder);
            return new JsonResultModel<TEntity>()
            {
                Rows = await queryable.ToPageListAsync(param.PageNum, param.PageSize, totalCount),
                TotalRows = totalCount
            };
        }

        /// <summary>
        ///  Gets entities with given predicate,page & sort params.
        /// </summary>
        /// <param name="param">page & sort</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="groupBy">groupBy</param>
        /// <param name="select">columns to be selected</param>
        /// <param name="totalCount">total count</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(GridParams param,
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> groupBy,
            Expression<Func<TEntity, TResult>> select,
            RefAsync<int> totalCount)
        {
            return await Db.Queryable<TEntity>().Where(predicate).GroupBy(groupBy).Select(select).ToPageListAsync(param.PageNum, param.PageSize, totalCount);
        }

        /// <summary>
        /// Gets exactly one entity with given predicate.
        /// Throws exception if no entity or more than one entity.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>entity</returns>
        public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Queryable<TEntity>().SingleAsync(predicate);
        }

        /// <summary>
        /// Gets exactly one entity with primary key
        /// </summary>
        /// <param name="id">primary key</param>
        /// <returns></returns>
        public async Task<TEntity> SingleAsync(int id)
        {
            return await SingleAsync(x => x.Id == id);
        }

        /// <summary>
        /// get entities with given primary keys
        /// </summary>
        /// <param name="ids">primary keys</param>
        /// <returns></returns>
        public async Task<List<TEntity>> Query(List<int> ids)
        {
            return await Db.Queryable<TEntity>().In(ids).ToListAsync();
        }

        /// <summary>
        /// get some columns of entities with given primary keys
        /// </summary>
        /// <typeparam name="TResult">the return entity</typeparam>
        /// <param name="ids">primary keys</param>
        /// <param name="select">columns to be selected</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(List<int> ids, Expression<Func<TEntity, TResult>> select)
        {
            return await Db.Queryable<TEntity>().In(ids).Select(select).ToListAsync();
        }

        /// <summary>
        /// get some columns with given predicate
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <param name="select">columns to be selected</param>
        /// <returns></returns>
        public async Task<List<TResult>> Query<TResult>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, TResult>> select)
        {
            return await Db.Queryable<TEntity>().Where(predicate).Select(select).ToListAsync();
        }

        #endregion

        #region JoinQuery
        /// <summary>
        /// Multi-table join query
        /// </summary>
        /// <typeparam name="TEntity1">Entity1</typeparam>
        /// <typeparam name="TEntity2">Entity2</typeparam>
        /// <typeparam name="TResult">return result</typeparam>
        /// <param name="join">a condition to join</param>
        /// <param name="select">columns of the TResult</param>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns>Entity</returns>
        public async Task<TResult> JoinQuery<TEntity1, TEntity2, TResult>(
            Expression<Func<TEntity1, TEntity2, object[]>> join,
            Expression<Func<TEntity1, TEntity2, TResult>> select,
            Expression<Func<TEntity1, TEntity2, bool>> predicate)
        {
            return await Db.Queryable(join).Where(predicate).Select(select).FirstAsync();
        }

        #endregion

        #region Insert
        /// <summary>
        /// insert an entity 
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns>primary key of the entity</returns>
        public async Task<int> InsertAsync(TEntity entity)
        {
            return await Db.Insertable(entity).ExecuteReturnIdentityAsync();
        }

        /// <summary>
        /// insert entities(Fast performance)
        /// </summary>
        /// <param name="listEntity">Entities</param>
        /// <returns></returns>
        public async Task<int> InsertAsync(List<TEntity> listEntity)
        {
            return await Db.Insertable(listEntity).ExecuteCommandAsync();
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates an existing entity by primary key.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity)
        {
            return await Db.Updateable(entity).IgnoreColumns(true).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Updates an existing entity by Non primary key columns.
        /// </summary>
        /// <param name="entity">Entity</param>
        /// <param name="whereColumns">Non primary key columns of the entity</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(TEntity entity, Expression<Func<TEntity, object>> whereColumns)
        {
            return await Db.Updateable(entity).IgnoreColumns(true).WhereColumns(whereColumns).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Updates some columns of an existing entity by primary key.
        /// </summary>
        /// <param name="id">primary key</param>
        /// <param name="updateColumns">columns of TEntity to be updated</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(int id, Expression<Func<TEntity, object>> updateColumns)
        {
            return await Db.Updateable<TEntity>().UpdateColumns(updateColumns).Where(x => x.Id == id).ExecuteCommandHasChangeAsync();
        }

        /// <summary>
        /// Updates a column of an existing entity by primary keys.
        /// </summary>
        /// <param name="ids">Primary keys</param>
        /// <param name="updateColumns">a column of the entity to be updated</param>
        /// <returns></returns>
        public async Task<bool> UpdateAsync(List<string> ids, Expression<Func<TEntity, bool>> updateColumns)
        {
            return await Db.Updateable<TEntity>().SetColumns(updateColumns).Where(it => ids.Contains(SqlFunc.ToString(it.Id))).ExecuteCommandHasChangeAsync();
        }

        #endregion

        #region Delete
        /// <summary>
        /// Deletes an entity by function.
        /// </summary>
        /// <param name="predicate">A condition to filter entities</param>
        /// <returns></returns>
        public async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Db.Deleteable<TEntity>().Where(predicate).ExecuteCommandHasChangeAsync();
        }
        #endregion

        #region Tran
        /// <summary>
        /// use tran
        /// </summary>
        /// <param name="action">action that need to used in tran</param>
        /// <returns></returns>
        public async Task<DbResult<bool>> UseTranAsync(Action action)
        {
            return await Db.Ado.UseTranAsync(action);
        }
        public async Task<DbResult<T1>> UseTranAsync<T1>(Func<T1> action)
        {
            return await Db.Ado.UseTranAsync(action);
        }
        #endregion

        //protected Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        //{
        //    var lambdaParam = Expression.Parameter(typeof(TEntity));

        //    var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

        //    var idValue = Convert.ChangeType(id, typeof(TPrimaryKey));

        //    Expression<Func<object>> closure = () => idValue;
        //    var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

        //    var lambdaBody = Expression.Equal(leftExpression, rightExpression);

        //    return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        //}
    }
}