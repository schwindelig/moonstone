using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using moonstone.core.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.authentication.managers
{
    public class SignInManager : Microsoft.AspNet.Identity.Owin.SignInManager<User, Guid>
    {
        public SignInManager(UserManager<User, Guid> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}