using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class DanhMucChuKyController : Controller
    {
        // GET: DanhMucChuKy


        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public ActionResult Index()
        {
            return View();
        }


        public JsonResult GetDanhMucCK()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_chuky.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public bool ExistTenChuKy()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_chuky.Where(x => x.tenChuKy == label.ToLower() && x.tenChuKy == label.ToUpper()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public string UpdateChuKy(qltdkt_dm_chuky _objCK)
        {
            try
            {
                if (_objCK.id == 0)
                {
                    qltdkt_dm_chuky _new = new qltdkt_dm_chuky();
                    _new.tenChuKy = _objCK.tenChuKy;
                    _new.ngayTao = DateTime.Now;
                    _new.daXoa = false;
                    _entities.qltdkt_dm_chuky.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_chuky _update = _entities.qltdkt_dm_chuky.Find(_objCK.id);

                    _update.tenChuKy = _objCK.tenChuKy;
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
            return Json(_entities.qltdkt_dm_chuky.Find(ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteById()
        {
            try
            {
                var id = Session["id"];

                int idChuKy = int.Parse(Request["ID"]);
                qltdkt_dm_chuky _old = _entities.qltdkt_dm_chuky.Find(idChuKy);
                if (_old != null)
                {
                    _entities.qltdkt_dm_chuky.Remove(_old);

                }
                var dh = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.chuKy == idChuKy && x.daXoa == false).ToList();
                if (dh != null && dh.Count > 0)
                {
                    for (int i = 0; i < dh.Count; i++)
                    {
                        if (dh[i].chuKy == idChuKy)
                        {
                            return "warning";
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
                    qltdkt_dm_chuky _old = _entities.qltdkt_dm_chuky.Find(int.Parse(iddonvi[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_chuky.Remove(_old);
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