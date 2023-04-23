using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class HttpResult
    {

        public int code { get; set; }
        public string msg { get; set; }
        /// <summary>
        /// 如果data值为null，则忽略序列化将不会返回data字段
        /// </summary>
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object? data { get; set; }

        /// <summary>
        /// 初始化一个新创建的HttpResult对象，使其表示一个空消息
        /// </summary>
        public HttpResult()
        {
        }

        /// <summary>
        /// 初始化一个新创建的 HttpResult 对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public HttpResult(int code, string msg)
        {
            this.code = code;
            this.msg = msg;
        }

        /// <summary>
        /// 初始化一个新创建的 HttpResult 对象
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        public HttpResult(int code, string msg, object data)
        {
            this.code = code;
            this.msg = msg;
            if (data != null)
            {
                this.data = data;
            }
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <returns></returns>
        public HttpResult Success()
        {
            this.code = 200;
            this.msg = "success";
            return this;
        }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="data">数据对象</param>
        /// <returns>成功消息</returns>
        public static HttpResult Success(object data) { return new HttpResult(200, "success", data); }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <returns>成功消息</returns>
        public static HttpResult Success(string msg) { return new HttpResult(200, msg, null); }

        /// <summary>
        /// 返回成功消息
        /// </summary>
        /// <param name="msg">返回内容</param>
        /// <param name="data">数据对象</param>
        /// <returns>成功消息</returns>
        public static HttpResult Success(string msg, object data) { return new HttpResult(200, msg, data); }

        /// <summary>
        /// 访问被拒
        /// </summary>
        /// <param name="apiResult"></param>
        /// <returns></returns>
        public HttpResult On401()
        {
            this.code = 401;
            msg = "access denyed";
            return this;
        }
        public HttpResult Error(ResultCode resultCode, string msg = "")
        {
            this.code = (int)resultCode;
            this.msg = msg;
            return this;
        }

        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="code"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static HttpResult Error(int code, string msg) { return new HttpResult(code, msg); }

/*        /// <summary>
        /// 返回失败消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static HttpResult Error(string msg) { return new HttpResult((int)ResultCode.CUSTOM_ERROR, msg); }*/

    }
}
