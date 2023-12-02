using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoTKTDService
{
    public class baoCaoTKDHModel
    {
        public int stt { get; set; }
        public string maNhanVien { get; set; }
        public string tenNhanVien { get; set; }
        public string capThanhTich { get; set; }
        public Nullable<int> maThanhTich { get; set; }
        public string ghiChu { get; set; }
        public string soQuyetDinh { get; set; }
        public string ngayBanHanh { get; set; }

        public string chuKy { get; set; }
        public string tuNam { get; set; }
        public string denNam { get; set; }
        public string nguoiKy { get; set; }


    }
}