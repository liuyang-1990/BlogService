using AspectCore.Injector;
using Blog.Model;
using Blog.Model.Response;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    [Injector(typeof(IGeographicBusiness), LifeTime = Lifetime.Scoped)]
    public class GeographicBusiness : IGeographicBusiness
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public GeographicBusiness(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [Caching(AbsoluteExpiration = 1440)]
        public async Task<List<Province>> GetProvince()
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, "https://ant-design-pro.netlify.com/api/geographic/province");
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<Province>>();
            }
            return null;
        }

        [Caching(AbsoluteExpiration = 1440)]
        public async Task<List<City>> GetCity(string key)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, $"https://ant-design-pro.netlify.com/api/geographic/city/{key}");
            var client = _httpClientFactory.CreateClient();
            var response = await client.SendAsync(requestMessage, HttpCompletionOption.ResponseHeadersRead);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<List<City>>();
            }
            return null;
        }
    }
}
