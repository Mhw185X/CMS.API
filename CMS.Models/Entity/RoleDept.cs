using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    [SugarTable("sys_role_dept")]
    public class RoleDept
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long RoleId { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public long DeptId { get; set; }
    }
}
