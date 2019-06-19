using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace CardDeviceUI.Util
{
    public class DriverHelper
    {
        public zkemkeeper.CZKEMClass device = new zkemkeeper.CZKEMClass();

        public Entity.Connection connection { get; set; }

        public bool connected { get; set; }

        public bool Connect()
        {
            this.connected = this.Connect(this.connection);

            return connected;
        }

        public void Disconnect()
        {
            this.device.Disconnect();
            this.connected = false;
        }

        public bool GetUsers()
        {
            string strEnrollNumber = "";
            string strName = "";
            string strPassword = "";
            int intPrivilege = 1;
            bool bolEnabled = false;

            int intFingerIndex;
            string strTmpData = "";
            int intTmpLength = 0;
            int intFlag = 0;

            string strCard = "";

            if (!this.connected)
            {
                Util.LogHelper.WriteLog("Call GetUsers failed. Device is disconnected.");
                return false;
            }


            //bool ok = false;
            //ok = this.device.IsTFTMachine(this.connection.MachineNumber);
            //if (ok) {
            //    this.device.ReadAllUserID(this.connection.MachineNumber);
            //    this.device.ReadAllTemplate(this.connection.MachineNumber);

            //}


            //if (this.device.ReadAllUserID(this.connection.MachineNumber))
            //{
            //    this.device.GetStrCardNumber(out strCard);
            //    MessageBox.Show(strCard);
            //}

            device.ReadAllUserID(connection.MachineNumber);
            device.ReadAllTemplate(connection.MachineNumber);
            while (device.SSR_GetAllUserInfo(connection.MachineNumber, out strEnrollNumber, out strName, out strPassword, out intPrivilege, out bolEnabled))//get all the users' information from the memory
            {
                for (intFingerIndex = 0; intFingerIndex < 10; intFingerIndex++)
                {
                    if (device.GetUserTmpExStr(connection.MachineNumber, strEnrollNumber, intFingerIndex, out intFlag, out strTmpData, out intTmpLength))//get the corresponding templates string and length from the memory
                    {
                        List<Entity.User> users = new List<Entity.User>();
                        users.Add(new Entity.User
                        {
                            EnrollNumber = strEnrollNumber,
                            Name = strName,
                            FingerIndex = intFingerIndex,
                            TmpData = strTmpData,
                            Privilege = intPrivilege,
                            Password = strPassword,
                            Enabled = bolEnabled,
                            Flag = intFlag
                        });

                        //ListViewItem list = new ListViewItem();
                        //list.Text = strEnrollNumber;
                        //list.SubItems.Add(strName);
                        //list.SubItems.Add(intFingerIndex.ToString());
                        //list.SubItems.Add(strTmpData);
                        //list.SubItems.Add(intPrivilege.ToString());
                        //list.SubItems.Add(strPassword);
                        //if (bolEnabled == true)
                        //{
                        //    list.SubItems.Add("true");
                        //}
                        //else
                        //{
                        //    list.SubItems.Add("false");
                        //}
                        //list.SubItems.Add(intFlag.ToString());
                    }
                }
            }

            return true;
        }

        public bool SetUser(Entity.User user)
        {
            int intErrorCode = 0;
            bool blnSetCard = false;
            bool blnIsTFTMachine = false;

            if (!this.connected)
            {
                Util.LogHelper.WriteLog("Call SetUser failed. Device is disconnected.");
                return false;
            }

            long l;
            

            //this.device.EnableDevice(this.connection.MachineNumber, true);
            bool isNew = false;
            isNew = this.device.IsNewFirmwareMachine(this.connection.MachineNumber);

            blnIsTFTMachine = this.device.IsTFTMachine(this.connection.MachineNumber);

            this.device.CardNumber[0] = 6511246;
            this.device.CardNumber[1] = 0;

            // blnSetCard = this.device.SetStrCardNumber(user.Card);//Before you using function SetUserInfo,set the card number to make sure you can upload it to the device

            //if (!blnSetCard)
            //{
            //    MessageBox.Show("SetStrCardNumber failed.");
            //}

            //int machineNumber = this.connection.MachineNumber;
            //string enrollNumber = user.EnrollNumber;
            //string name = user.Name;
            //string password = user.Password;
            //int privilege = 1;

            int machineNumber = this.connection.MachineNumber;
            int enrollNumber = Convert.ToInt32(user.EnrollNumber);
            string name = user.Name;
            string password = user.Password;
            int privilege = 1;


            //if (this.device.SSR_SetUserInfo(machineNumber, enrollNumber, name, password, privilege, true))      //upload the user's information(card number included)
            if (this.device.SetUserInfo(machineNumber,enrollNumber, name, password, privilege, true))
            {
                MessageBox.Show("(SSR_)SetUserInfo, UserID:" + user.EnrollNumber + " Privilege:" + user.Privilege + " Enabled:" + user.Enabled.ToString(), ",Success!");
            }
            else
            {
                this.device.GetLastError(ref intErrorCode);
                MessageBox.Show("Operation failed,ErrorCode=" + intErrorCode.ToString(), "Error");
                return false;
            }
            this.device.RefreshData(this.connection.MachineNumber);//the data in the device should be refreshed
            this.device.EnableDevice(this.connection.MachineNumber, true);

            return true;
        }

        //public bool Connect(Entity.Connection cn)
        private bool Connect(Entity.Connection cn)
        {
            bool isConnected = false;

            if (cn.IP.Trim() == "" || cn.Port.Trim() == "")
            {
                Util.LogHelper.WriteLog("IP and Port cannot be null.");
                
                return false;
            }

            int idwErrorCode = 0;

            isConnected = this.device.Connect_Net(cn.IP.Trim(), Convert.ToInt32(cn.Port.Trim()));

            if (isConnected)
            {

                //if (this.device.RegEvent(cn.MachineNumber, 65535))//Here you can register the realtime events that you want to be triggered(the parameters 65535 means registering all)
                //{
                //    this.device.OnConnected += new zkemkeeper._IZKEMEvents_OnConnectedEventHandler(OnConnected);
                //}


            } 
            else 
            {
                device.GetLastError(ref idwErrorCode);
                Util.LogHelper.WriteLog(string.Format("Unable to connect the device, ErrorCode={0}" , idwErrorCode.ToString()));
            }

            return isConnected;
        }

        //private void OnConnected()
        //{
        //    Util.LogHelper.WriteLog("OnConnected."); ;
        //}

        
    }
}
