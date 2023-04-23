using CMS.Common.Helper;
using CMS.Models;
using CMS.Models.Entity;
using CMS.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.Json;
using SqlSugar;
namespace CMS.API.Controllers
{
    /// <summary>
    /// 天气类
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IUserService _userService;
        ISqlSugarClient _sqlSugarClient;

        public WeatherForecastController(ISqlSugarClient sqlSugarClient, IUserService userService)
        {
            _sqlSugarClient = sqlSugarClient;
            _userService = userService;
        }

        /// <summary>
        /// 获取天气数据
        /// </summary>
        /// <returns></returns>
        [HttpGet("Weather")]
        [Authorize]
        public IEnumerable<WeatherForecast> GetWeather()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /// <summary>
        /// 测试是否能返回token 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost(Name = "Login")]
        public string Login(string account, string password)
        {
            //仅为模拟用户输入登录的情况
            if (account == "root" && password == "123456")
            {
                //这一段为获取appsettings.json定义的数据 , 可以忽视,后面用依赖注入的方法会更加方便
                IConfiguration configuration = new
                 ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Add(new JsonConfigurationSource
                 {
                     Path = "appsettings.json",
                     ReloadOnChange = true
                 }).Build(); ;
                TokenModelJwt tokenModel = configuration.GetSection("Jwt").Get<TokenModelJwt>();

                tokenModel.UserId = 1;
                /*tokenModel.Role = "Admin";*/
                tokenModel.UserName = "用户名";
                return JwtHelper.CreateJwt(tokenModel);
            }
            return "账号密码错误";
        }

        /// <summary>
        /// 创建数据表
        /// </summary>
        [HttpGet("Create")]
        public void CreateUserTable()
        {
            //  创建数据表
            _sqlSugarClient.CodeFirst.InitTables();
        }

    }
}