using QLTDKT.Models;
using QLTDKT.Models.Service.hinhThucKhenThuongService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class HinhThucKhenThuongController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private HinhThucKTService _htKTService = new HinhThucKTService();
        // GET: HinhThucKhenThuong
        /*public ActionResult Index()
        {
            return View();
        }*/
        public ActionResult getHinhThucKhenThuongCD()
        {
            return View();
        }
        public ActionResult getHinhThucKhenThuongCM()
        {
            return View();
        }
        public ActionResult getHinhThucKhenThuongDDT()
        {
            return View();
        }

        public JsonResult GetListHTTD()
        {
            int idHT = int.Parse(Request.QueryString["idHT"] != "" ? Request.QueryString["idHT"] : "0");
            int bophan = int.Parse(Request["bophan"]);

            return Json(new { data = _htKTService.getHinhThucKhenThuong(idHT, bophan) }, JsonRequestBehavior.AllowGet);
            /*return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);*/
        }
        public JsonResult GetListHTTDCD()
        {
            int idHT = int.Parse(Request.QueryString["idHT"] != "" ? Request.QueryString["idHT"] : "0");
            int bophan = 3;

            return Json(new { data = _htKTService.getHinhThucKhenThuong(idHT, bophan) }, JsonRequestBehavior.AllowGet);
            /*return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);*/
        }
        public JsonResult GetListHTTDDDT()
        {
            int idHT = int.Parse(Request.QueryString["idHT"] != "" ? Request.QueryString["idHT"] : "0");
            int bophan = 2;

            return Json(new { data = _htKTService.getHinhThucKhenThuong(idHT, bophan) }, JsonRequestBehavior.AllowGet);
            /*return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);*/
        }

        public JsonResult getChuKy()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            var data = _entities.qltdkt_dm_chuky.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getCapKyKT()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            var data = _entities.qltdkt_dm_capkykhenthuong.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public bool ExistHTTD()
        {
            int bophan = int.Parse(Request.QueryString["bophan"] != "0" ? Request.QueryString["bophan"] : "0");

            byte idcntt = byte.Parse(Request.QueryString["idcntt"] != "" ? Request.QueryString["idcntt"] : "");
            string label = Request.QueryString["label"] != "" ? Request.QueryString["label"] : "";

            var chk = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.bophan == bophan && x.loaiKhenThuong == idcntt && x.tenHinhThucKhenThuong == label.ToUpper() && x.tenHinhThucKhenThuong == label.ToLower() && x.daXoa == false).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public string UpdateHTTD(qltdkt_dm_hinhthuckhenthuong _objKH)
        {
            try
            {
                if (_objKH.id == 0)
                {
                    qltdkt_dm_hinhthuckhenthuong _new = new qltdkt_dm_hinhthuckhenthuong();
                    _new.tenHinhThucKhenThuong = _objKH.tenHinhThucKhenThuong;
                    _new.loaiKhenThuong = _objKH.loaiKhenThuong;
                    _new.bophan = _objKH.bophan;
                    _new.moTa = _objKH.moTa;
                    _new.maThanhTich = _objKH.maThanhTich;
                    _new.chuKy = _objKH.chuKy;
                    _new.capThanhTich = _objKH.capThanhTich;
                    _new.ngayTao = DateTime.Now;
                    _new.daXoa = false;
                    _entities.qltdkt_dm_hinhthuckhenthuong.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    qltdkt_dm_hinhthuckhenthuong _update = _entities.qltdkt_dm_hinhthuckhenthuong.Find(_objKH.id);
                    _update.tenHinhThucKhenThuong = _objKH.tenHinhThucKhenThuong;
                    _update.loaiKhenThuong = _objKH.loaiKhenThuong;
                    _update.bophan = _objKH.bophan;
                    _update.moTa = _objKH.moTa;
                    _update.maThanhTich = _objKH.maThanhTich;
                    _update.chuKy = _objKH.chuKy;
                    _update.capThanhTich = _objKH.capThanhTich;
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
            return Json(_entities.qltdkt_dm_hinhthuckhenthuong.FirstOrDefault(x => x.id == ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteByID()
        {
            try
            {
                var id = Session["id"];

                int idhinhthuc = int.Parse(Request["ID"]);
                qltdkt_dm_hinhthuckhenthuong _old = _entities.qltdkt_dm_hinhthuckhenthuong.Find(idhinhthuc);
                if (_old != null)
                {
                    _old.daXoa = true;


                }

                var kt = _entities.qltdkt_khenthuong.Where(x => x.daXoa == false && x.hinhThucKhenThuong != null).ToList();
                if (kt != null && kt.Count > 0)
                {
                    for (int i = 0; i < kt.Count; i++)
                    {
                        if (kt[i].hinhThucKhenThuong == idhinhthuc)
                        {
                            return "warning";
                        }
                    }
                }
                var dh = _entities.qltdkt_danhhieu.Where(y => y.daXoa == false && y.hinhThucTraoTang != null).ToList();
                if (dh != null && dh.Count > 0)
                {
                    for (int j = 0; j < dh.Count; j++)
                    {
                        if (dh[j].hinhThucTraoTang == (byte)idhinhthuc)
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
                    qltdkt_dm_hinhthuckhenthuong _old = _entities.qltdkt_dm_hinhthuckhenthuong.Find(int.Parse(iddonvi[i]));
                    if (_old != null)
                    {
                        _old.daXoa = true;
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