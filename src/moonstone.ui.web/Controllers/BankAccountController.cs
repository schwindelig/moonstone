using moonstone.resources;
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
            try
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

                    return this.JsonSuccess(
                        data: null,
                        message: string.Format(ValidationResources.BankAccount_Create_Success, model.Name),
                        returnUrl: Routes.IndexBankAccounts.GetActionLink(Url));
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
        public ActionResult GetAllBankAccountsForCurrentGroup()
        {
            try
            {
                var res = this.Current.Services.BankAccountService.GetBankAccountsForGroup(
                    this.Current.User.CurrentGroupId.Value)
                    .Select(b => new { name = b.Name, value = b.Id })
                    .OrderBy(b => b.name);

                return this.JsonSuccess(data: res, message: null);
            }
            catch (Exception e)
            {
                this.HandleError(e);
                return this.JsonError(data: null, message: ValidationResources.BankAccount_GetAllForCurrentGroup_Error);
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}