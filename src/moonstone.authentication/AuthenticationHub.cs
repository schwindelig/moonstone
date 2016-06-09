using moonstone.authentication.managers;
using moonstone.authentication.stores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.authentication
{
    public class AuthenticationHub
    {
        public SignInManager SignInManager { get; set; }
        public UserManager UserManager { get; set; }
        public UserStore UserStore { get; set; }

        public AuthenticationHub(SignInManager signInManager, UserManager userManager, UserStore userStore)
        {
            this.SignInManager = signInManager;
            this.UserManager = userManager;
            this.UserStore = userStore;
        }
    }
}