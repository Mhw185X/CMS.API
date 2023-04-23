using CMS.Models;
using CMS.Models.Entity;
using CMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class UserServices : IUserServices
    {
        private readonly UserRepository _repository;

        public UserServices(UserRepository repository)
        {
            _repository = repository;
        }

        public IActionResult registerUser(User users)
        {
            try
            {
                if (_repository.Insert(users))
                {
                    return new OkObjectResult(HttpResult<User>.success("注册成功"));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                string err = ex.Message;
                if (ex.Message.Contains("Duplicate entry") && ex.Message.Contains(" for key 'Account'"))
                {
                    err = "账号重复,请换一个试试";
                }
                return new OkObjectResult(HttpResult<User>.error(err));
            }

            return new BadRequestObjectResult(HttpResult<User>.error("系统异常"));

        }

    }
}
