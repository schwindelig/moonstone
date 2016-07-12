using System.Web.Optimization;

namespace moonstone.ui.web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            // global js
            bundles.Add(new ScriptBundle("~/bundles/global/js").Include(
                "~/bower_components/jquery/dist/jquery.js",
                "~/bower_components/jquery-serialize-object/dist/jquery.serialize-object.min.js",
                "~/bower_components/toastr/toastr.min.js",
                "~/js/site.js"));

            // jquery validate with unobtrusive
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/bower_components/jquery-validation/dist/jquery.validate.min.js",
                        "~/bower_components/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"));

            // global css
            bundles.Add(new StyleBundle("~/bundles/global/css").Include(
                "~/bower_components/toastr/toastr.min.css",
                "~/css/site.css"));
        }
    }
}