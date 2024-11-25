using System;
using System.Drawing;
using System.Windows.Forms;

namespace Aegis
{
    partial class Home_Page
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Home_Page));
            this.Messages_Label = new System.Windows.Forms.Label();
            this.Settings_Label = new System.Windows.Forms.Label();
            this.Messages_Panel = new System.Windows.Forms.Panel();
            this.Settings_Panel = new System.Windows.Forms.Panel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.Message_Label_Panel = new System.Windows.Forms.Panel();
            this.Session_Joiner = new System.Windows.Forms.Button();
            this.Session_maker = new System.Windows.Forms.Button();
            this.tableLayoutPanel.SuspendLayout();
            this.Message_Label_Panel.SuspendLayout();
            this.SuspendLayout();
            // 
            // Messages_Label
            // 
            this.Messages_Label.AutoSize = true;
            this.Messages_Label.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Messages_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.Messages_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Messages_Label.Location = new System.Drawing.Point(0, 0);
            this.Messages_Label.Name = "Messages_Label";
            this.Messages_Label.Size = new System.Drawing.Size(111, 25);
            this.Messages_Label.TabIndex = 2;
            this.Messages_Label.Text = "Messages";
            this.Messages_Label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Settings_Label
            // 
            this.Settings_Label.AutoSize = true;
            this.Settings_Label.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Settings_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Settings_Label.Location = new System.Drawing.Point(481, 0);
            this.Settings_Label.Name = "Settings_Label";
            this.Settings_Label.Size = new System.Drawing.Size(90, 25);
            this.Settings_Label.TabIndex = 3;
            this.Settings_Label.Text = "Settings";
            this.Settings_Label.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Messages_Panel
            // 
            this.Messages_Panel.AutoScroll = true;
            this.Messages_Panel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Messages_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Messages_Panel.Location = new System.Drawing.Point(3, 43);
            this.Messages_Panel.Name = "Messages_Panel";
            this.Messages_Panel.Size = new System.Drawing.Size(472, 574);
            this.Messages_Panel.TabIndex = 4;
            // 
            // Settings_Panel
            // 
            this.Settings_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Settings_Panel.AutoScroll = true;
            this.Settings_Panel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Settings_Panel.Location = new System.Drawing.Point(481, 43);
            this.Settings_Panel.Name = "Settings_Panel";
            this.Settings_Panel.Size = new System.Drawing.Size(1110, 574);
            this.Settings_Panel.TabIndex = 5;
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.ColumnCount = 2;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel.Controls.Add(this.Message_Label_Panel, 0, 0);
            this.tableLayoutPanel.Controls.Add(this.Settings_Label, 1, 0);
            this.tableLayoutPanel.Controls.Add(this.Messages_Panel, 0, 1);
            this.tableLayoutPanel.Controls.Add(this.Settings_Panel, 1, 1);
            this.tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 2;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel.Size = new System.Drawing.Size(1594, 620);
            this.tableLayoutPanel.TabIndex = 6;
            // 
            // Message_Label_Panel
            // 
            this.Message_Label_Panel.Controls.Add(this.Session_Joiner);
            this.Message_Label_Panel.Controls.Add(this.Session_maker);
            this.Message_Label_Panel.Controls.Add(this.Messages_Label);
            this.Message_Label_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Message_Label_Panel.Location = new System.Drawing.Point(3, 3);
            this.Message_Label_Panel.Name = "Message_Label_Panel";
            this.Message_Label_Panel.Size = new System.Drawing.Size(472, 34);
            this.Message_Label_Panel.TabIndex = 3;
            // 
            // Session_Joiner
            // 
            this.Session_Joiner.BackgroundImage = global::Aegis.Properties.Resources._8666749_plus_add_icon;
            this.Session_Joiner.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Session_Joiner.Dock = System.Windows.Forms.DockStyle.Right;
            this.Session_Joiner.Location = new System.Drawing.Point(386, 0);
            this.Session_Joiner.Name = "Session_Joiner";
            this.Session_Joiner.Size = new System.Drawing.Size(43, 34);
            this.Session_Joiner.TabIndex = 3;
            this.Session_Joiner.UseVisualStyleBackColor = true;
            this.Session_Joiner.Click += new System.EventHandler(this.Session_Joiner_Click);
            // 
            // Session_maker
            // 
            this.Session_maker.BackgroundImage = global::Aegis.Properties.Resources._8666681_edit_icon;
            this.Session_maker.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Session_maker.Dock = System.Windows.Forms.DockStyle.Right;
            this.Session_maker.Location = new System.Drawing.Point(429, 0);
            this.Session_maker.Name = "Session_maker";
            this.Session_maker.Size = new System.Drawing.Size(43, 34);
            this.Session_maker.TabIndex = 2;
            this.Session_maker.UseVisualStyleBackColor = true;
            this.Session_maker.Click += new System.EventHandler(this.Session_maker_Click);
            // 
            // Home_Page
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1594, 620);
            this.Controls.Add(this.tableLayoutPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Home_Page";
            this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultBounds;
            this.Text = "Aegis";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.tableLayoutPanel.ResumeLayout(false);
            this.tableLayoutPanel.PerformLayout();
            this.Message_Label_Panel.ResumeLayout(false);
            this.Message_Label_Panel.PerformLayout();
            this.ResumeLayout(false);

        }




        #endregion
        private System.Windows.Forms.Label Messages_Label;
        private System.Windows.Forms.Label Settings_Label;
        public System.Windows.Forms.Panel Messages_Panel;
        private System.Windows.Forms.Panel Settings_Panel;
        private Button Session_maker;
        private Panel Message_Label_Panel;
        private Button Session_Joiner;
        public TableLayoutPanel tableLayoutPanel;
    }
}