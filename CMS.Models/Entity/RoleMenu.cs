using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Entity
{
    [SugarTable("role_menu")]
    public class RoleMenu
    {
        [SugarColumn(IsPrimaryKey = true)]
        public long Role_id { get; set; }

        [SugarColumn(IsPrimaryKey = true)]
        public long Menu_id { get; set; }
    }
}
