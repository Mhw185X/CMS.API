using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using System.DirectoryServices.Protocols;
using CMS.Models;

namespace CMS.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected IActionResult SUCCESS(object data)
        {
            string jsonStr = GetJsonStr(GetHttpResult(data != null ? 200 : 401, data));
            return Content(jsonStr, "application/json");
        }

        /// <summary>
        /// json输出
        /// </summary>
        /// <param name="httpResult"></param>
        /// <returns></returns>
        protected IActionResult ToResponse(HttpResult httpResult)
        {
            string jsonStr = GetJsonStr(httpResult);

            return Content(jsonStr, "application/json");
        }

        /// <summary>
        /// 序列化为Json
        /// </summary>
        /// <param name="httpResult"></param>
        /// <returns></returns>
        private static string GetJsonStr(HttpResult httpResult)
        {
            var serializerSettings = new JsonSerializerSettings
            {
                // 设置为驼峰命名
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            };

            return JsonConvert.SerializeObject(httpResult, Formatting.Indented, serializerSettings);
        }

        /// <summary>
        /// 封装为HTTPReuslt
        /// </summary>
        /// <param name="resultCode"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        protected HttpResult GetHttpResult(int resultCode, string msg)
        {
            return new HttpResult(resultCode, msg);
        }

        protected HttpResult GetHttpResult(int resultCode, object? data = null)
        {
            var httpResult = new HttpResult(resultCode, resultCode.ToString())
            {
                data = data
            };

            return httpResult;
        }
    }
}
