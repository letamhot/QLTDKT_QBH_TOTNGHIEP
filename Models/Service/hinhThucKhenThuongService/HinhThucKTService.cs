using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.hinhThucKhenThuongService
{
    public class HinhThucKTService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();

        public List<HinhThucKTModel> getHinhThucKhenThuong(int idHT, int bophan)
        {
            List<HinhThucKTModel> dataHT = null;

            //List<DanhHieuTDModel> result = new List<DanhHieuTDModel>();
            if (bophan != 0)
            {
                if (idHT > 0)
                {
                    var data = from a in _entities.qltdkt_dm_hinhthuckhenthuong
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false && a.id == idHT && a.bophan == bophan
                               orderby a.loaiKhenThuong ascending
                               select new HinhThucKTModel
                               {
                                   idHT = a.id,
                                   bophan = a.bophan,
                                   tenHinhThucKhenThuong = a.tenHinhThucKhenThuong,
                                   loaiKhenThuong = a.loaiKhenThuong,
                                   moTa = a.moTa,
                                   maThanhTich = a.maThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyKT = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };

                    dataHT = data.ToList();
                }
                else
                {
                    var data = from a in _entities.qltdkt_dm_hinhthuckhenthuong
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false && a.bophan == bophan
                               orderby a.loaiKhenThuong ascending

                               select new HinhThucKTModel
                               {
                                   idHT = a.id,
                                   bophan = a.bophan,
                                   tenHinhThucKhenThuong = a.tenHinhThucKhenThuong,
                                   loaiKhenThuong = a.loaiKhenThuong,
                                   moTa = a.moTa,
                                   maThanhTich = a.maThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyKT = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };


                    dataHT = data.ToList();
                }
                return dataHT;

            }
            else
            {
                if (idHT > 0)
                {
                    var data = from a in _entities.qltdkt_dm_hinhthuckhenthuong
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false && a.id == idHT
                               orderby a.loaiKhenThuong ascending
                               select new HinhThucKTModel
                               {
                                   idHT = a.id,
                                   bophan = a.bophan,
                                   tenHinhThucKhenThuong = a.tenHinhThucKhenThuong,
                                   loaiKhenThuong = a.loaiKhenThuong,
                                   moTa = a.moTa,
                                   maThanhTich = a.maThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyKT = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };

                    dataHT = data.ToList();
                }
                else
                {
                    var data = from a in _entities.qltdkt_dm_hinhthuckhenthuong
                               join b in _entities.qltdkt_dm_chuky on a.chuKy equals b.id
                               join c in _entities.qltdkt_dm_capkykhenthuong on a.capThanhTich equals c.id
                               where a.daXoa == false
                               orderby a.loaiKhenThuong ascending

                               select new HinhThucKTModel
                               {
                                   idHT = a.id,
                                   bophan = a.bophan,
                                   tenHinhThucKhenThuong = a.tenHinhThucKhenThuong,
                                   loaiKhenThuong = a.loaiKhenThuong,
                                   moTa = a.moTa,
                                   maThanhTich = a.maThanhTich,
                                   chuKy = b.id,
                                   capThanhTich = c.id,
                                   chuKyKT = b.tenChuKy,
                                   capKyThanhTich = c.tenCapKyKhenThuong
                               };


                    dataHT = data.ToList();
                }
                return dataHT;
            }


        }
    }
}