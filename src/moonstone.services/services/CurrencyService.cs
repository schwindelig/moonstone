using moonstone.core.models;
using moonstone.core.repositories;
using moonstone.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class CurrencyService : BaseService, ICurrencyService
    {
        public CurrencyService(RepositoryHub repoHub) : base(repoHub)
        {
        }

        public IEnumerable<Currency> GetAllCurrencies()
        {
            var nodaCurrencies = NodaMoney.Currency.GetAllCurrencies().Where(c => !c.IsObsolete);
            var currencies = nodaCurrencies.Select(c =>
                new Currency
                {
                    Code = c.Code,
                    LocalizedName = c.EnglishName, // TODO: Get localized name
                    Symbol = c.Symbol
                })
                .OrderBy(c => c.Code);

            return currencies;
        }
    }
}