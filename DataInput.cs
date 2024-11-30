using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using Aegis_main;
using LocalDatabaseApp;

namespace Aegis
{
    public partial class DataInput : Form
    {
        private string UserID;
        private string TargetSession;
        private int TargetPort;
        private Home_Page Home_Page;
        private Settings Settings;
        private List<int> Ports;
        private string UserIP;


        public DataInput(string UserID, Home_Page home_page, List<int> Ports, Settings CurrentAppSettings, string UserIP)
        {
            this.UserID = UserID;

            this.Home_Page = home_page;
            this.Settings = CurrentAppSettings;
            this.Ports = Ports;
            this.UserIP = UserIP;

            InitializeComponent();
        }

        private async void EnterButton_Click(object sender, EventArgs e)
        {
            var input = EnterBox.Text;
            var SessionInfo = Mains.InfoFromSessionCode(input);

            string TargetIP = SessionInfo.ipAddress;
            this.TargetPort = SessionInfo.port;
            this.TargetSession = SessionInfo.sessionID;

            string sessionData = await RequestSessionDataAsync(TargetIP, TargetPort, TargetSession);

            HandleResponse(sessionData);

            if (!string.IsNullOrEmpty(sessionData))
            {
                MessageBox.Show($"Session Data Received:\n{sessionData}");
            }
            else
            {
                MessageBox.Show("Failed to retrieve session data.");
            }
            
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
                        string request = $"GET_SESSION_INFO:{sessionID} | USERID: {UserID}";
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

        private void HandleResponse(string responseMessage)
        {
            try
            {
                const string commandPrefix = "SESSION_INFO_RECEIVED:";
                if (responseMessage.StartsWith(commandPrefix))
                {
                    // Remove the command prefix to extract the actual data
                    string data = responseMessage.Substring(commandPrefix.Length).Trim();

                    // Split the data at the comma (',')
                    string[] parts = data.Split(',');

                    if (parts.Length == 2)
                    {
                        string HUserID = parts[0].Trim();
                        string EncryptionType = parts[1].Trim();

                        // Output or use the extracted information
                        DB.Create_Session(TargetSession, HUserID, EncryptionType, TargetPort);

                        Refresh_Sessions();
                    }
                    else
                    {
                        Console.WriteLine("Invalid response format: expected two values separated by a comma.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid response: does not start with 'SESSION_INFO_RECEIVED:'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        public void Refresh_Sessions()
        {
            Home_Page.Messages_Panel.Controls.Clear();

            var chatSessions = DB.LoadChatSessions();
            if (chatSessions == null || chatSessions.Count == 0)
            {
                Console.WriteLine("NO SESSIONS LOADED: NULL OR EMPTY");
            }
            else
            {
                foreach (var session in chatSessions)
                {
                    ChatSessionButton newChat = new ChatSessionButton(session.SessionID, session.CreatedAt, Settings, UserID , UserIP, Ports);
                    newChat.InitializeButton();
                    Home_Page.Messages_Panel.Controls.Add(newChat.GetButton());
                }
            }
        }
    }
}
