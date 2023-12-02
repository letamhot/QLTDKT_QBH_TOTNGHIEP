using QLTDKT.Models.Service.soKyYeuService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class SoKyYeuController : Controller
    {
        private soKyYeuService _service = new soKyYeuService();
        // GET: SoKyYeu
        public ActionResult Index()
        {
            return View();
        }

        public bool LuuSoKyYeu()
        {
            int idThiDua = int.Parse(Request["idThiDua"]);
            return _service.luuHoSoKyYeu(idThiDua);
        }
        public JsonResult getSoKyYeu()
        {
            return Json(new { data = _service.getSoKyYeu() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChiTietKyYeu(string id)
        {
            return Json(new { data = _service.ChiTietKyYeu(id) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Reload()
        {
            return Json(new { data = _service.Reload() }, JsonRequestBehavior.AllowGet);
        }

        //export file chi tiết sổ kỷ yếu
        [HttpPost]
        [ValidateInput(false)]
        public EmptyResult ExportChiTietSokyYeu(string GridHtml)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            string FileName = "ChiTietSoKyYeu" + DateTime.Now.ToString("dd/MM/yyyy") + ".doc";
            Response.AddHeader("content-disposition",
                    "attachment;filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/msword ";
            Response.Output.Write(GridHtml);
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
    }
}