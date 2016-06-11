using moonstone.ui.web.Models;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(Current current) : base(current)
        {
        }

        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public string Seeed()
        {
            var res = this.Current.Authentication.UserManager.CreateAsync(
                new core.models.User
                {
                    Email = "david.szoeke@gmail.com",
                    Culture = "en-US"
                }, "mooT$12!d9");

            if (!res.Result.Succeeded)
            {
                return "failed to create user";
            }

            return "seeded";
        }
    }
}