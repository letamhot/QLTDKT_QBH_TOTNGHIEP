using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class DanhMucSangKienController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();

        // GET: DanhMucSangKien
        public ActionResult Index()
        {
            return View();
        }

       
        public JsonResult GetListSK()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_sangkien.ToList() }, JsonRequestBehavior.AllowGet);
        }
       
        [HttpPost]
        public string UpdateSK(qltdkt_dm_sangkien _objKH)
        {
            try
            {
                if (_objKH.id == 0)
                {
                    qltdkt_dm_sangkien _new = new qltdkt_dm_sangkien();
                    _new.tenSangKien = _objKH.tenSangKien;
                    _new.noiDungSangKien = _objKH.noiDungSangKien;
                    _new.ngayTao = DateTime.Now;
                    _new.ngayCapNhat = null;
                    _entities.qltdkt_dm_sangkien.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_sangkien _update = _entities.qltdkt_dm_sangkien.Find(_objKH.id);

                    _update.tenSangKien = _objKH.tenSangKien;
                    _update.noiDungSangKien = _objKH.noiDungSangKien;
                    _update.ngayCapNhat = DateTime.Now;
                    _entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public JsonResult GetSKById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int ID = int.Parse(Request.QueryString["ID"]);
            return Json(_entities.qltdkt_dm_sangkien.Find(ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteByID()
        {
            try
            {
                var id = Session["id"];

                int idsangkien = int.Parse(Request["ID"]);
                qltdkt_dm_sangkien _old = _entities.qltdkt_dm_sangkien.Find(idsangkien);
                if (_old != null)
                {
                    _entities.qltdkt_dm_sangkien.Remove(_old);

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
                string idsangkienuarr = Request["ID"];
                string[] idsangkien = idsangkienuarr.Split(' ');
                for (int i = 0; i < idsangkien.Length; i++)
                {
                    qltdkt_dm_sangkien _old = _entities.qltdkt_dm_sangkien.Find(int.Parse(idsangkien[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_sangkien.Remove(_old);
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