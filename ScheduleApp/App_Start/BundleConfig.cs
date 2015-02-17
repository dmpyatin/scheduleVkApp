using System.Web;
using System.Web.Optimization;

namespace ScheduleApp
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/angularjs").Include(
                      "~/Scripts/angular.js",
                      "~/Scripts/angular-route.js",
                      "~/Scripts/angular-cookies.js",
                      "~/Scripts/angular-dragdrop.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/select2.css",
                      "~/Content/select2-bootstrap.css"));

            bundles.Add(new ScriptBundle("~/bundles/angularui").Include(
                      "~/Scripts/angular-ui/ui-bootstrap.js",
                      "~/Scripts/angular-ui/ui-bootstrap-tpls.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/chart").Include(
                        "~/Scripts/Chart.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/sigma").Include(
                        "~/Scripts/sigma.min.js",
                        "~/Scripts/sigma.parsers.json.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/scheduleapp").Include(
                      "~/App/app.js",
                      "~/App/controllers/scheduleController.js",
                      "~/App/controllers/reportController.js",
                      "~/App/controllers/planningController.js",
                      "~/App/customscripts/staticCharts.js"));


            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            BundleTable.EnableOptimizations = true;
        }
    }
}
