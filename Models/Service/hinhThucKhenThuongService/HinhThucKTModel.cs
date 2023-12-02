using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.hinhThucKhenThuongService
{
    public class HinhThucKTModel
    {
        public int idHT { get; set; }
        public string tenHinhThucKhenThuong { get; set; }
        public string moTa { get; set; }
        public Nullable<System.DateTime> ngayTao { get; set; }
        public Nullable<bool> daXoa { get; set; }
        public Nullable<byte> loaiKhenThuong { get; set; }
        public Nullable<int> chuKy { get; set; }
        public string maThanhTich { get; set; }
        public Nullable<int> capThanhTich { get; set; }
        public Nullable<int> bophan { get; set; }
        public string chuKyKT { get; set; }
        public string capKyThanhTich { get; set; }
    }
}