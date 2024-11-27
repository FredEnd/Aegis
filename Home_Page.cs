using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Aegis.Properties;
using Aegis_main;
using LocalDatabaseApp;
using YourNamespace;

namespace Aegis
{
    public partial class Home_Page : Form
    {
        public FlowLayoutPanel flowLayoutPanel;
        private Settings CurrentAppSettings;
        private List<int> Ports;
        private string IPaddress;
        private string pcName;
        


        public Home_Page(string IPaddress, List<int> Ports, string pcName)
        {
            if (IPaddress == "null")
            {
                Application.Exit();
            }

            else
            {
                InitializeComponent();

                this.Ports = Ports;
                this.IPaddress = IPaddress;
                this.pcName = pcName;

                Settings appSettings = Initialise_settings();
                CurrentAppSettings = appSettings;

                this.FormClosing += new FormClosingEventHandler(Mains.Forms_FormClosing);

                flowLayoutPanel = new FlowLayoutPanel();


                var chatSessions = DB.LoadChatSessions();
                if (chatSessions == null || chatSessions.Count == 0)
                {
                    Console.WriteLine("NO SESSIONS LOADED: NULL OR EMPTY");
                }
                else
                {
                    foreach (var session in chatSessions)
                    {
                        ChatSessionButton newChat = new ChatSessionButton(session.SessionID, session.CreatedAt, CurrentAppSettings, pcName, IPaddress);
                        newChat.InitializeButton();
                        Messages_Panel.Controls.Add(newChat.GetButton());
                    }
                }
            }
        }


        private Settings Initialise_settings()
        {
            // Assuming Settings_Panel is a panel already present in your form
            Panel settingsPanel = this.Settings_Panel;
            Settings appSettings = new Settings();


            // Clear any existing controls in case of reloading
            settingsPanel.Controls.Clear();
            settingsPanel.Dock = DockStyle.Fill;


            // Calculate dynamic dimensions based on panel size
            int controlWidth = (int)(settingsPanel.Width * 0.5);
            int controlHeight = (int)(settingsPanel.Height * 0.15);
            int labelHeight = (int)(settingsPanel.Height * 0.09);
            int spacing = (int)(settingsPanel.Height * 0.05);
            int startX = (int)(settingsPanel.Width * 0.1);
            int startY = (int)(settingsPanel.Height * 0.05);


            // systems Recall Button
            Button recallButton = new Button
            {
                Text = "Systems Recall",
                Name = "btnRecall",
                Size = new Size(controlWidth, controlHeight),
                Location = new Point(startX, startY),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            recallButton.Click += (s, e) =>
            {
                var content = DB.RecallDB();

                using (var messageBox = new ScrollableMessageBox(content, appSettings))
                {
                    messageBox.ShowDialog();
                }
            };
            settingsPanel.Controls.Add(recallButton);


            // delete DB Button (DONE)
            Button deleteDbButton = new Button
            {
                Text = "Delete Database",
                Name = "btnDeleteDb",
                Size = new Size(controlWidth, controlHeight),
                Location = new Point(startX, startY + controlHeight + spacing),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            deleteDbButton.Click += (s, e) =>
            {
                bool wasDeleted = deleteDBConfirmation();
                if (wasDeleted)
                {
                    DB.DeleteDb();
                    Application.Exit();
                }
            };

            settingsPanel.Controls.Add(deleteDbButton);


            // Light and Dark Mode Selector Label (DONE)
            Label themeLabel = new Label
            {
                Text = "Theme:",
                Size = new Size(controlWidth, labelHeight),
                Location = new Point(startX, startY + 2 * (controlHeight + spacing)),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            settingsPanel.Controls.Add(themeLabel);


            // Light and Dark Mode Selector (ComboBox)
            ComboBox themeSelector = new ComboBox
            {
                Items = { "Light", "Dark" },
                DropDownStyle = ComboBoxStyle.DropDownList,
                Name = "cmbTheme",
                Size = new Size(controlWidth, controlHeight),
                Location = new Point(startX, themeLabel.Location.Y + labelHeight + spacing),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };


            // set the initial value of the ComboBox from settings
            themeSelector.SelectedItem = appSettings.Theme;


            // update settings when the selection changes
            themeSelector.SelectedIndexChanged += (s, e) =>
            {
                appSettings.Theme = themeSelector.SelectedItem.ToString();
                ApplyThemeHomePage(appSettings.Theme);
            };
            settingsPanel.Controls.Add(themeSelector);


            // font Size Selector Label (DONE)
            Label fontSizeLabel = new Label
            {
                Text = "Font Size:",
                Size = new Size(controlWidth, labelHeight),
                Location = new Point(startX, themeSelector.Location.Y + controlHeight + spacing),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            settingsPanel.Controls.Add(fontSizeLabel);


            // font size selector (NumericUpDown)
            NumericUpDown fontSizeSelector = new NumericUpDown
            {
                Minimum = 8,
                Maximum = 20,
                Name = "nudFontSize",
                Size = new Size(controlWidth, controlHeight),
                Location = new Point(startX, fontSizeLabel.Location.Y + labelHeight + spacing),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };


            // set the initial value of NumericUpDown from settings
            fontSizeSelector.Value = appSettings.FontSize;


            // update settings when the value changes
            fontSizeSelector.ValueChanged += (s, e) =>
            {
                appSettings.FontSize = (int)fontSizeSelector.Value;
                // optionally, apply the font size to your application
                ApplyFontSize(appSettings.FontSize);
            };
            settingsPanel.Controls.Add(fontSizeSelector);


            // notifications Toggle Button (CheckBox) (Not done yet)
            CheckBox notificationsToggle = new CheckBox
            {
                Text = "Enable Notifications",
                Name = "chkNotifications",
                Size = new Size(controlWidth, controlHeight),
                Location = new Point(startX, fontSizeSelector.Location.Y + controlHeight + spacing),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };


            // set the initial checked state from settings
            notificationsToggle.Checked = appSettings.NotificationsEnabled;


            // update settings when the checkbox state changes
            notificationsToggle.CheckedChanged += (s, e) =>
            {
                appSettings.NotificationsEnabled = notificationsToggle.Checked;
                MessageBox.Show($"Notifications {(appSettings.NotificationsEnabled ? "enabled" : "disabled")}.");
            };
            settingsPanel.Controls.Add(notificationsToggle);

            return appSettings;
        }

        private bool deleteDBConfirmation()
        {
            using (var confirmationDialog = new ConfirmationDialog())
            {
                var result = confirmationDialog.ShowDialog();
                return confirmationDialog.UserConfirmed; 
            }
        }

        // method to apply the theme to my application 
        private void ApplyThemeHomePage(string theme)
        {
            // Implement theme application logic here
            if (theme == "Dark")
            {
                this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                Settings_Label.BackColor = System.Drawing.SystemColors.ControlDark;
                Settings_Panel.BackColor = System.Drawing.SystemColors.ControlDark;
                Messages_Label.BackColor = System.Drawing.SystemColors.ControlDark;
                Messages_Panel.BackColor = System.Drawing.SystemColors.ControlDark;
            }
            else
            {
                this.BackColor = System.Drawing.SystemColors.ControlDark;
                Settings_Label.BackColor = System.Drawing.SystemColors.ControlLight;
                Settings_Panel.BackColor = System.Drawing.SystemColors.ControlLight;
                Messages_Label.BackColor = System.Drawing.SystemColors.ControlLight;
                Messages_Panel.BackColor = System.Drawing.SystemColors.ControlLight;
            }
        }

        private void ApplyFontSize(int fontSize)
        {
            foreach (Control control in this.Controls)
            {
                control.Font = new Font(control.Font.FontFamily, fontSize);
            }
        }

        private void ApplyFontSizeOtherForms(Control control, int fontSize)
        {
            if (!(control is Label))
            {
                control.Font = new Font(control.Font.FontFamily, fontSize);
            }

            foreach (Control ControlElements in control.Controls)
            {
                ApplyFontSize(fontSize);
            }
        }

        private void Session_maker_Click(object sender, EventArgs e)
        {
            Screen currentScreen = Screen.FromControl(Session_maker);
            Session_Settings NewSessionSettings = new Session_Settings(Ports, IPaddress, pcName, this, CurrentAppSettings);

            // Check the theme in the settings and apply the appropriate colors
            if (CurrentAppSettings.Theme == "Dark")
            {
                NewSessionSettings.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                NewSessionSettings.MainPanel.BackColor = System.Drawing.SystemColors.ControlDark;
                NewSessionSettings.SessionLabelPanel.BackColor = System.Drawing.SystemColors.ControlDark;
                NewSessionSettings.SessionSettingsLabel.BackColor = System.Drawing.SystemColors.ControlDark;
                Console.WriteLine("MESSAGER THEME CHANGED TO DARK");
            }
            else
            {
                NewSessionSettings.BackColor = System.Drawing.SystemColors.ControlDark;
                NewSessionSettings.MainPanel.BackColor = System.Drawing.SystemColors.ControlLight;
                NewSessionSettings.SessionLabelPanel.BackColor = System.Drawing.SystemColors.ControlLight;
                NewSessionSettings.SessionSettingsLabel.BackColor = System.Drawing.SystemColors.ControlLight;
                Console.WriteLine("MESSAGER THEME CHANGED TO LIGHT");
            }

            ApplyFontSizeOtherForms(NewSessionSettings, CurrentAppSettings.FontSize);

            NewSessionSettings.StartPosition = FormStartPosition.Manual;
            NewSessionSettings.Location = currentScreen.WorkingArea.Location;
            NewSessionSettings.Show();
        }

        private void Session_Joiner_Click(object sender, EventArgs e)
        {

        }
    }

    public class Settings
    {
        public string Theme { get; set; } = "Dark";
        public int FontSize { get; set; } = 8;
        public bool NotificationsEnabled { get; set; } = true;
    }

    public class ChatSessionButton
    {
        // decalare the attributes of the class
        private string sessionID;
        private string createdAt;
        private Settings appSettings;
        private Button sessionButton;
        private string UserID;
        private string IPaddress;

        public ChatSessionButton(string sessionID, string createdAt,Settings settings, string UserID, string IPaddress)
        {
            // initalise the button styling 
            this.sessionID = sessionID;
            this.createdAt = createdAt;
            this.appSettings = settings;
            this.UserID = UserID;
            this.IPaddress = IPaddress;
            InitializeButton();
        }

        public void InitializeButton()
        {
            // button styling
            sessionButton = new Button();
            sessionButton.Text = $"Session {sessionID}\nCreated: {createdAt}";
            sessionButton.Size = new Size(280, 60);  // adjust size as needed
            sessionButton.Margin = new Padding(10);
            sessionButton.Dock = DockStyle.Top;
            sessionButton.Click += (sender, e) => OpenSessionForm();
        }

        public void OpenSessionForm()
        {
            Screen currentScreen = Screen.FromControl(sessionButton);
            Message_Window sessionForm = new Message_Window(sessionID, UserID, IPaddress);

            // Check the theme in the settings and apply the appropriate colors
            if (appSettings.Theme == "Dark")
            {
                sessionForm.BackColor = System.Drawing.SystemColors.ControlDarkDark;
                sessionForm.Session_Label.BackColor = System.Drawing.SystemColors.ControlDark;
                sessionForm.Message_Panel.BackColor = System.Drawing.SystemColors.ControlDark;
                sessionForm.Input_Panel.BackColor = System.Drawing.SystemColors.ControlDark;
                Console.WriteLine("MESSAGER THEME CHANGED TO DARK");
            }
            else
            {
                sessionForm.BackColor = System.Drawing.SystemColors.ControlDark;
                sessionForm.Session_Label.BackColor = System.Drawing.SystemColors.ControlLight;
                sessionForm.Message_Panel.BackColor = System.Drawing.SystemColors.ControlLight;
                sessionForm.Input_Panel.BackColor = System.Drawing.SystemColors.ControlLight;
                Console.WriteLine("MESSAGER THEME CHANGED TO LIGHT");
            }

            ApplyFontSize(sessionForm, appSettings.FontSize);

            sessionForm.Text = $"{sessionID}";
            sessionForm.Session_Label.Text = $"Session: {sessionID}";
            sessionForm.StartPosition = FormStartPosition.Manual;
            sessionForm.Location = currentScreen.WorkingArea.Location;
            sessionForm.Show();
        }

        private void ApplyFontSize(Control control, int fontSize)
        {
            if (!(control is Label))
            {
                control.Font = new Font(control.Font.FontFamily, fontSize);
            }

            foreach (Control ControlElements in control.Controls)
            {
                ApplyFontSize(ControlElements, fontSize);  
            }
        }

        public Button GetButton()
        {
            return sessionButton;
        }
    }

    public class Session
    {
        private string SessionID { get; set; }
        private string HostUserID { get; set; }
        private string Encryption {  get; set; }
        private int portNum { get; set; }

        public Session(string sessionId, string hostUserId, string encryptionType, int portNum = 0)
        {
            this.SessionID = sessionId;
            this.HostUserID = hostUserId;
            this.Encryption = encryptionType;
            this.portNum = portNum; 
        }

        public Session() { }

        public void Add_Session()
        {
            DB.Create_Session(SessionID, HostUserID, Encryption, portNum);
        }
    }
}
