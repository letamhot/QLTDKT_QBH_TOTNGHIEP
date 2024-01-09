using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace QLTDKT.Models.Service.thiDuaService
{

    public class thiDuaService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();

        public List<thiDuaModel> getDanhSachThiDua(int capPhatDong, string soHieu, string ngayPhatDong_TuNgay, string ngayPhatDong_DenNgay, int trangThai)
        {
            List<thiDuaModel> _result = new List<thiDuaModel>();
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT a.id, a.phatDongId, b.tenPhatDong, a.soHieu, a.ngayPhatDong, a.NgayKetThuc, a.noiDungPhatDong, a.quyetDinh, a.trangThai FROM qltdkt_thidua a LEFT JOIN qltdkt_dm_donviphatdong b ");
            sql.Append("ON b.id = a.phatDongId ");
            sql.Append("WHERE 1=1 ");
            if (capPhatDong > 0)
                sql.Append(string.Format("AND a.phatDongId = {0} ", capPhatDong));
            if (soHieu.Trim().Length > 0 || soHieu != "")
                sql.Append(string.Format("AND a.soHieu LIKE '%{0}%' ", soHieu));
            if (ngayPhatDong_TuNgay != "" || ngayPhatDong_DenNgay != "")
            {
                if (ngayPhatDong_TuNgay == "")
                {
                    if (ngayPhatDong_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngayPhatDong, 'dd/MM/yyyy') <= '{0}' ", ngayPhatDong_DenNgay));
                }
                else
                {
                    if (ngayPhatDong_DenNgay != "")
                        sql.Append(string.Format("AND FORMAT(a.ngayPhatDong, 'dd/MM/yyyy') <= '{0}' AND FORMAT(a.ngayPhatDong, 'dd/MM/yyyy') >= '{1}' ", ngayPhatDong_DenNgay, ngayPhatDong_TuNgay));
                    else
                        sql.Append(string.Format("AND FORMAT(a.ngayPhatDong, 'dd/MM/yyyy') >= '{0}' ", ngayPhatDong_TuNgay));
                }
            }
            if (trangThai > 0)
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
                        thiDuaModel _newObj = new thiDuaModel
                        {
                            id = int.Parse(dt.Rows[i]["id"].ToString()),
                            stt = stt,
                            capPhatDong = dt.Rows[i]["tenPhatDong"].ToString(),
                            noiDungPhatDong = dt.Rows[i]["noiDungPhatDong"].ToString(),
                            ngayPhatDong = (DateTime.Parse(dt.Rows[i]["ngayPhatDong"].ToString())).ToString("dd/MM/yyyy"),
                            ngayKetThuc = (DateTime.Parse(dt.Rows[i]["ngayKetThuc"].ToString())).ToString("dd/MM/yyyy"),
                            soHieu = dt.Rows[i]["soHieu"].ToString(),
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
        public editThiDuaModel getThiDuaByID(int idThiDua)
        {
            var dh = _entities.qltdkt_thidua.Find(idThiDua);
            editThiDuaModel a = new editThiDuaModel();
            if (dh != null)
            {
                a.id = dh.id;
                a.capPhatDong = dh.phatDongId;
                a.kieuThiDua = dh.kieuThiDua;
                a.idDmThiDua = dh.idDmThiDua;
                a.soHieu = dh.soHieu;
                a.noiDungPhatDong = dh.noiDungPhatDong;
                a.ngayPhatDong = ((DateTime)dh.ngayPhatDong).ToString("dd/MM/yyyy");
                a.ngayKetThuc = ((DateTime)dh.ngayKetThuc).ToString("dd/MM/yyyy");
                a.quyetDinh = dh.quyetDinh;
                a.doiTuongThamGia = dh.doiTuongThamGia;
                a.nguoiKy = dh.nguoiKy;
                a.hinhThucKhenThuong = dh.hinhThucKhenThuong;
                a.trangThai = (int)dh.trangThai;



                return a;
            }
            else
            {
                return a;
            }
        }
        public List<chiTietDonViDaDangKy> getDanhSachCacDonViDaDKThiDua(int idThiDua)
        {
            List<chiTietDonViDaDangKy> result = new List<chiTietDonViDaDangKy>();

            var rs = _entities.qltdkt_dangkythidua.Where(x => x.thiDuaId == idThiDua && x.daXoa == false).ToList();
            if (rs != null)
            {
                int stt = 1;
                for (int i = 0; i < rs.Count; i++)
                {
                    string tenDonViDK = Util.getFullNameDonVi(rs[i].donViDangKyId);
                    chiTietDonViDaDangKy _new = new chiTietDonViDaDangKy
                    {
                        stt = stt,
                        tenDonViDaDangKy = tenDonViDK,
                        idDonVi = rs[i].donViDangKyId,
                        fileDinhKem = rs[i].fileDinhKem,
                        ngayDangKy = (rs[i].ngayDangKy).ToString("dd/MM/yyyy"),
                    };
                    result.Add(_new);
                    stt++;
                }
            }

            return result;
        }
        public void AddChild(List<qltdkt_dm_donvi> lsDonVi, List<int> rs, int parentId)
        {
            List<qltdkt_dm_donvi> result = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == parentId)
                {
                    result.Add(item);
                }
            }
            try
            {
                foreach (var item in result)
                {
                    rs.Add(item.id);
                    AddChild(lsDonVi, rs, item.id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<int> getDonViChildByDonViId(int idDonVi)
        {
            List<int> rs = new List<int>();
            rs.Add(idDonVi);


            List<qltdkt_dm_donvi> lsDonVi = _entities.qltdkt_dm_donvi.ToList();
            List<qltdkt_dm_donvi> lsCha = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == idDonVi)
                {
                    lsCha.Add(item);
                }
            }
            foreach (var item in lsCha)
            {
                rs.Add(item.id);
                AddChild(lsDonVi, rs, item.id);
            }
            return rs;
        }


        public dangKyThiDuaModel getChiTietDangKyThiDuaByIdThiDua2DonVi(int idThiDua, int idDonVi)
        {
            dangKyThiDuaModel result = new dangKyThiDuaModel();
            qltdkt_dangkythidua _objDangKyThiDua = _entities.qltdkt_dangkythidua.FirstOrDefault(x => x.thiDuaId == idThiDua && x.donViDangKyId == idDonVi);
            if (_objDangKyThiDua != null)
            {
                result.id = _objDangKyThiDua.id;
                result.thiDuaId = _objDangKyThiDua.thiDuaId;
                result.donViDangKyId = _objDangKyThiDua.donViDangKyId;
                result.ngayDangKy = ((DateTime)_objDangKyThiDua.ngayDangKy).ToString("dd/MM/yyyy");
                result.fileDinhKem = _objDangKyThiDua.fileDinhKem;
                result.xepHangThiDua = _objDangKyThiDua.xepHangThiDua;
                result.trangThaiThiDua = _objDangKyThiDua.trangThaiThiDua;
                result.nhanXetChung = _objDangKyThiDua.nhanXetChung;
                result.nguoiKyDanhGia = _objDangKyThiDua.nguoiKyDanhGia;
                result.noiDungDangKy = _objDangKyThiDua.noiDungDangKy;
                result.noiDungDanhGia = _objDangKyThiDua.noiDungDanhGia;
                result.listCaNhanDangKy = JsonConvert.DeserializeObject<List<chiTietDangKyThiDua>>(_objDangKyThiDua.caNhanDangKy_KetQua);
            }
            else
            {
                List<chiTietDangKyThiDua> lsCaNhanDangKy = new List<chiTietDangKyThiDua>();
                List<int> lsDonViAll = getDonViChildByDonViId(idDonVi);
                var lsNhanVien = (from item in _entities.qltdkt_dm_nhanvien where lsDonViAll.Contains(item.donViId) && item.maNhanVien != "cntt_01" && item.daXoa == false && item.trangThai == 0 select item).ToList();
                if (lsNhanVien.Count > 0)
                {
                    for (int i = 0; i < lsNhanVien.Count; i++)
                    {
                        qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);
                        chiTietDangKyThiDua _new = new chiTietDangKyThiDua
                        {
                            idNhanVien = _objNv.id,
                            tenNhanVien = _objNv.hoTen,
                            isDangKy = false,
                            nhanXet = "",
                            noiDungDangKy = "",
                            xepHang = 0,
                            tenDonVi = _entities.qltdkt_dm_donvi.Find(_objNv.donViId).tenDonVi
                        };
                        lsCaNhanDangKy.Add(_new);
                    }
                }
                result.listCaNhanDangKy = lsCaNhanDangKy;
            }
            return result;
        }

        public List<chiTietDangKyThiDua> getLsNhanVienByIdDonVi(int idDonVi)
        {
            List<chiTietDangKyThiDua> lsCaNhanDangKy = new List<chiTietDangKyThiDua>();
            List<int> lsDonViAll = getDonViChildByDonViId(idDonVi);
            var lsNhanVien = (from item in _entities.qltdkt_dm_nhanvien where lsDonViAll.Contains(item.donViId) && item.maNhanVien != "cntt_01" && item.daXoa == false && item.trangThai == 0 select item).ToList();
            if (lsNhanVien.Count > 0)
            {
                for (int i = 0; i < lsNhanVien.Count; i++)
                {
                    qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);
                    chiTietDangKyThiDua _new = new chiTietDangKyThiDua
                    {
                        idNhanVien = _objNv.id,
                        tenNhanVien = _objNv.hoTen,
                        tenDonVi = Util.getFullNameDonVi(idDonVi)
                    };
                    lsCaNhanDangKy.Add(_new);
                }
            }
            return lsCaNhanDangKy;
        }

        public List<chiTietDangKyThiDua> getListChiTietDangKyByIdThiDua2DonVi(int idThiDua, int idDonVi)
        {
            List<chiTietDangKyThiDua> result = new List<chiTietDangKyThiDua>();
            qltdkt_dangkythidua _objDangKyThiDua = _entities.qltdkt_dangkythidua.FirstOrDefault(x => x.thiDuaId == idThiDua && x.donViDangKyId == idDonVi);
            if (_objDangKyThiDua != null)
            {
                if (_objDangKyThiDua.caNhanDangKy_KetQua.Length > 0)
                {
                    result = JsonConvert.DeserializeObject<List<chiTietDangKyThiDua>>(_objDangKyThiDua.caNhanDangKy_KetQua);
                }
                else
                {

                }
            }
            List<int> lsDonViAll = getDonViChildByDonViId(idDonVi);
            var lsNhanVien = (from item in _entities.qltdkt_dm_nhanvien where lsDonViAll.Contains(item.donViId) && item.maNhanVien != "cntt_01" && item.daXoa == false && item.trangThai == 0 select item).ToList();
            if (lsNhanVien.Count > 0)
            {
                for (int i = 0; i < lsNhanVien.Count; i++)
                {
                    qltdkt_dm_nhanvien _objNv = _entities.qltdkt_dm_nhanvien.Find(lsNhanVien[i].id);
                    chiTietDangKyThiDua _new = new chiTietDangKyThiDua
                    {
                        idNhanVien = _objNv.id,
                        tenNhanVien = _objNv.hoTen,
                        isDangKy = false,
                        nhanXet = "",
                        noiDungDangKy = "",
                        xepHang = 0
                    };
                    result.Add(_new);
                }
            }
            return result;
        }


        public List<chiTietBaoCaoThanhTich> getChiTietBaoCaoTT(int idThiDua)
        {
            List<chiTietBaoCaoThanhTich> result = new List<chiTietBaoCaoThanhTich>();
            var rs = _entities.qltdkt_dangkythidua.Where(x => x.thiDuaId == idThiDua && x.daXoa == false).ToList();
            if (rs != null)
            {
                for (int i = 0; i < rs.Count; i++)
                {
                    string tenDonViDK = Util.getFullNameDonVi(rs[i].donViDangKyId);
                    string jSonChiTietCaNhan = rs[i].caNhanDangKy_KetQua;
                    List<chiTietDangKyThiDua> lsCaNhan = new List<chiTietDangKyThiDua>();
                    if (jSonChiTietCaNhan.Length > 0)
                    {
                        lsCaNhan = JsonConvert.DeserializeObject<List<chiTietDangKyThiDua>>(jSonChiTietCaNhan);
                        for (int j = 0; j < lsCaNhan.Count; j++)
                        {
                            if (!lsCaNhan[j].isDangKy)
                            {
                                lsCaNhan.Remove(lsCaNhan[j]);
                                j--;
                            }
                        }
                    }
                    chiTietBaoCaoThanhTich _new = new chiTietBaoCaoThanhTich
                    {
                        daBaoCao = rs[i].daBaoCao != null ? (bool)rs[i].daBaoCao : false,
                        idDonViDangKy = rs[i].donViDangKyId,
                        tenDonViDangKy = tenDonViDK,
                        xepHang = getXepHang((byte)rs[i].xepHangThiDua),
                        listCaNhanDangKy = lsCaNhan
                    };
                    result.Add(_new);
                }
            }
            return result;
        }

        public List<hoSoThiDuaDisplay> getDanhSachHoSoThiDua(int idHoSoThiDua)
        {
            //List<hoSoThiDuaDisplay> result = new List<hoSoThiDuaDisplay>();
            if (idHoSoThiDua > 0)
            {
                var data = from a in _entities.qltdkt_hosothidua
                           join b in _entities.qltdkt_thidua on a.thiDuaId equals b.id
                           join c in _entities.qltdkt_dm_thidua on b.idDmThiDua equals c.id
                           where a.daXoa == false && a.id == idHoSoThiDua
                           select new hoSoThiDuaDisplay
                           {
                               idHoSoThiDua = a.id,
                               tenHoSoThiDua = a.tenHoSoThiDua,
                               tenThiDua = a.tenThiDua,
                               soHieu = b.soHieu,
                               kieuThiDua = b.kieuThiDua,
                               fileBaoCaoThanhTich = a.fileBaoCaoThanhTich,
                               vanBanHuongDan = a.fileDinhKem,
                               trangThaiTD = b.trangThai
                           };

                return data.ToList();
            }
            else
            {
                var data = from a in _entities.qltdkt_hosothidua
                           join b in _entities.qltdkt_thidua on a.thiDuaId equals b.id
                           join c in _entities.qltdkt_dm_thidua on b.idDmThiDua equals c.id
                           where a.daXoa == false
                           select new hoSoThiDuaDisplay
                           {
                               idHoSoThiDua = a.id,
                               tenHoSoThiDua = a.tenHoSoThiDua,
                               tenThiDua = a.tenThiDua,
                               soHieu = b.soHieu,
                               kieuThiDua = b.kieuThiDua,
                               fileBaoCaoThanhTich = a.fileBaoCaoThanhTich,
                               vanBanHuongDan = a.fileDinhKem,
                               trangThaiTD = b.trangThai
                           };

                return data.ToList();
            }
        }

        public string getTenByIdNhanVienTT(int id, int type)
        {
            if (type == 1)
            {
                return Util.getFullNameDonVi(id);
            }
            if (type == 2)
            {
                return _entities.qltdkt_dm_nhanvien.Find(id).hoTen;
            }
            return "";
        }

        private string getXepHang(int index)
        {
            if (index == 0)
            {
                return "Chưa xếp hạng";
            }
            if (index == 1)
            {
                return "Giỏi";
            }
            if (index == 2)
            {
                return "Khá";
            }
            if (index == 3)
            {
                return "Trung bình";
            }
            if (index == 4)
            {
                return "Yếu";
            }
            if (index == 5)
            {
                return "Kém";
            }
            return "";
        }
    }
}