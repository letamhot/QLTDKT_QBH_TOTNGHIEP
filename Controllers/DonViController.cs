using Newtonsoft.Json;
using QLTDKT.Models;
using QLTDKT.Models.Service.groupUserService;
using QLTDKT.Util;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class DonViController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public JsTreeAccess _jsUtil = new JsTreeAccess();
        private groupUserService _service = new groupUserService();
        // GET: DonVi
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetListDonVi()
        {
            int iddonvikt = Convert.ToInt32(Request["id"]);
            _entities.Configuration.ProxyCreationEnabled = false;
            var exam = _entities.qltdkt_dm_donvi.Where(x => x.id == iddonvikt || x.idCha == iddonvikt).ToList();
            return Json(new { data = exam }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult LoadAllDonVi()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_donvi.ToList() }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetDonViById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int iddonvi = int.Parse(Request.QueryString["ID"]);
            return Json(_entities.qltdkt_dm_donvi.Find(iddonvi), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public string GetNameDonViById()
        {

            int iddonvi = int.Parse(Request.QueryString["ID"]);
            if (iddonvi == 0)
            {
                iddonvi = 122;
            }
            try
            {

                var _obj = _entities.qltdkt_dm_donvi.FirstOrDefault(x => x.id == iddonvi);
                return _obj.tenDonVi;
            }
            catch (Exception)
            {
                return "";
                throw;
            }

        }
        public string DeleteById()
        {
            var id = Session["IDNhanVien"];
            int idtochuc = int.Parse(Request["ID"]);
            try
            {
                qltdkt_dm_donvi _obj = _entities.qltdkt_dm_donvi.Find(idtochuc);

                if (_obj != null)
                {

                    _entities.qltdkt_dm_donvi.Remove(_obj);
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
        public bool ExistTenDonVi()
        {
            string label = Request.QueryString["label"];
            var chk = _entities.qltdkt_dm_donvi.Where(x => x.tenDonVi == label.ToUpper() && x.tenDonVi == label.ToLower()).FirstOrDefault();
            if (chk != null)
            {
                return false;
            }
            return true;
        }
        public string UpdateDonVi()
        {
            try
            {
                int idcha;
                int id = int.Parse(Request.Form.Get("id"));
                if (Request.Form.Get("idCha") != "")
                {
                    idcha = int.Parse(Request.Form.Get("idCha"), System.Globalization.NumberStyles.Any);

                }
                else
                {
                    idcha = 122;
                }
                string ten = (Request.Form.Get("tenDonVi"));
                string output_filedinhkem = "";
                string f = "";
                if (id == 0)
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
                                f = ftype;
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
                            return "warning";
                            throw;
                        }
                    }
                    if (f != "image/jpeg" && f != "image/png" && f != "")
                    {
                        return "anh";
                    }
                    else
                    {
                        qltdkt_dm_donvi _new = new qltdkt_dm_donvi();
                        _new.idCha = idcha;
                        _new.tenDonVi = ten;
                        _new.anh = output_filedinhkem;
                        _entities.qltdkt_dm_donvi.Add(_new);
                        _entities.SaveChanges();

                        return "addsuccess";
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
                                f = ftype;
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
                            return "warning";
                            throw;
                        }
                    }
                    if (f != "image/jpeg" && f != "image/png" && f != "")
                    {
                        return "anh";
                    }
                    else
                    {
                        qltdkt_dm_donvi _update = _entities.qltdkt_dm_donvi.Find(id);
                        _update.anh = output_filedinhkem;
                        _update.idCha = idcha;
                        _update.tenDonVi = ten;
                        _entities.SaveChanges();
                        return "updatesuccess";
                    }
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }
        public JsonResult LoadTreeDonVi()
        {
            return Json(_jsUtil.getTreeCaNhanTapThe(), JsonRequestBehavior.AllowGet);

        }
    }
}