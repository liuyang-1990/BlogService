using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITimeLineBusiness : IBaseBusiness<TimeLine>
    {
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        Task<List<TimeLine>> GetList();

        
    }
}
