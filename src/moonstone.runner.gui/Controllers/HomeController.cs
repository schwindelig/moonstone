using moonstone.authentication;
using moonstone.core.models;
using moonstone.sql.configs;
using moonstone.sql.context;
using moonstone.sql.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.runner.gui.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public async System.Threading.Tasks.Task<ActionResult> Index()
        {
            var context = new SqlContext("moonstone_dev_tests", ".");
            context.RegisterModelDescription(ModelDescriptions.User());

            var userRepo = new SqlUserRepository(context);
            var userStore = new moonstone.authentication.stores.UserStore(userRepo);

            var userManager = new moonstone.authentication.managers.UserManager(userStore);
            var signInManager = new authentication.managers.SignInManager(userManager, HttpContext.GetOwinContext().Authentication);

            var user = new User()
            {
                Email = $"login_{Guid.NewGuid().ToString().Substring(0, 8)}@schwindelig.ch"
            };
            var password = "p@55w0rd";
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception();
            }

            var loginRes = await signInManager.PasswordSignInAsync(user.UserName, password, isPersistent: false, shouldLockout: false);

            if (loginRes != Microsoft.AspNet.Identity.Owin.SignInStatus.Success)
            {
                throw new Exception();
            }

            return View();
        }
    }
}