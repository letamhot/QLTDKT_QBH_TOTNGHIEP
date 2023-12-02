using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class danhSachCaNhanTapThe
    {
        public int loaiDanhHieu { get; set; }
        //public int idTenDanhHieu { get; set; }
        //public string tenDanhHieu { get; set; }
        public List<dsChiTietCaNhanTapThe> dsChiTietCaNhanTapThe { get; set; }
    }
}