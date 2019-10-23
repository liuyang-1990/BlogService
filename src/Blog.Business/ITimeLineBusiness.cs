using Blog.Model.Db;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITimeLineBusiness : IBaseBusiness<Tbl_Time_Line>
    {
        Task<List<Tbl_Time_Line>> GetList();

        
    }
}
