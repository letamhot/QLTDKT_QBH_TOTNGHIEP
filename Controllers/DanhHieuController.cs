using ExcelDataReader;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLTDKT.Models;
using QLTDKT.Models.Service.danhHieuService;
using QLTDKT.Models.Service.soKyYeuService;
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
    public class DanhHieuController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private danhHieuService _danhhieuService = new danhHieuService();
        private JsTreeAccess _util = new JsTreeAccess();
        // GET: DanhHieu
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult getDanhHieuDDT()
        {
            return View();
        }
        public ActionResult getDanhHieuCM()
        {
            return View();
        }
        public ActionResult getDanhHieuCD()
        {
            return View();
        }
        [HttpPost]

        public int ImportFile()
        {

            int idDanhHieu = 0;
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

                            DataTable dt_SoHieu = SelectDistinct(dt, "column6");//Lấy dữ liệu theo cột số hiệu

                            if (dt_SoHieu.Rows.Count > 0)
                            {
                                for (int v = 0; v < dt_SoHieu.Rows.Count; v++)
                                {
                                    qltdkt_danhhieu dh = new qltdkt_danhhieu();

                                    danhSachCaNhanTapThe _ds = new danhSachCaNhanTapThe();
                                    List<dsChiTietCaNhanTapThe> lsCNTT = new List<dsChiTietCaNhanTapThe>();
                                    DataRow[] found = dt.Select("column6 = '" + dt_SoHieu.Rows[v][0] + "'");
                                    if (found.Length > 0)
                                    {
                                        dh.soHieu = found[0][6].ToString();
                                        dh.noiDung = found[0][8].ToString();
                                        dh.ngayDanhHieu = DateTime.Parse(found[0][7].ToString());
                                        dh.kieuDanhHieu = byte.Parse(found[0][1].ToString());
                                        dh.namDanhHieu = int.Parse(found[0][9].ToString());
                                        dh.idTenDanhHieu = byte.Parse(found[0][5].ToString());
                                        dh.tuNam = found[0][10].ToString();
                                        dh.denNam = found[0][11].ToString();
                                        dh.daXoa = false;
                                        dh.nguoiKy = found[0][12].ToString();
                                        dh.bophan = int.Parse(found[0][13].ToString());
                                        dh.daTraoTang = bool.Parse(found[0][14].ToString());
                                        if (found[0][15].ToString() == "0")
                                        {
                                            dh.hinhThucTraoTang = 0;
                                        }
                                        else
                                        {
                                            dh.hinhThucTraoTang = byte.Parse(found[0][15].ToString());

                                        }
                                        _entities.qltdkt_danhhieu.Add(dh);
                                        _entities.SaveChanges();
                                        idDanhHieu = dh.id;
                                    }
                                    for (int k = 0; k < found.Length; k++)
                                    {
                                        dsChiTietCaNhanTapThe _object = new dsChiTietCaNhanTapThe();
                                        _object.tenCaNhanTapThe = found[k][2].ToString();
                                        _object.loaiDanhHieu = (int)dh.kieuDanhHieu;
                                        _object.id = int.Parse(found[k][0].ToString());
                                        if (_object.loaiDanhHieu == 1)
                                        {
                                            _object.chucDanh = found[k][3].ToString();

                                        }
                                        else if (_object.loaiDanhHieu == 0)
                                        {
                                            _object.chucDanh = "";

                                        }

                                        _object.donVi = found[k][4].ToString();
                                        lsCNTT.Add(_object);
                                    }
                                    _ds.loaiDanhHieu = (int)dh.kieuDanhHieu;
                                    _ds.dsChiTietCaNhanTapThe = lsCNTT;

                                    qltdkt_danhhieu _update = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                                    _update.danhSachCaNhanTapThe = JsonConvert.SerializeObject(_ds);
                                    _entities.SaveChanges();
                                }
                            }


                        }
                    }
                }
                return 1;

            }

            catch (Exception)
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

            ExcelWorksheet sheet2 = ep.Workbook.Worksheets.Add("Danh Hiệu");
            int row = 1;


            sheet2.Cells["A" + row.ToString()].Value = "Mã cá nhân/ tập thể";
            sheet2.Cells["B" + row.ToString()].Value = "Đối tượng(0 - Tập thể; 1 - Cá nhân)";
            sheet2.Cells["C" + row.ToString()].Value = "Tên cá nhân/tập thể";
            sheet2.Cells["D" + row.ToString()].Value = "Chức danh";
            sheet2.Cells["E" + row.ToString()].Value = "Đơn vị";
            sheet2.Cells["F" + row.ToString()].Value = "ID tên danh hiệu";
            sheet2.Cells["G" + row.ToString()].Value = "Số Quyết định";
            sheet2.Cells["H" + row.ToString()].Value = "Ngày ban hành";
            sheet2.Cells["I" + row.ToString()].Value = "Nội dung danh hiệu";
            sheet2.Cells["J" + row.ToString()].Value = "Năm danh hiệu";
            sheet2.Cells["K" + row.ToString()].Value = "Từ Năm";
            sheet2.Cells["L" + row.ToString()].Value = "Đến Năm";
            sheet2.Cells["M" + row.ToString()].Value = "Người ký";
            sheet2.Cells["N" + row.ToString()].Value = "Bộ phận(1: Chuyên môn; 2: Đảng - Đoàn thể; 3: Công đoàn)";
            sheet2.Cells["O" + row.ToString()].Value = "Trạng thái trao tặng(true - Đã trao tặng; false - Chưa trao tặng)";
            sheet2.Cells["P" + row.ToString()].Value = "Hình thức trao tặng";

            sheet2.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet2.Row(row).Style.Font.Bold = true;
            sheet2.Cells[row, 1, row, 16].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 16].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 16].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 16].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            var data = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.daXoa == false).ToList();

            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Danh mục Danh hiệu thi đua");
            row = 1;
            sheet.Cells["A" + row.ToString()].Value = "Mã danh hiệu thi đua";
            sheet.Cells["B" + row.ToString()].Value = "Kiểu danh hiệu";
            sheet.Cells["C" + row.ToString()].Value = "Tên danh hiệu thi đua";

            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    {
                        sheet.Cells["A" + row.ToString()].Value = data[i].id;
                        sheet.Cells["B" + row.ToString()].Value = data[i].loaiDanhHieu;
                        sheet.Cells["C" + row.ToString()].Value = data[i].tenDanhHieuThiDua;

                    }
                    sheet.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }




            var data1 = _entities.qltdkt_dm_nhanvien.ToList();
            ExcelWorksheet sheet1 = ep.Workbook.Worksheets.Add("Danh mục nhân viên");
            row = 1;
            sheet1.Cells["A" + row.ToString()].Value = "Id nhân viên";
            sheet1.Cells["B" + row.ToString()].Value = "Tên nhân viên";
            sheet1.Cells["C" + row.ToString()].Value = "Chức danh";
            sheet1.Cells["D" + row.ToString()].Value = "Đơn vị";
            sheet1.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet1.Row(row).Style.Font.Bold = true;
            sheet1.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data1 != null && data1.Count > 0)
            {
                for (int i = 0; i < data1.Count; i++)
                {
                    {
                        sheet1.Cells["A" + row.ToString()].Value = data1[i].id;
                        sheet1.Cells["B" + row.ToString()].Value = data1[i].hoTen;
                        sheet1.Cells["C" + row.ToString()].Value = _entities.qltdkt_dm_chucvu.Find(data1[i].chucVuId).tenChucVu;
                        sheet1.Cells["D" + row.ToString()].Value = _entities.qltdkt_dm_donvi.Find(data1[i].donViId).tenDonVi;

                    }
                    sheet1.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet1.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }
            var data3 = _entities.qltdkt_dm_donvi.ToList();
            ExcelWorksheet sheet3 = ep.Workbook.Worksheets.Add("Danh mục đơn vị");
            row = 1;
            sheet3.Cells["A" + row.ToString()].Value = "Id đơn vị";
            sheet3.Cells["B" + row.ToString()].Value = "Tên đơn vị";
            sheet3.Cells["C" + row.ToString()].Value = "Tên đơn vị cha";
            sheet3.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet3.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet3.Row(row).Style.Font.Bold = true;
            sheet3.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet3.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet3.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet3.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data3 != null && data3.Count > 0)
            {
                for (int i = 0; i < data3.Count; i++)
                {
                    if (data3[i].id == 122 || data3[i].idCha == 0)
                    {
                        string tenDonViCha = _entities.qltdkt_dm_donvi.Find(data3[i].id).tenDonVi;
                        {
                            sheet3.Cells["A" + row.ToString()].Value = data3[i].id;
                            sheet3.Cells["B" + row.ToString()].Value = data3[i].tenDonVi;

                            sheet3.Cells["C" + row.ToString()].Value = tenDonViCha;

                        }
                    }
                    else
                    {
                        int idCha = _entities.qltdkt_dm_donvi.Find(data3[i].id).idCha;
                        string tenDonViCha = _entities.qltdkt_dm_donvi.Find(idCha).tenDonVi;
                        {
                            sheet3.Cells["A" + row.ToString()].Value = data3[i].id;
                            sheet3.Cells["B" + row.ToString()].Value = data3[i].tenDonVi;

                            sheet3.Cells["C" + row.ToString()].Value = tenDonViCha;

                        }
                    }


                    sheet3.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet3.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet3.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet3.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }
            var data4 = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.daXoa == false).ToList();

            ExcelWorksheet sheet4 = ep.Workbook.Worksheets.Add("Danh mục Hình thức trao tặng");
            row = 1;
            sheet4.Cells["A" + row.ToString()].Value = "Mã hình thức trao tặng";
            sheet4.Cells["B" + row.ToString()].Value = "Tên hình thức trao tặng";
            sheet4.Cells["C" + row.ToString()].Value = "Tên bộ phận";
            sheet4.Cells["D" + row.ToString()].Value = "Loại khen thưởng(0:tập thể / 1: cá nhân)";

            sheet4.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet4.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet4.Row(row).Style.Font.Bold = true;
            sheet4.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet4.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet4.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet4.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data4 != null && data4.Count > 0)
            {
                for (int i = 0; i < data4.Count; i++)
                {
                    {
                        sheet4.Cells["A" + row.ToString()].Value = data4[i].id;
                        sheet4.Cells["B" + row.ToString()].Value = data4[i].tenHinhThucKhenThuong;
                        sheet4.Cells["C" + row.ToString()].Value = data4[i].bophan;
                        sheet4.Cells["C" + row.ToString()].Value = data4[i].loaiKhenThuong;

                    }
                    sheet4.Cells[row, 1, row, 3].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet4.Cells[row, 1, row, 3].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet4.Cells[row, 1, row, 3].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet4.Cells[row, 1, row, 3].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }


            sheet2.Cells["A:AZ"].AutoFitColumns();
            sheet.Cells["A:AZ"].AutoFitColumns();
            sheet1.Cells["A:AZ"].AutoFitColumns();
            sheet3.Cells["A:AZ"].AutoFitColumns();
            sheet4.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "danhMuc.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();

        }
        public JsonResult GetDanhSachDanhHieu()
        {
            var a = Request["idTenDanhHieu"];
            var b = Request["capKyKhenThuong"];
            int idTenDanhHieu = Request["idTenDanhHieu"] != "" ? int.Parse(Request["idTenDanhHieu"]) : 0;
            int capKyKhenThuong = Request["capKyKhenThuong"] != "" ? int.Parse(Request["capKyKhenThuong"]) : 0;
            string soHieu = Request["soHieu"];
            string ngayDanhHieu_TuNgay = Request["ngayDanhHieu_TuNgay"];
            string ngayDanhHieu_DenNgay = Request["ngayDanhHieu_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int namDanhHieu = int.Parse(Request["namDanhHieu"]);
            int bophan = int.Parse(Request["bophan"]);
            return Json(new { data = _danhhieuService.getDanhSachDanhHieu(idTenDanhHieu, bophan, capKyKhenThuong, soHieu, ngayDanhHieu_TuNgay, ngayDanhHieu_DenNgay, trangThai, namDanhHieu) }, JsonRequestBehavior.AllowGet);
        }

        /*Danh sách danh hiệu công đoàn*/
        public JsonResult GetDanhSachDanhHieuCD()
        {
            var a = Request["idTenDanhHieu"];
            var b = Request["capKyKhenThuong"];
            int idTenDanhHieu = Request["idTenDanhHieu"] != "" ? int.Parse(Request["idTenDanhHieu"]) : 0;
            int capKyKhenThuong = Request["capKyKhenThuong"] != "" ? int.Parse(Request["capKyKhenThuong"]) : 0;
            string soHieu = Request["soHieu"];
            string ngayDanhHieu_TuNgay = Request["ngayDanhHieu_TuNgay"];
            string ngayDanhHieu_DenNgay = Request["ngayDanhHieu_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int namDanhHieu = int.Parse(Request["namDanhHieu"]);
            int bophan = 3;
            return Json(new { data = _danhhieuService.getDanhSachDanhHieu(idTenDanhHieu, bophan, capKyKhenThuong, soHieu, ngayDanhHieu_TuNgay, ngayDanhHieu_DenNgay, trangThai, namDanhHieu) }, JsonRequestBehavior.AllowGet);
        }
        /*Danh sách danh hiệu đảng đoàn thể*/

        public JsonResult GetDanhSachDanhHieuDDT()
        {
            var a = Request["idTenDanhHieu"];
            var b = Request["capKyKhenThuong"];
            int idTenDanhHieu = Request["idTenDanhHieu"] != "" ? int.Parse(Request["idTenDanhHieu"]) : 0;
            int capKyKhenThuong = Request["capKyKhenThuong"] != "" ? int.Parse(Request["capKyKhenThuong"]) : 0;
            string soHieu = Request["soHieu"];
            string ngayDanhHieu_TuNgay = Request["ngayDanhHieu_TuNgay"];
            string ngayDanhHieu_DenNgay = Request["ngayDanhHieu_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int namDanhHieu = int.Parse(Request["namDanhHieu"]);
            int bophan = 2;
            return Json(new { data = _danhhieuService.getDanhSachDanhHieu(idTenDanhHieu, bophan, capKyKhenThuong, soHieu, ngayDanhHieu_TuNgay, ngayDanhHieu_DenNgay, trangThai, namDanhHieu) }, JsonRequestBehavior.AllowGet);
        }
        public int CapNhatDanhHieu()
        {
            int idDanhHieu = int.Parse(Request.Form.Get("id") != "0" ? Request.Form.Get("id") : "0");
            byte idTenDanhHieu = byte.Parse(Request.Form.Get("idTenDanhHieu") != "0" ? Request.Form.Get("idTenDanhHieu") : "0");
            string soHieu = Request.Form.Get("soHieu");
            byte kieuDanhHieu = byte.Parse(Request.Form.Get("kieuDanhHieu"));
            string ngayDanhHieu = Request.Form.Get("ngayDanhHieu");
            string capKyKhenThuong = Request.Form.Get("capKyKhenThuong");
            string noiDungDanhHieu = Request.Form.Get("noiDungDanhHieu");
            string tuNam = Request.Form.Get("tuNam");
            string denNam = Request.Form.Get("denNam");
            string nguoiKy = Request.Form.Get("nguoiKy");
            string ghiChuTT = Request.Form.Get("ghiChuTT");
            int namDanhHieu = int.Parse(Request.Form.Get("namDanhHieu"));
            byte bophan = byte.Parse(Request.Form.Get("bophan"));
            int hinhThucTraoTang = int.Parse(Request.Form.Get("hinhThucTraoTang") != "" ? Request.Form.Get("hinhThucTraoTang") : "0");
            int daTraoTang = int.Parse(Request.Form.Get("daTraoTang"));
            bool dtt = true;
            if (daTraoTang == 0)
            {
                dtt = false;
            }

            string output_filedinhkem = "";

            string danhSachCaNhanTapThe = Request.Unvalidated.Form.Get("danhSachCaNhanTapThe");

            string fileUploadTT = "";


            if (idDanhHieu == 0) ///Thêm mới  danh hiệu
            {
                List<dsChiTietCaNhanTapThe> lsDsCNTT = new List<dsChiTietCaNhanTapThe>();
                if (danhSachCaNhanTapThe.Length > 0)
                {
                    string[] spl = danhSachCaNhanTapThe.Split(',');
                    for (int i = 0; i < spl.Length; i++)
                    {
                        if (spl[i] != "")
                        {
                            if (kieuDanhHieu == 0)
                            {
                                int id_DV = int.Parse(spl[i]);
                                qltdkt_dm_donvi _dmDV = _entities.qltdkt_dm_donvi.Find(id_DV);
                                dsChiTietCaNhanTapThe _newDV = new dsChiTietCaNhanTapThe
                                {
                                    id = _dmDV.id,
                                    ischeck = true,
                                    loaiDanhHieu = 0,
                                    tenCaNhanTapThe = _dmDV.tenDonVi,
                                    chucDanh = "",
                                    donVi = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha).tenDonVi,
                                    donViCha = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(_dmDV.idCha).idCha).tenDonVi,

                                };
                                lsDsCNTT.Add(_newDV);
                            }
                            else if (kieuDanhHieu == 1)
                            {
                                int id_NV = int.Parse(spl[i]);
                                qltdkt_dm_nhanvien _dmNV = _entities.qltdkt_dm_nhanvien.Find(id_NV);
                                dsChiTietCaNhanTapThe _newNV = new dsChiTietCaNhanTapThe
                                {
                                    id = _dmNV.id,
                                    ischeck = true,
                                    loaiDanhHieu = 1,
                                    tenCaNhanTapThe = _dmNV.hoTen,
                                    chucDanh = _entities.qltdkt_dm_chucvu.Find(_dmNV.chucVuId).tenChucVu,
                                    donVi = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId).tenDonVi,
                                    donViCha = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(_dmNV.donViId).idCha).tenDonVi,
                                };
                                lsDsCNTT.Add(_newNV);
                            }

                        }

                    }
                }
                danhSachCaNhanTapThe dsCNTT = new danhSachCaNhanTapThe();
                dsCNTT.loaiDanhHieu = kieuDanhHieu;
                dsCNTT.dsChiTietCaNhanTapThe = lsDsCNTT;

                /*var check = _entities.qltdkt_danhhieu.Where(x => x.soHieu.Trim() == soHieu.Trim() && x.daXoa == false).FirstOrDefault();
                if (check == null)
                {*/
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        string currentYearTT = DateTime.Now.Year.ToString();
                        string fpathTT = Server.MapPath("~/Uploads/" + currentYearTT + "/");
                        if (!Directory.Exists(fpathTT))
                        {
                            Directory.CreateDirectory(fpathTT);
                        }
                        HttpFileCollectionBase filesTT = Request.Files;
                        List<KeyValuePair<string, string>> lsFileDinhKemTT = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < filesTT.Count; i++)
                        {
                            string tKeyTT = filesTT.AllKeys[i];
                            string filename = Path.GetFileName(Request.Files[i].FileName);
                            HttpPostedFileBase fileTT = filesTT[i];
                            string fnameTT, ftypeTT;
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = fileTT.FileName.Split(new char[] { '\\' });
                                fnameTT = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fnameTT = fileTT.FileName;
                            }
                            fnameTT = Path.Combine(fpathTT, fnameTT);
                            ftypeTT = fileTT.ContentType;
                            if (!System.IO.File.Exists(fnameTT))
                            {
                                fileTT.SaveAs(fnameTT);
                            }
                            lsFileDinhKemTT.Add(new KeyValuePair<string, string>(fnameTT, ftypeTT));
                        }
                        fileUploadTT = JsonConvert.SerializeObject(lsFileDinhKemTT);
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
                    qltdkt_danhhieu _newObj = new qltdkt_danhhieu
                    {
                        id = idDanhHieu,
                        idTenDanhHieu = idTenDanhHieu,
                        fileGoc = output_filedinhkem,
                        soHieu = soHieu,
                        bophan = bophan,
                        hinhThucTraoTang = (byte)hinhThucTraoTang,
                        kieuDanhHieu = kieuDanhHieu,
                        danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCNTT),
                        ngayDanhHieu = DateTime.ParseExact(ngayDanhHieu, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ngayTraoTang = DateTime.ParseExact(ngayDanhHieu, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        noiDung = noiDungDanhHieu,
                        capKyKhenThuong = capKyKhenThuong,
                        ngayTao = DateTime.Now,
                        namDanhHieu = namDanhHieu,
                        tuNam = tuNam,
                        denNam = denNam,
                        ghiChuTT = ghiChuTT,
                        hinhAnhTraoTang = fileUploadTT,
                        nguoiKy = nguoiKy,
                        daTraoTang = dtt,
                        daXoa = false
                    };
                    _entities.qltdkt_danhhieu.Add(_newObj);
                    _entities.SaveChanges();
                    LuuSoKyYeu(_newObj.id, _newObj.hinhAnhTraoTang);

                    return 0;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
                /* }
             else
             {
                 return 5; //Đã tồn tại số hiệu và tên danh hiệu
             }*/
            }
            else ///Cập nhật danh hiệu
            {
                /* var check = _entities.qltdkt_danhhieu.Where(x => x.soHieu.Trim() == soHieu.Trim() && x.id != idDanhHieu && x.daXoa == false).FirstOrDefault();
                 if (check == null)
                 {*/
                List<dsChiTietCaNhanTapThe> lsDsCNTT = new List<dsChiTietCaNhanTapThe>();
                if (danhSachCaNhanTapThe.Length > 0)
                {
                    string[] spl = danhSachCaNhanTapThe.Split(',');
                    for (int i = 0; i < spl.Length; i++)
                    {
                        if (spl[i] != "")
                        {
                            if (kieuDanhHieu == 0)
                            {
                                int id_DV = int.Parse(spl[i]);
                                qltdkt_dm_donvi _dmDV = _entities.qltdkt_dm_donvi.Find(id_DV);
                                qltdkt_dm_donvi _dmDVC = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha);
                                if (_dmDVC.idCha > 0)
                                {
                                    dsChiTietCaNhanTapThe _newDV = new dsChiTietCaNhanTapThe
                                    {
                                        id = _dmDV.id,
                                        ischeck = true,
                                        loaiDanhHieu = 0,
                                        tenCaNhanTapThe = _dmDV.tenDonVi,
                                        chucDanh = "",
                                        donVi = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha).tenDonVi,
                                        donViCha = _entities.qltdkt_dm_donvi.Find(_dmDVC.idCha).tenDonVi

                                    };
                                    lsDsCNTT.Add(_newDV);

                                }
                                else
                                {
                                    dsChiTietCaNhanTapThe _newDV = new dsChiTietCaNhanTapThe
                                    {
                                        id = _dmDV.id,
                                        ischeck = true,
                                        loaiDanhHieu = 0,
                                        tenCaNhanTapThe = _dmDV.tenDonVi,
                                        chucDanh = "",
                                        donVi = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha).tenDonVi

                                    };
                                    lsDsCNTT.Add(_newDV);

                                }

                            }
                            else if (kieuDanhHieu == 1)
                            {
                                int id_NV = int.Parse(spl[i]);
                                qltdkt_dm_nhanvien _dmNV = _entities.qltdkt_dm_nhanvien.Find(id_NV);
                                dsChiTietCaNhanTapThe _newNV = new dsChiTietCaNhanTapThe
                                {
                                    id = _dmNV.id,
                                    ischeck = true,
                                    loaiDanhHieu = 1,
                                    tenCaNhanTapThe = _dmNV.hoTen,
                                    chucDanh = _entities.qltdkt_dm_chucvu.Find(_dmNV.chucVuId).tenChucVu,
                                    donVi = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId).tenDonVi,
                                    donViCha = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(_dmNV.donViId).idCha).tenDonVi,
                                };
                                lsDsCNTT.Add(_newNV);
                            }

                        }

                    }
                }
                danhSachCaNhanTapThe dsCNTT = new danhSachCaNhanTapThe();
                dsCNTT.loaiDanhHieu = kieuDanhHieu;
                dsCNTT.dsChiTietCaNhanTapThe = lsDsCNTT;
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        string currentYearTT = DateTime.Now.Year.ToString();
                        string fpathTT = Server.MapPath("~/Uploads/" + currentYearTT + "/");
                        if (!Directory.Exists(fpathTT))
                        {
                            Directory.CreateDirectory(fpathTT);
                        }
                        HttpFileCollectionBase filesTT = Request.Files;
                        List<KeyValuePair<string, string>> lsFileDinhKemTT = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < filesTT.Count; i++)
                        {
                            string tKeyTT = filesTT.AllKeys[i];
                            string filename = Path.GetFileName(Request.Files[i].FileName);
                            HttpPostedFileBase fileTT = filesTT[i];
                            string fnameTT, ftypeTT;
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = fileTT.FileName.Split(new char[] { '\\' });
                                fnameTT = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fnameTT = fileTT.FileName;
                            }
                            fnameTT = Path.Combine(fpathTT, fnameTT);
                            ftypeTT = fileTT.ContentType;
                            if (!System.IO.File.Exists(fnameTT))
                            {
                                fileTT.SaveAs(fnameTT);
                            }
                            lsFileDinhKemTT.Add(new KeyValuePair<string, string>(fnameTT, ftypeTT));
                        }
                        fileUploadTT = JsonConvert.SerializeObject(lsFileDinhKemTT);
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
                    fileUploadTT = _entities.qltdkt_danhhieu.Find(idDanhHieu).hinhAnhTraoTang;
                    output_filedinhkem = _entities.qltdkt_danhhieu.Find(idDanhHieu).fileGoc;
                }
                try
                {
                    var dh = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                    dh.id = idDanhHieu;

                    dh.idTenDanhHieu = idTenDanhHieu;
                    if (dh.fileGoc == "")
                    {
                        dh.fileGoc = output_filedinhkem;

                    }
                    else
                    {
                        output_filedinhkem = dh.fileGoc;
                    }
                    dh.soHieu = soHieu;
                    dh.kieuDanhHieu = kieuDanhHieu;
                    dh.hinhThucTraoTang = (byte)hinhThucTraoTang;
                    dh.bophan = bophan;
                    dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCNTT);

                    dh.ngayDanhHieu = DateTime.ParseExact(ngayDanhHieu, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dh.ngayTraoTang = DateTime.ParseExact(ngayDanhHieu, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dh.noiDung = noiDungDanhHieu;
                    dh.capKyKhenThuong = capKyKhenThuong;
                    dh.namDanhHieu = namDanhHieu;
                    dh.tuNam = tuNam;
                    dh.denNam = denNam;
                    dh.ghiChuTT = ghiChuTT;
                    dh.nguoiKy = nguoiKy;
                    dh.daTraoTang = dtt;
                    dh.hinhAnhTraoTang = fileUploadTT;

                    //dh.daXoa = false;
                    _entities.SaveChanges();
                    LuuSoKyYeu(dh.id, dh.hinhAnhTraoTang);

                    return 2;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
                /*}
                else
            {
                return 5; //Đã tồn tại số hiệu và tên danh hiệu
            }*/
            }
            //return 0;
        }
        public int LuuSoKyYeu(int iddh, string fileUploadTT)
        {
            var dh = _entities.qltdkt_danhhieu.Find(iddh);

            var chitietdh = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
            if (chitietdh.loaiDanhHieu == 0)
            {
                int t = 0;
                foreach (var item in chitietdh.dsChiTietCaNhanTapThe)
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {

                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idDanhHieu == iddh).FirstOrDefault();
                            if (ck == null) //check đã có mã danh hiệu này chưa, nếu chưa thì add
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 2;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                if (dh.hinhThucTraoTang != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = dh.nguoiKy;
                                //sky.idTenDanhHieu = ((byte)dh.idTenDanhHieu).ToString();
                                sky.idDanhHieu = iddh;
                                sky.namDanhHieu = (int)dh.namDanhHieu;
                                sky.hinhAnhTraoTang = fileUploadTT;

                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;
                                _entities.SaveChanges();

                            }

                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                    else //chưa có thì add
                    {
                        try
                        {
                            qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                            hsky.idDonVi = item.id;
                            hsky.idNhanVien = null;
                            hsky.ngayTao = DateTime.Now;
                            hsky.daXoa = 0;

                            soKyYeuModel sky = new soKyYeuModel();
                            sky.kieuKyYeu = 2;

                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                            if (dh.hinhThucTraoTang != 0)
                            {
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                            }
                            else
                            {
                                sky.hinhThucTraoTang = "0";
                            }
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = dh.nguoiKy;

                            sky.namDanhHieu = (int)dh.namDanhHieu;
                            sky.hinhAnhTraoTang = fileUploadTT;
                            //sky.capDanhHieu = getCapDanhHieu(dh.capDanhHieu);
                            sky.idDanhHieu = iddh;

                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }

                }
                if (t == chitietdh.dsChiTietCaNhanTapThe.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (chitietdh.loaiDanhHieu == 1)
            {
                int t = 0;
                foreach (var item in chitietdh.dsChiTietCaNhanTapThe)
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();

                    if (check != null) //nếu có thì update
                    {

                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idDanhHieu == iddh).FirstOrDefault();
                            if (ck == null)
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 2;

                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                if (dh.hinhThucTraoTang != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = dh.nguoiKy;

                                sky.namDanhHieu = (int)dh.namDanhHieu;
                                sky.idDanhHieu = iddh;
                                sky.hinhAnhTraoTang = fileUploadTT;

                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;
                                _entities.SaveChanges();
                                t++;


                            }

                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                    else //chưa có thì add
                    {

                        try
                        {
                            qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                            hsky.idDonVi = _entities.qltdkt_dm_nhanvien.Find(item.id).donViId;
                            hsky.idNhanVien = item.id;
                            hsky.ngayTao = DateTime.Now;
                            hsky.daXoa = 0;

                            soKyYeuModel sky = new soKyYeuModel();
                            sky.kieuKyYeu = 2;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                            if (dh.hinhThucTraoTang != 0)
                            {
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                            }
                            else
                            {
                                sky.hinhThucTraoTang = "0";
                            }
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = dh.nguoiKy;

                            sky.namDanhHieu = (int)dh.namDanhHieu;
                            sky.idDanhHieu = iddh;
                            sky.hinhAnhTraoTang = fileUploadTT;

                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
                if (t == chitietdh.dsChiTietCaNhanTapThe.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                int t = 0;
                foreach (var item in chitietdh.dsChiTietCaNhanTapThe)
                {
                    if (item.loaiDanhHieu == 0) // tập thể
                    {
                        var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                        if (check != null) //nếu có thì update
                        {

                            try
                            {
                                //check.idDanhHieu = iddh;
                                //_entities.SaveChanges();
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                                var ck = hsky.Where(x => x.idDanhHieu == iddh).FirstOrDefault();
                                if (ck == null) //check đã có mã danh hiệu này chưa, nếu chưa thì add
                                {
                                    soKyYeuModel sky = new soKyYeuModel();
                                    sky.kieuKyYeu = 2;
                                    sky.ngayKyYeu = DateTime.Now;
                                    sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                    if (dh.hinhThucTraoTang != 0)
                                    {
                                        sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                                    }
                                    else
                                    {
                                        sky.hinhThucTraoTang = "0";
                                    }
                                    sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                    sky.capKhenThuong = dh.nguoiKy;
                                    //sky.idTenDanhHieu = ((byte)dh.idTenDanhHieu).ToString();
                                    sky.idDanhHieu = iddh;
                                    sky.namDanhHieu = (int)dh.namDanhHieu;
                                    sky.hinhAnhTraoTang = fileUploadTT;

                                    hsky.Add(sky);
                                    check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                    check.daXoa = 0;
                                    _entities.SaveChanges();

                                }

                            }
                            catch (Exception)
                            {
                                return 0;
                            }
                        }
                        else //chưa có thì add
                        {
                            try
                            {
                                //check.idDanhHieu = iddh;
                                qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                                hsky.idDonVi = item.id;
                                hsky.idNhanVien = null;
                                hsky.ngayTao = DateTime.Now;
                                hsky.daXoa = 0;

                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 2;

                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                if (dh.hinhThucTraoTang != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = dh.nguoiKy;

                                sky.namDanhHieu = (int)dh.namDanhHieu;
                                sky.hinhAnhTraoTang = fileUploadTT;
                                //sky.capDanhHieu = getCapDanhHieu(dh.capDanhHieu);
                                sky.idDanhHieu = iddh;

                                List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                                _sky.Add(sky);

                                hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                                _entities.qltdkt_hosokyyeu.Add(hsky);
                                _entities.SaveChanges();
                            }
                            catch (Exception)
                            {
                                return 0;
                            }
                        }
                    }
                    else if (item.loaiDanhHieu == 1)// cá nhân
                    {
                        var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();

                        if (check != null) //nếu có thì update
                        {

                            try
                            {
                                //check.idDanhHieu = iddh;
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                                var ck = hsky.Where(x => x.idDanhHieu == iddh).FirstOrDefault();
                                if (ck == null)
                                {
                                    soKyYeuModel sky = new soKyYeuModel();
                                    sky.kieuKyYeu = 2;

                                    sky.ngayKyYeu = DateTime.Now;
                                    sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                    if (dh.hinhThucTraoTang != 0)
                                    {
                                        sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                                    }
                                    else
                                    {
                                        sky.hinhThucTraoTang = "0";
                                    }
                                    sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                    sky.capKhenThuong = dh.nguoiKy;

                                    sky.namDanhHieu = (int)dh.namDanhHieu;
                                    sky.idDanhHieu = iddh;
                                    sky.hinhAnhTraoTang = fileUploadTT;

                                    hsky.Add(sky);
                                    check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                    check.daXoa = 0;
                                    _entities.SaveChanges();


                                }

                            }
                            catch (Exception)
                            {
                                return 0;
                            }
                        }
                        else //chưa có thì add
                        {

                            try
                            {
                                //check.idDanhHieu = iddh;

                                qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                                hsky.idDonVi = _entities.qltdkt_dm_nhanvien.Find(item.id).donViId;
                                hsky.idNhanVien = item.id;
                                hsky.ngayTao = DateTime.Now;
                                hsky.daXoa = 0;

                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 2;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                if (dh.hinhThucTraoTang != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;

                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = dh.nguoiKy;

                                sky.namDanhHieu = (int)dh.namDanhHieu;
                                sky.idDanhHieu = iddh;
                                sky.hinhAnhTraoTang = fileUploadTT;

                                List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                                _sky.Add(sky);

                                hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                                _entities.qltdkt_hosokyyeu.Add(hsky);
                                _entities.SaveChanges();
                            }
                            catch (Exception)
                            {
                                return 0;
                            }
                        }
                    }
                }
                if (t == chitietdh.dsChiTietCaNhanTapThe.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }



        }

        public JsonResult GetDSCNTTByLoaiDanhHieu()
        {
            int kieuDanhHieu = (Request.QueryString["kieuDanhHieu"] != "" ? int.Parse(Request.QueryString["kieuDanhHieu"]) : -1);
            if (kieuDanhHieu == 1)
            {


                return Json(new { data = _danhhieuService.loadNhanVienByKieu() }, JsonRequestBehavior.AllowGet);

            }
            else if (kieuDanhHieu == 0)
            {
                return Json(new { data = _danhhieuService.loadDonViByKieu() }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult ChinhSuaDanhHieu()
        {
            int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.getDanhHieuByID(idDanhHieu);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetTenDanhHieu(int loaiDanhHieu)
        //{
        //    //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
        //    var data = _danhhieuService.GetTenDanhHieu(loaiDanhHieu);
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult getCapKyKhenThuong()
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.ListCapKyKhenThuong();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDmHinhThucKhenThuong()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int kieuDanhHieu = (Request.QueryString["kieuDanhHieu"] != "" ? int.Parse(Request.QueryString["kieuDanhHieu"]) : -1);
            int bophan = (Request.QueryString["bophan"] != "" ? int.Parse(Request.QueryString["bophan"]) : 0);
            return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == kieuDanhHieu && x.bophan == bophan && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
            //var dh = _entities.qltdkt_danhhieu.Find(idkt);
            //return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == (int)dh.kieuDanhHieu && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadNhanVienByDonVi(int idDonVi)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.loadNhanVienByDonVi(idDonVi);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadDonViByDonVi(int idDonVi)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.loadDonViByDonVi(idDonVi);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadTenDanhHieu()
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            _entities.Configuration.ProxyCreationEnabled = false;

            var data = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDmDanhHieuByKieu()
        {
            int kieuDanhHieu = (Request.QueryString["kieuDanhHieu"] != "" ? int.Parse(Request.QueryString["kieuDanhHieu"]) : -1);
            int bophan = (Request.QueryString["bophan"] != "" ? int.Parse(Request.QueryString["bophan"]) : 0);
            return Json(new { data = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.loaiDanhHieu == kieuDanhHieu && x.bophan == bophan && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadDsCaNhanTapThe(int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.loadDsCaNhanTapThe(iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ThemCaNhanTapThe(string idcn, string idtt, byte ldh, int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.ThemCaNhanTapThe(idcn, idtt, ldh, iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ThemCaNhanTapTheCache(string idcn, string idtt, int ldh, int iddh, string listCurrent)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.ThemCaNhanTapTheCache(idcn, idtt, ldh, iddh, listCurrent);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchCaNhan(string idDonVi, string searchText)
        {
            var data = _danhhieuService.searchCaNhan(idDonVi, searchText);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult searchTapThe(string idDonVi, string searchText)
        {
            var data = _danhhieuService.searchTapThe(idDonVi, searchText);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LuuCaNhan(string idcn, int ldh, int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.LuuCaNhan(idcn, ldh, iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LuuTapThe(string idtt, int ldh, int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.LuuTapThe(idtt, ldh, iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LuuTapTheCaNhan(string idtt, string idcn, int ldh, int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.LuuTapTheCaNhan(idtt, idcn, ldh, iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult xoaCaNhanTapThe(string idcntt, int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.xoaCaNhanTapThe(idcntt, iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TraoTangDanhHieu()
        {
            int data = 0;
            int iddh = int.Parse(Request.Form.Get("iddh"));
            string fileUploadTT = "";
            if (Request.Files.Count > 0)
            {
                try
                {
                    string currentYearTT = DateTime.Now.Year.ToString();
                    string fpathTT = Server.MapPath("~/Uploads/anhDanhHieu/" + currentYearTT + "/");
                    if (!Directory.Exists(fpathTT))
                    {
                        Directory.CreateDirectory(fpathTT);
                    }
                    HttpFileCollectionBase filesTT = Request.Files;
                    List<KeyValuePair<string, string>> lsFileDinhKemTT = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < filesTT.Count; i++)
                    {
                        string tKeyTT = filesTT.AllKeys[i];
                        string filename = Path.GetFileName(Request.Files[i].FileName);
                        HttpPostedFileBase fileTT = filesTT[i];
                        string fnameTT, ftypeTT;
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = fileTT.FileName.Split(new char[] { '\\' });
                            fnameTT = testfiles[testfiles.Length - 1];
                        }
                        else
                        {
                            fnameTT = fileTT.FileName;
                        }
                        fnameTT = Path.Combine(fpathTT, fnameTT);
                        ftypeTT = fileTT.ContentType;
                        if (!System.IO.File.Exists(fnameTT))
                        {
                            fileTT.SaveAs(fnameTT);
                        }
                        lsFileDinhKemTT.Add(new KeyValuePair<string, string>(fnameTT, ftypeTT));
                    }
                    fileUploadTT = JsonConvert.SerializeObject(lsFileDinhKemTT);
                }
                catch (Exception)
                {
                    //return -1;
                    throw;
                }
            }
            var hosokyyeu = _entities.qltdkt_hosokyyeu.Where(x => x.daXoa == 0).ToList();

            if (hosokyyeu != null)
            {
                for (int i = 0; i < hosokyyeu.Count; i++)
                {
                    if (iddh == (int)hosokyyeu[i].idDanhHieu)
                    {
                        data = _danhhieuService.TraoTangDanhHieu((int)hosokyyeu[i].idDanhHieu, fileUploadTT);

                    }

                }



            }
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        public JsonResult huyTraoTangDanhHieu(int idDanhHieu)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.HuyTraoTangDanhHieu(idDanhHieu);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult xemDanhHieu()
        {
            int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.xemDanhHieu(idDanhHieu);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult XoaDanhHieu()
        {
            int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            try
            {
                var huyttvaxoatrongsky = _danhhieuService.HuyTraoTangDanhHieu(idDanhHieu);
                if (huyttvaxoatrongsky == 1)
                {
                    var dh = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                    dh.daXoa = true;
                    _entities.SaveChanges();
                    return Json(1, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(0, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                //throw ex;
                return Json(0, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult chuyenSangKhenThuong(int iddh)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _danhhieuService.chuyenSangKhenThuong(iddh);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadTreeDonVi()
        {
            return Json(_util.getTreeDonVi(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadTreeCaNhan()
        {
            return Json(_util.getTreeCaNhan(), JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadFile()
        {
            string fullpath = Request["fullpath"];
            string strfull = fullpath.Replace(",", "\\");
            string file = Request["file"];
            string type = Request["type"];
            return File(strfull, type, file);
        }

    }
}