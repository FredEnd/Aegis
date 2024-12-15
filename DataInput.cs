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
using static Aegis.Message_Window;
using static Aegis_main.Mains;

namespace Aegis
{
    public partial class DataInput : Form
    {
        private string UserID;
        public string TargetSession;
        private int TargetPort;
        private string TargetHost;
        private string TargetEncryption;
        private Home_Page Home_Page;
        private Settings Settings;
        private List<int> Ports;
        private string UserIP;
        private TcpClient tcpClient;


        public DataInput(string UserID, Home_Page home_page, List<int> Ports, Settings CurrentAppSettings, string UserIP, TcpClient tcpClient)
        {
            this.UserID = UserID;

            this.Home_Page = home_page;
            this.Settings = CurrentAppSettings;
            this.Ports = Ports;
            this.UserIP = UserIP;
            this.tcpClient = tcpClient;

            InitializeComponent();
        }

        private async void EnterButton_ClickAsync(object sender, EventArgs e)
        {
            var input = EnterBox.Text;
            var SessionInfo = Mains.InfoFromSessionCode(input);

            string TargetIP = SessionInfo.ipAddress;
            this.TargetPort = SessionInfo.port;
            this.TargetSession = SessionInfo.sessionID;
            this.TargetHost = SessionInfo.host;
            this.TargetEncryption = SessionInfo.encryption;

            string sessionData = await RequestSessionDataAsync(TargetIP, TargetPort, TargetSession);

            DB.Create_Session(TargetSession, TargetHost, TargetEncryption, TargetPort);
            this.Home_Page.Refresh_Sessions();

            this.Close();
            /*
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
                if (!tcpClient.Connected)
                {
                    await tcpClient.ConnectAsync(ip, port);
                    Console.WriteLine($"Connected to server {ip}:{port}");
                }

                NetworkStream stream = tcpClient.GetStream();

                // Create a request message for the session data
                string request =  $"USERID:testUser|TestMessage";
                byte[] requestBytes = Encoding.UTF32.GetBytes(request);

                // Send the request to the server
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);
                Console.WriteLine("Request sent for session data.");

                // Wait for the server's response
                byte[] responseBuffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                if (bytesRead > 0)
                {
                    // Convert the response to a string
                    string response = Encoding.UTF32.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine($"Response received: {response}");

                    // Safely add to connected clients
                    if (this.Home_Page != null)
                    {
                        this.Home_Page.connectedClients.Add(sessionID, tcpClient);
                        Console.WriteLine("Client added to Home_Page.");
                    }
                    else
                    {
                        Console.WriteLine("Home_Page instance is null.");
                    }

                    return response;
                }
                else
                {
                    Console.WriteLine("No data received from server.");
                    return null;
                }
                
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Socket error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
            finally
            {
                tcpClient.Close();
                this.Close();
            }
        }


        private void HandleResponse(string responseMessage)
        {
            try
            {
                if (responseMessage != null)
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
                else
                {
                    Console.WriteLine("No Response");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing response: {ex.Message}");
            }
        }

        public void Refresh_Sessions()
        {
            this.Home_Page.Refresh_Sessions();
        }
    }
}
