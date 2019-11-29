using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SunMvcExpress.Dao;
using SunMvcExpress.Core.BLL;


namespace HjCommDA
{
    public class BaseDABll
    {

        /// <summary>
        /// 根据下发命令获得下发对象
        /// </summary>
        /// <param name="hds"></param>
        /// <returns></returns>
        public static object GetObjByDownSet(vwlyHrzDownSet hds)
        {
            string DetsqlCondition, bsOsqlCondition, OnlyDetsqlCondition, OnlybsOsqlCondition;
            string SettingTName = "";
            //hds.CommNo-> 协议--〉得到地址对应的协议-〉得到对应的表名GathTName，作为对象，从这个里面获取最后一条记录的对象
            //对于多机组，还要根据地址去顶是哪个机组，进而获取对应机组的最后一条数据。
            bsProtocal proobj;

            proobj = EntityManager<bsProtocal>.GetBySql<bsProtocal>("PId=" + hds.bsP_Id.ToString() + " and FromAddr<='" + hds.SetFlag + "' and ToAddr>='" + hds.SetFlag + "'");

            if (proobj == null)
            {
                proobj = EntityManager<bsProtocal>.GetBySql<bsProtocal>("PId=" + hds.bsP_Id.ToString() + " and FromAddr='" + hds.SetFlag + "'");
                if (proobj==null)
                    proobj = EntityManager<bsProtocal>.GetBySql<bsProtocal>("Id=" + hds.bsP_Id.ToString());
         
            }
            SettingTName = proobj.SettingTName;
            DetsqlCondition = "bsO_Id='" + hds.bsO_Id.ToString() + "' and GathDt=(select max(GathDt) from " + SettingTName + " where bsO_Id='" + hds.bsO_Id.ToString() + "'";
            bsOsqlCondition = "bsO_Id='" + hds.bsO_Id.ToString() + "' and GathDt=(select max(GathDt) from " + SettingTName + " where bsO_Id='" + hds.bsO_Id.ToString() + "'";
            OnlyDetsqlCondition = "bsO_Id='" + hds.bsO_Id.ToString() + "'";
            OnlybsOsqlCondition = "bsO_Id='" + hds.bsO_Id.ToString() + "'";

            if (SettingTName == "HJHrzControlData2Part1")
            {
                return EntityManager<HJHrzControlData2Part1>.GetBySql<HJHrzControlData2Part1>(DetsqlCondition);
            }
            else if (SettingTName == "HJHrzControlData2Part2")
            {
                return EntityManager<HJHrzControlData2Part2>.GetBySql<HJHrzControlData2Part2>(DetsqlCondition);
            }
            else if (SettingTName == "HjPlcParaControlCurve")
            {
                return EntityManager<HjPlcParaControlCurve>.GetBySql<HjPlcParaControlCurve>(bsOsqlCondition);
            }
            else if (SettingTName == "HrzRangeAlarmConf")
            {
                if (hds.FlagDesp.Contains("量程"))
                {
                    return EntityManager<HrzRangeAlarmConf>.GetBySql<HrzRangeAlarmConf>(OnlybsOsqlCondition + " and  Filter='量程'");
                }
                else if (hds.FlagDesp.Contains("报警"))
                {
                    return EntityManager<HrzRangeAlarmConf>.GetBySql<HrzRangeAlarmConf>(OnlybsOsqlCondition + " and  Filter='报警'");
                }
                else
                {
                    return null;
                }
            }
            else if (SettingTName == "HrzRunCurve")
            {
                return EntityManager<HrzRunCurve>.GetByPk<HrzRunCurve>("Id", Guid.Parse(hds.SetValues));
            }
            else
                return null;
        }


        public static DTUProduct GetDtuProduct(string commno)
        {
            return EntityManager<DTUProduct>.GetByStringFieldName<DTUProduct>("CommNo", commno);
        }

        public static List<DetailDevice> GetDetailDevice(Guid dtuid)
        {
            return EntityManager<DetailDevice>.GetListNoPaging<DetailDevice>("DTU_Id='" + dtuid.ToString() + "' and DataStatus='正常'", "Convert(int,SubDevNo)");

        }
        public static List<DetailDevice> GetDetailDevice(string commno)
        {
            DTUProduct dtu= EntityManager<DTUProduct>.GetByStringFieldName<DTUProduct>("CommNo", commno);
            return EntityManager<DetailDevice>.GetListNoPaging<DetailDevice>("DTU_Id='"+dtu.Id.ToString()+"'", "");

        }
        public static bsOrganize GetOrganize(Guid bsO_Id)
        {
            return EntityManager<bsOrganize>.GetByPk<bsOrganize>("Id", bsO_Id);

        }
        public static bsOrganize GetOrganize(string commno)
        {
            DTUProduct dtu = EntityManager<DTUProduct>.GetByStringFieldName<DTUProduct>("CommNo", commno);
            return EntityManager<bsOrganize>.GetByPk<bsOrganize>("Id",(Guid)dtu.AZWZ);

        }

        public static List<bsProtItem> GetProtocalItem(int bsP_Id)
        {
            return EntityManager<bsProtItem>.GetListNoPaging<bsProtItem>("bsP_Id=" + bsP_Id.ToString() + " and FieldName is not null and RegCount is not null", "StartRegAddress asc");
        }


    }
}
