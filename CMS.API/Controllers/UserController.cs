using CMS.Common.Helper;
using CMS.Models;
using CMS.Models.Dto;
using CMS.Models.Entity;
using CMS.Models.Vo;
using CMS.Services.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace CMS.API.Controllers
{

    /// <summary>
    /// 用户类
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public UserController(IUserService userService, IConfiguration configuration)
        {
            _userService = userService;
            _configuration = configuration;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public IActionResult Login(LoginUser loginUser)
        {
            User user = _userService.UserLogin(loginUser);

            TokenModelJwt tokenModel = _configuration.GetSection("Jwt").Get<TokenModelJwt>();
            tokenModel.UserId = user.UserId;
            tokenModel.UserName = user.UserName;
            
            UserVo vo = new UserVo(user.UserId,user.UserName,JwtHelper.CreateJwt(tokenModel));
            return ToResponse(new HttpResult(200,"登录成功",vo));
        }

        /// <summary>
        /// 验证登录
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost("check")]
        public IActionResult CheckLogin(string token)
        {
            if(JwtHelper.SerializeJwt(token) != null)
            {
                return ToResponse(new HttpResult(200, "验证成功", null));
            }else
            {
                return ToResponse(new HttpResult(403, "登录状态已失效", null));
            }
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        public IActionResult CreateUser(UserDto userDto)
        {
            User user = new User
            {
                RealName = userDto.RealName,
                UserName = userDto.UserName,
                Password = userDto.Password,
                Phonenumber = userDto.Phonenumber,
                Sex = userDto.Sex,
                Status = "0",
            };

            if(user != null)
            {
                if (_userService.registerUser(user) == 1)
                {
                    return ToResponse(new HttpResult(200, "注册成功"));
                }
                else
                {
                    return ToResponse(new HttpResult(500, "注册失败"));
                }
            }else 
            {
                return ToResponse(new HttpResult(500, "用户信息不可为空"));
            };
        }

        /// <summary>
        /// 用户删除
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpDelete("delete")]
        public IActionResult DeleteUser(int userId)
        {
            if(_userService.DeleteUser(userId) == 1)
            {
                return ToResponse(new HttpResult(200, "删除成功"));
            }else { return ToResponse(new HttpResult(500, "删除失败"));}
            
        }


    }
}
