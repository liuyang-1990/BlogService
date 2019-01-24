﻿using Blog.Model.Db;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Blog.Business
{
    public interface IArticleBusiness : IBaseBusiness<ArticleInfo>
    {

        Task<BaseResponse> Insert(ArticleDto articleDto);

        Task<BaseResponse> Update(ArticleDto articleDto);

        Task<V_Article_Info> GetArticleDetail(int id);

        Task<List<ArticleInfo>> GetArticleByCategory(int categoryId, int pageIndex, int pageSize);

        Task<List<ArticleInfo>> GetArticleByTag(int tagId, int pageIndex, int pageSize);
    }
}