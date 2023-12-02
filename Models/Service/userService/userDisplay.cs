using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.userService
{
    public class userDisplay
    {
        public int id { get; set; }

        public int idNhanVien { get; set; }
        public string tenDangNhap { get; set; }
        public string passWord { get; set; }
        public string nhomNguoiDung { get; set; }
    }

    public class userFull : qltdkt_user
    {
        public int groupUserId { get; set; }
    }
}