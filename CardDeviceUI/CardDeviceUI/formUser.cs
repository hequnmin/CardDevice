using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CardDeviceUI
{
    public partial class formUser : Form
    {
        public formUser()
        {
            InitializeComponent();
        }

        public void RefreshConnection() 
        {
            
            cmbConnection.Items.Clear();
            foreach (Entity.Connection cn in formMain.config.Connections)
            {
                cmbConnection.Items.Add(cn.ConnectionName);
            }
            return;
        }

        private void formUser_Load(object sender, EventArgs e)
        {
            this.RefreshConnection();
        }

        private void btnSync_Click(object sender, EventArgs e)
        {
            string connectionName = cmbConnection.Text ;

            if (connectionName.Trim() ==  "")
            {
                MessageBox.Show("Please select connection.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Util.DriverHelper driver = new Util.DriverHelper();

            Entity.Connection cn = new Entity.Connection();

            cn = formMain.config.Connections.FirstOrDefault(c => c.ConnectionName == connectionName);
            if (cn == null)
            {
                MessageBox.Show("Connection not exists.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            driver.connection = cn;
            if (driver.Connect())
            {
                driver.GetUsers();
            }
            else
            {
                MessageBox.Show("Connect failed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
