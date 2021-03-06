﻿using Blog.Business.Implement;
using Blog.Model.Common;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Request.Category;
using Blog.Model.Response;
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
    public class CategoryBusinessUnitTest
    {
        private readonly Mock<ICategoryRepository> _categoryRepository;
        private readonly CategoryBusiness _categoryBusiness;
        public CategoryBusinessUnitTest()
        {
            _categoryRepository = new Mock<ICategoryRepository>();
            _categoryBusiness = new CategoryBusiness(_categoryRepository.Object);
        }

        [Fact]
        public async Task GetPageList_Test()
        {
            var expectedModel = new JsonResultModel<CategoryInfo>()
            {
                Rows = new List<CategoryInfo>(),
                TotalRows = 10
            };
            _categoryRepository
                .Setup(x => x.Query(It.IsAny<GridParams>(), It.IsAny<Expression<Func<CategoryInfo, bool>>>()))
                .ReturnsAsync(() => expectedModel);
            var actualModel = await _categoryBusiness.GetPageList(new CategorySearchRequest());
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }



        [Theory]
        [MemberData(nameof(Data))]
        public async Task Insert_Test(CategoryInfo categoryRequest, bool isExist, ResultModel<string> expectedModel)
        {
            _categoryRepository
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<CategoryInfo, bool>>>())).ReturnsAsync(isExist);
            _categoryRepository.Setup(x => x.InsertAsync(It.IsAny<CategoryInfo>())).ReturnsAsync(1);
            var actualModel = await _categoryBusiness.InsertAsync(categoryRequest);
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }

        public static List<object[]> Data = new List<object[]>()
        {
            new object[]
            {
                new CategoryInfo(),
                true,
                new ResultModel<string>()
                {
                    Status="2"
                }
            },
            new object[]
            {
                new CategoryInfo(),
                false,
                new ResultModel<string>()
                {
                    IsSuccess = true,
                    Status="0"
                }
            }
        };


        [Fact]
        public async Task GetAll_Test()
        {
            _categoryRepository.Setup(x => x.QueryAll()).ReturnsAsync(() => new List<CategoryInfo>() { new CategoryInfo() });
            var actualModel = await _categoryBusiness.GetAllCategoryInfos();
            Assert.Single(actualModel);
        }

        [Theory]
        [MemberData(nameof(Data))]
        public async Task Update_Test(CategoryInfo categoryInfo, bool isExist, ResultModel<string> expectedModel)
        {
            _categoryRepository
                .Setup(x => x.AnyAsync(It.IsAny<Expression<Func<CategoryInfo, bool>>>())).ReturnsAsync(isExist);
            _categoryRepository.Setup(x => x.UpdateAsync(It.IsAny<CategoryInfo>())).ReturnsAsync(true);
            var actualModel = await _categoryBusiness.UpdateAsync(categoryInfo);
            var actualStr = JsonConvert.SerializeObject(actualModel);
            var expectedStr = JsonConvert.SerializeObject(expectedModel);
            Assert.Equal(expectedStr, actualStr);
        }



    }
}