﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class dsChiTietCaNhanTapThe
    {
        public int id { get; set; }
        public bool ischeck { get; set; }
        public int loaiDanhHieu { get; set; }
        public string tenCaNhanTapThe { get; set; }
        public string chucDanh { get; set; }
        public string donVi { get; set; }
    }
}