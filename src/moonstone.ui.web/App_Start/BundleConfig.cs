using System.Web.Optimization;

namespace moonstone.ui.web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            BundleTable.EnableOptimizations = true;

            // global
            bundles.Add(new StyleBundle("~/bundles/global/css").Include(
                "~/semantic/dist/semantic.css"));

            bundles.Add(new ScriptBundle("~/bundles/global/js").Include(
                "~/bower_components/jquery/dist/jquery.js",
                "~/semantic/dist/semantic.js"));
        }
    }
}