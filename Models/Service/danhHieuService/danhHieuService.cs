using Newtonsoft.Json;
using QLTDKT.Models.Service.DanhHieuService;
using QLTDKT.Models.Service.khenThuongService;
using QLTDKT.Models.Service.KhenThuongService;
using QLTDKT.Models.Service.soKyYeuService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace QLTDKT.Models.Service.danhHieuService
{
    public class danhHieuService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();
        public List<danhHieuModel> getDanhSachDanhHieu(int idTenDanhHieu, int bophan, int capKyKhenThuong, string soHieu, string ngayDanhHieu_TuNgay, string ngayDanhHieu_DenNgay, int trangThai, int namDanhHieu)
        {
            List<danhHieuModel> _result = new List<danhHieuModel>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.id, a.idTenDanhHieu, b.capThanhTich, a.bophan, a.soHieu, a.ngayDanhHieu, a.noiDung, a.fileGoc, a.daTraoTang, a.namDanhHieu, a.tuNam, a.denNam, a.nguoiKy FROM qltdkt_danhhieu a " +
                "left join qltdkt_dm_danhhieuthidua b on a.idTenDanhHieu = b.id ");
            sql.Append("WHERE 1=1 ");
            if (idTenDanhHieu != 0)
                sql.Append(string.Format("AND a.idTenDanhHieu = {0} ", idTenDanhHieu));
            if (bophan != 0)
                sql.Append(string.Format("AND a.bophan = {0} ", bophan));
            if (capKyKhenThuong != 0)
                sql.Append(string.Format("AND b.capThanhTich = {0} ", capKyKhenThuong));
            if (soHieu.Trim().Length > 0 || soHieu != "")
                sql.Append(string.Format("AND a.soHieu LIKE '%{0}%' ", soHieu));
            if (ngayDanhHieu_TuNgay != "" || ngayDanhHieu_DenNgay != "")
            {
                if (ngayDanhHieu_TuNgay == "")
                {
                    if (ngayDanhHieu_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngayDanhHieu, 'dd/MM/yyyy') <= '{0}' ", ngayDanhHieu_DenNgay));
                }
                else
                {
                    if (ngayDanhHieu_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngayDanhHieu, 'dd/MM/yyyy') <= '{0}' AND FORMAT(a.ngayDanhHieu, 'dd/MM/yyyy') >= '{1}' ", ngayDanhHieu_DenNgay, ngayDanhHieu_TuNgay));
                    else
                        sql.Append(string.Format("AND FORMAT(a.ngayDanhHieu, 'dd/MM/yyyy') >= '{0}' ", ngayDanhHieu_TuNgay));
                }
            }
            if (trangThai < 2)
            {
                sql.Append(string.Format("AND a.daTraoTang = {0} ", trangThai));
            }
            if (namDanhHieu != 0)
                sql.Append(string.Format("AND a.namDanhHieu = {0} ", namDanhHieu));
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
                        danhHieuModel _newObj = new danhHieuModel
                        {
                            id = int.Parse(dt.Rows[i]["id"].ToString()),
                            stt = stt,
                            bophan = int.Parse(dt.Rows[i]["bophan"].ToString()),
                            capKyKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(int.Parse(dt.Rows[i]["capThanhTich"].ToString())).tenCapKyKhenThuong,
                            tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(int.Parse(dt.Rows[i]["idTenDanhHieu"].ToString())).tenDanhHieuThiDua,
                            ngayDanhHieu = (DateTime.Parse(dt.Rows[i]["ngayDanhHieu"].ToString())).ToString("dd/MM/yyyy"),
                            soHieu = dt.Rows[i]["soHieu"].ToString(),
                            noiDungDanhHieu = dt.Rows[i]["noiDung"].ToString(),
                            fileGoc = dt.Rows[i]["fileGoc"].ToString(),
                            daTraoTang = bool.Parse(dt.Rows[i]["daTraoTang"].ToString()),
                            namDanhHieu = int.Parse(dt.Rows[i]["namDanhHieu"].ToString()),
                            tuNam = dt.Rows[i]["tuNam"].ToString(),
                            denNam = dt.Rows[i]["denNam"].ToString(),
                            nguoiKy = dt.Rows[i]["nguoiKy"].ToString()

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

        public List<dsChiTietCaNhanTapThe> ThemCaNhanTapTheCache(string idcn, string idtt, int ldh, int iddh, string listCurrent)
        {
            List<dsChiTietCaNhanTapThe> result = new List<dsChiTietCaNhanTapThe>();
            if (iddh == 0)
            {
                if (idcn == "" && idtt == "" && listCurrent == "")
                {
                    var dh = _entities.qltdkt_danhhieu.FirstOrDefault();
                    if (dh.danhSachCaNhanTapThe == null || dh.danhSachCaNhanTapThe == "")
                    {
                        return result;
                    }
                    danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                    if (dscu.loaiDanhHieu == ldh)
                    {
                        return dscu.dsChiTietCaNhanTapThe;
                    }
                }
                else
                {
                    var dh = _entities.qltdkt_danhhieu.FirstOrDefault();

                    if (ldh == 1)
                    {
                        var newlist = getDSChiTietCNTTByID(idcn, 1);
                        if (newlist != null)
                        {
                            var curlist = getDSChiTietCNTTByID(listCurrent, 1);
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
                    else if (ldh == 0)
                    {
                        var newlist = getDSChiTietCNTTByID(idtt, 0);
                        if (newlist != null)
                        {
                            var curlist = getDSChiTietCNTTByID(listCurrent, 0);
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
                    else
                    {
                        if (idtt != "")
                        {
                            List<dsChiTietCaNhanTapThe> resultTT = new List<dsChiTietCaNhanTapThe>();
                            var newlisttt = getDSChiTietCNTTByID(idtt, 0);
                            var curlisttt = getDSChiTietCNTTByID(listCurrent, 0);
                            resultTT.AddRange(newlisttt);
                            resultTT.AddRange(curlisttt);
                            var lstTT = resultTT.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstTT);
                        }
                        if (idcn != "")
                        {
                            List<dsChiTietCaNhanTapThe> resultCN = new List<dsChiTietCaNhanTapThe>();
                            var newlistcn = getDSChiTietCNTTByID(idcn, 1);
                            var curlistcn = getDSChiTietCNTTByID(listCurrent, 1);
                            resultCN.AddRange(newlistcn);
                            resultCN.AddRange(curlistcn);
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
                    var dh = _entities.qltdkt_danhhieu.Find(iddh);
                    if (dh.danhSachCaNhanTapThe == null || dh.danhSachCaNhanTapThe == "")
                    {
                        return result;
                    }
                    danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                    if (dscu.loaiDanhHieu == ldh)
                    {
                        return dscu.dsChiTietCaNhanTapThe;
                    }
                }
                else
                {
                    var dh = _entities.qltdkt_danhhieu.Find(iddh);

                    if (ldh == 1)
                    {
                        var newlist = getDSChiTietCNTTByID(idcn, 1);
                        if (newlist != null)
                        {
                            var curlist = getDSChiTietCNTTByID(listCurrent, 1);
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
                    else if (ldh == 0)
                    {
                        var newlist = getDSChiTietCNTTByID(idtt, 0);
                        if (newlist != null)
                        {
                            var curlist = getDSChiTietCNTTByID(listCurrent, 0);
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
                    else
                    {
                        if (idtt != "")
                        {
                            List<dsChiTietCaNhanTapThe> resultTT = new List<dsChiTietCaNhanTapThe>();
                            var newlisttt = getDSChiTietCNTTByID(idtt, 0);
                            var curlisttt = getDSChiTietCNTTByID(listCurrent, 0);
                            resultTT.AddRange(newlisttt);
                            resultTT.AddRange(curlisttt);
                            var lstTT = resultTT.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstTT);
                        }
                        if (idcn != "")
                        {
                            List<dsChiTietCaNhanTapThe> resultCN = new List<dsChiTietCaNhanTapThe>();
                            var newlistcn = getDSChiTietCNTTByID(idcn, 1);
                            var curlistcn = getDSChiTietCNTTByID(listCurrent, 1);
                            resultCN.AddRange(newlistcn);
                            resultCN.AddRange(curlistcn);
                            var lstCN = resultCN.GroupBy(x => x.id).Select(x => x.First()).OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
                            result.AddRange(lstCN);
                        }
                    }
                }
            }

            return result;
        }
        public int LuuCaNhan(string idcn, int ldh, int iddh)
        {
            var dh = _entities.qltdkt_danhhieu.Find(iddh);
            danhSachCaNhanTapThe dsCaNhanTapThe = new danhSachCaNhanTapThe();
            dsCaNhanTapThe.loaiDanhHieu = ldh;
            dsCaNhanTapThe.dsChiTietCaNhanTapThe = getDSChiTietCNTTByID(idcn, ldh);
            try
            {
                dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int LuuTapThe(string idtt, int ldh, int iddh)
        {
            var dh = _entities.qltdkt_danhhieu.Find(iddh);
            danhSachCaNhanTapThe dsCaNhanTapThe = new danhSachCaNhanTapThe();
            dsCaNhanTapThe.loaiDanhHieu = ldh;
            dsCaNhanTapThe.dsChiTietCaNhanTapThe = getDSChiTietCNTTByID(idtt, ldh);
            try
            {
                dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int LuuTapTheCaNhan(string idtt, string idcn, int ldh, int iddh)
        {
            var dh = _entities.qltdkt_danhhieu.Find(iddh);
            danhSachCaNhanTapThe dsCaNhanTapThe = new danhSachCaNhanTapThe();
            dsCaNhanTapThe.loaiDanhHieu = ldh;
            var dstt = getDSChiTietCNTTByID(idtt, 0);
            var dscn = getDSChiTietCNTTByID(idcn, 1);

            List<dsChiTietCaNhanTapThe> result = new List<dsChiTietCaNhanTapThe>();
            result.AddRange(dstt);
            result.AddRange(dscn);
            dsCaNhanTapThe.dsChiTietCaNhanTapThe = result;
            try
            {
                dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                _entities.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        public int ThemCaNhanTapThe(string idcn, string idtt, byte ldh, int iddh)
        {
            if (ldh < 2)
            {
                qltdkt_danhhieu dhk = new qltdkt_danhhieu();
                danhSachCaNhanTapThe dsCaNhanTapThe = new danhSachCaNhanTapThe();

                dhk.kieuDanhHieu = ldh;
                if (ldh == 1)
                {
                    dsCaNhanTapThe.dsChiTietCaNhanTapThe = getDSChiTietCNTTByID(idcn, 1);
                }
                else if (ldh == 0)
                {
                    dsCaNhanTapThe.dsChiTietCaNhanTapThe = getDSChiTietCNTTByID(idtt, 0);
                }

                try
                {
                    var dh = _entities.qltdkt_danhhieu.Find(iddh);
                    if (dh.danhSachCaNhanTapThe == null || dh.danhSachCaNhanTapThe == "")
                    {
                        dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                        _entities.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                        if (dscu.loaiDanhHieu == ldh)
                        {
                            dscu.dsChiTietCaNhanTapThe.AddRange(dsCaNhanTapThe.dsChiTietCaNhanTapThe);
                            dscu.dsChiTietCaNhanTapThe = dscu.dsChiTietCaNhanTapThe.GroupBy(x => x.id).Select(x => x.First()).ToList();

                            dsCaNhanTapThe.dsChiTietCaNhanTapThe = dscu.dsChiTietCaNhanTapThe;
                            dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                            _entities.SaveChanges();
                            return 1;
                        }
                        else
                        {
                            danhSachCaNhanTapThe _dsNew = new danhSachCaNhanTapThe();
                            _dsNew.loaiDanhHieu = ldh;
                            _dsNew.dsChiTietCaNhanTapThe = dsCaNhanTapThe.dsChiTietCaNhanTapThe;
                            dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(_dsNew);
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
            else
            {
                danhSachCaNhanTapThe dsCaNhanTapThe = new danhSachCaNhanTapThe();

                dsCaNhanTapThe.loaiDanhHieu = ldh;

                var dscn = getDSChiTietCNTTByID(idcn, 1);
                var dstt = getDSChiTietCNTTByID(idtt, 0);

                try
                {
                    var dh = _entities.qltdkt_danhhieu.Find(iddh);
                    if (dh.danhSachCaNhanTapThe == null || dh.danhSachCaNhanTapThe == "")
                    {
                        dsCaNhanTapThe.dsChiTietCaNhanTapThe.AddRange(dscn);
                        dsCaNhanTapThe.dsChiTietCaNhanTapThe.AddRange(dstt);
                        dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                        _entities.SaveChanges();
                        return 1;
                    }
                    else
                    {
                        danhSachCaNhanTapThe dscu = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);

                        List<dsChiTietCaNhanTapThe> _dscn = dscu.dsChiTietCaNhanTapThe.Where(x => x.loaiDanhHieu == 1).ToList();
                        _dscn.AddRange(dscn);
                        _dscn = _dscn.GroupBy(x => x.id).Select(x => x.First()).ToList();

                        List<dsChiTietCaNhanTapThe> _dstt = dscu.dsChiTietCaNhanTapThe.Where(x => x.loaiDanhHieu == 0).ToList();
                        _dstt.AddRange(dstt);
                        _dstt = _dstt.GroupBy(x => x.id).Select(x => x.First()).ToList();

                        List<dsChiTietCaNhanTapThe> _dsNew = new List<dsChiTietCaNhanTapThe>();
                        _dsNew.AddRange(_dscn);
                        _dsNew.AddRange(_dstt);


                        //dscu.dsChiTietCaNhanTapThe.AddRange(dsCaNhanTapThe.dsChiTietCaNhanTapThe);
                        //dscu.dsChiTietCaNhanTapThe = dscu.dsChiTietCaNhanTapThe.GroupBy(x => x.id).Select(x => x.First()).ToList();

                        dsCaNhanTapThe.dsChiTietCaNhanTapThe = _dsNew;
                        dh.danhSachCaNhanTapThe = JsonConvert.SerializeObject(dsCaNhanTapThe);
                        _entities.SaveChanges();
                        return 1;
                    }
                }
                catch (Exception)
                {
                    return -1;
                }
            }

        }
        public int xoaCaNhanTapThe(string idcntt, int iddh)
        {
            string[] spl = idcntt.Split('_');
            int _idcntt = int.Parse(spl[1]);
            if (spl[0] == "tt")
            {
                try
                {
                    var dh = _entities.qltdkt_danhhieu.Find(iddh);
                    danhSachCaNhanTapThe ds = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                    var dstt = ds.dsChiTietCaNhanTapThe.Where(x => x.loaiDanhHieu == 0).ToList();
                    var dscn = ds.dsChiTietCaNhanTapThe.Where(x => x.loaiDanhHieu == 1).ToList();

                    var dsttNew = dstt.Where(x => x.id != _idcntt).ToList();
                    List<dsChiTietCaNhanTapThe> _New = new List<dsChiTietCaNhanTapThe>();

                    _New.AddRange(dsttNew);
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
            else
            {
                try
                {
                    var dh = _entities.qltdkt_danhhieu.Find(iddh);
                    danhSachCaNhanTapThe ds = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                    var dstt = ds.dsChiTietCaNhanTapThe.Where(x => x.loaiDanhHieu == 0).ToList();
                    var dscn = ds.dsChiTietCaNhanTapThe.Where(x => x.loaiDanhHieu == 1).ToList();

                    var dscnNew = dscn.Where(x => x.id != _idcntt).ToList();
                    List<dsChiTietCaNhanTapThe> _New = new List<dsChiTietCaNhanTapThe>();

                    _New.AddRange(dscnNew);
                    _New.AddRange(dstt);

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
                             where a.donViId == idDV && a.daXoa == false && a.trangThai == 0
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
                                  where a.donViId == item.id && a.daXoa == false && a.trangThai == 0
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
                             where a.hoTen.Contains(searchText) && a.daXoa == false && a.trangThai == 0
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

        public int TraoTangDanhHieu(int iddh, string fileUpload)
        {
            try
            {
                var dh = _entities.qltdkt_danhhieu.Find(iddh);
                dh.hinhAnhTraoTang = fileUpload;
                _entities.SaveChanges();
                var ck = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).luuSoKyYeu;
                if (ck == true)
                {
                    LuuSoKyYeu(iddh, fileUpload);
                }
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public int LuuSoKyYeu(int iddh, string fileUpload)
        {
            var dh = _entities.qltdkt_danhhieu.Find(iddh);

            var chitietdh = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);

            foreach (var item in chitietdh.dsChiTietCaNhanTapThe)
            {
                if (item.loaiDanhHieu == 0) // tập thể
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {
                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idDanhHieu == iddh).FirstOrDefault();
                            if (ck == null) //check đã có mã danh hiệu này chưa, nếu chưa thì add
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 2;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = dh.nguoiKy;
                                //sky.idTenDanhHieu = ((byte)dh.idTenDanhHieu).ToString();
                                sky.idDanhHieu = iddh;
                                sky.namDanhHieu = (int)dh.namDanhHieu;
                                sky.hinhAnhTraoTang = fileUpload;
                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;

                                _entities.SaveChanges();
                            }
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
                            sky.kieuKyYeu = 2;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                            sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = dh.nguoiKy;

                            sky.namDanhHieu = (int)dh.namDanhHieu;
                            sky.hinhAnhTraoTang = fileUpload;
                            //sky.capDanhHieu = getCapDanhHieu(dh.capDanhHieu);
                            sky.idDanhHieu = iddh;

                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
                else // cá nhân
                {
                    var check = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == item.id).FirstOrDefault();
                    if (check != null) //nếu có thì update
                    {
                        try
                        {
                            var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(check.hoSoKyYeu);
                            var ck = hsky.Where(x => x.idDanhHieu == iddh).FirstOrDefault();
                            if (ck == null)
                            {
                                soKyYeuModel sky = new soKyYeuModel();
                                sky.kieuKyYeu = 2;
                                sky.ngayKyYeu = DateTime.Now;
                                sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                                sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;
                                sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                                sky.capKhenThuong = dh.nguoiKy;

                                sky.namDanhHieu = (int)dh.namDanhHieu;
                                sky.idDanhHieu = iddh;
                                sky.hinhAnhTraoTang = fileUpload;

                                hsky.Add(sky);
                                check.hoSoKyYeu = JsonConvert.SerializeObject(hsky);
                                check.daXoa = 0;

                                _entities.SaveChanges();
                            }
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
                            sky.kieuKyYeu = 2;
                            sky.ngayKyYeu = DateTime.Now;
                            sky.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                            sky.hinhThucTraoTang = _entities.qltdkt_dm_hinhthuckhenthuong.Find(dh.hinhThucTraoTang).tenHinhThucKhenThuong;
                            sky.ngayTraoTang = (DateTime)dh.ngayTraoTang;
                            sky.capKhenThuong = dh.nguoiKy;

                            sky.namDanhHieu = (int)dh.namDanhHieu;
                            sky.idDanhHieu = iddh;
                            sky.hinhAnhTraoTang = fileUpload;

                            List<soKyYeuModel> _sky = new List<soKyYeuModel>();
                            _sky.Add(sky);

                            hsky.hoSoKyYeu = JsonConvert.SerializeObject(_sky);

                            _entities.qltdkt_hosokyyeu.Add(hsky);
                            _entities.SaveChanges();
                        }
                        catch (Exception)
                        {
                            return 0;
                        }
                    }
                }
            }
            return 1;



        }

        public int chuyenSangKhenThuong(int iddh)
        {
            var dh = _entities.qltdkt_danhhieu.Find(iddh);
            danhSachCaNhanTapThe dsCaNhanTapThe = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
            var check = _entities.qltdkt_khenthuong.Where(x => x.soHieu.Trim() == dh.soHieu.Trim() && x.daXoa == false).FirstOrDefault();
            if (check != null)
            {
                return 0;  // "Đã tồn tại số hiệu văn bản ở phần Khen thưởng"
            }
            else
            {
                try
                {
                    qltdkt_khenthuong kt = new qltdkt_khenthuong();

                    kt.quyetDinh = dh.fileGoc;
                    kt.soHieu = dh.soHieu;
                    kt.ngayKhenThuong = (DateTime)dh.ngayDanhHieu;
                    kt.doiTuongThamGia = dh.kieuDanhHieu;
                    kt.kieuKhenThuong = 1; //1: năm, 2: đột xuất, 3: chuyên đề, 4: giai đoạn; tạm đặt mặc định = 1
                    kt.hinhThucKhenThuong = dh.hinhThucTraoTang;
                    kt.noiDungKhenThuong = dh.noiDung;
                    kt.ngayTao = DateTime.Now;
                    kt.trangThai = 0;
                    kt.daTraoTang = false;
                    kt.daXoa = false;

                    dstapthecanhankt dscnttkt = new dstapthecanhankt();
                    dscnttkt.loaiKhenThuong = dsCaNhanTapThe.loaiDanhHieu;

                    List<dschitietcanhantapthekhenthuong> _listct = new List<dschitietcanhantapthekhenthuong>();
                    foreach (var item in dsCaNhanTapThe.dsChiTietCaNhanTapThe)
                    {
                        dschitietcanhantapthekhenthuong ct = new dschitietcanhantapthekhenthuong();
                        ct.id = item.id;
                        ct.ischeck = true;
                        ct.tenCaNhanTapThe = item.tenCaNhanTapThe;
                        ct.loaiKT = (byte)item.loaiDanhHieu;
                        ct.donVi = item.donVi;
                        ct.chucDanh = item.chucDanh;
                        _listct.Add(ct);
                    }

                    dscnttkt.dschitietcanhantapthekhenthuong = _listct;
                    kt.danhSachCaNhanKhenThuong = JsonConvert.SerializeObject(dscnttkt);

                    _entities.qltdkt_khenthuong.Add(kt);
                    _entities.SaveChanges();
                    return 1; // "Chuyển dữ liệu danh hiệu sang khen thưởng thành công.";
                }
                catch (Exception ex)
                {
                    return 2; //Lỗi
                }
            }
        }

        public int HuyTraoTangDanhHieu(int idDanhHieu)
        {
            try
            {
                int xoasky = XoaDuLieuTrongSoKyYeu(idDanhHieu);
                if (xoasky == 1)
                {
                    var dh = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                    dh.ngayTraoTang = null;
                    dh.daTraoTang = false;
                    dh.hinhThucTraoTang = null;
                    _entities.SaveChanges();
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int XoaDuLieuTrongSoKyYeu(int idDanhHieu)
        {
            try
            {
                var dh = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                if (dh != null)
                {
                    if (dh.danhSachCaNhanTapThe != null)
                    {
                        danhSachCaNhanTapThe ds = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(dh.danhSachCaNhanTapThe);
                        foreach (var item in ds.dsChiTietCaNhanTapThe)
                        {
                            if (item.loaiDanhHieu == 0)
                            {
                                var tt = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == item.id && x.idNhanVien == null).FirstOrDefault();
                                if (tt != null)
                                {
                                    var hsky = JsonConvert.DeserializeObject<List<soKyYeuModel>>(tt.hoSoKyYeu);
                                    List<soKyYeuModel> newhsky = new List<soKyYeuModel>();
                                    foreach (var rm in hsky)
                                    {
                                        if (rm.idDanhHieu != idDanhHieu)
                                        {
                                            newhsky.Add(rm);
                                        }
                                    }

                                    tt.hoSoKyYeu = JsonConvert.SerializeObject(newhsky);
                                    tt.daXoa = 1;
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
                                        if (rm.idDanhHieu != idDanhHieu)
                                        {
                                            newhsky.Add(rm);
                                        }
                                    }

                                    tt.hoSoKyYeu = JsonConvert.SerializeObject(newhsky);
                                    tt.daXoa = 1;

                                    _entities.SaveChanges();
                                }
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

        public xemDanhHieuModel xemDanhHieu(int idDanhHieu)
        {
            xemDanhHieuModel a = new xemDanhHieuModel();
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
                    a.danhSachCaNhanTapThe = dh.danhSachCaNhanTapThe;
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
            /*xemDanhHieuModel dh = new xemDanhHieuModel();
            try
            {
                var a = _entities.qltdkt_danhhieu.Find(idDanhHieu);
                dh.id = a.id;
                dh.soHieu = a.soHieu;
                dh.ngayDanhHieu = ((DateTime)a.ngayDanhHieu).ToString("dd/MM/yyyy");
                dh.ngayTraoTang = ((DateTime)a.ngayDanhHieu).ToString("dd/MM/yyyy");
                dh.namDanhHieu = (int)a.namDanhHieu;
                dh.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(a.idTenDanhHieu).tenDanhHieuThiDua;
                dh.fileGoc = a.fileGoc;
                dh.noiDungDanhHieu = a.noiDung;
                dh.capKyKhenThuong = _entities.qltdkt_dm_capkykhenthuong.Find(int.Parse(a.capKyKhenThuong)).tenCapKyKhenThuong;
                dh.daTraoTang = a.daTraoTang == true ? "Đã trao tặng" : "Chưa trao tặng";
                dh.hinhThucTraoTang = a.hinhThucTraoTang != null ? _entities.qltdkt_dm_hinhthuckhenthuong.Find(a.hinhThucTraoTang).tenHinhThucKhenThuong : "";
                dh.ngayTraoTang = a.ngayTraoTang != null ? ((DateTime)a.ngayTraoTang).ToString("dd/MM/yyyy") : "";
                a.danhSachCaNhanTapThe = dh.danhSachCaNhanTapThe;
                dh.kieuDanhHieu = a.kieuDanhHieu;
                return dh;
            }
            catch (Exception)
            {
                return dh;
            }*/
        }

        public danhSachCaNhanTapThe loadDsCaNhanTapThe(int idDanhHieu)
        {
            danhSachCaNhanTapThe ds = new danhSachCaNhanTapThe();
            var item = _entities.qltdkt_danhhieu.Find(idDanhHieu);
            if (item.danhSachCaNhanTapThe != null)
            {
                ds = JsonConvert.DeserializeObject<danhSachCaNhanTapThe>(item.danhSachCaNhanTapThe);
            }
            else
            {
                ds.loaiDanhHieu = (int)item.kieuDanhHieu;
            }
            return ds;
        }
        public List<dsChiTietCaNhanTapThe> getDSChiTietCNTTByID(string idcntt, int ldh)
        {
            List<dsChiTietCaNhanTapThe> dsct = new List<dsChiTietCaNhanTapThe>();
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
                                     select new dsChiTietCaNhanTapThe
                                     {
                                         id = a.id,
                                         loaiDanhHieu = ldh,
                                         tenCaNhanTapThe = a.hoTen,
                                         chucDanh = b.tenChucVu,
                                         donVi = c.id.ToString(),
                                         donViCha = c.idCha.ToString()
                                     };
                            dsct.Add(ac.FirstOrDefault());
                        }
                    }
                    foreach (var x in dsct)
                    {
                        x.donVi = getFullNameDonVi(int.Parse(x.donVi), ldh);
                    }
                }
                else
                {
                    for (int i = 0; i < ls.Length; i++)
                    {
                        if (ls[i] != "")
                        {
                            dsChiTietCaNhanTapThe ac = new dsChiTietCaNhanTapThe();
                            int id = int.Parse(ls[i]);
                            var ds = _entities.qltdkt_dm_donvi.Find(id);
                            ac.id = ds.id;
                            ac.tenCaNhanTapThe = ds.tenDonVi;
                            ac.loaiDanhHieu = ldh;
                            ac.donVi = getFullNameDonVi(ds.id, ldh);
                            dsct.Add(ac);
                        }
                    }
                }
            }
            dsct = dsct.OrderBy(x => x.id).ThenBy(x => x.donVi).ToList();
            return dsct;
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
                         tenDonVi = b.tenDonVi,
                         tenChucDanh = c.tenChucVu
                     };
            return dv.ToList();
        }


        public List<qltdkt_dm_capkykhenthuong> ListCapKyKhenThuong()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            return _entities.qltdkt_dm_capkykhenthuong.Where(x => x.daXoa == false).ToList();
        }

        public List<qltdkt_dm_danhhieuthidua> GetListLoaiDanhHieu()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            return _entities.qltdkt_dm_danhhieuthidua.Where(x => x.daXoa == false).ToList();
        }

        public danhHieuModel getDanhHieuByID(int idDanhHieu)
        {
            var dh = _entities.qltdkt_danhhieu.Find(idDanhHieu);
            danhHieuModel a = new danhHieuModel();
            if (dh != null)
            {
                a.idTenDanhHieu = (int)dh.idTenDanhHieu;
                a.id = dh.id;
                a.ngayDanhHieu = ((DateTime)dh.ngayDanhHieu).ToString("dd/MM/yyyy");
                a.ngayTraoTang = ((DateTime)dh.ngayDanhHieu).ToString("dd/MM/yyyy");
                a.soHieu = dh.soHieu;
                a.bophan = dh.bophan;
                a.kieuDanhHieu = dh.kieuDanhHieu;
                a.fileGoc = dh.fileGoc;
                a.tuNam = dh.tuNam;
                a.denNam = dh.denNam;
                a.nguoiKy = dh.nguoiKy;
                a.hinhAnhTraoTang = dh.hinhAnhTraoTang;
                a.daTraoTang = (bool)dh.daTraoTang;
                a.noiDungDanhHieu = dh.noiDung;
                a.danhSachCaNhanTapThe = dh.danhSachCaNhanTapThe;
                a.capKyKhenThuong = dh.capKyKhenThuong;
                a.namDanhHieu = (int)dh.namDanhHieu;
                a.tenDanhHieu = _entities.qltdkt_dm_danhhieuthidua.Find(dh.idTenDanhHieu).tenDanhHieuThiDua;
                if (dh.hinhThucTraoTang != null)
                {
                    a.hinhThucTraoTang = dh.hinhThucTraoTang;

                }
                else
                {
                    a.hinhThucTraoTang = null;
                }
                if (dh.ghiChuTT != null || dh.ghiChuTT != "")
                {
                    a.ghiChuTT = dh.ghiChuTT;

                }
                else
                {
                    a.ghiChuTT = "";
                }


                return a;
            }
            else
            {
                return a;
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
    }
}