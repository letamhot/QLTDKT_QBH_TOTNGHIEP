using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLTDKT.Models;
using QLTDKT.Models.Service.baoCaoTKTDService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class BaoCaoThongKeThiDuaController : Controller
    {
        private BaoCaoTKDHService _bctkService = new BaoCaoTKDHService();
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();

        // GET: BaoCaoThongKeThiDua
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult bcThongKeDanhHieu()
        {
            string startDate = Request.QueryString["from"];
            string endDate = Request.QueryString["to"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return Json(new { data = _bctkService.getBaoCaoThongKeThiDua(from, to) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcThongKeDanhHieuVNPTQB()
        {
            string startDate = Request.QueryString["from"];
            string endDate = Request.QueryString["to"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            return Json(new { data = _bctkService.getBaoCaoThongKeThiDuaVNPTQB(from, to) }, JsonRequestBehavior.AllowGet);
        }

        public void ExportExcelBaoCaoThongke()
        {
            string startDate = Request.QueryString["from"];
            string endDate = Request.QueryString["to"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var data = _bctkService.getBaoCaoThongKeThiDua(from, to);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            //sheet.Column(1).Width = 5;
            //sheet.Column(2).Width = 35;
            //sheet.Column(3).Width = 55;
            //sheet.Column(4).Width = 35;
            //sheet.Column(5).Width = 20;
            //sheet.Column(6).Width = 30;
            sheet.Cells[row, 1, row + 3, 12].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "Báo Cáo Thống Kê Danh Hiệu";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 4;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Mã nhân viên";
            sheet.Cells["C" + row.ToString()].Value = "Tên nhân viên";
            sheet.Cells["D" + row.ToString()].Value = "Cấp thành tích";
            sheet.Cells["E" + row.ToString()].Value = "Mã thành tích";
            sheet.Cells["F" + row.ToString()].Value = "Ghi chú(Nếu có Thành tích khác)";
            sheet.Cells["G" + row.ToString()].Value = "Số Quyết định";
            sheet.Cells["H" + row.ToString()].Value = "Ngày ban hành";
            sheet.Cells["I" + row.ToString()].Value = "Chu kỳ";
            sheet.Cells["J" + row.ToString()].Value = "Từ năm";
            sheet.Cells["K" + row.ToString()].Value = "Đến năm";
            sheet.Cells["L" + row.ToString()].Value = "Người ký";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 12].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 12].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].stt != 0)
                    {
                        sheet.Cells["A" + row.ToString()].Value = data[i].stt;

                    }
                    else
                    {
                        sheet.Cells["A" + row.ToString()].Value = "";
                    }
                    sheet.Cells["B" + row.ToString()].Value = data[i].maNhanVien;
                    sheet.Cells["C" + row.ToString()].Value = data[i].tenNhanVien;
                    sheet.Cells["D" + row.ToString()].Value = data[i].capThanhTich;
                    if (data[i].maThanhTich == 0)
                    {
                        sheet.Cells["E" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["E" + row.ToString()].Value = data[i].maThanhTich;

                    }
                    sheet.Cells["F" + row.ToString()].Value = data[i].ghiChu;
                    sheet.Cells["G" + row.ToString()].Value = data[i].soQuyetDinh;
                    sheet.Cells["H" + row.ToString()].Value = data[i].ngayBanHanh;
                    if (data[i].chuKy == null)
                    {
                        sheet.Cells["I" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["I" + row.ToString()].Value = data[i].chuKy;

                    }
                    if (data[i].tuNam == null)
                    {
                        sheet.Cells["J" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["J" + row.ToString()].Value = data[i].tuNam;

                    }
                    if (data[i].denNam == null)
                    {
                        sheet.Cells["K" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["K" + row.ToString()].Value = data[i].denNam;

                    }
                    if (data[i].nguoiKy == null)
                    {
                        sheet.Cells["L" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["L" + row.ToString()].Value = data[i].nguoiKy;

                    }
                    sheet.Cells[row, 1, row, 12].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 12].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "BaoCaoThiDuaKhenThuong.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();



        }
        public void ExportExcelBaoCaoThongkeVNPTQB()
        {
            string startDate = Request.QueryString["from"];
            string endDate = Request.QueryString["to"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            var data = _bctkService.getBaoCaoThongKeThiDuaVNPTQB(from, to);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            //sheet.Column(1).Width = 5;
            //sheet.Column(2).Width = 35;
            //sheet.Column(3).Width = 55;
            //sheet.Column(4).Width = 35;
            //sheet.Column(5).Width = 20;
            //sheet.Column(6).Width = 30;
            sheet.Cells[row, 1, row + 3, 12].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "Báo Cáo Thống Kê Danh Hiệu";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 4;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Mã nhân viên";
            sheet.Cells["C" + row.ToString()].Value = "Tên nhân viên";
            sheet.Cells["D" + row.ToString()].Value = "Cấp thành tích";
            sheet.Cells["E" + row.ToString()].Value = "Mã thành tích";
            sheet.Cells["F" + row.ToString()].Value = "Ghi chú(Nếu có Thành tích khác)";
            sheet.Cells["G" + row.ToString()].Value = "Số Quyết định";
            sheet.Cells["H" + row.ToString()].Value = "Ngày ban hành";
            sheet.Cells["I" + row.ToString()].Value = "Chu kỳ";
            sheet.Cells["J" + row.ToString()].Value = "Từ năm";
            sheet.Cells["K" + row.ToString()].Value = "Đến năm";
            sheet.Cells["L" + row.ToString()].Value = "Người ký";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 12].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 12].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].stt != 0)
                    {
                        sheet.Cells["A" + row.ToString()].Value = data[i].stt;

                    }
                    else
                    {
                        sheet.Cells["A" + row.ToString()].Value = "";
                    }
                    sheet.Cells["B" + row.ToString()].Value = data[i].maNhanVien;
                    sheet.Cells["C" + row.ToString()].Value = data[i].tenNhanVien;
                    sheet.Cells["D" + row.ToString()].Value = data[i].capThanhTich;
                    if (data[i].maThanhTich == 0)
                    {
                        sheet.Cells["E" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["E" + row.ToString()].Value = data[i].maThanhTich;

                    }
                    sheet.Cells["F" + row.ToString()].Value = data[i].ghiChu;
                    sheet.Cells["G" + row.ToString()].Value = data[i].soQuyetDinh;
                    sheet.Cells["H" + row.ToString()].Value = data[i].ngayBanHanh;
                    if (data[i].chuKy == null)
                    {
                        sheet.Cells["I" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["I" + row.ToString()].Value = data[i].chuKy;

                    }
                    if (data[i].tuNam == null)
                    {
                        sheet.Cells["J" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["J" + row.ToString()].Value = data[i].tuNam;

                    }
                    if (data[i].denNam == null)
                    {
                        sheet.Cells["K" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["K" + row.ToString()].Value = data[i].denNam;

                    }
                    if (data[i].nguoiKy == null)
                    {
                        sheet.Cells["L" + row.ToString()].Value = "";
                    }
                    else
                    {
                        sheet.Cells["L" + row.ToString()].Value = data[i].nguoiKy;

                    }
                    sheet.Cells[row, 1, row, 12].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 12].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 12].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 12].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "BaoCaoThiDuaKhenThuong.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();



        }
    }
}