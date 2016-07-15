using moonstone.ui.web.Models;
using System;
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

#if DEBUG

        [AllowAnonymous]
        public string Seeed()
        {
            var res = this.Current.Authentication.UserManager.CreateAsync(
                new core.models.User
                {
                    Email = "david.szoeke@gmail.com",
                    Culture = "en-US",
                    CreateDateUtc = DateTime.UtcNow
                }, "mooT$12!d9");

            if (!res.Result.Succeeded)
            {
                return "failed to create user";
            }

            var user = this.Current.Services.UserService.GetUerByEmail("david.szoeke@gmail.com");
            var group = this.Current.Services.GroupService.CreateGroup(new core.models.Group()
            {
                CreateUserId = user.Id,
                Description = "Seeded default group for david.szoeke@gmail.com",
                Name = "David's group",
                CreateDateUtc = DateTime.UtcNow
            });
            user.CurrentGroupId = group.Id;
            this.Current.Services.UserService.SetCurrentGroup(user.Id, group.Id);

            return "seeded";
        }

#endif
    }
}