using Microsoft.AspNet.Identity;
using moonstone.ui.web.Models;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace moonstone.ui.web.Controllers
{
    [Authorize]
    public class BaseController : Controller
    {
        public Current Current { get; set; }

        public BaseController(Current current)
        {
            this.Current = current;
        }

        public void AddModelError(string message)
        {
            this.ModelState.AddModelError(string.Empty, message);
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                this.Current.Services.EnvironmentService.SetCulture(this.Current.User.Culture);

                var logoutRoute = Routes.Logout;
                var selectGroupRoute = Routes.SelectGroup;
                if (!this.Current.User.CurrentGroupId.HasValue &&
                    !selectGroupRoute.IsSame(filterContext.RouteData) &&
                    !logoutRoute.IsSame(filterContext.RouteData))
                {
                    filterContext.Result = this.RedirectToAction(selectGroupRoute.Action, selectGroupRoute.Controller);
                }
            }

            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception;
            filterContext.ExceptionHandled = true;

            var res = Routes.Error;
            filterContext.Result = this.RedirectToAction(res.Action, res.Controller);

            base.OnException(filterContext);
        }
    }
}