using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Newtonsoft.Json;


namespace CardDeviceUI
{
    public partial class formConnection : Form
    {
        public formConnection()
        {
            InitializeComponent();
        }

        public zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();
        private bool isConnected = false;
        private int machineNumber = 1;


        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            bool connected = this.TestConnect();
            if (connected)
            {
                MessageBox.Show("Connection Successful.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Connection failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool TestConnect()
        {
            Entity.Connection cn = new Entity.Connection
            {
                ConnectionName = txtConnectionName.Text.Trim(),
                IP = txtIP.Text.Trim(),
                Port = txtPort.Text.Trim(),
                Provider = "ZKTeco",
                MachineNumber = Convert.ToInt32(numMachineNumber.Value)
            };

            Cursor = Cursors.WaitCursor;

            Util.DriverHelper driverHelper = new Util.DriverHelper { connection = cn };
            driverHelper.Connect();
            
            Cursor = Cursors.Default;

            return driverHelper.connected;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (this.SaveConfig())
            {
                this.Dispose();
            }


        }

        private void LogWrite()
        {
            Util.LogHelper.WriteLog(string.Format("当前时间{0}", DateTime.Now.ToString()));
        }

        private bool SaveConfig()
        {
            bool saveSuccess = false;

            if (txtConnectionName.Text.Trim() == "")
            {
                MessageBox.Show("Please input connection name.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return saveSuccess;
            }

            if (formMain.config.Connections.FirstOrDefault(c => c.ConnectionName == txtConnectionName.Text.Trim()) != null)
            {
                MessageBox.Show("Connection with same connection name already exists.", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return saveSuccess;
            }

            Cursor = Cursors.WaitCursor;
            btnOK.Enabled = false;

            Entity.Connection cn = new Entity.Connection
            { 
                ConnectionName = txtConnectionName.Text.Trim(), 
                Provider = "ZKTeco", 
                IP = txtIP.Text.Trim(), 
                Port = txtPort.Text.Trim(),
                MachineNumber = Convert.ToInt32(numMachineNumber.Value)
            };
            Util.DriverHelper driver = new Util.DriverHelper();
            driver.connection = cn;
            driver.Connect();
            if (driver.connected)
            {
                formMain.config.Connections.Add(cn);
                Util.JsonHelper.SerializeFile(formMain.config);
                saveSuccess = true;
            }
            else
            {
                saveSuccess = false;
            }

            Cursor = Cursors.Default;
            btnOK.Enabled = true;

            return saveSuccess;
        }
    }


}
