using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace HeatingDSC.Controls
{
    public partial class RangeInput : UserControl
    {
        log4net.ILog log = log4net.LogManager.GetLogger("RangeInput");

        private decimal txtvalue;
        public decimal RangeDown{
            get{
                try
                {
                    txtvalue = Convert.ToDecimal(txtDown.Text);
                }
                catch
                {
                    txtvalue = 0;
                }
                return txtvalue;
            
            }
            set { txtDown.Text = value.ToString(); }
        }

        public decimal RangeUp{
            get
            {
                try
                {
                    txtvalue = Convert.ToDecimal(txtUp.Text);
                }
                catch (Exception ex)
                {
                    txtvalue = 0;
                    log.Error(ex.Message);
                }
                return txtvalue;
            }
            set { txtUp.Text = value.ToString(); }
        }
        public RangeInput()
        {
            InitializeComponent();
        }
    }
}
