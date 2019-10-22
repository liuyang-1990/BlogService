using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ITimeLineRepository), LifeTime = Lifetime.Scoped)]
    public class TimeLineRepository : BaseRepository<Tbl_Time_Line>, ITimeLineRepository
    {
        public Task<List<Tbl_Time_Line>> GetList()
        {
            return base.QueryAll();
        }
    }
}
