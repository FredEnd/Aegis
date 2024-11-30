using LocalDatabaseApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis_main;
using YourNamespace;
using System.Net.Sockets;
using System.Net;


namespace Aegis
{
    public partial class Message_Window : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        private readonly string sessionId;
        public string HUserID;
        public string EncryptionType;
        private Settings AppSettings;

        public Message_Window(string sessionId, string UserID, string IPaddress, List<int> Ports, Settings Appsettings)
        {
            List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

            var setting = sessionSettings[0];
            this.sessionId = sessionId;
            this.HUserID = UserID;
            this.AppSettings = Appsettings;

            if (UserID != setting.HostUserID)
            {
                InitializeComponent();
                _= StartUpMessages();
                _ = InitializeMessageWindowClientAsync(IPaddress, setting.portNum);
            }

            else
            {
                InitializeComponent();
                _ = StartUpMessages();
                _ = InitializeMessageWindowHostAsync(IPaddress, Ports);
            }

            this.FormClosing += Message_Window_FormClosing;
        }

        //-------------------------------------------------------------------------------------------------

        private async Task InitializeMessageWindowClientAsync(string ipAddress, int port)
        {
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync(ipAddress, port);
                Console.WriteLine($"Connected to host at {ipAddress}:{port}");

                using (NetworkStream stream = client.GetStream())
                {
                    if (AppSettings.NotificationsEnabled == true)
                    {
                        Mains.play_notifercation();
                    }

                    string message = "Hello from the client!";
                    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
                    await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    Console.WriteLine("Message sent to host.");

                    byte[] responseBuffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF8.GetString(responseBuffer, 0, bytesRead);
                        Console.WriteLine($"Response received from host: {response}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Client initialization error: {ex.Message}");
            }
        }

        //-------------------------------------------------------------------------------------------------

        private async Task InitializeMessageWindowHostAsync(string IPaddress, List<int> Ports)
        {
            List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

            if (sessionSettings.Count > 0)
            {
                var setting = sessionSettings[0];

                this.EncryptionType = setting.Encryption;

                string ChatCode = Mains.GenerateSessionCode(IPaddress, setting.portNum, sessionId);

                TextBox Code = new TextBox();
                Session_Panel.Controls.Add(Code);
                Code.Dock = DockStyle.Right;
                Code.ReadOnly = true;
                Code.Text = ChatCode;

                IPAddress publicIP = IPAddress.Parse(IPaddress);

                TcpListener listener = new TcpListener(publicIP, setting.portNum);
                listener.Start();
                Console.WriteLine($"Server listening on port {setting.portNum}...");

                try
                {
                    while (true)
                    {
                        TcpClient NewClient = await Mains.AcceptClientAsync(listener);

                        if (NewClient != null)
                        {
                            if (AppSettings.NotificationsEnabled == true)
                            {
                                Mains.play_notifercation();
                            }

                            string Message = await Mains.HandleClientAsync(NewClient);
                            if (Message != null)
                            {
                                Message = "Null Message";
                            }
                            else
                            {
                                MessageBox.Show(Message);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in server loop: {ex.Message}");
                }
                finally
                {
                    listener.Stop();
                    Console.WriteLine("Server stopped.");
                }
            }
            else
            {
                Console.WriteLine("No session settings found.");
            }
        }

        private void HandleIncomingMessage(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Received an empty message.");
                return;
            }

            try
            {
                // Split the message by '|'
                string[] commands = message.Split('|');

                foreach (string command in commands)
                {
                    // Trim whitespace and process each command
                    ProcessCommandOrMessage(command.Trim());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        //-------------------------------------------------------------------------------------------------

        private async void ProcessCommandOrMessage(string command)
        {
            const string getSessionInfoPrefix = "GET_SESSION_INFO:";
            const string userIdPrefix = "USERID:";

            string recipentuserID = null;

            if (command.StartsWith(getSessionInfoPrefix))
            {
                string sessionID = command.Substring(getSessionInfoPrefix.Length).Trim();

                if (!string.IsNullOrEmpty(sessionID))
                {
                    Console.WriteLine($"Command received to get session info for: {sessionID}");

                    byte[] response = Encoding.UTF8.GetBytes($"SESSION_INFO_RECEIVED:{HUserID},{EncryptionType}");
                }

                else if (command.StartsWith(userIdPrefix))
                {

                    if (!string.IsNullOrEmpty(recipentuserID))
                    {
                        Console.WriteLine("Recipient userID Recieved");
                        recipentuserID = command.Substring(userIdPrefix.Length).Trim();

                    }
                    else
                    {
                        Console.WriteLine("USERID command received but no user ID was provided.");
                    }
                }
                else
                {
                    Console.WriteLine($"Standard message received: {command}");

                    DB.Create_Message(sessionId, recipentuserID, command, "recieved");

                    byte[] response = Encoding.UTF8.GetBytes($"Recipient recieved message echo");

                    var chatData = await DB.GetMessagesBySessionAsync(sessionId);

                    if (chatData == null || chatData.Count == 0)
                    {
                        Console.WriteLine("NO MESSAGES");
                    }
                    else
                    {
                        MessageTable.Controls.Clear();

                        foreach (var message in chatData)
                        {
                            var messageInstance = new Message(message.UserID, message.Direction, message.MessageContent, message.SentAt);
                            var messagePanel = messageInstance.CreateMessagePanel();

                            messagePanel.AutoSize = true;
                            messagePanel.AutoSizeMode = AutoSizeMode.GrowOnly;
                            messagePanel.Dock = DockStyle.Top;

                            int columnIndex = message.Direction == "sent" ? 1 : 0;

                            MessageTable.Controls.Add(messagePanel, columnIndex, MessageTable.RowCount);

                            MessageTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                            MessageTable.RowCount++;

                            for (int i = 0; i < MessageTable.RowCount; i++)
                            {
                                MessageTable.RowStyles[i] = new RowStyle(SizeType.AutoSize);
                            }
                        }
                    }
                }
            }
        }

        //-------------------------------------------------------------------------------------------------

        private void Message_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var confirmationDialog = new ConfirmationDialog())
            {
                var result = confirmationDialog.ShowDialog();
                // Cancel form close if user does not confirm
                if (!confirmationDialog.UserConfirmed)
                {
                    e.Cancel = true; // Prevent form from closing if user did not confirm
                }
            }
        }

        private async Task StartUpMessages()
        {
            var chatData = await DB.GetMessagesBySessionAsync(sessionId);

            if (chatData == null || chatData.Count == 0)
            {
                Console.WriteLine("NO MESSAGES");
            }
            else
            {
                MessageTable.Controls.Clear();

                foreach (var message in chatData)
                {
                    var messageInstance = new Message(message.UserID, message.Direction, message.MessageContent, message.SentAt);
                    var messagePanel = messageInstance.CreateMessagePanel();

                    messagePanel.AutoSize = true;
                    messagePanel.AutoSizeMode = AutoSizeMode.GrowOnly;
                    messagePanel.Dock = DockStyle.Top;

                    int columnIndex = message.Direction == "sent" ? 1 : 0;

                    MessageTable.Controls.Add(messagePanel, columnIndex, MessageTable.RowCount);

                    MessageTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    MessageTable.RowCount++;

                    for (int i = 0; i < MessageTable.RowCount; i++)
                    {
                        MessageTable.RowStyles[i] = new RowStyle(SizeType.AutoSize);
                    }
                }
            }
        }

        public class Message
        {
            public string Content { get; set; }
            public string Direction { get; set; }
            public string SentAt { get; set; }
            public string UserID { get; set; }

            public Message(string userID, string direction, string content, string sentAt)
            {
                UserID = userID;
                Direction = direction;
                Content = content;
                SentAt = sentAt;
            }

            public Panel CreateMessagePanel()
            {
                Panel messagePanel = new Panel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    BackColor = Direction == "sent" ? Color.LightBlue : Color.LightGreen,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };

                // Define the dynamic TextBox
                TextBox messageTextBox = new TextBox
                {
                    Text = Content,
                    ReadOnly = true,
                    Multiline = true,
                    BackColor = messagePanel.BackColor,
                    BorderStyle = BorderStyle.None, 
                    Dock = DockStyle.Top,
                    Font = new Font("Arial", 10),
                    ScrollBars = ScrollBars.None,
                    WordWrap = true,
                    AutoSize = true,
                };

                // Create time label
                Label timeLabel = new Label
                {
                    Text = SentAt,
                    AutoSize = true,
                    Font = new Font("Arial", 8, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(0, messageTextBox.Height)
                };

                // Create user label
                Label userLabel = new Label
                {
                    Text = $"User: {UserID}",
                    AutoSize = true,
                    Font = new Font("Arial", 8, FontStyle.Bold),
                    ForeColor = Color.Black,
                    Location = new Point(0, timeLabel.Bottom)
                };

                // Add controls to the panel
                messagePanel.Controls.Add(userLabel);
                messagePanel.Controls.Add(timeLabel);
                messagePanel.Controls.Add(messageTextBox);

                return messagePanel;
            }

        }

        //-------------------------------------------------------------------------------------------------

        private void Emoji_Button_Click(object sender, EventArgs e)
        {

        }

        private void File_Upload_Button_Click(object sender, EventArgs e)
        {

        }

        private async void Send_button_Click(object sender, EventArgs e)
        {
            var MessageInput = Input_Box.Text;
            Console.WriteLine(MessageInput);
            Console.WriteLine(sessionId);

            await DB.NewMessageFromClientAsync(MessageInput, sessionId, HUserID);
            Console.WriteLine("SUCCESSFULLY WRITTEN MESSAGE");

            Input_Box.Text = null;

            var chatData = await DB.GetMessagesBySessionAsync(sessionId);

            if (chatData == null || chatData.Count == 0)
            {
                Console.WriteLine("NO MESSAGES");
            }
            else
            {
                MessageTable.Controls.Clear();

                foreach (var message in chatData)
                {
                    var messageInstance = new Message(message.UserID, message.Direction, message.MessageContent, message.SentAt);
                    var messagePanel = messageInstance.CreateMessagePanel();

                    messagePanel.AutoSize = true;
                    messagePanel.AutoSizeMode = AutoSizeMode.GrowOnly;
                    messagePanel.Dock = DockStyle.Top;

                    int columnIndex = message.Direction == "sent" ? 1 : 0;

                    MessageTable.Controls.Add(messagePanel, columnIndex, MessageTable.RowCount);

                    MessageTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                    MessageTable.RowCount++;

                    for (int i = 0; i < MessageTable.RowCount; i++)
                    {
                        MessageTable.RowStyles[i] = new RowStyle(SizeType.AutoSize);
                    }
                }
            }
        }
    }
}
