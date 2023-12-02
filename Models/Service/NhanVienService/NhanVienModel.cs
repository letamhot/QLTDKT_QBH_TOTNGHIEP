using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.NhanVienService
{
    public class NhanVienModel
    {
        public class danhSachTrinhDoHocVan
        {

            public int id { get; set; }

            public string tenTrinhDoHocVan { get; set; }
            public string chuyenNganh { get; set; }
        }
    }
}