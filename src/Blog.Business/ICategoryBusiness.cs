﻿using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface ICategoryBusiness : IBaseBusiness<CategoryInfo>
    {
        Task<JsonResultModel<CategoryInfo>> GetPageList(GridParams param, string categoryName);
        // Task<ResultModel<string>> Insert(CategoryRequest category);

        Task<IEnumerable<CategoryInfo>> GetAllCategoryInfos();
    }
}