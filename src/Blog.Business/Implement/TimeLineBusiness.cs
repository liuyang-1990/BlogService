using Blog.Model.Attribute;
using Blog.Model.Db;
using Blog.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    public class TimeLineBusiness : BaseBusiness<Tbl_Time_Line>, ITimeLineBusiness
    {
        private readonly ITimeLineRepository _timeLineRepository;

        public TimeLineBusiness(ITimeLineRepository timeLineRepository)
        {
            BaseRepository = timeLineRepository;
            _timeLineRepository = timeLineRepository;
        }

        [Caching]
        public Task<List<Tbl_Time_Line>> GetList()
        {
            return _timeLineRepository.GetList();
        }

    }
}
