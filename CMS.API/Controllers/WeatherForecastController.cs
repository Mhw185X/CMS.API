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
    /// ������
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
        /// ��ȡ��������
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
        /// �����Ƿ��ܷ���token 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost(Name = "Login")]
        public string Login(string account, string password)
        {
            //��Ϊģ���û������¼�����
            if (account == "root" && password == "123456")
            {
                //��һ��Ϊ��ȡappsettings.json��������� , ���Ժ���,����������ע��ķ�������ӷ���
                IConfiguration configuration = new
                 ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).Add(new JsonConfigurationSource
                 {
                     Path = "appsettings.json",
                     ReloadOnChange = true
                 }).Build(); ;
                TokenModelJwt tokenModel = configuration.GetSection("Jwt").Get<TokenModelJwt>();

                tokenModel.UserId = 1;
                /*tokenModel.Role = "Admin";*/
                tokenModel.UserName = "�û���";
                return JwtHelper.CreateJwt(tokenModel);
            }
            return "�˺��������";
        }

        /// <summary>
        /// �������ݱ�
        /// </summary>
        [HttpGet("Create")]
        public void CreateUserTable()
        {
            //  �������ݱ�
            _sqlSugarClient.CodeFirst.InitTables();
        }

    }
}