using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TCPLibrary.Abstracts;

using DSC_FTHj1;
using QyTech.Communication;

namespace TCPLibrary.DefaultImplements
{
    /// <summary>
    /// ZDataBuffer的默认实现
    /// </summary>
    public class BaseDataBuffer : ZDataBuffer
    {
        /// <summary>
        /// 按照规定协议，重写TryReadMessage方法
        /// </summary>
        /// <returns></returns>
        internal override ZMessage TryReadMessage()
        {
            //log.Info("TryREadMessage 1:");

            ZdFT_Packet dataPacket = new ZdFT_Packet();

            //log.Info("TryREadMessage 10:" + dataPacket.StrRawData);
            BaseMessage bm;

            //判断是否是注册包
            if (_length >= 28)   //  20+N
            {
                using (MemoryStream ms = new MemoryStream(_buffer))
                {
                    BinaryReader br = new BinaryReader(ms);
                    byte[] aaa1 = br.ReadBytes(6);  //读取消息开始标志
                    string aaa2 = BitConverter.ToString(aaa1).Replace("-", " ");
                    if (aaa2 == "43 48 49 54 49 43")
                    {
                        //解码采集器id和通讯号
                        aaa1 = br.ReadBytes(20);
                        aaa2 = "";
                        for (int i = 0; i < 20; i++)
                        {
                            aaa2 += (aaa1[i] - 48).ToString();
                        }
                        //log.Error("注册包:" + aaa2.Substring(0, 9) + "------" + aaa2.Substring(9, 11));
                        dataPacket.StrRawData = aaa2;
                        bm = new BaseMessage(1, dataPacket); //还原成一条完整的消息
                        Remove(20 + 8);  //注意！ 移除已读数据
                        //log.Info("TryREadMessage 20:" + dataPacket.StrRawData);

                        return bm;  //返回读取到的消息

                    }
                }
            }
            //log.Info("TryREadMessage 30 判断缓冲区有无数据有无:" + _length.ToString());


            if (_length >= 20)   //  20+N
            {

                using (MemoryStream ms = new MemoryStream(_buffer))
                {
                    try
                    {
                        BinaryReader br = new BinaryReader(ms);
                        byte ZhenStartFlag1 = br.ReadByte();  //读取消息开始标志

                        while (ZhenStartFlag1 != 0x68)
                        {
                            Remove(1);
                            ZhenStartFlag1 = br.ReadByte();
                        }

                        log.Info("TryREadMessage 31");
                        dataPacket.pLength = br.ReadInt16();  //读取消息长度
                        int looplenght = br.ReadInt16(); //重复数据

                        log.Info("TryREadMessage 32");
                        byte ZhenStartFlag2 = br.ReadByte();
                        if (ZhenStartFlag2 != 0x68)
                        {
                            Remove(6);
                            log.Info("TryREadMessage 80:数据标志不是0x68，序列不正确，删除线面缓冲区内容：" + _length.ToString());
                        }
                        else
                        {
                            log.Info("TryREadMessage 33");

                            byte byteControlFlag = br.ReadByte();
                            byte[] addrpart = br.ReadBytes(5);
                            log.Info("TryREadMessage 34");

                            dataPacket.AFN = br.ReadByte();
                            dataPacket.SEQ = br.ReadByte();
                            log.Info("TryREadMessage 36");

                            dataPacket.InfoDesp = br.ReadBytes(2);
                            dataPacket.InfoType = br.ReadBytes(2);

                            string rawdata = IProtocal.bytes2Hex(_buffer, _length);
                            log.Info("TryREadMessage 40: 有数据：缓冲区数据长度：" + _length.ToString() + "包数据长度:" + dataPacket.pLength.ToString() + "数据：" + rawdata + "    判断是否满足一个整包？");


                            if (_length >= dataPacket.pLength + 8)//(_length - 20 >= dataPacket.pLength - 12)  //如果缓冲区中存在一条完整消息，则读取
                            {

                                dataPacket.Data = br.ReadBytes(dataPacket.pLength - 12);
                                dataPacket.StrRawData = IProtocal.bytes2Hex(dataPacket.Data, dataPacket.Data.Length);
                                log.Info("TryREadMessage 50:" + dataPacket.StrRawData);

                                dataPacket.CheckByte = br.ReadByte();
                                byte EndChar = br.ReadByte();

                                log.Info("TryREadMessage 60:  满足，目前长度：" + _length.ToString() + " 数据：" + dataPacket.StrRawData);


                                //byte[] msgcontent = br.ReadBytes(msglength);  //读取消息内容
                                byte[] bflag = new byte[3];
                                bflag[0] = dataPacket.AFN;
                                bflag[1] = dataPacket.InfoType[0];
                                bflag[2] = dataPacket.InfoType[1];
                                dataPacket.CommFlag = BitConverter.ToString(bflag).Replace("-", "");

                                if (dataPacket.CommFlag != "000400")
                                    bm = new BaseMessage(2, dataPacket); //还原成一条完整的消息，写数据回应
                                else if (dataPacket.CommFlag != "550100")
                                    bm = new BaseMessage(10, dataPacket); //还原成一条完整的消息，返回读数据
                                else
                                    bm = new BaseMessage(55, dataPacket);//透明传输
                                Remove(20 + dataPacket.Data.Length);  //注意！ 移除已读数据

                                return bm;  //返回读取到的消息
                            }
                            else
                            {
                                log.Info("TryREadMessage 70: 不满足，目前长度：" + _length.ToString());
                                return null;
                            }
                        }
                        return null;
               
                    }
                    catch (Exception ex)
                    {
                        log.Error("TryREadMessage 100" + ex.Message);
                        return null;
                    }
                }

            }
            else
            {
                return null;
            }
        }
    }
}
