using LocalDatabaseApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Aegis
{
    public partial class Session_Settings : Form
    {
        private List<int> Ports;
        private string IPaddress;
        private string pcName;
        public Home_Page Home_Page;
        public Settings Settings;


        public Session_Settings(List<int> Ports, string IPaddress, string pcName, Home_Page home_page, Settings CurrentAppSettings)
        {
            InitializeComponent();

            this.Ports = Ports;
            this.IPaddress = IPaddress;
            this.pcName = pcName;
            this.Home_Page = home_page;
            this.Settings = CurrentAppSettings;


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

                EncryptionCombo.Items.AddRange(Algorithms.ToArray());
            }
            else
            {
                MessageBox.Show("No Ports Please use a network that gives open port");
                Application.Exit();
            }
        }

        public static List<string> Algorithms { get; } = new List<string>
        {
            "AES",
            "RSA",
            "DES"
        };

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
                    ChatSessionButton newChat = new ChatSessionButton(session.SessionID, session.CreatedAt, Settings, pcName, IPaddress, Ports);
                    newChat.InitializeButton();
                    Home_Page.Messages_Panel.Controls.Add(newChat.GetButton());
                }
            }   
        }

        private void Create_Session_Click(object sender, EventArgs e)
        {
            var SessionIDInput = SessionID_Input.Text;
            if (string.IsNullOrWhiteSpace(SessionIDInput))
            {
                MessageBox.Show("Session ID cannot be null or empty.");
                return;
            }
            Console.WriteLine(SessionIDInput);

            if (PortsCombo.SelectedItem == null)
            {
                MessageBox.Show("No port selected.");
                return;
            }
            var SelectedPort = Convert.ToInt32(PortsCombo.SelectedItem);
            Console.WriteLine($"{SelectedPort}, -- Selected");

            if (EncryptionCombo.SelectedItem == null)
            {
                MessageBox.Show("No encryption method selected.");
                return;
            }
            string encryptionMethod = EncryptionCombo.SelectedItem.ToString();

            Session newSession = new Session(SessionIDInput, pcName, encryptionMethod, SelectedPort);

            newSession.Add_Session();

            Refresh_Sessions();

            this.Close();

        }
    }
}
