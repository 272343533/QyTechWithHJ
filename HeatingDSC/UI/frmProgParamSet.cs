using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HeatingDSC.Models;


using SunMvcExpress.Dao;

using System.Diagnostics;

////using HeatingDSC.BLL;
using HeatingDSC.UI;
using QyTech.Communication;
using log4net;
using ChenDong.XmlConfigFile;


namespace HeatingDSC.UI
{
    public partial class frmProgParamSet : Form
    {
        public static ILog log = log4net.LogManager.GetLogger("ParamsSet");

        private int m_IntervalBetweenGath;
        private int m_gprsComPort;
        private int m_IntervalBetweenCmds;
        private int m_dtutimeout;

        private int m_saveDataTimerInterval;
        private int m_dataRemainDays;
        

        private string m_serverName;
        private string m_dbname;
        private string m_salogin;
        private string m_sapassword;

        public frmProgParamSet()
        {
            InitializeComponent();
        }
        //窗口加载
        private void frmProgParamSet_Load(object sender, EventArgs e)
        {
            try
            {
                updqueryTimer.Value = ProgParams.IntervalBetweenGath;
                nudCmdInterval.Value = ProgParams.IntervalBetweenCmds;//
                updgprsComPort.Value = ProgParams.GprsComPort;
                nudDtuTimeOut.Value = ProgParams.dtutimeout;
            }
            catch (System.ArgumentOutOfRangeException ex)
            {
                //strmessage=string.Format("查询数据时间间隔超出范围，正常值应该是 {0} 到 {1} 之间。", 20, 40);
                MessageBox.Show("数据超出范围");
            }
         

            saveCurParams();//暂存
        }

        //退出
        private void button2_Click(object sender, EventArgs e)
        {
            rollBackParamSet();
            this.Hide();       //关闭窗体
            //this.Dispose();   //释放所有资源
        }

        #region 参数设置
        private void btnSave_Click(object sender, EventArgs e)
        {

            appParams();
            if (ProgParams.writeParamsToXMLFile())
            {
                MessageBox.Show("程序设置参数已应用并保存");
            }
            else
            {
                MessageBox.Show("程序设置参数保存保存失败");
                rollBackParamSet();
            }

        }


        /// <summary>
        /// 暂存设置参数
        /// </summary>
        private void saveCurParams()
        {

            m_dtutimeout = ProgParams.dtutimeout;
            m_IntervalBetweenCmds = ProgParams.IntervalBetweenCmds;
            m_IntervalBetweenGath = ProgParams.IntervalBetweenGath;
            m_gprsComPort = ProgParams.GprsComPort;
            
            
            m_saveDataTimerInterval = ProgParams.SaveDataTimerInterval;
            m_dataRemainDays = ProgParams.DataRemainDays;
            m_serverName = ProgParams.ServerName;
            m_dbname = ProgParams.DbName;
            m_salogin = ProgParams.SaLogin;
            m_sapassword = ProgParams.Sapassword;

        
        }

        
        /// <summary>
        /// 设置回滚
        /// </summary>
        private void rollBackParamSet()
        {
            ProgParams.IntervalBetweenGath = m_IntervalBetweenGath;
            ProgParams.GprsComPort=m_gprsComPort;
            ProgParams.IntervalBetweenCmds = m_IntervalBetweenCmds;
            ProgParams.dtutimeout = m_dtutimeout;
            
            ProgParams.SaveDataTimerInterval = m_saveDataTimerInterval;
            ProgParams.DataRemainDays = m_dataRemainDays;
            
            ProgParams.ServerName=m_serverName;
            ProgParams.DbName = m_dbname;
            ProgParams.SaLogin = m_salogin;
            ProgParams.Sapassword=m_sapassword;
         
        }

        /// <summary>
        /// 应用参数
        /// </summary>
        private void appParams()
        {

            ProgParams.GprsComPort = (int)updgprsComPort.Value;
            ProgParams.IntervalBetweenGath = (int)updqueryTimer.Value;
            ProgParams.dtutimeout =(int)nudDtuTimeOut.Value;
            ProgParams.IntervalBetweenCmds = (int)nudCmdInterval.Value;
        }
     
        #endregion

    }
}
