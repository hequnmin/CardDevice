using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CardDeviceUI.Entity
{
    public class Connection
    {
        public string ConnectionName { get; set; }

        public string IP { get; set; }
        public string Port { get; set; }
        public string Provider { get; set; }
        public int MachineNumber { get; set; }
    }

    public class Config
    {
        public string Version { get; set; }
        public List<Connection> Connections {get; set; }

    }

    public class User
    {
        public string EnrollNumber { get; set; }
        public string Name { get; set; }
        public string Card { get; set; }
        public int FingerIndex { get; set; }
        public string TmpData { get; set; }
        public int Privilege { get; set; }
        public string Password { get; set; }
        public bool Enabled { get; set; }
        public int Flag { get; set; }
    }
}
