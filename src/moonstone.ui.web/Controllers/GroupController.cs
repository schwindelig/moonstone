using moonstone.core.models;
using moonstone.resources;
using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.Group;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class GroupController : BaseController
    {
        public GroupController(Current current)
            : base(current)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateGroupViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateGroupViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.Current.Services.GroupService.CreateGroup(
                        new Group
                        {
                            CreateDateUtc = DateTime.UtcNow,
                            CreateUserId = Current.UserId.Value,
                            Description = model.Description,
                            Name = model.Name
                        });

                    return this.JsonSuccess(data: null, message: string.Format(ValidationResources.Group_Create_Success, model.Name));
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
        public ActionResult Index()
        {
            return View();
        }
    }
}