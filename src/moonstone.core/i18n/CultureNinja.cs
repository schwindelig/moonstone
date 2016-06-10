using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace moonstone.core.i18n
{
    public class CultureNinja
    {
        public void SetCulture(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
            {
                throw new ArgumentException(nameof(culture));
            }

            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentUICulture = Thread.CurrentThread.CurrentCulture;
        }
    }
}