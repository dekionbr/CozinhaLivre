using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCC.CL.Core.Common
{
    public static class EnumerableExtend
    {
        public static IList<String> ConvertAllToString(this IList<int> enumerable)
        {
            IList<String> converted = new List<String>();

            foreach (var item in enumerable)
            {
                converted.Add(item.ToString());
            }

            return converted;
        }
    }
}
