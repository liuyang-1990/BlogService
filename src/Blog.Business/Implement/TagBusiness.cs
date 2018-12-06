﻿using Blog.Model.Db;
using Blog.Repository;

namespace Blog.Business.Implement
{
    public class TagBusiness : BaseBusiness<Tag>, ITagBusiness
    {
        public TagBusiness(ITagRespoitory respoitory)
        {
            base.BaseRepository = respoitory;
        }
    }
}