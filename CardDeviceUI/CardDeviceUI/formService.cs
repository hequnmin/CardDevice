using System;
using System.Collections;
//using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.ServiceProcess;
using System.Configuration.Install;

namespace CardDeviceUI
{
    public partial class formService : Form
    {
        public formService()
        {
            InitializeComponent();
        }

        string serviceFilePath = string.Format("{0}\\CardDeviceService.exe", Application.StartupPath);
        string serviceName = "MainService";

        private void formService_Load(object sender, EventArgs e)
        {
            this.RefreshServiceButton(serviceName);
        }

        //刷新按钮
        private void RefreshServiceButton(string serviceName)
        {
            if (this.IsServiceExisted(serviceName))
            {
                btnInstallService.Enabled = false;
                btnUninstallService.Enabled = true;
            }
            else
            {
                btnInstallService.Enabled = true;
                btnUninstallService.Enabled = false;
            }
        }

        //判断服务是否存在
        private bool IsServiceExisted(string serviceName)
        {
            ServiceController[] services = ServiceController.GetServices();
            foreach (ServiceController sc in services)
            {
                Console.WriteLine(sc.ServiceName);
                if (sc.ServiceName.ToLower() == serviceName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }

        //安装服务
        private void InstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                IDictionary savedState = new Hashtable();
                installer.Install(savedState);
                installer.Commit(savedState);
            }
        }

        //卸载服务
        private void UninstallService(string serviceFilePath)
        {
            using (AssemblyInstaller installer = new AssemblyInstaller())
            {
                installer.UseNewContext = true;
                installer.Path = serviceFilePath;
                installer.Uninstall(null);
            }
        }
        //启动服务
        private void ServiceStart(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Stopped)
                {
                    control.Start();
                }
            }
        }

        //停止服务
        private void ServiceStop(string serviceName)
        {
            using (ServiceController control = new ServiceController(serviceName))
            {
                if (control.Status == ServiceControllerStatus.Running)
                {
                    control.Stop();
                }
            }
        }

        private void btnInstallService_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.UninstallService(serviceName);
            }
            this.InstallService(serviceFilePath);

            this.RefreshServiceButton(serviceName);
        }

        private void btnStartService_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName)) this.ServiceStart(serviceName);
        }

        private void btnStopService_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName)) this.ServiceStop(serviceName);
        }

        private void btnUninstallService_Click(object sender, EventArgs e)
        {
            if (this.IsServiceExisted(serviceName))
            {
                this.ServiceStop(serviceName);
                this.UninstallService(serviceFilePath);
                this.RefreshServiceButton(serviceName);
            }
        }


    }
}
