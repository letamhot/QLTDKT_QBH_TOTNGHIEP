using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.thiDuaService
{
    public class thiDuaModel
    {
        public int stt { get; set; }
        public int id { get; set; }
        public string capPhatDong { get; set; }
        public string noiDungPhatDong { get; set; }
        public string soHieu { get; set; }
        public string ngayPhatDong { get; set; }
        public string ngayKetThuc { get; set; }
        public string quyetDinh { get; set; }
        public int trangThai { get; set; }
    }
    public class editThiDuaModel
    {
        public Nullable<int> idDmThiDua { get; set; }
        public int id { get; set; }
        public int kieuThiDua { get; set; }
        public int capPhatDong { get; set; }
        public string noiDungPhatDong { get; set; }
        public string soHieu { get; set; }
        public string ngayPhatDong { get; set; }
        public string ngayKetThuc { get; set; }
        public string quyetDinh { get; set; }
        public int trangThai { get; set; }
        public Nullable<byte> doiTuongThamGia { get; set; }
        public string nguoiKy { get; set; }
        public string hinhThucKhenThuong { get; set; }

    }

    public class chiTietDangKyThiDua
    {
        public int idNhanVien { get; set; }
        public bool isDangKy { get; set; }
        public bool daBaoCao { get; set; }
        public string tenNhanVien { get; set; }
        public string noiDungDangKy { get; set; }
        public int xepHang { get; set; }
        public string nhanXet { get; set; }
        public string tenDonVi { get; set; }
    }

    public class chiTietBaoCaoThanhTich
    {
        public int idDonViDangKy { get; set; }

        public string tenDonViDangKy { get; set; }
        public string xepHang { get; set; }
        public string nhanXet { get; set; }
        public bool daBaoCao { get; set; }
        public List<chiTietDangKyThiDua> listCaNhanDangKy { get; set; }
    }

    public class chiTietDonViDaDangKy
    {
        public int stt { get; set; }
        public int idDonVi { get; set; }
        public string tenDonViDaDangKy { get; set; }
        public string ngayDangKy { get; set; }
        public string fileDinhKem { get; set; }

    }

    public class chiTietThiDua
    {
        public bool isDangKy { get; set; }

        public string kieuThiDua { get; set; }
        public int idKieuThiDua { get; set; }
        public string tenThiDua { get; set; }
        public string soHieu { get; set; }
        public string ngayPhatDong { get; set; }
        public List<chiTietBaoCaoThanhTich> dsDangKy { get; set; }
    }

    public class chiTietThiDuaBaoCao : chiTietThiDua
    {
        public bool isBaoCao { get; set; }
        public List<lsDonViCaNhan> dsDonViCaNhan { get; set; }
        public string lsFileBaoCao { get; set; }
    }

    public class dangKyThiDuaModel
    {
        public int id { get; set; }
        public int thiDuaId { get; set; }
        public int donViDangKyId { get; set; }
        public string noiDungDangKy { get; set; }
        public Nullable<bool> daBaoCao { get; set; }
        public string caNhanDangKy_KetQua { get; set; }
        public string ngayDangKy { get; set; }
        public string fileDinhKem { get; set; }
        public string soHieu { get; set; }
        public Nullable<System.DateTime> ngayKetQua { get; set; }
        public Nullable<System.DateTime> ngayTao { get; set; }
        public Nullable<byte> trangThaiThiDua { get; set; }
        public Nullable<byte> xepHangThiDua { get; set; }
        public Nullable<System.DateTime> ngayCapNhat { get; set; }
        public string nhanXetChung { get; set; }
        public string nguoiKyDanhGia { get; set; }
        public string noiDungDanhGia { get; set; }
        public string fileBaoCao { get; set; }
        public Nullable<bool> daXoa { get; set; }
        public Nullable<bool> isCaNhanDangKy { get; set; }
        public List<chiTietDangKyThiDua> listCaNhanDangKy { get; set; }
    }

    public class ketQuaThiDuaDK
    {
        public string lsTT { get; set; }
        public string lsCN { get; set; }
    }

    public class fileDownLoad
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }

    public class lsDonViCaNhan
    {
        public int id { get; set; }
        public string name { get; set; }
        public int type { get; set; }

    }

    public class baoCaoThiDuaDisplay
    {
        public int stt { get; set; }
        public int idBaoCaoThiDua { get; set; }
        public Nullable<byte> trangThaiTD { get; set; }

        public string tenThiDua { get; set; }
        public string soHieu { get; set; }
        public string tenBaoCao { get; set; }
        public string fileBaoCao { get; set; }

    }

    public class hoSoThiDuaDisplay
    {

        public int idHoSoThiDua { get; set; }
        public string tenHoSoThiDua { get; set; }
        public string tenThiDua { get; set; }
        public string soHieu { get; set; }
        public string fileBaoCaoThanhTich { get; set; }
        public string vanBanHuongDan { get; set; }
        public int thiDuaId { get; set; }
        public int kieuThiDua { get; set; }
        public string toTrinh { get; set; }
        public string bienBanHopHoiDong { get; set; }
        public string sangKien { get; set; }
        public int trangThai { get; set; }

        public Nullable<byte> trangThaiTD { get; set; }


        public string xacNhanThue { get; set; }

        public string fileDinhKem { get; set; }
        public List<baoCaoThiDuaDisplay> dsBaoCao { get; set; }

        public DateTime ngayPhatDong { get; set; }
    }

    public class danhSachVanBan
    {

        public int id { get; set; }

        public string tenVB { get; set; }
        public string fileVB { get; set; }
    }
}