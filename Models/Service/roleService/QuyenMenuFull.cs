using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.roleService
{
    public class QuyenMenuFull
    {
        public int id { get; set; }
        public string roleName { get; set; }
        public int roleParent { get; set; }
        public string Class { get; set; }
        public string Styles { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public List<QuyenMenuFull> Children { get; set; }
    }
}