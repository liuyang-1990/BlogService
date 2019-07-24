using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Repository
{
    public interface ITimeLineRepository : IBaseRepository<Tbl_Time_Line>
    {
        Task<List<Tbl_Time_Line>> GetList();
    }
}
