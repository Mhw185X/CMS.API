using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    [SugarTable("role")]
    public class Role:Base
    {
        /// <summary>
        /// 角色ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long RoleId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 角色权限
        /// </summary>
        public string RoleKey { get; set; }

        /// <summary>
        /// 菜单组
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public long[] MenuIds { get; set; }


    }
}
