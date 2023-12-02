using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class DonViPhatDongController : Controller
    {
        // GET: DonViPhatDong
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetListDVPD()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_donviphatdong.ToList() }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string UpdateDVPD(qltdkt_dm_donviphatdong _objKH)
        {
            try
            {
                if (_objKH.id == 0)
                {
                    qltdkt_dm_donviphatdong _new = new qltdkt_dm_donviphatdong();
                    _new.tenPhatDong = _objKH.tenPhatDong;
                    _new.moTa = _objKH.moTa;
                    _entities.qltdkt_dm_donviphatdong.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_donviphatdong _update = _entities.qltdkt_dm_donviphatdong.Find(_objKH.id);
                    _update.tenPhatDong = _objKH.tenPhatDong;
                    _update.moTa = _objKH.moTa;
                    _entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public bool ExistTenDVPhatDong()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_donviphatdong.Where(x => x.tenPhatDong == label.ToLower() && x.tenPhatDong == label.ToUpper()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        public JsonResult GetCVById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int ID = int.Parse(Request.QueryString["id"]);

            return Json(_entities.qltdkt_dm_donviphatdong.Find(ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteByID()
        {
            try
            {
                var id = Session["id"];

                int iddonvi = int.Parse(Request["ID"]);
                qltdkt_dm_donviphatdong _old = _entities.qltdkt_dm_donviphatdong.Find(iddonvi);
                if (_old != null)
                {
                    _entities.qltdkt_dm_donviphatdong.Remove(_old);

                }
                var td = _entities.qltdkt_thidua.Where(x => x.phatDongId == iddonvi && x.daXoa == false).ToList();
                if (td != null && td.Count > 0)
                {
                    for (int i = 0; i < td.Count; i++)
                    {
                        if (td[i].phatDongId == iddonvi)
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
                    qltdkt_dm_donviphatdong _old = _entities.qltdkt_dm_donviphatdong.Find(int.Parse(iddonvi[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_donviphatdong.Remove(_old);
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