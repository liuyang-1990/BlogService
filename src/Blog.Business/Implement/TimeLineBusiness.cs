using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using Blog.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(ITimeLineBusiness), LifeTime = Lifetime.Scoped)]
    public class TimeLineBusiness : BaseBusiness<TimeLine>, ITimeLineBusiness
    {
        private readonly ITimeLineRepository _timeLineRepository;

        public TimeLineBusiness(ITimeLineRepository timeLineRepository)
        {
            BaseRepository = timeLineRepository;
            _timeLineRepository = timeLineRepository;
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
