using moonstone.ui.web.Models;
using System.Web.Mvc;

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
    }
}