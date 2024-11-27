﻿using LocalDatabaseApp;
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


namespace Aegis
{
    public partial class Message_Window : Form
    {
        private FlowLayoutPanel flowLayoutPanel;
        private readonly string sessionId;
        public string UserID;

        public Message_Window(string sessionId, string UserID, string IPaddress, List<int> Ports)
        {
            InitializeComponent();
            this.sessionId = sessionId;
            this.UserID = UserID;
            _ = StartUpMessages();

            List<(string HostUserID, string Encryption, int portNum)> sessionSettings = DB.LoadSessionSettings(sessionId);

            if (sessionSettings.Count > 0)
            {
                var setting = sessionSettings[0];

                if (!Ports.Contains(setting.portNum))
                {
                    MessageBox.Show("Please note that the port of this session is not in the port list that we have scanned from your network, therefore this chat wont be functional however you can still read from it...");
                }

                string ChatCode = Mains.GenerateSessionCode(IPaddress, setting.portNum, sessionId);

                TextBox Code = new TextBox();
                Session_Panel.Controls.Add(Code);
                Code.Dock = DockStyle.Right;
                Code.ReadOnly = true;
                Code.Text = ChatCode;

                _ = Mains.StartServerAsync(setting.portNum);
                Console.WriteLine("TCP SERVER LISTENING");
            }
            else
            {
                Console.WriteLine("No session settings found.");
            }

            this.FormClosing += Message_Window_FormClosing;
        }

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

            await DB.NewMessageFromClientAsync(MessageInput, sessionId, UserID);
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
