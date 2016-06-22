using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.BankAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class BankAccountController : BaseController
    {
        public BankAccountController(Current current) : base(current)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateBankAccountViewModel();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateBankAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                this.Current.Services.BankAccountService.CreateBankAccount(new core.models.BankAccount()
                {
                    Name = model.Name,
                    Description = model.Description,
                    CreateUserId = this.Current.User.Id,
                    GroupId = this.Current.User.CurrentGroupId.Value
                });

                var res = Routes.Home;
                return this.RedirectToAction(res.Action, res.Controller);
            }

            return View(model);
        }
    }
}