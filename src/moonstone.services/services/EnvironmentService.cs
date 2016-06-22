using moonstone.core.exceptions;
using moonstone.core.exceptions.serviceExceptions;
using moonstone.core.i18n;
using moonstone.core.repositories;
using moonstone.core.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.services
{
    public class EnvironmentService : BaseService, IEnvironmentService
    {
        protected CultureNinja CultureNinja { get; set; }

        public EnvironmentService(RepositoryHub repoHub, CultureNinja cultureNinja)
            : base(repoHub)
        {
            this.CultureNinja = cultureNinja;
        }

        public void SetCulture(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                throw new ArgumentException(nameof(culture));
            }

            try
            {
                this.CultureNinja.SetCulture(culture);
            }
            catch (Exception e)
            {
                throw new SetCultureException(
                    $"Failed to set culture to '{culture}'.", e);
            }
        }
    }
}