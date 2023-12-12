using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.sangKienService
{
    public class sangKienModel
    {
        public int id { get; set; }
        public int stt { get; set; }
        public string tenSangKien { get; set; }
        public string noiDungSangKien { get; set; }
        public int bophan { get; set; }
        public Nullable<int> loaiSangKien { get; set; }
        public string danhSachCaNhanTapThe { get; set; }
        public Nullable<double> tongTien { get; set; }
        public Nullable<bool> daXoa { get; set; }
        public string ngaySangKien { get; set; }
        public string soQuyetDinh { get; set; }
        public int idDmSangKien { get; set; }

    }
}