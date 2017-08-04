using System.Web.Optimization;

namespace TCC.CL.Web
{
    public static class Foundation
    {
        public static Bundle Styles()
        {
            return new StyleBundle("~/Content/foundation/css").Include(
                    "~/Content/themes/base/all.css",
                    "~/Content/themes/base/theme.cs",
                    "~/Content/foundation/foundation.css",
                    "~/Content/foundation/app.css");
        }

        public static Bundle Scripts()
        {
            return new ScriptBundle("~/bundles/foundation").Include(
                //"~/Scripts/foundation/vendor/jquery.min.js",
                      "~/Scripts/foundation/vendor/what-input.min.js",
                      "~/Scripts/foundation/foundation.js",
                      "~/Scripts/foundation/typeahead.bundle.js");
        }
    }
}