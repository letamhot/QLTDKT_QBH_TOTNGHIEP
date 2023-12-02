using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLTDKT.Models;
using QLTDKT.Models.Service.baoCaoThongKeService;
using QLTDKT.Models.Service.thiDuaService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class BaoCaoThiDuaController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private baoCaoThongKeService _service = new baoCaoThongKeService();
        private BaoCaoThiDuaService _bcService = new BaoCaoThiDuaService();
        private thiDuaService _thiDuaService = new thiDuaService();
        // GET: BaoCaoThiDua
        public ActionResult Index()
        {
            var lsThiDua = _entities.qltdkt_dm_thidua.ToList();
            return View(lsThiDua);
        }

        public JsonResult GetBaoCao()
        {
            DateTime from = DateTime.ParseExact(Request.QueryString["from"], "dd/MM/yyyy", null);
            DateTime to = DateTime.ParseExact(Request.QueryString["to"], "dd/MM/yyyy", null);
            int type = int.Parse(Request.QueryString["type"]);
            int thiDua = int.Parse(Request.QueryString["thiDua"]);

            return Json(new { data = _service.getBaoCaoThongKeThiDua(from, to, thiDua, type) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDmThiDuaByKieu()
        {
            int kieuThiDua = (Request.QueryString["kieuThiDua"] != "" ? int.Parse(Request.QueryString["kieuThiDua"]) : -1);
            return Json(new { data = _entities.qltdkt_dm_thidua.Where(x => x.kieuThiDua == kieuThiDua).ToList() }, JsonRequestBehavior.AllowGet);
        }
        [ValidateInput(false)]
        public int CapNhatBaoCaoThiDua()
        {
            string output_filedinhkem = "";
            int idBaoCaoThiDua = int.Parse(Request.Form.Get("idBaoCaoThiDua") != null ? Request.Form.Get("idBaoCaoThiDua") : "0");
            int idThiDua = int.Parse(Request.Form.Get("idThiDua"));
            string tenBaoCao = Request.Form.Get("tenBaoCao");
            string noiDungBaoCao = Request.Unvalidated.Form.Get("noiDungBaoCao");

            string lsTT = Request.Form.Get("lsIdTT");
            string lsCN = Request.Form.Get("lsIdCN");
            if (idBaoCaoThiDua == 0) //Thêm mới báo cáo thi đua 
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
                        return -1;
                        throw;
                    }
                }

                try
                {
                    var dsDKThiDua = _thiDuaService.getChiTietBaoCaoTT(idThiDua);
                    var rs = _entities.qltdkt_dangkythidua.Where(x => x.thiDuaId == idThiDua && x.daXoa == false).ToList();
                    if (lsTT.Length > 0)
                    {
                        string[] lsTT_spl = lsTT.Split(';');

                        if (rs != null && rs.Count > 0)
                        {
                            for (int i = 0; i < rs.Count; i++)
                            {
                                int idDonViDK = rs[i].donViDangKyId;
                                for (int j = 0; j < lsTT_spl.Length; j++)
                                {
                                    if (idDonViDK == int.Parse(lsTT_spl[j]))
                                    {
                                        rs[i].daBaoCao = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (lsCN.Length > 0)
                    {
                        string[] lsCN_spl = lsCN.Split(';');
                        if (rs != null && rs.Count > 0)
                        {
                            for (int i = 0; i < rs.Count; i++)
                            {
                                if (rs[i].caNhanDangKy_KetQua.Length > 0)
                                {
                                    string jSonChiTietCaNhan = rs[i].caNhanDangKy_KetQua;
                                    var dsCaNhan = JsonConvert.DeserializeObject<List<chiTietDangKyThiDua>>(jSonChiTietCaNhan);
                                    if (dsCaNhan != null && dsCaNhan.Count > 0)
                                    {
                                        for (int j = 0; j < dsCaNhan.Count; j++)
                                        {
                                            int idCaNhanDK = dsCaNhan[j].idNhanVien;
                                            for (int z = 0; z < lsCN_spl.Length; z++)
                                            {
                                                if (idCaNhanDK == int.Parse(lsCN_spl[z]))
                                                {
                                                    dsCaNhan[j].daBaoCao = true;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    rs[i].caNhanDangKy_KetQua = JsonConvert.SerializeObject(dsCaNhan);
                                }
                            }
                        }
                    }
                    ketQuaThiDuaDK kq = new ketQuaThiDuaDK
                    {
                        lsCN = lsCN,
                        lsTT = lsTT
                    };
                    qltdkt_baocaothidua _objNew = new qltdkt_baocaothidua
                    {
                        idThiDua = idThiDua,
                        tenBaoCao = tenBaoCao,
                        noiDungBaoCao = noiDungBaoCao,
                        fileDinhKem = output_filedinhkem,
                        daXoa = false,
                        dsCaNhanTTBaoCao = JsonConvert.SerializeObject(kq),
                        ngayTao = DateTime.Now
                    };
                    _entities.qltdkt_baocaothidua.Add(_objNew);
                    _entities.SaveChanges();
                    return 0;
                }
                catch (Exception ex)
                {
                    return -2;
                    throw;
                }
            }
            else //Cập nhật báo cáo thi đua
            {
                qltdkt_baocaothidua _update = _entities.qltdkt_baocaothidua.Find(idBaoCaoThiDua);
                string dulieu_dinhkem = _update.fileDinhKem;
                if (dulieu_dinhkem == "")
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
                            return -1;
                            throw;
                        }
                    }
                }
                else
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
                            return -1;
                            throw;
                        }
                    }
                    else
                    {
                        output_filedinhkem = dulieu_dinhkem;
                    }
                }

                try
                {
                    var dsDKThiDua = _thiDuaService.getChiTietBaoCaoTT(idThiDua);
                    var rs = _entities.qltdkt_dangkythidua.Where(x => x.thiDuaId == idThiDua && x.daXoa == false).ToList();
                    if (lsTT.Length > 0)
                    {
                        string[] lsTT_spl = lsTT.Split(';');

                        if (rs != null && rs.Count > 0)
                        {
                            for (int i = 0; i < rs.Count; i++)
                            {
                                int idDonViDK = rs[i].donViDangKyId;
                                for (int j = 0; j < lsTT_spl.Length; j++)
                                {
                                    if (lsTT_spl[j] != "")
                                    {
                                        if (idDonViDK == int.Parse(lsTT_spl[j]))
                                        {
                                            rs[i].daBaoCao = true;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (lsCN.Length > 0)
                    {
                        string[] lsCN_spl = lsCN.Split(';');
                        if (rs != null && rs.Count > 0)
                        {
                            for (int i = 0; i < rs.Count; i++)
                            {
                                if (rs[i].caNhanDangKy_KetQua.Length > 0)
                                {
                                    string jSonChiTietCaNhan = rs[i].caNhanDangKy_KetQua;
                                    var dsCaNhan = JsonConvert.DeserializeObject<List<chiTietDangKyThiDua>>(jSonChiTietCaNhan);
                                    if (dsCaNhan != null && dsCaNhan.Count > 0)
                                    {
                                        for (int j = 0; j < dsCaNhan.Count; j++)
                                        {
                                            int idCaNhanDK = dsCaNhan[j].idNhanVien;
                                            for (int z = 0; z < lsCN_spl.Length; z++)
                                            {
                                                if (lsCN_spl[z] != "")
                                                {
                                                    if (idCaNhanDK == int.Parse(lsCN_spl[z]))
                                                    {
                                                        dsCaNhan[j].daBaoCao = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    rs[i].caNhanDangKy_KetQua = JsonConvert.SerializeObject(dsCaNhan);
                                }
                            }
                        }
                    }
                    ketQuaThiDuaDK kq = new ketQuaThiDuaDK
                    {
                        lsCN = lsCN,
                        lsTT = lsTT
                    };
                    _update.tenBaoCao = tenBaoCao;
                    _update.noiDungBaoCao = noiDungBaoCao;
                    _update.fileDinhKem = output_filedinhkem;
                    _update.ngayCapNhat = DateTime.Now;
                    _update.dsCaNhanTTBaoCao = JsonConvert.SerializeObject(kq);
                    _entities.SaveChanges();
                    return 2;
                }
                catch (Exception ex)
                {
                    return -2;
                    throw;
                }
            }
        }


        public bool DeleteBaoCaoThiDua()
        {
            int idBaoCaoThiDua = int.Parse(Request["idBaoCaoThiDua"]);
            try
            {
                qltdkt_baocaothidua _delete = _entities.qltdkt_baocaothidua.Find(idBaoCaoThiDua);
                if (_delete != null)
                {
                    _delete.daXoa = true;
                    if (_delete.dsCaNhanTTBaoCao.Length > 0)
                    {
                        ketQuaThiDuaDK kq = JsonConvert.DeserializeObject<ketQuaThiDuaDK>(_delete.dsCaNhanTTBaoCao);
                        if (kq != null)
                        {
                            string lsTT = kq.lsTT;
                            string lsCN = kq.lsCN;
                            int idThiDua = _delete.idThiDua;
                            var rs = _entities.qltdkt_dangkythidua.Where(x => x.thiDuaId == idThiDua && x.daXoa == false).ToList();
                            if (lsTT.Length > 0)
                            {
                                string[] lsTT_spl = lsTT.Split(';');

                                if (rs != null && rs.Count > 0)
                                {
                                    for (int i = 0; i < rs.Count; i++)
                                    {
                                        int idDonViDK = rs[i].donViDangKyId;
                                        for (int j = 0; j < lsTT_spl.Length; j++)
                                        {
                                            if (lsTT_spl[j] != "")
                                            {
                                                if (idDonViDK == int.Parse(lsTT_spl[j]))
                                                {
                                                    rs[i].daBaoCao = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            var hstd = _entities.qltdkt_hosothidua.Where(z => z.thiDuaId == idThiDua && z.daXoa == false).ToList();
                            if (hstd != null && hstd.Count > 0)
                            {
                                for (int j = 0; j < hstd.Count; j++)
                                {
                                    if (_delete.idThiDua == hstd[j].thiDuaId)
                                    {
                                        hstd[j].daXoa = true;
                                    }

                                }
                            }
                            if (lsCN.Length > 0)
                            {
                                string[] lsCN_spl = lsCN.Split(';');
                                if (rs != null && rs.Count > 0)
                                {
                                    for (int i = 0; i < rs.Count; i++)
                                    {
                                        if (rs[i].caNhanDangKy_KetQua.Length > 0)
                                        {
                                            string jSonChiTietCaNhan = rs[i].caNhanDangKy_KetQua;
                                            var dsCaNhan = JsonConvert.DeserializeObject<List<chiTietDangKyThiDua>>(jSonChiTietCaNhan);
                                            if (dsCaNhan != null && dsCaNhan.Count > 0)
                                            {
                                                for (int j = 0; j < dsCaNhan.Count; j++)
                                                {
                                                    int idCaNhanDK = dsCaNhan[j].idNhanVien;
                                                    for (int z = 0; z < lsCN_spl.Length; z++)
                                                    {
                                                        if (lsCN_spl[z] != "")
                                                        {
                                                            if (idCaNhanDK == int.Parse(lsCN_spl[z]))
                                                            {
                                                                dsCaNhan[j].daBaoCao = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            rs[i].caNhanDangKy_KetQua = JsonConvert.SerializeObject(dsCaNhan);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    _entities.SaveChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
                throw;
            }
            return false;
        }


        public JsonResult getBaoCaoThiDua()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"] != "" ? Request.QueryString["idThiDua"] : "0");
            return Json(new { data = _bcService.getBaoCaoThiDua(idThiDua) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBaoCaoThiDuaById()
        {
            int idBaoCaoThiDua = int.Parse(Request["idBaoCaoThiDua"]);
            return Json(new { data = _entities.qltdkt_baocaothidua.Find(idBaoCaoThiDua) }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult ExportExcel()
        {
            DateTime from = DateTime.ParseExact(Request.QueryString["from"], "dd/MM/yyyy", null);
            DateTime to = DateTime.ParseExact(Request.QueryString["to"], "dd/MM/yyyy", null);
            int type = int.Parse(Request.QueryString["type"]);
            int thiDua = int.Parse(Request.QueryString["thiDua"]);


            var data = _service.getBaoCaoThongKeThiDua(from, to, thiDua, type);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            if (type == 1)
            {
                sheet.Column(1).Width = 5;
                sheet.Column(2).Width = 25;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 50;
                sheet.Column(6).Width = 50;
                sheet.Column(7).Width = 30;


                sheet.Cells[row, 1, row + 1, 7].Merge = true;
                sheet.Cells["A" + row.ToString()].Value = "BÁO CÁO THI ĐUA TẬP THỂ";
                sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                row = row + 2;
                sheet.Cells["A" + row.ToString()].Value = "STT";
                sheet.Cells["B" + row.ToString()].Value = "Số quyết định";
                sheet.Cells["C" + row.ToString()].Value = "Ngày ban hành";
                sheet.Cells["D" + row.ToString()].Value = "Tên tập thể";
                sheet.Cells["E" + row.ToString()].Value = "Tên thi đua";


                sheet.Cells["F" + row.ToString()].Value = "Nội dung thi đua";
                sheet.Cells["G" + row.ToString()].Value = "Người ký";

                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 1, row, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //sheet.Cells[row, 1, row, 3].Style.b
                row++;
                int stt = 1;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.Cells["A" + row.ToString()].Value = stt;
                        sheet.Cells["B" + row.ToString()].Value = data[i].soQuyetDinh;
                        sheet.Cells["C" + row.ToString()].Value = data[i].ngayPhatDong;
                        sheet.Cells["D" + row.ToString()].Value = data[i].tenDonVi_CaNhan;
                        sheet.Cells["E" + row.ToString()].Value = data[i].tenThiDua;

                        sheet.Cells["F" + row.ToString()].Value = data[i].noiDungThiDua;
                        sheet.Cells["G" + row.ToString()].Value = data[i].nguoiKy;


                        stt++;
                        row++;
                    }
                }
            }
            if (type == 2)
            {
                sheet.Column(1).Width = 5;
                sheet.Column(2).Width = 25;
                sheet.Column(3).Width = 50;
                sheet.Column(4).Width = 30;
                sheet.Column(5).Width = 15;
                sheet.Column(6).Width = 15;
                sheet.Column(7).Width = 30;
                sheet.Column(8).Width = 30;
                sheet.Column(9).Width = 30;

                sheet.Cells[row, 1, row + 1, 9].Merge = true;
                sheet.Cells["A" + row.ToString()].Value = "BÁO CÁO THI ĐUA CÁ NHÂN";
                sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                row = row + 2;
                sheet.Cells["A" + row.ToString()].Value = "STT";
                sheet.Cells["B" + row.ToString()].Value = "Số quyết định";
                sheet.Cells["C" + row.ToString()].Value = "Ngày ban hành";
                sheet.Cells["D" + row.ToString()].Value = "Tên cá nhân";
                sheet.Cells["E" + row.ToString()].Value = "Đơn vị";
                sheet.Cells["F" + row.ToString()].Value = "Chức vụ";
                sheet.Cells["G" + row.ToString()].Value = "Tên thi đua";

                sheet.Cells["H" + row.ToString()].Value = "Nội dung thi đua";
                sheet.Cells["I" + row.ToString()].Value = "Người Ký";


                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 1, row, 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                row++;
                int stt = 1;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.Cells["A" + row.ToString()].Value = stt;
                        sheet.Cells["B" + row.ToString()].Value = data[i].soQuyetDinh;
                        sheet.Cells["C" + row.ToString()].Value = data[i].ngayPhatDong.Split(' ')[0];
                        sheet.Cells["D" + row.ToString()].Value = data[i].tenDonVi_CaNhan;
                        sheet.Cells["E" + row.ToString()].Value = data[i].donVi;
                        sheet.Cells["F" + row.ToString()].Value = data[i].chucVu;
                        sheet.Cells["G" + row.ToString()].Value = data[i].tenThiDua;

                        sheet.Cells["H" + row.ToString()].Value = data[i].noiDungThiDua;
                        sheet.Cells["I" + row.ToString()].Value = data[i].nguoiKy;


                        sheet.Cells[row, 1, row, 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[row, 1, row, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[row, 1, row, 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        sheet.Cells[row, 1, row, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        stt++;
                        row++;
                    }
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "BaoCaoThiDua.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("Index", "BaoCaoThiDua");

        }

        public JsonResult getDSThiDua()
        {
            List<dsThiDua> result = new List<dsThiDua>();
            var lsDs = _entities.qltdkt_thidua.Where(x => x.trangThai == 2 && x.daXoa == false).ToList();
            if (lsDs != null && lsDs.Count > 0)
            {
                for (int i = 0; i < lsDs.Count; i++)
                {
                    dsThiDua _new = new dsThiDua
                    {
                        id = lsDs[i].id,
                        tenHienThi = (_entities.qltdkt_dm_thidua.Find(lsDs[i].idDmThiDua).tenThiDua) + " - " + lsDs[i].soHieu
                    };
                    result.Add(_new);
                }
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getThongTinThiDua()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"]);
            return Json(new { data = _bcService.getChiTietThiDua(idThiDua) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getThongTinThiDuaAndBaoCao()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"]);
            return Json(new { data = _bcService.getChiTietThiDuaBaoCao(idThiDua) }, JsonRequestBehavior.AllowGet);
        }
    }
    class dsThiDua
    {
        public int id { get; set; }
        public string tenHienThi { get; set; }
    }


}