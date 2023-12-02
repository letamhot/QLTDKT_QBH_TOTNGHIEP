using System.Web;
using System.Web.Optimization;

namespace QLTDKT
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
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
            bundles.Add(new StyleBundle("~/assets/css/default").Include(
                "~/assets/css/app.min.css",
                "~/assets/css/bootstrap.min.css",
                "~/assets/css/bootstrap_3.1.0.min.css",
                "~/assets/css/icon.min.css",
                "~/assets/css/font-awesome/css/all.min.css",
                "~/assets/css/mdicons.min.css",
                "~/assets/libs/custombox/custombox.min.css",
                "~/assets/libs/summernote/summernote-bs4.css",
                "~/assets/libs/jasny-bootstrap/jasny-bootstrap.min.css",
                "~/assets/libs/bootstrap-validator/dist/css/bootstrapValidator.min.css",
                "~/assets/libs/datatables/buttons.bootstrap4.min.css",
                "~/assets/libs/datatables/dataTables.bootstrap4.min.css",
                "~/assets/libs/datatables/fixedHeader.bootstrap4.min.css",
                "~/assets/libs/datatables/responsive.bootstrap4.min.css",
                "~/assets/libs/datatables/scroller.bootstrap4.min.css",
                "~/assets/libs/bootstrap-colorpicker/bootstrap-colorpicker.min.css",
                "~/assets/libs/bootstrap-datepicker/bootstrap-datepicker.css",
                "~/assets/libs/bootstrap-daterangepicker/bootstrap-daterangepicker.css",
                "~/assets/libs/bootstrap-tagsinput/bootstrap-tagsinput.css",
                "~/assets/libs/bootstrap-timepicker/bootstrap-timepicker.min.css",
                "~/assets/libs/bootstrap-touchspin/jquery.bootstrap-touchspin.min.css",
                "~/assets/libs/multiselect/multi-select.css",
                "~/assets/libs/dropify/dropify.min.css",
                "~/assets/libs/select2/select2.min.css",
                "~/assets/libs/jstree/style.min.css",
                "~/assets/libs/switchery/switchery.min.css",
                "~/assets/libs/toastr/toastr.min.css",
                "~/assets/fonts/materialdesignicons-webfont.woff",
                "~/assets/fonts/materialdesignicons-webfont.woff2",
                "~/assets/fonts/materialdesignicons-webfont.ttf"

            ));
            bundles.Add(new ScriptBundle("~/assets/js/default").Include(
                "~/assets/js/jquery.min.js",
                "~/assets/js/bootstrap.min.js",
                "~/assets/libs/jasny-bootstrap/jasny-bootstrap.min.js",
                "~/assets/libs/bootstrap-colorpicker/bootstrap-colorpicker.min.js",
                "~/assets/libs/bootstrap-datepicker/bootstrap-datepicker.min.js",
                "~/assets/libs/bootstrap-daterangepicker/daterangepicker.js",
                "~/assets/libs/bootstrap-maxlength/bootstrap-maxlength.min.js",
                "~/assets/libs/bootstrap-tagsinput/bootstrap-tagsinput.min.js",
                "~/assets/libs/bootstrap-timepicker/bootstrap-timepicker.min.js",
                "~/assets/libs/bootstrap-touchspin/bootstrap-touchspin.min.js",
                "~/assets/libs/summernote/summernote-bs4.min.js",
                "~/assets/libs/datatables/jquery.dataTables.min.js",
                "~/assets/libs/ckeditor/ckeditor.js",
                /*"~/assets/libs/datatables/buttons.html5.min.js",*/
                "~/assets/libs/datatables/buttons.print.min.js",
                "~/assets/libs/datatables/dataTables.bootstrap4.min.js",
                "~/assets/libs/datatables/dataTables.buttons.min.js",
                "~/assets/libs/datatables/dataTables.fixedHeader.min.js",
                "~/assets/libs/datatables/dataTables.keyTable.min.js",
                "~/assets/libs/datatables/dataTables.responsive.min.js",
                "~/assets/libs/datatables/dataTables.scroller.min.js",
                "~/assets/libs/datatables/responsive.bootstrap4.min.js",
                "~/assets/libs/jquery-mask-plugin/jquery-mask-plugin.min.js",
                "~/assets/libs/jquery-quicksearch/jquery-quicksearch.min.js",
                "~/assets/libs/bootstrap-validator/dist/js/bootstrapValidator.js",
                "~/assets/libs/jszip/jszip.min.js",
                "~/assets/libs/moment/moment.min.js",
                "~/assets/libs/multiselect/jquery.multi-select.js",
                "~/assets/libs/dropify/dropify.min.js",
                "~/assets/libs/custombox/custombox.min.js",
                "~/assets/libs/pdfmake/vfs_fonts.js",
                "~/assets/libs/select2/select2.min.js",
                "~/assets/libs/jstree/jstree.min.js",
                "~/assets/libs/switchery/switchery.min.js",
                "~/assets/libs/toastr/toastr.min.js"
            ));
            bundles.Add(new ScriptBundle("~/assets/js/application").Include(
                 "~/assets/js/vendor.min.js",
                "~/assets/libs/morris-js/morris.min.js",
                "~/assets/libs/raphael/raphael.min.js",
                "~/assets/js/app.min.js"
            ));
        }
    }
}
