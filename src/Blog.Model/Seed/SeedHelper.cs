using System;
using SqlSugar;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Blog.Infrastructure.Cryptography;
using Microsoft.Extensions.Logging;

namespace Blog.Model.Seed
{
    public class SeedHelper
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly IMd5Helper _md5Helper;
        private readonly ILogger<SeedHelper> _logger;
        public SeedHelper(ISqlSugarClient sqlSugarClient, IMd5Helper md5Helper, ILogger<SeedHelper> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _md5Helper = md5Helper;
            _logger = logger;
        }

        public async Task SeedAsync()
        {
            try
            {
                _logger.LogInformation("start CreateDatabase...");
                //CreateDatabase
                _sqlSugarClient.DbMaintenance.CreateDatabase();
                _logger.LogInformation("End CreateDatabase...");
                var types = Assembly.Load("Blog.Model").GetTypes().Where(x => typeof(BaseEntity).IsAssignableFrom(x) && x != typeof(BaseEntity));
                _logger.LogInformation("Start CreateTable...");
                //createTable
                _sqlSugarClient.CodeFirst.InitTables(types.ToArray());
                _logger.LogInformation("End CreateTable...");


                if (await _sqlSugarClient.Queryable<UserInfo>().AnyAsync())
                {
                    _logger.LogInformation("UserInfo already exist...");
                }
                else
                {
                    await _sqlSugarClient.Insertable(new UserInfo()
                    {
                        UserName = "admin",
                        Password = _md5Helper.Encrypt("123456"),
                        Role = 1,
                        IsActive = true
                    }).ExecuteCommandAsync();
                    _logger.LogInformation("Insert UserInfo...");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("SeedHelper error:" + ex);
            }

        }
    }
}