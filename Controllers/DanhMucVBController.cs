using QLTDKT.Models;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity.Validation;
using System.IO;
using Newtonsoft.Json;
using QLTDKT.Models.Service.thiDuaService;

namespace QLTDKT.Controllers
{
    public class DanhMucVBController : Controller
    {

        // GET: DanhMucVB
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public ActionResult Index()
        {

            return View();
        }


        //load form VB
        public JsonResult GetDanhMucVB()
        {

            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_vanbanhd.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;

            int ID = int.Parse(Request.QueryString["id"]);
            return Json(_entities.qltdkt_dm_vanbanhd.Find(ID), JsonRequestBehavior.AllowGet);
        }
        public bool ExistTenVB()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_vanbanhd.Where(x => x.tenVB == label.ToLower() && x.tenVB == label.ToUpper()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        public int CapNhatVB(qltdkt_dm_vanbanhd vbObj)
        {

            //int idVB = int.Parse(Request.Form.Get("id"));
            string tenVB = Request.Form.Get("tenVB");
            //string output_filedinhkem = "";
            string output_filedinhkem = "";
            if (vbObj.id == 0) ///Thêm mới  Văn bản
            {
                if (Request.Files.Count > 0)
                {
                    try
                    {
                        string currentYear = DateTime.Now.Year.ToString();
                        string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                        if (!Directory.Exists(fpath))
                        {
                            Directory.CreateDirectory(fpath);
                        }
                        HttpFileCollectionBase files = Request.Files;
                        List<KeyValuePair<string, string>> lsFileDownload = new List<KeyValuePair<string, string>>();
                        for (int i = 0; i < files.Count; i++)
                        {
                            //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                            string filename = Path.GetFileName(Request.Files[i].FileName);

                            HttpPostedFileBase file = files[i];
                            string fname, ftype;
                            if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                            {
                                string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                fname = testfiles[testfiles.Length - 1];
                            }
                            else
                            {
                                fname = file.FileName;
                            }
                            fname = Path.Combine(fpath, fname);
                            ftype = file.ContentType;
                            if (!System.IO.File.Exists(fname))
                            {
                                file.SaveAs(fname);
                            }
                            lsFileDownload.Add(new KeyValuePair<string, string>(fname, ftype));
                            output_filedinhkem = JsonConvert.SerializeObject(lsFileDownload);
                        }
                    }
                    catch (Exception)
                    {
                        return -1;
                        throw;
                    }
                }

                try
                {
                    qltdkt_dm_vanbanhd _new = new qltdkt_dm_vanbanhd
                    {
                        tenVB = tenVB,
                        fileVB = output_filedinhkem,
                        ngayTao = DateTime.Now,
                        daXoa = false,
                    };
                    _entities.qltdkt_dm_vanbanhd.Add(_new);
                    _entities.SaveChanges();
                    return 0;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
            }
            else ///Cập nhật văn bản
            {
                qltdkt_dm_vanbanhd _update = _entities.qltdkt_dm_vanbanhd.Find(vbObj.id);
                string dulieu_dinhkem = _update.fileVB;
                if (dulieu_dinhkem == "")
                {
                    if (Request.Files.Count > 0)
                    {
                        try
                        {
                            string currentYear = DateTime.Now.Year.ToString();
                            string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                            if (!Directory.Exists(fpath))
                            {
                                Directory.CreateDirectory(fpath);
                            }
                            HttpFileCollectionBase files = Request.Files;
                            List<KeyValuePair<string, string>> lsFileDownload = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < files.Count; i++)
                            {
                                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                                string filename = Path.GetFileName(Request.Files[i].FileName);

                                HttpPostedFileBase file = files[i];
                                string fname, ftype;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = file.FileName;
                                }
                                fname = Path.Combine(fpath, fname);
                                ftype = file.ContentType;
                                if (!System.IO.File.Exists(fname))
                                {
                                    file.SaveAs(fname);
                                }
                                lsFileDownload.Add(new KeyValuePair<string, string>(fname, ftype));
                                output_filedinhkem = JsonConvert.SerializeObject(lsFileDownload);
                            }
                        }
                        catch (Exception)
                        {
                            return -1;
                            throw;
                        }
                    }
                }
                else
                {
                    if (Request.Files.Count > 0)
                    {
                        try
                        {
                            string currentYear = DateTime.Now.Year.ToString();
                            string fpath = Server.MapPath("~/Uploads/" + currentYear + "/");
                            if (!Directory.Exists(fpath))
                            {
                                Directory.CreateDirectory(fpath);
                            }
                            HttpFileCollectionBase files = Request.Files;
                            List<KeyValuePair<string, string>> lsFileDownload = new List<KeyValuePair<string, string>>();
                            for (int i = 0; i < files.Count; i++)
                            {
                                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";
                                string filename = Path.GetFileName(Request.Files[i].FileName);

                                HttpPostedFileBase file = files[i];
                                string fname, ftype;
                                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                                {
                                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                                    fname = testfiles[testfiles.Length - 1];
                                }
                                else
                                {
                                    fname = file.FileName;
                                }
                                fname = Path.Combine(fpath, fname);
                                ftype = file.ContentType;
                                if (!System.IO.File.Exists(fname))
                                {
                                    file.SaveAs(fname);
                                }
                                lsFileDownload.Add(new KeyValuePair<string, string>(fname, ftype));
                                output_filedinhkem = JsonConvert.SerializeObject(lsFileDownload);
                            }
                        }
                        catch (Exception)
                        {
                            return -1;
                            throw;
                        }
                    }
                    else
                    {
                        output_filedinhkem = dulieu_dinhkem;
                    }
                }


                try
                {
                    _update.tenVB = tenVB;
                    _update.fileVB = output_filedinhkem;
                    _update.ngayCapNhat = DateTime.Now;

                    _entities.SaveChanges();
                    return 2;
                }
                catch (Exception)
                {
                    return -2;
                    throw;
                }
            }

        }

        [HttpPost]
        public string XoaVanBan()
        {
            int id = int.Parse(Request["ID"]);
            try
            {
                qltdkt_dm_vanbanhd _obj = _entities.qltdkt_dm_vanbanhd.Find(id);

                if (_obj != null)
                {
                    _entities.qltdkt_dm_vanbanhd.Remove(_obj);
                    _obj.daXoa = true;

                }
                var hstd = _entities.qltdkt_hosothidua.Where(x => x.fileDinhKem != null && x.daXoa == false).ToList();
                if (hstd != null && hstd.Count > 0)
                {
                    for (int i = 0; i < hstd.Count; i++)
                    {
                        List<danhSachVanBan> dsvb = JsonConvert.DeserializeObject<List<danhSachVanBan>>(hstd[i].fileDinhKem);
                        for (int j = 0; j < dsvb.Count; j++)
                        {
                            if (dsvb[j].id == id)
                            {
                                return "warning";
                            }
                        }


                    }
                }
                _entities.SaveChanges();
                return "success";

            }
            catch (Exception)
            {
                return "error";
                throw;
            }

        }

        public bool DeleteByArrayId()
        {
            try
            {

                var id = Session["id"];
                string idvbarr = Request["ID"];
                string[] idvb = idvbarr.Split(' ');
                for (int i = 0; i < idvb.Length; i++)
                {
                    qltdkt_dm_vanbanhd _old = _entities.qltdkt_dm_vanbanhd.Find(int.Parse(idvb[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_vanbanhd.Remove(_old);
                        _entities.SaveChanges();

                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public ActionResult DownloadFile()
        {
            string fullpath = Request["fullpath"];
            string strfull = fullpath.Replace(",", "\\");
            string file = Request["file"];
            string type = Request["type"];
            return File(strfull, type, file);
        }
    }
}