using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HeatingDSC.Models;
using HeatingDSC.DAL;

namespace HeatingDSC.BLL
{
    public static partial class StationManager
    {
        public static IList<Stations> Getallstations()
        {
            return StationService.Getallstations();
        }
    }
}
