using System.Web;
using System.Web.Optimization;

namespace ManagementTool
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {

            // Vendor scripts
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-3.1.1.min.js"));

            // jQuery Validation
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
            "~/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.min.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/bundles/inspinia").Include(
                      "~/Scripts/app/inspinia.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/plugins/slimScroll").Include(
                      "~/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            // jQuery plugins
            bundles.Add(new ScriptBundle("~/plugins/metsiMenu").Include(
                      "~/Scripts/plugins/metisMenu/metisMenu.min.js"));

            // pace
            bundles.Add(new ScriptBundle("~/plugins/pace").Include(
                      "~/Scripts/plugins/pace/pace.min.js"));

            // Jasny
            bundles.Add(new ScriptBundle("~/plugins/jasny").Include(
                      "~/Scripts/plugins/jasny/jasny-bootstrap.min.js"));


            // Typehead for *Auto Complete
            bundles.Add(new ScriptBundle("~/plugins/typehead").Include(
                      "~/Scripts/plugins/typehead/bootstrap3-typeahead.min.js"));

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/animate.css",
                      "~/Content/style.css",
                      "~/Content/css/plugins/jQueryUI/jquery-ui.css",
                      "~/Content/css/plugins/jQueryUI/jquery-ui-1.10.4.custom.min.css",                      
                      "~/Content/css/plugins/datepicker/datepicker3.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

        }
    }
}
