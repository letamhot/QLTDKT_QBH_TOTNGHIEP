using Newtonsoft.Json;
using QLTDKT.Models.Service.danhHieuService;
using QLTDKT.Models.Service.khenThuongService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

namespace QLTDKT.Models.Service.sangKienService
{
    public class sangKienService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();
        public List<sangKienModel> getDanhSachSangKien(int bophan, string soQuyetDinh, string ngaySangKien_TuNgay, string ngaySangKien_DenNgay)
        {
            List<sangKienModel> _result = new List<sangKienModel>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.id, a.soQuyetDinh, a.idSangKien,a.bophan, a.loaiSangKien,a.tienThuong, a.ngaySangKien, b.tenSangKien, b.noiDungSangKien FROM qltdkt_sangkien a LEFT JOIN qltdkt_dm_sangkien b ON b.id = a.idSangKien ");
            sql.Append("WHERE 1=1 ");
            
            if (bophan != 0)
                sql.Append(string.Format("AND a.bophan = {0} ", bophan));
            if (soQuyetDinh.Trim().Length > 0 || soQuyetDinh != "")
                sql.Append(string.Format("AND a.soQuyetDinh LIKE '%{0}%' ", soQuyetDinh));
            if (ngaySangKien_TuNgay != "" || ngaySangKien_DenNgay != "")
            {
                if (ngaySangKien_TuNgay == "")
                {
                    if (ngaySangKien_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngaySangKien, 'dd/MM/yyyy') <= '{0}' ", ngaySangKien_DenNgay));
                }
                else
                {
                    if (ngaySangKien_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngaySangKien, 'dd/MM/yyyy') <= '{0}' AND FORMAT(a.ngaySangKien, 'dd/MM/yyyy') >= '{1}' ", ngaySangKien_DenNgay, ngaySangKien_TuNgay));
                    else
                        sql.Append(string.Format("AND FORMAT(a.ngaySangKien, 'dd/MM/yyyy') >= '{0}' ", ngaySangKien_TuNgay));
                }
            }
            
            sql.Append("AND a.daXoa = 0 ");
            sql.Append("ORDER BY a.idSangKien DESC");
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
                        sangKienModel _newObj = new sangKienModel
                        {
                            id = int.Parse(dt.Rows[i]["id"].ToString()),
                            stt = stt,
                            bophan = int.Parse(dt.Rows[i]["bophan"].ToString()),
                            tenSangKien = _entities.qltdkt_dm_sangkien.Find(int.Parse(dt.Rows[i]["idSangKien"].ToString())).tenSangKien,
                            noiDungSangKien = _entities.qltdkt_dm_sangkien.Find(int.Parse(dt.Rows[i]["idSangKien"].ToString())).noiDungSangKien,
                            ngaySangKien = (DateTime.Parse(dt.Rows[i]["ngaySangKien"].ToString())).ToString("dd/MM/yyyy"),
                            soQuyetDinh = dt.Rows[i]["soQuyetDinh"].ToString(),
                            loaiSangKien = int.Parse(dt.Rows[i]["loaiSangKien"].ToString()),
                            tongTien = int.Parse(dt.Rows[i]["tienThuong"].ToString())
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
        public danhSachCaNhanTapThe loadDsCaNhanTapThe(int idKT)
        {
            danhSachCaNhanTapThe ds = new danhSachCaNhanTapThe();
            var item = _entities.qltdkt_sangkien.Find(idKT);
            if (item.danhSachCaNhanTapThe != null)
            {
                ds = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
            }
            else
            {
                ds.loaiSangKien = (int)item.loaiSangKien;
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
        public List<dsChiTietCaNhanTapThe> getDSChiTietCNTTByID(string idcntt, string tien, double tongtien)
        {
            if (idcntt == "")
            {
                return null;
            }
            else
            {
                List<dsChiTietCaNhanTapThe> dsct = new List<dsChiTietCaNhanTapThe>();
                string[] ls = idcntt.Split(',');
                string[] lst = tien.Split(',');
                tongtien = 0;
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
                             select new dsChiTietCaNhanTapThe
                             {
                                 id = a.id,
                                 tenCaNhanTapThe = a.hoTen,
                                 chucDanh = b.tenChucVu,
                                 donVi = c.tenDonVi,
                                 tienThuong = t
                             };
                    dsct.Add(ac.FirstOrDefault());
                }
                return dsct;
            }
        }
        public string getFullNameDonVi(int idDonVi)
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
        public List<dsChiTietCaNhanTapThe> getDSChiTietCNTTByIDcache(string idcntt)
        {
            List<dsChiTietCaNhanTapThe> dsct = new List<dsChiTietCaNhanTapThe>();
            if (idcntt != "")
            {
                string[] ls = idcntt.Split(',');
                for (int i = 0; i < ls.Length; i++)
                {
                    if (ls[i] != "")
                    {
                        int id = int.Parse(ls[i]);
                        var ac = from a in _entities.qltdkt_dm_nhanvien
                                 join b in _entities.qltdkt_dm_chucvu on a.chucVuId equals b.id
                                 join c in _entities.qltdkt_dm_donvi on a.donViId equals c.id
                                 where a.id == id && a.trangThai == 0 && a.daXoa == false && a.maNhanVien != "cntt_01"
                                 select new dsChiTietCaNhanTapThe
                                 {
                                     id = a.id,
                                     tenCaNhanTapThe = a.hoTen,
                                     chucDanh = b.tenChucVu,
                                     donVi = c.id.ToString(),

                                 };
                        dsct.Add(ac.FirstOrDefault());
                    }
                }
                foreach (var x in dsct)
                {
                    x.donVi = getFullNameDonVi(int.Parse(x.donVi));
                }
            }
            dsct = dsct.OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
            return dsct;
        }
        public List<dsChiTietCaNhanTapThe> ThemCaNhanTapTheCache(string idcn, int idsk, string listCurrent)
        {

            List<dsChiTietCaNhanTapThe> result = new List<dsChiTietCaNhanTapThe>();
            if (idsk == 0)
            {
                if (idcn == "" && listCurrent == "")
                {
                    var dh = _entities.qltdkt_sangkien.FirstOrDefault();
                    if (dh.danhSachCaNhanTapThe == null || dh.danhSachCaNhanTapThe == "")
                    {
                        return result;
                    }
                    danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                    return dscu.dsChiTietCaNhanTapThe;

                }
                else
                {
                    var dh = _entities.qltdkt_sangkien.FirstOrDefault();
                    var newlist = getDSChiTietCNTTByIDcache(idcn);
                    if (newlist != null)
                    {
                        var curlist = getDSChiTietCNTTByIDcache(listCurrent);
                        result.AddRange(curlist);


                    }
                    else
                    {
                        danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                        var curlist = dscu.dsChiTietCaNhanTapThe;
                        result.AddRange(curlist);
                    }

                    result.AddRange(newlist);

                    return result.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                }

            }
            else
            {
                if (idcn == ""  && listCurrent == "")
                {
                    var dh = _entities.qltdkt_sangkien.Find(idsk);
                    if (dh.danhSachCaNhanTapThe == null || dh.danhSachCaNhanTapThe == "")
                    {
                        return result;
                    }
                    danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                    return dscu.dsChiTietCaNhanTapThe;

                }
                else
                {
                    var dh = _entities.qltdkt_sangkien.Find(idsk);

                    var newlist = getDSChiTietCNTTByIDcache(idcn);
                    if (newlist == null || dh.danhSachCaNhanTapThe == null)
                    {
                        var curlist = getDSChiTietCNTTByIDcache(listCurrent);
                        result.AddRange(curlist);


                    }
                    else
                    {
                        danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                        var curlist = dscu.dsChiTietCaNhanTapThe;
                        result.AddRange(curlist);
                    }

                    result.AddRange(newlist);

                    return result.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                }
            }

            return result;


        }
        public int xoaCaNhanTapThe(string idcntt, int idsk)
        {
            string[] spl = idcntt.Split('_');
            int _idcntt = int.Parse(spl[1]);
            try
            {
                var dh = _entities.qltdkt_sangkien.Find(idsk);
                danhSachCaNhanTapThe ds = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                var dscn = ds.dsChiTietCaNhanTapThe.ToList();

                List<dsChiTietCaNhanTapThe> _New = new List<dsChiTietCaNhanTapThe>();

                _New.AddRange(dscn);

                ds.dsChiTietCaNhanTapThe = _New;
                dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(ds);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public sangKienModel getSangKienByID(int id)
        {
            var dh = _entities.qltdkt_sangkien.Find(id);
            sangKienModel a = new sangKienModel();
            if (dh != null)
            {
                a.id = dh.id;
                a.bophan = (int)dh.bophan;
                a.ngaySangKien = ((DateTime)dh.ngaySangKien).ToString("dd/MM/yyyy");
                a.soQuyetDinh = dh.soQuyetDinh;

                a.tenSangKien = _entities.qltdkt_dm_sangkien.Find(dh.idSangKien).tenSangKien;

                a.danhSachCaNhanTapThe = dh.danhSachCaNhanTapThe;
                a.tongTien = dh.tienThuong;
                a.loaiSangKien = dh.loaiSangKien;
                a.idDmSangKien = (int)dh.idSangKien;
                a.noiDungSangKien = _entities.qltdkt_dm_sangkien.Find(dh.idSangKien).noiDungSangKien;
                return a;
            }
            else
            {
                return a;
            }
        }
    }
}