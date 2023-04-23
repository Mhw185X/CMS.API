using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repository
{
    /**
     Where T ：class，这就是标识这个T为引用类型
    而new（）则表示这个泛型必须有构造函数否则不能使用。
     */
    public class BaseRepository<T>:SimpleClient<T> where T : class,new()
    {
        public BaseRepository(ISqlSugarClient context) : base(context)
        {
            base.Context = context;
        }

        public List<T>? CommQuery(string json)
        {
            //base.Context.Queryable<T>().ToList();可以拿到SqlSugarClient 做复杂操作
            return null;
        }
    }
}
