using Blog.Model;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using NLog;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{

    public class BaseBusiness<T> where T : BaseEntity, new()
    {
        protected IBaseRepository<T> BaseRepository;

        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        public virtual async Task<JsonResultModel<T>> GetPageList(int pageIndex, int pageSize, Expression<Func<T, bool>> expression)
        {
            return await BaseRepository.GetPageList(pageIndex, pageSize, expression);
        }

        public virtual async Task<T> GetDetail(int id)
        {
            return await BaseRepository.GetDetail(id);
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<BaseResponse> Insert(T entity)
        {
            var response = new BaseResponse();
            try
            {
                var isSuccess = await BaseRepository.Insert(entity);
                if (isSuccess)
                {
                    response.Code = (int)ResponseStatus.Ok;
                    response.Msg = MessageConst.Created;
                }
                else
                {
                    response.Code = (int)ResponseStatus.Fail;
                    response.Msg = MessageConst.Fail;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }

            return response;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public virtual async Task<BaseResponse> Update(T entity)
        {
            var response = new BaseResponse();
            try
            {
                var isSuccess = await BaseRepository.Update(entity);
                if (isSuccess)
                {
                    response.Code = (int)ResponseStatus.Ok;
                    response.Msg = MessageConst.Updated;
                }
                else
                {
                    response.Code = (int)ResponseStatus.Fail;
                    response.Msg = MessageConst.Fail;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<BaseResponse> Delete(int id)
        {
            var response = new BaseResponse();
            try
            {
                var isSuccess = await BaseRepository.Delete(id);
                if (isSuccess)
                {
                    response.Code = (int)ResponseStatus.Ok;
                    response.Msg = MessageConst.Updated;
                }
                else
                {
                    response.Code = (int)ResponseStatus.Fail;
                    response.Msg = MessageConst.Fail;
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                response.Code = (int)ResponseStatus.Fail;
                response.Msg = ex.Message;
            }
            return response;
        }
    }
}