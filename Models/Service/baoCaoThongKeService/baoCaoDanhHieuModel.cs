using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class baoCaoDanhHieuModel
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string tenDonVi_CaNhan { get; set; }
        public string donVi { get; set; }
        public string tenDanhHieu { get; set; }
        public string ngaySinh { get; set; }
        public string chucVu { get; set; }
        public string hinhThucTraoTang { get; set; }
        public string capDanhHieu { get; set; }
        public int loaiDanhHieu { get; set; }
        public string ngayDanhHieu { get; set; }
        public int namDanhHieu { get; set; }
        public Nullable<int> bophan { get; set; }
        public string capKyKhenThuong { get; set; }
        public string soQuyetDinh { get; set; }
        public string fileGoc { get; set; }
    }
}