using System.Web;
using System.Web.Optimization;

namespace Shell
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                .Include("~/content/js/jquery/jquery-{version}.js",
                    "~/content/js/idle/jquery.idle.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval")
                .Include("~/Content/js/jquery/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/angular")
               .Include("~/content/js/angular/angular.min.js",
                    "~/content/js/angular/angular-cookies.min.js",
                    "~/content/js/angular/angular-route.min.js",
                    "~/content/js/angular/angular-sanitize.js",
                    "~/content/js/angular/angular-input-match/angular-input-match.js",
                    "~/content/js/angular/angular-loading-bar/loading-bar.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr")
                .Include("~/Content/js/modernizr/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                .Include("~/Content/js/bootstrap/bootstrap.js",
                    "~/Content/js/respond/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/app")
                .Include("~/content/js/core.js",
                    "~/content/js/app.js",
                    "~/content/js/controllers/*.js",
                    "~/content/js/entities/*.js",
                    "~/content/js/services/*.js"));

            bundles.Add(new StyleBundle("~/Content/css")
                .Include("~/Content/css/bootstrap/bootstrap.css",
                    "~/content/css/fontawesome/font-awesome.css",
                    "~/Content/css/app.css"));
        }
    }
}
