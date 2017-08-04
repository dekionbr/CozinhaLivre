using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Helper
{
    public static class DataHelper
    {
        public static string ToDataPost(this DateTime date)
        {
            return date.ToString("dd/MM/yyyy HH:mm");
        }
    }
}
