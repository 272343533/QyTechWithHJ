using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeatingDSC.BLL
{

    public class PacketFac
    {
        public virtual IProduct CreatePacket(ProductType pt)
        {
            IProduct product = null;
            switch (pt)
            {
                case ProductType.GlfofJinLaiUpData:
                    product = new ParseGlfJinLaiUpData();
                    break;
                case ProductType.GlfOfJinLaiDownRange:
                    product = new ParseGlfJinLaiDownRangeSet();
                    break;
                case ProductType.HrzUpCommData:
                    product = new ParseHrzUpCommData();
                    break;
                case ProductType.HrzUpUnitData:
                    product = new ParseHrzUpUnitData();
                    break;
                case ProductType.HrzUpUnitRangeData:
                    product = new ParseHrzUpUnitRangeData();
                    break;
                case ProductType.HrzDownUnitRangeData:
                    product = new ParseHrzUpUnitRangeData();
                    break;
                case ProductType.HrzDownCurveAll:
                case ProductType.hrzDownCurveOnly:
                case ProductType.HrzDownCurveOffset:
                case ProductType.HrzDownCurveScale:
                    product=new ParseHrzDownCurve();
                    break;
                //case ProductType.HrzUpRunCurveData:
                //    product = new ParseHrzUpRunCurve();
                //    break;
                case ProductType.HrzDownWeather:
                    product = new ParseHrzDownWeather();
                    break;
                case ProductType.QxyUpData:
                    product = new ParseQxyUpData();
                    break;
                case ProductType.WdjUpData:
                    product = new ParseWdjUpData();
                    break;
                case ProductType.ResponseData:
                    product = new ParseResponsePacket();
                    break;
                case ProductType.GlfofGaodianchangUpData:
                    product = new ParseGlfGaodianchangUpData();
                    break;
            }
            return product;
        }
    }
}
