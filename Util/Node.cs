using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Util
{
    public class Node
    {
        public int id { get; set; }
        public string text { get; set; }
        public string icon { get; set; }
        public List<Node> children { get; set; }
    }
}