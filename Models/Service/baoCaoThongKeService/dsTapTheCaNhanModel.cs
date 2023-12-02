using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.baoCaoThongKeService
{
    public class dsTapTheCaNhanModel
    {
        public int stt { get; set; }
        public int idTapThe { get; set; }
        public string tenTapThe { get; set; }
        public List<dsTapTheCaNhanModel> dsDonViCap2 { get; set; }
        public List<CaNhan> dsCaNhan { get; set; }
    }

    public class CaNhan
    {
        public int stt { get; set; }
        public int idCaNhan { get; set; }
        public string tenCaNhan { get; set; }
    }
}