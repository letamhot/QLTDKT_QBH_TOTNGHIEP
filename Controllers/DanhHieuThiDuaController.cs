using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLTDKT.Models;
using QLTDKT.Models.Service.danhHieuTDService;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ExcelDataReader;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;



namespace QLTDKT.Controllers
{
    public class DanhHieuThiDuaController : Controller
    {

        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private DanhHieuTDService _dhTDService = new DanhHieuTDService();


        // GET: DanhHieuThiDua
        /*public ActionResult Index()
        {
            return View();
        }*/

        public ActionResult getDanhHieuThiDuaCK()
        {
            return View();
        }
        public ActionResult getDanhHieuThiDuaDDT()
        {
            return View();
        }
        public ActionResult getDanhHieuThiDuaCD()
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
                            string path = Server.MapPath("~/Content/Upload/");
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }

                            filePath = path + Path.GetFileName(postedFile.FileName);
                            string extension = Path.GetExtension(postedFile.FileName);
                            postedFile.SaveAs(filePath);

                            string conString = string.Empty;
                            switch (extension)
                            {
                                case ".xls": //Excel 97-03.
                                    conString = ConfigurationManager.ConnectionStrings["Excel03ConString"].ConnectionString;
                                    break;
                                case ".xlsx": //Excel 07 and above.
                                    conString = ConfigurationManager.ConnectionStrings["Excel07ConString"].ConnectionString;
                                    break;
                            }

                            DataTable dt = new DataTable();
                            conString = string.Format(conString, filePath);

                            using (OleDbConnection connExcel = new OleDbConnection(conString))
                            {
                                using (OleDbCommand cmdExcel = new OleDbCommand())
                                {
                                    using (OleDbDataAdapter odaExcel = new OleDbDataAdapter())
                                    {
                                        cmdExcel.Connection = connExcel;

                                        //Get the name of First Sheet.
                                        connExcel.Open();
                                        DataTable dtExcelSchema;
                                        dtExcelSchema = connExcel.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                        string sheetName = dtExcelSchema.Rows[0]["TABLE_NAME"].ToString();
                                        connExcel.Close();

                                        //Read Data from First Sheet.
                                        connExcel.Open();
                                        cmdExcel.CommandText = "SELECT * From [" + sheetName + "]";
                                        odaExcel.SelectCommand = cmdExcel;
                                        odaExcel.Fill(dt);
                                        connExcel.Close();
                                    }
                                }
                            }

                            conString = ConfigurationManager.ConnectionStrings["QLTDKT_Connection"].ConnectionString;
                            using (SqlConnection con = new SqlConnection(conString))
                            {
                                using (SqlBulkCopy sqlBulkCopy = new SqlBulkCopy(con))
                                {
                                    //Set the database table name.
                                    sqlBulkCopy.DestinationTableName = "dbo.qltdkt_dm_danhhieuthidua";

                                    //[OPTIONAL]: Map the Excel columns with that of the database table
                                    sqlBulkCopy.ColumnMappings.Add("ID danh hiệu", "id");
                                    sqlBulkCopy.ColumnMappings.Add("Loại danh hiệu", "loaiDanhHieu");
                                    sqlBulkCopy.ColumnMappings.Add("Tên danh hiệu thi đua", "tenDanhHieuThiDua");
                                    sqlBulkCopy.ColumnMappings.Add("Lưu vào sổ kỷ yếu", "luuSoKyYeu");
                                    sqlBulkCopy.ColumnMappings.Add("Mô tả", "moTa");
                                    sqlBulkCopy.ColumnMappings.Add("Ngày tạo", "ngayTao");
                                    sqlBulkCopy.ColumnMappings.Add("Đã xoá", "daXoa");
                                    sqlBulkCopy.ColumnMappings.Add("Mã thành tích", "idThanhTich");
                                    sqlBulkCopy.ColumnMappings.Add("Chu kỳ", "chuKy");
                                    sqlBulkCopy.ColumnMappings.Add("Cấp thành tích", "capThanhTich");
                                    sqlBulkCopy.ColumnMappings.Add("Bộ phận(1:Chuyên môn, 2:Đảng - Đoàn thể, 3:Công đoàn)", "bophan");
                                    con.Open();
                                    sqlBulkCopy.WriteToServer(dt);
                                    con.Close();
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
        //Danh sách danh hiệu thi đua công đoàn, chuyên môn, đảng đoàn thể
        public JsonResult GetListDHTD()
        {
            int idDanhHieu = int.Parse(Request.QueryString["idDanhHieu"] != "" ? Request.QueryString["idDanhHieu"] : "0");
            int bophan = int.Parse(Request["bophan"]);
            return Json(new { data = _dhTDService.getDanhHieuThiDua(idDanhHieu, bophan) }, JsonRequestBehavior.AllowGet);
            /*_entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_danhhieuthidua.ToList() }, JsonRequestBehavior.AllowGet);*/

        }
        //Danh sách danh hiệu thi đua công đoàn
        public JsonResult GetListDHTDCD()
        {
            int idDanhHieu = int.Parse(Request.QueryString["idDanhHieu"] != "" ? Request.QueryString["idDanhHieu"] : "0");
            int bophan = 3;//công đoàn
            return Json(new { data = _dhTDService.getDanhHieuThiDua(idDanhHieu, bophan) }, JsonRequestBehavior.AllowGet);
            /*_entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_danhhieuthidua.ToList() }, JsonRequestBehavior.AllowGet);*/

        }

        //Danh sách danh hiệu thi đua đảng đoàn thể
        public JsonResult GetListDHTDDDT()
        {
            int idDanhHieu = int.Parse(Request.QueryString["idDanhHieu"] != "" ? Request.QueryString["idDanhHieu"] : "0");
            int bophan = 2;//đảng đoàn thể
            return Json(new { data = _dhTDService.getDanhHieuThiDua(idDanhHieu, bophan) }, JsonRequestBehavior.AllowGet);
            /*_entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_danhhieuthidua.ToList() }, JsonRequestBehavior.AllowGet);*/

        }


        public JsonResult loadChuKy()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            var data = _entities.qltdkt_dm_chuky.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult loadCapKyKT()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            var data = _entities.qltdkt_dm_capkykhenthuong.Where(x => x.daXoa == false).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public bool ExistTenDanhHieu()
        {
            int bophan = int.Parse(Request.QueryString["bophan"] != "0" ? Request.QueryString["bophan"] : "0");
            byte idcntt = byte.Parse(Request.QueryString["idcntt"] != "" ? Request.QueryString["idcntt"] : "");
            string label = Request.QueryString["label"] != "" ? Request.QueryString["label"] : "";
            var chk = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.bophan == bophan && x.loaiDanhHieu == idcntt && x.daXoa == false && x.tenDanhHieuThiDua == label.ToLower() && x.tenDanhHieuThiDua == label.ToUpper()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }

        [HttpPost]
        public string UpdateDHTD(qltdkt_dm_danhhieuthidua _objKH)
        {
            try
            {
                if (_objKH.id == 0)
                {
                    qltdkt_dm_danhhieuthidua _new = new qltdkt_dm_danhhieuthidua();
                    _new.tenDanhHieuThiDua = _objKH.tenDanhHieuThiDua;
                    _new.loaiDanhHieu = _objKH.loaiDanhHieu;
                    _new.bophan = _objKH.bophan;
                    _new.ngayTao = DateTime.Now;
                    _new.daXoa = false;
                    _new.luuSoKyYeu = _objKH.luuSoKyYeu;
                    _new.moTa = null;
                    _new.idThanhTich = _objKH.idThanhTich;
                    _new.chuKy = _objKH.chuKy;
                    _new.capThanhTich = _objKH.capThanhTich;
                    _entities.qltdkt_dm_danhhieuthidua.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {

                    var _update = _entities.qltdkt_dm_danhhieuthidua.Find(_objKH.id);
                    _update.tenDanhHieuThiDua = _objKH.tenDanhHieuThiDua;
                    _update.loaiDanhHieu = _objKH.loaiDanhHieu;
                    _update.bophan = _objKH.bophan;
                    _update.daXoa = false;
                    _update.luuSoKyYeu = _objKH.luuSoKyYeu;
                    _update.moTa = _objKH.moTa;
                    _update.idThanhTich = _objKH.idThanhTich;
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
        public JsonResult GetDHTDById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int ID = int.Parse(Request.QueryString["id"] != "" ? Request.QueryString["id"] : "0");
            return Json(_entities.qltdkt_dm_danhhieuthidua.FirstOrDefault(x => x.id == ID), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string DeleteByID()
        {
            try
            {
                var id = Session["id"];

                int idDanhHieu = int.Parse(Request["ID"]);
                qltdkt_dm_danhhieuthidua _old = _entities.qltdkt_dm_danhhieuthidua.Find(idDanhHieu);
                if (_old != null)
                {
                    _old.daXoa = true;

                }
                var dh = _entities.qltdkt_danhhieu.Where(x => x.idTenDanhHieu == idDanhHieu && x.daXoa == false).ToList();
                if (dh != null && dh.Count > 0)
                {
                    for (int i = 0; i < dh.Count; i++)
                    {
                        if (dh[i].idTenDanhHieu == idDanhHieu)
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
                    qltdkt_dm_danhhieuthidua _old = _entities.qltdkt_dm_danhhieuthidua.Find(int.Parse(iddonvi[i]));
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

        public void ExportDm()
        {
            var data2 = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.daXoa == false && x.id == 13).ToList();
            ExcelPackage ep = new ExcelPackage();

            ExcelWorksheet sheet2 = ep.Workbook.Worksheets.Add("Danh Mục Danh Hiệu Thi Đua");
            int row = 1;

            sheet2.Cells["L" + row.ToString()].Value = "Đã Xoá = False";
            sheet2.Cells["M" + row.ToString()].Value = "Lưu vào sổ kỷ yếu= True";
            sheet2.Cells["N" + row.ToString()].Value = "Loại danh hiệu = 1 - Cá nhân; Loại danh hiệu = 2 - Tập thể";
            sheet2.Cells["L" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells["L" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet2.Cells["M" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells["M" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet2.Cells["N" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Cells["N" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            sheet2.Cells["A" + row.ToString()].Value = "ID danh hiệu";
            sheet2.Cells["B" + row.ToString()].Value = "Loại danh hiệu";
            sheet2.Cells["C" + row.ToString()].Value = "Tên danh hiệu thi đua";
            sheet2.Cells["D" + row.ToString()].Value = "Lưu vào sổ kỷ yếu";
            sheet2.Cells["E" + row.ToString()].Value = "Mô tả";
            sheet2.Cells["F" + row.ToString()].Value = "Ngày tạo";
            sheet2.Cells["G" + row.ToString()].Value = "Đã xoá";
            sheet2.Cells["H" + row.ToString()].Value = "Mã thành tích";
            sheet2.Cells["I" + row.ToString()].Value = "Chu kỳ";
            sheet2.Cells["J" + row.ToString()].Value = "Cấp thành tích";
            sheet2.Cells["K" + row.ToString()].Value = "Bộ phận(1:Chuyên môn, 2:Đảng - Đoàn thể, 3:Công đoàn)";

            sheet2.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet2.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet2.Row(row).Style.Font.Bold = true;
            sheet2.Cells[row, 1, row, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet2.Cells[row, 1, row, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            if (data2 != null && data2.Count > 0)
            {
                for (int i = 0; i < data2.Count; i++)
                {
                    {
                        sheet2.Cells["A" + row.ToString()].Value = data2[i].id;
                        sheet2.Cells["B" + row.ToString()].Value = data2[i].loaiDanhHieu;

                        sheet2.Cells["C" + row.ToString()].Value = data2[i].tenDanhHieuThiDua;
                        sheet2.Cells["D" + row.ToString()].Value = data2[i].luuSoKyYeu;
                        sheet2.Cells["E" + row.ToString()].Value = data2[i].moTa;
                        sheet2.Cells["F" + row.ToString()].Value = "2021-08-06";
                        sheet2.Cells["G" + row.ToString()].Value = data2[i].daXoa;
                        sheet2.Cells["H" + row.ToString()].Value = data2[i].idThanhTich;
                        sheet2.Cells["I" + row.ToString()].Value = data2[i].chuKy;
                        sheet2.Cells["J" + row.ToString()].Value = data2[i].capThanhTich;
                        sheet2.Cells["K" + row.ToString()].Value = data2[i].bophan;


                    }
                    sheet2.Cells[row, 1, row, 11].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet2.Cells[row, 1, row, 11].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet2.Cells[row, 1, row, 11].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet2.Cells[row, 1, row, 11].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }

            var data = _entities.qltdkt_dm_chuky.Where(x => x.daXoa == false).ToList();

            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Chu kỳ");
            row = 1;
            sheet.Cells[row, 1, row + 3, 2].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "Danh mục chu kỳ";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 4;
            sheet.Cells["A" + row.ToString()].Value = "Mã chu kỳ";
            sheet.Cells["B" + row.ToString()].Value = "Tên chu kỳ";
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
                        sheet.Cells["B" + row.ToString()].Value = data[i].tenChuKy;

                    }
                    sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    row++;
                }
            }
            var data1 = _entities.qltdkt_dm_capkykhenthuong.Where(x => x.daXoa == false).ToList();

            ExcelWorksheet sheet1 = ep.Workbook.Worksheets.Add("Cấp thành tích");
            sheet1.Cells[row = 1, 1, row + 3, 2].Merge = true;
            sheet1.Cells["A" + row.ToString()].Value = "Danh mục cấp thành tích";
            sheet1.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 4;
            sheet1.Cells["A" + row.ToString()].Value = "Mã cấp thành tích";
            sheet1.Cells["B" + row.ToString()].Value = "Tên cấp thành tích";
            sheet1.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet1.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet1.Row(row).Style.Font.Bold = true;
            sheet1.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet1.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;

            row++;
            if (data1 != null && data1.Count > 0)
            {
                for (int i = 0; i < data1.Count; i++)
                {
                    {
                        sheet1.Cells["A" + row.ToString()].Value = data1[i].id;
                        sheet1.Cells["B" + row.ToString()].Value = data1[i].tenCapKyKhenThuong;

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


    }

}

