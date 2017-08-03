using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TCC.CL.Core.Helper
{
    public static class StringHelper
    {
        /// <summary>
        /// Compiled regular expression for performance.
        /// </summary>
        static Regex _htmlRegex = new Regex(@"<[^>]*>", RegexOptions.Compiled);
        static Regex _htmlRegexImage = new Regex(@"/<img[^>]+\>/i", RegexOptions.Compiled);

        public static string RemoveHtml(this string html)
        {
            return _htmlRegexImage.Replace(_htmlRegex.Replace(html, string.Empty), string.Empty);
        }
    }
}
