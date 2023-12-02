using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class dstapthecanhankt
    {
        public int loaiKhenThuong { get; set; }
        public int idKhenThuong { get; set; }
        public int tongtien { get; set; }
        public string tenKhenThuong { get; set; }
        public List<dschitietcanhantapthekhenthuong> dschitietcanhantapthekhenthuong { get; set; }
    }
}