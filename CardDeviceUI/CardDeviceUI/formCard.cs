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
    public partial class formCard : Form
    {
        public formCard()
        {
            InitializeComponent();
        }

        private void btnSetCard_Click(object sender, EventArgs e)
        {
            bool blnSetSuccess = false;
            if (cmbConnection.Text == "") 
            {
                return;
            }
            
            Entity.Connection cn = formMain.config.Connections.FirstOrDefault(c => c.ConnectionName == cmbConnection.Text);
            if (cn == null) return;

            Util.DriverHelper device = new Util.DriverHelper
            {
                connection = cn
            };

            Entity.User user = new Entity.User{
                EnrollNumber = txtEnrollNumber.Text,
                Name = txtName.Text, 
                Card = txtCardNumber.Text, 
                Privilege = 0, 
                Password = "888888", 
                Enabled = true, 
                FingerIndex = 1, 
                Flag = 1, 
                TmpData = ""
            };


            device.Connect();

            blnSetSuccess = device.SetUser(user);
            if (blnSetSuccess)
            {
                device.Disconnect();
            }

        }

        private void formCard_Load(object sender, EventArgs e)
        {
            cmbConnection.Items.Clear();
            foreach (Entity.Connection cn in formMain.config.Connections)
            {
                cmbConnection.Items.Add(cn.ConnectionName);
            }
            if (cmbConnection.Items.Count > 0) cmbConnection.SelectedItem = cmbConnection.Items[0];
        }
    }
}
