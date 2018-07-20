using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeatingDSC.Models
{
    //站点类
    public class Stations
    {
        private string customerID;       //客户ID        static string varchar;//(23) NOT NULL,
	    private string stationID;        //站点ID        static string varchar;//(23) NOT NULL,
	    private string sSDW_DM;          //单位代码       static string char;//(4) NULL,
	    private string stationName;      //站点名称       static string varchar;//(255) NOT NULL,
	    private string stationType_DM;   //站点类型代码    static string char;//(4) NOT NULL,
	    private string protocol_DM;      //通信协议代码    static string char;//(4) NOT NULL,
	    private string contact;          //联系人         static string varchar;//(255) NULL,
	    private string phoneNumber;      //联系电话      static string varchar;//(255) NULL,
	    private string email;            //电子邮件 static string varchar;//(255) NULL,
	    private string addr;             //地址 static string varchar;//(255) NULL,
	    private DateTime regDate;        //注册日期 static string datetime;// NOT NULL,
	    private string dTUID;            //DTU编号 static string varchar;//(23) NOT NULL,
	    private string simNo;            //sim卡号 static string char;//(11) NULL,
	    private bool   isOnLine;// static string bit;// NOT NULL,
	    private bool isAlarming;// static string bit;// NOT NULL,
	    private string lastOnLineTime;// static string datetime;// NULL,
	    private string currentFMID;// static string varchar;//(23) NULL,
	    private string remark;// static string varchar;//(255) NULL,
	    //private string stflowpic;// static string nvarchar;//(50) NULL,
	    private DateTime simExpireDate;// static string datetime;// NULL,
        private string commid;


        public Stations() { }//构造函数

        public string CustomerID
        {
            get {return customerID;}
            set {customerID=value;}
        }

        public string StationID
        {
            get {return stationID;}
            set {stationID=value;}
        }

        public string SSDW_DM 
        {
            get {return sSDW_DM;}
            set {sSDW_DM=value;}
        }

        public string StationName
        {
            get {return stationName;}
            set {stationName=value;}
        }

        public string StationType_DM
        {
            get { return stationType_DM; }
            set { stationType_DM = value; }
        }

        public string Protocol_DM
        {
            get { return protocol_DM; }
            set { protocol_DM = value; }
        }

        public string Contact
        {
            get { return contact; }
            set {contact=value;}
        }

        public string PhoneNumber
        {
            get {return phoneNumber;}
            set {phoneNumber=value;}
        }

        public string Email
        {
            get {return email;}
            set {email=value;}
        }

        public string Addr{
            get {return addr;}
            set {addr=value;}
        }

        public DateTime RegDate{
            get {return regDate;}
            set {regDate=value;}
        }

        public string DTUID{
            get {return dTUID;}
            set {dTUID=value;}
        }

        public string SimNo{
            get {return simNo;}
            set {simNo=value;}
        }

        public bool IsOnLine{
            get {return isOnLine;}
            set {isOnLine=value;}
        }

        public bool IsAlarming{
            get {return isAlarming;}
            set {isAlarming=value;}
        }

        public string LastOnLineTime{
            get {return lastOnLineTime;}
            set {lastOnLineTime=value;}
        }

        public string CurrentFMID{
            get {return currentFMID;}
            set {currentFMID=value;}
        }

        public string Remark{
            get {return remark;}
            set {remark=value;}
        }

        /*
        public string Stflowpic{
            get {return Stflowpic;}
            set {stflowpic=value;}
        }
         * */

        public DateTime SimExpireDate
        {
            get { return simExpireDate; }
            set { simExpireDate = value; }
        }


        public string CommID
        {
            get { return commid; }
            set { commid = value; }
        }



    }
}
