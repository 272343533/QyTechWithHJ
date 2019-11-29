using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DAHeating;
using DAHeating.BLL;
using HeatingDSC.Models;
using HeatingDSC.BLL;
using HeatingDSC.DLL;
using log4net;

namespace HeatingDSC.UI
{
    public partial class frmControl : Form
    {
        public static ILog log = log4net.LogManager.GetLogger("frmcontrol");

        public frmStart frmstart;

        EmpOrganize Org;

        public frmControl()
        {
            InitializeComponent();
        }

        HrzDa hrzda=new HrzDa();
        OrganizeDa orgda = new OrganizeDa();
        SimcardDa simda=new SimcardDa();
        List<HRZRunCurve> curs;
        Device d;//当前设备
        private void frmControl_Load(object sender, EventArgs e)
        {
            this.ucTvDevice1.RefreshTreeAllDevice();
            this.ucTvDevice1.delNodeMouseClickHandler+=new HeatingDSC.Controls.delNodeMouseClick(ucTvDevice1_delNodeMouseClickHandler);
            curs = hrzda.GetAllCurve();
            foreach (HRZRunCurve hrc in curs)
            {
                cboCurve.Items.Add(hrc.RunCurveName);
            }
            cboCurve.SelectedIndex = 0;
            cboCurve_SelectionChangeCommitted(null, null);
          
        }

        private void ucTvDevice1_delNodeMouseClickHandler(int orgid)
        {
            Org = orgda.SelectOrganization(orgid);
        }

        private void GetWeatherData()
        {
            if (Org != null)
            {
                float[] wea = orgda.SelectOrgWeather(Org.OrganizeID);

                this.textBox4.Text = wea[0].ToString();
                this.textBox5.Text = wea[1].ToString();
                this.textBox6.Text = wea[2].ToString();
            }
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            
            
        }

        private void cboCurve_SelectionChangeCommitted(object sender, EventArgs e)
        {
            int index = cboCurve.SelectedIndex;
            this.dataGridView1.Rows.Clear();
            
            string[,] wds = new string[2, (int)curs[index].CurevePointCount + 1];
            string[] wdvs = curs[0].CurveHeatringWDS.Split(new char[] { ',' });
            this.dataGridView1.Rows.Add(2);
            this.dataGridView1.Rows[0].Cells[0].Value = "环境温度(℃)";
            this.dataGridView1.Rows[1].Cells[0].Value = "供热温度(℃)";
            for (int i = 1; i <= wdvs.Length; i++)
            {
                this.dataGridView1.Rows[0].Cells[i].Value = (curs[0].MInWD + curs[0].WDStep * (i - 1)).ToString();
                this.dataGridView1.Rows[1].Cells[i].Value = wdvs[i - 1];
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //this.ucTvDevice1.OrgId; 
            //curs[cboCurve.SelectedIndex].HRZRunCurveID
            //创建命令
            d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            SIMCardInfo siminfo;
            try
            {
                siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
                cmd2send = new DeviceCmd();
                cmd2send.DeviceSimNo = siminfo.SIMCardNo;

                string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x24;//首地址16进制
                ModbusCommand.RegOpNum = 63;//字节数量10进制

                PacketFac ppf = new PacketFac();
                IProduct product = ppf.CreatePacket(ProductType.hrzDownCurveOnly);
                byte[] sendData = product.Create(d.AZWZ);
                ModbusCommand.byteArr = null;
                if (sendData != null)
                {
                    byte[] temp = new byte[126];
                    Buffer.BlockCopy(sendData, 8, temp, 0, 126);
                    ModbusCommand.byteArr = temp;
                    ModbusCommand.byteArr[7] = 0xFF;
                    log.Error("asdfasdfasdf"+d.DeviceName);
                }
                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.sendcmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.command = strbuilder.ToString();
                    }

                    int ret = frmstart.SendCommand(cmd2send);
                       if (ret == 0)
                    {
                        MessageBox.Show("发送成功");
                    }
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("buildcommandforsetting:" + d.DeviceID.ToString() + "-" + ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
           
        }

        private void GetKD(int hrzid)
        {
            d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            SIMCardInfo siminfo;
            try
            {
                siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
                cmd2send = new DeviceCmd();
                cmd2send.DeviceSimNo = siminfo.SIMCardNo;

                string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x24;//首地址16进制
                ModbusCommand.RegOpNum = 67;//字节数量10进制

                PacketFac ppf = new PacketFac();
                IProduct product = ppf.CreatePacket(ProductType.hrzDownCurveOnly);
                byte[] sendData = product.Create(d.AZWZ);
                ModbusCommand.byteArr = null;
                if (sendData != null)
                {
                    byte[] temp = new byte[126];
                    Buffer.BlockCopy(sendData, 8, temp, 0, 126);
                    ModbusCommand.byteArr = temp;
                }
                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.sendcmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.command = strbuilder.ToString();
                    }

                    int ret = frmstart.SendCommand(cmd2send);
                    if (ret == 0)
                    {
                        MessageBox.Show("发送成功");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("buildcommandforsetting:" + d.DeviceID.ToString() + "-" + ex.Message);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
        }

        private void trRefresh_Tick(object sender, EventArgs e)
        {
            //try
            //{
            //    GetWeatherData();

                //d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
                //byte[] commdarr = new byte[9];
                //DeviceCmd cmd2send;
                //SIMCardInfo siminfo;
                //try
                //{
                //    siminfo = simda.SelectSIMCardInfo((int)d.simcardid);

                //    foreach (DeviceCmd cmd in frmstart.GatherCommandHrz)
                //    {
                //        if (cmd.DeviceSimNo == siminfo.SIMCardNo)
                //        {
                //            cmd2send = cmd;
                //            int ret=frmstart.SendCommand(cmd2send);
                //            log.Info("trRefresh_tick:ret=" +ret.ToString()+" "+ cmd2send.DeviceSimNo + "-" + cmd2send.command);
                //            break;
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);

                //log.Info("frmcontrol: organize name:" + ParseHrzUpCommData.ORG.OrganizeName);

            //    if (ParseHrzUpCommData.ORG != null)
            //    {
            //        if (chkGD.Checked)
            //        {
            //            if (ParseHrzUpCommData.ORG.OrganizeID == this.ucTvDevice1.OrgId)
            //            {
            //                this.textBox1.Text = ParseHrzUpCommData.FMKD.ToString();
            //                this.textBox3.Text = ParseHrzUpCommData.DdtjfAutoContFlag == 0 ? "手动" : "自动";
            //                this.textBox2.Text = ParseHrzUpCommData.P.ToString();
            //                this.textBox7.Text = ParseHrzUpCommData.I.ToString();
            //                this.label13.Text = ParseHrzUpCommData.ORG.OrganizeName;
            //            }
            //        }
            //        else
            //        {
            //            this.textBox1.Text = ParseHrzUpCommData.FMKD.ToString();
            //            this.textBox3.Text = ParseHrzUpCommData.DdtjfAutoContFlag == 0 ? "手动" : "自动";
            //            this.textBox2.Text = ParseHrzUpCommData.P.ToString();
            //            this.textBox7.Text = ParseHrzUpCommData.I.ToString();
            //            this.label13.Text = ParseHrzUpCommData.ORG.OrganizeName;

            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    log.Error("trRefresh_Tick:" + ex.InnerException + "-" + ex.Message);
            //}
 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            SIMCardInfo siminfo;
            try
            {
                siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
                cmd2send = new DeviceCmd();
                cmd2send.DeviceSimNo = siminfo.SIMCardNo;

                string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x12;//首地址16进制
                ModbusCommand.RegOpNum = 4;//字节数量10进制

                PacketFac ppf = new PacketFac();
                IProduct product = ppf.CreatePacket(ProductType.hrzDownCurveOnly);
                byte[] sendData = BitConverter.GetBytes(Convert.ToSingle(this.txtKD.Text));
                ModbusCommand.byteArr = sendData;
              
                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.sendcmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.command = strbuilder.ToString();
                    }

                    int ret = frmstart.SendCommand(cmd2send);
                    if (ret == 0)
                    {
                        MessageBox.Show("发送成功");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("buildcommandforsetting:" + d.DeviceID.ToString() + "-" + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            SIMCardInfo siminfo;
            try
            {
                siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
                cmd2send = new DeviceCmd();
                cmd2send.DeviceSimNo = siminfo.SIMCardNo;

                string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x26;//首地址16进制
                ModbusCommand.RegOpNum = 2;//字节数量10进制

                PacketFac ppf = new PacketFac();
                IProduct product = ppf.CreatePacket(ProductType.hrzDownCurveOnly);
                byte[] sendData = BitConverter.GetBytes((short)cboAutoOrNot.SelectedIndex);
                ModbusCommand.byteArr = sendData;

                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.sendcmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.command = strbuilder.ToString();
                    }

                    int ret = frmstart.SendCommand(cmd2send);
                    if (ret == 0)
                    {
                        MessageBox.Show("发送成功");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("buildcommandforsetting:" + d.DeviceID.ToString() + "-" + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            d = hrzda.SelectHrzGprsDevice(this.ucTvDevice1.OrgId);
            byte[] commdarr = new byte[9];
            DeviceCmd cmd2send;
            SIMCardInfo siminfo;
            try
            {
                siminfo = simda.SelectSIMCardInfo((int)d.simcardid);
                cmd2send = new DeviceCmd();
                cmd2send.DeviceSimNo = siminfo.SIMCardNo;

                string[] sets = d.CommProtocal.CPCode.Split(new char[] { ',', ';' });
                ModbusCommand.VerifyMode = ModbusCommand.RTU;
                ModbusCommand.Slaveaddr = Convert.ToByte(sets[0]);
                ModbusCommand.OperatMode = 0x10;
                ModbusCommand.RegStartAddr = 0x14;//首地址16进制
                ModbusCommand.RegOpNum = 4;//字节数量10进制

                PacketFac ppf = new PacketFac();
                IProduct product = ppf.CreatePacket(ProductType.hrzDownCurveOnly);
                byte[] sendData = new byte[8];
                byte[] buff = BitConverter.GetBytes(Convert.ToSingle(this.txtP.Text));
                CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, sendData, 0, 4);
                buff = BitConverter.GetBytes(Convert.ToSingle(this.txtI.Text));
                CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, sendData, 4, 4);


                ModbusCommand.byteArr = sendData;

                if (ModbusCommand.byteArr != null)//如果有下发的数据
                {
                    commdarr = ModbusCommand.Command();
                    cmd2send.sendcmd = commdarr;
                    //生成命令数组
                    if (commdarr != null)
                    {
                        StringBuilder strbuilder = new StringBuilder();
                        foreach (byte cmdbyte in commdarr)
                        {
                            strbuilder.Append(cmdbyte.ToString("X2"));
                        }
                        cmd2send.command = strbuilder.ToString();
                    }

                    int ret = frmstart.SendCommand(cmd2send);
                    if (ret == 0)
                    {
                        MessageBox.Show("发送成功");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("buildcommandforsetting:" + d.DeviceID.ToString() + "-" + ex.Message);
            }
        }

        protected void CrossHiLow(ref byte[] b)
        {
            byte tmp;
            for (int i = 0; i < b.Length / 2; i++)
            {
                tmp = b[i];
                b[i] = b[b.Length - 1 - i];
                b[b.Length - 1 - i] = tmp;
            }
        }

        public void RefreshHrzCommNet(string simno, EmpOrganize org, float kd, int autostatus, float p, float I, float wd, float rz, float fs)
        {
            //if (chkGD.Checked)
            //{
            //    if (ParseHrzUpCommData.ORG.OrganizeID == this.ucTvDevice1.OrgId)
            //    {
            //        //this.label13.Text = org.OrganizeName;
            //        this.txtP.Text = p.ToString();
            //        this.txtI.Text = I.ToString();
            //        this.txtKD.Text = kd.ToString();
            //        this.textBox4.Text = wd.ToString();
            //        this.textBox5.Text = fs.ToString();
            //        this.textBox6.Text = rz.ToString();
            //    }
               
            //}
            //else
            //{
            //    this.label13.Text = org.OrganizeName;
            //    this.txtP.Text = p.ToString();
            //    this.txtI.Text = I.ToString();
            //    this.txtKD.Text = kd.ToString();
            //    this.textBox4.Text = wd.ToString();
            //    this.textBox5.Text = fs.ToString();
            //    this.textBox6.Text = rz.ToString();
            //}
            //this.Refresh();
            //Application.DoEvents();
            //this.Refresh();
        }
    }
}
