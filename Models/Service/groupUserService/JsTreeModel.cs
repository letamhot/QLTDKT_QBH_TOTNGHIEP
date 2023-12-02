using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.groupUserService
{
    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public States state { get; set; }
        public string li_attr { get; set; }
        public string a_attr { get; set; }
    }
    public class States
    {
        public States()
        { }
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
        public bool hidden { get; set; }
    }
}