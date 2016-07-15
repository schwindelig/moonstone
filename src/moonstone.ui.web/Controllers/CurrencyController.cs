using moonstone.ui.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace moonstone.ui.web.Controllers
{
    public class CurrencyController : BaseController
    {
        public CurrencyController(Current current)
            : base(current)
        {
        }

        public ActionResult GetAllCurrencies()
        {
            try
            {
                var res = this.Current.Services.CurrencyService
                    .GetAllCurrencies()
                    .Select(c =>
                        new
                        {
                            name = $"{c.LocalizedName} ({c.Code})",
                            //description = c.Symbol,
                            value = c.Code
                        });

                return this.JsonSuccess(data: res, message: null);
            }
            catch (Exception e)
            {
                // TODO: Handle exception and return JsonError
                throw;
            }
        }

        // GET: Currency
        public ActionResult Index()
        {
            return View();
        }
    }
}