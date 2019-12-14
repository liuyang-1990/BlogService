using Blog.Infrastructure.DI;
using Blog.Model.Db;
using Microsoft.Extensions.DependencyInjection;

namespace Blog.Repository.Implement
{
    [Injector(typeof(ITimeLineRepository), Lifetime = ServiceLifetime.Scoped)]
    public class TimeLineRepository : BaseRepository<TimeLine>, ITimeLineRepository
    {
       
    }
}
