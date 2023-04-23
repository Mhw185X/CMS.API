using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models
{
    public class Base
    {

        [SugarColumn(IsOnlyIgnoreUpdate = true)]//设置后修改不会有此字段
        public DateTime Create_time { get; set; } = DateTime.Now;

        [SugarColumn(IsOnlyIgnoreInsert = true)]//设置后插入数据不会有此字段
        public DateTime? Update_time { get; set; } = DateTime.Now;

    }
}
