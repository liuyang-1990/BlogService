using Blog.Infrastructure;
using Blog.Infrastructure.Implement;
using Blog.Model;
using Blog.Model.Settings;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace Blog.Test.Infrastructure
{
    public class JwtHelperUnitTest
    {
        private readonly Mock<IOptions<JwtConfig>> _jwtConfig;
        private readonly Mock<IRedisHelper> _redisHelper;
        public JwtHelperUnitTest()
        {
            _jwtConfig = new Mock<IOptions<JwtConfig>>();
            _redisHelper = new Mock<IRedisHelper>();
            _jwtConfig.Setup(x => x.Value).Returns(() => new JwtConfig()
            {
                Audience = "test",
                Issuer = "test",
                SecurityKey = Guid.NewGuid().ToString()
            });
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IssueJwt_Test(bool isRefresh)
        {
            var helper = new JwtHelper(_jwtConfig.Object, _redisHelper.Object);
            var res = helper.IssueJwt(new JwtToken()
            {
                Uid = 1,
                Role = "Admin"
            }, isRefresh);
            Assert.NotNull(res);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public void RefreshJwt_Test_Null(bool contains, string value)
        {
            _redisHelper.Setup(x => x.ContainsKey("refresh_token_1")).Returns(contains);
            _redisHelper.Setup(x => x.Get<string>("refresh_token_1")).Returns(value);
            var helper = new JwtHelper(_jwtConfig.Object, _redisHelper.Object);
            var res = helper.RefreshJwt("test", new JwtToken()
            {
                Uid = 1,
                Role = "Admin"
            });
            Assert.Null(res);

        }

        public static List<object[]> Data = new List<object[]>()
        {
            new object[]
            {
               false,
               null
            },
            new object[]
            {
                true,
                null
            },
            new object[]
            {
                true,
                "test1"
            }
        };


        [Fact]
        public void RefreshJwt_Test_Ok()
        {
            _redisHelper.Setup(x => x.ContainsKey("refresh_token_1")).Returns(true);
            _redisHelper.Setup(x => x.Get<string>("refresh_token_1")).Returns("test");
            var helper = new JwtHelper(_jwtConfig.Object, _redisHelper.Object);
            var res = helper.RefreshJwt("test", new JwtToken()
            {
                Uid = 1,
                Role = "Admin"
            });
            Assert.NotNull(res);

        }
    }
}