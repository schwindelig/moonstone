using moonstone.resources;
using moonstone.ui.web.Models;
using moonstone.ui.web.Models.ViewModels.Transaction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class TransactionController : BaseController
    {
        public TransactionController(Current current) : base(current)
        {
        }

        [HttpGet]
        public ActionResult Create()
        {
            var model = new CreateTransactionViewModel
            {
                IsBooked = true,
                ValueDate = DateTime.UtcNow.ToLocalTime()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateTransactionViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    this.Current.Services.TransactionService.CreateTransaction(
                        new core.models.Transaction()
                        {
                            Amount = model.Amount,
                            CategoryId = model.CategoryId,
                            CreateUserId = this.Current.User.Id,
                            CreationDateUtc = DateTime.UtcNow,
                            Currency = model.Currency,
                            Description = model.Description,
                            DestinationBankAccountId = model.DestinationBankAccountId,
                            GroupId = this.Current.User.CurrentGroupId.Value,
                            IsBooked = model.IsBooked,
                            SourceBankAccountId = model.SourceBankAccountId,
                            Title = model.Title,
                            ValueDateUtc = model.ValueDate.ToUniversalTime()
                        });

                    return this.JsonSuccess(
                        data: null,
                        message: string.Format(ValidationResources.Transaction_Create_Success, model.Title),
                        returnUrl: Routes.IndexTransactions.GetActionLink(Url));
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