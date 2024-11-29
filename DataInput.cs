using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
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

        private async void EnterButton_Click(object sender, EventArgs e)
        {
            var input = EnterBox.Text;
            var SessionInfo = Mains.InfoFromSessionCode(input);

            string TargetIP = SessionInfo.ipAddress;
            int TargetPort = SessionInfo.port;
            string TargetSession = SessionInfo.sessionID;

            //string sessionData = await RequestSessionDataAsync(TargetIP, TargetPort, TargetSession);
            var (client, stream) = await Mains.ConnectAsync(TargetIP, TargetPort);
            MessageBox.Show(Convert.ToString(client), Convert.ToString(stream));


            /*
            if (!string.IsNullOrEmpty(sessionData))
            {
                MessageBox.Show($"Session Data Received:\n{sessionData}");
            }
            else
            {
                MessageBox.Show("Failed to retrieve session data.");
            }
            */
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            EnterBox.Text = string.Empty;
            this.Close();
        }

        private async Task<string> RequestSessionDataAsync(string ip, int port, string sessionID)
        {
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    // connect to the server
                    await client.ConnectAsync(ip, port);
                    Console.WriteLine($"Connected to server {ip}:{port}");

                    using (NetworkStream stream = client.GetStream())
                    {
                        // create a request message for the session data
                        string request = $"GET_SESSION_INFO:{sessionID}";
                        byte[] requestBytes = Encoding.UTF8.GetBytes(request);

                        // send the request to the server
                        await stream.WriteAsync(requestBytes, 0, requestBytes.Length);
                        Console.WriteLine("Request sent for session data.");

                        // wait for the server response
                        byte[] responseBuffer = new byte[4096];
                        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                        if (bytesRead > 0)
                        {
                            //convert the response to a string
                            string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                            Console.WriteLine($"Response received: {response}");
                            return response;
                        }
                        else
                        {
                            Console.WriteLine("No data received from server.");
                            return null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
    }
}
