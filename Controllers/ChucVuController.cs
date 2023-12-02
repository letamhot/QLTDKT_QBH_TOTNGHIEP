using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class ChucVuController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();

        // GET: ChucVu
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetListCV()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_chucvu.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public bool ExistTenChucVu()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_chucvu.Where(x => x.tenChucVu == label.ToUpper() && x.tenChucVu == label.ToLower()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public string UpdateCV(qltdkt_dm_chucvu _objKH)
        {
            try
            {
                if (_objKH.id == 0)
                {
                    qltdkt_dm_chucvu _new = new qltdkt_dm_chucvu();
                    _new.tenChucVu = _objKH.tenChucVu;
                    _new.moTa = _objKH.moTa;
                    _new.ngayTao = DateTime.Now;
                    _new.ngayCapNhat = null;
                    _entities.qltdkt_dm_chucvu.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_chucvu _update = _entities.qltdkt_dm_chucvu.Find(_objKH.id);

                    _update.tenChucVu = _objKH.tenChucVu;
                    _update.moTa = _objKH.moTa;
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
        public JsonResult GetCVById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int ID = int.Parse(Request.QueryString["ID"]);
            return Json(_entities.qltdkt_dm_chucvu.Find(ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteByID()
        {
            try
            {
                var id = Session["id"];

                int idChucvu = int.Parse(Request["ID"]);
                qltdkt_dm_chucvu _old = _entities.qltdkt_dm_chucvu.Find(idChucvu);
                if (_old != null)
                {
                    _entities.qltdkt_dm_chucvu.Remove(_old);

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
                    qltdkt_dm_chucvu _old = _entities.qltdkt_dm_chucvu.Find(int.Parse(iddonvi[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_chucvu.Remove(_old);
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