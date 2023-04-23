using CMS.Models.Entity;
using FluentValidation;

namespace CMS.API.Vaildator
{
    public class UsersValidator : AbstractValidator<User>
    {
        public UsersValidator()
        {
            //对每个字段进行单独校验



        }
    }
}
