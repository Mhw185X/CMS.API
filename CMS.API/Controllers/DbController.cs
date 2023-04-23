using CMS.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace CMS.API.Controllers
{
    /// <summary>
    /// 数据库控制器
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class DbController : Controller
    {
        private readonly ISqlSugarClient _sqlSugarClient;

        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="sqlSugarClient"></param>
        public DbController(ISqlSugarClient sqlSugarClient)
        {
            _sqlSugarClient = sqlSugarClient;
        }

        [HttpGet("CreateTable")]
        public void CreateUserTable()
        {
            //  创建数据表
            _sqlSugarClient.CodeFirst.InitTables(
                typeof(Dept),
                typeof(Menu),
                typeof(Role),
                typeof(User),
                typeof(RoleDept),
                typeof(RoleMenu), 
                typeof(UserRole)
                );
        }

    }
}
