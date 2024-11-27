using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis_main;

namespace Aegis
{
    public partial class DataInput : Form
    {
        public DataInput()
        {
            InitializeComponent();

        }

        private async void Enter_Click(object sender, EventArgs e)
        {
            var input = EnterBox.Text;
            var SessionInfo = Mains.InfoFromSessionCode(input);

            string TargetIP = SessionInfo.ipAddress;
            int TargetPort = SessionInfo.port;
            string TargetSession = SessionInfo.sessionID;

            var (client, stream) = await Mains.ConnectAsync(TargetIP, TargetPort);

            if (client != null && stream != null)
            {
                Console.WriteLine("Connection established now possible to send messages and or files.");

                string message = "Hello, this is a test!";
                byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                Console.WriteLine("Message sent!");
            }
            else
            {
                MessageBox.Show("Failed to establish connection.");
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {

        }
    }
}
