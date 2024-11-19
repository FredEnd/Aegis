using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Aegis
{
    public partial class ScrollableMessageBox : Form
    {
        private RichTextBox richTextBox;
        private Button closeButton;
        private Settings Settings;

        public ScrollableMessageBox(string content, Settings appSettings)
        {

            this.Text = "Database Content";
            this.Size = new Size(600, 400);
            this.StartPosition = FormStartPosition.CenterScreen;

            richTextBox = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                Text = content,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            closeButton = new Button
            {
                Text = "Close",
                Dock = DockStyle.Bottom,
                Height = 40
            };
            closeButton.Click += (sender, e) => this.Close();

            this.Controls.Add(richTextBox);
            this.Controls.Add(closeButton);
        }
    }
}
