using CMS.Models;
using CMS.Models.Dto;
using CMS.Models.Entity;
using CMS.Repository;
using CMS.Services.IService;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Services.Service
{
    public class UserService : IUserService
    {
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }

        public int DeleteUser(int id)
        {
            return _repository.Delete(id);
        }

        public int registerUser(User users)
        {
            return _repository.Insert(users);
        }

        public User UserLogin(LoginUser loginUser)
        {
            User user = _repository.Login(loginUser);

            if (user == null || user.UserId <= 0)
            {
                throw new Exception("用户名或密码错误");
            }
            if(user.Status == "1")
            {
                throw new Exception("此账号已被停用");
            }
            return user;
        }
    }
}
