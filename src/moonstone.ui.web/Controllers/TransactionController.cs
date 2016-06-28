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

                var res = Routes.Home;
                return this.RedirectToAction(res.Action, res.Controller);
            }

            return View(model);
        }
    }
}