using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class dschitietcanhantapthekhenthuong
    {
        public int id { get; set; }
        public bool ischeck { get; set; }
        public string tenCaNhanTapThe { get; set; }
        public string chucDanh { get; set; }
        public string donVi { get; set; }
        public Nullable<byte> loaiKT { get; set; }
        public string tienThuong { get; set; }
    }
}