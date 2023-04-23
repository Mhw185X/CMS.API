using SqlSugar;

namespace CMS.API.Extensions
{
    public static class SqlsugarSetup
    {
        public static void SqlSugarSetup(this IServiceCollection services,IConfiguration configuration,string ConnStr = "ConnStrings")
        {
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.MySql,
                ConnectionString = configuration[ConnStr],
                IsAutoCloseConnection = true,
            },
            db =>
            {
                db.Aop.OnLogExecuting = (sql, pars) =>
                {
                    // 输出执行的SQL
                    Console.WriteLine(sql);
                };
            }
            );
            // 这边是SqlSugarScope用AddSingleton
            services.AddSingleton<ISqlSugarClient>(sqlSugar);
        }
    }
}
