using Newtonsoft.Json;
using QLTDKT.Models.Service.groupUserService;
using QLTDKT.Models;
using QLTDKT.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.Globalization;
using System.IO;
using QLTDKT.Models.Service.KhenThuongService;
using QLTDKT.Models.Service.DanhHieuService;
using static QLTDKT.Models.Service.NhanVienService.NhanVienModel;

namespace QLTDKT.Controllers
{
    public class NhanVienController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public JsTreeAccess _jsUtil = new JsTreeAccess();
        private groupUserService _service = new groupUserService();


        // GET: NhanVien
        public ActionResult Index()
        {
            return View();
        }
       
       
        public JsonResult getAllDanhSach()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_nhanvien.Where(x => x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetListNhanVien()
        {

            int iddonvi = Convert.ToInt32(Request["id"]);
            _entities.Configuration.ProxyCreationEnabled = false;
            List<int> rs = _service.getDonViChildByDonViId(iddonvi);
            return Json(new
            {
                data = (from nv in _entities.qltdkt_dm_nhanvien
                        join cv in _entities.qltdkt_dm_chucvu
                            on nv.chucVuId equals cv.id
                        join dv in _entities.qltdkt_dm_donvi
                             on nv.donViId equals dv.id
                        where rs.Contains((int)nv.donViId) & nv.daXoa == false & nv.trangThai == 0 & nv.maNhanVien != "cntt_01"
                        
                        select new
                        {
                            IDNhanVien = nv.id,
                            MaNhanVien = nv.maNhanVien,
                            HoTen = nv.hoTen,
                            DienThoai = nv.soDienThoai,
                            GioiTinh = nv.gioiTinh,
                            TenDonVi = dv.tenDonVi,
                            trangThai = nv.trangThai,
                        }).ToList()
            }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadTreeDonVi()
        {
            return Json(_jsUtil.getTreeCaNhanTapThe(), JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetListCV()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_chucvu.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public string DeleteByID()
        {
            try
            {


                int idNhanvien = int.Parse(Request["id"]);
                qltdkt_dm_nhanvien _old = _entities.qltdkt_dm_nhanvien.Find(idNhanvien);
                if (_old != null)
                {
                    _old.daXoa = true;

                }
                var kt = _entities.qltdkt_khenthuong.Where(x => x.daTraoTang == false && x.danhSachCaNhanKhenThuong != null && x.daXoa == false).ToList();
                if (kt != null && kt.Count > 0)
                {
                    for (int i = 0; i < kt.Count; i++)
                    {
                        dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(kt[i].danhSachCaNhanKhenThuong);
                        var dscn = ds.dschitietcanhantapthekhenthuong;
                        for (int j = 0; j < dscn.Count; j++)
                        {
                            if (idNhanvien == dscn[j].id)
                            {
                                return "warning1";
                            }
                        }
                    }
                }
                var dh = _entities.qltdkt_danhhieu.Where(y => y.daTraoTang == false && y.danhSachCaNhanTapThe != null && y.daXoa == false).ToList();
                if (dh != null && dh.Count > 0)
                {
                    for (int k = 0; k < dh.Count; k++)
                    {
                        danhSachCaNhanTapThe dsdhcn = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh[k].danhSachCaNhanTapThe);
                        var dhcn = dsdhcn.dsChiTietCaNhanTapThe;
                        for (int v = 0; v < dhcn.Count; v++)
                        {
                            if (idNhanvien == dhcn[v].id)
                            {
                                return "warning2";
                            }
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
        public JsonResult GetNhanVienById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int IDNhanVien = int.Parse(Request["IDNhanVien"]);
            return Json(_entities.qltdkt_dm_nhanvien.Find(IDNhanVien), JsonRequestBehavior.AllowGet);
        }
        public bool ExistNhanVienEmail()
        {
            string labelEmail = Request.QueryString["labelEmail"];

            var chk = _entities.qltdkt_dm_nhanvien.Where(x => x.email == labelEmail.ToLower() && x.email == labelEmail.ToUpper() && x.daXoa == false).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        public bool ExistNhanVienMaNV()
        {
            string labelMaNV = Request.QueryString["labelMaNV"];
            var chk = _entities.qltdkt_dm_nhanvien.Where(x => x.maNhanVien == labelMaNV.ToLower() && x.maNhanVien == labelMaNV.ToUpper() && x.daXoa == false).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }

        public JsonResult loadDanhSachTrinhDoHocVan()
        {
            var data = _entities.qltdkt_dm_trinhdohocvan.ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public bool ExistNhanVienPhone()
        {
            string labelPhone = Request.QueryString["labelPhone"];
            var chk = _entities.qltdkt_dm_nhanvien.Where(x => x.soDienThoai == labelPhone && x.daXoa == false).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        [HttpPost]
        public string UpdateNhanVien()
        {
            int id = int.Parse(Request.Form.Get("id"));
            int chucVuId = int.Parse(Request.Form.Get("chucVuId"));
            int donViId = int.Parse(Request.Form.Get("donViId"));
            string hoTen = Request.Form.Get("hoTen");
            string maNhanVien = Request.Form.Get("maNhanVien");
            string ngaySinh = Request.Form.Get("ngaySinh");
            string email = Request.Form.Get("email");
            string soDienThoai = Request.Form.Get("soDienThoai");
            byte trangThai = byte.Parse(Request["trangThai"] != null ? Request["trangThai"] : "");
            bool gioiTinh = Convert.ToBoolean(int.Parse(Request.Form.Get("gioiTinh")));
            string output_filedinhkem = "";
            string f = "";
            string trinhDoHocVan = Request.Unvalidated.Form.Get("trinhDoHocVan");
            List<danhSachTrinhDoHocVan> lsTD = new List<danhSachTrinhDoHocVan>();
            if (trinhDoHocVan.Length > 0)
            {
                string[] spl = trinhDoHocVan.Split(',');
                for (int i = 0; i < spl.Length; i++)
                {
                    if (spl[i] != "")
                    {
                        int id_dm_td = int.Parse(spl[i]);
                        qltdkt_dm_trinhdohocvan _dmTD = _entities.qltdkt_dm_trinhdohocvan.Find(id_dm_td);
                        danhSachTrinhDoHocVan _newTD = new danhSachTrinhDoHocVan
                        {
                            id = _dmTD.id,
                            tenTrinhDoHocVan = _dmTD.tenTrinhDoHocVan,
                            chuyenNganh = _dmTD.chuyenNganh
                        };
                        lsTD.Add(_newTD);
                    }

                }
            }

            qltdkt_dm_nhanvien nv_check = _entities.qltdkt_dm_nhanvien.Where(x => x.maNhanVien == maNhanVien && x.daXoa == false).FirstOrDefault();
            if (id == 0)
            {

                if (nv_check is null)
                {
                    if (Request.Files.Count > 0)
                    {
                        try
                        {
                            string currentYear = DateTime.Now.Year.ToString();
                            string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                            if (!Directory.Exists(fpath))
                            {
                                Directory.CreateDirectory(fpath);
                            }
                            HttpFileCollectionBase files = Request.Files;
                            List<KeyValuePair<string, string>> lsFileDownload = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < files.Count; i++)
                            {
                                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                                string filename = Path.GetFileName(Request.Files[i].FileName);

                                HttpPostedFileBase file = files[i];
                                string fname, ftype;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = file.FileName;
                                }
                                fname = Path.Combine(fpath, fname);
                                ftype = file.ContentType;
                                f = ftype;

                                if (!System.IO.File.Exists(fname))
                                {
                                    file.SaveAs(fname);
                                }
                                lsFileDownload.Add(new KeyValuePair<string, string>(fname, ftype));
                                output_filedinhkem = JsonConvert.SerializeObject(lsFileDownload);
                            }

                        }
                        catch (Exception)
                        {
                            return "warning";
                            throw;
                        }
                    }
                    try
                    {
                        if (f != "image/jpeg" && f != "image/png" && f != "")
                        {
                            return "anh";
                        }
                        else
                        {
                            qltdkt_dm_nhanvien _new = new qltdkt_dm_nhanvien();
                            //_new.ID = ID;
                            _new.maNhanVien = maNhanVien;
                            _new.hoTen = hoTen;
                            _new.ngaySinh = DateTime.ParseExact(ngaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                            _new.donViId = donViId;
                            _new.chucVuId = chucVuId;
                            _new.gioiTinh = gioiTinh;
                            _new.soDienThoai = soDienThoai;
                            _new.trangThai = trangThai;
                            _new.ngayTao = DateTime.Now;
                            _new.ngayCapNhat = null;
                            _new.email = email;
                            _new.daXoa = false;
                            _new.anhDaiDien = output_filedinhkem;
                            _new.trinhDoHocVan = JsonConvert.SerializeObject(lsTD);
                            _entities.qltdkt_dm_nhanvien.Add(_new);
                            _entities.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        return "fail";
                    }
                }
                else
                {
                    return "trungMaNV";
                }

            }
            else
            {
                qltdkt_dm_nhanvien _nvupdate = _entities.qltdkt_dm_nhanvien.Where(c => c.id == id).FirstOrDefault();
                if (_nvupdate.maNhanVien != maNhanVien)
                {
                    if (nv_check is null)
                    {
                        if (Request.Files.Count > 0)
                        {
                            try
                            {
                                string currentYear = DateTime.Now.Year.ToString();
                                string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                                if (!Directory.Exists(fpath))
                                {
                                    Directory.CreateDirectory(fpath);
                                }
                                HttpFileCollectionBase files = Request.Files;
                                List<KeyValuePair<string, string>> lsFileDownload = new List<KeyValuePair<string, string>>();
                                for (int i = 0; i < files.Count; i++)
                                {
                                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                                    string filename = Path.GetFileName(Request.Files[i].FileName);

                                    HttpPostedFileBase file = files[i];
                                    string fname, ftype;
                                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                    {
                                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                        fname = testfiles[testfiles.Length - 1];
                                    }
                                    else
                                    {
                                        fname = file.FileName;
                                    }
                                    fname = Path.Combine(fpath, fname);

                                    ftype = file.ContentType;
                                    f = ftype;
                                    if (!System.IO.File.Exists(fname))
                                    {
                                        file.SaveAs(fname);
                                    }
                                    lsFileDownload.Add(new KeyValuePair<string, string>(fname, ftype));
                                    output_filedinhkem = JsonConvert.SerializeObject(lsFileDownload);
                                }
                            }
                            catch (Exception)
                            {
                                return "warning";
                                throw;
                            }
                        }
                        try
                        {
                            if (f != "image/jpeg" && f != "image/png" && f != "")
                            {
                                return "anh";
                            }
                            else
                            {
                                qltdkt_dm_nhanvien _update = _entities.qltdkt_dm_nhanvien.Find(id);
                                _update.maNhanVien = maNhanVien;
                                _update.hoTen = hoTen;
                                _update.ngaySinh = DateTime.ParseExact(ngaySinh, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                _update.gioiTinh = gioiTinh;
                                _update.trangThai = trangThai;
                                _update.donViId = donViId;
                                _update.chucVuId = chucVuId;
                                _update.soDienThoai = soDienThoai;
                                _update.ngayCapNhat = DateTime.Now;
                                _update.email = email;
                                _update.anhDaiDien = output_filedinhkem;
                                _update.trinhDoHocVan = JsonConvert.SerializeObject(lsTD);

                                _entities.SaveChanges();
                            }
                        }
                        catch (DbEntityValidationException e)
                        {
                            foreach (var eve in e.EntityValidationErrors)
                            {
                                Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                    eve.Entry.Entity.GetType().Name, eve.Entry.State);
                                foreach (var ve in eve.ValidationErrors)
                                {
                                    Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                        ve.PropertyName, ve.ErrorMessage);
                                }
                            }
                            return "fail";
                        }
                    }
                    else
                    {
                        return "trungMaNV";
                    }
                }
                else
                {
                    try
                    {
                        if (Request.Files.Count > 0)
                        {
                            try
                            {
                                string currentYear = DateTime.Now.Year.ToString();
                                string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                                if (!Directory.Exists(fpath))
                                {
                                    Directory.CreateDirectory(fpath);
                                }
                                HttpFileCollectionBase files = Request.Files;
                                List<KeyValuePair<string, string>> lsFileDownload = new List<KeyValuePair<string, string>>();
                                for (int i = 0; i < files.Count; i++)
                                {
                                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                                    string filename = Path.GetFileName(Request.Files[i].FileName);

                                    HttpPostedFileBase file = files[i];
                                    string fname, ftype;
                                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                    {
                                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                        fname = testfiles[testfiles.Length - 1];
                                    }
                                    else
                                    {
                                        fname = file.FileName;
                                    }
                                    fname = Path.Combine(fpath, fname);
                                    ftype = file.ContentType;
                                    f = ftype;
                                    if (!System.IO.File.Exists(fname))
                                    {
                                        file.SaveAs(fname);
                                    }
                                    lsFileDownload.Add(new KeyValuePair<string, string>(fname, ftype));
                                    output_filedinhkem = JsonConvert.SerializeObject(lsFileDownload);
                                }
                            }
                            catch (Exception)
                            {
                                return "warning";
                                throw;
                            }
                        }
                        if (f != "image/jpeg" && f != "image/png" && f != "")
                        {
                            return "anh";
                        }
                        else
                        {
                            qltdkt_dm_nhanvien _update = _entities.qltdkt_dm_nhanvien.Find(id);
                            _update.maNhanVien = maNhanVien;
                            _update.hoTen = hoTen;
                            _update.ngaySinh = DateTime.ParseExact(ngaySinh, "d/M/yyyy", CultureInfo.InvariantCulture);
                            _update.gioiTinh = gioiTinh;
                            _update.trangThai = trangThai;
                            _update.donViId = donViId;
                            _update.chucVuId = chucVuId;
                            _update.soDienThoai = soDienThoai;
                            _update.ngayCapNhat = DateTime.Now;
                            _update.email = email;
                            _update.anhDaiDien = output_filedinhkem;
                            _update.trinhDoHocVan = JsonConvert.SerializeObject(lsTD);
                            _entities.SaveChanges();
                        }
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        return "fail";
                    }
                }

            }
            return "updatesuccess";
        }
    }

}