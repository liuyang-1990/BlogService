using Blog.Business.Implement;
using Blog.Infrastructure;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Blog.Model.Request.User;
using Xunit;

namespace Blog.Test.Business
{
    public class UserBusinessUnitTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UserBusiness _userBusiness;
        private readonly Mock<ILogger<UserBusiness>> _logger;
        public UserBusinessUnitTest()
        {
            var md5Helper = new Mock<IMd5Helper>();
            _logger = new Mock<ILogger<UserBusiness>>();
            _userRepository = new Mock<IUserRepository>();
            md5Helper.Setup(x => x.Encrypt32(It.IsAny<string>())).Returns("123");
            _userBusiness = new UserBusiness(_userRepository.Object, md5Helper.Object, _logger.Object);
        }

        [Fact]
        public async Task GetPageList_Test()
        {
            var expectedModel = new JsonResultModel<UserInfo>()
            {
                Rows = new List<UserInfo>(),
                TotalRows = 10
            };
            _userRepository
                .Setup(x => x.QueryByPage(It.IsAny<GridParams>(), It.IsAny<Expression<Func<UserInfo, bool>>>()))
                .ReturnsAsync(() => expectedModel);
            var actualModel = await _userBusiness.GetPageList(new UserSearchRequest() { Status = 1 });
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Insert_Test(UserInfo userInfo, bool isExist, ResultModel<string> expectedModel)
        {

            // _userRepository.Setup(x => x.IsExist(userInfo, UserAction.Add)).ReturnsAsync(() => isExist);
            // _userRepository.Setup(x => x.Insert(userInfo)).ReturnsAsync(() => "1");
            var actualModel = await _userBusiness.Insert(userInfo);
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

        public static List<object[]> Data = new List<object[]>()
        {
            new object[]
            {
                new UserInfo(),
                true,
                new ResultModel<string>()
                {
                    Status="2"
                }
            },
            new object[]
            {
                new UserInfo(),
                false,
                new ResultModel<string>()
                {
                    IsSuccess = true,
                    Status="0"
                }
            },
            new object[]
            {
                new UserInfo()
                {
                    Password = "123456"
                },
                false,
                new ResultModel<string>()
                {
                    IsSuccess = true,
                    Status="0"
                }
            }
        };

    }
}