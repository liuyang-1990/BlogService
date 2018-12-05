﻿using Blog.Business;
using Blog.Model;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Controllers
{
    [BlogApiController]
    public class TagController : ControllerBase
    {
        private readonly ITagBusiness _tagBusiness;
        public TagController(ITagBusiness tagBusiness)
        {
            _tagBusiness = tagBusiness;
        }


        [HttpGet("page")]
        public string GetPageList(int pageIndex, int pageSize)
        {
            return _tagBusiness.GetPageList(pageIndex, pageSize);
        }

        [HttpGet("{id}")]
        public string GetDetailInfo(int id)
        {
            return _tagBusiness.GetDetail(id);
        }

        [HttpPost]
        public bool AddTag([FromBody]Tag tag)
        {

            return _tagBusiness.Insert(tag);
        }

        [HttpDelete("{id}")]
        public bool DeleteTag(int id)
        {
            return _tagBusiness.Delete(id);
        }

        [HttpPut]
        public bool UpdateTag([FromBody]Tag tag)
        {
            return _tagBusiness.Update(tag);
        }
    }
}