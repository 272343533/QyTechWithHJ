using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using SunMvcExpress.Dao;
using log4net;
using SunMvcExpress.Core.BLL;



namespace HeatingDSC.Controls
{

    public delegate void delNodeMouseClick(bsOrganize org,string Commno);

    public partial class ucTvDevice : UserControl
    {
        ILog log = LogManager.GetLogger("ucTvDevice");

        public event delNodeMouseClick delNodeMouseClickHandler;
        List<DetailDevice> devList = new List<DetailDevice>();
     
        public bool AddChannelFlag = false;

        public ucTvDevice()
        {
            InitializeComponent();
        }


        string deviceIdS_ = null;
        bool AllDevice = false;
        //bool _terminated = false;

        public string CurrentCommNo="";
        public bsOrganize CurrentOrg;
      
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

        public void RefreshTreeAllDevice()
        {
            AllDevice = true;
            RefreshTree("");
        }
        public void RefreshTree(string deviceIdS)
        {
            deviceIdS_ = deviceIdS;

            // List<bsOrganize> regList = EntityManager<bsOrganize>.GetListNoPaging<bsOrganize>("OrganizeStatus = '正常'", "Name desc");
            List<bsOrganize> regList = EntityManager<bsOrganize>.GetListNoPaging<bsOrganize>("", "Name desc");

            if (regList.Count != 0)
            {
                tv.Nodes.Clear();
                AddRootTreeViewNodes(regList);
            }
            tv.ExpandAll();
        }
        public void AddDeviceTreeViewNodes(List<DTUProduct> list)
        {

            TreeNode tn = tv.Nodes[0];
            foreach (DTUProduct c in list)
            {
                //if (c.DeviceID != null)
                //{
                TreeNode newNode = new TreeNode(c.DtuProName + "(" + c.CommNo + ")");
                    newNode.ToolTipText = c.CommNo;
                    newNode.Tag = c;
                    tn.Nodes.Add(newNode);
                //}
            }
        }

        public void AddRootTreeViewNodes(List<bsOrganize> list)
        {
            for (int i=list.Count-1;i>=0;i--)
            {
                Application.DoEvents();


                bsOrganize r =list[i];
                if (r.PId == Guid.Parse("E9E793CF-BFF0-4831-9A9E-8260AFF93943"))
                {
                    TreeNode newNode = new TreeNode(r.Name);
                    newNode.Name = "or" + r.Id.ToString();

                    newNode.Tag = r.Id;

                    DTUProduct dev = EntityManager<DTUProduct>.GetBySql<DTUProduct>("azwz='" + r.Id + "'");
                    if (dev != null)
                    {
                        newNode.ToolTipText = dev.CommNo;
                    }

                    tv.Nodes.Add(newNode);
                    list.RemoveAt(i);

                    AddChildTreeViewNodes(ref list, newNode);
                }
            }
        }

        private void AddRegionChannels(TreeNode orgTn, Guid Id)
        {
            List<DTUProduct> chs = EntityManager<DTUProduct>.GetListNoPaging<DTUProduct>("azwz='"+Id.ToString()+"'", "");

            AddRegionChannels(orgTn, chs);
        }

        private void AddRegionChannels(TreeNode regionTn, List<DTUProduct> devs)
        {
            foreach (DTUProduct dev in devs)
            {
                if (!AllDevice)
                {
                    if (!deviceIdS_.Contains(dev.DtuProID.ToString()))
                        continue;
                }
                TreeNode tn;
                tn = new TreeNode(dev.DtuProName);

                tn.Name = "de" + dev.DtuProID;
                tn.Tag = dev;
                tn.ToolTipText = dev.CommNo;
                regionTn.Nodes.Add(tn);
            }
        }

        private void AddChildTreeViewNodes(ref List<bsOrganize> list, TreeNode parentTreeViewNode)
        {

            for (int i = list.Count - 1; i >= 0; i--)
            {
                bsOrganize r = list[i];
                try
                {
                    if (r.PId == Guid.Parse(parentTreeViewNode.Tag.ToString()))
                    {
                        TreeNode newNode = new TreeNode(r.Name);
                        newNode.Name = "or" + r.Id.ToString();
                        newNode.Tag = r.Id;

                        DTUProduct dev = EntityManager<DTUProduct>.GetBySql<DTUProduct>("azwz='" + r.Id + "' and TranType like '%无线%' and bsP_Id!=69");
                        if (dev != null)
                        {
                            newNode.ToolTipText = dev.CommNo;
                        }

                        //增加channel
                        if (AddChannelFlag)
                            AddRegionChannels(newNode, r.Id);

                        parentTreeViewNode.Nodes.Add(newNode);

                        //list.RemoveAt(i);

                        AddChildTreeViewNodes(ref list, newNode);
                    }
                }
                catch(Exception ex) 
                {
                    log.Error(ex.Message);

                }
            }
        }


        public void GetSelectedChannels(ref string sResult)
        {
            GetSelectedChannels(tv.Nodes[0], ref sResult);
            if (sResult.Length > 0)
                sResult = sResult.Substring(1);
        }


        private void GetSelectedChannels(TreeNode tn, ref string sResult)
        {
            try
            {
                if (tn.Name.Substring(0, 2) == "de")
                {
                    if (tn.Checked)
                    {
                        sResult += "," + tn.Name.Substring(2);
                    }
                }
                else if (tn.Name.Substring(0, 2) == "or")
                {
                    for (int i = 0; i < tn.Nodes.Count; i++)
                    {
                        if (tn.Nodes[i].Name.Substring(0, 2) == "or")
                        {
                            GetSelectedChannels(tn.Nodes[i], ref sResult);
                        }
                        else if (tn.Nodes[i].Name.Substring(0, 2) == "de")
                        {
                            if (tn.Nodes[i].Checked)
                            {
                                sResult += "," + tn.Nodes[i].Name.Substring(2);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<DetailDevice> GetDevices()
        {
            List<DetailDevice> devchs = new List<DetailDevice>();
            GetSelectedChannels(tv.Nodes[0], ref devchs);
            return devchs;
        }

        private void GetSelectedChannels(TreeNode tn, ref List<DetailDevice> devs)
        {
            try
            {
                if (tn.Name.Substring(0, 2) == "de")
                {
                    if (tn.Checked)
                    {
                        devs.Add(tn.Tag as DetailDevice);
                    }
                }
                else if (tn.Name.Substring(0, 2) == "or")
                {
                    for (int i = 0; i < tn.Nodes.Count; i++)
                    {
                        if (tn.Nodes[i].Name.Substring(0, 2) == "or")
                        {
                            GetSelectedChannels(tn.Nodes[i], ref devs);
                        }
                        else if (tn.Nodes[i].Name.Substring(0, 2) == "de")
                        {
                            if (tn.Nodes[i].Checked)
                            {
                                devs.Add(tn.Nodes[i].Tag as DetailDevice);                                
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public void SetSelectChannels(string chs)
        {
            SetSelectChannels(tv.Nodes[0], chs);
        }
        private void SetSelectChannels(TreeNode tn, string chs)
        {
            if (tn.Name.Substring(0, 2) == "or")
            {
                if (chs.Contains(tn.Name.Substring(2)))
                    tn.Checked = true;
                else
                    tn.Checked = false;

            }
            else if (tn.Name.Substring(0, 2) == "or")
            {
                for (int i = 0; i < tn.Nodes.Count; i++)
                {
                    if (tn.Nodes[i].Name.Substring(0, 2) == "or")
                    {
                        SetSelectChannels(tn.Nodes[i], chs);
                    }
                    else if (tn.Nodes[i].Name.Substring(0, 2) == "de")
                    {
                        if (chs.Contains(tn.Nodes[i].Name.Substring(2)))
                            tn.Nodes[i].Checked = true;
                        else
                            tn.Nodes[i].Checked = false;
                    }
                }
            }
        }



        private void tv_AfterCheck(object sender, TreeViewEventArgs e)
        {
            TreeNode tn = e.Node;
            if (tn.Name.Length > 2)
            {
                if (tn.Name.Substring(0, 2) == "or")
                {
                    foreach (TreeNode tnch in tn.Nodes)
                    {
                        tnch.Checked = tn.Checked;
                    }
                }
            }

        }

        public Guid OrgId;

        public bool ChangeColorBySimcardNo(string cardno,Color color)
        {
            
            DTUProduct dev= EntityManager<DTUProduct>.GetBySql<DTUProduct>("commno='" + cardno + "'");
            bsOrganize org = EntityManager<bsOrganize>.GetByPk<bsOrganize>("Id",Guid.Parse(dev.AZWZ.ToString()));
            //if (org.OrganizeType == "换热站")
            //{
                foreach (TreeNode tn in tv.Nodes)
                {
                    if (ChangeColorByTreeNodeName(org.Id, tn, color))
                    {
                        return true;
                    }
                }
            //}
            return false;
        }
        private bool ChangeColorByTreeNodeName(Guid Id, TreeNode ptn, Color color)
        {
            bool find = false;
            if (ptn.Tag.ToString()== Id.ToString())
            {
                ptn.BackColor = color;
                find = true;
            }
            else
            {
                foreach (TreeNode tn in ptn.Nodes)
                {
                    find = ChangeColorByTreeNodeName(Id, tn, color);
                    if (find)
                        break;
                }
            }
            return find;
        }

        private void tv_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Node.Tag is Guid)
            {
                OrgId = Guid.Parse(e.Node.Tag.ToString());
                bsOrganize org = SunMvcExpress.Core.BLL.EntityManager<bsOrganize>.GetByPk<bsOrganize>("Id", OrgId);

                this.textBox1.Text = org.Name+"("+e.Node.ToolTipText+")";
                CurrentOrg = org;
                CurrentCommNo = e.Node.ToolTipText;

                if (delNodeMouseClickHandler != null)
                    delNodeMouseClickHandler(org, CurrentCommNo);

            }
            
        }

        private void tv_AfterSelect(object sender, TreeViewEventArgs e)
        {
           
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
