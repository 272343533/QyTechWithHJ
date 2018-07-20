using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SunMvcExpress.Core.Common
{
    public class ZTreeNode
    {
        public bool Checked { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid PId { get; set; }
        public bool open { get; set; }
        public string Url { get; set; }
        public string target{ get; set; }
        public string rel { get; set; }
        public bool noAddBtn { get; set; }
        public bool noEditBtn { get; set; }
        public bool noRemoveBtn { get; set; }
       // public bool Checked { get; set; }

    }
}
