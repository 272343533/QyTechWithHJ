using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using QyTech.HDGprs;
using SunMvcExpress.Dao;
using DAHeating.DataAccess;
namespace HeatingDSC.BLL
{
    /// <summary>
    /// 目前该函数已经不在使用，改为DownhrzControl class
    /// </summary>
    public class ParseHrzDownCurve:IProduct
    {
        public DownHrzCurve downHrzcurve_;

        public ParseHrzDownCurve()
        {
            Producttype = ProductType.HrzDownCurveAll;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj">DownHrzCurve</param>
        /// <returns>要下发的字节数组</returns>
        public override byte[] Create(object obj)
        {
            // base.Create(obj);
//26	0x0100	曲线比例系数	Single Float	2	Read/Write
//27	0x0102	曲线调节增量	Single Float	2	Read/Write
//28	0x0104	曲线中坐标点数	Single int 	1	Read/Write
//29	0x0105	室外温度最低值	Single int	1	Read/Write
//30	0x0106	室外温度变化步长	Single int 	1	Read/Write
//31	0x0107	1#点二次网温度	Single int 	1	Read/Write
//32	0x0108	2#点二次网温度	Single int	1	Read/Write
//33	0x0109	…	…	…	…
//34	0x017E	120#点二次网温度	Single int 	1	Read/Write
            //判断该组织结构是否有曲线需要下发
             int qxwd;
            downHrzcurve_=(DownHrzCurve)obj;
            if (downHrzcurve_.dcsetting == null)
                return null;

            //2:系数 2：增量 1：坐标点 1 最低值 1步长 120个点/2

            byte[] data;// = new byte[134];//[134];  //(2+2+3+60)*2             //old:(120+3+2*3)*2
            int byteindex = 0;
            data = new byte[4];
            if (downHrzcurve_.dcsetting.SetFlag==2)
            {
                data = new byte[4];
                data = BitConverter.GetBytes((float)downHrzcurve_.dcsetting.RunCurveScale);
                CrossHiLow(ref data);
            }
            else if (downHrzcurve_.dcsetting.SetFlag == 4)
            {
                data = BitConverter.GetBytes((float)downHrzcurve_.dcsetting.RunCurveOffset);
                CrossHiLow(ref data);
            }
            else if (downHrzcurve_.dcsetting.SetFlag == 1)
            {
                //return data;
                byte[] buff = new byte[4];
                data = new byte[126];
                byteindex = 0;
                buff = new byte[2];
                buff = BitConverter.GetBytes((short)downHrzcurve_.rc.CurevePointCount);
                CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, data, byteindex, 2);

                //预留字节
                buff = BitConverter.GetBytes((short)0);//downHrzcurve_.rc.MInWD);
                CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);
                buff = BitConverter.GetBytes((short)0);
                CrossHiLow(ref buff);
                Buffer.BlockCopy(buff, 0, data, byteindex += 2, 2);

                byteindex = 6;
                string[] wds = downHrzcurve_.rc.CurveHeatringWDS.Split(new char[] { ',' });
                for (int i = 0; i < downHrzcurve_.rc.CurevePointCount; i++)
                {

                    //Buffer.BlockCopy(BitConverter.GetBytes(Convert.ToByte(wd)), 0, data, byteindex, 2); 
                    data[byteindex + i * 2] = Convert.ToByte(wds[i]);
                    qxwd = (int)(downHrzcurve_.rc.OutWdMin + downHrzcurve_.rc.OutWdStep * i);
                    if (qxwd >= 0)
                        data[byteindex + i * 2 + 1] = (byte)qxwd;
                    else
                    {
                        data[byteindex + i * 2 + 1] = Convert.ToByte(256 + qxwd);//128+downHrzcurve_.rc.MInWD + downHrzcurve_.rc.WDStep * i);
                    }
                    if ((byteindex + i * 2) >= data.Length)//(14 + 120 * 2))
                        break;
                }

                //如果点的数量不足120个，则后续数据补0
                if (downHrzcurve_.rc.CurevePointCount < 60)
                {
                    for (int i = 0; i < 60 - downHrzcurve_.rc.CurevePointCount; i++)
                    {
                        data[133 - i] = 0;
                    }
                }
            }
            return data;
        }


        /// <summary>
        /// 曲线配置下发完毕后更改标识
        /// </summary>
        /// <param name="recdPtr"></param>
        /// <returns></returns>
        public override int SentData(GPRS_DATA_RECORD recdPtr)
        {
            //根据电话号码，地址，决定做什么
            //把对应的数据的1改为0，表示已经发送
            try
            {
                base.Parse(recdPtr);
                hrzda.UpdateTTRunCurveConf(Org.Id, RegStartAddr, RegCount);
            }
            catch (Exception ex)
            {
                log.Error("HrzDownCurve-SentData"+ex.InnerException+"-"+ex.Message);
                return 1;
            }
            return 0;
        }
       
    }
}
