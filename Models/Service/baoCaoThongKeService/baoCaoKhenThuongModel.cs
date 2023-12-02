using QLTDKT.Models.Service.KhenThuongService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class baoCaoKhenThuongModel
    {

        public string sqd { get; set; }
        public int stt { get; set; }
        public int id { get; set; }
        public string tenDonVi_CaNhan { get; set; }
        public string quyetDinh { get; set; }
        public string donVi { get; set; }
        public string donViCha { get; set; }
        public string tenKhenThuong { get; set; }
        public string ngayBH { get; set; }
        public string hinhThucKhenThuong { get; set; }
        public string kieuKhenThuong { get; set; }

        public int loai { get; set; }
        public string tienthuong { get; set; }
        public Nullable<int> tongtienthuong { get; set; }
        public Nullable<int> bophan { get; set; }
        public string capKhenThuong { get; set; }
        public dstapthecanhankt danhSachCaNhanTapThe { get; set; }

    }
}