using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.IO;

namespace CardDeviceService
{
    partial class MainService : ServiceBase
    {
        public MainService()
        {
            InitializeComponent();
        }

        string filePath = @"D:\CardDeviceServiceLog.txt";

        protected override void OnStart(string[] args)
        {
            // TODO: 在此处添加代码以启动服务。
            using (FileStream stream = new FileStream(filePath,FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(string.Format("{0},服务启动！", DateTime.Now));
            }
        }

        protected override void OnStop()
        {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
            using (FileStream stream = new FileStream(filePath, FileMode.Append))
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.WriteLine(string.Format("{0},服务停止！", DateTime.Now));
            }
        }
    }
}
