using System;
using System.Windows.Forms;

namespace YourNamespace
{
    public partial class ConfirmationDialog : Form
    {
        public bool UserConfirmed { get; private set; } 

        public ConfirmationDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UserConfirmed = true;
            this.DialogResult = DialogResult.Yes; 
            this.Close(); 
        }

        // Event handler for the No button
        private void button2_Click(object sender, EventArgs e)
        {
            UserConfirmed = false; 
            this.DialogResult = DialogResult.No; 
            this.Close(); 
        }
    }
}
