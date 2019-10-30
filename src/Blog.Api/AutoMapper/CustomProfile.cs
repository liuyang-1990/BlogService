using AutoMapper;
using Blog.Model.Db;
using Blog.Model.Request.Category;
using Blog.Model.Request.TimeLine;
using Blog.Model.Request.User;

namespace Blog.Api.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<CommonTimeLineRequest, TimeLine>();
            CreateMap<UpdateTimeLineRequest, TimeLine>();
            CreateMap<CommonCategoryRequest, CategoryInfo>();
            CreateMap<UpdateCategoryRequest, CategoryInfo>();
            CreateMap<AddUserRequest, UserInfo>();
            CreateMap<UpdateUserRequest, UserInfo>();
        }
    }
}