using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.BLL;
using DAHeating.DataAccess;


namespace HeatingDSC.Controls
{
    public partial class uctvHrz : UserControl
    {

        log4net.ILog log = log4net.LogManager.GetLogger("uctvHrz");

        public event delNodeMouseClick delNodeMouseClickHandler;
        List<DetailDevice> devList = new List<DetailDevice>();

        private List<Guid> HrzIDs_ = new List<Guid>();

        OrganizeDa orgDa ;
        ////////SimcardDa simcardDa;

        public List<Guid> HrzIDs//当前选择换热站ID
        {
            get{
                HrzIDs_.Clear();
                foreach (TreeNode tn in tv.Nodes[0].Nodes)
                {
                    if (tn.Name.Substring(0, 2) == "hr")
                    {
                        if (tn.Checked)
                        {
                            HrzIDs_.Add(Guid.Parse(tn.Tag.ToString()));
                        }
                    }
                }
                return HrzIDs_;
            }
        }

        public uctvHrz()
        {
            InitializeComponent();
        }
       
        private void tv_DragEnter(object sender, DragEventArgs e)
        {
            //设置拖放类别(复制，移动等)
            e.Effect = DragDropEffects.Copy;
        }

        private void tv_ItemDrag(object sender, ItemDragEventArgs e)
        {
            try
            {
                TreeNode tn = (TreeNode)e.Item;
                //if (CanDragTreeNode(tn))
                {
                    //启动拖放操作，设置拖放类型为Copy
                    tv.DoDragDrop(e.Item, DragDropEffects.Copy);
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }
        }

        private bool CanDragTreeNode(TreeNode tn)
        {
            bool ret = false;

            try
            {
                if (tn != null && tn.Tag != null && tn.Tag is DTUProduct)
                {
                    DTUProduct dc = tn.Tag as DTUProduct;
                }
            }
            catch (Exception ex)
            {
                log.Error(ex.Message);
            }

            return ret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="orgid">供热站ID</param>
        public void RefreshTreeAllDevice(Guid orgid)
        {
            orgDa = new OrganizeDa();
            ////////simcardDa = new SimcardDa();

            bsOrganize grz = EntityManager<bsOrganize>.GetByPk<bsOrganize>("Id",orgid);
            tv.Nodes.Clear();
            if (grz != null)
            {
                TreeNode newNode = new TreeNode(grz.Name);
                newNode.Name = "gr" + grz.Id.ToString();

                newNode.Tag = grz.Id;

                tv.CheckBoxes = true;
                tv.Nodes.Add(newNode);
            }

            List<bsOrganize> regList =EntityManager<bsOrganize>.GetListNoPaging<bsOrganize>("PId='"+orgid.ToString()+"'","");

            if (regList.Count != 0)
            {
                foreach (bsOrganize r in regList)
                {
                    Application.DoEvents();

                    TreeNode newNode = new TreeNode(r.Name);
                    newNode.Name = "hr" + r.Id.ToString();

                    newNode.Tag = r.Id;
                 
                    tv.Nodes[0].Nodes.Add(newNode);
                }
            }
            tv.ExpandAll();
        }
        private void tv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Name.Length > 2)
            {
                if (tn.Name.Substring(0, 2) == "gr")
                {
                    foreach (TreeNode tnch in tn.Nodes)
                    {
                        tnch.Checked = tn.Checked;
                    }
                }
            }

        }

        private Guid OrgId;
        private void tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is int)
            {
                OrgId = Guid.Parse(e.Node.Tag.ToString());
                bsOrganize org = orgDa.SelectOrganization(OrgId);
                //if (org.OrganizeType == "换热站")
                //{
                    this.textBox1.Text = org.Name;
                    if (delNodeMouseClickHandler != null)
                        delNodeMouseClickHandler(org);
                //}
                //else
                //{
                //    this.textBox1.Text = "";
                //}
            }
            
        }

    }
}
