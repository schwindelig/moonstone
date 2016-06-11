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
            }

            base.OnActionExecuting(filterContext);
        }
    }
}