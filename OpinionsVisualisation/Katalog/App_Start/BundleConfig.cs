using System.Web;
using System.Web.Optimization;

namespace Katalog
{
    public class BundleConfig
    {
        // Aby uzyskać więcej informacji na temat tworzenia pakietów (Bundling), odwiedź stronę http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // Użyj wersji deweloperskiej biblioteki Modernizr do nauki i opracowywania rozwiązań. Następnie, kiedy wszystko będzie
            // gotowe do produkcji, wybierz tylko potrzebne testy za pomocą narzędzia kompilacji z witryny http://modernizr.com.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/Simplestyle Banner/style.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/floaten-simple/style.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/modern bussines/layout/styles/layout.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/modern bussines/layout/styles/navi.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/modern bussines/layout/styles/tables.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/modern bussines/layout/styles/forms.css"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/modern bussines/layout/styles/featured_slide.css"));
            //bundles.Add(new StyleBundle("~/Content/js").Include("~/Content/themes/modern bussines/layout/scripts/jquery.min.js"));
            //bundles.Add(new StyleBundle("~/Content/js").Include("~/Content/themes/modern bussines/layout/scripts/jquery.nivo.pack.js"));
            //bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/themes/fixed-light-green/style.css"));

        }
    }
}