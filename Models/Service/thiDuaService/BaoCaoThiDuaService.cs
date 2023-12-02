using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.thiDuaService
{
    public class BaoCaoThiDuaService
    {
        quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        thiDuaService _thiDuaService = new thiDuaService();
        public chiTietThiDua getChiTietThiDua(int idThiDua)
        {
            chiTietThiDua result = new chiTietThiDua();
            qltdkt_dangkythidua _dkThiDua = _entities.qltdkt_dangkythidua.FirstOrDefault(x => x.thiDuaId == idThiDua);
            if (_dkThiDua != null)
            {
                result.isDangKy = true;
                qltdkt_thidua _thiDua = _entities.qltdkt_thidua.Find(idThiDua);
                if (_thiDua != null)
                {
                    result.tenThiDua = _entities.qltdkt_dm_thidua.Find(_thiDua.idDmThiDua).tenThiDua;
                    result.soHieu = _thiDua.soHieu;
                    result.kieuThiDua = Util.getKieuThiDua(_thiDua.kieuThiDua);
                    result.ngayPhatDong = _thiDua.ngayPhatDong.ToString();
                    result.dsDangKy = _thiDuaService.getChiTietBaoCaoTT(idThiDua);
                }
            }
            else
            {
                result.isDangKy = false;
            }
            return result;
        }

        public chiTietThiDuaBaoCao getChiTietThiDuaBaoCao(int idThiDua)
        {
            chiTietThiDuaBaoCao result = new chiTietThiDuaBaoCao();
            List<qltdkt_baocaothidua> lsBaoCaoThiDua = _entities.qltdkt_baocaothidua.Where(x => x.idThiDua == idThiDua).ToList();
            if (lsBaoCaoThiDua != null && lsBaoCaoThiDua.Count > 0)
            {
                result.isBaoCao = true;
                qltdkt_thidua _thiDua = _entities.qltdkt_thidua.Find(idThiDua);
                if (_thiDua != null)
                {
                    result.tenThiDua = _entities.qltdkt_dm_thidua.Find(_thiDua.idDmThiDua).tenThiDua;
                    result.soHieu = _thiDua.soHieu;
                    result.kieuThiDua = Util.getKieuThiDua(_thiDua.kieuThiDua);
                    result.idKieuThiDua = _thiDua.kieuThiDua;
                    result.ngayPhatDong = _thiDua.ngayPhatDong.ToString();
                    List<lsDonViCaNhan> _lsDonViCaNhan = new List<lsDonViCaNhan>();
                    List<KeyValuePair<string, string>> lsFileBaoCao = new List<KeyValuePair<string, string>>();
                    for (int i = 0; i < lsBaoCaoThiDua.Count; i++)
                    {
                        string str_dsCNTTBaoCao = lsBaoCaoThiDua[i].dsCaNhanTTBaoCao;
                        string fileBaoCaoTT = lsBaoCaoThiDua[i].fileDinhKem;

                        if (fileBaoCaoTT.Length > 0)
                        {
                            var file = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(fileBaoCaoTT);

                            lsFileBaoCao.Add(file[0]);
                        }
                        ketQuaThiDuaDK kq = new ketQuaThiDuaDK();
                        if (str_dsCNTTBaoCao.Length > 0)
                        {
                            kq = JsonConvert.DeserializeObject<ketQuaThiDuaDK>(str_dsCNTTBaoCao);
                        }
                        string lsCN = kq.lsCN;
                        string lsTT = kq.lsTT;
                        if (lsCN.Length > 0)
                        {
                            string[] spl_CN = lsCN.Split(';');
                            if (spl_CN.Length > 0)
                            {
                                for (int j = 0; j < spl_CN.Length; j++)
                                {
                                    if (spl_CN[j] != "")
                                    {
                                        lsDonViCaNhan _new = new lsDonViCaNhan
                                        {
                                            id = int.Parse(spl_CN[j]),
                                            name = getTenByIdNhanVien(int.Parse(spl_CN[j])),
                                            type = 2
                                        };
                                        _lsDonViCaNhan.Add(_new);
                                    }
                                }
                            }
                        }
                        if (lsTT.Length > 0)
                        {
                            string[] spl_TT = lsTT.Split(';');
                            if (spl_TT.Length > 0)
                            {
                                for (int j = 0; j < spl_TT.Length; j++)
                                {
                                    if (spl_TT[j] != "")
                                    {
                                        lsDonViCaNhan _new = new lsDonViCaNhan
                                        {
                                            id = int.Parse(spl_TT[j]),
                                            name = Util.getFullNameDonVi(int.Parse(spl_TT[j])),
                                            type = 1
                                        };
                                        _lsDonViCaNhan.Add(_new);
                                    }
                                }
                            }
                        }
                    }
                    result.dsDonViCaNhan = _lsDonViCaNhan;
                    result.lsFileBaoCao = JsonConvert.SerializeObject(lsFileBaoCao);
                }
            }
            else
            {
                result.isBaoCao = false;
            }
            return result;
        }

        public List<baoCaoThiDuaDisplay> getBaoCaoThiDua(int idThiDua)
        {
            //List<baoCaoThiDuaDisplay> result = new List<baoCaoThiDuaDisplay>();
            if (idThiDua > 0)
            {
                var data = from a in _entities.qltdkt_baocaothidua
                           join b in _entities.qltdkt_thidua on a.idThiDua equals b.id
                           join c in _entities.qltdkt_dm_thidua on b.idDmThiDua equals c.id
                           where a.daXoa == false && a.idThiDua == idThiDua
                           select new baoCaoThiDuaDisplay
                           {
                               idBaoCaoThiDua = a.id,
                               tenBaoCao = a.tenBaoCao,
                               soHieu = b.soHieu,
                               tenThiDua = c.tenThiDua,
                               fileBaoCao = a.fileDinhKem,
                               trangThaiTD = b.trangThai
                           };

                return data.ToList();
            }
            else
            {
                var data = from a in _entities.qltdkt_baocaothidua
                           join b in _entities.qltdkt_thidua on a.idThiDua equals b.id
                           join c in _entities.qltdkt_dm_thidua on b.idDmThiDua equals c.id
                           where a.daXoa == false
                           select new baoCaoThiDuaDisplay
                           {
                               idBaoCaoThiDua = a.id,
                               tenBaoCao = a.tenBaoCao,
                               soHieu = b.soHieu,
                               tenThiDua = c.tenThiDua,
                               fileBaoCao = a.fileDinhKem,
                               trangThaiTD = b.trangThai

                           };

                return data.ToList();
            }
        }
        private string getTenByIdNhanVien(int idNhanVien)
        {
            return _entities.qltdkt_dm_nhanvien.Find(idNhanVien).hoTen;
        }

    }
}