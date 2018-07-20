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

    public class Read0C2003 : ZdFT_Packet
    {


        /// <summary>
        /// 阀门数据读取
        /// </summary>
        public Read0C2003()
            : base()
        {
        }

  
        //把数据部分解析为对象
        public override int Parse(byte[] bytes)
        {

            HUserGathData obj = new HUserGathData();

            try
            {
                byte[] buff = new byte[4];
                Buffer.BlockCopy(bytes, 0, buff, 0, 4); //Array.Reverse(buff);
                //CrossHiLow(ref buff);

                obj.BathNo = (Int32)BitConverter.ToSingle(buff, 0);

                buff = new byte[5]; Buffer.BlockCopy(bytes, 4, buff, 0, 5); Array.Reverse(buff);
                obj.DataDt = BitConverter.ToString(buff, 0);
                obj.DataDt = "20" + obj.DataDt.Substring(0, 8) + " " + obj.DataDt.Substring(9, 2) + ":" + obj.DataDt.Substring(12, 2);

                Buffer.BlockCopy(bytes, 9, buff, 0, 5); Array.Reverse(buff);
                obj.DevCode = BitConverter.ToString(buff, 0).Replace("-", "");



                buff = new byte[4]; Buffer.BlockCopy(bytes, 14, buff, 0, 4); //Array.Reverse(buff);
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
                
                obj.CommonVar20_6 = BitConverter.ToUInt32(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 38, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_7 = BitConverter.ToUInt32(buff, 0).ToString();

                //buff = new byte[2]; Buffer.BlockCopy(bytes, 42, buff, 0, 2); //Array.Reverse(buff);
                //obj.CommonVar20_8 = (BitConverter.ToInt16(buff, 0)*0.01).ToString();
                //buff = new byte[2]; Buffer.BlockCopy(bytes, 44, buff, 0, 2); //Array.Reverse(buff);
                //obj.CommonVar20_8 = BitConverter.ToInt16(buff, 0).ToString();


                //buff = new byte[4]; Buffer.BlockCopy(bytes, 46, buff, 0, buff.Length);// Array.Reverse(buff);
                //obj.CommonVar20_9 = BitConverter.ToSingle(buff, 0).ToString();
                //buff = new byte[4]; Buffer.BlockCopy(bytes, 50, buff, 0, buff.Length);// Array.Reverse(buff);
                //obj.CommonVar20_10 = BitConverter.ToSingle(buff, 0).ToString();
                //buff = new byte[4]; Buffer.BlockCopy(bytes, 54, buff, 0, buff.Length);// Array.Reverse(buff);
                //obj.CommonVar20_11 = BitConverter.ToSingle(buff, 0).ToString();
                //buff = new byte[4]; Buffer.BlockCopy(bytes, 58, buff, 0, buff.Length);// Array.Reverse(buff);
                //obj.CommonVar20_12 = BitConverter.ToSingle(buff, 0).ToString();
                //buff = new byte[4]; Buffer.BlockCopy(bytes, 62, buff, 0, buff.Length);// Array.Reverse(buff);
                //obj.CommonVar20_13 = BitConverter.ToSingle(buff, 0).ToString();
                //buff = new byte[4]; Buffer.BlockCopy(bytes, 66, buff, 0, buff.Length);// Array.Reverse(buff);
                //obj.CommonVar20_14 = BitConverter.ToSingle(buff, 0).ToString();


                //buff = new byte[1]; Buffer.BlockCopy(bytes, 70, buff, 0, 1);// Array.Reverse(buff);
                //obj.CommonVar20_15 =buff[0].ToString();
             


                //buff = new byte[4]; Buffer.BlockCopy(bytes, 44, buff, 0, 4);// Array.Reverse(buff);
                //obj.CommonVar20_9 = BitConverter.ToString(buff, 0).Replace("-", "");


                buff = new byte[2]; Buffer.BlockCopy(bytes, 42, buff, 0, 2); //Array.Reverse(buff);
                obj.CommonVar20_8 = (BitConverter.ToInt16(buff, 0) * 0.01).ToString();
                buff = new byte[2]; Buffer.BlockCopy(bytes, 44, buff, 0, 2); //Array.Reverse(buff);
                obj.CommonVar20_9 = BitConverter.ToInt16(buff, 0).ToString();


                buff = new byte[4]; Buffer.BlockCopy(bytes, 46, buff, 0, buff.Length);// Array.Reverse(buff);
                obj.CommonVar20_10 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 50, buff, 0, buff.Length);// Array.Reverse(buff);
                obj.CommonVar20_11 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 54, buff, 0, buff.Length);// Array.Reverse(buff);
                obj.CommonVar20_12 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 58, buff, 0, buff.Length);// Array.Reverse(buff);
                obj.CommonVar20_13 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 62, buff, 0, buff.Length);// Array.Reverse(buff);
                obj.CommonVar20_14 = BitConverter.ToSingle(buff, 0).ToString();
                buff = new byte[4]; Buffer.BlockCopy(bytes, 66, buff, 0, buff.Length);// Array.Reverse(buff);
                obj.CommonVar20_15 = BitConverter.ToSingle(buff, 0).ToString();


                buff = new byte[4]; Buffer.BlockCopy(bytes, 70, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_16 = BitConverter.ToString(buff, 0).Replace("-", "");



                buff = new byte[4]; Buffer.BlockCopy(bytes, 74, buff, 0, 4);// Array.Reverse(buff);
                obj.CommonVar20_17 = BitConverter.ToSingle(buff, 0).ToString();

                buff = new byte[1]; Buffer.BlockCopy(bytes, 78, buff, 0, 1);// Array.Reverse(buff);
                int a= Convert.ToInt16(BitConverter.ToString(buff, 0).Replace("-", ""),16);
                obj.CommonVar20_18 =a.ToString();
             
             

                obj.GathDt = DateTime.Now;
                obj.DataType = CommFlag + FunDesp;





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

//   0 C3000000
//   4 1222071117
//   9 0205091700
//1 14 AE478141
//2 18 48E18041
//3 22 0000A041
//4 26 00009041
//5 30 00009041
//6 34 20230000
//7 38 00000000
//8 42 6400
//9 44 0000
//0 46 00000000
//1 50 00000000
//2 54 00000000
//3 58 00000000
//4 62 00000000
//5 66 00000000
//6 70 42020000
//7 74 000080BF
//8 78 00
    }
}
