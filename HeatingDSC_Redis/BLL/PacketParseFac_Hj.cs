using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HeatingDSC.BLL
{

    public class PacketFac_Hj:PacketFac
    {
        public override IProduct CreatePacket(ProductType pt)
        {
            IProduct product = null;
            switch (pt)
            {
                case ProductType.HJGathData1:
                    product = new HjHrzReadOnlyData();
                    break;
                case ProductType.HJPlcParaControl1:
                    product = new HjHrzControlData1();
                    break;
                case ProductType.HJPlcParaControl2:
                    product = new HjHrzControlData2();
                    break;
                case ProductType.HJPlcParaControlCurve:
                    product = new HjHrzControlCurve();
                    break;
                    
                case ProductType.GathAlarmData_Hj:
                    product = new HjHrzAlarmData();
                    break;
                case ProductType.GathRangeData_Hj:
                    product = new HjHrzRangeData();
                    break;
                
            }
            return product;
        }
    }
}
