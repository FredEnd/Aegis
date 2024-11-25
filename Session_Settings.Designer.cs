namespace Aegis
{
    partial class Session_Settings
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
            this.SessionSettingsMainTable = new System.Windows.Forms.TableLayoutPanel();
            this.MainPanel = new System.Windows.Forms.Panel();
            this.SettingsTable = new System.Windows.Forms.TableLayoutPanel();
            this.EncryptionCombo = new System.Windows.Forms.ComboBox();
            this.SessionIDLabel = new System.Windows.Forms.Label();
            this.Port_Label = new System.Windows.Forms.Label();
            this.EncryptionSettingLabel = new System.Windows.Forms.Label();
            this.SessionID_Input = new System.Windows.Forms.TextBox();
            this.PortsCombo = new System.Windows.Forms.ComboBox();
            this.SessionLabelPanel = new System.Windows.Forms.Panel();
            this.Create_Session = new System.Windows.Forms.Button();
            this.SessionSettingsLabel = new System.Windows.Forms.Label();
            this.SessionSettingsMainTable.SuspendLayout();
            this.MainPanel.SuspendLayout();
            this.SettingsTable.SuspendLayout();
            this.SessionLabelPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // SessionSettingsMainTable
            // 
            this.SessionSettingsMainTable.ColumnCount = 1;
            this.SessionSettingsMainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SessionSettingsMainTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.SessionSettingsMainTable.Controls.Add(this.MainPanel, 0, 1);
            this.SessionSettingsMainTable.Controls.Add(this.SessionLabelPanel, 0, 0);
            this.SessionSettingsMainTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionSettingsMainTable.Location = new System.Drawing.Point(0, 0);
            this.SessionSettingsMainTable.Name = "SessionSettingsMainTable";
            this.SessionSettingsMainTable.RowCount = 2;
            this.SessionSettingsMainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.44444F));
            this.SessionSettingsMainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.55556F));
            this.SessionSettingsMainTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.SessionSettingsMainTable.Size = new System.Drawing.Size(800, 481);
            this.SessionSettingsMainTable.TabIndex = 0;
            // 
            // MainPanel
            // 
            this.MainPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.MainPanel.Controls.Add(this.SettingsTable);
            this.MainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainPanel.Location = new System.Drawing.Point(3, 62);
            this.MainPanel.Name = "MainPanel";
            this.MainPanel.Size = new System.Drawing.Size(794, 416);
            this.MainPanel.TabIndex = 0;
            // 
            // SettingsTable
            // 
            this.SettingsTable.ColumnCount = 2;
            this.SettingsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.18892F));
            this.SettingsTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 74.81108F));
            this.SettingsTable.Controls.Add(this.EncryptionCombo, 1, 2);
            this.SettingsTable.Controls.Add(this.SessionIDLabel, 0, 0);
            this.SettingsTable.Controls.Add(this.Port_Label, 0, 1);
            this.SettingsTable.Controls.Add(this.EncryptionSettingLabel, 0, 2);
            this.SettingsTable.Controls.Add(this.SessionID_Input, 1, 0);
            this.SettingsTable.Controls.Add(this.PortsCombo, 1, 1);
            this.SettingsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SettingsTable.Location = new System.Drawing.Point(0, 0);
            this.SettingsTable.Name = "SettingsTable";
            this.SettingsTable.RowCount = 3;
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SettingsTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.SettingsTable.Size = new System.Drawing.Size(794, 416);
            this.SettingsTable.TabIndex = 0;
            // 
            // EncryptionCombo
            // 
            this.EncryptionCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this.EncryptionCombo.FormattingEnabled = true;
            this.EncryptionCombo.Location = new System.Drawing.Point(203, 279);
            this.EncryptionCombo.Name = "EncryptionCombo";
            this.EncryptionCombo.Size = new System.Drawing.Size(588, 21);
            this.EncryptionCombo.TabIndex = 6;
            // 
            // SessionIDLabel
            // 
            this.SessionIDLabel.AutoSize = true;
            this.SessionIDLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.SessionIDLabel.Location = new System.Drawing.Point(3, 0);
            this.SessionIDLabel.Name = "SessionIDLabel";
            this.SessionIDLabel.Size = new System.Drawing.Size(194, 13);
            this.SessionIDLabel.TabIndex = 0;
            this.SessionIDLabel.Text = "SessionID";
            // 
            // Port_Label
            // 
            this.Port_Label.AutoSize = true;
            this.Port_Label.Dock = System.Windows.Forms.DockStyle.Top;
            this.Port_Label.Location = new System.Drawing.Point(3, 138);
            this.Port_Label.Name = "Port_Label";
            this.Port_Label.Size = new System.Drawing.Size(194, 13);
            this.Port_Label.TabIndex = 2;
            this.Port_Label.Text = "Port";
            // 
            // EncryptionSettingLabel
            // 
            this.EncryptionSettingLabel.AutoSize = true;
            this.EncryptionSettingLabel.Dock = System.Windows.Forms.DockStyle.Top;
            this.EncryptionSettingLabel.Location = new System.Drawing.Point(3, 276);
            this.EncryptionSettingLabel.Name = "EncryptionSettingLabel";
            this.EncryptionSettingLabel.Size = new System.Drawing.Size(194, 13);
            this.EncryptionSettingLabel.TabIndex = 1;
            this.EncryptionSettingLabel.Text = "Encryption Method";
            // 
            // SessionID_Input
            // 
            this.SessionID_Input.Dock = System.Windows.Forms.DockStyle.Top;
            this.SessionID_Input.Location = new System.Drawing.Point(203, 3);
            this.SessionID_Input.Name = "SessionID_Input";
            this.SessionID_Input.Size = new System.Drawing.Size(588, 20);
            this.SessionID_Input.TabIndex = 4;
            // 
            // PortsCombo
            // 
            this.PortsCombo.Dock = System.Windows.Forms.DockStyle.Top;
            this.PortsCombo.FormattingEnabled = true;
            this.PortsCombo.Location = new System.Drawing.Point(203, 141);
            this.PortsCombo.Name = "PortsCombo";
            this.PortsCombo.Size = new System.Drawing.Size(588, 21);
            this.PortsCombo.TabIndex = 5;
            // 
            // SessionLabelPanel
            // 
            this.SessionLabelPanel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SessionLabelPanel.Controls.Add(this.Create_Session);
            this.SessionLabelPanel.Controls.Add(this.SessionSettingsLabel);
            this.SessionLabelPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SessionLabelPanel.Location = new System.Drawing.Point(3, 3);
            this.SessionLabelPanel.Name = "SessionLabelPanel";
            this.SessionLabelPanel.Size = new System.Drawing.Size(794, 53);
            this.SessionLabelPanel.TabIndex = 2;
            // 
            // Create_Session
            // 
            this.Create_Session.Dock = System.Windows.Forms.DockStyle.Right;
            this.Create_Session.Location = new System.Drawing.Point(719, 0);
            this.Create_Session.Name = "Create_Session";
            this.Create_Session.Size = new System.Drawing.Size(75, 53);
            this.Create_Session.TabIndex = 2;
            this.Create_Session.Text = "Apply And Create";
            this.Create_Session.UseVisualStyleBackColor = true;
            this.Create_Session.Click += new System.EventHandler(this.Create_Session_Click);
            // 
            // SessionSettingsLabel
            // 
            this.SessionSettingsLabel.AutoSize = true;
            this.SessionSettingsLabel.BackColor = System.Drawing.SystemColors.ControlDark;
            this.SessionSettingsLabel.Dock = System.Windows.Forms.DockStyle.Left;
            this.SessionSettingsLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SessionSettingsLabel.Location = new System.Drawing.Point(0, 0);
            this.SessionSettingsLabel.Name = "SessionSettingsLabel";
            this.SessionSettingsLabel.Size = new System.Drawing.Size(327, 37);
            this.SessionSettingsLabel.TabIndex = 1;
            this.SessionSettingsLabel.Text = "New Session Settings";
            // 
            // Session_Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(800, 481);
            this.Controls.Add(this.SessionSettingsMainTable);
            this.Name = "Session_Settings";
            this.Text = "Aegis";
            this.SessionSettingsMainTable.ResumeLayout(false);
            this.MainPanel.ResumeLayout(false);
            this.SettingsTable.ResumeLayout(false);
            this.SettingsTable.PerformLayout();
            this.SessionLabelPanel.ResumeLayout(false);
            this.SessionLabelPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel SessionSettingsMainTable;
        private System.Windows.Forms.Panel MainPanel;
        private System.Windows.Forms.Label SessionSettingsLabel;
        private System.Windows.Forms.TableLayoutPanel SettingsTable;
        private System.Windows.Forms.Label SessionIDLabel;
        private System.Windows.Forms.Label Port_Label;
        private System.Windows.Forms.Label EncryptionSettingLabel;
        private System.Windows.Forms.TextBox SessionID_Input;
        private System.Windows.Forms.ComboBox EncryptionCombo;
        private System.Windows.Forms.ComboBox PortsCombo;
        private System.Windows.Forms.Panel SessionLabelPanel;
        private System.Windows.Forms.Button Create_Session;
    }
}