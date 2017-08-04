using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Common
{
    public static class DateTimeExtend
    {
        public static bool IsDefault(this DateTime date)
        {
            return date == default(DateTime);
        }
    }
}
