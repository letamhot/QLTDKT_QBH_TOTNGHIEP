//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QLTDKT.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class qltdkt_thidua
    {
        public int id { get; set; }
        public string quyetDinh { get; set; }
        public int phatDongId { get; set; }
        public int kieuThiDua { get; set; }
        public string chiTietKieu { get; set; }
        public Nullable<int> idDmThiDua { get; set; }
        public string soHieu { get; set; }
        public Nullable<System.DateTime> ngayPhatDong { get; set; }
        public Nullable<System.DateTime> ngayKetThuc { get; set; }
        public Nullable<int> thoiGianThucHien { get; set; }
        public string noiDungPhatDong { get; set; }
        public string chiTieuThiDua { get; set; }
        public string hinhThucKhenThuong { get; set; }
        public Nullable<byte> doiTuongThamGia { get; set; }
        public string nguoiKy { get; set; }
        public string vanBanLuu { get; set; }
        public Nullable<System.DateTime> ngayTao { get; set; }
        public Nullable<System.DateTime> ngayCapNhat { get; set; }
        public Nullable<byte> trangThai { get; set; }
        public Nullable<bool> daXoa { get; set; }
    }
}
