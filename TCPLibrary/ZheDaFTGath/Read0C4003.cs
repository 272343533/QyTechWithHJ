using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SunMvcExpress.Dao;
//using QyTech.Core;
//using QyTech.Core.BLL;
using SunMvcExpress.Core;
using SunMvcExpress.Core.BLL;


using log4net;

namespace DSC_FTHj1
{

    public class Read0C4003 : ZdFT_Packet
    {

       /// <summary>
       /// 热表数据读取
       /// </summary>
        public Read0C4003():base()
        {
        }

        //public override byte[] Create(string data)
        //{
        //    return base.CreateAsc(data);
        //}

        public override int Parse(byte[] bytes)
        {
            HUserGathData obj = new HUserGathData();

            try
            {
                byte[] buff = new byte[2];
                Buffer.BlockCopy(bytes, 0, buff, 0, 2); Array.Reverse(buff);
                //CrossHiLow(ref buff);

                obj.BathNo = (int)BitConverter.ToInt16(buff, 0);

                buff = new byte[5]; Buffer.BlockCopy(bytes, 4, buff, 0, 5); Array.Reverse(buff);
                obj.DataDt = BitConverter.ToString(buff, 0);
                obj.DataDt = "20" + obj.DataDt.Substring(0, 8) + " " + obj.DataDt.Substring(9, 2) + ":" + obj.DataDt.Substring(12, 2);

                Buffer.BlockCopy(bytes, 9, buff, 0, 5); Array.Reverse(buff);
                obj.DevCode = BitConverter.ToString(buff, 0).Replace("-", "");



                buff = new byte[4]; Buffer.BlockCopy(bytes, 14, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_1 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 18, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_2 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 22, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_3 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 26, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_4 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 30, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_5 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 34, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_6 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 38, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_7 = BitConverter.ToSingle(buff, 0).ToString();

                buff = new byte[2]; Buffer.BlockCopy(bytes, 42, buff, 0, 2); //Array.Reverse(buff);
                obj.CommonVar20_8 = BitConverter.ToInt16(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 44, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_9 = BitConverter.ToString(buff, 0).Replace("-", "");
             

                obj.GathDt = DateTime.Now;
                //log.Error("Parse:0c4003:"+devNo);
                 obj.DataType = CommFlag+FunDesp;


                //并直接保存到数据中
                EntityManager<HUserGathData>.Add<HUserGathData>(obj);
            }
            catch (Exception ex)
            {
                log.Error("ReadData.Parse" + ex.Message);
                return 0;
            }
            return 1;
    
        }
    }
}
