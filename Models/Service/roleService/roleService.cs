using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.roleService
{
    public class roleService
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        private SqlDataAccess _sqlAccess = new SqlDataAccess();

        /* các nhánh trong sidebar*/
        public List<QuyenMenuFull> getListRoleParent(int userId)
        {
            List<QuyenMenuFull> lsQuyenCha = new List<QuyenMenuFull>();
            DataTable dt = new DataTable();
            string sql = "select * from qltdkt_dm_role where roleParent = 0 and id in (select roleId from qltdkt_groupuserbyroles where groupUserId in (select groupUserId from qltdkt_userbygroup where userId = " + userId + ")) order by priority";
            dt = _sqlAccess.getDataFromSql(sql, "").Tables[0];
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    List<QuyenMenuFull> lsChild = new List<QuyenMenuFull>();
                    DataTable dt_child = new DataTable();
                    string sql_child = "select * from qltdkt_dm_role where roleParent = " + int.Parse(dt.Rows[i]["id"].ToString()) + " and id in (select roleId from qltdkt_groupuserbyroles where groupUserId in (select groupUserId from qltdkt_userbygroup where userId = " + userId + ")) order by priority";
                    dt_child = _sqlAccess.getDataFromSql(sql_child, "").Tables[0];
                    if (dt_child.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt_child.Rows.Count; j++)
                        {
                            /*QuyenMenuFull qmf = new QuyenMenuFull
                            {
                                id = int.Parse(dt_child.Rows[j]["id"].ToString()),
                                roleName = dt_child.Rows[j]["roleName"].ToString(),
                                roleParent = int.Parse(dt_child.Rows[j]["roleParent"].ToString()),
                                Class = dt_child.Rows[j]["class"].ToString(),
                                Controller = dt_child.Rows[j]["controller"].ToString(),
                                Action = dt_child.Rows[j]["action"].ToString(),
                                Styles = dt_child.Rows[j]["styles"].ToString()
                            };*/
                            //lsChild.Add(qmf);
                            DataTable dt_child2 = new DataTable();
                            List<QuyenMenuFull> lsChild2 = new List<QuyenMenuFull>();
                            string sql_child2 = "select * from qltdkt_dm_role where roleParent = " + int.Parse(dt_child.Rows[j]["id"].ToString()) + " and id in (select roleId from qltdkt_groupuserbyroles where groupUserId in (select groupUserId from qltdkt_userbygroup where userId = " + userId + ")) order by priority";
                            dt_child2 = _sqlAccess.getDataFromSql(sql_child2, "").Tables[0];


                            if (dt_child2.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt_child2.Rows.Count; k++)
                                {
                                    QuyenMenuFull dtchi2 = new QuyenMenuFull
                                    {
                                        id = int.Parse(dt_child2.Rows[k]["id"].ToString()),
                                        roleName = dt_child2.Rows[k]["roleName"].ToString(),
                                        roleParent = int.Parse(dt_child2.Rows[k]["roleParent"].ToString()),
                                        Class = dt_child2.Rows[k]["class"].ToString(),
                                        Controller = dt_child2.Rows[k]["controller"].ToString(),
                                        Action = dt_child2.Rows[k]["action"].ToString(),
                                        Styles = dt_child2.Rows[k]["styles"].ToString()
                                    };
                                    lsChild2.Add(dtchi2);

                                }
                            }
                            QuyenMenuFull qcp = new QuyenMenuFull
                            {
                                id = int.Parse(dt_child.Rows[j]["id"].ToString()),
                                roleName = dt_child.Rows[j]["roleName"].ToString(),
                                roleParent = int.Parse(dt_child.Rows[j]["roleParent"].ToString()),
                                Class = dt_child.Rows[j]["class"].ToString(),
                                Controller = dt_child.Rows[j]["controller"].ToString(),
                                Action = dt_child.Rows[j]["action"].ToString(),
                                Styles = dt_child.Rows[j]["styles"].ToString(),
                                Children = lsChild2
                            };
                            lsChild.Add(qcp);

                        }

                    }
                    QuyenMenuFull qm = new QuyenMenuFull
                    {
                        id = int.Parse(dt.Rows[i]["id"].ToString()),
                        roleName = dt.Rows[i]["roleName"].ToString(),
                        Class = dt.Rows[i]["class"].ToString(),
                        Styles = dt.Rows[i]["styles"].ToString(),
                        Children = lsChild
                    };
                    lsQuyenCha.Add(qm);
                }
            }
            return lsQuyenCha;
        }
    }
}