using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Dto
{
    public class LoginUser
    {
        /// <summary>
        /// 用户名
        /// </summary> 
        public string userName  { get; set; }
        
        /// <summary>
        /// 用户密码
        /// </summary>
        public string password { get; set; }
    }
}
