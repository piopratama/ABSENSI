using System.Web;
using System.Web.Optimization;

namespace ABSENSI
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.UseCdn = true;
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new Bundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.css",
                      "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/bundles/sweetAlertCdn", "https://cdn.jsdelivr.net/npm/sweetalert2@11")
                                   .Include("~/Scripts/sweetAlert.js"));

            bundles.Add(new ScriptBundle("~/bundles/fullCalendar", "https://cdn.jsdelivr.net/npm/fullcalendar@5.11.0/main.min.js")
                                    .Include("~/Scripts/fullCalendar.js"));

            bundles.Add(new StyleBundle("~/Content/fullCalendar", "https://cdn.jsdelivr.net/npm/fullcalendar@5.11.0/main.min.css")
                                    .Include("~/Content/fullCalendar.css"));

            bundles.Add(new ScriptBundle("~/bundles/moment", "https://cdn.jsdelivr.net/momentjs/latest/moment.min.js")
                                    .Include("~/Scripts/moment.js"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
