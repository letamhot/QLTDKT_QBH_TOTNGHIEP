using Newtonsoft.Json;
using QLTDKT.Models.Service.danhHieuService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoTKTDService
{
    public class BaoCaoTKDHService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();
        public List<baoCaoTKDHModel> getBaoCaoThongKeThiDua(DateTime from, DateTime to)
        {
            List<baoCaoTKDHModel> result = new List<baoCaoTKDHModel>();
            if (from.ToString("dd/MM") == "01/01" && to.ToString("dd/MM") == "31/12")
            {
                var dsNhanVien = _entities.qltdkt_dm_nhanvien.Where(x => x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01" && !(x.maNhanVien.Contains("CTV"))).ToList();
                var dsDanhHieu = _entities.qltdkt_danhhieu.Where(y => y.namDanhHieu >= from.Year && y.namDanhHieu <= to.Year && y.daTraoTang == true && y.daXoa == false).ToList();
                int i = 1;
                for (int j = 0; j < dsNhanVien.Count; j++)
                {
                    bool isCheck = false;
                    foreach (var item in dsDanhHieu)
                    {
                        var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);

                        if (item.idTenDanhHieu != null)
                        {

                            /*var test = from u in chiTietDanhHieu.dsChiTietCaNhanTapThe
                                       orderby u.id
                                       select u;*/
                            //chiTietDanhHieu.dsChiTietCaNhanTapThe.Sort();

                            foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                            {
                                if (item2.id == dsNhanVien[j].id)
                                {

                                    baoCaoTKDHModel tt = new baoCaoTKDHModel();
                                    if (isCheck)
                                    {
                                        tt.stt = 0;
                                        i--;
                                    }
                                    else
                                        tt.stt = i;
                                    tt.maNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).maNhanVien;
                                    tt.tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).hoTen;
                                    if (_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich != null)
                                    {
                                        tt.maThanhTich = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich;

                                    }
                                    else
                                    {
                                        tt.maThanhTich = 0;
                                    }
                                    if (_entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong != "")
                                    {
                                        tt.capThanhTich = _entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong;

                                    }
                                    else
                                    {
                                        tt.capThanhTich = "";
                                    }

                                    tt.ghiChu = "";

                                    tt.soQuyetDinh = item.soHieu;

                                    tt.ngayBanHanh = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    tt.chuKy = _entities.qltdkt_dm_chuky.Find(_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).chuKy).tenChuKy;
                                    tt.tuNam = item.tuNam;
                                    tt.denNam = item.denNam;
                                    tt.nguoiKy = item.nguoiKy;


                                    result.Add(tt);
                                    i++;
                                    isCheck = true;
                                    break;
                                }
                            }




                        }

                    }
                    if (!isCheck)
                    {
                        if (dsNhanVien[j].maNhanVien != "cntt_01")
                        {
                            baoCaoTKDHModel tt = new baoCaoTKDHModel();
                            tt.stt = i;
                            tt.maNhanVien = dsNhanVien[j].maNhanVien;
                            tt.tenNhanVien = dsNhanVien[j].hoTen;
                            tt.maThanhTich = 0;
                            tt.capThanhTich = "";
                            tt.ghiChu = "";
                            tt.soQuyetDinh = "";
                            tt.ngayBanHanh = "";
                            tt.tuNam = "";
                            tt.denNam = "";
                            tt.nguoiKy = "";
                            i++;

                            result.Add(tt);
                        }
                    }
                }
            }
            else
            {
                var dsNhanVien = _entities.qltdkt_dm_nhanvien.Where(x => x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01" && !(x.maNhanVien.Contains("CTV"))).ToList();
                var dsDanhHieu = _entities.qltdkt_danhhieu.Where(y => (DateTime)y.ngayDanhHieu >= from && (DateTime)y.ngayDanhHieu <= to && y.daTraoTang == true && y.daXoa == false).ToList();
                int i = 1;
                for (int j = 0; j < dsNhanVien.Count; j++)
                {
                    bool isCheck = false;
                    foreach (var item in dsDanhHieu)
                    {
                        var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);

                        if (item.idTenDanhHieu != null)
                        {


                            foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                            {
                                if (item2.id == dsNhanVien[j].id)
                                {

                                    baoCaoTKDHModel tt = new baoCaoTKDHModel();
                                    if (isCheck)
                                    {
                                        tt.stt = 0;
                                        i--;
                                    }
                                    else
                                        tt.stt = i;
                                    tt.maNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).maNhanVien;
                                    tt.tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).hoTen;
                                    if (_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich != null)
                                    {
                                        tt.maThanhTich = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich;

                                    }
                                    else
                                    {
                                        tt.maThanhTich = 0;
                                    }
                                    if (_entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong != "")
                                    {
                                        tt.capThanhTich = _entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong;

                                    }
                                    else
                                    {
                                        tt.capThanhTich = "";
                                    }

                                    tt.ghiChu = "";

                                    tt.soQuyetDinh = item.soHieu;

                                    tt.ngayBanHanh = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    tt.chuKy = _entities.qltdkt_dm_chuky.Find(_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).chuKy).tenChuKy;
                                    tt.tuNam = item.tuNam;
                                    tt.denNam = item.denNam;
                                    tt.nguoiKy = item.nguoiKy;


                                    result.Add(tt);
                                    i++;
                                    isCheck = true;
                                    break;
                                }
                            }




                        }

                    }
                    if (!isCheck)
                    {
                        if (dsNhanVien[j].maNhanVien != "cntt_01")
                        {
                            baoCaoTKDHModel tt = new baoCaoTKDHModel();
                            tt.stt = i;
                            tt.maNhanVien = dsNhanVien[j].maNhanVien;
                            tt.tenNhanVien = dsNhanVien[j].hoTen;
                            tt.maThanhTich = 0;
                            tt.capThanhTich = "";
                            tt.ghiChu = "";
                            tt.soQuyetDinh = "";
                            tt.ngayBanHanh = "";
                            tt.tuNam = "";
                            tt.denNam = "";
                            tt.nguoiKy = "";
                            i++;

                            result.Add(tt);
                        }
                    }
                }
            }
            return result;

        }
        public List<baoCaoTKDHModel> getBaoCaoThongKeThiDuaVNPTQB(DateTime from, DateTime to)
        {
            List<baoCaoTKDHModel> result = new List<baoCaoTKDHModel>();
            if (from.ToString("dd/MM") == "01/01" && to.ToString("dd/MM") == "31/12")
            {
                var dsNhanVien = _entities.qltdkt_dm_nhanvien.Where(x => x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01").OrderBy(x => x.maNhanVien.Contains("CTV")).ToList();
                var dsDanhHieu = _entities.qltdkt_danhhieu.Where(y => y.namDanhHieu >= from.Year && y.namDanhHieu <= to.Year && y.daTraoTang == true && y.daXoa == false).ToList();
                int i = 1;
                for (int j = 0; j < dsNhanVien.Count; j++)
                {
                    bool isCheck = false;
                    foreach (var item in dsDanhHieu)
                    {
                        var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);

                        if (item.idTenDanhHieu != null)
                        {

                            /*var test = from u in chiTietDanhHieu.dsChiTietCaNhanTapThe
                                       orderby u.id
                                       select u;*/
                            //chiTietDanhHieu.dsChiTietCaNhanTapThe.Sort();

                            foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                            {
                                if (item2.id == dsNhanVien[j].id)
                                {

                                    baoCaoTKDHModel tt = new baoCaoTKDHModel();
                                    if (isCheck)
                                    {
                                        tt.stt = 0;
                                        i--;
                                    }
                                    else
                                        tt.stt = i;
                                    tt.maNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).maNhanVien;
                                    tt.tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).hoTen;
                                    if (_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich != null)
                                    {
                                        tt.maThanhTich = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich;

                                    }
                                    else
                                    {
                                        tt.maThanhTich = 0;
                                    }
                                    if (_entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong != "")
                                    {
                                        tt.capThanhTich = _entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong;

                                    }
                                    else
                                    {
                                        tt.capThanhTich = "";
                                    }

                                    tt.ghiChu = "";

                                    tt.soQuyetDinh = item.soHieu;

                                    tt.ngayBanHanh = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    tt.chuKy = _entities.qltdkt_dm_chuky.Find(_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).chuKy).tenChuKy;
                                    tt.tuNam = item.tuNam;
                                    tt.denNam = item.denNam;
                                    tt.nguoiKy = item.nguoiKy;


                                    result.Add(tt);
                                    i++;
                                    isCheck = true;
                                    break;
                                }
                            }




                        }

                    }
                    if (!isCheck)
                    {
                        if (dsNhanVien[j].maNhanVien != "cntt_01")
                        {
                            baoCaoTKDHModel tt = new baoCaoTKDHModel();
                            tt.stt = i;
                            tt.maNhanVien = dsNhanVien[j].maNhanVien;
                            tt.tenNhanVien = dsNhanVien[j].hoTen;
                            tt.maThanhTich = 0;
                            tt.capThanhTich = "";
                            tt.ghiChu = "";
                            tt.soQuyetDinh = "";
                            tt.ngayBanHanh = "";
                            tt.tuNam = "";
                            tt.denNam = "";
                            tt.nguoiKy = "";
                            i++;

                            result.Add(tt);
                        }
                    }
                }
            }
            else
            {
                var dsNhanVien = _entities.qltdkt_dm_nhanvien.Where(x => x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01").OrderBy(x => x.maNhanVien.Contains("CTV")).ToList();
                var dsDanhHieu = _entities.qltdkt_danhhieu.Where(y => (DateTime)y.ngayDanhHieu >= from && (DateTime)y.ngayDanhHieu <= to && y.daTraoTang == true && y.daXoa == false).ToList();
                int i = 1;
                for (int j = 0; j < dsNhanVien.Count; j++)
                {
                    bool isCheck = false;
                    foreach (var item in dsDanhHieu)
                    {
                        var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);

                        if (item.idTenDanhHieu != null)
                        {


                            foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                            {
                                if (item2.id == dsNhanVien[j].id)
                                {

                                    baoCaoTKDHModel tt = new baoCaoTKDHModel();
                                    if (isCheck)
                                    {
                                        tt.stt = 0;
                                        i--;
                                    }
                                    else
                                        tt.stt = i;
                                    tt.maNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).maNhanVien;
                                    tt.tenNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).hoTen;
                                    if (_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich != null)
                                    {
                                        tt.maThanhTich = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).idThanhTich;

                                    }
                                    else
                                    {
                                        tt.maThanhTich = 0;
                                    }
                                    if (_entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong != "")
                                    {
                                        tt.capThanhTich = _entities.qltdkt_dm_capkykhenthuong.Find(_entities.qltdkt_dm_danhhieuthidua.Find(_entities.qltdkt_danhhieu.Find(item.id).idTenDanhHieu).capThanhTich).tenCapKyKhenThuong;

                                    }
                                    else
                                    {
                                        tt.capThanhTich = "";
                                    }

                                    tt.ghiChu = "";

                                    tt.soQuyetDinh = item.soHieu;

                                    tt.ngayBanHanh = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    tt.chuKy = _entities.qltdkt_dm_chuky.Find(_entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).chuKy).tenChuKy;
                                    tt.tuNam = item.tuNam;
                                    tt.denNam = item.denNam;
                                    tt.nguoiKy = item.nguoiKy;


                                    result.Add(tt);
                                    i++;
                                    isCheck = true;
                                    break;
                                }
                            }




                        }

                    }
                    if (!isCheck)
                    {
                        if (dsNhanVien[j].maNhanVien != "cntt_01")
                        {
                            baoCaoTKDHModel tt = new baoCaoTKDHModel();
                            tt.stt = i;
                            tt.maNhanVien = dsNhanVien[j].maNhanVien;
                            tt.tenNhanVien = dsNhanVien[j].hoTen;
                            tt.maThanhTich = 0;
                            tt.capThanhTich = "";
                            tt.ghiChu = "";
                            tt.soQuyetDinh = "";
                            tt.ngayBanHanh = "";
                            tt.tuNam = "";
                            tt.denNam = "";
                            tt.nguoiKy = "";
                            i++;

                            result.Add(tt);
                        }
                    }
                }
            }
            return result;

        }
    }
}