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
using System.IO;
using static Aegis.Message_Window;
using System.Runtime.InteropServices.ComTypes;
using Aegis.Properties;
using System.Security.Cryptography;
using System.Net.Http;
using System.Net.NetworkInformation;


namespace Aegis
{
    public partial class Message_Window : Form
    {
        // main variables
        private FlowLayoutPanel flowLayoutPanel;
        private readonly string sessionId;
        public string IPaddress;
        public string HUserID;
        public string CUserID;
        public string EncryptionType;
        private Settings AppSettings;

        //old variables
        private NetworkStream CurrentStreamH;
        private NetworkStream CurrentStreamC;

        //Both varibales
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private TcpListener CurrentListener;

        //new method variables
        public Dictionary<string, TcpClient> connectedClients = new Dictionary<string, TcpClient>();
        private TcpClient TcpClient = null;

        private string recipientUserID = null;



        public Message_Window(string sessionId, string UserID, string IPaddress, List<int> Ports, Settings Appsettings, Dictionary<string, TcpClient> connectedClients)
        {
            List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

            var setting = sessionSettings[0];
            this.sessionId = sessionId;
            this.HUserID = setting.HostUserID;
            this.CUserID = UserID;
            this.AppSettings = Appsettings;
            this.IPaddress = IPaddress;
            this.EncryptionType = setting.Encryption;

            if (UserID != setting.HostUserID)
            {
                this.connectedClients = connectedClients;

                //this.TcpClient = this.connectedClients[sessionId];

                InitializeComponent();
                _ = StartUpMessages();

                _ = InitializeMessageWindowClientAsync(IPaddress, setting.portNum);
            }

            else
            {
                InitializeComponent();
                _ = StartUpMessages();
                _ = StartListenerAsync(setting.portNum);
                //_ = InitializeMessageWindowHostAsync(IPaddress, Ports);
            }

            this.FormClosing += Message_Window_FormClosing;
        } //Initalise the Message window and construct the elements and if it is a host cleint or a client client...

        //-------------------------------------------------------------------------------------------------

        private async Task InitializeMessageWindowClientAsync(string ipAddress, int port)
        {
            try
            {
                Console.WriteLine("Client version");

                Console.WriteLine($"{ipAddress}, {port}");

                List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

                if (sessionSettings == null || sessionSettings.Count == 0)
                {
                    Console.WriteLine("Error: No session settings found.");
                    return;
                }

                var setting = sessionSettings[0];

                if (this.TcpClient == null || !this.TcpClient.Connected)
                {
                    try
                    {
                        TcpClient Client = new TcpClient();

                        this.TcpClient = Client;
                        Console.WriteLine($"Attempting to connect to server {ipAddress}:{port}");
                        await TcpClient.ConnectAsync(ipAddress, port);
                        Console.WriteLine($"Connected to server {ipAddress}:{port}");
                    }
                    catch (SocketException ex)
                    {
                        Console.WriteLine($"Socket error during connection: {ex.Message}");
                        return;
                    }
                }

                NetworkStream stream = TcpClient.GetStream();

                // Send a request message
                string request = $"GET_SESSION_INFO: | USERID: ";
                byte[] requestBytes = Encoding.UTF32.GetBytes(request);
                await stream.WriteAsync(requestBytes, 0, requestBytes.Length);
                Console.WriteLine("Request sent for session data.");

                // Receive a response
                byte[] responseBuffer = new byte[4096];
                int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);
                if (bytesRead > 0)
                {
                    string response = Encoding.UTF32.GetString(responseBuffer, 0, bytesRead);
                    Console.WriteLine($"Response received: {response}");
                }
                else
                {
                    Console.WriteLine("No data received from server.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in InitializeMessageWindowClientAsync: {ex.Message}");
            }
            finally
            {
                if (TcpClient != null)
                {
                    TcpClient.Close();
                    Console.WriteLine("TcpClient closed.");
                }
            }
        } //Tries to make the ocnnection Logic is sound and it should work, however it is highly depended on the Network settings and the handling for your network settingss, if you want to establish a connection you will need to configue your network such that it will work.



        private async Task sendMessageToListener(string message, TcpClient client)
        {
            try
            {
                if (client.Connected)
                {
                    NetworkStream stream = client.GetStream();

                    string messageToSend = $"USERID:{CUserID} | {message}";
                    byte[] messageBytes = Encoding.UTF32.GetBytes(messageToSend);

                    await stream.WriteAsync(messageBytes, 0, messageBytes.Length);
                    Console.WriteLine($"Message Sent: {messageToSend}");

                    byte[] responseBuffer = new byte[4096];
                    int bytesRead = await stream.ReadAsync(responseBuffer, 0, responseBuffer.Length);

                    if (bytesRead > 0)
                    {
                        string response = Encoding.UTF32.GetString(responseBuffer, 0, bytesRead);
                        Console.WriteLine($"Response from server: {response}");
                    }
                    else
                    {
                        Console.WriteLine("No response received from server.");
                    }
                }
                else
                {
                    Console.WriteLine("TCP client is no longer connected.");
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error during message transmission: {ex.Message}");
            }
            catch (Exception ex)
            {
                // Handle other types of errors
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        } //Sends a message from the Client Client to the Host Client




        //-------------------------------------------------------------------------------------------------

        private async Task InitializeMessageWindowHostAsync(string IPaddress, List<int> Ports)
        {
            Console.WriteLine("host mode");

            try
            {
                // Load session settings
                List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

                if (sessionSettings.Count == 0)
                {
                    Console.WriteLine("No session settings found.");
                    return;
                }

                var setting = sessionSettings[0];
                this.EncryptionType = setting.Encryption;

                string ChatCode = Mains.GenerateSessionCode(IPaddress, setting.portNum, sessionId, setting.HostUserID, setting.Encryption);

                TextBox Code = new TextBox
                {
                    Dock = DockStyle.Right,
                    ReadOnly = true,
                    Text = ChatCode
                };
                Session_Panel.Controls.Add(Code);

                IPAddress TargetIP = IPAddress.Parse(IPaddress);
                Console.WriteLine(TargetIP);

                var token = _cancellationTokenSource.Token;

                TcpListener listener = new TcpListener(TargetIP, setting.portNum);
                this.CurrentListener = listener;
                listener.Start();

                Console.WriteLine($"Server listening on port {setting.portNum}...");

                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("New client connected.");

                    try
                    {
                        using (var stream = client.GetStream())
                        {
                            this.CurrentStreamH = stream;

                            while (client.Connected)
                            {
                                try
                                {
                                    var (message, clientStream) = await Mains.HandleClientAsync(client);
                                    if (string.IsNullOrEmpty(message))
                                    {
                                        Console.WriteLine("Received null message from client.");
                                        continue;
                                    }

                                    Console.WriteLine("Processing message...");
                                    HandleIncomingMessage(message, client, clientStream);
                                }
                                catch (IOException ioEx)
                                {
                                    Console.WriteLine($"Client disconnected unexpectedly: {ioEx.Message}");
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine($"Error processing message: {ex.Message}");
                                }
                            }
                        }
                    }
                    finally
                    {
                        client?.Close();
                        Console.WriteLine("Client connection closed.");
                    }
                }
                catch (ObjectDisposedException)
                {
                    Console.WriteLine("Listener stopped. Exiting...");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error accepting client: {ex.Message}");
                }
                finally
                {
                    listener.Stop();
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in server initialization: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("Listener function final call");
            }
        } //Starts up the host connection see the comment on the client to find out about the network, however the logic on this is also sound and will work, it just needs adjusting for the network it is on and given propper permissons.


        private void HandleIncomingMessage(string message, TcpClient client, NetworkStream stream)
        {
            Console.WriteLine("HandleIncomingMessage");

            if (string.IsNullOrEmpty(message))
            {
                Console.WriteLine("Received an empty message.");
                return;
            }

            try
            {
                string[] commands = message.Split('|');

                foreach (string command in commands)
                {
                    
                    string trimmedCommand = command.Trim();
                    _ = ProcessCommandOrMessage(trimmedCommand, client, stream);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing message: {ex.Message}");
            }
        } //host handles the incomming message


        private void StopServer()
        {
            try
            {
                if (_cancellationTokenSource != null)
                {
                    _cancellationTokenSource.Cancel();
                    _cancellationTokenSource.Dispose();
                    _cancellationTokenSource = null;
                }

                if (CUserID != HUserID && CurrentListener != null)
                {
                    CurrentListener.Stop();
                    CurrentListener = null;
                }

                Console.WriteLine("Server stopped successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error stopping server: {ex.Message}");
            }
        } //Stops the host listeneing

        private async Task StartListenerAsync(int port)
        {
            List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

            if (sessionSettings.Count == 0)
            {
                Console.WriteLine("No session settings found.");
                return;
            }

            var setting = sessionSettings[0];
            this.EncryptionType = setting.Encryption;

            string ChatCode = Mains.GenerateSessionCode(IPaddress, setting.portNum, sessionId, setting.HostUserID, setting.Encryption);

            TextBox Code = new TextBox
            {
                Dock = DockStyle.Right,
                ReadOnly = true,
                Text = ChatCode
            };
            Session_Panel.Controls.Add(Code);

            IPAddress ip = IPAddress.Parse(this.IPaddress);

            try
            {
                TcpListener listener = new TcpListener(IPAddress.Parse(this.IPaddress), port);
                listener.Start();
                Console.WriteLine($"Server listening on port {port}...");

                while (true)
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("New client connected.");
                    _ = HandleClientAsync(client); // Handle clients without stopping the listener
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Listener error: {ex.Message}");
            }


        } //Old start listener function

        private async Task HandleClientAsync(TcpClient client)
        {
            string clientId = Guid.NewGuid().ToString();
            connectedClients[clientId] = client;

            try
            {
                using (var stream = client.GetStream())
                {
                    byte[] buffer = new byte[4096];

                    while (true)
                    {
                        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (bytesRead == 0) break;

                        string message = Encoding.UTF32.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Received from {clientId}: {message}");

                        if (message.StartsWith("GET_SESSION_INFO"))
                        {
                            HandleIncomingMessage(message, client, stream);
                            break;
                        }
                        else
                        {
                            Console.WriteLine("NON GET REQUEST");
                            HandleIncomingMessage(message, client, stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client {clientId}: {ex.Message}");
            }
            finally
            {
                client.Close();
                connectedClients.Remove(clientId);
                Console.WriteLine($"Client {clientId} disconnected.");
            }
        } //Handle the Client connection


        //-------------------------------------------------------------------------------------------------

        private async Task ProcessCommandOrMessage(string command, TcpClient client, NetworkStream stream)
        {
            Console.WriteLine("PROCESSING MESSAGE");

            const string getSessionInfoPrefix = "GET_SESSION_INFO:";
            const string userIdPrefix = "USERID:";
            const string ivPrefix = "IV:";
            const string keyPrefix = "KEY:";
            const string rsaPrivateKeyPrefix = "RSA_PK:";

            string encryptionIV = null;
            string encryptionKey = null;
            string rsaPrivateKey = null;
            RSAParameters rsaParameters = default;

            try
            {
                if (string.IsNullOrWhiteSpace(command))
                {
                    Console.WriteLine("Error: Received an empty or null command.");
                    return;
                }

                // 1. Handle USERID prefix
                if (command.StartsWith(userIdPrefix))
                {
                    string userId = command.Substring(userIdPrefix.Length).Trim();
                    if (!string.IsNullOrEmpty(userId))
                    {
                        this.recipientUserID = userId;
                        Console.WriteLine($"USERID set to: {recipientUserID}");
                    }
                    else
                    {
                        Console.WriteLine("Error: USERID command received but no ID was provided.");
                    }
                    return; // USERID set, no further processing required
                }

                // 2. Handle GET_SESSION_INFO prefix
                if (command.StartsWith(getSessionInfoPrefix))
                {
                    string sessionInfo = $"SessionID: {sessionId}, UserID: {recipientUserID}";
                    Console.WriteLine($"Sending session info: {sessionInfo}");

                    if (stream.CanWrite)
                    {
                        byte[] response = Encoding.UTF32.GetBytes(sessionInfo);
                        await stream.WriteAsync(response, 0, response.Length);
                    }
                    return;
                }

                // 3. Handle RSA private key prefix
                if (command.StartsWith(rsaPrivateKeyPrefix))
                {
                    rsaPrivateKey = command.Substring(rsaPrivateKeyPrefix.Length).Trim();
                    if (!string.IsNullOrEmpty(rsaPrivateKey))
                    {
                        Console.WriteLine("RSA private key received.");
                        rsaParameters = Base64ToRSAParameters(rsaPrivateKey);
                    }
                    else
                    {
                        Console.WriteLine("RSA private key command received but no key was provided.");
                    }
                    return;
                }

                // 4. Handle IV prefix
                if (command.StartsWith(ivPrefix))
                {
                    encryptionIV = command.Substring(ivPrefix.Length).Trim();
                    Console.WriteLine($"Encryption IV received: {encryptionIV}");
                    return;
                }

                // 5. Handle KEY prefix
                if (command.StartsWith(keyPrefix))
                {
                    encryptionKey = command.Substring(keyPrefix.Length).Trim();
                    Console.WriteLine($"Encryption Key received: {encryptionKey}");
                    return;
                }

                // 6. Standard message handling
                Console.WriteLine($"Standard message received: {command}");

                if (string.IsNullOrEmpty(this.recipientUserID))
                {
                    Console.WriteLine("Error: Cannot process message without a USERID.");
                    if (stream.CanWrite)
                    {
                        byte[] errorResponse = Encoding.UTF32.GetBytes("Error: USERID not set. Please send USERID command first.");
                        await stream.WriteAsync(errorResponse, 0, errorResponse.Length);
                    }
                    return;
                }

                string decryptedMessage = command;

                // 7. Decrypt message if encryption data is available
                if (rsaParameters.Modulus != null || (!string.IsNullOrEmpty(encryptionIV) && !string.IsNullOrEmpty(encryptionKey)))
                {
                    try
                    {
                        byte[] commandBytes = Encoding.UTF32.GetBytes(command);
                        List<string> encodedMessageAsList = commandBytes.Select(b => b.ToString()).ToList();

                        decryptedMessage = Mains.DecryptDataWithKeyTransport(encodedMessageAsList, encryptionKey, this.EncryptionType, rsaParameters, encryptionIV);
                        Console.WriteLine($"Decrypted message: {decryptedMessage}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error decrypting the message: {ex.Message}");
                        if (stream.CanWrite)
                        {
                            byte[] errorResponse = Encoding.UTF32.GetBytes("Error: Decryption failed.");
                            await stream.WriteAsync(errorResponse, 0, errorResponse.Length);
                        }
                        return;
                    }
                }

                // 8. Save decrypted message to the database
                try
                {
                    Console.WriteLine(this.recipientUserID);
                    DB.Create_Message(sessionId, this.recipientUserID, decryptedMessage, "received");
                    Console.WriteLine("Message successfully written to the database.");

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
                catch (Exception ex)
                {
                    Console.WriteLine($"Error saving message to the database: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        } //Preocesses the prefixes from a message


        // RSA Decryption Logic
        private string DecryptMessageWithRSA(string encryptedMessage, RSAParameters privateKey)
        {
            using (var rsa = RSA.Create())
            {
                rsa.ImportParameters(privateKey);
                byte[] encryptedBytes = Convert.FromBase64String(encryptedMessage);
                byte[] decryptedBytes = rsa.Decrypt(encryptedBytes, RSAEncryptionPadding.Pkcs1);
                return Encoding.UTF32.GetString(decryptedBytes);
            }
        } //unused decrypt function that isnt used because it was used for itinial testing

        public static RSAParameters Base64ToRSAParameters(string base64String)
        {
            byte[] rsaBytes = Convert.FromBase64String(base64String);

            return GetRSAParametersFromBytes(rsaBytes);
        }

        private static RSAParameters GetRSAParametersFromBytes(byte[] rsaBytes)
        {
            using (var ms = new System.IO.MemoryStream(rsaBytes))
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                return (RSAParameters)formatter.Deserialize(ms);
            }
        }

        public static string RSAParametersToBase64(RSAParameters rsaParams)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(ms, rsaParams);
                return Convert.ToBase64String(ms.ToArray());
            }
        }

        //all RSA functions above are for converting RSA parameters into base 64 stings and bacl.






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
                else
                {
                    StopServer();
                }
            }
        } //Handles the form closing differently to the main one.

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
        } //loads up the starting saved messages for this form

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

        } //Message instance.

        //-------------------------------------------------------------------------------------------------

        private void Emoji_Button_Click(object sender, EventArgs e)
        {
            EmojiForm emojiPanelForm = new EmojiForm(this.Input_Box);
            Screen currentScreen = Screen.FromControl(this);
            emojiPanelForm.StartPosition = FormStartPosition.Manual;
            emojiPanelForm.Location = currentScreen.WorkingArea.Location;

            emojiPanelForm.Show();

            Console.WriteLine("EmojiBox Shown");
        } //Loads up the emoji button

        private void File_Upload_Button_Click(object sender, EventArgs e)
        {

        } //Not inplemented ran out of time and probrably wont be able to test without relyable TCP connection which requires a properly set up network which i also didnt have time for.

        private bool IsConnectionEstablished()
        {
            if (HUserID == CUserID)
            {
                return CurrentStreamH != null && CurrentStreamH.CanWrite;
            }
            else
            {
                return CurrentStreamC != null && CurrentStreamC.CanWrite;
            }
        } //Old connection established function

        private async void Send_button_Click(object sender, EventArgs e)
        {

            var MessageInput = Input_Box.Text;
            Console.WriteLine(MessageInput);
            Console.WriteLine(sessionId);

            if (string.IsNullOrEmpty(sessionId))
            {
                Console.WriteLine("sessionId is null or empty.");
                return;
            }

            await DB.NewMessageFromClientAsync(MessageInput, sessionId, HUserID);
            Console.WriteLine("SUCCESSFULLY WRITTEN MESSAGE");

            // For host messages
            if (HUserID == CUserID)
            {
                Console.WriteLine("host message");

                (RSAParameters publicKey, RSAParameters privateKey) = Mains.GenerateRSAKeyPair();

                (List<string> EncryptedDataChunks, string EncryptedKey, string IV) = Mains.EncryptDataWithKeyTransport(MessageInput, this.EncryptionType, publicKey);

                if (CurrentStreamH != null && CurrentStreamH.CanWrite)
                {
                    try
                    {
                        string message = $"USERID:{HUserID} | {MessageInput}";

                        if (!string.IsNullOrEmpty(IV))
                        {
                            message += $" | IV:{IV}";
                        }

                        if (!string.IsNullOrEmpty(EncryptedKey))
                        {
                            message += $" | KEY:{EncryptedKey}";
                        }

                        if (privateKey.Modulus != null)
                        {
                            message += $" | RSA_PK:{RSAParametersToBase64(privateKey)}";
                        }

                        byte[] response = Encoding.UTF32.GetBytes(message);
                        await CurrentStreamH.WriteAsync(response, 0, response.Length);
                        Console.WriteLine("Message sent to client.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending message: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine("No Stream / TCP Connection for host.");
                    MessageBox.Show("No Stream / TCP Connection");
                }
            }

            // For client
            if (HUserID != CUserID)
            {
                (RSAParameters publicKey, RSAParameters privateKey) = Mains.GenerateRSAKeyPair();

                (List<string> EncryptedDataChunks, string EncryptedKey, string IV) = Mains.EncryptDataWithKeyTransport(MessageInput, this.EncryptionType, publicKey);

                if (CurrentStreamH != null && CurrentStreamH.CanWrite)
                {
                    try
                    {
                        string message = $"USERID:{HUserID} | {MessageInput}";

                        if (!string.IsNullOrEmpty(IV))
                        {
                            message += $" | IV:{IV}";
                        }

                        else if (!string.IsNullOrEmpty(EncryptedKey))
                        {
                            message += $" | KEY:{EncryptedKey}";
                        }

                        else if (privateKey.Modulus != null)
                        {
                            message += $" | RSA_PK:{privateKey}";
                        }

                        await sendMessageToListener(MessageInput, this.TcpClient);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error sending message: {ex.Message}");
                    }
                }
            }

            // Clear input text after sending the message
            Input_Box.Text = string.Empty;

            // Refresh chat messages
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
        } //Handles the client or host clients sending a message to the other.
    }
}


