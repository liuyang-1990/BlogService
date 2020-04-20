using Blog.Infrastructure.DI;
using Blog.Infrastructure.Model;
using Blog.Infrastructure.OAuth;
using Blog.Model.Db;
using Blog.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Blog.Infrastructure.Cryptography;

namespace Blog.Business.Implement
{
    [Injector(typeof(IAccountBusiness), Lifetime = ServiceLifetime.Scoped)]
    public class AccountBusiness : BaseBusiness<UserOAuthMapping>, IAccountBusiness
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IUserRepository _userRepository;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IMd5Helper _md5Helper;
        public AccountBusiness(IAccountRepository accountRepository,
            IUserRepository userRepository,
            IConfiguration configuration,
            IMd5Helper md5Helper,
            IHttpClientFactory httpClientFactory)
        {
            _accountRepository = accountRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _md5Helper = md5Helper;
            _httpClient = httpClientFactory.CreateClient();
            _httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
            BaseRepository = accountRepository;
        }

        internal string Code { get; set; }

        internal string State { get; set; }

        private string ClientId => _configuration["Authentication:Github:ClientId"];

        private string ClientSecret => _configuration["Authentication:Github:ClientSecret"];
        private async Task<string> GetAccessToken()
        {
            var content = new
            {
                client_id = ClientId,
                client_secret = ClientSecret,
                code = Code,
                state = State
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "https://github.com/login/oauth/access_token")
            {
                Content = new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json")
            };
            var response = await _httpClient.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode)
            {
                return null;
            }
            var data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data)) return string.Empty;
            var jObject = JObject.Parse(data);
            return jObject["access_token"].ToString();
        }

        private async Task<OAuth2Account> GetUserInfo()
        {
            var token = await GetAccessToken();
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.github.com/user");
            request.Headers.TryAddWithoutValidation("Authorization", $"token {token}");
            request.Headers.TryAddWithoutValidation("User-Agent", "liuyang-1990");
            var response = await _httpClient.SendAsync(request);
            if (response == null || !response.IsSuccessStatusCode) return null;
            var data = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(data)) return null;
            var jObject = JObject.Parse(data);
            return new OAuth2Account()
            {
                Avatar = jObject["avatar_url"].ToString(),
                Email = jObject["email"].ToString(),
                NickName = jObject["name"].ToString(),
                OpenID = jObject["node_id"].ToString()
            };
        }

        public async Task<UserInfo> Authorize(string code, string state)
        {
            Code = code;
            State = state;
            var account = await GetUserInfo();
            var user = await _accountRepository.SingleAsync(i => i.OpenId == account.OpenID);
            if (user == null) //保存用户信息
            {
                var result = await _accountRepository.UseTranAsync(async () =>
                {
                    var userInfo = new UserInfo()
                    {
                        UserName = account.NickName,
                        Avatar = account.Avatar,
                        Email = account.Email,
                        Password = _md5Helper.Encrypt("Blog.Core"),
                        IsActive = true,
                        Role = 0
                    };
                    var id = await _userRepository.InsertAsync(userInfo);
                    userInfo.Id = id;
                    var userOAuth = new UserOAuthMapping()
                    {
                        UserId = id,
                        OpenId = account.OpenID,
                        Type = "Github"
                    };
                    await _accountRepository.InsertAsync(userOAuth);
                    return userInfo;
                });
                if (result.IsSuccess)
                {
                    return await result.Data;
                }
                throw new ServiceException(result.ErrorMessage, "200", HttpStatusCode.BadRequest);
            }
            return await _userRepository.SingleAsync(x => x.Id == user.UserId);
        }


    }
}