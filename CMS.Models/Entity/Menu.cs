using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    [SugarTable("menu")]
    public class Menu:Base
    {
        /// <summary>
        /// 菜单ID
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public long MenuId { get; set; }

        /// <summary>
        /// 菜单名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 父菜单ID
        /// </summary>
        public long parentId { get; set; }

        /// <summary>
        /// 显示顺序
        /// </summary>
        public int orderNum { get; set; }

        /// <summary>
        /// 路由地址
        /// </summary>
        public string path { get; set; } = "#";
    }
}
