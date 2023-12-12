using Newtonsoft.Json;
using QLTDKT.Models;
using QLTDKT.Models.Service.sangKienService;
using QLTDKT.Util;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class SangKienController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private sangKienService _sangKienService = new sangKienService();
        private JsTreeAccess _util = new JsTreeAccess();
        // GET: SangKien
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getSangKienDDT()
        {
            return View();
        }
        public ActionResult getSangKienCM()
        {
            return View();
        }
        public ActionResult getSangKienCD()
        {
            return View();
        }
        public JsonResult loadSangKien()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            var data = _entities.qltdkt_dm_sangkien.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult tenSangKienByNoiDung()
        {
            int idSangKien = int.Parse(Request.QueryString["idSangKien"]);
            return Json(new { data = _entities.qltdkt_dm_sangkien.Find(idSangKien) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ThemCaNhanTapTheCache(string idcn, int idsk, string listCurrent)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _sangKienService.ThemCaNhanTapTheCache(idcn, idsk, listCurrent);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachSangKienCM()
        {
            string soQuyetDinh = Request["soQuyetDinh"];
            string ngaySangKien_TuNgay = Request["ngaySangKien_TuNgay"];
            string ngaySangKien_DenNgay = Request["ngaySangKien_DenNgay"];
            int bophan = int.Parse(Request["bophan"]);
            return Json(new { data = _sangKienService.getDanhSachSangKien( bophan, soQuyetDinh, ngaySangKien_TuNgay, ngaySangKien_DenNgay) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachSangKienCD()
        {

            string soQuyetDinh = Request["soQuyetDinh"];
            string ngaySangKien_TuNgay = Request["ngaySangKien_TuNgay"];
            string ngaySangKien_DenNgay = Request["ngaySangKien_DenNgay"];
            int bophan = 3;
            return Json(new { data = _sangKienService.getDanhSachSangKien(bophan, soQuyetDinh, ngaySangKien_TuNgay, ngaySangKien_DenNgay) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachSangKienDDT()
        {

            string soQuyetDinh = Request["soQuyetDinh"];
            string ngaySangKien_TuNgay = Request["ngaySangKien_TuNgay"];
            string ngaySangKien_DenNgay = Request["ngaySangKien_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int bophan = 2;
            return Json(new { data = _sangKienService.getDanhSachSangKien(bophan, soQuyetDinh, ngaySangKien_TuNgay, ngaySangKien_DenNgay) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadTreeCaNhan()
        {
            return Json(_util.getTreeCaNhan(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult loadNhanVienByDonVi(int idDonVi)
        {
            //int idSangKien = int.Parse(Request["idSangKien"]);
            var data = _sangKienService.loadNhanVienByDonVi(idDonVi);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult xoaCaNhanTapThe(string idcntt, int idkt)
        {
            //int idSangKien = int.Parse(Request["idSangKien"]);
            var data = _sangKienService.xoaCaNhanTapThe(idcntt, idkt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult searchCaNhan(string idDonVi, string searchText)
        {
            var data = _sangKienService.searchCaNhan(idDonVi, searchText);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        //cập nhật khen thưởng
        public int CapNhatSangKien()
        {
            int idSangKien;
            if (Request.Form.Get("idSangKien") == "")
            {
                idSangKien = 0;
            }
            else
            {
                idSangKien = int.Parse(Request.Form.Get("idSangKien"));

            }
            int loaiSangKien = int.Parse(Request.Form.Get("loaiSangKien"));
            byte bophan = byte.Parse(Request.Form.Get("bophan"));
            string soQuyetDinh = Request.Form.Get("soQuyetDinh");
            string ngaySangKien = Request.Form.Get("ngaySangKien");
            int idDMSangKien = int.Parse(Request.Form.Get("tenDmSangKien"));
            string tien = Request.Form.Get("tienThuong");
            int tongtien = 0;

            //string hinhThucSangKien = Request.Form.Get("hinhThucSangKien");
            //string danhSachCaNhanTapThe = Request.Form.Get("danhSachCaNhanTapThe");
            string danhSachCaNhanTapThe = Request.Unvalidated.Form.Get("danhSachCaNhanTapThe");

            //byte trangThai = byte.Parse(Request.Form.Get("trangThai"));
           

            if (idSangKien == 0) ///Thêm mới  thi đua
            {
                List<dsChiTietCaNhanTapThe> lsDsCNTT = new List<dsChiTietCaNhanTapThe>();
                if (danhSachCaNhanTapThe.Length > 0)
                {
                    string[] spl = danhSachCaNhanTapThe.Split(',');
                    string[] lst = tien.Split(',');
                    for (int i = 0; i < spl.Length; i++)
                    {

                        if (spl[i] != "")
                        {
                            int id_NV = int.Parse(spl[i]);
                            qltdkt_dm_nhanvien _dmNV = _entities.qltdkt_dm_nhanvien.Find(id_NV);
                            dsChiTietCaNhanTapThe _newNV = new dsChiTietCaNhanTapThe
                            {
                                id = _dmNV.id,
                                tenCaNhanTapThe = _dmNV.hoTen,
                                chucDanh = _entities.qltdkt_dm_chucvu.Find(_dmNV.chucVuId).tenChucVu,
                                donVi = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId).tenDonVi,
                                donViCha = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(_dmNV.donViId).idCha).tenDonVi,
                                tienThuong = lst[i] != "" ? lst[i] : "0",
                            };
                            tongtien = tongtien + int.Parse(_newNV.tienThuong);
                            lsDsCNTT.Add(_newNV);

                        }

                    }
                }
                danhSachCaNhanTapThe dsCNTT = new danhSachCaNhanTapThe();
                dsCNTT.loaiSangKien = loaiSangKien;
                dsCNTT.tongtien = tongtien;
                dsCNTT.dsChiTietCaNhanTapThe = lsDsCNTT;
                
                try
                {
                    qltdkt_sangkien _newObj = new qltdkt_sangkien
                    {
                        id = idSangKien,
                        loaiSangKien = loaiSangKien,
                        bophan = bophan,
                        soQuyetDinh = soQuyetDinh,
                        danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCNTT),
                        idSangKien = idDMSangKien,
                        ngaySangKien = DateTime.ParseExact(ngaySangKien, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        tienThuong = tongtien,
                        daXoa = 0,
                    };

                    _entities.qltdkt_sangkien.Add(_newObj);
                    _entities.SaveChanges();

                    return 0;
                }
                catch (Exception ex)
                {
                    return -2;
                    throw;
                }

            }
            else ///Cập nhật thi đua
            {
                tongtien = 0;
                List<dsChiTietCaNhanTapThe> lsDsCNTT = new List<dsChiTietCaNhanTapThe>();
                if (danhSachCaNhanTapThe.Length > 0)
                {
                    string[] spl = danhSachCaNhanTapThe.Split(',');
                    string[] lst = tien.Split(',');
                    for (int i = 0; i < spl.Length; i++)
                    {

                        if (spl[i] != "")
                        {
                            int id_NV = int.Parse(spl[i]);
                            qltdkt_dm_nhanvien _dmNV = _entities.qltdkt_dm_nhanvien.Find(id_NV);
                            qltdkt_dm_donvi _dmDVC = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId);
                            if (_dmDVC.idCha > 0)
                            {
                                dsChiTietCaNhanTapThe _newNV = new dsChiTietCaNhanTapThe
                                {
                                    id = _dmNV.id,
                                    tenCaNhanTapThe = _dmNV.hoTen,
                                    chucDanh = _entities.qltdkt_dm_chucvu.Find(_dmNV.chucVuId).tenChucVu,
                                    donVi = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId).tenDonVi,
                                    donViCha = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(_dmNV.donViId).idCha).tenDonVi,

                                    tienThuong = lst[i] != "" ? lst[i] : "0",
                                };
                                tongtien = tongtien + int.Parse(_newNV.tienThuong);

                                lsDsCNTT.Add(_newNV);
                            }
                            else
                            {
                                dsChiTietCaNhanTapThe _newNV = new dsChiTietCaNhanTapThe
                                {
                                    id = _dmNV.id,
                                    tenCaNhanTapThe = _dmNV.hoTen,
                                    chucDanh = _entities.qltdkt_dm_chucvu.Find(_dmNV.chucVuId).tenChucVu,
                                    donVi = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId).tenDonVi,
                                    tienThuong = lst[i] != "" ? lst[i] : "0",
                                };
                                tongtien = tongtien + int.Parse(_newNV.tienThuong);

                                lsDsCNTT.Add(_newNV);
                            }

                        }

                    }
                }
                danhSachCaNhanTapThe dsCNTT = new danhSachCaNhanTapThe();
                dsCNTT.loaiSangKien = loaiSangKien;
                dsCNTT.tongtien = tongtien;
                dsCNTT.dsChiTietCaNhanTapThe = lsDsCNTT;
               
                try
                {
                    var dh = _entities.qltdkt_sangkien.Find(idSangKien);
                    dh.id = idSangKien;
                    dh.soQuyetDinh = soQuyetDinh;
                    dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCNTT);
                    dh.bophan = bophan;
                    dh.idSangKien = idDMSangKien;
                    dh.ngaySangKien = DateTime.ParseExact(ngaySangKien, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dh.tienThuong = tongtien;

                    _entities.SaveChanges();
                    return 2;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
            }
            //return 0;
        }
        public JsonResult ChinhSuaSangKien()
        {
            int idSangKien = int.Parse(Request["idSangKien"]);
            var data = _sangKienService.getSangKienByID(idSangKien);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

    }
}