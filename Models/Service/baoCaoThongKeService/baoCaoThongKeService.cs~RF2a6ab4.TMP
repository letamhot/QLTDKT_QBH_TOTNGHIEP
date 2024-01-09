using Newtonsoft.Json;
using QLTDKT.Models.Service.danhHieuService;
using QLTDKT.Models.Service.thiDuaService;
using QLTDKT.Models.Service.khenThuongService;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Data;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class baoCaoThongKeService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();

        public List<qltdkt_baocaothongke> ListAllBaoCao()
        {
            return _entities.qltdkt_baocaothongke.ToList();
        }

        public List<baoCaoThiDuaModel> getBaoCaoThongKeThiDua(DateTime from, DateTime to, int thiDua, int type)
        {
            List<baoCaoThiDuaModel> result = new List<baoCaoThiDuaModel>();
            var lsThiDua = _entities.qltdkt_thidua.Where(x => x.idDmThiDua == thiDua && (x.kieuThiDua == type || x.kieuThiDua == 2 || x.kieuThiDua == 1 || x.kieuThiDua == 0) && x.ngayPhatDong >= from && x.ngayPhatDong <= to && x.trangThai == 3).ToList();
            if (lsThiDua != null && lsThiDua.Count > 0)
            {
                for (int i = 0; i < lsThiDua.Count; i++)
                {
                    int idThiDua = lsThiDua[i].id;
                    qltdkt_hosothidua _hstd = _entities.qltdkt_hosothidua.FirstOrDefault(x => x.thiDuaId == idThiDua && x.daXoa == false);
                    if (_hstd != null)
                    {
                        var chiTietBaoCao = JsonConvert.DeserializeObject<List<lsDonViCaNhan>>(_hstd.chiTietBaoCaoThanhTich);
                        if (chiTietBaoCao != null && chiTietBaoCao.Count > 0)
                        {
                            for (int j = 0; j < chiTietBaoCao.Count; j++)
                            {
                                if (type == 1)
                                {
                                    if (chiTietBaoCao[j].type == 1)
                                    {
                                        baoCaoThiDuaModel _new = new baoCaoThiDuaModel
                                        {
                                            tenDonVi_CaNhan = chiTietBaoCao[j].name,
                                            tenThiDua = _hstd.tenThiDua,
                                            soQuyetDinh = _hstd.soHieu,
                                            ngayPhatDong = _hstd.ngayPhatDong.ToString(),
                                            noiDungThiDua = lsThiDua[i].noiDungPhatDong,
                                            nguoiKy = lsThiDua[i].nguoiKy,

                                        };
                                        result.Add(_new);
                                    }

                                }

                                if (type == 2)
                                {
                                    if (chiTietBaoCao[j].type == 2)
                                    {
                                        baoCaoThiDuaModel _new = new baoCaoThiDuaModel
                                        {
                                            tenDonVi_CaNhan = chiTietBaoCao[j].name,
                                            donVi = _entities.qltdkt_dm_donvi.Find(_entities.qltdkt_dm_nhanvien.Find(chiTietBaoCao[j].id).donViId).tenDonVi,
                                            chucVu = _entities.qltdkt_dm_chucvu.Find(_entities.qltdkt_dm_nhanvien.Find(chiTietBaoCao[j].id).chucVuId).tenChucVu,
                                            tenThiDua = _hstd.tenThiDua,
                                            soQuyetDinh = _hstd.soHieu,
                                            ngayPhatDong = _hstd.ngayPhatDong.ToString(),
                                            noiDungThiDua = lsThiDua[i].noiDungPhatDong,
                                            nguoiKy = lsThiDua[i].nguoiKy,

                                        };
                                        result.Add(_new);
                                    }

                                }
                            }
                        }
                    }
                }
            }
            return result;
        }
        //Xem danh sách cá nhân/ tập thể báo cáo danh hiệu
        public xemDanhHieuBaoCaoModel xemDanhHieu(int idDanhHieu)
        {
            xemDanhHieuBaoCaoModel a = new xemDanhHieuBaoCaoModel();
            try
            {
                var dh = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                if (dh != null)
                {
                    a.idTenDanhHieu = (int)dh.idTenDanhHieu;
                    a.id = dh.id;
                    a.ngayDanhHieu = ((DateTime)dh.ngayDanhHieu).ToString("dd/MM/yyyy");
                    a.ngayTraoTang = ((DateTime)dh.ngayDanhHieu).ToString("dd/MM/yyyy");
                    a.soHieu = dh.soHieu;
                    a.bophan = dh.bophan;
                    a.kieuDanhHieu = dh.kieuDanhHieu;
                    a.ghiChuTT = dh.ghiChuTT;
                    a.fileGoc = dh.fileGoc;
                    a.tuNam = dh.tuNam;
                    a.denNam = dh.denNam;
                    a.nguoiKy = dh.nguoiKy;
                    a.daTraoTang = (bool)dh.daTraoTang;
                    a.noiDungDanhHieu = dh.noiDung;
                    a.danhSachCaNhanTapThe = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);

                    //a.danhSachCaNhanTapThe = dh.danhSachCaNhanTapThe;
                    a.capKyKhenThuong = dh.capKyKhenThuong;
                    a.namDanhHieu = (int)dh.namDanhHieu;
                    a.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                    a.xemhinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find((int)dh.hinhThucTraoTang).tenHinhThucKhenThuong;
                }
                return a;


            }

            catch
            {
                return a;
            }

        }

        //Hiển thị Danh sách báo cáo danh hiệu
        public List<baoCaoDanhHieuModel> getBaoCaoDanhHieu(DateTime from, DateTime to, int bophan, int loaiDanhHieu, int tenDanhHieu, int hinhThucKhenThuong, string soQuyetDinh)
        {
            List<baoCaoDanhHieuModel> result = new List<baoCaoDanhHieuModel>();
            if (bophan != 0)
            {
                if (loaiDanhHieu != 2)
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;

                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;

                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.idTenDanhHieu == tenDanhHieu && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.idTenDanhHieu == tenDanhHieu && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                }

            }
            else
            {
                if (loaiDanhHieu != 2)
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;

                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;

                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.idTenDanhHieu == tenDanhHieu && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.idTenDanhHieu == tenDanhHieu && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    i++;
                                    result.Add(dh);

                                }


                            }
                            else
                            {
                                //List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                var ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.daXoa == false).ToList();

                                foreach (var item in ListDH)
                                {
                                    int i = 1;
                                    baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                    dh.id = item.id;
                                    dh.stt = i;
                                    dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                    dh.hinhThucTraoTang = item.hinhThucTraoTang != 0 ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Trao tặng trực tiếp / hình thức khác";
                                    dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                    dh.namDanhHieu = (int)item.namDanhHieu;
                                    dh.soQuyetDinh = item.soHieu;
                                    dh.bophan = item.bophan;
                                    dh.fileGoc = item.fileGoc;
                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }
                                    }
                                    i++;
                                    result.Add(dh);

                                }


                            }
                        }
                    }
                }

            }


            return result;
        }
        //Export File Excel Full Danh Sách báo cáo Danh hiệu
        public List<baoCaoDanhHieuModel> getBaoCaoDanhHieuTT(DateTime from, DateTime to, int bophan, int loaiDanhHieu, int tenDanhHieu, int hinhThucKhenThuong, string soQuyetDinh)
        {
            List<baoCaoDanhHieuModel> result = new List<baoCaoDanhHieuModel>();
            if (bophan != 0)
            {
                if (loaiDanhHieu < 2)
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.bophan == bophan && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                }
            }
            else
            {
                if (loaiDanhHieu < 2)
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                }
                else
                {
                    if (tenDanhHieu != 0)
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                    else
                    {
                        if (hinhThucKhenThuong != 0)
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }


                        }
                        else
                        {
                            if (soQuyetDinh != "")
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.soHieu.Contains(soQuyetDinh.Trim()) && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }


                            }
                            else
                            {
                                List<qltdkt_danhhieu> ListDH = new List<qltdkt_danhhieu>();
                                ListDH = _entities.qltdkt_danhhieu.Where(x => x.ngayTraoTang >= from && x.ngayTraoTang <= to && x.kieuDanhHieu == loaiDanhHieu && x.idTenDanhHieu == tenDanhHieu && x.hinhThucTraoTang == hinhThucKhenThuong && x.daXoa == false).ToList();
                                int i = 1;
                                baoCaoDanhHieuModel dh = new baoCaoDanhHieuModel();
                                foreach (var item in ListDH)
                                {

                                    var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                                    foreach (var xxx in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                                    {
                                        dh.stt = i;
                                        dh.id = item.id;
                                        dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(item.idTenDanhHieu).tenDanhHieuThiDua;
                                        dh.hinhThucTraoTang = item.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(item.hinhThucTraoTang).tenHinhThucKhenThuong : "Chưa trao tặng";
                                        dh.ngayDanhHieu = ((DateTime)item.ngayDanhHieu).ToString("dd/MM/yyyy");
                                        dh.namDanhHieu = (int)item.namDanhHieu;
                                        dh.soQuyetDinh = item.soHieu;
                                        dh.bophan = item.bophan;
                                        dh.fileGoc = item.fileGoc;
                                        if (xxx.loaiDanhHieu == loaiDanhHieu)
                                        {

                                            if (xxx.loaiDanhHieu == 0)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;

                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(xxx.id, 0);
                                            }
                                            else if (xxx.loaiDanhHieu == 1)
                                            {
                                                dh.loaiDanhHieu = chiTietDanhHieu.loaiDanhHieu;
                                                dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                                dh.donVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(xxx.id).donViId, 1);
                                            }
                                        }

                                        i++;
                                        result.Add(dh);
                                    }

                                }

                            }
                        }
                    }
                }

            }


            return result;
        }
        //Xem danh sách cá nhân/tập thể báo cáo khen thưởng
        public baoCaoKhenThuongModel xemKhenThuong(int idKhenThuong)
        {
            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
            try
            {
                var a = _entities.qltdkt_khenthuong.Find(idKhenThuong);
                dh.id = a.id;
                dh.sqd = a.soHieu;
                dh.ngayBH = ((DateTime)a.ngayTraoTang).ToString("dd/MM/yyyy");
                dh.loai = (int)a.doiTuongThamGia;
                //dh.capKhenThuong = getCapKhenThuong(((byte)a.capKhenThuong));
                dh.quyetDinh = a.quyetDinh;
                dh.tongtienthuong = a.tongTienThuong;
                dh.tenKhenThuong = a.noiDungKhenThuong;
                dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(a.capKhenThuong)).tenPhatDong;
                if (a.hinhThucKhenThuong != 0)
                {
                    dh.hinhThucKhenThuong = _entities.qltdkt_dm_hinhthuckhenthuong.Find(a.hinhThucKhenThuong).tenHinhThucKhenThuong;

                }
                else
                {
                    dh.hinhThucKhenThuong = "0";
                }
                dh.danhSachCaNhanTapThe = JsonConvert.DeserializeObject<dstapthecanhankt>(a.danhSachCaNhanKhenThuong);
                var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(a.danhSachCaNhanKhenThuong);
                int i = 1;
                if (dh.loai == 1)
                {
                    foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                    {
                        if (xxx.loaiKT == 1)
                        {
                            //baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                            //dh.sqd = item.soHieu;
                            dh.donVi = xxx.donVi;
                            //dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.tienthuong = xxx.tienThuong;
                            //dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            //dh.quyetDinh = item.quyetDinh;
                            //dh.tongtienthuong = item.tongTienThuong;
                            //dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                            i++;
                        }
                    }
                }
                else
                {
                    foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                    {
                        if (xxx.loaiKT == 0)
                        {
                            
                            dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                            dh.donVi = xxx.donVi;
                            dh.tienthuong = xxx.tienThuong;
                            i++;
                        }
                    }
                }
                return dh;
            }
            catch (Exception)
            {
                return dh;
            }
        }
        // Hiển thị danh sách báo cáo khen thưởng
        public List<baoCaoKhenThuongModel> getBaoCaoKhenThuong(DateTime from, DateTime to, int bophan, string loaiKhenThuong, string capKhenThuong, string kieuKhenThuong)
        {
            List<baoCaoKhenThuongModel> result = new List<baoCaoKhenThuongModel>();
            if (bophan != 0)//Chọn bộ phận
            {
                if (capKhenThuong != "0") // chọn cấp
                {
                    int _capKhenThuong = int.Parse(capKhenThuong);
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.capKhenThuong == _capKhenThuong && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.id = item.id;
                            dh.stt = i;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                //int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.stt = i;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);

                        }

                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.capKhenThuong == _capKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.id = item.id;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);

                        }
                    }
                }
                else
                {
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.id = item.id;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            dh.danhSachCaNhanTapThe = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);

                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);
                        }
                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.id = item.id;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);

                        }

                    }
                }
            }
            else
            {
                if (capKhenThuong != "0") // chọn cấp
                {
                    int _capKhenThuong = int.Parse(capKhenThuong);
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.capKhenThuong == _capKhenThuong && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.id = item.id;
                            dh.stt = i;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                       
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.stt = i;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);

                        }

                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.capKhenThuong == _capKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.id = item.id;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);

                        }
                    }
                }
                else
                {
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.id = item.id;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            dh.danhSachCaNhanTapThe = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);

                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);
                        }
                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            int i = 1;
                            baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                            dh.stt = i;
                            dh.id = item.id;
                            dh.sqd = item.soHieu;
                            dh.tenKhenThuong = item.noiDungKhenThuong;
                            dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                            dh.quyetDinh = item.quyetDinh;
                            dh.bophan = item.bophan;
                            dh.tongtienthuong = item.tongTienThuong;
                            dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(Convert.ToByte(item.capKhenThuong)).tenCapKyKhenThuong;
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                //int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.tienthuong = xxx.tienThuong;
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                //int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.tienthuong = xxx.tienThuong;
                                    i++;
                                }
                            }
                            result.Add(dh);

                        }

                    }
                }
            }

            return result;
        }
        public List<baoCaoKhenThuongModel> getBaoCaoKhenThuongTT(DateTime from, DateTime to, int bophan, string loaiKhenThuong, string capKhenThuong, string kieuKhenThuong)
        {
            List<baoCaoKhenThuongModel> result = new List<baoCaoKhenThuongModel>();
            if (bophan != 0)
            {
                if (capKhenThuong != "0") // chọn cấp
                {
                    int _capKhenThuong = int.Parse(capKhenThuong);
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.capKhenThuong == _capKhenThuong && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.capKhenThuong == _capKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.bophan == bophan && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                if (capKhenThuong != "0") // chọn cấp
                {
                    int _capKhenThuong = int.Parse(capKhenThuong);
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.capKhenThuong == _capKhenThuong && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.capKhenThuong == _capKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (kieuKhenThuong != "0") // chọn kiểu
                    {
                        int _kieuKhenThuong = int.Parse(kieuKhenThuong);
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.kieuKhenThuong == _kieuKhenThuong && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }
                    }
                    else
                    {
                        var listDH = _entities.qltdkt_khenthuong.Where(x => x.ngayKhenThuong >= from && x.ngayKhenThuong <= to && x.daTraoTang == true).ToList();
                        foreach (var item in listDH)
                        {
                            var chiTietKhenThuong = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
                            if (loaiKhenThuong == "1") //chọn loại
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 1)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.bophan = item.bophan;
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }

                            }
                            else if (loaiKhenThuong == "0")
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    if (xxx.loaiKT == 0)
                                    {
                                        baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                        dh.stt = i;
                                        dh.bophan = item.bophan;

                                        dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                        dh.sqd = item.soHieu;
                                        dh.donVi = xxx.donVi;
                                        dh.tenKhenThuong = item.noiDungKhenThuong;
                                        dh.tienthuong = xxx.tienThuong;
                                        dh.ngayBH = dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                        dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                        i++;
                                        result.Add(dh);
                                    }
                                }
                            }
                            else
                            {
                                int i = 1;
                                foreach (var xxx in chiTietKhenThuong.dschitietcanhantapthekhenthuong)
                                {
                                    baoCaoKhenThuongModel dh = new baoCaoKhenThuongModel();
                                    dh.stt = i;
                                    dh.bophan = item.bophan;
                                    dh.tenDonVi_CaNhan = xxx.tenCaNhanTapThe;
                                    dh.donVi = xxx.donVi;
                                    dh.sqd = item.soHieu;
                                    dh.tenKhenThuong = item.noiDungKhenThuong;
                                    dh.tienthuong = xxx.tienThuong;
                                    dh.ngayBH = ((DateTime)item.ngayTraoTang).ToString("dd/MM/yyyy");
                                    dh.capKhenThuong = _entities.qltdkt_dm_donviphatdong.Find(Convert.ToByte(item.capKhenThuong)).tenPhatDong;
                                    i++;
                                    result.Add(dh);
                                }
                            }
                        }

                    }
                }
            }

            return result;
        }
        public List<nguoiDungModel> bcDanhSachLaoDongTienTien(int tuNam, int denNam)
        {
            DateTime startDate = DateTime.ParseExact("01/01/" + tuNam, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact("31/12/" + denNam, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            var dsDanhHieu = _entities.qltdkt_danhhieu.Where(x => x.ngayDanhHieu >= startDate && x.ngayDanhHieu <= endDate && x.daTraoTang == true).ToList();
            List<nguoiDungModel> listNV = new List<nguoiDungModel>();
            int i = 1;
            foreach (var item in dsDanhHieu)
            {
                var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                if (item.idTenDanhHieu == 16)
                {
                    foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                    {
                        nguoiDungModel nd = new nguoiDungModel();
                        nd.stt = i;
                        nd.idNguoiDung = item2.id;
                        nd.maNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).maNhanVien;
                        nd.hoTen = item2.tenCaNhanTapThe;
                        nd.tenChucDanh = item2.chucDanh;
                        nd.tenDonVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(item2.id).donViId, 1);
                        nd.namDanhHieu = ((DateTime)item.ngayDanhHieu).Year.ToString();
                        listNV.Add(nd);
                        i++;
                    }
                }
            }
            return listNV;
        }
        public List<nguoiDungModel> bcDanhSachChienSyThiDuaCoSo(int tuNam, int denNam)
        {
            var dsDanhHieu = _entities.qltdkt_danhhieu.Where(x => x.namDanhHieu >= tuNam && x.namDanhHieu <= denNam).ToList();
            List<nguoiDungModel> listNV = new List<nguoiDungModel>();
            int i = 1;
            foreach (var item in dsDanhHieu)
            {
                var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                if (item.idTenDanhHieu == 15)
                {
                    foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                    {
                        nguoiDungModel nd = new nguoiDungModel();
                        nd.stt = i;
                        nd.idNguoiDung = item2.id;
                        nd.maNhanVien = _entities.qltdkt_dm_nhanvien.Find(item2.id).maNhanVien;
                        nd.hoTen = item2.tenCaNhanTapThe;
                        nd.tenChucDanh = item2.chucDanh;
                        nd.tenDonVi = getFullNameDonVi(_entities.qltdkt_dm_nhanvien.Find(item2.id).donViId, 1);
                        nd.namDanhHieu = item.namDanhHieu.ToString();
                        nd.trangThaiDanhHieu = item.daTraoTang == true ? "Đã trao tặng" : "Chưa trao tặng";
                        listNV.Add(nd);
                        i++;
                    }
                }
            }
            return listNV;
        }
        public List<donViModel> bcDanhSachTapTheLaoDongTienTien(int tuNam, int denNam)
        {
            var dsDanhHieu = _entities.qltdkt_danhhieu.Where(x => x.namDanhHieu >= tuNam && x.namDanhHieu <= denNam && x.daTraoTang == true).ToList();
            List<donViModel> listTT = new List<donViModel>();
            int i = 1;
            foreach (var item in dsDanhHieu)
            {
                var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                if (item.idTenDanhHieu == 22)
                {
                    foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                    {
                        donViModel tt = new donViModel();
                        tt.stt = i;
                        tt.idDonVi = item2.id;
                        tt.tenDonVi = item2.tenCaNhanTapThe;
                        tt.tenDonViCha = item2.donVi;
                        tt.namDanhHieu = ((DateTime)item.ngayDanhHieu).Year.ToString();
                        listTT.Add(tt);
                        i++;
                    }
                }
            }
            return listTT;
        }
        public List<donViModel> bcDanhSachTapTheLaoDongXuatSac(int tuNam, int denNam)
        {
            var dsDanhHieu = _entities.qltdkt_danhhieu.Where(x => x.namDanhHieu >= tuNam && x.namDanhHieu <= denNam && x.daTraoTang == true).ToList();
            List<donViModel> listTT = new List<donViModel>();
            int i = 1;
            foreach (var item in dsDanhHieu)
            {
                var chiTietDanhHieu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
                if (item.idTenDanhHieu == 20)
                {
                    foreach (var item2 in chiTietDanhHieu.dsChiTietCaNhanTapThe)
                    {
                        donViModel tt = new donViModel();
                        tt.stt = i;
                        tt.idDonVi = item2.id;
                        tt.tenDonVi = item2.tenCaNhanTapThe;
                        tt.tenDonViCha = item2.donVi;
                        tt.namDanhHieu = ((DateTime)item.ngayDanhHieu).Year.ToString();
                        listTT.Add(tt);
                        i++;
                    }
                }
            }
            return listTT;
        }
        public List<nguoiDungModel> bcDanhSachNguoiLaoDong(int donViCap1, int donViCap2, int donViCap3)
        {
            if (donViCap3 != 0)
            {
                List<nguoiDungModel> listNV = new List<nguoiDungModel>();
                var listAll = _entities.qltdkt_dm_nhanvien.Where(x => x.donViId == donViCap3 && x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01").ToList();
                int i = 1;
                foreach (var item in listAll)
                {
                    nguoiDungModel nd = new nguoiDungModel();
                    nd.stt = i;
                    nd.idNguoiDung = item.id;
                    nd.maNhanVien = item.maNhanVien;
                    nd.hoTen = item.hoTen;
                    nd.tenChucDanh = _entities.qltdkt_dm_chucvu.Find(item.chucVuId).tenChucVu;
                    nd.tenDonVi = getFullNameDonVi(item.donViId, 1);
                    listNV.Add(nd);
                    i++;
                }
                return listNV;
            }
            else if (donViCap2 != 0)
            {
                List<nguoiDungModel> listNV = new List<nguoiDungModel>();
                var listAll = _entities.qltdkt_dm_nhanvien.Where(x => x.donViId == donViCap2 && x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01").ToList();
                int i = 1;
                foreach (var item in listAll)
                {
                    nguoiDungModel nd = new nguoiDungModel();
                    nd.stt = i;
                    nd.idNguoiDung = item.id;
                    nd.maNhanVien = item.maNhanVien;
                    nd.hoTen = item.hoTen;
                    nd.tenChucDanh = _entities.qltdkt_dm_chucvu.Find(item.chucVuId).tenChucVu;
                    nd.tenDonVi = getFullNameDonVi(item.donViId, 1);
                    listNV.Add(nd);
                    i++;
                }
                var listcon = _entities.qltdkt_dm_donvi.Where(x => x.idCha == donViCap2).ToList();
                foreach (var item in listcon)
                {
                    var listnd = _entities.qltdkt_dm_nhanvien.Where(x => x.donViId == item.id && x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01").ToList();
                    foreach (var item2 in listnd)
                    {
                        nguoiDungModel nd = new nguoiDungModel();
                        nd.stt = i;
                        nd.idNguoiDung = item2.id;
                        nd.maNhanVien = item2.maNhanVien;
                        nd.hoTen = item2.hoTen;
                        nd.tenChucDanh = _entities.qltdkt_dm_chucvu.Find(item2.chucVuId).tenChucVu;
                        nd.tenDonVi = getFullNameDonVi(item2.donViId, 1);
                        listNV.Add(nd);
                        i++;
                    }
                }
                return listNV;
            }
            else
            {
                List<nguoiDungModel> listNV = new List<nguoiDungModel>();
                var listAll = _entities.qltdkt_dm_nhanvien.Where(x => x.daXoa == false && x.trangThai == 0 && x.maNhanVien != "cntt_01").ToList();
                int i = 1;
                foreach (var item in listAll)
                {
                    nguoiDungModel nd = new nguoiDungModel();
                    nd.stt = i;
                    nd.idNguoiDung = item.id;
                    nd.maNhanVien = item.maNhanVien;
                    nd.hoTen = item.hoTen;
                    nd.tenChucDanh = _entities.qltdkt_dm_chucvu.Find(item.chucVuId).tenChucVu;
                    nd.tenDonVi = getFullNameDonVi(item.donViId, 1);

                    listNV.Add(nd);
                    i++;
                }
                return listNV;
            }
        }
        public List<dsTapTheCaNhanModel> bcDanhSachTapTheToPhongTrucThuocVNPTQB()
        {
            var listDV = _entities.qltdkt_dm_donvi.Where(x => x.idCha == 1).ToList();
            List<dsTapTheCaNhanModel> result = new List<dsTapTheCaNhanModel>();
            int i = 1;
            foreach (var item in listDV)
            {
                dsTapTheCaNhanModel a = new dsTapTheCaNhanModel();
                a.stt = i;
                a.idTapThe = item.id;
                a.tenTapThe = item.tenDonVi;
                result.Add(a);
                i++;
            }

            int j = 1;
            foreach (var item in result)
            {
                var dvcap2 = (from a in _entities.qltdkt_dm_donvi
                              where a.idCha == item.idTapThe
                              select new dsTapTheCaNhanModel
                              {
                                  idTapThe = a.id,
                                  tenTapThe = a.tenDonVi
                              }).ToList();

                item.dsDonViCap2 = dvcap2;
            }
            return result;
        }
        public List<dsTapTheCaNhanModel> bcDanhSachTapTheTrucThuocVNPTQB()
        {
            var listDV = _entities.qltdkt_dm_donvi.Where(x => x.idCha == 1).ToList();
            List<dsTapTheCaNhanModel> result = new List<dsTapTheCaNhanModel>();
            int i = 1;
            foreach (var item in listDV)
            {
                dsTapTheCaNhanModel a = new dsTapTheCaNhanModel();
                a.stt = i;
                a.tenTapThe = item.tenDonVi;
                result.Add(a);
                i++;
            }
            return result;
        }

        public List<donViModel> listDonViByIDCha(int idDonVi)
        {
            var result = (from a in _entities.qltdkt_dm_donvi
                          where a.idCha == idDonVi
                          select new donViModel
                          {
                              idDonVi = a.id,
                              tenDonVi = a.tenDonVi
                          }).ToList();
            return result;
        }
        public string getFullNameDonVi(int idDonVi, int l)
        {
            if (l == 0)
            {
                if (idDonVi == 1)
                {
                    return "Viễn Thông Quảng Bình";
                }
                else
                {
                    string result = "";
                    int idParent = _entities.qltdkt_dm_donvi.Find(idDonVi).idCha;
                    int idGrandParent = _entities.qltdkt_dm_donvi.Find(idParent).idCha;
                    result = _entities.qltdkt_dm_donvi.Find(idParent).tenDonVi + " / " + _entities.qltdkt_dm_donvi.Find(idGrandParent).tenDonVi;
                    return result;
                }
            }
            else
            {
                if (idDonVi == 1)
                {
                    return "Viễn Thông Quảng Bình";
                }
                else
                {
                    string result = "";
                    int idParent = _entities.qltdkt_dm_donvi.Find(idDonVi).idCha;
                    result = _entities.qltdkt_dm_donvi.Find(idDonVi).tenDonVi + " / " + _entities.qltdkt_dm_donvi.Find(idParent).tenDonVi;
                    return result;
                }
            }
        }

        public List<BaoCaoTrangChu> getThongKeTrangChu(int kieuDanhHieu, int namDanhHieu)
        {
            List<BaoCaoTrangChu> result = new List<BaoCaoTrangChu>();
            string sql = "";
            if (kieuDanhHieu == 0) //Tập thể
            {
                sql = "SELECT YEAR(ngayDanhHieu) as 'ngayDanhHieu',sum(kieuDanhHieu) as 'tongDanhHieuTapThe' " +
                         "FROM qltdkt_danhhieu WHERE YEAR(ngayDanhHieu) = "+namDanhHieu+" GROUP BY YEAR(ngayDanhHieu)";

            }
            if (kieuDanhHieu == 1) //Cá nhân
            {
                sql = "SELECT YEAR(ngayDanhHieu) as 'ngayDanhHieu',sum(kieuDanhHieu) as 'tongDanhHieuCaNhan'" +
                    "FROM qltdkt_danhhieu WHERE YEAR(ngayDanhHieu) = "+namDanhHieu+" GROUP BY YEAR(ngayDanhHieu)";
            }

            DataTable dt = new DataTable();
            try
            {
                dt = _sqlAccess.getDataFromSql(sql.ToString(), "").Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (kieuDanhHieu == 0)
                        {
                            BaoCaoTrangChu bcObj = new BaoCaoTrangChu
                            {
                                namDanhHieu = dt.Rows[i]["ngayDanhHieu"].ToString() != "" ? int.Parse(dt.Rows[i]["ngayDanhHieu"].ToString()) : 0,
                                danhHieuTapThe = dt.Rows[i]["tongDanhHieuTapThe"].ToString() != "" ? int.Parse(dt.Rows[i]["tongDanhHieuTapThe"].ToString()) : 0,
                                kieuDanhHieu = kieuDanhHieu
                                //loiNhuan = dt.Rows[i]["tongLoiNhuan"] != null ? double.Parse(dt.Rows[i]["tongLoiNhuan"].ToString()) : 0
                            };
                            result.Add(bcObj);

                        }
                        else
                        {
                            BaoCaoTrangChu bcObj = new BaoCaoTrangChu
                            {
                                namDanhHieu = dt.Rows[i]["ngayDanhHieu"].ToString() != "" ? int.Parse(dt.Rows[i]["ngayDanhHieu"].ToString()) : 0,
                                danhHieuCaNhan = dt.Rows[i]["tongDanhHieuCaNhan"].ToString() != "" ? int.Parse(dt.Rows[i]["tongDanhHieuCaNhan"].ToString()) : 0,
                                kieuDanhHieu = kieuDanhHieu
                                //loiNhuan = dt.Rows[i]["tongLoiNhuan"] != null ? double.Parse(dt.Rows[i]["tongLoiNhuan"].ToString()) : 0
                            };
                            result.Add(bcObj);

                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return result;
        }

    }

}