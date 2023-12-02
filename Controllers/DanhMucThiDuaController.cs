using QLTDKT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class DanhMucThiDuaController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();

        // GET: DanhMucThiDua
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetDanhMucTD()
        {

            return Json(new { data = _entities.qltdkt_dm_thidua.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            int ID = int.Parse(Request.QueryString["id"]);
            return Json(_entities.qltdkt_dm_thidua.Find(ID), JsonRequestBehavior.AllowGet);
        }


        public bool checkExistLabel()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_thidua.Where(x => x.tenThiDua == label.ToUpper() && x.tenThiDua == label.ToLower()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }


        [HttpPost]

        public string CapNhatThiDua(qltdkt_dm_thidua _objTD)
        {


            try
            {
                if (_objTD.id == 0)
                {
                    var chk = _entities.qltdkt_dm_thidua.Where(x => x.tenThiDua == _objTD.tenThiDua).FirstOrDefault();
                    if (chk == null)
                    {
                        qltdkt_dm_thidua _new = new qltdkt_dm_thidua();

                        _new.tenThiDua = _objTD.tenThiDua;
                        _new.moTa = _objTD.moTa;
                        _new.kieuThiDua = _objTD.kieuThiDua;
                        _new.ngayTao = DateTime.Now;
                        _entities.qltdkt_dm_thidua.Add(_new);
                        _entities.SaveChanges();
                        return "addsuccess";
                    }
                    else
                    {
                        return "warning";
                    }



                }
                else
                {

                    qltdkt_dm_thidua _update = _entities.qltdkt_dm_thidua.Find(_objTD.id);

                    _update.tenThiDua = _objTD.tenThiDua;
                    _update.moTa = _objTD.moTa;
                    _update.kieuThiDua = _objTD.kieuThiDua;
                    _entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }


        }

        public string XoaThiDua()
        {
            int id = int.Parse(Request["ID"]);
            try
            {
                qltdkt_dm_thidua _obj = _entities.qltdkt_dm_thidua.Find(id);

                if (_obj != null)
                {
                    _entities.qltdkt_dm_thidua.Remove(_obj);

                }
                var td = _entities.qltdkt_thidua.Where(x => x.idDmThiDua == id && x.daXoa == false).ToList();
                if (td != null && td.Count > 0)
                {
                    for (int i = 0; i < td.Count; i++)
                    {
                        if (td[i].idDmThiDua == id)
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

        public bool DeleteByArrayId()
        {
            try
            {

                var id = Session["id"];
                string idvbarr = Request["ID"];
                string[] idvb = idvbarr.Split(' ');
                for (int i = 0; i < idvb.Length; i++)
                {
                    qltdkt_dm_thidua _old = _entities.qltdkt_dm_thidua.Find(int.Parse(idvb[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_thidua.Remove(_old);
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