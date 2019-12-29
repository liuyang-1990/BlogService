using Blog.Infrastructure;
using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Blog.Repository;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(ITimeLineBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class TimeLineBusiness : BaseBusiness<TimeLine>, ITimeLineBusiness
    {
        public TimeLineBusiness(ITimeLineRepository timeLineRepository)
        {
            BaseRepository = timeLineRepository;
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [Caching("Tbl_Time_Line:GetList")]
        public Task<List<TimeLine>> GetList()
        {
            return base.QueryAll();
        }

    }
}
