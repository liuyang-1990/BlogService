using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Db;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ITimeLineRepository), LifeTime = Lifetime.Scoped)]
    public class TimeLineRepository : BaseRepository<TimeLine>, ITimeLineRepository
    {
       
    }
}
