using Microsoft.AspNet.Identity.Owin;
using moonstone.core.services.results;
using moonstone.resources;
using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.User;
using System;
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

            var res = Routes.Home;
            return this.RedirectToAction(res.Action, res.Controller);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult LoggedOut()
        {
            var res = Routes.Login;
            return this.RedirectToAction(res.Action, res.Controller);
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
            try
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
                        return this.JsonSuccess(
                            data: null,
                            message: string.Format(ValidationResources.User_Login_Success, model.Email),
                            returnUrl: Routes.Home.GetActionLink(Url));
                    }
                    else
                    {
                        switch (result)
                        {
                            case LoginResult.LockedOut:
                                return this.JsonError(data: null, message: ValidationResources.User_Login_LockedOut);

                            case LoginResult.RequiresVerification:
                                return this.JsonError(data: null, message: ValidationResources.User_Login_RequiresVerification);

                            case LoginResult.Failed:
                            default:
                                return this.JsonError(data: null, message: ValidationResources.User_Login_Failed);
                        }
                    }
                }
                else
                {
                    return this.JsonError(data: null, message: ValidationResources.Generic_ModelState_Error);
                }
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult Logout()
        {
            this.Current.Services.LoginService.Logout();

            var res = Routes.LoggedOut;
            return RedirectToAction(res.Action, res.Controller);
        }

        [HttpGet]
        public ActionResult SelectGroup()
        {
            var model = new SelectGroupViewModel();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SelectGroup(SelectGroupViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.Current.Services.UserService.SetCurrentGroup(userId: Current.UserId.Value, groupId: model.GroupId);

                var res = Routes.Home;
                return this.RedirectToAction(res.Action, res.Controller);
            }

            return View(model);
        }
    }
}