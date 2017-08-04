using System.Web;
using System.Web.Optimization;

namespace BAPA_LMS
{
	public class BundleConfig
	{
		// For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Scripts/jquery-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
						"~/Scripts/jquery.validate*"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Scripts/modernizr-*"));

			bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
					  "~/Scripts/bootstrap.js",
					  "~/Scripts/respond.js"));

			bundles.Add(new StyleBundle("~/Content/css").Include(
					  "~/Content/bootstrap.css",
					  "~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/lms").Include(
						"~/Scripts/modernizr-*",
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/jquery.unobtrusive-ajax.js",
						"~/Scripts/jquery.validate*",
						"~/Scripts/bootstrap.js",
						"~/Scripts/respond.js",
						"~/Scripts/bootstrap-treeview.js",
						"~/Scripts/bootstrap-datepicker.js",
						"~/Scripts/bootstrap-datepicker.sv.min.js",
						"~/Scripts/jquery.timepicker.js",
						"~/Scripts/teacher.js"
                        ));
			
			bundles.Add(new StyleBundle("~/Content/lms").Include(
						"~/Content/bootstrap.css",
						"~/Content/bootstrap-treeview.css",
						"~/Content/bootstrap-datepicker3.css",
						"~/Content/jquery.timepicker.css",
						"~/Content/site.css"));

			bundles.Add(new ScriptBundle("~/bundles/lms-calendar").Include(
						"~/Scripts/modernizr-*",
						"~/Scripts/jquery-{version}.js",
						"~/Scripts/bootstrap.js",
						"~/Scripts/respond.js",
						"~/Scripts/moment.min.js",
						"~/Scripts/fullcalendar/fullcalendar.min.js",
						"~/Scripts/fullcalendar/locale/sv.js",
						"~/Scripts/student.js"));

			bundles.Add(new StyleBundle("~/Content/lms-calendar").Include(
						"~/Content/bootstrap.css",
						"~/Content/fullcalendar.css",
						"~/Content/site.css"));
		}
	}
}
