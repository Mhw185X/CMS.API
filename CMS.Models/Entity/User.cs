using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CMS.Models.Entity
{
    [SugarTable("Users")]
    public class User:Base
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true,IsIdentity = true)]
        public int UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 真实姓名
        /// </summary>
        public string RealName { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 手机号
        /// </summary>
        public string Phonenumber { get; set; }

        /// <summary>
        /// 用户性别（0男 1女）
        /// </summary>
        public string Sex { get; set; }

        /// <summary>
        /// 用户状态（0正常 1停用）
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptId { get; set; }
    }
}
