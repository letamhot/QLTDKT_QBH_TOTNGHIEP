using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.danhHieuService
{
    public class danhHieuModel
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string capDanhHieu { get; set; }
        public string soHieu { get; set; }
        public string ngayDanhHieu { get; set; }
        public string fileGoc { get; set; }
        public string noiDungDanhHieu { get; set; }
        public string danhSachCaNhanTapThe { get; set; }
        public string capKyKhenThuong { get; set; }
        public bool daTraoTang { get; set; }
        public int idTenDanhHieu { get; set; }
        public string tenDanhHieu { get; set; }
        public string ghiChuTT { get; set; }
        public int namDanhHieu { get; set; }
        public string tuNam { get; set; }
        public string denNam { get; set; }
        public string nguoiKy { get; set; }
        public string hinhAnhTraoTang { get; set; }
        public Nullable<byte> kieuDanhHieu { get; set; }
        public Nullable<int> bophan { get; set; }
        public Nullable<byte> hinhThucTraoTang { get; set; }
        public string xemhinhThucTraoTang { get; set; }
        public string ngayTraoTang { get; set; }
    }
}