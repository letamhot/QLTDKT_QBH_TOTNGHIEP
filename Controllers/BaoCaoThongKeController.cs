using QLTDKT.Models;
using QLTDKT.Models.Service.baoCaoThongKeService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using QLTDKT.Models.Service.danhHieuService;
using System.IO;
using Newtonsoft.Json;

namespace QLTDKT.Controllers
{
    public class BaoCaoThongKeController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private baoCaoThongKeService _bctkService = new baoCaoThongKeService();
        // GET: BaoCaoThongKe
        public ActionResult Index()
        {
            var listbc = _bctkService.ListAllBaoCao();
            return View(listbc);
        }
        public ActionResult BaoCao(int id)
        {
            switch (id)
            {
                case 1: return RedirectToAction("DanhSachTapTheTrucThuocVNPTQuangBinh", "BaoCaoThongKe");
                case 2: return RedirectToAction("DanhSachTapTheToPhongTrucThuocVNPTQuangBinh", "BaoCaoThongKe");
                case 3: return RedirectToAction("DanhSachNguoiLaoDongTaiCacToDonViVNPTQuangBinh", "BaoCaoThongKe");
                case 4: return RedirectToAction("DanhSachLaoDongTienTien", "BaoCaoThongKe");
                case 5: return RedirectToAction("DanhSachChienSyThiDuaCoSo", "BaoCaoThongKe");
                case 6: return RedirectToAction("DanhSachTapTheLaoDongTienTien", "BaoCaoThongKe");
                case 7: return RedirectToAction("DanhSachTapTheLaoDongXuatSac", "BaoCaoThongKe");
            }
            return View("Index");
        }
        public ActionResult DanhSachTapTheTrucThuocVNPTQuangBinh()
        {
            return View();
        }
        public ActionResult DanhSachTapTheToPhongTrucThuocVNPTQuangBinh()
        {
            return View();
        }
        public ActionResult DanhSachNguoiLaoDongTaiCacToDonViVNPTQuangBinh()
        {
            return View();
        }
        public ActionResult DanhSachLaoDongTienTien()
        {
            return View();
        }
        public ActionResult DanhSachChienSyThiDuaCoSo()
        {
            return View();
        }
        public ActionResult DanhSachTapTheLaoDongTienTien()
        {
            return View();
        }
        public ActionResult DanhSachTapTheLaoDongXuatSac()
        {
            return View();
        }
        public ActionResult BaoCaoDanhHieu()
        {
            return View();
        }
        public ActionResult BaoCaoKhenThuong()
        {
            return View();
        }
        public JsonResult bcDanhSachTapTheTrucThuocVNPTQB()
        {
            return Json(new { data = _bctkService.bcDanhSachTapTheTrucThuocVNPTQB() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcDanhSachTapTheToPhongTrucThuocVNPTQB()
        {
            return Json(new { data = _bctkService.bcDanhSachTapTheToPhongTrucThuocVNPTQB() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcDanhSachNguoiLaoDong(int donViCap1, int donViCap2, int donViCap3)
        {
            return Json(new { data = _bctkService.bcDanhSachNguoiLaoDong(donViCap1, donViCap2, donViCap3) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcDanhSachLaoDongTienTien(int tuNam, int denNam)
        {
            return Json(new { data = _bctkService.bcDanhSachLaoDongTienTien(tuNam, denNam) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcDanhSachChienSyThiDuaCoSo(int tuNam, int denNam)
        {
            return Json(new { data = _bctkService.bcDanhSachChienSyThiDuaCoSo(tuNam, denNam) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcDanhSachTapTheLaoDongTienTien(int tuNam, int denNam)
        {
            return Json(new { data = _bctkService.bcDanhSachTapTheLaoDongTienTien(tuNam, denNam) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult bcDanhSachTapTheLaoDongXuatSac(int tuNam, int denNam)
        {
            return Json(new { data = _bctkService.bcDanhSachTapTheLaoDongXuatSac(tuNam, denNam) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetBaoCaoDanhHieu()
        {
            string startDate = Request.QueryString["from"];
            string endDate = Request.QueryString["to"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int loaiDanhHieu = int.Parse(Request.QueryString["loaiDanhHieu"]);
            int bophan = int.Parse(Request.QueryString["bophan"]);
            int tenDanhHieu = int.Parse(Request.QueryString["tenDanhHieu"]);
            int hinhThucKhenThuong = int.Parse(Request.QueryString["hinhThucKhenThuong"]);
            string soQuyetDinh = (Request.QueryString["soQuyetDinh"] != "" ? Request.QueryString["soQuyetDinh"] : "");

            return Json(new { data = _bctkService.getBaoCaoDanhHieu(from, to, bophan, loaiDanhHieu, tenDanhHieu, hinhThucKhenThuong, soQuyetDinh) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDmDanhHieuByKieu()
        {
            int kieuDanhHieu = (Request.QueryString["kieuDanhHieu"] != "" ? int.Parse(Request.QueryString["kieuDanhHieu"]) : -1);
            int bophan = (Request.QueryString["bophan"] != "0" ? int.Parse(Request.QueryString["bophan"]) : 0);
            return Json(new { data = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.loaiDanhHieu == kieuDanhHieu && x.bophan == bophan && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetBaoCaoKhenThuong()
        {
            string startDate = Request.QueryString["fromkt"];
            string endDate = Request.QueryString["tokt"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string loaiKhenThuong = Request.QueryString["loaiKhenThuong"];
            string capKhenThuong = Request.QueryString["capKhenThuong"];
            string kieuKhenThuong = Request.QueryString["kieuKhenThuong"];
            int bophan = int.Parse(Request.QueryString["bophan"]);


            return Json(new { data = _bctkService.getBaoCaoKhenThuong(from, to, bophan, loaiKhenThuong, capKhenThuong, kieuKhenThuong) }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult xemKhenThuong(int idKhenThuong)
        {
            //int idKhenThuong = int.Parse(Request["idKhenThuong"]);
            var data = _bctkService.xemKhenThuong(idKhenThuong);
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
        public JsonResult listAllDonViCap2()
        {
            var data = (from a in _entities.qltdkt_dm_donvi
                        where a.idCha == 1
                        select new donViModel
                        {
                            idDonVi = a.id,
                            tenDonVi = a.tenDonVi
                        }).ToList();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult listDonViByIDCha(int idDonVi)
        {
            var data = _bctkService.listDonViByIDCha(idDonVi);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDmHinhThucKhenThuong(byte kieutraotang)
        {
            return Json(new { data = _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == kieutraotang && x.daXoa == false).ToList() }, JsonRequestBehavior.AllowGet);
        }

        //export file chi tiết danh hiệu
        [HttpPost]
        [ValidateInput(false)]
        public EmptyResult ExportBaoCaoDanhHieu(string GridHtml)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            string FileName = "BaoCaoDanhHieu" + DateTime.Now.ToString("dd/MM/yyyy") + ".doc";
            Response.AddHeader("content-disposition",
                    "attachment;filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/msword ";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Output.Write(GridHtml);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }

        //export file chi tiết khen thưởng
        [HttpPost]
        [ValidateInput(false)]
        public EmptyResult ExportBaoCaoDSKhenThuong(string GridHtml)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            string FileName = "ExportBaoCaoDSKhenThuong" + DateTime.Now.ToString("dd/MM/yyyy") + ".doc";
            Response.AddHeader("content-disposition",
                    "attachment;filename=" + FileName);
            Response.Charset = "";
            Response.ContentType = "application/msword ";
            Response.ContentEncoding = System.Text.Encoding.UTF8;
            Response.Output.Write(GridHtml);
            Response.Flush();
            Response.End();
            return new EmptyResult();
        }
        public void ExportExcelBaoCaoKhenThuong()
        {

            DateTime from = DateTime.ParseExact(Request.QueryString["from"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(Request.QueryString["to"], "dd/MM/yyyy", CultureInfo.InvariantCulture);
            string capkhenthuong = Request.QueryString["capKhenThuong"];
            string loaikhenthuong = Request.QueryString["loaiKhenThuong"];
            string kieukhenthuong = Request.QueryString["kieuKhenThuong"];
            int bophan = int.Parse(Request.QueryString["bophan"]);

            var data = _bctkService.getBaoCaoKhenThuongTT(from, to, bophan, loaikhenthuong, capkhenthuong, kieukhenthuong);
            string cap = "";
            if (capkhenthuong != "0")
            {
                cap = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(capkhenthuong)).tenPhatDong;
            }
            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            if (loaikhenthuong == "0")
            {
                sheet.Cells[row, 1, row + 1, 8].Merge = true;
                sheet.Cells["A" + row.ToString()].Value = "BÁO CÁO KHEN THƯỞNG TẬP THỂ CẤP " + cap + " TỪ " + from + " ĐẾN " + to;
                sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                row = row + 2;
                sheet.Cells["A" + row.ToString()].Value = "STT";
                sheet.Cells["B" + row.ToString()].Value = "SỐ QUYẾT ĐỊNH";
                sheet.Cells["C" + row.ToString()].Value = "NGÀY BAN HÀNH";
                sheet.Cells["D" + row.ToString()].Value = "CẤP KHEN THƯỞNG";
                sheet.Cells["E" + row.ToString()].Value = "NỘI DUNG";
                sheet.Cells["F" + row.ToString()].Value = "TIỀN THƯỞNG";
                sheet.Cells["G" + row.ToString()].Value = "TẬP THỂ";
                sheet.Cells["H" + row.ToString()].Value = "BỘ PHẬN(1: Chuyên môn; 2: Đảng - Đoàn thể; 3: Công đoàn)";
                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 1, row, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //sheet.Cells[row, 1, row, 3].Style.b
                row++;
                int stt = 1;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.Cells["A" + row.ToString()].Value = stt;
                        sheet.Cells["B" + row.ToString()].Value = data[i].sqd;
                        sheet.Cells["C" + row.ToString()].Value = data[i].ngayBH;
                        sheet.Cells["D" + row.ToString()].Value = data[i].capKhenThuong;
                        sheet.Cells["E" + row.ToString()].Value = data[i].tenKhenThuong;
                        sheet.Cells["F" + row.ToString()].Value = data[i].tienthuong;
                        sheet.Cells["G" + row.ToString()].Value = data[i].tenDonVi_CaNhan;
                        sheet.Cells["H" + row.ToString()].Value = data[i].bophan;
                        stt++;
                        row++;
                    }
                }
            }
            if (loaikhenthuong == "1")
            {

                sheet.Cells[row, 1, row + 1, 7].Merge = true;
                sheet.Cells["A" + row.ToString()].Value = "BÁO CÁO KHEN THƯỞNG CÁ NHÂN CẤP " + cap + " TỪ " + from + " ĐẾN " + to;
                sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                row = row + 2;
                sheet.Cells["A" + row.ToString()].Value = "STT";
                sheet.Cells["B" + row.ToString()].Value = "SỐ QUYẾT ĐỊNH";
                sheet.Cells["C" + row.ToString()].Value = "NGÀY BAN HÀNH";
                sheet.Cells["D" + row.ToString()].Value = "CẤP KHEN THƯỞNG";
                sheet.Cells["E" + row.ToString()].Value = "NỘI DUNG";
                sheet.Cells["F" + row.ToString()].Value = "TIỀN THƯỞNG";
                sheet.Cells["G" + row.ToString()].Value = "CÁ NHÂN";
                sheet.Cells["H" + row.ToString()].Value = "BỘ PHẬN(0: Chuyên môn; 1: Đảng - Đoàn thể; 2: Công đoàn)";
                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 1, row, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //sheet.Cells[row, 1, row, 3].Style.b
                row++;
                int stt = 1;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.Cells["A" + row.ToString()].Value = stt;
                        sheet.Cells["B" + row.ToString()].Value = data[i].sqd;
                        sheet.Cells["C" + row.ToString()].Value = data[i].ngayBH;
                        sheet.Cells["D" + row.ToString()].Value = data[i].capKhenThuong;
                        sheet.Cells["E" + row.ToString()].Value = data[i].tenKhenThuong;
                        sheet.Cells["F" + row.ToString()].Value = data[i].tienthuong;
                        sheet.Cells["G" + row.ToString()].Value = data[i].tenDonVi_CaNhan;
                        sheet.Cells["H" + row.ToString()].Value = data[i].bophan;
                        stt++;
                        row++;
                    }
                }
            }
            if (loaikhenthuong == "2")
            {

                sheet.Cells[row, 1, row + 1, 8].Merge = true;
                sheet.Cells["A" + row.ToString()].Value = "BÁO CÁO KHEN THƯỞNG TẬP THỂ/CÁ NHÂN CẤP " + cap + " TỪ " + from + " ĐẾN " + to;
                sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                row = row + 2;
                sheet.Cells["A" + row.ToString()].Value = "STT";
                sheet.Cells["B" + row.ToString()].Value = "SỐ QUYẾT ĐỊNH";
                sheet.Cells["C" + row.ToString()].Value = "NGÀY BAN HÀNH";
                sheet.Cells["D" + row.ToString()].Value = "CẤP KHEN THƯỞNG";
                sheet.Cells["E" + row.ToString()].Value = "NỘI DUNG";
                sheet.Cells["F" + row.ToString()].Value = "TIỀN THƯỞNG";
                sheet.Cells["G" + row.ToString()].Value = "TẬP THỂ/CÁ NHÂN";
                sheet.Cells["H" + row.ToString()].Value = "BỘ PHẬN(0: Chuyên môn; 1: Đảng - Đoàn thể; 2: Công đoàn)";
                sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                sheet.Cells[row, 1, row, 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                sheet.Cells[row, 1, row, 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                //sheet.Cells[row, 1, row, 3].Style.b
                row++;
                int stt = 1;
                if (data != null && data.Count > 0)
                {
                    for (int i = 0; i < data.Count; i++)
                    {
                        sheet.Cells["A" + row.ToString()].Value = stt;
                        sheet.Cells["B" + row.ToString()].Value = data[i].sqd;
                        sheet.Cells["C" + row.ToString()].Value = data[i].ngayBH;
                        sheet.Cells["D" + row.ToString()].Value = data[i].capKhenThuong;
                        sheet.Cells["E" + row.ToString()].Value = data[i].tenKhenThuong;
                        sheet.Cells["F" + row.ToString()].Value = data[i].tienthuong;
                        sheet.Cells["G" + row.ToString()].Value = data[i].tenDonVi_CaNhan;
                        sheet.Cells["H" + row.ToString()].Value = data[i].bophan;
                        stt++;
                        row++;
                    }
                }
            }

            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "BaoCaoKhenThuong.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
        }

        public JsonResult xemDanhHieu()
        {
            int idDanhHieu = int.Parse(Request["idDanhHieu"]);
            var data = _bctkService.xemDanhHieu(idDanhHieu);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public void ExportExcelBaoCaoDanhHieu()
        {
            string startDate = Request.QueryString["from"];
            string endDate = Request.QueryString["to"];
            DateTime from = DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime to = DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            int loaiDanhHieu = int.Parse(Request.QueryString["loaiDanhHieu"]);
            int tenDanhHieu = int.Parse(Request.QueryString["tenDanhHieu"]);
            int hinhThucKhenThuong = int.Parse(Request.QueryString["hinhThucKhenThuong"]);
            string soQuyetDinh = Request.QueryString["soQuyetDinh"];
            int bophan = int.Parse(Request.QueryString["bophan"]);


            var data = _bctkService.getBaoCaoDanhHieuTT(from, to, bophan, loaiDanhHieu, tenDanhHieu, hinhThucKhenThuong, soQuyetDinh);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            //sheet.Column(1).Width = 5;
            //sheet.Column(2).Width = 35;
            //sheet.Column(3).Width = 55;
            //sheet.Column(4).Width = 35;
            //sheet.Column(5).Width = 20;
            //sheet.Column(6).Width = 30;
            sheet.Cells[row, 1, row + 1, 9].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "DANH SÁCH CÁ NHÂN/TẬP THỂ ĐẠT DANH HIỆU";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Số quyết định";
            sheet.Cells["C" + row.ToString()].Value = "Ngày quyết định";
            sheet.Cells["D" + row.ToString()].Value = "Tên danh hiệu";
            sheet.Cells["E" + row.ToString()].Value = "Tên tập thể/cá nhân";
            sheet.Cells["F" + row.ToString()].Value = "Đơn vị";
            sheet.Cells["G" + row.ToString()].Value = "Năm danh hiệu";
            sheet.Cells["H" + row.ToString()].Value = "Hình thức danh hiệu";
            sheet.Cells["I" + row.ToString()].Value = "BỘ PHẬN(0: Chuyên môn; 1: Đảng - Đoàn thể; 2: Công đoàn)";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            //a
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].soQuyetDinh;
                    sheet.Cells["C" + row.ToString()].Value = data[i].ngayDanhHieu;
                    sheet.Cells["D" + row.ToString()].Value = data[i].tenDanhHieu;
                    sheet.Cells["E" + row.ToString()].Value = data[i].tenDonVi_CaNhan;
                    sheet.Cells["F" + row.ToString()].Value = data[i].donVi;
                    sheet.Cells["G" + row.ToString()].Value = data[i].namDanhHieu;
                    sheet.Cells["H" + row.ToString()].Value = data[i].hinhThucTraoTang;
                    sheet.Cells["I" + row.ToString()].Value = data[i].bophan;
                    sheet.Cells[row, 1, row, 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "BaoCaoDanhHieu.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
        }

        public ActionResult ExportExcelDanhSachTapTheTrucThuocVNPTQuangBinh()
        {
            var data = _bctkService.bcDanhSachTapTheTrucThuocVNPTQB();

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Column(1).Width = 20;
            sheet.Column(2).Width = 200;
            sheet.Cells[row, 1, row + 1, 2].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "DANH SÁCH TẬP THỂ TRỰC THUỘC VNPT QUẢNG BÌNH";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Tên tập thể";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            row++;
            int stt = 1;

            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = data[i].stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].tenTapThe;
                    sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachTapTheTrucThuocVNPTQuangBinh.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachTapTheTrucThuocVNPTQuangBinh", "BaoCaoThongKe");
        }

        public ActionResult ExportExcelDanhSachTapTheToPhongTrucThuocVNPTQuangBinh()
        {
            var data = _bctkService.bcDanhSachTapTheToPhongTrucThuocVNPTQB();

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Column(1).Width = 20;
            sheet.Column(2).Width = 200;
            sheet.Cells[row, 1, row + 1, 2].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "DANH SÁCH TẬP THỂ TỔ/PHÒNG TRỰC THUỘC VNPT QUẢNG BÌNH";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Tên tập thể tổ/phòng";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].tenTapThe;
                    sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                    stt++;
                    row++;
                    if (data[i].dsDonViCap2.Count > 0)
                    {
                        for (int j = 0; j < data[i].dsDonViCap2.Count; j++)
                        {
                            sheet.Cells["A" + row.ToString()].Value = "";
                            sheet.Cells["B" + row.ToString()].Value = data[i].dsDonViCap2[j].tenTapThe;
                            sheet.Cells[row, 1, row, 2].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[row, 1, row, 2].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[row, 1, row, 2].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                            sheet.Cells[row, 1, row, 2].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                            row++;
                        }
                    }
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachTapTheToPhongTrucThuocVNPTQuangBinh.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachTapTheToPhongTrucThuocVNPTQuangBinh", "BaoCaoThongKe");
        }

        public ActionResult ExportExcelDanhSachNguoiLaoDongTaiCacToDonViVNPTQuangBinh(int donViCap1, int donViCap2, int donViCap3)
        {
            var data = _bctkService.bcDanhSachNguoiLaoDong(donViCap1, donViCap2, donViCap3);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Cells[row, 1, row + 1, 5].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "DANH SÁCH NGƯỜI LAO ĐỘNG TẠI CÁC TỔ/ĐƠN VỊ TRỰC THUỘC VNPT QUẢNG BÌNH";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Mã nhân viên";
            sheet.Cells["C" + row.ToString()].Value = "Họ tên";
            sheet.Cells["D" + row.ToString()].Value = "Chức vụ";
            sheet.Cells["E" + row.ToString()].Value = "Đơn vị";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].maNhanVien;
                    sheet.Cells["C" + row.ToString()].Value = data[i].hoTen;
                    sheet.Cells["D" + row.ToString()].Value = data[i].tenChucDanh;
                    sheet.Cells["E" + row.ToString()].Value = data[i].tenDonVi;
                    sheet.Cells[row, 1, row, 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachNguoiLaoDongTaiCacToDonViVNPTQuangBinh.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachNguoiLaoDongTaiCacToDonViVNPTQuangBinh", "BaoCaoThongKe");
        }

        public ActionResult ExportExcelDanhSachLaoDongTienTien(int tuNam, int denNam)
        {
            var data = _bctkService.bcDanhSachLaoDongTienTien(tuNam, denNam);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Cells[row, 1, row + 1, 6].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = "DANH SÁCH LAO ĐỘNG TIÊN TIẾN";
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Mã nhân viên";
            sheet.Cells["C" + row.ToString()].Value = "Họ tên";
            sheet.Cells["D" + row.ToString()].Value = "Chức vụ";
            sheet.Cells["E" + row.ToString()].Value = "Đơn vị";
            sheet.Cells["F" + row.ToString()].Value = "Năm danh hiệu";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].maNhanVien;
                    sheet.Cells["C" + row.ToString()].Value = data[i].hoTen;
                    sheet.Cells["D" + row.ToString()].Value = data[i].tenChucDanh;
                    sheet.Cells["E" + row.ToString()].Value = data[i].tenDonVi;
                    sheet.Cells["F" + row.ToString()].Value = data[i].namDanhHieu;
                    sheet.Cells[row, 1, row, 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachLaoDongTienTien.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachLaoDongTienTien", "BaoCaoThongKe");
        }
        public ActionResult ExportExcelDanhSachChienSyThiDuaCoSo(int tuNam, int denNam)
        {
            string title = "DANH SÁCH CHIẾN SỸ THI ĐUA CƠ SỞ";
            if (tuNam == denNam)
            {
                title += " NĂM " + tuNam;
            }
            else
            {
                title += " TỪ NĂM " + tuNam + " ĐẾN NĂM " + denNam;
            }
            var data = _bctkService.bcDanhSachChienSyThiDuaCoSo(tuNam, denNam);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Cells[row, 1, row + 1, 7].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = title;
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Mã nhân viên";
            sheet.Cells["C" + row.ToString()].Value = "Họ tên";
            sheet.Cells["D" + row.ToString()].Value = "Chức vụ";
            sheet.Cells["E" + row.ToString()].Value = "Đơn vị";
            sheet.Cells["F" + row.ToString()].Value = "Năm danh hiệu";
            sheet.Cells["G" + row.ToString()].Value = "Trạng thái";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
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
                    sheet.Cells["B" + row.ToString()].Value = data[i].maNhanVien;
                    sheet.Cells["C" + row.ToString()].Value = data[i].hoTen;
                    sheet.Cells["D" + row.ToString()].Value = data[i].tenChucDanh;
                    sheet.Cells["E" + row.ToString()].Value = data[i].tenDonVi;
                    sheet.Cells["F" + row.ToString()].Value = data[i].namDanhHieu;
                    sheet.Cells["G" + row.ToString()].Value = data[i].trangThaiDanhHieu;
                    sheet.Cells[row, 1, row, 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachChienSyThiDuaCoSo.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachChienSyThiDuaCoSo", "BaoCaoThongKe");
        }
        public ActionResult ExportExcelDanhSachTapTheLaoDongTienTien(int tuNam, int denNam)
        {
            string title = "DANH SÁCH TẬP THỂ LAO ĐỘNG TIÊN TIẾN";
            if (tuNam == denNam)
            {
                title += " NĂM " + tuNam;
            }
            else
            {
                title += " TỪ NĂM " + tuNam + " ĐẾN NĂM " + denNam;
            }
            var data = _bctkService.bcDanhSachTapTheLaoDongTienTien(tuNam, denNam);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Cells[row, 1, row + 1, 4].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = title;
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Tên tập thể";
            sheet.Cells["C" + row.ToString()].Value = "Tên đơn vị";
            sheet.Cells["D" + row.ToString()].Value = "Năm danh hiệu";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].tenDonVi;
                    sheet.Cells["C" + row.ToString()].Value = data[i].tenDonViCha;
                    sheet.Cells["D" + row.ToString()].Value = data[i].namDanhHieu;
                    sheet.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachTapTheLaoDongTienTien.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachTapTheLaoDongTienTien", "BaoCaoThongKe");
        }
        public ActionResult ExportExcelDanhSachTapTheLaoDongXuatSac(int tuNam, int denNam)
        {
            string title = "DANH SÁCH TẬP THỂ LAO ĐỘNG XUẤT SẮC";
            if (tuNam == denNam)
            {
                title += " NĂM " + tuNam;
            }
            else
            {
                title += " TỪ NĂM " + tuNam + " ĐẾN NĂM " + denNam;
            }
            var data = _bctkService.bcDanhSachTapTheLaoDongXuatSac(tuNam, denNam);

            ExcelPackage ep = new ExcelPackage();
            ExcelWorksheet sheet = ep.Workbook.Worksheets.Add("Report");
            int row = 1;
            sheet.Cells[row, 1, row + 1, 4].Merge = true;
            sheet.Cells["A" + row.ToString()].Value = title;
            sheet.Cells["A" + row.ToString()].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Cells["A" + row.ToString()].Style.VerticalAlignment = ExcelVerticalAlignment.Center;

            row = row + 2;
            sheet.Cells["A" + row.ToString()].Value = "STT";
            sheet.Cells["B" + row.ToString()].Value = "Tên tập thể";
            sheet.Cells["C" + row.ToString()].Value = "Tên đơn vị";
            sheet.Cells["D" + row.ToString()].Value = "Năm danh hiệu";
            sheet.Row(row).Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            sheet.Row(row).Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            sheet.Row(row).Style.Font.Bold = true;
            sheet.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
            sheet.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
            //sheet.Cells[row, 1, row, 3].Style.b
            row++;
            int stt = 1;
            if (data != null && data.Count > 0)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    sheet.Cells["A" + row.ToString()].Value = stt;
                    sheet.Cells["B" + row.ToString()].Value = data[i].tenDonVi;
                    sheet.Cells["C" + row.ToString()].Value = data[i].tenDonViCha;
                    sheet.Cells["D" + row.ToString()].Value = data[i].namDanhHieu;
                    sheet.Cells[row, 1, row, 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                    sheet.Cells[row, 1, row, 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                    stt++;
                    row++;
                }
            }
            sheet.Cells["A:AZ"].AutoFitColumns();
            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment: filename=" + "DanhSachTapTheLaoDongXuatSac.xlsx");
            Response.BinaryWrite(ep.GetAsByteArray());
            Response.End();
            return View("DanhSachTapTheLaoDongXuatSac", "BaoCaoThongKe");
        }
    }
}