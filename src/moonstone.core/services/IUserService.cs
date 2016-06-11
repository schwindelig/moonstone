using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.services
{
    public interface IUserService
    {
        User CreateUser(User user);

        User GetUserById(Guid userId);

        void SetCulture(Guid userId, string culture);
    }
}