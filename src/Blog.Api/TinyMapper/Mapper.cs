using Blog.Model.Db;
using Blog.Model.Request.Category;
using Blog.Model.Request.TimeLine;
using Blog.Model.Request.User;
using Nelibur.ObjectMapper;

namespace Blog.Api
{
    public class Mapper
    {
        public static void InitMapping()
        {
            TinyMapper.Bind<UpdateTimeLineRequest, TimeLine>();
            TinyMapper.Bind<CommonCategoryRequest, CategoryInfo>();
            TinyMapper.Bind<UpdateCategoryRequest, CategoryInfo>();
            TinyMapper.Bind<AddUserRequest, UserInfo>();
            TinyMapper.Bind<UpdateUserRequest, UserInfo>();
        }
    }
}
