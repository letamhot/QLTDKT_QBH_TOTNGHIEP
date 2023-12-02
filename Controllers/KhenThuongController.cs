using Newtonsoft.Json;
using QLTDKT.Models;
using QLTDKT.Models.Service.thiDuaService;
using QLTDKT.Models.Service.khenThuongService;
using QLTDKT.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Web.UI.WebControls;
using System.Web.Services;
using ExcelDataReader;
using QLTDKT.Models.Service.soKyYeuService;
using QLTDKT.Models.Service.KhenThuongService;

namespace QLTDKT.Controllers
{
    public class KhenThuongController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private khenThuongService _khenthuongService = new khenThuongService();
        private JsTreeAccess _util = new JsTreeAccess();
        // GET: KhenThuong
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult getKhenThuongDDT()
        {
            return View();
        }
        public ActionResult getKhenThuongCM()
        {
            return View();
        }
        public ActionResult getKhenThuongCD()
        {
            return View();
        }
        [HttpPost]

        public int ImportFile()
        {
            /*qltdkt_khenthuong kt = new qltdkt_khenthuong();

            dstapthecanhankt _ds = new dstapthecanhankt();
            List<dschitietcanhantapthekhenthuong> lsCNTT = new List<dschitietcanhantapthekhenthuong>();*/
            int id_khenthuong = 0;
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
                                    qltdkt_khenthuong kt = new qltdkt_khenthuong();

                                    dstapthecanhankt _ds = new dstapthecanhankt();
                                    List<dschitietcanhantapthekhenthuong> lsCNTT = new List<dschitietcanhantapthekhenthuong>();
                                    DataRow[] found = dt.Select("column6 = '" + dt_SoHieu.Rows[v][0] + "'");
                                    Console.WriteLine("=============found[0][7].ToString(): " + found[0][7].ToString());
                                    if (found.Length > 0)
                                    {
                                        kt.soHieu = found[0][6].ToString();
                                        kt.ngayKhenThuong = DateTime.Parse(found[0][7].ToString());
                                        kt.ngayTraoTang = DateTime.Parse(found[0][7].ToString());
                                        kt.kieuKhenThuong = 1;//1:Năm;2:Đột xuất;3:Chuyên đề;4:Giai đoạn
                                        kt.doiTuongThamGia = byte.Parse(found[0][1].ToString());
                                        kt.noiDungKhenThuong = found[0][8].ToString();
                                        kt.capKhenThuong = byte.Parse(found[0][5].ToString());
                                        kt.capKyKhenThuong = found[0][10].ToString();
                                        kt.bophan = int.Parse(found[0][11].ToString());
                                        kt.daTraoTang = bool.Parse(found[0][12].ToString());
                                        if (found[0][13].ToString() == "0")
                                        {
                                            kt.hinhThucKhenThuong = 0;
                                        }
                                        else
                                        {
                                            kt.hinhThucKhenThuong = int.Parse(found[0][13].ToString());

                                        }
                                        kt.trangThai = 0;
                                        kt.daXoa = false;
                                        _entities.qltdkt_khenthuong.Add(kt);
                                        _entities.SaveChanges();
                                        id_khenthuong = kt.id;
                                    }
                                    for (int k = 0; k < found.Length; k++)
                                    {
                                        dschitietcanhantapthekhenthuong _object = new dschitietcanhantapthekhenthuong();
                                        _object.tenCaNhanTapThe = found[k][2].ToString();
                                        _object.loaiKT = kt.doiTuongThamGia;
                                        _object.ischeck = true;
                                        _object.id = int.Parse(found[k][0].ToString());
                                        if (_object.loaiKT == 1)
                                        {
                                            _object.chucDanh = found[k][3].ToString();

                                        }
                                        else if (_object.loaiKT == 0)
                                        {
                                            _object.chucDanh = "";

                                        }

                                        _object.donVi = found[k][4].ToString();
                                        _object.tienThuong = found[k][9].ToString();
                                        _ds.tongtien += int.Parse(found[k][9].ToString());
                                        lsCNTT.Add(_object);
                                    }
                                    _ds.idKhenThuong = id_khenthuong;
                                    _ds.loaiKhenThuong = (int)kt.doiTuongThamGia;
                                    _ds.tenKhenThuong = kt.noiDungKhenThuong;

                                    _ds.dschitietcanhantapthekhenthuong = lsCNTT;

                                    qltdkt_khenthuong _update = _entities.qltdkt_khenthuong.Find(id_khenthuong);
                                    _update.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(_ds);
                                    _update.tongTienThuong = _ds.tongtien;
                                    _entities.SaveChanges();

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
            var data2 = _entities.qltdkt_khenthuong.Where(x => x.daXoa == false && x.id == 15).ToList();
            ExcelPackage ep = new ExcelPackage();

            ExcelWorksheet sheet2 = ep.Workbook.Worksheets.Add("Khen Thưởng");
            int row = 1;


            sheet2.Cells["A" + row.ToString()].Value = "Mã cá nhân/ tập thể";
            sheet2.Cells["B" + row.ToString()].Value = "Đối tượng(0 - Tập thể; 1 - Cá nhân)";
            sheet2.Cells["C" + row.ToString()].Value = "Tên cá nhân/tập thể";
            sheet2.Cells["D" + row.ToString()].Value = "Chức danh";
            sheet2.Cells["E" + row.ToString()].Value = "Đơn vị";
            sheet2.Cells["F" + row.ToString()].Value = "Cấp khen thưởng";
            sheet2.Cells["G" + row.ToString()].Value = "Số Quyết định";
            sheet2.Cells["H" + row.ToString()].Value = "Ngày ban hành";
            sheet2.Cells["I" + row.ToString()].Value = "Nội dung khen thưởng";
            sheet2.Cells["J" + row.ToString()].Value = "Tiền thưởng";
            sheet2.Cells["K" + row.ToString()].Value = "Người ký";
            sheet2.Cells["L" + row.ToString()].Value = "Bộ phận(1: Chuyên môn; 2: Đảng - Đoàn thể; 3: Công đoàn)";
            sheet2.Cells["M" + row.ToString()].Value = "Trạng thái khen thưởng(true - Đã khen thưởng; false - Chưa khen thưởng)";
            sheet2.Cells["N" + row.ToString()].Value = "Hình thức khen thưởng";


            sheet2.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet2.Row(row).Style.Font.Bold = true;
            sheet2.Cells[row, 1, row, 13].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 13].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 13].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 13].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;

            var data = _entities.qltdkt_dm_capkykhenthuong.ToList();

            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Danh mục cấp ký khen thưởng");
            row = 1;
            sheet.Cells["E" + row.ToString()].Value = "Kiểu khen thưởng = 1:Năm; 2:Đột xuất; 3:Chuyên đề; 4:Giai đoạn";
            sheet.Cells["C" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["C" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["D" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["D" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["E" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["E" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Value = "Mã cấp ký khen thưởng";
            sheet.Cells["B" + row.ToString()].Value = "Tên cấp ký khen thưởng";
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
                        sheet.Cells["B" + row.ToString()].Value = data[i].tenCapKyKhenThuong;

                    }
                    sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
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
            sheet3.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet3.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet3.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet3.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
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
            var data4 = _entities.qltdkt_dm_hinhthuckhenthuong.ToList();
            ExcelWorksheet sheet4 = ep.Workbook.Worksheets.Add("Danh mục hình thức khen thưởng");
            row = 1;
            sheet4.Cells["A" + row.ToString()].Value = "Id hình thức khen thưởng";
            sheet4.Cells["B" + row.ToString()].Value = "Cá nhân - 1/ Tập thể - 0";
            sheet4.Cells["C" + row.ToString()].Value = "Tên hình thức khen thưởng";
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
                    sheet4.Cells["A" + row.ToString()].Value = data4[i].id;
                    sheet4.Cells["B" + row.ToString()].Value = data4[i].loaiKhenThuong;
                    sheet4.Cells["C" + row.ToString()].Value = data4[i].tenHinhThucKhenThuong;




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
        public JsonResult GetDanhSachKhenThuong()
        {
            int capKhenThuong;
            if ((Request["capKhenThuong"] == ""))
            {
                capKhenThuong = 0;
            }
            else
            {
                capKhenThuong = int.Parse(Request["capKhenThuong"]);
            }
            string soHieu = Request["soHieu"];
            string ngayKhenThuong_TuNgay = Request["ngayKhenThuong_TuNgay"];
            string ngayKhenThuong_DenNgay = Request["ngayKhenThuong_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int bophan = int.Parse(Request["bophan"]);
            return Json(new { data = _khenthuongService.getDanhSachKhenThuong(capKhenThuong, bophan, soHieu, ngayKhenThuong_TuNgay, ngayKhenThuong_DenNgay, trangThai) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachKhenThuongCD()
        {
            int capKhenThuong;
            if ((Request["capKhenThuong"] == ""))
            {
                capKhenThuong = 0;
            }
            else
            {
                capKhenThuong = int.Parse(Request["capKhenThuong"]);
            }
            string soHieu = Request["soHieu"];
            string ngayKhenThuong_TuNgay = Request["ngayKhenThuong_TuNgay"];
            string ngayKhenThuong_DenNgay = Request["ngayKhenThuong_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int bophan = 3;
            return Json(new { data = _khenthuongService.getDanhSachKhenThuong(capKhenThuong, bophan, soHieu, ngayKhenThuong_TuNgay, ngayKhenThuong_DenNgay, trangThai) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDanhSachKhenThuongDDT()
        {
            int capKhenThuong;
            if ((Request["capKhenThuong"] == ""))
            {
                capKhenThuong = 0;
            }
            else
            {
                capKhenThuong = int.Parse(Request["capKhenThuong"]);
            }
            string soHieu = Request["soHieu"];
            string ngayKhenThuong_TuNgay = Request["ngayKhenThuong_TuNgay"];
            string ngayKhenThuong_DenNgay = Request["ngayKhenThuong_DenNgay"];
            int trangThai = int.Parse(Request["trangThai"]);
            int bophan = 2;
            return Json(new { data = _khenthuongService.getDanhSachKhenThuong(capKhenThuong, bophan, soHieu, ngayKhenThuong_TuNgay, ngayKhenThuong_DenNgay, trangThai) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ChinhSuaKhenThuong()
        {
            int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.getKhenThuongByID(idKhenThuong);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult loadTenKhenThuong()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            var data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetListDVPD()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_capkykhenthuong.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTenKhenThuong(int loaiKhenThuong)
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            var data = _khenthuongService.GetTenKhenThuong(loaiKhenThuong);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadNhanVienByDonVi(int idDonVi)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.loadNhanVienByDonVi(idDonVi);
            return Json(data, JsonRequestBehavior.AllowGet);
        }


        public JsonResult loadDonViByDonVi(int idDonVi)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.loadDonViByDonVi(idDonVi);
            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public JsonResult loadDsCaNhanTapThe(int idkt)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.loadDsCaNhanTapThe(idkt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ThemCaNhanTapThe(string idcntt, int ldh, string tdh, int idkt, string tien, int tongtien)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.ThemCaNhanTapThe(idcntt, ldh, tdh, idkt, tien, tongtien);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult xoaCaNhanTapThe(string idcntt, int idkt)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.xoaCaNhanTapThe(idcntt, idkt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult suatienCaNhanTapThe(string idcntt, int idkt, string tien)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.suatienCaNhanTapThe(idcntt, idkt, tien);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult TraoTangKhenThuong(string idkt, string ngaytraotang, string trangThai, int htkt)
        {

            var data = _khenthuongService.TraoTangKhenThuong(int.Parse(idkt), ngaytraotang, int.Parse(trangThai), htkt);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult huyTraoTangKhenThuong(int idKhenThuong)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.HuyTraoTangKhenThuong(idKhenThuong);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult xemKhenThuong(int idKhenThuong)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _khenthuongService.xemKhenThuong(idKhenThuong);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult XoaKhenThuong()
        {
            int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            try
            {
                var huyttvaxoatrongsky = _khenthuongService.HuyTraoTangKhenThuong(idKhenThuong);
                if (huyttvaxoatrongsky == 1)
                {
                    var dh = _entities.qltdkt_khenthuong.Find(idKhenThuong);
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

        //cập nhật khen thưởng
        public int CapNhatKhenThuong()
        {
            int idKhenThuong;
            if (Request.Form.Get("id") == "")
            {
                idKhenThuong = 0;
            }
            else
            {
                idKhenThuong = int.Parse(Request.Form.Get("id"));

            }
            byte capKhenThuong;

            if (Request.Form.Get("capKhenThuong") == "")
            {
                capKhenThuong = 0;
            }
            else
            {
                capKhenThuong = byte.Parse(Request.Form.Get("capKhenThuong"));
            }
            byte kieuKhenThuong = byte.Parse(Request.Form.Get("kieuKhenThuong"));
            byte doiTuongThamGia = byte.Parse(Request.Form.Get("doiTuongThamGia"));
            byte bophan = byte.Parse(Request.Form.Get("bophan"));
            string soHieu = Request.Form.Get("soHieu");
            string ngayKhenThuong = Request.Form.Get("ngayKhenThuong");
            string capKyKhenThuong = Request.Form.Get("capKyKhenThuong");
            string ghiChuHTKT = Request.Form.Get("ghichuHTKT");
            string noiDungKhenThuong = Request.Form.Get("noiDungKhenThuong");
            string tien = Request.Form.Get("tienThuong");
            int hinhThucKhenThuong = int.Parse(Request.Form.Get("hinhThucKhenThuong") != "0" ? Request.Form.Get("hinhThucKhenThuong") : "0");
            int tongtien = 0;

            //string hinhThucKhenThuong = Request.Form.Get("hinhThucKhenThuong");
            //string danhSachCaNhanTapThe = Request.Form.Get("danhSachCaNhanTapThe");
            string danhSachCaNhanTapThe = Request.Unvalidated.Form.Get("danhSachCaNhanTapThe");

            int daTraoTang = int.Parse(Request.Form.Get("daTraoTang"));
            //byte trangThai = byte.Parse(Request.Form.Get("trangThai"));
            bool dtt = true;
            if (daTraoTang == 0)
            {
                dtt = false;
            }

            string output_filedinhkem = "";

            if (idKhenThuong == 0) ///Thêm mới  thi đua
            {
                List<dschitietcanhantapthekhenthuong> lsDsCNTT = new List<dschitietcanhantapthekhenthuong>();
                if (danhSachCaNhanTapThe.Length > 0)
                {
                    string[] spl = danhSachCaNhanTapThe.Split(',');
                    string[] lst = tien.Split(',');
                    for (int i = 0; i < spl.Length; i++)
                    {

                        if (spl[i] != "")
                        {
                            if (doiTuongThamGia == 0)
                            {
                                int id_DV = int.Parse(spl[i]);
                                qltdkt_dm_donvi _dmDV = _entities.qltdkt_dm_donvi.Find(id_DV);
                                dschitietcanhantapthekhenthuong _newDV = new dschitietcanhantapthekhenthuong
                                {
                                    id = _dmDV.id,
                                    ischeck = true,
                                    loaiKT = 0,
                                    tenCaNhanTapThe = _dmDV.tenDonVi,
                                    chucDanh = "",
                                    donVi = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha).tenDonVi,
                                    donViCha = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(_dmDV.idCha).idCha).tenDonVi,
                                    tienThuong = lst[i] != "" ? lst[i] : "0",
                                };
                                tongtien = tongtien + int.Parse(_newDV.tienThuong);
                                lsDsCNTT.Add(_newDV);
                            }
                            else if (doiTuongThamGia == 1)
                            {
                                int id_NV = int.Parse(spl[i]);
                                qltdkt_dm_nhanvien _dmNV = _entities.qltdkt_dm_nhanvien.Find(id_NV);
                                dschitietcanhantapthekhenthuong _newNV = new dschitietcanhantapthekhenthuong
                                {
                                    id = _dmNV.id,
                                    loaiKT = 1,
                                    ischeck = true,

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
                }
                dstapthecanhankt dsCNTT = new dstapthecanhankt();
                dsCNTT.loaiKhenThuong = doiTuongThamGia;
                dsCNTT.tongtien = tongtien;
                dsCNTT.tenKhenThuong = noiDungKhenThuong;
                dsCNTT.dschitietcanhantapthekhenthuong = lsDsCNTT;
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
                    qltdkt_khenthuong _newObj = new qltdkt_khenthuong
                    {
                        id = idKhenThuong,
                        capKhenThuong = capKhenThuong,
                        quyetDinh = output_filedinhkem,
                        kieuKhenThuong = kieuKhenThuong,
                        hinhThucKhenThuong = hinhThucKhenThuong,
                        doiTuongThamGia = doiTuongThamGia,
                        bophan = bophan,
                        soHieu = soHieu,
                        danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCNTT),

                        ngayKhenThuong = DateTime.ParseExact(ngayKhenThuong, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        ngayTraoTang = DateTime.ParseExact(ngayKhenThuong, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                        noiDungKhenThuong = noiDungKhenThuong,
                        tongTienThuong = tongtien,
                        capKyKhenThuong = capKyKhenThuong,
                        ghichuHTKT = ghiChuHTKT,
                        ngayTao = DateTime.Now,
                        daTraoTang = dtt,
                        daXoa = false,
                        trangThai = 0,
                    };

                    _entities.qltdkt_khenthuong.Add(_newObj);
                    _entities.SaveChanges();
                    LuuSoKyYeu(_newObj.id);

                    //ThemCaNhanTapTache("", "", _newObj.kieuKhenThuong, _newObj.id, "");
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
                List<dschitietcanhantapthekhenthuong> lsDsCNTT = new List<dschitietcanhantapthekhenthuong>();
                if (danhSachCaNhanTapThe.Length > 0)
                {
                    string[] spl = danhSachCaNhanTapThe.Split(',');
                    string[] lst = tien.Split(',');
                    for (int i = 0; i < spl.Length; i++)
                    {

                        if (spl[i] != "")
                        {
                            if (doiTuongThamGia == 0)
                            {
                                int id_DV = int.Parse(spl[i]);
                                qltdkt_dm_donvi _dmDV = _entities.qltdkt_dm_donvi.Find(id_DV);
                                qltdkt_dm_donvi _dmDVC = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_donvi.Find(id_DV).idCha);
                                if (_dmDVC.idCha > 0)
                                {
                                    dschitietcanhantapthekhenthuong _newDV = new dschitietcanhantapthekhenthuong
                                    {
                                        id = _dmDV.id,
                                        ischeck = true,
                                        loaiKT = 0,
                                        tenCaNhanTapThe = _dmDV.tenDonVi,
                                        chucDanh = "",
                                        donVi = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha).tenDonVi,
                                        donViCha = _entities.qltdkt_dm_donvi.Find(_dmDVC.idCha).tenDonVi,
                                        tienThuong = lst[i] != "" ? lst[i] : "0",
                                    };
                                    tongtien = tongtien + int.Parse(_newDV.tienThuong);

                                    lsDsCNTT.Add(_newDV);
                                }
                                else
                                {
                                    dschitietcanhantapthekhenthuong _newDV = new dschitietcanhantapthekhenthuong
                                    {
                                        id = _dmDV.id,
                                        ischeck = true,
                                        loaiKT = 0,
                                        tenCaNhanTapThe = _dmDV.tenDonVi,
                                        chucDanh = "",
                                        donVi = _entities.qltdkt_dm_donvi.Find(_dmDV.idCha).tenDonVi,
                                        tienThuong = lst[i] != "" ? lst[i] : "0",
                                    };
                                    tongtien = tongtien + int.Parse(_newDV.tienThuong);

                                    lsDsCNTT.Add(_newDV);
                                }

                            }
                            else if (doiTuongThamGia == 1)
                            {
                                int id_NV = int.Parse(spl[i]);
                                qltdkt_dm_nhanvien _dmNV = _entities.qltdkt_dm_nhanvien.Find(id_NV);
                                qltdkt_dm_donvi _dmDVC = _entities.qltdkt_dm_donvi.Find(_dmNV.donViId);
                                if (_dmDVC.idCha > 0)
                                {
                                    dschitietcanhantapthekhenthuong _newNV = new dschitietcanhantapthekhenthuong
                                    {
                                        id = _dmNV.id,
                                        ischeck = true,
                                        loaiKT = 1,
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
                                    dschitietcanhantapthekhenthuong _newNV = new dschitietcanhantapthekhenthuong
                                    {
                                        id = _dmNV.id,
                                        ischeck = true,
                                        loaiKT = 1,
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
                }
                dstapthecanhankt dsCNTT = new dstapthecanhankt();
                dsCNTT.loaiKhenThuong = doiTuongThamGia;
                dsCNTT.tongtien = tongtien;
                dsCNTT.idKhenThuong = idKhenThuong;
                dsCNTT.tenKhenThuong = noiDungKhenThuong;
                dsCNTT.dschitietcanhantapthekhenthuong = lsDsCNTT;
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
                    var dh = _entities.qltdkt_khenthuong.Find(idKhenThuong);
                    dh.id = idKhenThuong;
                    dh.capKhenThuong = capKhenThuong;
                    if (dh.quyetDinh == "")
                    {
                        dh.quyetDinh = output_filedinhkem;

                    }
                    else
                    {
                        output_filedinhkem = dh.quyetDinh;
                    }
                    dh.soHieu = soHieu;
                    dh.hinhThucKhenThuong = hinhThucKhenThuong;
                    dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCNTT);
                    dh.bophan = bophan;
                    dh.ngayKhenThuong = DateTime.ParseExact(ngayKhenThuong, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dh.ngayTraoTang = DateTime.ParseExact(ngayKhenThuong, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    dh.noiDungKhenThuong = noiDungKhenThuong;
                    dh.kieuKhenThuong = kieuKhenThuong;
                    dh.tongTienThuong = tongtien;
                    dh.capKyKhenThuong = capKyKhenThuong;
                    dh.ghichuHTKT = ghiChuHTKT;
                    dh.daTraoTang = dtt;
                    //trangThai = trangThai;
                    //dh.daXoa = false;

                    _entities.SaveChanges();
                    LuuSoKyYeu(dh.id);

                    //ThemCaNhanTapTheCache("", "", dh.kieuKhenThuong, dh.id, "");

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
        //Lưu Sổ kỷ yếu
        public int LuuSoKyYeu(int idkt)
        {
            var dh = _entities.qltdkt_khenthuong.Find(idkt);
            var chitietdh = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
            if (chitietdh.loaiKhenThuong == 0) //tập thể
            {
                int t = 0;
                foreach (var item in chitietdh.dschitietcanhantapthekhenthuong)
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {
                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                            if (ck == null) //
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                sky.capKyKhenThuong = dh.capKyKhenThuong;
                                if (dh.hinhThucKhenThuong != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }
                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                sky.idKhenThuong = idkt;
                                sky.daXoa = false;

                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;
                                check.ngayTao = DateTime.Now;
                                _entities.SaveChanges();
                            }
                            t++;
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
                            sky.kieuKyYeu = 3;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenKhenThuong = chitietdh.tenKhenThuong;
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                            if (dh.hinhThucKhenThuong != 0)
                            {
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                            }
                            else
                            {
                                sky.hinhThucTraoTang = "0";
                            }

                            sky.capKyKhenThuong = dh.capKyKhenThuong;
                            sky.idKhenThuong = idkt;
                            sky.daXoa = false;

                            sky.tienThuong = decimal.Parse(item.tienThuong);
                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                            t++;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
                if (t == chitietdh.dschitietcanhantapthekhenthuong.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (chitietdh.loaiKhenThuong == 1)
            {
                int t = 0;
                foreach (var item in chitietdh.dschitietcanhantapthekhenthuong)
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {
                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                            if (ck == null)
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                if (dh.hinhThucKhenThuong != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }

                                sky.capKyKhenThuong = dh.capKyKhenThuong;
                                sky.idKhenThuong = idkt;
                                sky.daXoa = false;

                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;
                                check.ngayTao = DateTime.Now;

                                _entities.SaveChanges();
                            }

                            t++;
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
                            sky.kieuKyYeu = 3;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenKhenThuong = chitietdh.tenKhenThuong;
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                            if (dh.hinhThucKhenThuong != 0)
                            {
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                            }
                            else
                            {
                                sky.hinhThucTraoTang = "0";
                            }

                            sky.capKyKhenThuong = dh.capKyKhenThuong;

                            sky.idKhenThuong = idkt;
                            sky.tienThuong = decimal.Parse(item.tienThuong);
                            sky.daXoa = false;

                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                            t++;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
                if (t == chitietdh.dschitietcanhantapthekhenthuong.Count())
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
                foreach (var item in chitietdh.dschitietcanhantapthekhenthuong)
                {
                    if (item.loaiKT == 1)
                    {
                        var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                        if (check != null) //nếu có thì update
                        {
                            try
                            {
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                                var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                                if (ck == null)
                                {
                                    soKyYeuModel sky = new soKyYeuModel();
                                    sky.kieuKyYeu = 3;
                                    sky.ngayKyYeu = DateTime.Now;
                                    sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                    sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                    sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                    if (dh.hinhThucKhenThuong != 0)
                                    {
                                        sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                    }
                                    else
                                    {
                                        sky.hinhThucTraoTang = "0";
                                    }

                                    sky.capKyKhenThuong = dh.capKyKhenThuong;

                                    sky.idKhenThuong = idkt;
                                    sky.tienThuong = decimal.Parse(item.tienThuong);
                                    sky.daXoa = false;

                                    hsky.Add(sky);
                                    check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                    check.daXoa = 0;
                                    check.ngayTao = DateTime.Now;
                                    _entities.SaveChanges();
                                }

                                t++;
                            }
                            catch (Exception ex)
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

                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                if (dh.hinhThucKhenThuong != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }

                                sky.capKyKhenThuong = dh.capKyKhenThuong;

                                sky.idKhenThuong = idkt;
                                sky.daXoa = false;

                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                                _sky.Add(sky);
                                hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);
                                _entities.qltdkt_hosokyyeu.Add(hsky);
                                _entities.SaveChanges();
                                t++;
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                        if (check != null) //nếu có thì update
                        {
                            try
                            {
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                                var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                                if (ck == null) //
                                {
                                    soKyYeuModel sky = new soKyYeuModel();
                                    sky.kieuKyYeu = 3;
                                    sky.ngayKyYeu = DateTime.Now;
                                    sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                    sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                    sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                    if (dh.hinhThucKhenThuong != 0)
                                    {
                                        sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                    }
                                    else
                                    {
                                        sky.hinhThucTraoTang = "0";
                                    }

                                    sky.capKyKhenThuong = dh.capKyKhenThuong;

                                    sky.tienThuong = decimal.Parse(item.tienThuong);
                                    sky.idKhenThuong = idkt;
                                    sky.daXoa = false;

                                    hsky.Add(sky);
                                    check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                    check.daXoa = 0;
                                    check.ngayTao = DateTime.Now;
                                    _entities.SaveChanges();
                                }
                                t++;
                            }
                            catch (Exception ex)
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

                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                if (dh.hinhThucKhenThuong != 0)
                                {
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                }
                                else
                                {
                                    sky.hinhThucTraoTang = "0";
                                }
                                sky.capKyKhenThuong = dh.capKyKhenThuong;
                                sky.idKhenThuong = idkt;
                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                sky.daXoa = false;
                                List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                                _sky.Add(sky);

                                hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                                _entities.qltdkt_hosokyyeu.Add(hsky);
                                _entities.SaveChanges();
                                t++;
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }

                    }
                }
                if (t == chitietdh.dschitietcanhantapthekhenthuong.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        //danh sách danh mục khen thưởng 
        public JsonResult GetDmHinhThucKhenThuongDT()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int loaiKhenThuong = (Request.QueryString["loaiKhenThuong"] != "" ? int.Parse(Request.QueryString["loaiKhenThuong"]) : -1);
            int bophan = (Request.QueryString["bophan"] != "" ? int.Parse(Request.QueryString["bophan"]) : 0);
            //var htkt = _entities.qltdkt_dm_hinhthuckhenthuong.Find(loaiKhenThuong);
            return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == loaiKhenThuong && x.bophan == bophan && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);



        }

        public JsonResult GetDmHinhThucKhenThuongByKieu()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int loaiKhenThuong = (Request.QueryString["loaiKhenThuong"] != "" ? int.Parse(Request.QueryString["loaiKhenThuong"]) : -1);
            int bophan = (Request.QueryString["bophan"] != "" ? int.Parse(Request.QueryString["bophan"]) : 0);
            return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == loaiKhenThuong && x.bophan == bophan && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
        }
        //danh sách cá nhân tập thể theo kiểu khen thưởng
        public JsonResult GetCaNhanTapTheByKieu()
        {
            int loaiKhenThuong = (Request.QueryString["loaiKhenThuong"] != "" ? int.Parse(Request.QueryString["loaiKhenThuong"]) : -1);
            if (loaiKhenThuong == 1)
            {

                return Json(new { data = _khenthuongService.loadNhanVienByKieu() }, JsonRequestBehavior.AllowGet);

            }
            else if (loaiKhenThuong == 0)
            {
                return Json(new { data = _khenthuongService.loadDonViByKieu() }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { data = "" }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LoadTreeDonVi()
        {
            return Json(_util.getTreeDonVi(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadTreeCaNhan()
        {
            return Json(_util.getTreeCaNhan(), JsonRequestBehavior.AllowGet);
        }
        public JsonResult searchTapThe(string idDonVi, string searchText)
        {
            var data = _khenthuongService.searchTapThe(idDonVi, searchText);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult searchCaNhan(string idDonVi, string searchText)
        {
            var data = _khenthuongService.searchCaNhan(idDonVi, searchText);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult ThemCaNhanTapTheCache(string idcn, string idtt, int lkt, int idkt, string listCurrent)
        {
            //int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _khenthuongService.ThemCaNhanTapTheCache(idcn, idtt, lkt, idkt, listCurrent);
            return Json(data, JsonRequestBehavior.AllowGet);
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