using Blog.Business.Implement;
using Blog.Infrastructure;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Test.Business
{
    public class UserBusinessUnitTest
    {
        private readonly Mock<IUserRepository> _userRepository;
        private readonly UserBusiness _userBusiness;
        public UserBusinessUnitTest()
        {
            var md5Helper = new Mock<IMd5Helper>();
            _userRepository = new Mock<IUserRepository>();
            md5Helper.Setup(x => x.Encrypt32(It.IsAny<string>())).Returns("123");
            _userBusiness = new UserBusiness(_userRepository.Object, md5Helper.Object);
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
                .Setup(x => x.GetPageList(It.IsAny<GridParams>(), It.IsAny<Expression<Func<UserInfo, bool>>>()))
                .ReturnsAsync(() => expectedModel);
            var actualModel = await _userBusiness.GetPageList(new UserRequest() { Status = 1 }, new GridParams());
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Insert_Test(UserInfo userInfo, bool isExist, ResultModel<string> expectedModel)
        {

            _userRepository.Setup(x => x.IsExist(userInfo, UserAction.Add)).ReturnsAsync(() => isExist);
            _userRepository.Setup(x => x.Insert(userInfo)).ReturnsAsync(() => true);
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

        [Fact]
        public async Task GetUserByUserName_Test_Ok()
        {
            _userRepository.Setup(x => x.GetUserByUserName("123", "123")).ReturnsAsync(() => new UserInfo()
            {
                Id = 1,
                UserName = "123"
            });
            var actualModel = await _userBusiness.GetUserByUserName("123", "123");
            Assert.NotNull(actualModel);
        }

        [Fact]
        public async Task GetUserByUserName_Test_Exception()
        {
            _userRepository.Setup(x => x.GetUserByUserName("123", "123")).ThrowsAsync(new Exception("test"));
            var actualModel = await _userBusiness.GetUserByUserName("123", "123");
            Assert.Null(actualModel);
        }

        [Theory]
        [MemberData(nameof(Data4Password))]
        public async Task UpdatePassword_Test(UserInfo userInfo, ResultModel<string> expectedModel)
        {
            _userRepository.Setup(x => x.ChangePassword(userInfo)).ReturnsAsync(() => true);
            _userRepository.Setup(x => x.GetUserByUserName("123", "123")).ReturnsAsync(() => userInfo);
            var actualModel = await _userBusiness.UpdatePassword(new ChangePasswordRequest()
            {
                UserName = "123",
                OldPassword = "123",
                Password = "456"
            });
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);

        }

        public static List<object[]> Data4Password = new List<object[]>()
        {
            new object[]
            {
                null,
                new ResultModel<string>()
                {
                    Status="2"
                }
            },
            new object[]
            {
                new UserInfo(),
                new ResultModel<string>()
                {
                    Status="0",
                    IsSuccess = true
                }
            },

        };
    }
}