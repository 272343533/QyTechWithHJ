using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunMvcExpress.Core.Common
{
    public class SimpleSuggetJsonObject
    {
        public SimpleSuggetJsonObject(int id, string name, string desp)
        {
            Id = id;
            Name = name;
            Desp = desp;
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Desp { get; set; }
    }
}
