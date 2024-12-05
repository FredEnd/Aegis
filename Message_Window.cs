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
using System.Threading;


namespace Aegis
{
    public partial class Message_Window : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        private readonly string sessionId;
        public string HUserID;
        public string CUserID;
        public string EncryptionType;
        private Settings AppSettings;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private TcpListener CurrentListener;


        public Message_Window(string sessionId, string UserID, string IPaddress, List<int> Ports, Settings Appsettings)
        {
            List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

            var setting = sessionSettings[0];
            this.sessionId = sessionId;
            this.HUserID = setting.HostUserID;
            this.CUserID = UserID;
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
                using (TcpClient client = new TcpClient())
                {
                    await client.ConnectAsync(ipAddress, port);
                    Console.WriteLine($"Connected to server {ipAddress}:{port}");

                    using (NetworkStream stream = client.GetStream())
                    {
                        // Send a startup message
                        string startupMessage = $"USERID:{HUserID}";
                        byte[] messageBytes = Encoding.UTF32.GetBytes(startupMessage);
                        await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                        Console.WriteLine("Startup message sent.");

                        // Optionally, handle server responses
                        byte[] responseBuffer = new byte[4096];
                        int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                        if (bytesRead > 0)
                        {
                            string response = Encoding.UTF32.GetString(responseBuffer, 0, bytesRead);
                            Console.WriteLine($"Response from server: {response}");
                        }
                        else
                        {
                            Console.WriteLine("No response received from the server.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error initializing client: {ex.Message}");
            }
        }

        //-------------------------------------------------------------------------------------------------

        private async Task InitializeMessageWindowHostAsync(string IPaddress, List<int> Ports)
        {
            try
            {
                List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

                if (sessionSettings.Count == 0)
                {
                    Console.WriteLine("No session settings found.");
                    return;
                }

                var setting = sessionSettings[0];
                this.EncryptionType = setting.Encryption;

                string ChatCode = Mains.GenerateSessionCode(IPaddress, setting.portNum, sessionId);

                TextBox Code = new TextBox
                {
                    Dock = DockStyle.Right,
                    ReadOnly = true,
                    Text = ChatCode
                };
                Session_Panel.Controls.Add(Code);

                IPAddress publicIP = IPAddress.Any;
                Console.WriteLine(publicIP);
                TcpListener listener = new TcpListener(publicIP, setting.portNum);

                //calls to stop listener when close
                this.CurrentListener = listener;
                
                listener.Start();
                Console.WriteLine($"Server listening on port {setting.portNum}...");

                // Server loop
                var token = _cancellationTokenSource.Token;
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        TcpClient newClient = await listener.AcceptTcpClientAsync();
                        Console.WriteLine("New client connected.");

                        // Handle client in a separate task
                        _ = Task.Run(async () =>
                        {
                            try
                            {
                                var (message, stream) = await Mains.HandleClientAsync(newClient);

                                if (string.IsNullOrEmpty(message))
                                {
                                    Console.WriteLine("nullMessage");
                                }
                                else
                                {
                                    HandleIncomingMessage(message, stream);
                                }

                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error handling client: {ex.Message}");
                            }
                            finally
                            {
                                newClient.Close();
                            }
                        }, token);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error accepting client: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in server initialization: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Server stopped.");
            }
        }

        private void HandleIncomingMessage(string message, NetworkStream stream)
        {
            Console.WriteLine("HandleIncomingMessage");

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
                    ProcessCommandOrMessage(command, stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        }

        private void StopServer()
        {
            _cancellationTokenSource.Cancel();
            if (CUserID != HUserID)
            {
                CurrentListener.Stop();
            }
        }

        //-------------------------------------------------------------------------------------------------

        private async void ProcessCommandOrMessage(string command, NetworkStream stream)
        {
            Console.WriteLine("PORCESSING MESSAGE");
            const string getSessionInfoPrefix = "GET_SESSION_INFO:";
            const string userIdPrefix = "USERID:";

            string recipentuserID = null;

            using (stream)
            {
                if (command.StartsWith(getSessionInfoPrefix))
                {
                    string sessionID = command.Substring(getSessionInfoPrefix.Length).Trim();

                    if (!string.IsNullOrEmpty(sessionID))
                    {
                        Console.WriteLine($"Command received to get session info for: {sessionID}");
                    

                        byte[] response = Encoding.UTF32.GetBytes($"SESSION_INFO_RECEIVED:{HUserID},{EncryptionType}");

                        await stream.WriteAsync(response, 0, response.Length);

                        Console.WriteLine("Response for session info sent");

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

                        if (AppSettings.NotificationsEnabled)
                        {
                            Mains.play_notifercation();
                        }

                        byte[] response = Encoding.UTF32.GetBytes($"Recipient recieved message echo");

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
        }

        //-------------------------------------------------------------------------------------------------

        private void Message_Window_FormClosing(object sender, FormClosingEventArgs e)
        {
            using (var confirmationDialog = new ConfirmationDialog())
            {
                StopServer();
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
                    var messageInstance = new Message(CUserID, message.Direction, message.MessageContent, message.SentAt);
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
