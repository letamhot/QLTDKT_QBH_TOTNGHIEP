using QLTDKT.Models.Service.groupUserService;
using QLTDKT.Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLTDKT.Models;
using System.Data.Entity.Validation;
using System.Data;

namespace QLTDKT.Controllers
{
    public class UserGroupController : Controller
    {
        private SqlDataAccess _sqlAccess = new SqlDataAccess();
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private groupUserService _service = new groupUserService();
        // GET: UserGroup
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetNhomQuyen()
        {
            _entities.Configuration.ProxyCreationEnabled = false;
            return Json(new { data = _entities.qltdkt_groupuser.ToList() }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetDataNhomNguoiDungById()
        {
            int idNhomNguoiDung = int.Parse(Request.QueryString["id"]);
            return Json(_entities.qltdkt_groupuser.FirstOrDefault(x => x.id == idNhomNguoiDung), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public string UpdateNhom(qltdkt_groupuser _objGroup)
        {
            try
            {
                if (_objGroup.id == 0)
                {
                    qltdkt_groupuser _new = new qltdkt_groupuser
                    {
                        tenNhom = _objGroup.tenNhom,
                        ngayTao = DateTime.Now
                    };
                    _entities.qltdkt_groupuser.Add(_new);
                    _entities.SaveChanges();
                    return "addsuccess";
                }
                else
                {
                    qltdkt_groupuser _update = _entities.qltdkt_groupuser.Find(_objGroup.id);
                    _update.tenNhom = _objGroup.tenNhom;
                    _update.ngayCapNhat = DateTime.Now;

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
                    return "updatesuccess";
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
        }

        public JsonResult GetTreeQuyenMenu(int ID)
        {
            DataTable dt = new DataTable();
            JsTreeModel nodeRoot = new JsTreeModel { id = "0", parent = "#", text = "Danh sách quyền", icon = "fa fa-folder-open fa-lg", state = new States { opened = true } };
            var nodes = new List<JsTreeModel>
            {
                nodeRoot
            };
            dt = _service.GetQuyenMain();
            DataTable dtSelected = new DataTable();
            dtSelected = _service.GetRoleSelectedByNhomQuyenID(ID);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                nodes.Add(new JsTreeModel() { id = dt.Rows[i]["id"].ToString(), parent = dt.Rows[i]["roleParent"].ToString(), text = dt.Rows[i]["roleName"].ToString(), state = new States { selected = false }, icon = "fa fa-users text-primary fa-lg" });
                DataTable dtChild = _service.GetRoleChild(int.Parse(dt.Rows[i]["id"].ToString()));
                if (dtChild.Rows.Count > 0)
                {
                    for (int k = 0; k < dtChild.Rows.Count; k++)
                    {
                        bool isChecked = false;

                        for (int j = 0; j < dtSelected.Rows.Count; j++)
                        {

                            if (dtSelected.Rows[j]["roleId"].ToString() == dtChild.Rows[k]["id"].ToString())
                            {
                                isChecked = true;
                                break;
                            }
                        }

                        DataTable dtChild_2 = _service.GetRoleChild(int.Parse(dtChild.Rows[k]["id"].ToString()));

                        if (dtChild_2.Rows.Count > 0)
                        {
                            nodes.Add(new JsTreeModel() { id = dtChild.Rows[k]["id"].ToString(), parent = dt.Rows[i]["id"].ToString(), text = dtChild.Rows[k]["roleName"].ToString(), state = new States { selected = false }, icon = "fa fa-user text-danger fa-lg" });

                            for (int l = 0; l < dtChild_2.Rows.Count; l++)
                            {
                                bool isChecked1 = false;

                                for (int m = 0; m < dtSelected.Rows.Count; m++)
                                {

                                    if (dtSelected.Rows[m]["roleId"].ToString() == dtChild_2.Rows[l]["id"].ToString())
                                    {
                                        isChecked1 = true;
                                        break;
                                    }
                                }
                                nodes.Add(new JsTreeModel() { id = dtChild_2.Rows[l]["id"].ToString(), parent = dtChild.Rows[k]["id"].ToString(), text = dtChild_2.Rows[l]["roleName"].ToString(), state = new States { selected = isChecked1 }, icon = "fa fa-user text-danger fa-lg" });

                            }


                        }
                        else
                        {
                            nodes.Add(new JsTreeModel() { id = dtChild.Rows[k]["id"].ToString(), parent = dt.Rows[i]["id"].ToString(), text = dtChild.Rows[k]["roleName"].ToString(), state = new States { selected = isChecked }, icon = "fa fa-user text-danger fa-lg" });

                        }

                    }
                }
            }
            return Json(nodes, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public bool DeleteByIDGroupUser()
        {
            try
            {
                int idquyenmenu = int.Parse(Request["idquyenmenu"]);
                qltdkt_groupuser _old = _entities.qltdkt_groupuser.Find(idquyenmenu);

                if (_old != null)
                {
                    _old.daXoa = "1";
                    _entities.qltdkt_groupuser.Remove(_old);
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
        public bool SaveNhomQuyenMenu(int userGroupId, List<int> listRole)
        {
            bool deloldrole = _sqlAccess.DbCommand("delete from qltdkt_groupuserbyroles where groupUserId = " + userGroupId);
            if (deloldrole)
            {
                if (listRole.Count == 0)
                {
                    return true;
                }
                else
                {
                    listRole.Sort();
                    try
                    {
                        for (int i = 0; i < listRole.Count; i++)
                        {
                            if (listRole[i] != 0)
                            {
                                qltdkt_groupuserbyroles _new = new qltdkt_groupuserbyroles
                                {
                                    groupUserId = userGroupId,
                                    roleId = listRole[i]
                                };
                                _entities.qltdkt_groupuserbyroles.Add(_new);
                                _entities.SaveChanges();
                                int parentid = _service.GetParentIdByQuyenId(listRole[i]);
                                if (parentid != 0)
                                {
                                    if (!_service.CheckExistParentNode(userGroupId, parentid))
                                    {
                                        qltdkt_groupuserbyroles _roleParent = new qltdkt_groupuserbyroles
                                        {
                                            groupUserId = userGroupId,
                                            roleId = parentid
                                        };
                                        _entities.qltdkt_groupuserbyroles.Add(_roleParent);
                                        _entities.SaveChanges();
                                    }
                                }
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
            }
            return false;
        }
    }
}