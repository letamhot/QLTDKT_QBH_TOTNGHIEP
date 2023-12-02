using ExcelDataReader;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLTDKT.Models;
using QLTDKT.Models.Service.thiDuaService;
using QLTDKT.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class ThiDuaController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private thiDuaService _thiduaService = new thiDuaService();
        private BaoCaoThiDuaService _bcService = new BaoCaoThiDuaService();
        private JsTreeAccess _util = new JsTreeAccess();
        // GET: ThiDua
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public int ImportFile()
        {

            try
            {
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                    string filename = Path.GetFileName(Request.Files[i].FileName);

                    HttpPostedFileBase postedFile = files[i];

                    if (ModelState.IsValid)
                    {
                        string filePath = string.Empty;
                        if (postedFile != null)
                        {
                            Stream stream = postedFile.InputStream;

                            IExcelDataReader reader = null;


                            if (postedFile.FileName.EndsWith(".xls"))
                            {
                                reader = ExcelReaderFactory.CreateBinaryReader(stream);
                            }
                            else if (postedFile.FileName.EndsWith(".xlsx"))
                            {
                                reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                            }
                            else
                            {
                                ModelState.AddModelError("File", "This file format is not supported");
                                return 0;
                            }

                            int fieldcount = reader.FieldCount;
                            int rowcount = reader.RowCount;
                            DataTable dt = new DataTable();
                            DataRow row;
                            DataTable dt_ = new DataTable();
                            try
                            {
                                dt_ = reader.AsDataSet().Tables[0];
                                for (int j = 0; j < dt_.Columns.Count; j++)
                                {
                                    dt.Columns.Add("column" + j);
                                }
                                int rowcounter = 0;
                                for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
                                {
                                    row = dt.NewRow();

                                    for (int col = 0; col < dt_.Columns.Count; col++)
                                    {
                                        row[col] = dt_.Rows[row_][col].ToString();
                                        rowcounter++;
                                    }
                                    dt.Rows.Add(row);
                                }

                            }
                            catch (Exception ex)
                            {
                                ModelState.AddModelError("File", "Unable to Upload file!");
                                return 0;
                            }

                            DataTable dt_SoHieu = SelectDistinct(dt, "column4");//Lấy dữ liệu theo cột số hiệu

                            if (dt_SoHieu.Rows.Count > 0)
                            {
                                for (int v = 0; v < dt_SoHieu.Rows.Count; v++)
                                {
                                    qltdkt_thidua td = new qltdkt_thidua();

                                    DataRow[] found = dt.Select("column4 = '" + dt_SoHieu.Rows[v][0] + "'");
                                    if (found.Length > 0)
                                    {
                                        td.idDmThiDua = int.Parse(found[0][3].ToString());
                                        td.ngayPhatDong = DateTime.Parse(found[0][5].ToString());
                                        td.ngayKetThuc = DateTime.Parse(found[0][6].ToString());
                                        td.noiDungPhatDong = found[0][7].ToString();
                                        td.phatDongId = byte.Parse(found[0][1].ToString());
                                        td.soHieu = found[0][4].ToString();
                                        td.kieuThiDua = byte.Parse(found[0][2].ToString());
                                        td.doiTuongThamGia = byte.Parse(found[0][8].ToString());
                                        td.nguoiKy = found[0][9].ToString();
                                        td.trangThai = 2;
                                        td.daXoa = false;
                                        _entities.qltdkt_thidua.Add(td);
                                        _entities.SaveChanges();
                                    }

                                }
                            }

                        }
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                return 0;
                throw;
            }
        }

        #region DATASET HELPER  
        private bool ColumnEqual(object A, object B)
        {
            // Compares two values to see if they are equal. Also compares DBNULL.Value.             
            if (A == DBNull.Value && B == DBNull.Value) //  both are DBNull.Value  
                return true;
            if (A == DBNull.Value || B == DBNull.Value) //  only one is BNull.Value  
                return false;
            return (A.Equals(B)); // value type standard comparison  
        }
        public DataTable SelectDistinct(DataTable SourceTable, string FieldName)
        {
            // Create a Datatable – datatype same as FieldName  
            DataTable dt = new DataTable(SourceTable.TableName);
            dt.Columns.Add(FieldName, SourceTable.Columns[FieldName].DataType);
            // Loop each row & compare each value with one another  
            // Add it to datatable if the values are mismatch  
            object LastValue = null;
            foreach (DataRow dr in SourceTable.Select("", FieldName))
            {
                if (LastValue == null || !(ColumnEqual(LastValue, dr[FieldName])))
                {
                    LastValue = dr[FieldName];
                    dt.Rows.Add(new object[] { LastValue });
                }
            }
            return dt;
        }
        #endregion

        public void ExportDm()
        {
            var data2 = _entities.qltdkt_danhhieu.Where(x => x.daXoa == false && x.id == 15).ToList();
            ExcelPackage ep = new ExcelPackage();

            ExcelWorksheet sheet2 = ep.Workbook.Worksheets.Add("Thi đua");
            int row = 1;


            sheet2.Cells["A" + row.ToString()].Value = "ID thi đua";
            sheet2.Cells["B" + row.ToString()].Value = "Cấp phát động";
            sheet2.Cells["C" + row.ToString()].Value = "Kiểu thi đua";
            sheet2.Cells["D" + row.ToString()].Value = "ID danh mục thi đua";
            sheet2.Cells["E" + row.ToString()].Value = "Số hiệu";
            sheet2.Cells["F" + row.ToString()].Value = "Ngày phát động";
            sheet2.Cells["G" + row.ToString()].Value = "Ngày kết thúc";
            sheet2.Cells["H" + row.ToString()].Value = "Nội dung phát động";
            sheet2.Cells["I" + row.ToString()].Value = "Đối tượng tham gia";
            sheet2.Cells["J" + row.ToString()].Value = "Người ký";

            sheet2.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet2.Row(row).Style.Font.Bold = true;
            sheet2.Cells[row, 1, row, 10].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 10].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 10].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 10].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            var data = _entities.qltdkt_dm_donviphatdong.ToList();

            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Danh mục Đơn vị phát động");
            row = 1;
            sheet.Cells["A" + row.ToString()].Value = "Mã đơn vị phát động";
            sheet.Cells["B" + row.ToString()].Value = "Tên đơn vị phát động";
            sheet.Cells["C" + row.ToString()].Value = "Kiểu thi đua = 0:Năm / 1: Giai đoạn / 2: Chuyên đề";
            sheet.Cells["D" + row.ToString()].Value = "Trạng Thái = 2";
            sheet.Cells["E" + row.ToString()].Value = "Đối tượng tham gia = 1 - Cá nhân; Đối tượng tham gia = 0 - Tập thể";
            sheet.Cells["C" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["C" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["D" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["D" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["E" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["E" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    {
                        sheet.Cells["A" + row.ToString()].Value = data[i].id;
                        sheet.Cells["B" + row.ToString()].Value = data[i].tenPhatDong;

                    }
                    sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }
            var data1 = _entities.qltdkt_dm_thidua.ToList();

            ExcelWorksheet sheet1 = ep.Workbook.Worksheets.Add("Danh mục thi đua");
            row = 1;
            sheet1.Cells["A" + row.ToString()].Value = "Mã thi đua";
            sheet1.Cells["B" + row.ToString()].Value = "Tên thi đua";

            sheet1.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet1.Row(row).Style.Font.Bold = true;
            sheet1.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data1 != null && data1.Count > 0)
            {
                for (int i = 0; i < data1.Count; i++)
                {
                    {
                        sheet1.Cells["A" + row.ToString()].Value = data1[i].id;
                        sheet1.Cells["B" + row.ToString()].Value = data1[i].tenThiDua;

                    }
                    sheet1.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }




            sheet2.Cells["A:AZ"].AutoFitColumns();
            sheet.Cells["A:AZ"].AutoFitColumns();
            sheet1.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "danhMuc.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();

        }

        public ActionResult DangKyThiDua()
        {
            return View();
        }

        public ActionResult BaoCaoThiDua()
        {
            return View();
        }

        public ActionResult HoSoThiDua()
        {
            return View();
        }
        public JsonResult GetDanhSachThiDua()
        {
            int capPhatDong = 0;
            try
            {
                capPhatDong = int.Parse(Request["capPhatDong"]);
            }
            catch (Exception)
            {
                capPhatDong = 0;
                //throw;
            }
            string soHieu = Request["soHieu"];
            string ngayPhatDong_TuNgay = Request["ngayPhatDong_TuNgay"];
            string ngayPhatDong_DenNgay = Request["ngayPhatDong_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);

            return Json(new { data = _thiduaService.getDanhSachThiDua(capPhatDong, soHieu, ngayPhatDong_TuNgay, ngayPhatDong_DenNgay, trangThai) }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult getThiDuaInfo()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"]);
            return Json(_thiduaService.getThiDuaByID(idThiDua), JsonRequestBehavior.AllowGet);
        }

        public int CapNhatThiDua()
        {
            int idThiDua = int.Parse(Request.Form.Get("id") != "" ? Request.Form.Get("id") : "");
            int capPhatDong = int.Parse(Request.Form.Get("capPhatDong"));
            int idDmThiDua = int.Parse(Request.Form.Get("idDmThiDua"));
            int kieuThiDua = int.Parse(Request.Form.Get("kieuThiDua"));
            string soHieu = Request.Form.Get("soHieu");
            string ngayPhatDong = (Request.Form.Get("ngayPhatDong") != null ? Request.Form.Get("ngayPhatDong") : "");

            string ngayKetThuc = (Request.Form.Get("ngayKetThuc") != null ? Request.Form.Get("ngayKetThuc") : "");
            string noiDungPhatDong = Request.Form.Get("noiDungPhatDong");
            string chiTieuThiDua = Request.Form.Get("chiTieuThiDua");
            string hinhThucKhenThuong = Request.Form.Get("hinhThucKhenThuong");
            byte doiTuongThamGia = byte.Parse(Request.Form.Get("doiTuongThamGia"));
            string nguoiKy = Request.Form.Get("nguoiKy");

            string output_filedinhkem = "";

            if (idThiDua == 0) ///Thêm mới  thi đua
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
                    qltdkt_thidua _newObj = new qltdkt_thidua
                    {
                        phatDongId = capPhatDong,
                        idDmThiDua = idDmThiDua,
                        quyetDinh = output_filedinhkem,
                        kieuThiDua = kieuThiDua,
                        soHieu = soHieu,
                        ngayPhatDong = DateTime.ParseExact(ngayPhatDong, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ngayKetThuc = DateTime.ParseExact(ngayKetThuc, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        noiDungPhatDong = noiDungPhatDong,
                        chiTieuThiDua = chiTieuThiDua,
                        hinhThucKhenThuong = hinhThucKhenThuong,
                        doiTuongThamGia = doiTuongThamGia,
                        nguoiKy = nguoiKy,
                        ngayTao = DateTime.Now,
                        trangThai = 2,
                        daXoa = false
                    };
                    _entities.qltdkt_thidua.Add(_newObj);
                    _entities.SaveChanges();
                    return 0;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
            }
            else ///Cập nhật thi đua
            {
                qltdkt_thidua _update = _entities.qltdkt_thidua.Find(idThiDua);
                string dulieu_dinhkem = _update.quyetDinh;
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

                _update.quyetDinh = output_filedinhkem;
                _update.phatDongId = capPhatDong;
                _update.kieuThiDua = kieuThiDua;
                _update.soHieu = soHieu;
                _update.idDmThiDua = idDmThiDua;
                _update.ngayPhatDong = DateTime.ParseExact(ngayPhatDong, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _update.ngayKetThuc = DateTime.ParseExact(ngayKetThuc, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                _update.noiDungPhatDong = noiDungPhatDong;
                _update.chiTieuThiDua = chiTieuThiDua;
                _update.hinhThucKhenThuong = hinhThucKhenThuong;
                _update.doiTuongThamGia = doiTuongThamGia;
                _update.nguoiKy = nguoiKy;
                _update.ngayCapNhat = DateTime.Now;
                _entities.SaveChanges();
                return 2;
            }
        }

        [ValidateInput(false)]
        public int CapNhatDangKyThiDua()
        {
            string output_filedinhkem = "";
            int idDangKyThiDua = int.Parse(Request.Form.Get("id") != null ? Request.Form.Get("id") : "0");
            int thiDuaId = int.Parse(Request.Form.Get("thiDuaId"));
            int donViDangKyId = int.Parse(Request.Form.Get("donViDangKyId"));
            string noiDungDangKy = Request.Unvalidated.Form.Get("noiDungDangKy");
            DateTime ngayDangKy = DateTime.Parse(Request.Form.Get("ngayDangKy") != "" ? Request.Form.Get("ngayDangKy") : "");
            byte xepHangThiDua = byte.Parse(Request.Form.Get("xepHangThiDua"));
            byte trangThaiThiDua = byte.Parse(Request.Form.Get("trangThaiThiDua"));
            string nhanXetChung = Request.Unvalidated.Form.Get("nhanXetChung");
            string nguoiKyDanhGia = Request.Unvalidated.Form.Get("nguoiKyDanhGia");
            string noiDungDanhGia = Request.Unvalidated.Form.Get("noiDungDanhGia");
            string lsIdCaNhanDangKy = Request.Form.Get("lsIdCaNhanDangKy");
            string lsCaNhanDangKy = Request.Form.Get("lsCaNhanDangKy");
            string lsXepHang = Request.Form.Get("lsXepHang");
            string lsNhanXet = Request.Form.Get("lsNhanXet");
            if (idDangKyThiDua == 0) //Thêm mới đăng ký thi đua 
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
                    string output_canhandangky = "";
                    if (lsIdCaNhanDangKy.Length > 0)
                    {
                        List<int> lsDonViAll = _thiduaService.getDonViChildByDonViId(donViDangKyId);
                        var lsNhanVien = (from item in _entities.qltdkt_dm_nhanvien where lsDonViAll.Contains(item.donViId) && item.maNhanVien != "cntt_01" && item.daXoa == false select item).ToList();
                        List<chiTietDangKyThiDua> lsChiTietCaNhanDangKy = new List<chiTietDangKyThiDua>();
                        string[] splId = lsIdCaNhanDangKy.Split(';');
                        string[] splNoiDungDangKy = lsCaNhanDangKy.Split(';');
                        string[] splXepHangDangKy = lsXepHang.Split(';');
                        string[] splNhanXet = lsNhanXet.Split(';');
                        for (int i = 0; i < lsNhanVien.Count; i++)
                        {
                            bool isCheck = false;
                            for (int j = 0; j < splId.Length; j++)
                            {
                                qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);

                                if (int.Parse(splId[j]) == lsNhanVien[i].id)
                                {
                                    chiTietDangKyThiDua _newObj = new chiTietDangKyThiDua
                                    {
                                        idNhanVien = int.Parse(splId[j]),
                                        tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(int.Parse(splId[j])).hoTen,
                                        isDangKy = true,
                                        xepHang = int.Parse(splXepHangDangKy[j]),
                                        noiDungDangKy = splNoiDungDangKy[j],
                                        nhanXet = splNhanXet[j],
                                        tenDonVi = _entities.qltdkt_dm_donvi.Find(_objNv.donViId).tenDonVi
                                    };
                                    lsChiTietCaNhanDangKy.Add(_newObj);
                                    isCheck = true;
                                    break;
                                }
                            }
                            if (!isCheck)
                            {
                                qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);

                                chiTietDangKyThiDua _newObj = new chiTietDangKyThiDua
                                {
                                    idNhanVien = lsNhanVien[i].id,
                                    tenNhanVien = lsNhanVien[i].hoTen,
                                    isDangKy = false,
                                    xepHang = 0,
                                    noiDungDangKy = "",
                                    nhanXet = "",
                                    tenDonVi = _entities.qltdkt_dm_donvi.Find(_objNv.donViId).tenDonVi

                                };
                                lsChiTietCaNhanDangKy.Add(_newObj);
                            }
                        }

                        output_canhandangky = JsonConvert.SerializeObject(lsChiTietCaNhanDangKy);
                    }
                    qltdkt_dangkythidua _objNew = new qltdkt_dangkythidua
                    {
                        thiDuaId = thiDuaId,
                        donViDangKyId = donViDangKyId,
                        ngayDangKy = ngayDangKy,
                        trangThaiThiDua = trangThaiThiDua,
                        xepHangThiDua = xepHangThiDua,
                        fileDinhKem = output_filedinhkem,
                        nhanXetChung = nhanXetChung,
                        nguoiKyDanhGia = nguoiKyDanhGia,
                        noiDungDanhGia = noiDungDanhGia,
                        noiDungDangKy = noiDungDangKy,
                        caNhanDangKy_KetQua = output_canhandangky,
                        ngayTao = DateTime.Now,
                        daXoa = false
                    };
                    _entities.qltdkt_dangkythidua.Add(_objNew);
                    _entities.SaveChanges();
                    return 0;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
            }
            else //Cập nhật đăng ký thi đua
            {
                qltdkt_dangkythidua _update = _entities.qltdkt_dangkythidua.Find(idDangKyThiDua);
                string duLieuDinhKem = _update.fileDinhKem;
                if (duLieuDinhKem.Trim() == "")
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
                            var ls = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(duLieuDinhKem);
                            string currentYear = DateTime.Now.Year.ToString();
                            string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                            if (!Directory.Exists(fpath))
                            {
                                Directory.CreateDirectory(fpath);
                            }
                            HttpFileCollectionBase files = Request.Files;
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
                                bool ischeck = true;
                                for (int j = 0; j < ls.Count; j++)
                                {
                                    if (ls[j].Key == fname && ls[j].Value == ftype)
                                    {
                                        ischeck = false;
                                        break;
                                    }
                                }
                                if (ischeck)
                                {
                                    ls.Add(new KeyValuePair<string, string>(fname, ftype));
                                }
                            }
                            output_filedinhkem = JsonConvert.SerializeObject(ls);
                        }
                        catch (Exception)
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        output_filedinhkem = duLieuDinhKem;
                    }
                }
                _update.fileDinhKem = output_filedinhkem;
                _update.ngayDangKy = ngayDangKy;
                _update.noiDungDangKy = noiDungDangKy;
                _update.xepHangThiDua = xepHangThiDua;
                _update.trangThaiThiDua = trangThaiThiDua;
                _update.nhanXetChung = nhanXetChung;
                _update.nguoiKyDanhGia = nguoiKyDanhGia;
                _update.noiDungDanhGia = noiDungDanhGia;
                _update.ngayCapNhat = DateTime.Now;
                string output_canhandangky = "";
                if (lsIdCaNhanDangKy.Length > 0)
                {
                    List<int> lsDonViAll = _thiduaService.getDonViChildByDonViId(donViDangKyId);

                    var lsNhanVien = (from item in _entities.qltdkt_dm_nhanvien where lsDonViAll.Contains(item.donViId) && item.maNhanVien != "cntt_01" && item.daXoa == false select item).ToList();
                    List<chiTietDangKyThiDua> lsChiTietCaNhanDangKy = new List<chiTietDangKyThiDua>();
                    string[] splId = lsIdCaNhanDangKy.Split(';');
                    string[] splNoiDungDangKy = lsCaNhanDangKy.Split(';');
                    string[] splXepHangDangKy = lsXepHang.Split(';');
                    string[] splNhanXet = lsNhanXet.Split(';');
                    for (int i = 0; i < lsNhanVien.Count; i++)
                    {

                        bool isCheck = false;
                        for (int j = 0; j < splId.Length; j++)
                        {
                            qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);

                            if (int.Parse(splId[j]) == lsNhanVien[i].id)
                            {
                                chiTietDangKyThiDua _newObj = new chiTietDangKyThiDua
                                {
                                    idNhanVien = int.Parse(splId[j]),
                                    tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(int.Parse(splId[j])).hoTen,
                                    isDangKy = true,
                                    xepHang = int.Parse(splXepHangDangKy[j]),
                                    noiDungDangKy = splNoiDungDangKy[j],
                                    nhanXet = splNhanXet[j],
                                    tenDonVi = _entities.qltdkt_dm_donvi.Find(_objNv.donViId).tenDonVi

                                };
                                lsChiTietCaNhanDangKy.Add(_newObj);
                                isCheck = true;
                                break;
                            }
                        }
                        if (!isCheck)
                        {
                            qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);
                            chiTietDangKyThiDua _newObj = new chiTietDangKyThiDua
                            {
                                idNhanVien = lsNhanVien[i].id,
                                tenNhanVien = lsNhanVien[i].hoTen,
                                isDangKy = false,
                                xepHang = 0,
                                noiDungDangKy = "",
                                nhanXet = "",
                                tenDonVi = _entities.qltdkt_dm_donvi.Find(_objNv.donViId).tenDonVi

                            };
                            lsChiTietCaNhanDangKy.Add(_newObj);
                        }
                    }
                    /*for (int i = 0; i < splId.Length; i++)
                    {
                        chiTietDangKyThiDua _newObj = new chiTietDangKyThiDua
                        {
                            idNhanVien = int.Parse(splId[i]),
                            tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(int.Parse(splId[i])).hoTen,
                            isDangKy = true,
                            xepHang = int.Parse(splXepHangDangKy[i]),
                            noiDungDangKy = splNoiDungDangKy[i],
                            nhanXet = splNhanXet[i]
                        };
                        lsChiTietCaNhanDangKy.Add(_newObj);
                    }*/
                    output_canhandangky = JsonConvert.SerializeObject(lsChiTietCaNhanDangKy);
                }
                _update.caNhanDangKy_KetQua = output_canhandangky;
                _entities.SaveChanges();
                return 2;
            }
        }

        public JsonResult LoadTreeDonVi()
        {
            return Json(_util.getTreeCaNhan(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadDataHoSoThiDua()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"]);
            return Json(new { data = _entities.qltdkt_hosothidua.FirstOrDefault(x => x.thiDuaId == idThiDua) }, JsonRequestBehavior.AllowGet);
        }

        public bool DeleteThiDua()
        {
            int idThiDua = int.Parse(Request["idThiDua"]);
            try
            {
                qltdkt_thidua _delete = _entities.qltdkt_thidua.Find(idThiDua);
                if (_delete != null)
                {
                    _delete.daXoa = true;
                }
                var _dkThiDua = _entities.qltdkt_dangkythidua.Where(x => x.thiDuaId == idThiDua).ToList();
                if (_dkThiDua != null && _dkThiDua.Count > 0)
                {
                    for (int i = 0; i < _dkThiDua.Count; i++)
                    {
                        _dkThiDua[i].daXoa = true;
                    }
                }
                var _bcThiDua = _entities.qltdkt_baocaothidua.Where(x => x.idThiDua == idThiDua).ToList();
                if (_bcThiDua != null && _bcThiDua.Count > 0)
                {
                    for (int i = 0; i < _bcThiDua.Count; i++)
                    {
                        _bcThiDua[i].daXoa = true;
                    }
                }
                var _hsThiDua = _entities.qltdkt_hosothidua.Where(x => x.thiDuaId == idThiDua).ToList();
                if (_hsThiDua != null && _hsThiDua.Count > 0)
                {
                    for (int i = 0; i < _hsThiDua.Count; i++)
                    {
                        _hsThiDua[i].daXoa = true;
                    }
                }
                _entities.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public int CapNhatHoSoThiDua()
        {
            string output_filedinhkem = "";
            int idThiDua = int.Parse(Request.Form.Get("idThiDua") != "" ? Request.Form.Get("idThiDua") : "0");
            int idHoSoThiDua = int.Parse(Request.Form.Get("idHoSoThiDua"));
            byte kieuHoSo = byte.Parse(Request.Form.Get("kieuHoSo") != "" ? Request.Form.Get("kieuHoSo") : "0");
            string tenHoSoThiDua = Request.Form.Get("tenHoSoThiDua");
            string tenThiDua = Request.Form.Get("tenThiDua");
            string soHieu = Request.Form.Get("soHieu");
            DateTime ngayPhatDong = DateTime.Parse(Request.Form.Get("ngayPhatDong"));


            string lsCN = Request.Form.Get("lsCN");
            string lsTT = Request.Form.Get("lsTT");
            string strFileDinhKem = Request.Unvalidated.Form.Get("fileDinhKem");
            List<danhSachVanBan> lsDsVB = new List<danhSachVanBan>();
            if (strFileDinhKem.Length > 0)
            {
                string[] spl = strFileDinhKem.Split(',');
                for (int i = 0; i < spl.Length; i++)
                {
                    if (spl[i] != "")
                    {
                        int id_dm_vb = int.Parse(spl[i]);
                        qltdkt_dm_vanbanhd _dmBV = _entities.qltdkt_dm_vanbanhd.Find(id_dm_vb);
                        danhSachVanBan _newVB = new danhSachVanBan
                        {
                            id = _dmBV.id,
                            fileVB = _dmBV.fileVB,
                            tenVB = _dmBV.tenVB
                        };
                        lsDsVB.Add(_newVB);
                    }

                }
            }

            if (idHoSoThiDua == 0) //Thêm mới hồ sơ thi đua
            {
                if (idThiDua == 0) //Tạo hồ sơ thi đua đột xuất, không dựa vào thi đua
                {
                    List<lsDonViCaNhan> _lsDonViCaNhan = new List<lsDonViCaNhan>();
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
                    if (lsCN.Length > 0)
                    {
                        string[] spl_CN = lsCN.Split(';');
                        if (spl_CN.Length > 0)
                        {
                            for (int j = 0; j < spl_CN.Length; j++)
                            {
                                if (spl_CN[j] != "")
                                {
                                    lsDonViCaNhan _new = new lsDonViCaNhan
                                    {
                                        id = int.Parse(spl_CN[j]),
                                        name = _thiduaService.getTenByIdNhanVienTT(int.Parse(spl_CN[j]), 2),
                                        type = 2
                                    };
                                    _lsDonViCaNhan.Add(_new);
                                }
                            }
                        }
                    }
                    if (lsTT.Length > 0)
                    {
                        string[] spl_TT = lsTT.Split(';');
                        if (spl_TT.Length > 0)
                        {
                            for (int j = 0; j < spl_TT.Length; j++)
                            {
                                if (spl_TT[j] != "")
                                {
                                    lsDonViCaNhan _new = new lsDonViCaNhan
                                    {
                                        id = int.Parse(spl_TT[j]),
                                        name = _thiduaService.getTenByIdNhanVienTT(int.Parse(spl_TT[j]), 1),
                                        type = 1
                                    };
                                    _lsDonViCaNhan.Add(_new);
                                }
                            }
                        }
                    }
                    try
                    {
                        qltdkt_hosothidua _new = new qltdkt_hosothidua
                        {
                            thiDuaId = 0,
                            tenThiDua = tenThiDua,
                            ngayPhatDong = ngayPhatDong,
                            soHieu = soHieu,
                            tenHoSoThiDua = tenHoSoThiDua,
                            kieuThiDua = kieuHoSo,
                            fileDinhKem = JsonConvert.SerializeObject(lsDsVB),
                            chiTietBaoCaoThanhTich = JsonConvert.SerializeObject(_lsDonViCaNhan),
                            daXoa = false,
                            toTrinh = "Son nguyen da code!",
                            ngayTao = DateTime.Now,
                            fileBaoCaoThanhTich = output_filedinhkem
                        };
                        _entities.qltdkt_hosothidua.Add(_new);
                        _entities.SaveChanges();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        return -1;
                        throw;
                    }
                }
                else //Tạo hồ sơ thi đua dựa trên thi đua đã được tạo
                {
                    chiTietThiDuaBaoCao _chiTietBaoCao = new chiTietThiDuaBaoCao();
                    qltdkt_thidua _objThiDua = _entities.qltdkt_thidua.Find(idThiDua);
                    _chiTietBaoCao = _bcService.getChiTietThiDuaBaoCao(idThiDua);
                    try
                    {
                        qltdkt_hosothidua _new = new qltdkt_hosothidua
                        {
                            thiDuaId = idThiDua,
                            daXoa = false,
                            ngayTao = DateTime.Now,
                            tenHoSoThiDua = tenHoSoThiDua,
                            tenThiDua = _entities.qltdkt_dm_thidua.Find(_objThiDua.idDmThiDua).tenThiDua,
                            soHieu = _objThiDua.soHieu,
                            ngayPhatDong = _objThiDua.ngayPhatDong,
                            kieuThiDua = (byte)_objThiDua.kieuThiDua,
                            toTrinh = "Son nguyen da code!",
                            fileDinhKem = JsonConvert.SerializeObject(lsDsVB),
                            fileBaoCaoThanhTich = _chiTietBaoCao.lsFileBaoCao,
                            chiTietBaoCaoThanhTich = JsonConvert.SerializeObject(_chiTietBaoCao.dsDonViCaNhan)
                        };
                        _entities.qltdkt_hosothidua.Add(_new);
                        _entities.SaveChanges();
                        return 0;
                    }
                    catch (Exception)
                    {
                        return -1;
                        throw;
                    }

                }
            }
            else //Cập nhật hồ sơ thi đua
            {
                qltdkt_hosothidua _update = _entities.qltdkt_hosothidua.Find(idHoSoThiDua);
                string duLieuDinhKem = _update.fileBaoCaoThanhTich;
                string fileDinhKem = _update.fileDinhKem;
                List<lsDonViCaNhan> _lsDonViCaNhan = new List<lsDonViCaNhan>();
                if (idThiDua == 0)
                {
                    if (duLieuDinhKem.Trim() == "")
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
                                var ls = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(duLieuDinhKem);
                                string currentYear = DateTime.Now.Year.ToString();
                                string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                                if (!Directory.Exists(fpath))
                                {
                                    Directory.CreateDirectory(fpath);
                                }
                                HttpFileCollectionBase files = Request.Files;
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
                                    bool ischeck = true;
                                    for (int j = 0; j < ls.Count; j++)
                                    {
                                        if (ls[j].Key == fname && ls[j].Value == ftype)
                                        {
                                            ischeck = false;
                                            break;
                                        }
                                    }
                                    if (ischeck)
                                    {
                                        ls.Add(new KeyValuePair<string, string>(fname, ftype));
                                    }
                                }
                                output_filedinhkem = JsonConvert.SerializeObject(ls);
                            }
                            catch (Exception)
                            {
                                return -1;
                            }
                        }
                        else
                        {
                            output_filedinhkem = duLieuDinhKem;
                        }
                    }
                    if (lsCN.Length > 0)
                    {
                        string[] spl_CN = lsCN.Split(';');
                        if (spl_CN.Length > 0)
                        {
                            for (int j = 0; j < spl_CN.Length; j++)
                            {
                                if (spl_CN[j] != "")
                                {
                                    lsDonViCaNhan _new = new lsDonViCaNhan
                                    {
                                        id = int.Parse(spl_CN[j]),
                                        name = _thiduaService.getTenByIdNhanVienTT(int.Parse(spl_CN[j]), 2),
                                        type = 2
                                    };
                                    _lsDonViCaNhan.Add(_new);
                                }
                            }
                        }
                    }
                    if (lsTT.Length > 0)
                    {
                        string[] spl_TT = lsTT.Split(';');
                        if (spl_TT.Length > 0)
                        {
                            for (int j = 0; j < spl_TT.Length; j++)
                            {
                                if (spl_TT[j] != "")
                                {
                                    lsDonViCaNhan _new = new lsDonViCaNhan
                                    {
                                        id = int.Parse(spl_TT[j]),
                                        name = _thiduaService.getTenByIdNhanVienTT(int.Parse(spl_TT[j]), 1),
                                        type = 1
                                    };
                                    _lsDonViCaNhan.Add(_new);
                                }
                            }
                        }
                    }

                    _update.fileBaoCaoThanhTich = output_filedinhkem;
                    _update.tenHoSoThiDua = tenHoSoThiDua;
                    _update.fileDinhKem = JsonConvert.SerializeObject(lsDsVB);
                    _update.kieuThiDua = kieuHoSo;
                    _update.chiTietBaoCaoThanhTich = JsonConvert.SerializeObject(_lsDonViCaNhan);
                    _entities.SaveChanges();
                    return 2;
                }
                else
                {
                    _update.tenHoSoThiDua = tenHoSoThiDua;
                    _update.fileDinhKem = JsonConvert.SerializeObject(lsDsVB);

                    _entities.SaveChanges();
                    return 2;
                }
            }
        }

        public JsonResult getDanhSachHoSoThiDua()
        {
            int idHoSoThiDua = int.Parse(Request.QueryString["idHoSoThiDua"] != "" ? Request.QueryString["idHoSoThiDua"] : "0");

            return Json(new { data = _thiduaService.getDanhSachHoSoThiDua(idHoSoThiDua) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetHoSoThiDuaById()
        {
            int idHoSoThiDua = int.Parse(Request["idHoSoThiDua"]);
            return Json(new { data = _entities.qltdkt_hosothidua.Find(idHoSoThiDua) }, JsonRequestBehavior.AllowGet);
        }

        public bool DeleteBaoCaoThiDua()
        {
            int idHoSoThiDua = int.Parse(Request["idHoSoThiDua"]);
            qltdkt_hosothidua _delete = _entities.qltdkt_hosothidua.Find(idHoSoThiDua);
            if (_delete != null)
            {
                _delete.daXoa = true;
                _entities.SaveChanges();
                return true;
            }

            return false;
        }

        public JsonResult getDanhSachDonViDaDK()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"]);
            return Json(new { data = _thiduaService.getDanhSachCacDonViDaDKThiDua(idThiDua) }, JsonRequestBehavior.AllowGet);
        }

        public string getTenByIdNhanVienTT()
        {
            int idTTCN = int.Parse(Request.QueryString["idTTCN"]);
            int type = int.Parse(Request.QueryString["type"]);
            return _thiduaService.getTenByIdNhanVienTT(idTTCN, type);
        }

        public JsonResult loadDanhSachVanBan()
        {
            var data = _entities.qltdkt_dm_vanbanhd.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getThanhTichCaNhanTapThe()
        {
            int idThiDua = int.Parse(Request.QueryString["idThiDua"]);
            return Json(new { data = _thiduaService.getChiTietBaoCaoTT(idThiDua) }, JsonRequestBehavior.AllowGet);
        }


        public bool DeleteDonViDangKy()
        {
            int idThiDua = int.Parse(Request["idThiDua"]);
            int idDonVi = int.Parse(Request["idDonVi"]);

            qltdkt_dangkythidua _delete = _entities.qltdkt_dangkythidua.FirstOrDefault(x => x.donViDangKyId == idDonVi && x.thiDuaId == idThiDua);
            if (_delete != null)
            {
                _entities.qltdkt_dangkythidua.Remove(_delete);

            }
            var bctd = _entities.qltdkt_baocaothidua.Where(y => y.idThiDua == idThiDua && y.daXoa == false).ToList();
            if (bctd != null && bctd.Count > 0)
            {
                for (int i = 0; i < bctd.Count; i++)
                {
                    if (_delete.thiDuaId == bctd[i].idThiDua)
                    {
                        bctd[i].daXoa = true;
                    }

                }
            }
            var hstd = _entities.qltdkt_hosothidua.Where(z => z.thiDuaId == idThiDua && z.daXoa == false).ToList();
            if (hstd != null && hstd.Count > 0)
            {
                for (int j = 0; j < hstd.Count; j++)
                {
                    if (_delete.thiDuaId == hstd[j].thiDuaId)
                    {
                        hstd[j].daXoa = true;
                    }

                }
            }
            _entities.SaveChanges();
            return true;
        }

        public bool ProcessThiDua()
        {
            int idThiDua = int.Parse(Request["idThiDua"]);
            int type = int.Parse(Request["type"]);
            qltdkt_thidua _update = _entities.qltdkt_thidua.Find(idThiDua);
            if (_update != null)
            {
                if (type == 0)
                {
                    _update.trangThai = 3;
                }
                else
                {
                    _update.trangThai = 2;
                }
                _entities.SaveChanges();
                return true;
            }
            return true;
        }
        public ActionResult DownloadFile()
        {
            string fullpath = Request["fullpath"];
            string strfull = fullpath.Replace(",", "\\");
            string file = Request["file"];
            string type = Request["type"];
            return File(strfull, type, file);
        }

        public JsonResult getDsCapPhatDong()
        {
            return Json(new { data = _entities.qltdkt_dm_donviphatdong.ToList() }, JsonRequestBehavior.AllowGet);
        }



        public JsonResult GetDmThiDuaByKieu()
        {
            int kieuThiDua = (Request.QueryString["kieuThiDua"] != "" ? int.Parse(Request.QueryString["kieuThiDua"]) : -1);
            return Json(new { data = _entities.qltdkt_dm_thidua.Where(x => x.kieuThiDua == kieuThiDua).ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDanhSachCaNhanDangKy()
        {
            int idThiDua = int.Parse(Request["idThiDua"]);
            int idDonVi = int.Parse(Request["idDonVi"]);

            return Json(new { data = _thiduaService.getChiTietDangKyThiDuaByIdThiDua2DonVi(idThiDua, idDonVi) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult getDsNhanVienByDonVi()
        {
            int idDonVi = int.Parse(Request["idDonVi"]);
            return Json(new { data = _thiduaService.getLsNhanVienByIdDonVi(idDonVi) }, JsonRequestBehavior.AllowGet);
        }

    }
}