using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.danhHieuTDService
{
    public class DanhHieuTDModel
    {


        public int idDanhHieu { get; set; }
        public Nullable<byte> loaiDanhHieu { get; set; }
        public string tenDanhHieuThiDua { get; set; }
        public Nullable<bool> luuSoKyYeu { get; set; }
        public string moTa { get; set; }
        public Nullable<System.DateTime> ngayTao { get; set; }
        public Nullable<bool> daXoa { get; set; }
        public Nullable<int> idThanhTich { get; set; }
        public Nullable<int> chuKy { get; set; }
        public Nullable<int> capThanhTich { get; set; }
        public Nullable<int> bophan { get; set; }
        public string chuKyDH { get; set; }
        public string capKyThanhTich { get; set; }

    }
}