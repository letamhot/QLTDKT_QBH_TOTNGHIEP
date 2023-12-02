using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QLTDKT.Models.Service.groupUserService
{
    public class groupUserService
    {
        SqlDataAccess _sqlAccess = new SqlDataAccess();

        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public DataTable GetQuyenMain()
        {
            DataTable dt = new DataTable();
            string sql = "select * from qltdkt_dm_role where roleParent = 0";
            dt = _sqlAccess.getDataFromSql(sql, "").Tables[0];
            return dt;
        }

        public DataTable GetRoleChild(int parentid)
        {
            DataTable dt = new DataTable();
            string sql = "select * from qltdkt_dm_role where roleParent = " + parentid;
            dt = _sqlAccess.getDataFromSql(sql, "").Tables[0];
            return dt;
        }

        public DataTable GetRoleSelectedByNhomQuyenID(int userGroupId)
        {
            DataTable dt = new DataTable();
            string query = "select roleId from qltdkt_groupuserbyroles where groupUserId = " + userGroupId;
            dt = _sqlAccess.getDataFromSql(query, "").Tables[0];
            return dt;
        }

        public int GetParentIdByQuyenId(int roleId)
        {
            DataTable dt = new DataTable();
            string sql = "select roleParent from qltdkt_dm_role where id = " + roleId;
            dt = _sqlAccess.getDataFromSql(sql, "").Tables[0];
            if (dt.Rows.Count > 0)
            {
                return int.Parse(dt.Rows[0]["roleParent"].ToString());
            }
            return 0;
        }
        public List<int> getDonViChildByDonViId(int iddonvi)
        {
            List<int> rs = new List<int>();
            rs.Add(iddonvi);


            List<qltdkt_dm_donvi> lsDonVi = _entities.qltdkt_dm_donvi.ToList();
            List<qltdkt_dm_donvi> lsCha = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == iddonvi)
                {
                    lsCha.Add(item);
                }
            }
            foreach (var item in lsCha)
            {
                rs.Add(item.id);
                AddChild(lsDonVi, rs, item.id);
            }
            return rs;
        }
        public void AddChild(List<qltdkt_dm_donvi> lsDonvi, List<int> rs, int parentId)
        {
            List<qltdkt_dm_donvi> result = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonvi)
            {
                if (item.idCha == parentId)
                {
                    result.Add(item);
                }
            }
            try
            {
                foreach (var item in result)
                {
                    rs.Add(item.id);
                    AddChild(lsDonvi, rs, item.id);
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        public bool CheckExistParentNode(int userGroupId, int roleId)
        {
            string sql = "select * from qltdkt_groupuserbyroles where groupUserId = " + userGroupId + " and roleId = " + roleId;
            DataTable dt = _sqlAccess.getDataFromSql(sql, "").Tables[0];
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
    }
}