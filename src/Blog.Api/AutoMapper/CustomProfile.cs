using AutoMapper;
using Blog.Model.Db;
using Blog.Model.Request;

namespace Blog.Api.AutoMapper
{
    public class CustomProfile : Profile
    {
        /// <summary>
        /// 配置构造函数，用来创建关系映射
        /// </summary>
        public CustomProfile()
        {
            CreateMap<UserRequest, UserInfo>();
        }
    }
}