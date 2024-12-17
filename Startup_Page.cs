using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using Aegis_main;
using LocalDatabaseApp;
using YourNamespace;

namespace Aegis
{
    public partial class Startup_Page : Form
    {
        private bool? IsLocal;
        private string IPaddress = string.Empty;
        private string pcName = string.Empty;

        public Startup_Page()
        {
            ConfirmationDialog dialog = new ConfirmationDialog();

            dialog.Confirmation_Label.Text = "What kind of TCP connection do you want to make?";
            dialog.button1.Text = "Local";
            dialog.button2.Text = "Public";

            Mains.ExampleUsage();

            DialogResult result = dialog.ShowDialog();
            
            Console.WriteLine(result.ToString());

            if (result == DialogResult.Yes)
            {
                InitializeComponent();

                this.IsLocal = true;

                string pcName = Environment.MachineName;
                System_Name.Text = "System Name: " + pcName;

                // Get the public IP address
                string publicIP = Mains.GetLocalIPAddress();
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
            else
            {
                InitializeComponent();

                this.IsLocal = false;

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


        } //Creates a new dialgoue to decide if the application will open in local or public mode and then runs the loading form

        public async Task WaitAndOpenHome()
        {
            List<int> ports;

            if (this.IsLocal == null)
            {
                Console.WriteLine("IsLocal is undecided. Exiting application.");
                Application.Exit();
                return;
            }

            if (this.IsLocal == true)
            {
                ports = await Mains.TestLocalPortsAsync(IPaddress, 2500, 2800);
                Console.WriteLine("Retrieved local ports:");
                foreach (var port in ports)
                {
                    Console.WriteLine($"Port {port} is open.");
                }
            }
            else
            {
                ports = await Mains.TestPortsAsync(IPaddress, 0, 2800);
                Console.WriteLine("Retrieved open public ports:");
                foreach (var port in ports)
                {
                    Console.WriteLine($"Port {port} is open.");
                }
            }

            Mains.play_notifercation();
            await Task.Delay(3000);

            this.Hide();
            Home_Page homePage = new Home_Page(IPaddress, ports, pcName);

            Screen currentScreen = Screen.FromControl(this);
            homePage.StartPosition = FormStartPosition.Manual;
            homePage.Location = currentScreen.WorkingArea.Location;
            homePage.Show();
        } //opens a loading form which runs the port scanners to find open ports to message on

    }
}
