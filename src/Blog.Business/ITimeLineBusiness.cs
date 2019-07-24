using Blog.Model.Db;
using Blog.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ITimeLineBusiness : IBaseBusiness<Tbl_Time_Line>
    {
        Task<List<Tbl_Time_Line>> GetList();

        
    }
}
