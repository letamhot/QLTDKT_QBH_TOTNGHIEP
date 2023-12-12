using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.sangKienService
{
    public class danhSachCaNhanTapThe
    {
        public int loaiSangKien { get; set; }
        public double tongtien { get; set; }

        public List<dsChiTietCaNhanTapThe> dsChiTietCaNhanTapThe { get; set; }
    }
}