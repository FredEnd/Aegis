﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis_main;
using LocalDatabaseApp;

namespace Aegis
{
    public partial class Startup_Page : Form
    {
        private string IPaddress = string.Empty;
        private string pcName = string.Empty;

        public Startup_Page()
        {
            InitializeComponent();

            string pcName = Environment.MachineName;
            System_Name.Text = "System Name: " + pcName;

            // Get the public IP address
            string publicIP = Mains.GetPublicIPAddress();
            this.IPaddress = publicIP;
            this.pcName = pcName;

            System_IP.Text = "Public IP Address: " + publicIP;

            try
            {
                DB.Database_Check();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in Database_Check: {ex.Message}");
            }

            //DB.Database_Test_Input_sessions(pcName, publicIP);

            _ = WaitAndOpenHome();
        }

        public async Task WaitAndOpenHome()
        {
            List<int> ports = await Task.Run(() => Mains.TestPorts(IPaddress, 0, 1000));

            Mains.play_notifercation();
            await Task.Delay(3000);

            this.Hide();

            Home_Page homePage = new Home_Page(IPaddress, ports, pcName);
            Screen currentScreen = Screen.FromControl(this);
            homePage.StartPosition = FormStartPosition.Manual;
            homePage.Location = currentScreen.WorkingArea.Location;
            homePage.Show();
        }
    }
}
