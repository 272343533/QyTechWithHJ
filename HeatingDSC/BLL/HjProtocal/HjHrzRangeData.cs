using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SunMvcExpress.Dao;
using QyTech.Communication;
using QyTech.HDGprs;
using QyTech.Protocal;

namespace HeatingDSC.BLL
{
    public class HjHrzRangeData : IProduct
    {
        //public clsHJGathData11 obj = new clsHJGathData11();
        public HjHrzRangeData()
        {
            Producttype = ProductType.GathRangeData_Hj;
        }
        public override int Parse(GPRS_DATA_RECORD recdPtr)
        {
            base.Parse(recdPtr);
            List<DetailDevice> hrzjzs = devda.SelectHrzJzDeviceBySim(recdPtr.m_userid);
            int resualt = 0;
            try
            {
                #region 解析数据对象
                HrzGathRange obj = new HrzGathRange();
                obj.GathDt = Convert.ToDateTime(recdPtr.m_recv_date);
                obj.Det_Id = hrzjzs[0].Id;
                obj.bsO_Id = Org.Id;
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0200-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F1Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0202-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F1Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0204-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F2Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0206-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F2Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0208-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F3Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x020A-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F3Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x020C-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F4Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x020E-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F4own = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0210-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F5Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0212-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F5Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0214-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F6Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0216-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F6Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0218-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F7Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x021A-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F7Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x021C-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F8Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x021E-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F8Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0220-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F9Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0222-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F9Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0224-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F10Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0226-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F10Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0228-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F11Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x022A-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F11Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x022C-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F12Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x022E-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F12Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0230-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F13Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0232-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F13Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0234-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F14Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0236-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F14Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0238-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F15Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x023A-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F15Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x023C-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F16Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x023E-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F16Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0240-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F17Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0242-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F17Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0244-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F18Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0246-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F18Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0248-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F19Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x024A-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F19Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x024C-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F20Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x024E-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F20Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0250-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F21Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0252-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F21Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0254-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F22Up = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0256-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F22Down = (decimal)BitConverter.ToSingle(buff, 0);
                buff = new byte[4];
                Buffer.BlockCopy(GetData, (0x0268-0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F23Up = (decimal)BitConverter.ToSingle(buff, 0);
                
                Buffer.BlockCopy(GetData, (0x026A - 0x0200) * 2, buff, 0, 4);
                CrossHiLow(ref buff);
                obj.F23Down = (decimal)BitConverter.ToSingle(buff, 0);


                using (WSExpressEntities db = new WSExpressEntities())
                {
                    db.AddToHrzGathRange(obj);
                    db.SaveChanges();
                }
               
                #endregion
                //OnHrzRangeGathProgress(obj);
                obj = null;
            }
            catch (Exception ex)
            {
                log.Error("hrzupunit parse:simno:" + recdPtr.m_userid + ":" + ex.InnerException + "-" + ex.Message);
                resualt = -1;
            }
            return resualt;
        }

        public override DeviceCmd CreateReadCommand(string simno)
        {
            return base.CreateModbusRtuReadCommand(simno, 0x01, 0x03, 0x200, 0x26A+1+1-0x200);
        }

        public byte[] Create(Guid orgid)
        {
            DeviceCmd cmd2send = new DeviceCmd();

            int len = (0x2BE - 0x280 + 1) * 2;
            byte[] data = new byte[len];
            int byteindex = 0;
            byte[] buff = new byte[4];


            using (var db = new WSExpressEntities())
            {
                vwlyGprsDevice simobj = db.vwlyGprsDevice.Where(u => u.bsO_Id == orgid).FirstOrDefault<vwlyGprsDevice>();
                if (simobj != null)
                {
                    cmd2send.CommNo = simobj.CommNo;
                    List<vwlyHjSetRange> dbobjs = db.vwlyHjSetRange.Where(u => u.Id == orgid).OrderBy(o => o.RangeSetNo).ToList<vwlyHjSetRange>();
                    //或者是按照协议地址进行存储，就是担心有些项没有数据

                    for (int i = 0; i < dbobjs.Count; i++)// (AlarmRangeSet ars in dbobjs)
                    {
                        vwlyHjSetRange ars = dbobjs[i];
                        buff = BitConverter.GetBytes((float)ars.RangeUp);
                        CrossHiLow(ref buff);
                        byteindex = Convert.ToInt16(ars.RangeSetNo.ToString(), 16) - 0x200;
                        Buffer.BlockCopy(buff, 0, data, byteindex, 4);
                    }
                }
                return data;
            }

        }

    }
}
