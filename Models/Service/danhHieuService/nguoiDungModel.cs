using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.danhHieuService
{
    public class nguoiDungModel
    {
        public int stt { get; set; }
        public int idNguoiDung { get; set; }
        public int idDonVi { get; set; }
        public string TenDonVi { get; set; }
        public string maNhanVien { get; set; }
        public string hoTen { get; set; }
        public DateTime? ngaySinh { get; set; }
        public string tenChucDanh { get; set; }
        public string namDanhHieu { get; set; }
        public string gioiTinh { get; set; }
        public string soDienThoai { get; set; }
        public string tenDonVi { get; set; }
        public string tienthuong { get; set; }
        public string trangThaiDanhHieu { get; set; }
    }
}