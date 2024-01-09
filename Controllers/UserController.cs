using QLTDKT.Models;
using QLTDKT.Models.Service.userService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace QLTDKT.Controllers
{
    public class UserController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private userService _service = new userService();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNhomNguoiDung()
        {
            return Json(new { data = _entities.qltdkt_groupuser.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDsNhanVien()
        {
            return Json(new { data = _entities.qltdkt_dm_nhanvien.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public bool CapNhatLienKet()
        {
            int idNguoiDung = int.Parse(Request["idLienKet"]);
            int idNhanVien = int.Parse(Request["idNhanVien"]);

            var _objNguoiDung = _entities.qltdkt_user.Find(idNguoiDung);
            if (_objNguoiDung != null)
            {
                _objNguoiDung.nhanVienId = idNhanVien;
                _entities.SaveChanges();
                return true;
            }
            return false;
        }
        [HttpPost]
        public string UpdateUser(userFull _objUser)
        {
            try
            {
                if (_objUser.id == 0)
                {

                    qltdkt_user _new = new qltdkt_user
                    {
                        tenDangNhap = _objUser.tenDangNhap,
                        matKhau = _objUser.matKhau,
                        ngayTao = DateTime.Now,
                        daXoa = false
                    };
                    _entities.qltdkt_user.Add(_new);
                    _entities.SaveChanges();
                    qltdkt_userbygroup _role = new qltdkt_userbygroup
                    {
                        userId = _new.id,
                        groupUserId = _objUser.groupUserId
                    };
                    _entities.qltdkt_userbygroup.Add(_role);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {
                    qltdkt_user _update = _entities.qltdkt_user.Find(_objUser.id);
                    if (_update != null)
                    {
                        _update.tenDangNhap = _objUser.tenDangNhap;
                        _update.matKhau = _objUser.matKhau;
                        _update.ngayCapNhat = DateTime.Now;
                    }
                    qltdkt_userbygroup _usUd = _entities.qltdkt_userbygroup.FirstOrDefault(x => x.userId == _objUser.id);
                    if (_usUd != null)
                    {
                        _usUd.groupUserId = _objUser.groupUserId;
                    }
                    _entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        [HttpPost]
        public bool DeleteByID()
        {
            try
            {
                int idiNguoiDung = int.Parse(Request["id"]);
                qltdkt_user _old = _entities.qltdkt_user.Find(idiNguoiDung);
                if (_old != null)
                {
                    _old.daXoa = true;
                    _entities.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public JsonResult GetDataNguoiDungById()
        {
            int idNguoiDung = int.Parse(Request.QueryString["id"]);
            return Json(_service.getUserInfo(idNguoiDung), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataUser()
        {
            return Json(new { data = _service.getUserData() }, JsonRequestBehavior.AllowGet);
        }
    }
}