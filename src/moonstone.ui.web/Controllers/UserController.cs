using Microsoft.AspNet.Identity.Owin;
using moonstone.core.services.results;
using moonstone.resources;
using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.User;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class UserController : BaseController
    {
        public UserController(Current current) : base(current)
        {
        }

        [HttpGet]
        public ActionResult ChangeCulture(string culture)
        {
            this.Current.Services.UserService.SetCulture(this.Current.UserId.Value, culture);

            return this.RedirectToRoute(Routes.Get().Home);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            var model = new LoginViewModel
            {
                ReturnUrl = returnUrl
            };

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = this.Current.Services.LoginService.Login(
                    username: model.Email,
                    password: model.Password,
                    isPersistent: false,
                    shouldLockOut: false);

                if (result == LoginResult.Success)
                {
                    return this.RedirectToRoute(Routes.Get().Home);
                }
                else
                {
                    switch (result)
                    {
                        case LoginResult.Failed:
                            this.AddModelError(ValidationResources.Login_Failed);
                            break;

                        case LoginResult.LockedOut:
                            this.AddModelError(ValidationResources.Login_LockedOut);
                            break;

                        case LoginResult.RequiresVerification:
                            this.AddModelError(ValidationResources.Login_RequiresVerification);
                            break;
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.Current.Services.LoginService.Logout();

            return RedirectToRoute(Routes.Get().Logout);
        }
    }
}