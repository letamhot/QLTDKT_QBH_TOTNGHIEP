using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class TrinhDoHocVanController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();

        // GET: TrinhDoHocVan
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetListTDHV()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_trinhdohocvan.ToList() }, JsonRequestBehavior.AllowGet);
        }
        
        [HttpPost]
        public string UpdateTDHV(qltdkt_dm_trinhdohocvan _objKH)
        {
            try
            {
                if (_objKH.id == 0)
                {
                    qltdkt_dm_trinhdohocvan _new = new qltdkt_dm_trinhdohocvan();
                    _new.tenTrinhDoHocVan = _objKH.tenTrinhDoHocVan;
                    _new.chuyenNganh = _objKH.chuyenNganh;
                    _entities.qltdkt_dm_trinhdohocvan.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_trinhdohocvan _update = _entities.qltdkt_dm_trinhdohocvan.Find(_objKH.id);

                    _update.tenTrinhDoHocVan = _objKH.tenTrinhDoHocVan;
                    _update.chuyenNganh = _objKH.chuyenNganh;
                    _entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public JsonResult GetTDHVById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int ID = int.Parse(Request.QueryString["ID"]);
            return Json(_entities.qltdkt_dm_trinhdohocvan.Find(ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteByID()
        {
            try
            {
                var id = Session["id"];

                int idTD = int.Parse(Request["ID"]);
                qltdkt_dm_trinhdohocvan _old = _entities.qltdkt_dm_trinhdohocvan.Find(idTD);
                if (_old != null)
                {
                    _entities.qltdkt_dm_trinhdohocvan.Remove(_old);

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
                string idTDuarr = Request["ID"];
                string[] idTD = idTDuarr.Split(' ');
                for (int i = 0; i < idTD.Length; i++)
                {
                    qltdkt_dm_trinhdohocvan _old = _entities.qltdkt_dm_trinhdohocvan.Find(int.Parse(idTD[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_trinhdohocvan.Remove(_old);
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