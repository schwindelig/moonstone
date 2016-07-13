using moonstone.resources;
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
            try
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

                    return this.JsonSuccess(
                        data: null,
                        message: string.Format(ValidationResources.Category_Create_Success, model.Name),
                        returnUrl: Routes.IndexCategories.GetActionLink(Url));
                }

                return this.JsonError(data: null, message: ValidationResources.Generic_ModelState_Error);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Generic_Error);
            }
        }

        [HttpGet]
        public ActionResult GetAllCategoriesForCurrentGroup()
        {
            try
            {
                var res =
                    this.Current.Services.CategoryService.GetCategoriesForGroup(
                        this.Current.User.CurrentGroupId.Value)
                    .Select(c => new { name = c.Name, value = c.Id })
                    .OrderBy(c => c.name);

                return this.JsonSuccess(data: res, message: null);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.Category_GetAllForCurrentGroup_Error);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}