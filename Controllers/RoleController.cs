using QLTDKT.Models;
using QLTDKT.Util;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QLTDKT.Controllers
{
    public class RoleController : Controller
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public JsTreeAccess _jsUtil = new JsTreeAccess();
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult RoleMenu()
        {
            return View();
        }

        public JsonResult GetListQuyen()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_dm_role.ToList() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public string UpdateQuyen(qltdkt_dm_role _objQuyen)
        {
            try
            {
                if (_objQuyen.id == 0)
                {
                    qltdkt_dm_role _new = new qltdkt_dm_role();
                    _new.roleName = _objQuyen.roleName;
                    _new.roleParent = _objQuyen.roleParent;
                    _new.styles = _objQuyen.styles;
                    _new.@class = _objQuyen.@class;
                    _new.controller = _objQuyen.controller;
                    _new.action = _objQuyen.action;
                    _entities.qltdkt_dm_role.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {
                    qltdkt_dm_role _update = _entities.qltdkt_dm_role.Find(_objQuyen.id);
                    _update.roleName = _objQuyen.roleName;
                    _update.roleParent = _objQuyen.roleParent;
                    _update.styles = _objQuyen.styles;
                    _update.@class = _objQuyen.@class;
                    _update.controller = _objQuyen.controller;
                    _update.action = _objQuyen.action;
                    try
                    {
                        // Your code...
                        // Could also be before try if you know the exception occurs in SaveChanges

                        _entities.SaveChanges();
                    }
                    catch (DbEntityValidationException e)
                    {
                        foreach (var eve in e.EntityValidationErrors)
                        {
                            Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                                eve.Entry.Entity.GetType().Name, eve.Entry.State);
                            foreach (var ve in eve.ValidationErrors)
                            {
                                Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                    ve.PropertyName, ve.ErrorMessage);
                            }
                        }
                        throw;
                    }
                    //_entities.SaveChanges();
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public JsonResult GetQuyenById()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            int QuyenMenuId = int.Parse(Request.QueryString["id"]);
            return Json(_entities.qltdkt_dm_role.FirstOrDefault(x => x.id == QuyenMenuId), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public string GetNameQuyenById()
        {
            int idquyenmenuid = int.Parse(Request.QueryString["id"]);
            try
            {
                var _obj = _entities.qltdkt_dm_role.FirstOrDefault(x => x.id == idquyenmenuid);
                return _obj.roleName;
            }
            catch (Exception)
            {
                return "";
                throw;
            }
        }

        [HttpPost]
        public bool DeleteByID()
        {
            try
            {
                int idquyenmenu = int.Parse(Request["id"]);
                qltdkt_dm_role _old = _entities.qltdkt_dm_role.Find(idquyenmenu);
                if (_old != null)
                {
                    _entities.qltdkt_dm_role.Remove(_old);
                    _entities.SaveChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }



        [HttpPost]
        public bool DeleteByArrayId()
        {
            try
            {
                string idquyenmenuarr = Request["id"];
                string[] idquyenmenu = idquyenmenuarr.Split(' ');
                for (int i = 0; i < idquyenmenu.Length; i++)
                {
                    qltdkt_dm_role _old = _entities.qltdkt_dm_role.Find(int.Parse(idquyenmenu[i]));
                    if (_old != null)
                    {
                        _entities.qltdkt_dm_role.Remove(_old);
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

        public JsonResult LoadTreeQuyen()
        {
            List<qltdkt_dm_role> lsQuyen = _entities.qltdkt_dm_role.ToList();
            Node root = new Node();
            root.id = 0;
            root.children = new List<Node>();
            root.text = "Danh sách nhóm quyền";
            root.icon = "fa fa-folder-open fa-lg";
            List<qltdkt_dm_role> lsCha = new List<qltdkt_dm_role>();
            foreach (var item in lsQuyen)
            {
                if (item.roleParent == 0)
                {
                    lsCha.Add(item);
                }
            }
            foreach (var item in lsCha)
            {
                Node cNode = new Node();
                cNode.id = item.id;
                cNode.text = item.roleName;
                cNode.icon = "fa fa-users text-primary fa-lg";
                cNode.children = new List<Node>();
                root.children.Add(cNode);
                _jsUtil.AddChildItemsQuyen(lsQuyen, cNode, item.id);
            }
            return Json(root, JsonRequestBehavior.AllowGet);
        }
    }
}