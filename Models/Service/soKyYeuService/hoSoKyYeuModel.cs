using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.soKyYeuService
{
    public class hoSoKyYeuModel
    {
        public int id { get; set; }
        public int idDonVi { get; set; }
        //public int idCaNhan { get; set; }
        public string tenDonVi { get; set; }
        //public string tenCaNhan { get; set; }
        public List<hoSoKyYeuModel> dsDonViCon { get; set; }
        public List<Canhan> dsCaNhan { get; set; }
    }
    public class Canhan
    {
        public int idCaNhan { get; set; }
        public string tenCaNhan { get; set; }
        public string chucVu { get; set; }
    }
}