using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

//using System.Configuration;

namespace CardDeviceUI
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
            //Application.Run(new Form1());
            
            //log4net.Config.XmlConfigurator.Configure();
            //Console.WriteLine(ConfigurationManager.AppSettings["Version"]);

            Application.Run(new formMain());
        }
    }
}
