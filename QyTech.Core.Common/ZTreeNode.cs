using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QyTech.Core.Common
{
    public class ZTreeNode
    {
        public bool Checked { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string PId { get; set; }
        public bool open { get; set; }
        public string Url { get; set; }
        public string target{ get; set; }
        public string rel { get; set; }
        public bool addBtnFlag { get; set; }
        public bool editBtnFlag { get; set; }
        public bool removeBtnFlag { get; set; }
        public bool nocheck { get; set; }
        public bool chkDisabled { get; set; }
    }
}
