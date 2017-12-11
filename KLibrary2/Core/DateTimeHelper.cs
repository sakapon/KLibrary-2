using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Keiho
{
    public static class DateTimeHelper
    {
        public static string ToGmtString(this DateTime dateTime)
        {
            return dateTime.ToString(@"ddd, dd MMM yyyy HH:mm:ss G\MT", CultureInfo.InvariantCulture);
        }
    }
}
