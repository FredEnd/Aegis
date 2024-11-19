using System.Drawing;

namespace Aegis
{
    partial class Startup_Page
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Startup_Page));
            this.Loading_screen_logo = new System.Windows.Forms.PictureBox();
            this.Main_title = new System.Windows.Forms.Label();
            this.System_Name = new System.Windows.Forms.Label();
            this.System_IP = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.Loading_screen_logo)).BeginInit();
            this.SuspendLayout();
            // 
            // Loading_screen_logo
            // 
            this.Loading_screen_logo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Loading_screen_logo.Image = global::Aegis.Properties.Resources.New_Project__41_;
            this.Loading_screen_logo.Location = new System.Drawing.Point(255, 88);
            this.Loading_screen_logo.Name = "Loading_screen_logo";
            this.Loading_screen_logo.Size = new System.Drawing.Size(509, 425);
            this.Loading_screen_logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.Loading_screen_logo.TabIndex = 0;
            this.Loading_screen_logo.TabStop = false;
            // 
            // Main_title
            // 
            this.Main_title.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)));
            this.Main_title.AutoSize = true;
            this.Main_title.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Main_title.Location = new System.Drawing.Point(296, 9);
            this.Main_title.Name = "Main_title";
            this.Main_title.Size = new System.Drawing.Size(426, 55);
            this.Main_title.TabIndex = 1;
            this.Main_title.Text = "Welcome to Aegis";
            this.Main_title.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // System_Name
            // 
            this.System_Name.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.System_Name.AutoSize = true;
            this.System_Name.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.System_Name.Location = new System.Drawing.Point(2, 428);
            this.System_Name.Name = "System_Name";
            this.System_Name.Size = new System.Drawing.Size(151, 25);
            this.System_Name.TabIndex = 2;
            this.System_Name.Text = "System Name:";
            // 
            // System_IP
            // 
            this.System_IP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.System_IP.AutoSize = true;
            this.System_IP.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.System_IP.Location = new System.Drawing.Point(2, 466);
            this.System_IP.Name = "System_IP";
            this.System_IP.Size = new System.Drawing.Size(187, 25);
            this.System_IP.TabIndex = 3;
            this.System_IP.Text = "Public IP Address:";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBox1.Location = new System.Drawing.Point(824, 332);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(166, 181);
            this.textBox1.TabIndex = 5;
            this.textBox1.Text = resources.GetString("textBox1.Text");
            this.textBox1.UseWaitCursor = true;
            // 
            // Startup_Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1002, 525);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.System_IP);
            this.Controls.Add(this.System_Name);
            this.Controls.Add(this.Main_title);
            this.Controls.Add(this.Loading_screen_logo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Startup_Page";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Aegis";
            ((System.ComponentModel.ISupportInitialize)(this.Loading_screen_logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Loading_screen_logo;
        private System.Windows.Forms.Label Main_title;
        private System.Windows.Forms.Label System_Name;
        private System.Windows.Forms.Label System_IP;
        private System.Windows.Forms.TextBox textBox1;
    }
}

