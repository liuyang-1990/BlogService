using Blog.Infrastructure.Implement;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using Xunit;

namespace Blog.Test.Infrastructure
{
    public class JwtHelperUnitTest
    {
        private readonly Mock<IConfiguration> _configuration;
        private readonly Mock<IDistributedCache> _redisHelper;
        public JwtHelperUnitTest()
        {
            _configuration = new Mock<IConfiguration>();
            _redisHelper = new Mock<IDistributedCache>();
            _configuration.Setup(x => x["JwtAuth:Issuer"]).Returns("test");
            _configuration.Setup(x => x["JwtAuth:Audience"]).Returns("test");
            _configuration.Setup(x => x["JwtAuth:SecurityKey"]).Returns(Guid.NewGuid().ToString());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IssueJwt_Test(bool isRefresh)
        {
            var helper = new JwtHelper(_configuration.Object, _redisHelper.Object);
            var res = helper.IssueJwt(new Blog.Infrastructure.JwtToken()
            {
                Uid = "1",
                Role = "Admin"
            }, isRefresh);
            Assert.NotNull(res);
        }

        //[Theory]
        //[MemberData(nameof(Data))]
        //public void RefreshJwt_Test_Null(string value)
        //{
        //    // _redisHelper.Setup(x => x.ContainsKey("refresh_token_1")).Returns(contains);
        //    _redisHelper.Setup(x => x.GetString("refresh_token_1")).Returns(value);
        //    var helper = new JwtHelper(_configuration.Object, _redisHelper.Object);
        //    var res = helper.RefreshJwt("test", new JwtToken()
        //    {
        //        Uid = 1,
        //        Role = "Admin"
        //    });
        //    Assert.Null(res);

        //}

        //public static List<object[]> Data = new List<object[]>()
        //{
        //    new object[]
        //    {
        //       false,
        //       null
        //    },
        //    new object[]
        //    {
        //        true,
        //        null
        //    },
        //    new object[]
        //    {
        //        true,
        //        "test1"
        //    }
        //};


        //[Fact]
        //public void RefreshJwt_Test_Ok()
        //{
        //    //_redisHelper.Setup(x => x.ContainsKey("refresh_token_1")).Returns(true);
        //    _redisHelper.Setup(x => x.GetString("refresh_token_1")).Returns("test");
        //    var helper = new JwtHelper(_configuration.Object, _redisHelper.Object);
        //    var res = helper.RefreshJwt("test", new JwtToken()
        //    {
        //        Uid = 1,
        //        Role = "Admin"
        //    });
        //    Assert.NotNull(res);

        //}
    }
}