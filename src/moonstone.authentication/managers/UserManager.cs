using Microsoft.AspNet.Identity;
using moonstone.core.models;
using System;

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