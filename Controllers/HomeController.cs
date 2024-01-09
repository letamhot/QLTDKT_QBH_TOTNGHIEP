using Newtonsoft.Json;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using QLTDKT.Models;
using QLTDKT.Models.Service.baoCaoThongKeService;
using QLTDKT.Models.Service.roleService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class HomeController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private roleService _roleService = new roleService();
        public baoCaoThongKeService _bcService = new baoCaoThongKeService();

        public ActionResult Index()
        {
            
            return View();
        }
        public JsonResult ThongKeDanhHieu()
        {
            var danhHieu = _entities.qltdkt_danhhieu.ToList().Count();
            return Json(danhHieu, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ThongKeKhenThuong()
        {
            var khenThuong = _entities.qltdkt_khenthuong.ToList().Count();

            return Json(khenThuong, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ThongKeThiDua()
        {
            var thiDua = _entities.qltdkt_thidua.ToList().Count();

            return Json(thiDua, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ThongKeNhanVien()
        {
            var nhanVien = _entities.qltdkt_dm_nhanvien.ToList().Count();

            return Json(nhanVien, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getThongKeBaoCaoTT()
        {
            int kieuDanhHieu = int.Parse(Request.QueryString["kieuDanhHieu"]);
            int namDanhHieu = int.Parse(Request.QueryString["namDanhHieu"]);

            return Json(new { data = _bcService.getThongKeTrangChuTT(kieuDanhHieu, namDanhHieu) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult getThongKeBaoCaoCN()
        {
            int kieuDanhHieu = int.Parse(Request.QueryString["kieuDanhHieu"]);
            int namDanhHieu = int.Parse(Request.QueryString["namDanhHieu"]);

            return Json(new { data = _bcService.getThongKeTrangChuCN(kieuDanhHieu, namDanhHieu) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult SideBar(int userId)
        {
            ViewBag.Menu = _roleService.getListRoleParent(userId);
            return PartialView("SideBar");
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            var loginInfo = _entities.qltdkt_user.FirstOrDefault(x => x.tenDangNhap == email && x.matKhau == password);
            if (loginInfo != null)
            {
                byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
                byte[] key = Guid.NewGuid().ToByteArray();
                string token = Convert.ToBase64String(time.Concat(key).ToArray());
                int idNhanVien = 0;
                try
                {
                    idNhanVien = (int)loginInfo.nhanVienId;
                }
                catch (Exception)
                {

                }
                if (idNhanVien > 0)
                {
                    qltdkt_dm_nhanvien _nv = _entities.qltdkt_dm_nhanvien.Find(idNhanVien);
                    Session["tenNhanVien"] = _nv.hoTen;
                    Session["email"] = _nv.email;
                    Session["anhDaiDien"] = _nv.anhDaiDien;



                }

                Session["token"] = token;
                Session["userId"] = loginInfo.id;
                return RedirectToAction("Index");

            }
            ViewBag.error = "<div class=\"alert alert-warning fade show\"><span class=\"close\" data-dismiss=\"alert\">&times;</span><strong>Đăng nhập không thành công!!</strong></div>";
            return View();
        }

        public ActionResult Logout()
        {
            Session["token"] = null;
            Session["userId"] = null;
            Session["tenNhanVien"] = null;
            return View();
        }
    }
}