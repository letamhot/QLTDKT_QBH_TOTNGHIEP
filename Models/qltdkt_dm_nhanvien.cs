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
    
    public partial class qltdkt_dm_nhanvien
    {
        public int id { get; set; }
        public string maNhanVien { get; set; }
        public string hoTen { get; set; }
        public string email { get; set; }
        public int chucVuId { get; set; }
        public int donViId { get; set; }
        public string soDienThoai { get; set; }
        public Nullable<System.DateTime> ngayTao { get; set; }
        public Nullable<System.DateTime> ngayCapNhat { get; set; }
        public Nullable<bool> daXoa { get; set; }
        public Nullable<System.DateTime> ngaySinh { get; set; }
        public Nullable<bool> gioiTinh { get; set; }
        public string anhDaiDien { get; set; }
        public Nullable<byte> trangThai { get; set; }
        public string trinhDoHocVan { get; set; }
    }
}
