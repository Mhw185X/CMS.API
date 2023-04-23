using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CMS.Models.Vo
{
    public class UserVo
    {


        public int UserId { get; set; }

        public string UserName { get; set; }

        public string token { get; set; }

        public UserVo(int userId, string userName, string token)
        {
            UserId = userId;
            UserName = userName;
            this.token = token;
        }
    }
}
