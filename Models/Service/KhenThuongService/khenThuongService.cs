using Newtonsoft.Json;
using QLTDKT.Models.Service.danhHieuService;
using QLTDKT.Models.Service.khenThuongService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using QLTDKT.Models.Service.soKyYeuService;
using System.Web;
using QLTDKT.Models.Service.hinhThucKhenThuongService;
using QLTDKT.Models.Service.KhenThuongService;

namespace QLTDKT.Models.Service.khenThuongService
{
    public class khenThuongService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();
        public List<khenThuongModel> getDanhSachKhenThuong(int capKT, int bophan, string soHieu, string ngayPhatDong_TuNgay, string ngayPhatDong_DenNgay, int trangThai)
        {
            List<khenThuongModel> _result = new List<khenThuongModel>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.id, a.quyetDinh, a.capKhenThuong, a.bophan, a.soHieu, a.trangThai, a.ngayKhenThuong,  a.daTraoTang FROM qltdkt_khenthuong a LEFT JOIN qltdkt_dm_capkykhenthuong b ON b.id = a.capKhenThuong ");
            sql.Append("WHERE 1=1 ");
            if (capKT != 0)
                sql.Append(string.Format("AND a.capKhenThuong = {0} ", capKT));
            if (bophan != 0)
                sql.Append(string.Format("AND a.bophan = {0} ", bophan));
            if (soHieu.Trim().Length > 0 || soHieu != "")
                sql.Append(string.Format("AND a.soHieu LIKE '%{0}%' ", soHieu));
            if (ngayPhatDong_TuNgay != "" || ngayPhatDong_DenNgay != "")
            {
                if (ngayPhatDong_TuNgay == "")
                {
                    if (ngayPhatDong_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngayKhenThuong, 'dd/MM/yyyy') <= '{0}' ", ngayPhatDong_DenNgay));
                }
                else
                {
                    if (ngayPhatDong_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngayKhenThuong, 'dd/MM/yyyy') <= '{0}' AND FORMAT(a.ngayKhenThuong, 'dd/MM/yyyy') >= '{1}' ", ngayPhatDong_DenNgay, ngayPhatDong_TuNgay));
                    else
                        sql.Append(string.Format("AND FORMAT(a.ngayKhenThuong, 'dd/MM/yyyy') >= '{0}' ", ngayPhatDong_TuNgay));
                }
            }
            if (trangThai < 2)
            {
                sql.Append(string.Format("AND a.trangThai = {0} ", trangThai));
            }
            sql.Append("AND a.daXoa = 0 ");
            sql.Append("ORDER BY a.ngayTao DESC");
            Console.WriteLine("==============SQL: " + sql.ToString());
            DataTable dt = new DataTable();
            try
            {
                dt = _sqlAccess.getDataFromSql(sql.ToString(), "").Tables[0];
                if (dt.Rows.Count > 0)
                {
                    int stt = 0;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        stt++;
                        khenThuongModel _newObj = new khenThuongModel
                        {
                            id = int.Parse(dt.Rows[i]["id"].ToString()),
                            stt = stt,
                            bophan = int.Parse(dt.Rows[i]["bophan"].ToString()),
                            capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(int.Parse(dt.Rows[i]["capKhenThuong"].ToString())).tenCapKyKhenThuong,
                            ngayKhenThuong = (DateTime.Parse(dt.Rows[i]["ngayKhenThuong"].ToString())).ToString("dd/MM/yyyy"),
                            soHieu = dt.Rows[i]["soHieu"].ToString(),
                            daTraoTang = bool.Parse(dt.Rows[i]["daTraoTang"].ToString()),
                            quyetDinh = dt.Rows[i]["quyetDinh"].ToString(),
                            trangThai = int.Parse(dt.Rows[i]["trangThai"].ToString())
                        };
                        _result.Add(_newObj);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _result;
        }
        public dstapthecanhankt loadDsCaNhanTapThe(int idKT)
        {
            dstapthecanhankt ds = new dstapthecanhankt();
            var item = _entities.qltdkt_khenthuong.Find(idKT);
            if (item.danhSachCaNhanKhenThuong != null)
            {
                ds = JsonConvert.DeserializeObject<dstapthecanhankt>(item.danhSachCaNhanKhenThuong);
            }
            else
            {
                ds.loaiKhenThuong = (int)item.doiTuongThamGia;
            }
            return ds;
        }
        public List<nguoiDungModel> searchCaNhan(string idDonVi, string searchText)
        {
            List<nguoiDungModel> result = new List<nguoiDungModel>();

            if (idDonVi != "")
            {
                if (searchText != "")
                {
                    var idDV = int.Parse(idDonVi);
                    var nd = from a in _entities.qltdkt_dm_nhanvien
                             join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                             join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                             where a.donViId == idDV && a.daXoa == false && a.hoTen.Contains(searchText) && a.trangThai == 0
                             select new nguoiDungModel
                             {
                                 idNguoiDung = a.id,
                                 maNhanVien = a.maNhanVien,
                                 hoTen = a.hoTen,
                                 //ngaySinh = a.ngaySinh,
                                 tenChucDanh = c.tenChucVu,
                                 TenDonVi = b.tenDonVi
                             };
                    result.AddRange(nd.ToList());
                    var listdvcon = _entities.qltdkt_dm_donvi.Where(x => x.idCha == idDV).ToList();
                    foreach (var item in listdvcon)
                    {
                        var ndc = from a in _entities.qltdkt_dm_nhanvien
                                  join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                                  join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                                  where a.donViId == item.id && a.daXoa == false && a.hoTen.Contains(searchText) && a.trangThai == 0
                                  select new nguoiDungModel
                                  {
                                      idNguoiDung = a.id,
                                      maNhanVien = a.maNhanVien,
                                      hoTen = a.hoTen,
                                      //ngaySinh = a.ngaySinh,
                                      tenChucDanh = c.tenChucVu,
                                      TenDonVi = b.tenDonVi
                                  };
                        result.AddRange(ndc);
                    }
                }
                else
                {
                    var idDV = int.Parse(idDonVi);
                    var nd = from a in _entities.qltdkt_dm_nhanvien
                             join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                             join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                             where a.donViId == idDV && a.daXoa == false && a.trangThai == 0 && a.maNhanVien != "cntt_01"
                             select new nguoiDungModel
                             {
                                 idNguoiDung = a.id,
                                 maNhanVien = a.maNhanVien,
                                 hoTen = a.hoTen,
                                 //ngaySinh = a.ngaySinh,
                                 tenChucDanh = c.tenChucVu,
                                 TenDonVi = b.tenDonVi
                             };
                    result.AddRange(nd.ToList());
                    var listdvcon = _entities.qltdkt_dm_donvi.Where(x => x.idCha == idDV).ToList();
                    foreach (var item in listdvcon)
                    {
                        var ndc = from a in _entities.qltdkt_dm_nhanvien
                                  join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                                  join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                                  where a.donViId == item.id && a.daXoa == false && a.trangThai == 0 && a.maNhanVien != "cntt_01"
                                  select new nguoiDungModel
                                  {
                                      idNguoiDung = a.id,
                                      maNhanVien = a.maNhanVien,
                                      hoTen = a.hoTen,
                                      //ngaySinh = a.ngaySinh,
                                      tenChucDanh = c.tenChucVu,
                                      TenDonVi = b.tenDonVi
                                  };
                        result.AddRange(ndc);
                    }
                }
            }
            else
            {
                if (searchText != "")
                {
                    var nd = from a in _entities.qltdkt_dm_nhanvien
                             join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                             join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                             where a.hoTen.Contains(searchText) && a.daXoa == false && a.trangThai == 0 && a.maNhanVien != "cntt_01"
                             select new nguoiDungModel
                             {
                                 idNguoiDung = a.id,
                                 maNhanVien = a.maNhanVien,
                                 hoTen = a.hoTen,
                                 //ngaySinh = a.ngaySinh,
                                 tenChucDanh = c.tenChucVu,
                                 TenDonVi = b.tenDonVi
                             };
                    result.AddRange(nd.ToList());
                }
            }
            return result;
        }
        public List<donViModel> searchTapThe(string idDonVi, string searchText)
        {
            List<donViModel> result = new List<donViModel>();

            if (idDonVi != "")
            {
                if (searchText != "")
                {
                    var idDV = int.Parse(idDonVi);
                    var dv = from a in _entities.qltdkt_dm_donvi
                             join b in _entities.qltdkt_dm_donvi on a.idCha equals b.id
                             where a.idCha == idDV && a.tenDonVi.Contains(searchText)
                             select new donViModel
                             {
                                 idDonVi = a.id,
                                 tenDonVi = a.tenDonVi,
                                 tenDonViCha = b.tenDonVi
                             };
                    result.AddRange(dv.ToList());
                }
                else
                {
                    var idDV = int.Parse(idDonVi);
                    var dv = from a in _entities.qltdkt_dm_donvi
                             join b in _entities.qltdkt_dm_donvi on a.idCha equals b.id
                             where a.idCha == idDV
                             select new donViModel
                             {
                                 idDonVi = a.id,
                                 tenDonVi = a.tenDonVi,
                                 tenDonViCha = b.tenDonVi
                             };
                    result.AddRange(dv.ToList());
                }
            }
            else
            {
                if (searchText != "")
                {
                    var dv = from a in _entities.qltdkt_dm_donvi
                             join b in _entities.qltdkt_dm_donvi on a.idCha equals b.id
                             where a.tenDonVi.Contains(searchText)
                             select new donViModel
                             {
                                 idDonVi = a.id,
                                 tenDonVi = a.tenDonVi,
                                 tenDonViCha = b.tenDonVi
                             };
                    result.AddRange(dv.ToList());
                }
            }
            return result;
        }
        public List<nguoiDungModel> loadNhanVienByDonVi(int idDonVi)
        {
            List<nguoiDungModel> result = new List<nguoiDungModel>();
            var nd = from a in _entities.qltdkt_dm_nhanvien
                     join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                     join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                     where a.donViId == idDonVi && a.daXoa == false && a.maNhanVien != "cntt_01" && a.trangThai == 0
                     select new nguoiDungModel
                     {
                         idNguoiDung = a.id,
                         maNhanVien = a.maNhanVien,
                         hoTen = a.hoTen,
                         //ngaySinh = a.ngaySinh,
                         tenChucDanh = c.tenChucVu,
                         TenDonVi = b.tenDonVi
                     };
            result.AddRange(nd);
            var listdvcon = _entities.qltdkt_dm_donvi.Where(x => x.idCha == idDonVi).ToList();
            foreach (var item in listdvcon)
            {
                var ndc = from a in _entities.qltdkt_dm_nhanvien
                          join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                          join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                          where a.donViId == item.id && a.daXoa == false && a.maNhanVien != "cntt_01" && a.trangThai == 0
                          select new nguoiDungModel
                          {
                              idNguoiDung = a.id,
                              maNhanVien = a.maNhanVien,
                              hoTen = a.hoTen,
                              //ngaySinh = a.ngaySinh,
                              tenChucDanh = c.tenChucVu,
                              TenDonVi = b.tenDonVi
                          };
                result.AddRange(ndc);
            }
            return result;
            //return nd.ToList();

        }
        private string getCapKhenThuong(byte? capKhenThuong)
        {
            switch (capKhenThuong)
            {
                case 1: return "Tập đoàn Bưu chính Viễn thông Việt Nam";
                case 2: return "UBND Tỉnh";
                case 3: return "Viễn Thông Quảng Bình";

            }
            return "";
        }
        public int LuuSoKyYeu(int idkt)
        {
            var dh = _entities.qltdkt_khenthuong.Find(idkt);
            var chitietdh = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
            if (chitietdh.loaiKhenThuong == 0) //tập thể
            {
                int t = 0;
                foreach (var item in chitietdh.dschitietcanhantapthekhenthuong)
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {
                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                            if (ck == null) //
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                sky.capKyKhenThuong = dh.capKyKhenThuong;
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                sky.idKhenThuong = idkt;
                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;
                                check.ngayTao = DateTime.Now;
                                _entities.SaveChanges();
                            }
                            t++;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                    else //chưa có thì add
                    {
                        try
                        {
                            qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                            hsky.idDonVi = item.id;
                            hsky.idNhanVien = null;
                            hsky.ngayTao = DateTime.Now;
                            hsky.daXoa = 0;


                            soKyYeuModel sky = new soKyYeuModel();
                            sky.kieuKyYeu = 3;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenKhenThuong = chitietdh.tenKhenThuong;
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                            sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;

                            sky.capKyKhenThuong = dh.capKyKhenThuong;
                            sky.idKhenThuong = idkt;
                            sky.tienThuong = decimal.Parse(item.tienThuong);
                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                            t++;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
                if (t == chitietdh.dschitietcanhantapthekhenthuong.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else if (chitietdh.loaiKhenThuong == 1)
            {
                int t = 0;
                foreach (var item in chitietdh.dschitietcanhantapthekhenthuong)
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {
                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                            if (ck == null)
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;

                                sky.capKyKhenThuong = dh.capKyKhenThuong;
                                sky.idKhenThuong = idkt;
                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;
                                check.ngayTao = DateTime.Now;

                                _entities.SaveChanges();
                            }

                            t++;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                    else //chưa có thì add
                    {
                        try
                        {
                            qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                            hsky.idDonVi = _entities.qltdkt_dm_nhanvien.Find(item.id).donViId;
                            hsky.idNhanVien = item.id;
                            hsky.ngayTao = DateTime.Now;
                            hsky.daXoa = 0;

                            soKyYeuModel sky = new soKyYeuModel();
                            sky.kieuKyYeu = 3;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenKhenThuong = chitietdh.tenKhenThuong;
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                            sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;

                            sky.capKyKhenThuong = dh.capKyKhenThuong;

                            sky.idKhenThuong = idkt;
                            sky.tienThuong = decimal.Parse(item.tienThuong);
                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                            t++;
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
                if (t == chitietdh.dschitietcanhantapthekhenthuong.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }

            }
            else
            {
                int t = 0;
                foreach (var item in chitietdh.dschitietcanhantapthekhenthuong)
                {
                    if (item.loaiKT == 1)
                    {
                        var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                        if (check != null) //nếu có thì update
                        {
                            try
                            {
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                                var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                                if (ck == null)
                                {
                                    soKyYeuModel sky = new soKyYeuModel();
                                    sky.kieuKyYeu = 3;
                                    sky.ngayKyYeu = DateTime.Now;
                                    sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                    sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                    sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;

                                    sky.capKyKhenThuong = dh.capKyKhenThuong;

                                    sky.idKhenThuong = idkt;
                                    sky.tienThuong = decimal.Parse(item.tienThuong);
                                    hsky.Add(sky);
                                    check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                    check.daXoa = 0;
                                    check.ngayTao = DateTime.Now;
                                    _entities.SaveChanges();
                                }

                                t++;
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }
                        else //chưa có thì add
                        {
                            try
                            {
                                qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                                hsky.idDonVi = _entities.qltdkt_dm_nhanvien.Find(item.id).donViId;
                                hsky.idNhanVien = item.id;
                                hsky.ngayTao = DateTime.Now;

                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;

                                sky.capKyKhenThuong = dh.capKyKhenThuong;

                                sky.idKhenThuong = idkt;
                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                                _sky.Add(sky);
                                hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);
                                _entities.qltdkt_hosokyyeu.Add(hsky);
                                _entities.SaveChanges();
                                t++;
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }
                    }
                    else
                    {
                        var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                        if (check != null) //nếu có thì update
                        {
                            try
                            {
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                                var ck = hsky.Where(x => x.idKhenThuong == idkt).FirstOrDefault();
                                if (ck == null) //
                                {
                                    soKyYeuModel sky = new soKyYeuModel();
                                    sky.kieuKyYeu = 3;
                                    sky.ngayKyYeu = DateTime.Now;
                                    sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                    sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                    sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                    sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;

                                    sky.capKyKhenThuong = dh.capKyKhenThuong;

                                    sky.tienThuong = decimal.Parse(item.tienThuong);
                                    sky.idKhenThuong = idkt;
                                    hsky.Add(sky);
                                    check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                    check.daXoa = 0;
                                    check.ngayTao = DateTime.Now;
                                    _entities.SaveChanges();
                                }
                                t++;
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }
                        else //chưa có thì add
                        {
                            try
                            {
                                qltdkt_hosokyyeu hsky = new qltdkt_hosokyyeu();
                                hsky.idDonVi = item.id;
                                hsky.idNhanVien = null;
                                hsky.ngayTao = DateTime.Now;

                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 3;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenKhenThuong = chitietdh.tenKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)dh.capKhenThuong).tenCapKyKhenThuong;
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucKhenThuong).tenHinhThucKhenThuong;
                                sky.capKyKhenThuong = dh.capKyKhenThuong;
                                sky.idKhenThuong = idkt;
                                sky.tienThuong = decimal.Parse(item.tienThuong);
                                List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                                _sky.Add(sky);

                                hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                                _entities.qltdkt_hosokyyeu.Add(hsky);
                                _entities.SaveChanges();
                                t++;
                            }
                            catch (Exception ex)
                            {
                                return 0;
                            }
                        }

                    }
                }
                if (t == chitietdh.dschitietcanhantapthekhenthuong.Count())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        public List<dschitietcanhantapthekhenthuong> getDSChiTietCNTTByID(string idcntt, int lkt, string tien, int tongtien)
        {
            if (idcntt == "")
            {
                return null;
            }
            else
            {
                List<dschitietcanhantapthekhenthuong> dsct = new List<dschitietcanhantapthekhenthuong>();
                string[] ls = idcntt.Split(',');
                string[] lst = tien.Split(',');
                tongtien = 0;
                if (lkt == 1)
                {
                    for (int i = 0; i < ls.Length; i++)
                    {
                        int id = int.Parse(ls[i]);
                        string t;
                        //tongtien += int.Parse(lst[i]);
                        if (lst[i] == null)
                        {
                            t = Convert.ToString('0');
                        }
                        else
                        {
                            t = lst[i];
                        }
                        var ac = from a in _entities.qltdkt_dm_nhanvien
                                 join b in _entities.qltdkt_dm_chucvu on a.chucVuId equals b.id
                                 join c in _entities.qltdkt_dm_donvi on a.donViId equals c.id
                                 where a.id == id
                                 select new dschitietcanhantapthekhenthuong
                                 {
                                     loaiKT = 1,
                                     id = a.id,
                                     ischeck = true,
                                     tenCaNhanTapThe = a.hoTen,
                                     chucDanh = b.tenChucVu,
                                     donVi = c.tenDonVi,
                                     tienThuong = t
                                 };
                        dsct.Add(ac.FirstOrDefault());
                    }
                    return dsct;
                }
                else if (lkt == 0)
                {
                    for (int i = 0; i < ls.Length; i++)
                    {
                        dschitietcanhantapthekhenthuong ac = new dschitietcanhantapthekhenthuong();
                        string t = lst[i];
                        int id = int.Parse(ls[i]);
                        //tongtien += id;

                        var ds = _entities.qltdkt_dm_donvi.Find(id);
                        ac.ischeck = true;

                        ac.loaiKT = 0;
                        ac.id = ds.id;
                        ac.tenCaNhanTapThe = ds.tenDonVi;
                        ac.tienThuong = t;
                        ac.donVi = _entities.qltdkt_dm_donvi.Where(x => x.id == ds.idCha).FirstOrDefault().tenDonVi;
                        dsct.Add(ac);
                    }
                    return dsct;
                }
                else
                {
                    for (int i = 0; i < ls.Length; i++)
                    {
                        dschitietcanhantapthekhenthuong ac = new dschitietcanhantapthekhenthuong();
                        string t = lst[i];
                        int id = int.Parse(ls[i]);
                        //tongtien += id;

                        var ds = _entities.qltdkt_dm_donvi.Find(id);
                        if (ds != null)
                        {
                            ac.loaiKT = 0;
                            ac.ischeck = true;

                            ac.id = ds.id;
                            ac.tenCaNhanTapThe = ds.tenDonVi;
                            ac.tienThuong = t;
                            ac.donVi = _entities.qltdkt_dm_donvi.Where(x => x.id == ds.idCha).FirstOrDefault().tenDonVi;
                            dsct.Add(ac);
                        }
                        else
                        {
                            var ac2 = from a in _entities.qltdkt_dm_nhanvien
                                      join b in _entities.qltdkt_dm_chucvu on a.chucVuId equals b.id
                                      join c in _entities.qltdkt_dm_donvi on a.donViId equals c.id
                                      where a.id == id && a.trangThai == 0 && a.daXoa == false && a.maNhanVien != "cntt_01"
                                      select new dschitietcanhantapthekhenthuong
                                      {
                                          loaiKT = 1,
                                          ischeck = true,
                                          id = a.id,
                                          tenCaNhanTapThe = a.hoTen,
                                          chucDanh = b.tenChucVu,
                                          donVi = c.tenDonVi,
                                          tienThuong = t
                                      };
                            dsct.Add(ac2.FirstOrDefault());
                        }
                    }
                    return dsct;
                }
            }
        }
        public int ThemCaNhanTapThe(string idcntt, int lkt, string tdh, int idkt, string tien, int tongtien)
        {
            dstapthecanhankt dsCaNhanTapThe = new dstapthecanhankt();

            dsCaNhanTapThe.loaiKhenThuong = lkt;
            dsCaNhanTapThe.tenKhenThuong = tdh;
            dsCaNhanTapThe.tongtien = tongtien;
            //dsCaNhanTapThe.tenKhenThuong = _entities.qltdkt_dm_hinhthuckhenthuong.Find(tdh).tenHinhThucKhenThuong;
            dsCaNhanTapThe.dschitietcanhantapthekhenthuong = getDSChiTietCNTTByID(idcntt, lkt, tien, tongtien);
            var dh = _entities.qltdkt_khenthuong.Find(idkt);
            if (dh.danhSachCaNhanKhenThuong == null || dh.danhSachCaNhanKhenThuong == "")
            {
                dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCaNhanTapThe);
                _entities.SaveChanges();
                return 1;
            }
            else
            {
                dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                try
                {

                    if (dsCaNhanTapThe.dschitietcanhantapthekhenthuong == null)
                    {
                        dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCaNhanTapThe);
                        _entities.SaveChanges();
                        return 1;
                    }
                    else if (dscu.dschitietcanhantapthekhenthuong == null)
                    {
                        dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCaNhanTapThe);
                        _entities.SaveChanges();
                        return 1;
                    }
                    else
                    {

                        if (dscu.dschitietcanhantapthekhenthuong != null && dscu.dschitietcanhantapthekhenthuong.Count >= dsCaNhanTapThe.dschitietcanhantapthekhenthuong.Count)
                        {
                            dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCaNhanTapThe);
                            _entities.SaveChanges();
                            return 1;
                        }
                        else
                        {
                            dscu.dschitietcanhantapthekhenthuong.AddRange(dsCaNhanTapThe.dschitietcanhantapthekhenthuong);
                            dscu.dschitietcanhantapthekhenthuong = dscu.dschitietcanhantapthekhenthuong.GroupBy(x => x.id).Select(x => x.First()).ToList();

                            dsCaNhanTapThe.dschitietcanhantapthekhenthuong = dscu.dschitietcanhantapthekhenthuong;
                            dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dsCaNhanTapThe);
                            _entities.SaveChanges();
                            return 1;
                        }
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
            }

        }
        public xemKhenThuongModal xemKhenThuong(int idKhenThuong)
        {
            xemKhenThuongModal dh = new xemKhenThuongModal();
            try
            {
                var a = _entities.qltdkt_khenthuong.Find(idKhenThuong);
                dh.id = a.id;
                dh.soHieu = a.soHieu;
                dh.ngayKhenThuong = ((DateTime)a.ngayKhenThuong).ToString("dd/MM/yyyy");
                dh.capKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find((int)a.capKhenThuong).tenCapKyKhenThuong;
                dh.quyetDinh = a.quyetDinh;
                dh.tongTienThuong = a.tongTienThuong;
                dh.noidungKhenThuong = a.noiDungKhenThuong;
                dh.capKyKhenThuong = a.capKyKhenThuong;
                dh.daTraoTang = a.daTraoTang == true ? "Đã trao tặng" : "Chưa trao tặng";
                dh.danhSachCaNhanTapThe = JsonConvert.DeserializeObject<dstapthecanhankt>(a.danhSachCaNhanKhenThuong);
                dh.hinhThucKhenThuong = _entities.qltdkt_dm_hinhthuckhenthuong.Find(a.hinhThucKhenThuong).tenHinhThucKhenThuong;
                return dh;
            }
            catch (Exception)
            {
                return dh;
            }
        }


        public int XoaDuLieuTrongSoKyYeu(int idKhenThuong)
        {
            try
            {
                var dh = _entities.qltdkt_khenthuong.Find(idKhenThuong);
                dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                if (ds.loaiKhenThuong == 0)
                {
                    foreach (var item in ds.dschitietcanhantapthekhenthuong)
                    {
                        var tt = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                        if (tt != null)
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(tt.hoSoKyYeu);
                            List<soKyYeuModel> newhsky = new List<soKyYeuModel>();
                            foreach (var rm in hsky)
                            {
                                if (rm.idKhenThuong != idKhenThuong)
                                {
                                    newhsky.Add(rm);
                                }
                            }

                            tt.hoSoKyYeu = JsonConvert.SerializeObject(newhsky);
                            tt.daXoa = 1;
                            tt.ngayTao = null;
                            _entities.SaveChanges();
                        }
                    }
                }
                else if (ds.loaiKhenThuong == 1)
                {
                    foreach (var item in ds.dschitietcanhantapthekhenthuong)
                    {
                        var tt = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                        if (tt != null)
                        {
                            List<soKyYeuModel> hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(tt.hoSoKyYeu);
                            List<soKyYeuModel> newhsky = new List<soKyYeuModel>();
                            foreach (var rm in hsky)
                            {
                                if (rm.idKhenThuong != idKhenThuong)
                                {
                                    newhsky.Add(rm);
                                }
                            }

                            tt.hoSoKyYeu = JsonConvert.SerializeObject(newhsky);
                            tt.daXoa = 1;
                            tt.ngayTao = null;

                            _entities.SaveChanges();
                        }
                    }
                }
                else
                {
                    foreach (var item in ds.dschitietcanhantapthekhenthuong)
                    {
                        if (item.loaiKT == 0)
                        {
                            var tt = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                            if (tt != null)
                            {
                                var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(tt.hoSoKyYeu);
                                List<soKyYeuModel> newhsky = new List<soKyYeuModel>();
                                foreach (var rm in hsky)
                                {
                                    if (rm.idKhenThuong != idKhenThuong)
                                    {
                                        newhsky.Add(rm);
                                    }
                                }

                                tt.hoSoKyYeu = JsonConvert.SerializeObject(newhsky);
                                tt.daXoa = 1;
                                tt.ngayTao = null;

                                _entities.SaveChanges();
                            }
                        }
                        else
                        {
                            var tt = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                            if (tt != null)
                            {
                                List<soKyYeuModel> hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(tt.hoSoKyYeu);
                                List<soKyYeuModel> newhsky = new List<soKyYeuModel>();
                                foreach (var rm in hsky)
                                {
                                    if (rm.idKhenThuong != idKhenThuong)
                                    {
                                        newhsky.Add(rm);
                                    }
                                }

                                tt.hoSoKyYeu = JsonConvert.SerializeObject(newhsky);
                                tt.daXoa = 1;
                                tt.ngayTao = null;

                                _entities.SaveChanges();
                            }
                        }
                    }
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int HuyTraoTangKhenThuong(int idKhenThuong)
        {
            try
            {
                var dh = _entities.qltdkt_khenthuong.Find(idKhenThuong);
                dh.ngayTraoTang = null;
                dh.daTraoTang = false;
                dh.hinhThucKhenThuong = null;
                XoaDuLieuTrongSoKyYeu(idKhenThuong);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public List<donViModel> loadDonViByDonVi(int idDonVi)
        {
            var dv = from a in _entities.qltdkt_dm_donvi
                     join b in _entities.qltdkt_dm_donvi on a.idCha equals b.id
                     where a.idCha == idDonVi
                     select new donViModel
                     {
                         idDonVi = a.id,
                         tenDonVi = a.tenDonVi,
                         tenDonViCha = b.tenDonVi
                     };
            return dv.ToList();

        }
        public List<donViModel> loadDonViByKieu()
        {
            var dv = from a in _entities.qltdkt_dm_donvi
                     join b in _entities.qltdkt_dm_donvi on a.idCha equals b.id

                     select new donViModel
                     {
                         idDonVi = a.id,

                         tenDonVi = a.tenDonVi,
                         tenDonViCha = b.tenDonVi
                     };
            return dv.ToList();

        }
        public List<nguoiDungModel> loadNhanVienByKieu()
        {
            var dv = from a in _entities.qltdkt_dm_nhanvien
                     join b in _entities.qltdkt_dm_donvi on a.donViId equals b.id
                     join c in _entities.qltdkt_dm_chucvu on a.chucVuId equals c.id
                     where a.daXoa == false && a.maNhanVien != "cntt_01" && a.trangThai == 0
                     select new nguoiDungModel
                     {
                         idNguoiDung = a.id,
                         hoTen = a.hoTen,
                         maNhanVien = a.maNhanVien,
                         tenDonVi = b.tenDonVi,
                         tenChucDanh = c.tenChucVu
                     };
            return dv.ToList();

        }

        public int TraoTangKhenThuong(int idkt, string ngaytraotang, int trangThai, int htkt)
        {
            try
            {
                var ngay = DateTime.ParseExact(ngaytraotang, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                var dh = _entities.qltdkt_khenthuong.Find(idkt);
                dh.ngayTraoTang = ngay;
                dh.daTraoTang = true;
                dh.trangThai = Convert.ToByte(trangThai);
                dh.hinhThucKhenThuong = htkt;
                dh.ngayTao = DateTime.Now;
                _entities.SaveChanges();
                LuuSoKyYeu(idkt);
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int xoaCaNhanTapThe(string idcntt, int idkt)
        {
            string[] spl = idcntt.Split('_');
            int _idcntt = int.Parse(spl[1]);
            if (spl[0] == "tt")
            {
                try
                {
                    var dh = _entities.qltdkt_khenthuong.Find(idkt);
                    dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                    var dstt = ds.dschitietcanhantapthekhenthuong.Where(x => x.loaiKT == 0).ToList();
                    var dscn = ds.dschitietcanhantapthekhenthuong.Where(x => x.loaiKT == 1).ToList();

                    var dsttNew = dstt.Where(x => x.id != _idcntt).ToList();
                    List<dschitietcanhantapthekhenthuong> _New = new List<dschitietcanhantapthekhenthuong>();

                    _New.AddRange(dsttNew);
                    _New.AddRange(dscn);

                    ds.dschitietcanhantapthekhenthuong = _New;
                    dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(ds);
                    _entities.SaveChanges();
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            else
            {
                try
                {
                    var dh = _entities.qltdkt_khenthuong.Find(idkt);
                    dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                    var dstt = ds.dschitietcanhantapthekhenthuong.Where(x => x.loaiKT == 0).ToList();
                    var dscn = ds.dschitietcanhantapthekhenthuong.Where(x => x.loaiKT == 1).ToList();

                    var dscnNew = dscn.Where(x => x.id != _idcntt).ToList();
                    List<dschitietcanhantapthekhenthuong> _New = new List<dschitietcanhantapthekhenthuong>();

                    _New.AddRange(dscnNew);
                    _New.AddRange(dstt);

                    ds.dschitietcanhantapthekhenthuong = _New;
                    dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(ds);
                    _entities.SaveChanges();
                    return 1;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            /* try
             {
                 var dh = _entities.qltdkt_khenthuong.Find(idkt);
                 dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                 var dsmoi = ds.dschitietcanhantapthekhenthuong.Where(x => x.id != idcntt).ToList();
                 ds.dschitietcanhantapthekhenthuong = dsmoi;
                 dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(ds);
                 _entities.SaveChanges();
                 return 1;
             }
             catch (Exception)
             {
                 return 0;
             }*/

        }
        public int suatienCaNhanTapThe(string idcntt, int idkt, string tien)
        {
            try
            {
                string[] ls = idcntt.Split(',');

                var dh = _entities.qltdkt_khenthuong.Find(idkt);
                dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                var dsmoi = ds.dschitietcanhantapthekhenthuong.ToList();
                for (int i = 0; i < ls.Length; i++)
                {
                    int q = int.Parse(ls[i]);

                    if (dsmoi.Any(x => x.id == q))
                    {
                        var item = dsmoi.FirstOrDefault(x => x.id == q);
                        item.tienThuong = tien;
                    }
                }
                ds.dschitietcanhantapthekhenthuong = dsmoi;
                dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(ds);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }
        public khenThuongModel getKhenThuongByID(int idKhenThuong)
        {
            var dh = _entities.qltdkt_khenthuong.Find(idKhenThuong);
            khenThuongModel a = new khenThuongModel();
            if (dh != null)
            {
                a.capKhenThuong = dh.capKhenThuong.ToString();
                a.id = dh.id;
                a.bophan = dh.bophan;
                a.ngayKhenThuong = ((DateTime)dh.ngayKhenThuong).ToString("dd/MM/yyyy");
                a.soHieu = dh.soHieu;
                if (dh.quyetDinh != "")
                {
                    a.quyetDinh = dh.quyetDinh;

                }
                else
                {
                    a.quyetDinh = "";
                }
                a.daTraoTang = (bool)dh.daTraoTang;
                a.noiDungKhenThuong = dh.noiDungKhenThuong;
                a.danhSachCaNhanTapThe = dh.danhSachCaNhanKhenThuong;
                a.capKyKhenThuong = dh.capKyKhenThuong;
                a.tongTien = dh.tongTienThuong;
                a.kieuKhenThuong = dh.kieuKhenThuong;
                if (dh.hinhThucKhenThuong != null)
                {
                    a.hinhThucKhenThuong = dh.hinhThucKhenThuong;

                }
                else
                {
                    a.hinhThucKhenThuong = null;
                }
                if (dh.ghichuHTKT != null || dh.ghichuHTKT != "")
                {
                    a.ghiChuHTKT = dh.ghichuHTKT;

                }
                else
                {
                    a.ghiChuHTKT = "";
                }
                a.doiTuongThamGia = dh.doiTuongThamGia;

                return a;
            }
            else
            {
                return a;
            }
        }

        public List<qltdkt_dm_hinhthuckhenthuong> GetTenKhenThuong(int loaiKhenThuong)
        {
            if (loaiKhenThuong == 0)
            {
                return _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == 0 && x.daXoa == false).ToList();
            }
            else if (loaiKhenThuong == 1)
            {
                return _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.loaiKhenThuong == 1 && x.daXoa == false).ToList();
            }
            else
            {
                return _entities.qltdkt_dm_hinhthuckhenthuong.Where(x => x.daXoa == false).ToList();
            }
        }
        public int xoaCaNhanTapTheKT(int idcnttkt, int idkt)
        {
            try
            {
                var dh = _entities.qltdkt_khenthuong.Find(idkt);
                dstapthecanhankt ds = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                var dsmoi = ds.dschitietcanhantapthekhenthuong.Where(x => x.id != idcnttkt).ToList();
                ds.dschitietcanhantapthekhenthuong = dsmoi;
                dh.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(ds);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

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
        public List<dschitietcanhantapthekhenthuong> getDSChiTietCNTTByIDcache(string idcntt, int ldh)
        {
            List<dschitietcanhantapthekhenthuong> dsct = new List<dschitietcanhantapthekhenthuong>();
            if (idcntt != "")
            {
                string[] ls = idcntt.Split(',');
                if (ldh == 1)
                {
                    for (int i = 0; i < ls.Length; i++)
                    {
                        if (ls[i] != "")
                        {
                            int id = int.Parse(ls[i]);
                            var ac = from a in _entities.qltdkt_dm_nhanvien
                                     join b in _entities.qltdkt_dm_chucvu on a.chucVuId equals b.id
                                     join c in _entities.qltdkt_dm_donvi on a.donViId equals c.id
                                     where a.id == id && a.trangThai == 0 && a.daXoa == false && a.maNhanVien != "cntt_01"
                                     select new dschitietcanhantapthekhenthuong
                                     {
                                         id = a.id,
                                         loaiKT = (byte)ldh,
                                         tenCaNhanTapThe = a.hoTen,
                                         chucDanh = b.tenChucVu,
                                         donVi = c.id.ToString(),

                                     };
                            dsct.Add(ac.FirstOrDefault());
                        }
                    }
                    foreach (var x in dsct)
                    {
                        x.donVi = getFullNameDonVi(int.Parse(x.donVi), ldh);
                    }
                }
                else if (ldh == 0)
                {
                    for (int i = 0; i < ls.Length; i++)
                    {
                        if (ls[i] != "")
                        {
                            dschitietcanhantapthekhenthuong ac = new dschitietcanhantapthekhenthuong();
                            int id = int.Parse(ls[i]);
                            var ds = _entities.qltdkt_dm_donvi.Find(id);
                            ac.id = ds.id;
                            ac.tenCaNhanTapThe = ds.tenDonVi;
                            ac.loaiKT = (byte)ldh;
                            ac.donVi = getFullNameDonVi(ds.id, ldh);
                            dsct.Add(ac);
                        }
                    }
                }
            }
            dsct = dsct.OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
            return dsct;
        }
        public List<dschitietcanhantapthekhenthuong> ThemCaNhanTapTheCache(string idcn, string idtt, int lkt, int idkt, string listCurrent)
        {

            List<dschitietcanhantapthekhenthuong> result = new List<dschitietcanhantapthekhenthuong>();
            if (idkt == 0)
            {
                if (idcn == "" && idtt == "" && listCurrent == "")
                {
                    var dh = _entities.qltdkt_khenthuong.FirstOrDefault();
                    if (dh.danhSachCaNhanKhenThuong == null || dh.danhSachCaNhanKhenThuong == "")
                    {
                        return result;
                    }
                    dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                    if (dscu.loaiKhenThuong == lkt)
                    {
                        return dscu.dschitietcanhantapthekhenthuong;
                    }
                }
                else
                {
                    var dh = _entities.qltdkt_khenthuong.FirstOrDefault();

                    if (lkt == 1)
                    {



                        var newlist = getDSChiTietCNTTByIDcache(idcn, 1);
                        if (newlist != null)
                        {
                            var curlist = getDSChiTietCNTTByIDcache(listCurrent, 1);
                            result.AddRange(curlist);


                        }
                        else
                        {
                            dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                            var curlist = dscu.dschitietcanhantapthekhenthuong;
                            result.AddRange(curlist);
                        }

                        result.AddRange(newlist);

                        return result.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                    }
                    else if (lkt == 0)
                    {

                        var newlist = getDSChiTietCNTTByIDcache(idtt, 0);
                        if (newlist != null)
                        {
                            var curlist = getDSChiTietCNTTByIDcache(listCurrent, 0);
                            result.AddRange(curlist);


                        }
                        else
                        {
                            dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                            var curlist = dscu.dschitietcanhantapthekhenthuong;
                            result.AddRange(curlist);
                        }

                        result.AddRange(newlist);
                        return result.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                    }
                    else
                    {
                        if (idtt != "")
                        {
                            List<dschitietcanhantapthekhenthuong> resultTT = new List<dschitietcanhantapthekhenthuong>();

                            var newlist = getDSChiTietCNTTByIDcache(idtt, 0);
                            if (newlist == null || dh.danhSachCaNhanKhenThuong == null)
                            {
                                var curlist = getDSChiTietCNTTByIDcache(listCurrent, 0);
                                result.AddRange(curlist);
                            }
                            else
                            {
                                dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                                var curlist = dscu.dschitietcanhantapthekhenthuong;
                                result.AddRange(curlist);
                            }

                            result.AddRange(newlist);
                            var lstTT = resultTT.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstTT);
                        }
                        if (idcn != "")
                        {
                            List<dschitietcanhantapthekhenthuong> resultCN = new List<dschitietcanhantapthekhenthuong>();

                            var newlist = getDSChiTietCNTTByIDcache(idcn, 1);
                            if (newlist == null || dh.danhSachCaNhanKhenThuong == null)
                            {
                                var curlist = getDSChiTietCNTTByIDcache(listCurrent, 1);
                                result.AddRange(curlist);


                            }
                            else
                            {
                                dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                                var curlist = dscu.dschitietcanhantapthekhenthuong;
                                result.AddRange(curlist);
                            }

                            result.AddRange(newlist);
                            var lstCN = resultCN.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstCN);
                        }
                    }
                }

            }
            else
            {
                if (idcn == "" && idtt == "" && listCurrent == "")
                {
                    var dh = _entities.qltdkt_khenthuong.Find(idkt);
                    if (dh.danhSachCaNhanKhenThuong == null || dh.danhSachCaNhanKhenThuong == "")
                    {
                        return result;
                    }
                    dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                    if (dscu.loaiKhenThuong == lkt)
                    {
                        return dscu.dschitietcanhantapthekhenthuong;
                    }
                }
                else
                {
                    var dh = _entities.qltdkt_khenthuong.Find(idkt);

                    if (lkt == 1)
                    {



                        var newlist = getDSChiTietCNTTByIDcache(idcn, 1);
                        if (newlist == null || dh.danhSachCaNhanKhenThuong == null)
                        {
                            var curlist = getDSChiTietCNTTByIDcache(listCurrent, 1);
                            result.AddRange(curlist);


                        }
                        else
                        {
                            dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                            var curlist = dscu.dschitietcanhantapthekhenthuong;
                            result.AddRange(curlist);
                        }

                        result.AddRange(newlist);

                        return result.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                    }
                    else if (lkt == 0)
                    {

                        var newlist = getDSChiTietCNTTByIDcache(idtt, 0);
                        if (newlist == null || dh.danhSachCaNhanKhenThuong == null)
                        {
                            var curlist = getDSChiTietCNTTByIDcache(listCurrent, 0);
                            result.AddRange(curlist);


                        }
                        else
                        {
                            dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                            var curlist = dscu.dschitietcanhantapthekhenthuong;
                            result.AddRange(curlist);
                        }

                        result.AddRange(newlist);
                        return result.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                    }
                    else
                    {
                        if (idtt != "")
                        {
                            List<dschitietcanhantapthekhenthuong> resultTT = new List<dschitietcanhantapthekhenthuong>();

                            var newlist = getDSChiTietCNTTByIDcache(idtt, 0);
                            if (newlist == null || dh.danhSachCaNhanKhenThuong == null)
                            {
                                var curlist = getDSChiTietCNTTByIDcache(listCurrent, 0);
                                result.AddRange(curlist);
                            }
                            else
                            {
                                dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                                var curlist = dscu.dschitietcanhantapthekhenthuong;
                                result.AddRange(curlist);
                            }

                            result.AddRange(newlist);
                            var lstTT = resultTT.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstTT);
                        }
                        if (idcn != "")
                        {
                            List<dschitietcanhantapthekhenthuong> resultCN = new List<dschitietcanhantapthekhenthuong>();

                            var newlist = getDSChiTietCNTTByIDcache(idcn, 1);
                            if (newlist == null || dh.danhSachCaNhanKhenThuong == null)
                            {
                                var curlist = getDSChiTietCNTTByIDcache(listCurrent, 1);
                                result.AddRange(curlist);


                            }
                            else
                            {
                                dstapthecanhankt dscu = JsonConvert.DeserializeObject<dstapthecanhankt>(dh.danhSachCaNhanKhenThuong);
                                var curlist = dscu.dschitietcanhantapthekhenthuong;
                                result.AddRange(curlist);
                            }

                            result.AddRange(newlist);
                            var lstCN = resultCN.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstCN);
                        }
                    }
                }
            }

            return result;


        }


    }

}