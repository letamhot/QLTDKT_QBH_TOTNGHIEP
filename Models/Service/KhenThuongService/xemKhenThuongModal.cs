using QLTDKT.Models.Service.KhenThuongService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.khenThuongService
{
    public class xemKhenThuongModal
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string capKhenThuong { get; set; }
        public string soHieu { get; set; }
        public string ngayKhenThuong { get; set; }
        public string quyetDinh { get; set; }
        public string noidungKhenThuong { get; set; }
        public dstapthecanhankt danhSachCaNhanTapThe { get; set; }
        public string capKyKhenThuong { get; set; }
        public string daTraoTang { get; set; }
        public Nullable<int> tongTienThuong { get; set; }
        public Nullable<int> tienThuong { get; set; }
        public string hinhThucKhenThuong { get; set; }
        public string nguoiKy { get; set; }
    }
}