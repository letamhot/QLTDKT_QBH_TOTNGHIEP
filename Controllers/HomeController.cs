using Newtonsoft.Json;
using QLTDKT.Models;
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
        public ActionResult Index()
        {
            return View();
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