using Microsoft.AspNet.Identity;
using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.authentication.managers
{
    public class UserManager : UserManager<User, Guid>
    {
        public UserManager(IUserStore<User, Guid> store)
            : base(store)
        {
        }
    }
}