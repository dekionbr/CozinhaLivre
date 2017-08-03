using System.Web;
using System.Web.Optimization;

namespace TCC.CL.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            #region Foundation Bundles

            bundles.Add(Foundation.Styles());

            bundles.Add(Foundation.Scripts());

            #endregion

            bundles.Add(new StyleBundle("~/Content/font/css").Include(
                                        "~/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/bundles/inputmask").Include(
                        "~/Scripts/jquery.inputmask/inputmask.js",
                        "~/Scripts/jquery.inputmask/jquery.inputmask.js",
                        "~/Scripts/jquery.inputmask/inputmask.extensions.js",
                        "~/Scripts/jquery.inputmask/inputmask.date.extensions.js",
                //and other extensions you want to include
                        "~/Scripts/jquery.inputmask/inputmask.numeric.extensions.js"));

            bundles.Add(new ScriptBundle("~/bundles/jQueryMultFiles").Include(
                "~/Scripts/jquery.MultiFile.js"));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                "~/Scripts/foundation/app.js"));

            bundles.Add(new ScriptBundle("~/bundles/tinymce").Include(
                "~/Scripts/tinymce/tinymce.js"));

            //#region Telerik
            
            //bundles.Add(new StyleBundle("~/Content/telerik/css").Include(
            //                            "~/Content/Telerik/kendo.common.min.css",
            //                            "~/Content/Telerik/kendo.rtl.min.css",
            //                            "~/Content/Telerik/kendo.default.min.css",
            //                            "~/Content/Telerik/kendo.default.mobile.min.css"
            //                            ));

            //bundles.Add(new ScriptBundle("~/bundles/telerik/scripts").Include(
            //    "~/Scripts/Telerik/jszip.min.js",
            //    "~/Scripts/Telerik/kendo.all.min.js"
            //    ));

            //#endregion

            //<link href="../../styles/kendo.common.min.css" rel="stylesheet">
            //<link href="../../styles/kendo.rtl.min.css" rel="stylesheet">
            //<link href="../../styles/kendo.default.min.css" rel="stylesheet">
            //<link href="../../styles/kendo.default.mobile.min.css" rel="stylesheet">
            //<script src="../../js/jquery.min.js"></script>
            //<script src="../../js/jszip.min.js"></script>
            //<script src="../../js/kendo.all.min.js"></script>

            System.Web.Optimization.BundleTable.EnableOptimizations = false;
        }
    }
}