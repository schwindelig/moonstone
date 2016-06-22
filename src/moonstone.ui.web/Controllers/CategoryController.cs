using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class CategoryController : BaseController
    {
        public CategoryController(Current current)
            : base(current)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateCategoryViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateCategoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.Current.Services.CategoryService.CreateCategory(
                    new core.models.Category
                    {
                        CreateUserId = this.Current.User.Id,
                        GroupId = this.Current.User.CurrentGroupId.Value,
                        Name = model.Name
                    });

                var res = Routes.Home;
                return this.RedirectToAction(res.Action, res.Controller);
            }

            return View(model);
        }

        // GET: Category
        public ActionResult Index()
        {
            return View();
        }
    }
}