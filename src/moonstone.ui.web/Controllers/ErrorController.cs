using moonstone.ui.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(Current current)
            : base(current)
        {
        }

        // GET: Error
        public ActionResult Index()
        {
            return View();
        }
    }
}