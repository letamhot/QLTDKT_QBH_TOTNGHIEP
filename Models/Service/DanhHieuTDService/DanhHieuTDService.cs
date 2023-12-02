using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Data;
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace QLTDKT.Models.Service.danhHieuTDService
{
    public class DanhHieuTDService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();

        public List<DanhHieuTDModel> getDanhHieuThiDua(int idDanhHieu, int bophan)
        {
            //List<DanhHieuTDModel> result = new List<DanhHieuTDModel>();
            //var dhtd = _entities.qltdkt_dm_danhhieuthidua.FirstOrDefault(x => x.daXoa == false);
            List<DanhHieuTDModel> dataDH = null;
            if (bophan != 0)
            {
                if (idDanhHieu > 0)
                {
                    var data = from a in _entities.qltdkt_dm_danhhieuthidua
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false && a.id == idDanhHieu && a.bophan == bophan
                               orderby a.loaiDanhHieu ascending
                               select new DanhHieuTDModel
                               {
                                   idDanhHieu = a.id,
                                   loaiDanhHieu = a.loaiDanhHieu,
                                   tenDanhHieuThiDua = a.tenDanhHieuThiDua,
                                   bophan = a.bophan,

                                   luuSoKyYeu = a.luuSoKyYeu,
                                   moTa = a.moTa,
                                   idThanhTich = a.idThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyDH = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };

                    dataDH = data.ToList();
                }
                else
                {
                    var data = from a in _entities.qltdkt_dm_danhhieuthidua
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false && a.bophan == bophan
                               orderby a.loaiDanhHieu ascending

                               select new DanhHieuTDModel
                               {
                                   idDanhHieu = a.id,
                                   loaiDanhHieu = a.loaiDanhHieu,
                                   tenDanhHieuThiDua = a.tenDanhHieuThiDua,
                                   luuSoKyYeu = a.luuSoKyYeu,
                                   bophan = a.bophan,

                                   moTa = a.moTa,
                                   idThanhTich = a.idThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyDH = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };


                    dataDH = data.ToList();
                }
                return dataDH;

            }
            else
            {
                if (idDanhHieu > 0)
                {
                    var data = from a in _entities.qltdkt_dm_danhhieuthidua
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false && a.id == idDanhHieu
                               orderby a.loaiDanhHieu ascending
                               select new DanhHieuTDModel
                               {
                                   idDanhHieu = a.id,
                                   loaiDanhHieu = a.loaiDanhHieu,
                                   tenDanhHieuThiDua = a.tenDanhHieuThiDua,
                                   bophan = a.bophan,

                                   luuSoKyYeu = a.luuSoKyYeu,
                                   moTa = a.moTa,
                                   idThanhTich = a.idThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyDH = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };

                    dataDH = data.ToList();
                }
                else
                {
                    var data = from a in _entities.qltdkt_dm_danhhieuthidua
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false
                               orderby a.loaiDanhHieu ascending

                               select new DanhHieuTDModel
                               {
                                   idDanhHieu = a.id,
                                   loaiDanhHieu = a.loaiDanhHieu,
                                   tenDanhHieuThiDua = a.tenDanhHieuThiDua,
                                   luuSoKyYeu = a.luuSoKyYeu,
                                   bophan = a.bophan,

                                   moTa = a.moTa,
                                   idThanhTich = a.idThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyDH = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };


                    dataDH = data.ToList();
                }
                return dataDH;
            }

        }



    }
}