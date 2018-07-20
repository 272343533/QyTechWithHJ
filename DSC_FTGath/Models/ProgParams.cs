using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using SunMvcExpress.Common.Xml;
namespace HeatingDSC.Models
{
    //程序参数,多数需要设置，少数直接取出
    public class ProgParams
    {
        static public int dtutimeout;
        static public int IntervalBetweenCmds;
        static public int IntervalBetweenGath;
        static public int gprsComPort;


        static private int saveDataTimerInterval;
        static private int dataRemainDays;
 
        static private string serverName;
        static private string dbName;
        static public string saLogin;
        static private string sapassword;

        static public string appPath;
        static private int dtuprotocol;//0--UDP  1--TCP

        static private bool reStart;

        static public bool ReStart
        {
            get { return reStart; }
            set { reStart = value; }
        }

        static public int SaveDataTimerInterval
        {
            get { return saveDataTimerInterval; }
            set { saveDataTimerInterval = value; }
        }
      
        static public int DataRemainDays
        {
            get { return dataRemainDays; }
            set { dataRemainDays = value; }
        }
        static public int GprsComPort
        {
            get { return gprsComPort; }
            set { gprsComPort = value; }
        }
        static public string ServerName
        {
            get { return serverName; }
            set { serverName = value; }
        }
        static public string DbName
        {
            get { return dbName; }
            set { dbName = value; }
        }
        static public string SaLogin
        {
            get { return saLogin; }
            set { saLogin = value; }
        }
        static public string Sapassword
        {
            get { return sapassword; }
            set { sapassword = value; }
        }

        static public int dtuProtocol
        {
            get { return dtuprotocol; }
            set { dtuprotocol = value; }
        }//0--UDP  1--TCP



        //从XML文件读取参数
        public static void readParamsFromXMLFile()
        {
            dtutimeout = int.Parse(XmlConfig.GetValue("DTUTimeout"));
            IntervalBetweenCmds = int.Parse(XmlConfig.GetValue("CmdInterval"));
            gprsComPort = int.Parse(XmlConfig.GetValue("GPRSPort"));
            IntervalBetweenGath = int.Parse(XmlConfig.GetValue("QueryTimer"));
 
            
            serverName = XmlConfig.GetValue("HeatingIP");
            dbName = XmlConfig.GetValue("HeatingDataSource");
            saLogin = XmlConfig.GetValue("HeatingSqlLogin");
            sapassword = XmlConfig.GetValue("HeatingSqlPwd");
            
            saveDataTimerInterval = int.Parse(XmlConfig.GetValue("SaveTimer"));
            dataRemainDays = int.Parse(XmlConfig.GetValue("DataRemainDays"));
            dtuprotocol = int.Parse(XmlConfig.GetValue("DTUPotocolMode"));

            reStart = XmlConfig.GetValue("Restart") == "0" ? false : true;
        }


        //将参数写入XML文件
        public static bool writeParamsToXMLFile()
        {

           XmlConfig.SetValue("GPRSPort", string.Format("{0}", gprsComPort));
           XmlConfig.SetValue("QueryTimer", string.Format("{0}", IntervalBetweenGath));
           XmlConfig.SetValue("DTUTimeout", string.Format("{0}", dtutimeout));
           XmlConfig.SetValue("CmdInterval", string.Format("{0}", IntervalBetweenCmds));
        
           
            XmlConfig.SetValue("HeatingIP", serverName);
            XmlConfig.SetValue("HeatingDataSource", dbName);
            XmlConfig.SetValue("HeatingSqlLogin",saLogin);
            XmlConfig.SetValue("HeatingSqlPwd", sapassword);

            XmlConfig.SetValue("SaveTimer", string.Format("{0}", saveDataTimerInterval));
            XmlConfig.SetValue("DataRemainDays", string.Format("{0}", dataRemainDays));
            XmlConfig.SetValue("DTUPotocolMode", string.Format("{0}",dtuprotocol));
            return true;                
        }
    }
}



            
          