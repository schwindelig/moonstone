﻿using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace moonstone.core.i18n
{
    public class TimeZoneUtils
    {
        public static IEnumerable<string> GetAvailableDateFormat()
        {
            return new List<string>()
            {
                "YYYY-MM-DD",
                "DD.MM.YYYY"
            };
        }

        public static IEnumerable<string> GetAvailableTimeZones()
        {
            return DateTimeZoneProviders.Tzdb.Ids;
        }
    }
}