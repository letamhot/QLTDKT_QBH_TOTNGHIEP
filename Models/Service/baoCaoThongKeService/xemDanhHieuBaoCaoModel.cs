using QLTDKT.Models.Service.DanhHieuService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class xemDanhHieuBaoCaoModel
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string capDanhHieu { get; set; }
        public string soHieu { get; set; }
        public string ngayDanhHieu { get; set; }
        public string fileGoc { get; set; }
        public string noiDungDanhHieu { get; set; }
        public danhSachCaNhanTapThe danhSachCaNhanTapThe { get; set; }

        public string capKyKhenThuong { get; set; }
        public bool daTraoTang { get; set; }
        public int idTenDanhHieu { get; set; }
        public Nullable<byte> loaiDanhHieu { get; set; }
        public string tenDanhHieu { get; set; }
        public string ghiChuTT { get; set; }
        public int namDanhHieu { get; set; }
        public string tuNam { get; set; }
        public string denNam { get; set; }
        public string nguoiKy { get; set; }
        public Nullable<byte> kieuDanhHieu { get; set; }
        public Nullable<int> bophan { get; set; }
        public Nullable<int> hinhThucTraoTang { get; set; }
        public string xemhinhThucTraoTang { get; set; }
        public string ngayTraoTang { get; set; }
    }
}