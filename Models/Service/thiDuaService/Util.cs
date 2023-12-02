using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace QLTDKT.Models.Service.thiDuaService
{
    public static class Util
    {
        static quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public static string getFullNameDonVi(int idDonVi)
        {
            string result = "";
            int idParent = _entities.qltdkt_dm_donvi.Find(idDonVi).idCha;
            result = _entities.qltdkt_dm_donvi.Find(idDonVi).tenDonVi + "/" + _entities.qltdkt_dm_donvi.Find(idParent).tenDonVi;

            return result;
        }

        public static string getXepHang(int index)
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

        public static string getKieuThiDua(int kieu)
        {
            if (kieu == 0)
            {
                return "Năm";
            }
            if (kieu == 1)
            {
                return "Giai đoạn";
            }
            if (kieu == 2)
            {
                return "Chuyên đề";
            }
            return "";
        }
    }
}