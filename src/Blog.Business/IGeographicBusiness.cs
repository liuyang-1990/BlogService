using Blog.Model.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IGeographicBusiness
    {
        Task<List<Province>> GetProvince();

        Task<List<City>> GetCity(string key);
    }
}
