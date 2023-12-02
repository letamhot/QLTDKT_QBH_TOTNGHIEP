using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.soKyYeuService
{
    public class soKyYeuModel
    {
        public int kieuKyYeu { get; set; } //1: Thi đua, 2: Khen thưởng, 3: Danh hiệu
        public DateTime ngayKyYeu { get; set; }
        public int idThanhTich { get; set; }
        public int thanhTichDatDuoc { get; set; }
        public string hinhThucTraoTang { get; set; }
        public DateTime ngayTraoTang { get; set; }
        public string hinhAnhToChuc { get; set; }
        public decimal? tienThuong { get; set; }
        public string tenDanhHieu { get; set; }
        public string idTenDanhHieu { get; set; }
        public int namDanhHieu { get; set; }
        public string capKyKhenThuong { get; set; }
        public string hinhAnhTraoTang { get; set; }

        public string capKhenThuong { get; set; }
        public int idDanhHieu { get; set; }
        public string tenKhenThuong { get; set; }
        public int idKhenThuong { get; set; }

        public Nullable<bool> daXoa { get; set; }
    }
}