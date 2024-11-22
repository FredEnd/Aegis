using LocalDatabaseApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aegis
{
    public partial class Session_Settings : Form
    {
        private List<int> Ports;
        private string IPaddress;

        public Session_Settings(List<int> Ports, string IPaddress)
        {
            InitializeComponent();

            this.Ports = Ports;
            this.IPaddress = IPaddress;

            Ports.Sort();

            if (Ports.Count > 0)
            {
                foreach (int Port in Ports)
                {
                    if (Port > 0)
                    {
                        PortsCombo.Items.Add(Port);
                    }
                }
            }
            else
            {
                MessageBox.Show("No Ports Please use a network that gives open port");
                Application.Exit();
            }
        }

        private void Create_Session_Click(object sender, EventArgs e)
        {
            var MessageInput = SessionID_Input.Text;
            Console.WriteLine(MessageInput);

            var SelectedPort = PortsCombo.SelectedIndex;

            var EncryptionMethod = EncryptionCombo.SelectedIndex;
        }
    }
}
