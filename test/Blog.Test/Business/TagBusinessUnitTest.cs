using Blog.Business.Implement;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Tag;
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
    public class TagBusinessUnitTest
    {
        private readonly Mock<ITagRepository> _tagRespoitory;
        private readonly TagBusiness _tagBusiness;

        public TagBusinessUnitTest()
        {
            _tagRespoitory = new Mock<ITagRepository>();
            _tagBusiness = new TagBusiness(_tagRespoitory.Object);
        }


        [Theory]
        [MemberData(nameof(Data))]
        public async Task Insert_Test(TagInfo tagInfo, bool isExist, ResultModel<string> expectedModel)
        {
            _tagRespoitory
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TagInfo, bool>>>())).ReturnsAsync(isExist);
            _tagRespoitory.Setup(x => x.Insert(It.IsAny<TagInfo>())).ReturnsAsync(1);
            var actualModel = await _tagBusiness.Insert(tagInfo);
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

        public static List<object[]> Data = new List<object[]>()
        {
            new object[]
            {
                new TagInfo(),
                true,
                new ResultModel<string>()
                {
                    Status="2"
                }
            },
            new object[]
            {
                new TagInfo(),
                false,
                new ResultModel<string>()
                {
                    IsSuccess = true,
                    Status="0"
                }
            }
        };

        [Fact]
        public async Task GetPageList_Test()
        {
            var expectedModel = new JsonResultModel<TagInfo>()
            {
                Rows = new List<TagInfo>(),
                TotalRows = 10
            };
            _tagRespoitory
                .Setup(x => x.QueryByPage(It.IsAny<GridParams>(), It.IsAny<Expression<Func<TagInfo, bool>>>()))
                .ReturnsAsync(() => expectedModel);
            var actualModel = await _tagBusiness.GetPageList(new TagSearchRequest());
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

        [Fact]
        public async Task GetAllTags_Test()
        {
            _tagRespoitory.Setup(x => x.QueryAll()).ReturnsAsync(() => new List<TagInfo>() { new TagInfo() });
            var actualModel = await _tagBusiness.GetAllTags();
            Assert.Single(actualModel);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Update_Test(TagInfo tagInfo, bool isExist, ResultModel<string> expectedModel)
        {
            _tagRespoitory
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<TagInfo, bool>>>())).ReturnsAsync(isExist);
            _tagRespoitory.Setup(x => x.Update(It.IsAny<TagInfo>())).ReturnsAsync(true);
            var actualModel = await _tagBusiness.Update(tagInfo);
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

    }
}