﻿using Blog.Infrastructure;
using Blog.Model;
using Blog.Model.Db;
using Blog.Model.Request;
using Blog.Model.Response;
using Blog.Model.ViewModel;
using Blog.Repository;
using NLog;
using SqlSugar;
using System;
using System.Threading.Tasks;

namespace Blog.Business.Implement
{
    public class UserBusiness : BaseBusiness<UserInfo>, IUserBusiness
    {
        private readonly IMd5Helper _md5Helper;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        public UserBusiness(IUserRepository repository, IMd5Helper md5Helper)
        {
            BaseRepository = repository;
            _md5Helper = md5Helper;
            _userRepository = repository;
        }


        public async Task<JsonResultModel<UserInfo>> GetPageList(UserRequest searchParams, GridParams param)
        {
            var exp = Expressionable.Create<UserInfo>()
                .OrIF(!string.IsNullOrEmpty(searchParams.UserName), it => it.UserName.Contains(searchParams.UserName))
                .AndIF(true, it => it.Status == searchParams.Status).ToExpression();
            return await base.GetPageList(param, exp);
        }


        public override async Task<ResultModel<string>> Insert(UserInfo user)
        {
            var response = new ResultModel<string>();
            var isExist = await _userRepository.IsExist(user, UserAction.Add);
            if (isExist)
            {
                response.IsSuccess = false;
                response.Status = "2";//已经存在
                return response;
            }
            if (string.IsNullOrEmpty(user.Password))
            {
                user.Password = "123456"; //默认密码
            }
            user.Password = _md5Helper.Encrypt32(user.Password);
            return await base.Insert(user);
        }
        public override async Task<ResultModel<string>> Update(UserInfo user)
        {
            var response = new ResultModel<string>();
            var isExist = await _userRepository.IsExist(user, UserAction.Update);
            if (isExist)
            {
                response.IsSuccess = false;
                response.Status = "2";//已经存在
                return response;
            }
            if (!string.IsNullOrEmpty(user.Password))
            {
                user.Password = _md5Helper.Encrypt32(user.Password);
            }
            return await base.Update(user);
        }


        public async Task<UserInfo> GetUserByUserName(string userName, string password)
        {
            try
            {
                password = _md5Helper.Encrypt32(password);
                return await _userRepository.GetUserByUserName(userName, password);
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return null;
            }
        }

        public async Task<ResultModel<string>> UpdatePassword(ChangePasswordRequest request)
        {
            var response = new ResultModel<string>();
            var userInfo = await GetUserByUserName(request.UserName, request.OldPassword);
            if (userInfo == null)
            {
                response.IsSuccess = false;
                response.Status = "2"; //旧密码不正确
                return response;
            }
            userInfo.Password = _md5Helper.Encrypt32(request.Password);
            response.IsSuccess = await _userRepository.ChangePassword(userInfo);
            response.Status = response.IsSuccess ? "0" : "1";
            return response;

        }
    }
}