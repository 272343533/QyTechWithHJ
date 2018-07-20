using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using QyTech.InHandDtu;
using log4net;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "log4net.config", Watch = true)]
namespace TestInHand
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public const int USER = 0x0400;//用户自定义消息的开始数值
        [DllImport("user32.dll")]
        public static extern void PostMessage(IntPtr hWnd, int msg, int wParam, int lParam);

        InhandDtuDll inhandServ = InhandDtuDll.Instance;

        public static ILog log = log4net.LogManager.GetLogger("frmstart");

        private void Form1_Load(object sender, EventArgs e)
        {
          
        }
        //protected override void DefWndProc(ref Message m)
        //{
        //    if (m.Msg < 1024)
        //    {
        //        base.DefWndProc(ref m);
        //        return;
        //    }
        //    //string strlog = "DefWndProc数据来了:" + m.Msg.ToString() + ":" + m.LParam.ToString() + "-1:" + m.WParam.ToString() + "-2:" + m.Result.ToString() + "-3:" + m.HWnd.ToString();
        //   // textBox1.Text += strlog + "\r\n";
         
        //    base.DefWndProc(ref m);
        //}
   

        protected override void WndProc(ref Message m)
        {
           if (m.Msg < 1024)
            {
                base.WndProc(ref m);
                return;
            }
            // TODO:  添加 Form1.WndProc 实现
         
            if (m.Msg == InhandDtuDll.WM_DTU)
            {

                string strlog = "映翰通数据来了:" + m.Msg.ToString() + ":" + m.LParam.ToString() + "-1:" + m.WParam.ToString() + "-2:" + m.Result.ToString() + "-3:" + m.HWnd.ToString();
                textBox1.Text = strlog + "\r\n"+textBox1.Text;

                InHand_DATA_RECORD idr = new InHand_DATA_RECORD();
                bool reply = false;
                StringBuilder mess = new StringBuilder(1000);
                int ret = InhandDtuDll.do_read_proc(ref idr, mess, reply);
                string strType = idr.m_data_type == 1 ? "心跳包" : idr.m_data_type == 2 ? "退出包" : idr.m_data_type == 3 ? "登录包" : idr.m_data_type == 9 ? "数据包" : "其它包";
                textBox1.Text = "分析结果：" + strType + ":" + idr.m_userid + ":" + mess + "\r\n" + textBox1.Text;
                switch ((int)idr.m_data_type)
                {

                    case 2://退出
                        break;
                    case 3:
                        InHand_USER_INFO iui = new InHand_USER_INFO();
                        byte[] byteuser = Encoding.Default.GetBytes(idr.m_userid);
                        InhandDtuDll.get_user_info(byteuser, ref iui);

                        textBox1.Text ="登录信息:"+ iui.m_userid+"-"+iui.m_logon_date + "\r\n" + textBox1.Text;
              
                        break;
                    case 9://数据
                        for (int i = 0; i < idr.m_data_len; i++)
                        {
                            textBox1.Text += idr.m_data_buf[i].ToString("X2");;
                        }
                        textBox1.Text = "\r\n"+textBox1.Text;
                        break;
                }
            }
            else
            {
                base.WndProc(ref m);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StringBuilder mess = new StringBuilder(1000);
            bool flag = inhandServ.StartService(this.Handle,uint.Parse(this.txtAddDtuWM.Text),int.Parse(this.txtPort.Text),mess);
            if (flag)
                textBox1.Text = mess + "\r\n" + textBox1.Text; ;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            StringBuilder mess = new StringBuilder();
            bool flag = inhandServ.StopService(ref mess);
            if (flag)
                textBox1.Text = mess + "\r\n" + textBox1.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int count = InhandDtuDll.get_online_user_amount();
            txtUsers.Text += "在线数量" + count.ToString() + "\r\n";
            if (count > 0)
            {
                InHand_USER_INFO iui = new InHand_USER_INFO();
                for (uint i = 0; i < count; i++)
                {

                    InhandDtuDll.get_user_at(i, ref iui);
                    
                    txtUsers.Text += iui.m_userid+"\t"+Convert.ToUInt32(iui.m_update_time.Substring(0,4)).ToString()+":"+iui.m_status.ToString()+":"+iui.m_logon_date;
               }
            }
                 
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int count = InhandDtuDll.get_online_user_amount();
            txtUsers.Text = "";
            if (count > 0)
            {
                InHand_USER_INFO iui=new InHand_USER_INFO();
                for (uint i = 0; i < count; i++)
                {
                    
                    InhandDtuDll.get_user_at(i,ref iui);
                    txtUsers.Text+=iui.m_userid;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder mess = new StringBuilder(1000);
                bool ret = inhandServ.SendCommand(this.txtSendUser.Text, this.txtSend.Text, mess);
                MessageBox.Show(mess.ToString());
                textBox1.Text = "发送" + (ret ? "成功" : "失败") + "\r\n" +textBox1.Text;
       
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            InHand_USER_INFO iui = new InHand_USER_INFO();
            try
            {
                byte[] byteuser = Encoding.Default.GetBytes(this.txtSendUser.Text);
                int ret = InhandDtuDll.get_user_info(byteuser, ref iui);
                if (ret == 0)
                {
                    txtUsers.Text += iui.m_userid + ":" + iui.m_status.ToString() + ":" + iui.m_logon_date;
             
                    txtUsers.Text += iui.m_userid + "\t" + iui.m_update_time.Length.ToString()+"-"+iui.m_update_time.ToString() + ":" + iui.m_status.ToString() + ":" + iui.m_logon_date;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
            
        
    }
}
