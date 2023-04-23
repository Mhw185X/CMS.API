using CMS.Models.Dto;
using CMS.Models.Entity;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Repository
{
    public class UserRepository : BaseRepository<User>
    {
        public UserRepository(ISqlSugarClient context) : base(context)
        {
        }

        public int Insert(User user)
        {
            return Context.Insertable(user).ExecuteCommand();
        }

        public User Login(LoginUser loginUser)
        {
            return Context.Queryable<User>().First(it => it.UserName == loginUser.userName && it.Password == loginUser.password);
        }

        public int Delete(int userId)
        {
            return Context.Deleteable<User>().Where(it => it.UserId == userId).ExecuteCommand();
        }
    }
}
