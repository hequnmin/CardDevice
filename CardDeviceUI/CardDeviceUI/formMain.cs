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
    public partial class formMain : Form
    {
        //private int childFormNumber = 0;

        public formMain()
        {
            InitializeComponent();
            
        }

        public static Entity.Config config = new Entity.Config();

        private void ShowNewForm(object sender, EventArgs e)
        {
            //Form childForm = new Form1();
            //childForm.MdiParent = this;
            //childForm.Text = "窗口 " + childFormNumber++;
            //childForm.Show();
        }

        private void OpenFile(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            openFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (openFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = openFileDialog.FileName;
            }
        }

        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                string FileName = saveFileDialog.FileName;
            }
        }

        private void ExitToolsStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void ToolBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStrip.Visible = toolBarToolStripMenuItem.Checked;
        }

        private void StatusBarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            statusStrip.Visible = statusBarToolStripMenuItem.Checked;
        }

        private void CascadeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.Cascade);
        }

        private void TileVerticalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileVertical);
        }

        private void TileHorizontalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.TileHorizontal);
        }

        private void ArrangeIconsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LayoutMdi(MdiLayout.ArrangeIcons);
        }

        private void CloseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (Form childForm in MdiChildren)
            {
                childForm.Close();
            }
        }

        private void menuConnection_Click(object sender, EventArgs e)
        {
            Form frmConnection = new formConnection();
            frmConnection.ShowDialog();
            this.RefreshConfig();
        }

        private void ServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmService = new formService();
            frmService.ShowDialog();
        }

        private void formMain_Load(object sender, EventArgs e)
        {

            this.RefreshConfig();
        }

        private void RefreshConfig()
        {
            config = Util.JsonHelper.DeserializeFile<Entity.Config>();

            if (config != null && config.Connections.Count > 0)
            {
                tvwConnection.Nodes.Clear();
                foreach (Entity.Connection cn in config.Connections)
                {
                    tvwConnection.Nodes.Add(cn.ConnectionName);
                }
            }

        }

        private void devicesPanelMenuItem_Click(object sender, EventArgs e)
        {
            DevicePanel.Visible = devicesPanelMenuItem.Checked;
        }

        private void tvwConnections_Click(object sender, EventArgs e)
        {

        }

        private void tvwConnections_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {

        }

        private void tvwConnections_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void tvwConnection_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Entity.Connection cn = new Entity.Connection();
            cn = config.Connections.First<Entity.Connection>(c=>c.ConnectionName == e.Node.Text);
            if (cn != null)
            {
                ppgConnection.SelectedObject = cn;
            }
        }

        private void formMain_Deactivate(object sender, EventArgs e)
        {
            config = Util.JsonHelper.DeserializeFile<Entity.Config>();

            if (config != null && config.Connections.Count > 0)
            {
                tvwConnection.Nodes.Clear();
                foreach (Entity.Connection cn in config.Connections)
                {
                    tvwConnection.Nodes.Add(cn.ConnectionName);
                }
            }
        }

        private void tvwConnection_MouseUp(object sender, MouseEventArgs e)
        {

        }

        private void tvwConnectionMenu_DeleteConnection_Click(object sender, EventArgs e)
        {
            string connectionName = tvwConnection.SelectedNode.Text;

            if (MessageBox.Show(string.Format("Are you sure you want to delete <{0}>?", connectionName), "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                config.Connections.Remove(config.Connections.First<Entity.Connection>(c => c.ConnectionName == connectionName));

                Util.JsonHelper.SerializeFile(config);
                this.RefreshConfig();
            }
        }

        private void usersFormMenuItem_Click(object sender, EventArgs e)
        {
            formUser frmUser = new formUser();
            frmUser.MdiParent = this;
            frmUser.Show();
        }

        private void cardsFormMenuItem_Click(object sender, EventArgs e)
        {
            formCard frmCard = new formCard();
            frmCard.MdiParent = this;
            frmCard.Show();
        }
    }
}
