using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.userService
{
    public class userService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        SqlDataAccess _sqlAccess = new SqlDataAccess();
        public List<userDisplay> getUserData()
        {
            var data = from a in _entities.qltdkt_user
                       join b in _entities.qltdkt_userbygroup on a.id equals b.userId
                       join c in _entities.qltdkt_groupuser on b.groupUserId equals c.id
                       where a.daXoa == false
                       select new userDisplay
                       {
                           id = a.id,
                           passWord = a.matKhau,
                           tenDangNhap = a.tenDangNhap,
                           nhomNguoiDung = c.tenNhom,
                           idNhanVien = (int)(a.nhanVienId != null ? a.nhanVienId : 0)
                       };
            return data.ToList();
        }

        public userFull getUserInfo(int userId)
        {
            userFull result = new userFull();
            qltdkt_user _us = _entities.qltdkt_user.Find(userId);
            if (_us != null)
            {
                result.tenDangNhap = _us.tenDangNhap;
                result.matKhau = _us.matKhau;
                qltdkt_userbygroup _ugroup = _entities.qltdkt_userbygroup.FirstOrDefault(x => x.userId == userId);
                if (_ugroup != null)
                {
                    result.groupUserId = _ugroup.groupUserId;
                }
            }
            return result;
        }
    }
}