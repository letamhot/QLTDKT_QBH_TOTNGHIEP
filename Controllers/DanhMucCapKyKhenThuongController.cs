using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class DanhMucCapKyKhenThuongController : Controller
    {
        // GET: DanhMucCapKhenThuong
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetDanhMucCK()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_capkykhenthuong.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public bool ExistTenCapKy()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_capkykhenthuong.Where(x => x.tenCapKyKhenThuong == label.ToUpper() && x.tenCapKyKhenThuong == label.ToLower()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public string UpdateCK(qltdkt_dm_capkykhenthuong _objCK)
        {
            try
            {
                if (_objCK.id == 0)
                {
                    qltdkt_dm_capkykhenthuong _new = new qltdkt_dm_capkykhenthuong();
                    _new.tenCapKyKhenThuong = _objCK.tenCapKyKhenThuong;
                    _new.moTa = _objCK.moTa;
                    _new.ngayTao = DateTime.Now;
                    _new.daXoa = false;
                    _entities.qltdkt_dm_capkykhenthuong.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_capkykhenthuong _update = _entities.qltdkt_dm_capkykhenthuong.Find(_objCK.id);

                    _update.tenCapKyKhenThuong = _objCK.tenCapKyKhenThuong;
                    _update.moTa = _objCK.moTa;

                    _entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public JsonResult GetById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int ID = int.Parse(Request.QueryString["ID"]);
            return Json(_entities.qltdkt_dm_capkykhenthuong.Find(ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteById()
        {
            try
            {
                var id = Session["id"];

                int idCapKy = int.Parse(Request["ID"]);
                qltdkt_dm_capkykhenthuong _old = _entities.qltdkt_dm_capkykhenthuong.Find(idCapKy);
                if (_old != null)
                {
                    _entities.qltdkt_dm_capkykhenthuong.Remove(_old);

                }

                var ckkt = _entities.qltdkt_khenthuong.Where(x => x.capKhenThuong == (byte)idCapKy && x.daXoa == false).ToList();
                if (ckkt != null && ckkt.Count > 0)
                {
                    for (int i = 0; i < ckkt.Count; i++)
                    {
                        if (ckkt[i].capKhenThuong == (byte)idCapKy)
                        {
                            return "warning";
                        }
                    }
                }
                var dmdh = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.capThanhTich == idCapKy && x.daXoa == false).ToList();
                if (dmdh != null && dmdh.Count > 0)
                {
                    for (int i = 0; i < dmdh.Count; i++)
                    {
                        if (dmdh[i].capThanhTich == idCapKy)
                        {
                            return "warning1";
                        }
                    }
                }
                _entities.SaveChanges();

                return "success";
            }
            catch (Exception)
            {
                return "error";
                throw;
            }
        }
        [HttpPost]
        public bool DeleteByArrayId()
        {
            try
            {

                var id = Session["id"];
                string iddonviuarr = Request["ID"];
                string[] iddonvi = iddonviuarr.Split(' ');
                for (int i = 0; i < iddonvi.Length; i++)
                {
                    qltdkt_dm_capkykhenthuong _old = _entities.qltdkt_dm_capkykhenthuong.Find(int.Parse(iddonvi[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_capkykhenthuong.Remove(_old);
                        _entities.SaveChanges();

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}