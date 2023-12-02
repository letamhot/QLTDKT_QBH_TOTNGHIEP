using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using QLTDKT.Models;

namespace QLTDKT.Util
{
    public class JsTreeAccess
    {
        private quanlythiduakhenthuongEntities _entities = new quanlythiduakhenthuongEntities();
        public void AddChildItems(List<qltdkt_dm_donvi> lsDonVi, Node parentNode, int parentID)
        {
            List<qltdkt_dm_donvi> result = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == parentID)
                {
                    result.Add(item);
                }
            }
            try
            {
                foreach (var item in result)
                {
                    Node node = new Node();
                    node.id = item.id;
                    node.text = item.tenDonVi;
                    node.icon = "fa fa-user text-danger fa-lg";
                    node.children = new List<Node>();
                    if (node != null)
                    {
                        parentNode.children.Add(node);
                    }
                    AddChildItems(lsDonVi, node, item.id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void AddChildItemsNEW(List<qltdkt_dm_donvi> lsDonVi, Node parentNode, int parentID)
        {
            List<qltdkt_dm_donvi> result = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == 1)
                {
                    result.Add(item);
                }
            }
            try
            {
                foreach (var item in result)
                {
                    Node node = new Node();
                    node.id = item.id;
                    node.text = item.tenDonVi;
                    node.icon = "fa fa-user text-danger fa-lg";
                    node.children = new List<Node>();
                    if (node != null)
                    {
                        parentNode.children.Add(node);
                    }
                    //AddChildItemsNEW(lsDonVi, node, item.id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public void AddChildItemsQuyen(List<qltdkt_dm_role> lsQuyen, Node parentNode, int parentID)
        {
            List<qltdkt_dm_role> result = new List<qltdkt_dm_role>();
            foreach (var item in lsQuyen)
            {
                if (item.roleParent == parentID)
                {
                    result.Add(item);
                }
            }
            try
            {
                foreach (var item in result)
                {
                    Node node = new Node();
                    node.id = item.id;
                    node.text = item.roleName;
                    node.icon = "fa fa-user text-danger fa-lg";
                    node.children = new List<Node>();
                    if (node != null)
                    {
                        parentNode.children.Add(node);
                    }
                    AddChildItemsQuyen(lsQuyen, node, item.id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        public Node getTreeCaNhanTapThe()
        {
            List<qltdkt_dm_donvi> lsDonVi = _entities.qltdkt_dm_donvi.ToList();
            var donVi = _entities.qltdkt_dm_donvi.FirstOrDefault(x => x.idCha == 0);
            Node root = new Node();
            root.id = donVi.id;
            root.children = new List<Node>();
            root.text = donVi.tenDonVi;
            root.icon = "fa fa-folder-open fa-lg";
            List<qltdkt_dm_donvi> lsCha = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == donVi.id)
                {
                    lsCha.Add(item);
                }
            }
            foreach (var item in lsCha)
            {
                Node cNode = new Node();
                cNode.id = item.id;
                cNode.text = item.tenDonVi;
                cNode.icon = "fa fa-users text-primary fa-lg";
                cNode.children = new List<Node>();
                root.children.Add(cNode);
                AddChildItems(lsDonVi, cNode, item.id);
            }
            return root;
        }
        public Node getTreeCaNhan()
        {
            List<qltdkt_dm_donvi> lsDonVi = _entities.qltdkt_dm_donvi.ToList();
            var donVi = _entities.qltdkt_dm_donvi.FirstOrDefault(x => x.idCha == 0);
            Node root = new Node();
            root.id = donVi.id;
            root.children = new List<Node>();
            root.text = donVi.tenDonVi;
            root.icon = "fa fa-folder-open fa-lg";
            List<qltdkt_dm_donvi> lsCha = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == donVi.id)
                {
                    lsCha.Add(item);
                }
            }
            foreach (var item in lsCha)
            {
                Node cNode = new Node();
                cNode.id = item.id;
                cNode.text = item.tenDonVi;
                cNode.icon = "fa fa-users text-primary fa-lg";
                cNode.children = new List<Node>();
                root.children.Add(cNode);
                AddChildItemsNEW(lsDonVi, cNode, item.id);
            }
            return root;
        }

        public Node getTreeDonVi()
        {
            List<qltdkt_dm_donvi> lsDonVi = _entities.qltdkt_dm_donvi.ToList();
            var donVi = _entities.qltdkt_dm_donvi.FirstOrDefault(x => x.idCha == 0);
            Node root = new Node();
            root.id = donVi.id;
            root.children = new List<Node>();
            root.text = donVi.tenDonVi;
            root.icon = "fa fa-folder-open fa-lg";
            List<qltdkt_dm_donvi> lsCha = new List<qltdkt_dm_donvi>();
            foreach (var item in lsDonVi)
            {
                if (item.idCha == donVi.id)
                {
                    lsCha.Add(item);
                }
            }
            foreach (var item in lsCha)
            {
                Node cNode = new Node();
                cNode.id = item.id;
                cNode.text = item.tenDonVi;
                cNode.icon = "fa fa-users text-primary fa-lg";
                cNode.children = new List<Node>();
                root.children.Add(cNode);
                AddChildItemsNEW(lsDonVi, cNode, item.id);
            }
            return root;
        }
    }
}