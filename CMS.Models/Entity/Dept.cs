using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    /// <summary>
    /// 部门表
    /// </summary>
    [SugarTable("dept")]
    public class Dept:Base
    {
        /// <summary>
        /// 部门ID
        /// </summary>
        [SqlSugar.SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public long DeptId { get; set; }

        /// <summary>
        /// 上级部门ID
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }

        /// <summary>
        /// 部门领导
        /// </summary>
        public string Leader { get; set; }
    }
}
