using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ChenDong.Common.SingleInstance;
namespace HeatingDSC
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //SingleApplication.Run(new frmStart());
            Application.Run(new frmStart());
            //System.Diagnostics.Debug.WriteLine(DateTime.Now.ToLongTimeString());
        }
    }
}
