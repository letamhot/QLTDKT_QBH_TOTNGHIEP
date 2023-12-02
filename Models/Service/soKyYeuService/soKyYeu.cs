using Newtonsoft.Json;
using QLTDKT.Models.Service.danhHieuService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.soKyYeuService
{
    public class soKyYeuService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private thiDuaService.thiDuaService _thiDua = new thiDuaService.thiDuaService();
        private danhHieuService.danhHieuService _danhHieuService = new danhHieuService.danhHieuService();

        SqlDataAccess _sqlAccess = new SqlDataAccess();
        public bool luuHoSoKyYeu(int idThiDua)
        {
            var rs = _thiDua.getChiTietBaoCaoTT(idThiDua);
            try
            {
                if (rs != null && rs.Count > 0)
                {
                    for (int i = 0; i < rs.Count; i++)
                    {
                        int id_donViDK = rs[i].idDonViDangKy;
                        qltdkt_dm_donvi _infoDonVi = _entities.qltdkt_dm_donvi.Find(id_donViDK);
                        List<soKyYeuModel> _lsThanhTich = new List<soKyYeuModel>();
                        qltdkt_hosokyyeu _hs = null;
                        try
                        {
                            _hs = _entities.qltdkt_hosokyyeu.FirstOrDefault(x => x.idDonVi == id_donViDK && x.idNhanVien == null);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }

                        if (_hs != null)
                        {
                            _lsThanhTich = JsonConvert.DeserializeObject<List<soKyYeuModel>>(_hs.hoSoKyYeu);
                            if (_lsThanhTich.Count > 0)
                            {
                                bool isCheck = false;
                                for (int j = 0; j < _lsThanhTich.Count; j++)
                                {
                                    if (_lsThanhTich[j].idThanhTich == idThiDua && _lsThanhTich[j].kieuKyYeu == 1)
                                    {
                                        _lsThanhTich[j].hinhAnhToChuc = "";
                                        _lsThanhTich[j].hinhThucTraoTang = "";
                                        _lsThanhTich[j].ngayKyYeu = DateTime.Now;
                                        _lsThanhTich[j].thanhTichDatDuoc = 1;
                                        _lsThanhTich[j].tienThuong = 100000;
                                        isCheck = true;
                                        break;
                                    }
                                }
                                if (!isCheck)
                                {
                                    soKyYeuModel _new = new soKyYeuModel
                                    {
                                        idThanhTich = idThiDua,
                                        kieuKyYeu = 1,
                                        hinhAnhToChuc = "",
                                        hinhThucTraoTang = "",
                                        thanhTichDatDuoc = 1,
                                        ngayKyYeu = DateTime.Now,
                                        tienThuong = 10000
                                    };
                                    _lsThanhTich.Add(_new);
                                }
                            }
                            _hs.anhDaiDien = "";
                            _hs.soQDThanhLap = "";
                            _hs.hoSoKyYeu = JsonConvert.SerializeObject(_lsThanhTich);
                            _hs.chucNangNhiemVu = "";
                        }
                        else
                        {
                            soKyYeuModel _new = new soKyYeuModel
                            {
                                idThanhTich = idThiDua,
                                kieuKyYeu = 1,
                                hinhAnhToChuc = "",
                                hinhThucTraoTang = "",
                                thanhTichDatDuoc = 1,
                                ngayKyYeu = DateTime.Now,
                                tienThuong = 10000
                            };
                            _lsThanhTich.Add(_new);
                            qltdkt_hosokyyeu _newDonVi = new qltdkt_hosokyyeu
                            {
                                idDonVi = id_donViDK,
                                anhDaiDien = "",
                                soQDThanhLap = "",
                                chucNangNhiemVu = "",
                                ngayTao = DateTime.Now,
                                hoSoKyYeu = JsonConvert.SerializeObject(_lsThanhTich)
                            };
                            _entities.qltdkt_hosokyyeu.Add(_newDonVi);
                        }


                        if (rs[i].listCaNhanDangKy.Count > 0)
                        {
                            for (int j = 0; j < rs[i].listCaNhanDangKy.Count; j++)
                            {
                                int id_nhanvienDK = rs[i].listCaNhanDangKy[j].idNhanVien;
                                List<soKyYeuModel> _lsThanhTichCaNhan = new List<soKyYeuModel>();
                                int id_donViCp = getIdDonViCaNhan(id_nhanvienDK);
                                qltdkt_hosokyyeu _hsNhanVien = null;
                                try
                                {
                                    _hsNhanVien = _entities.qltdkt_hosokyyeu.FirstOrDefault(x => x.idDonVi == id_donViCp && x.idNhanVien == id_nhanvienDK);
                                }
                                catch (Exception)
                                {

                                }
                                if (_hsNhanVien != null)
                                {
                                    _lsThanhTichCaNhan = JsonConvert.DeserializeObject<List<soKyYeuModel>>(_hsNhanVien.hoSoKyYeu);
                                    if (_lsThanhTichCaNhan.Count > 0)
                                    {
                                        bool isCheck = false;
                                        for (int k = 0; k < _lsThanhTichCaNhan.Count; k++)
                                        {
                                            if (_lsThanhTichCaNhan[k].idThanhTich == idThiDua && _lsThanhTichCaNhan[k].kieuKyYeu == 1)
                                            {
                                                _lsThanhTichCaNhan[k].hinhAnhToChuc = "";
                                                _lsThanhTichCaNhan[k].hinhThucTraoTang = "";
                                                _lsThanhTichCaNhan[k].ngayKyYeu = DateTime.Now;
                                                _lsThanhTichCaNhan[k].thanhTichDatDuoc = 1;
                                                _lsThanhTichCaNhan[k].tienThuong = 100000;
                                                isCheck = true;
                                                break;
                                            }
                                        }
                                        if (!isCheck)
                                        {
                                            soKyYeuModel _new = new soKyYeuModel
                                            {
                                                idThanhTich = idThiDua,
                                                kieuKyYeu = 1,
                                                hinhAnhToChuc = "",
                                                hinhThucTraoTang = "",
                                                thanhTichDatDuoc = 1,
                                                ngayKyYeu = DateTime.Now,
                                                tienThuong = 10000
                                            };
                                            _lsThanhTichCaNhan.Add(_new);
                                        }
                                    }
                                    _hsNhanVien.anhDaiDien = "";
                                    _hsNhanVien.chucNangNhiemVu = "";
                                    _hsNhanVien.hoSoKyYeu = JsonConvert.SerializeObject(_lsThanhTichCaNhan);
                                    _hsNhanVien.soQDThanhLap = "";
                                }
                                else
                                {
                                    soKyYeuModel _new = new soKyYeuModel
                                    {
                                        idThanhTich = idThiDua,
                                        kieuKyYeu = 1,
                                        hinhAnhToChuc = "",
                                        hinhThucTraoTang = "",
                                        thanhTichDatDuoc = 1,
                                        ngayKyYeu = DateTime.Now,
                                        tienThuong = 10000
                                    };
                                    _lsThanhTichCaNhan.Add(_new);
                                    qltdkt_hosokyyeu _newCaNhan = new qltdkt_hosokyyeu
                                    {
                                        idDonVi = getIdDonViCaNhan(id_nhanvienDK),
                                        idNhanVien = id_nhanvienDK,
                                        anhDaiDien = "",
                                        soQDThanhLap = "",
                                        chucNangNhiemVu = "",
                                        ngayTao = DateTime.Now,
                                        hoSoKyYeu = JsonConvert.SerializeObject(_lsThanhTichCaNhan)
                                    };
                                    _entities.qltdkt_hosokyyeu.Add(_newCaNhan);
                                }
                            }
                        }
                    }
                }
                _entities.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public int Reload()
        {
            var dsTenDanhHieuLuuSKY = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.luuSoKyYeu == true && x.daXoa == false).ToList();
            var dsTenDanhHieuNONLuuSKY = _entities.qltdkt_dm_danhhieuthidua.Where(x => x.luuSoKyYeu == false && x.daXoa == false).ToList();
            List<qltdkt_danhhieu> dsNew = new List<qltdkt_danhhieu>();
            List<qltdkt_danhhieu> dsDel = new List<qltdkt_danhhieu>();


            foreach (var item in dsTenDanhHieuLuuSKY)
            {
                var ds1 = _entities.qltdkt_danhhieu.Where(x => x.idTenDanhHieu == item.id).ToList();
                dsNew.AddRange(ds1);
            }
            foreach (var item in dsTenDanhHieuNONLuuSKY)
            {
                var ds1 = _entities.qltdkt_danhhieu.Where(x => x.idTenDanhHieu == item.id).ToList();
                dsDel.AddRange(ds1);
            }


            try
            {
                foreach (var item in dsNew)
                {
                    var x = _danhHieuService.LuuSoKyYeu(item.id, item.hinhAnhTraoTang);
                }
                foreach (var item in dsDel)
                {
                    var x = _danhHieuService.XoaDuLieuTrongSoKyYeu(item.id);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return 0;
            }

        }

        bool checkDonVi(int idDonVi)
        {

            return true;
        }

        int getLastNodeDonViId(int idDonVi)
        {
            return 0;
        }

        int getIdDonViCaNhan(int idNhanVien)
        {
            return _entities.qltdkt_dm_nhanvien.Find(idNhanVien).donViId;
        }

        public List<hoSoKyYeuModel> getSoKyYeu()
        {
            var result = (from a in _entities.qltdkt_dm_donvi
                          where a.idCha == 1
                          select new hoSoKyYeuModel
                          {
                              idDonVi = a.id,
                              tenDonVi = a.tenDonVi
                          }).ToList();
            int i = 1;
            foreach (var item in result)
            {
                item.id = i;
                i++;

                var listcon = (from a in _entities.qltdkt_dm_donvi
                               where a.idCha == item.idDonVi
                               select new hoSoKyYeuModel
                               {
                                   idDonVi = a.id,
                                   tenDonVi = a.tenDonVi
                               }).ToList();
                item.dsDonViCon = listcon;

                var listcncha = (from a in _entities.qltdkt_dm_nhanvien
                                 where a.donViId == item.idDonVi && a.daXoa == false && a.maNhanVien != "cntt_01" && a.trangThai == 0
                                 select new Canhan
                                 {
                                     idCaNhan = a.id,
                                     tenCaNhan = a.hoTen,
                                     //chucVu = getTenChucVu(a.chucVuId)
                                 }).ToList();
                item.dsCaNhan = listcncha;

                foreach (var item2 in listcon)
                {
                    var listcncon = (from a in _entities.qltdkt_dm_nhanvien
                                     where a.donViId == item2.idDonVi && a.daXoa == false && a.maNhanVien != "cntt_01" && a.trangThai == 0
                                     select new Canhan
                                     {
                                         idCaNhan = a.id,
                                         tenCaNhan = a.hoTen,
                                         //chucVu = getTenChucVu(a.chucVuId)
                                     }).ToList();
                    item2.dsCaNhan = listcncon;
                }
            }

            return result;

            //code cũ
            //var hs = from a in _entities.qltdkt_hosokyyeu
            //         join b in _entities.qltdkt_dm_donvi on a.idDonVi equals b.id
            //         join c in _entities.qltdkt_dm_nhanvien on a.idNhanVien equals c.id
            //         select new
            //         {
            //             id = a.id,
            //             idDonVi = a.idDonVi,
            //             tenDonVi = b.tenDonVi,
            //             idCaNhan = (int)a.idNhanVien,
            //             tenCaNhan = c.hoTen
            //         };
            //var results = (from x in hs
            //               group x by x.idDonVi into g
            //               select new
            //               {
            //                   idDonVi = g.Key,
            //                   tenDonVi = g.Select(x => x.tenDonVi).FirstOrDefault(),
            //                   dsCaNhan = g.Select(x => x.idCaNhan)
            //               }).ToList();

            //List<hoSoKyYeuModel> y = new List<hoSoKyYeuModel>();
            //foreach (var item in results)
            //{
            //    hoSoKyYeuModel z = new hoSoKyYeuModel();
            //    z.idDonVi = item.idDonVi;
            //    z.tenDonVi = item.tenDonVi;
            //    List<Canhan> listCaNhan = new List<Canhan>();
            //    foreach (var itemcanhan in item.dsCaNhan)
            //    {
            //        Canhan cn = new Canhan();
            //        cn.idCaNhan = itemcanhan;
            //        cn.tenCaNhan = _entities.qltdkt_dm_nhanvien.Find(itemcanhan).hoTen;
            //        listCaNhan.Add(cn);
            //    }
            //    z.dsCaNhan = listCaNhan;
            //    y.Add(z);
            //}
            //return y;
        }

        private string getTenChucVu(int id)
        {
            return _entities.qltdkt_dm_chucvu.Find(id).tenChucVu;
        }

        public class avatar
        {
            public Dictionary<string, string> keyValuePairs { get; set; }
        }

        private string getanhdaidien(string url)
        {
            var cv = JsonConvert.DeserializeObject<List<KeyValuePair<string, string>>>(url);
            string[] spl = cv[0].Key.ToString().Split('\\');
            var sr = "/" + spl[spl.Length - 3] + "/" + spl[spl.Length - 2] + "/" + spl[spl.Length - 1];
            return sr;
        }

        public string ChiTietKyYeu(string id)
        {
            string[] spl = id.Split('_');
            string html = "";
            if (spl[0] == "tt")
            {
                int idtt = int.Parse(spl[1]);
                string urlavatar = string.IsNullOrEmpty(_entities.qltdkt_dm_donvi.Find(idtt).anh) ? "/Uploads/avatartapthe.jpg" : getanhdaidien(_entities.qltdkt_dm_donvi.Find(idtt).anh);
                html += "<div class='row' style='text-align: center;'><h1>" + _entities.qltdkt_dm_donvi.Find(idtt).tenDonVi.ToUpper() + "</h1></div>";
                html += "<div class='row'><img src='" + urlavatar + "' style='width:100%; height:100%;' /></div><br/>";
                html += "<div class='row'><div class='col-md-4'><label style='font-size:medium;'>Số quyết định thành lập: </label></div><div class='col-md-8'><label style='font-size:medium;'></label></div></div>";
                html += "<div class='row'><div class='col-md-4'><label style='font-size:medium;'>Chức năng/nhiệm vụ quyền hạn: </label></div><div class='col-md-8'><label style='font-size:medium;'>Mô tả chức năng nhiệm vụ quyền hạn của đơn vị</label></div></div>";
                html += "<div class='row'><div class='col-xl-12'><h3>THÀNH TÍCH ĐẠT ĐƯỢC</h3></div>";
                var hs = _entities.qltdkt_hosokyyeu.Where(x => x.idDonVi == idtt && x.idNhanVien == null).FirstOrDefault();
                if (hs != null)
                {
                    List<soKyYeuModel> listAll = new List<soKyYeuModel>();
                    var chitiet = JsonConvert.DeserializeObject<List<soKyYeuModel>>(hs.hoSoKyYeu);
                    listAll.AddRange(chitiet);

                    var listdh = listAll.Where(x => x.kieuKyYeu == 2).ToList();
                    var listkt = listAll.Where(x => x.kieuKyYeu == 3).ToList();

                    if (listdh.Count() > 0)
                    {
                        html += "<div class='col-xl-12'><h4>DANH HIỆU:</h4></div>";
                        html += "<div class='col-xl-12'><table width='100%'><thead><tr><th width='5%'>STT</th><th width='20%'>Người ký danh hiệu</th><th width='25%'>Tên danh hiệu</th><th width='10%'>Năm Danh Hiệu</th><th width='20%'>Hình thức trao tặng</th><th width='15%'>Ngày trao tặng</th><th width='5%'></th></tr></thead><tbody>";
                        int sttdh = 1;
                        foreach (var item in listdh)
                        {
                            html += "<tr><td width='5%'>" + sttdh + "</td><td width='20%'>" + item.capKhenThuong + "</td><td width='25%'>" + item.tenDanhHieu + "</td><td width='10%'>" + item.namDanhHieu + "</td><td width='20%'>" + (item.hinhThucTraoTang == "0" ? "Chưa có hình thức trao tặng danh hiệu/ trao tặng trực tiếp" : item.hinhThucTraoTang) + " " + "</td><td width='15%'>" + item.ngayTraoTang.ToString("dd/MM/yyyy") + "</td><td width='5%'><a class='fa fa-images' style='color: blue' href='#' data-toggle='tooltip' title='Xem ảnh/video' onclick='return xemAnh(" + item.hinhAnhTraoTang + ")'></a></td></tr>";
                            sttdh++;
                        }
                        html += "</tbody></table></div><br />";
                    }
                    if (listkt.Count() > 0)
                    {
                        html += "<div class='col-xl-12'><h4>KHEN THƯỞNG:</h4></div>";
                        html += "<div class='col-xl-12'><table width='100%'><thead><tr><th width='5%'>STT</th><th width='15%'>Cấp khen thưởng</th><th width='35%'>Tên khen thưởng</th><th width='15%'>Ngày</th><th width='15%'>Tiền thưởng</th><th width='15%'>Người ký khen thưởng</th></tr></thead><tbody>";
                        int sttdh = 1;
                        foreach (var item in listkt)
                        {
                            html += "<tr><td width='5%'>" + sttdh + "</td><td width='15%'>" + item.capKhenThuong + "</td><td width='35%'>" + item.tenKhenThuong + "</td><td width='15%'>" + item.ngayTraoTang.ToString("dd/MM/yyyy") + "</td><td width='15%'>" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", item.tienThuong) + "</td><td width='15%'>" + item.capKyKhenThuong + "</td></tr>";
                            sttdh++;
                        }
                        html += "</tbody></table></div><br />";
                    }
                }
            }
            else
            {
                int idcn = int.Parse(spl[1]);
                var info = _entities.qltdkt_dm_nhanvien.Find(idcn);
                string urlavatar = string.IsNullOrEmpty(_entities.qltdkt_dm_nhanvien.Find(idcn).anhDaiDien) ? "/Uploads/avatarcanhan.jpg" : getanhdaidien(_entities.qltdkt_dm_nhanvien.Find(idcn).anhDaiDien);

                html += "<div class='col-xl-12'><div class='row'><div class='col-xl-4' style='text-align:center; align-items:center'><img src='" + urlavatar + "' style='width:200px;height:200px;' /></div>";
                html += "<div class='col-xl-8'><h1>" + _entities.qltdkt_dm_nhanvien.Find(idcn).hoTen.ToUpper() + "</h1>";
                html += "<div class='row'><div class='col-md-3'><label>Đơn vị: </label></div><div class='col-md-8'><label>" + _entities.qltdkt_dm_donvi.Find(info.donViId).tenDonVi + "</label></div></div>";
                html += "<div class='row'><div class='col-md-3'><label>Chức vụ: </label></div><div class='col-md-8'><label>" + _entities.qltdkt_dm_chucvu.Find(info.chucVuId).tenChucVu + "</label></div></div>";
                html += "<div class='row'><div class='col-md-3'><label>Ngày sinh: </label> </div><div class='col-md-8'><label>" + ((DateTime)info.ngaySinh).ToString("dd/MM/yyyy") + "</label></div></div>";
                html += "<div class='row'><div class='col-md-3'><label>Giới tính: </label> </div><div class='col-md-8'><label>" + getGioiTinh(info.gioiTinh) + "</label></div></div>";
                html += "<div class='row'><div class='col-md-3'><label>Số điện thoại: </label> </div><div class='col-md-8'><label>" + info.soDienThoai + "</label></div></div>";
                html += "<div class='row'><div class='col-md-3'><label>Email: </label> </div><div class='col-md-8'><label>" + info.email + "</label></div></div></div></div>";
                html += "<div class='row'><div class='col-xl-12'><h3>THÀNH TÍCH ĐẠT ĐƯỢC</h3></div>";
                var hs = _entities.qltdkt_hosokyyeu.Where(x => x.idNhanVien == idcn).FirstOrDefault();
                if (hs != null)
                {
                    List<soKyYeuModel> listAll = new List<soKyYeuModel>();
                    var chitiet = JsonConvert.DeserializeObject<List<soKyYeuModel>>(hs.hoSoKyYeu);
                    listAll.AddRange(chitiet);

                    var listdh = listAll.Where(x => x.kieuKyYeu == 2).ToList();
                    var listkt = listAll.Where(x => x.kieuKyYeu == 3).ToList();

                    if (listdh.Count() > 0)
                    {
                        html += "<div class='col-xl-12'><h4>DANH HIỆU:</h4></div>";
                        html += "<div class='col-xl-12'><table width='100%'><thead><tr><th width='5%'>STT</th><th width='20%'>Người ký danh hiệu</th><th width='25%'>Tên danh hiệu</th><th width='10%'>Năm Danh Hiệu</th><th width='20%'>Hình thức trao tặng</th><th width='15%'>Ngày trao tặng</th><th width='5%'></th></tr></thead><tbody>";
                        int sttdh = 1;
                        foreach (var item in listdh)
                        {
                            html += "<tr><td width='5%'>" + sttdh + "</td><td width='20%'>" + item.capKhenThuong + "</td><td width='25%'>" + item.tenDanhHieu + "</td><td width='10%'>" + item.namDanhHieu + "</td><td width='20%'>" + (item.hinhThucTraoTang == "0" ? "Chưa có hình thức trao tặng danh hiệu/ trao tặng trực tiếp" : item.hinhThucTraoTang) + " " + "</td><td width='15%'>" + item.ngayTraoTang.ToString("dd/MM/yyyy") + "</td><td width='5%'><a class='fa fa-images' style='color: blue' href='#' data-toggle='tooltip' title='Xem ảnh/video' onclick='return xemAnh(" + item.hinhAnhTraoTang + ")'></a></td></tr>";
                            sttdh++;
                        }
                        html += "</tbody></table></div><br />";
                    }
                    if (listkt.Count() > 0)
                    {
                        html += "<div class='col-xl-12'><h4>KHEN THƯỞNG:</h4></div>";
                        html += "<div class='col-xl-12'><table width='100%'><thead><tr><th width='5%'>STT</th><th width='15%'>Cấp khen thưởng</th><th width='35%'>Tên khen thưởng</th><th width='15%'>Ngày</th><th width='15%'>Tiền thưởng</th><th width='15%'>Người ký khen thưởng</th></tr></thead><tbody>";
                        int sttdh = 1;
                        foreach (var item in listkt)
                        {
                            html += "<tr><td width='5%'>" + sttdh + "</td><td width='15%'>" + item.capKhenThuong + "</td><td width='35%'>" + item.tenKhenThuong + "</td><td width='15%'>" + item.ngayTraoTang.ToString("dd/MM/yyyy") + "</td><td width='15%'>" + string.Format(System.Globalization.CultureInfo.GetCultureInfo("vi-VN"), "{0:C}", item.tienThuong) + "</td><td width='15%'>" + item.capKyKhenThuong + "</td></tr>";
                            sttdh++;
                        }
                        html += "</tbody></table></div><br />";
                    }
                }
            }
            return html;
        }

        private string getGioiTinh(bool? gioiTinh)
        {
            switch (gioiTinh)
            {
                case false: return "Nam";
                case true: return "Nữ";
            }
            return "";
        }
    }
}