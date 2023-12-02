using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.khenThuongService
{
    public class khenThuongModel
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string capKhenThuong { get; set; }
        public string soHieu { get; set; }
        public string ngayKhenThuong { get; set; }
        public string fileGoc { get; set; }
        public string noiDungKhenThuong { get; set; }
        public string danhSachCaNhanTapThe { get; set; }
        public string capKyKhenThuong { get; set; }
        public string quyetDinh { get; set; }
        public int trangThai { get; set; }
        public bool daTraoTang { get; set; }
        public Nullable<int> tongTien { get; set; }

        public byte kieuKhenThuong { get; set; }
        public Nullable<byte> doiTuongThamGia { get; set; }
        public Nullable<int> hinhThucKhenThuong { get; set; }
        public Nullable<int> bophan { get; set; }
        public string ghiChuHTKT { get; set; }
    }

}