using System.Drawing;
using System.Windows.Forms;

namespace Aegis
{
    partial class Message_Window
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
        public void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Message_Window));
            this.tableLayout_Messager = new System.Windows.Forms.TableLayoutPanel();
            this.Message_Panel = new System.Windows.Forms.Panel();
            this.Session_Panel = new System.Windows.Forms.Panel();
            this.Session_Settings = new System.Windows.Forms.Button();
            this.MessageTable = new System.Windows.Forms.TableLayoutPanel();
            this.Input_Panel = new System.Windows.Forms.Panel();
            this.InputTablePanel = new System.Windows.Forms.TableLayoutPanel();
            this.Input_Box = new System.Windows.Forms.TextBox();
            this.Emoji_Button = new System.Windows.Forms.Button();
            this.File_Upload_Button = new System.Windows.Forms.Button();
            this.Send_button = new System.Windows.Forms.Button();
            this.Session_Label = new System.Windows.Forms.Label();
            this.tableLayout_Messager.SuspendLayout();
            this.Message_Panel.SuspendLayout();
            this.Session_Panel.SuspendLayout();
            this.Input_Panel.SuspendLayout();
            this.InputTablePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayout_Messager
            // 
            this.tableLayout_Messager.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayout_Messager.AutoSize = true;
            this.tableLayout_Messager.ColumnCount = 1;
            this.tableLayout_Messager.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_Messager.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayout_Messager.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayout_Messager.Controls.Add(this.Session_Panel, 0, 0);
            this.tableLayout_Messager.Controls.Add(this.Message_Panel, 0, 1);
            this.tableLayout_Messager.Controls.Add(this.Input_Panel, 0, 2);
            this.tableLayout_Messager.Location = new System.Drawing.Point(0, 0);
            this.tableLayout_Messager.Name = "tableLayout_Messager";
            this.tableLayout_Messager.RowCount = 3;
            this.tableLayout_Messager.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 12.73101F));
            this.tableLayout_Messager.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 87.269F));
            this.tableLayout_Messager.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayout_Messager.Size = new System.Drawing.Size(999, 562);
            this.tableLayout_Messager.TabIndex = 0;
            // 
            // Message_Panel
            // 
            this.Message_Panel.AutoScroll = true;
            this.Message_Panel.Controls.Add(this.MessageTable);
            this.Message_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Message_Panel.Location = new System.Drawing.Point(3, 65);
            this.Message_Panel.Name = "Message_Panel";
            this.Message_Panel.Size = new System.Drawing.Size(993, 419);
            this.Message_Panel.TabIndex = 1;
            // 
            // Session_Panel
            // 
            this.Session_Panel.Controls.Add(this.Session_Label);
            this.Session_Panel.Controls.Add(this.Session_Settings);
            this.Session_Panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Session_Panel.Location = new System.Drawing.Point(3, 3);
            this.Session_Panel.Name = "Session_Panel";
            this.Session_Panel.Size = new System.Drawing.Size(993, 56);
            this.Session_Panel.TabIndex = 4;
            // 
            // Session_Settings
            // 
            this.Session_Settings.BackgroundImage = global::Aegis.Properties.Resources._8666681_edit_icon;
            this.Session_Settings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Session_Settings.Dock = System.Windows.Forms.DockStyle.Right;
            this.Session_Settings.Location = new System.Drawing.Point(950, 0);
            this.Session_Settings.Name = "Session_Settings";
            this.Session_Settings.Size = new System.Drawing.Size(43, 56);
            this.Session_Settings.TabIndex = 3;
            this.Session_Settings.UseVisualStyleBackColor = true;
            // 
            // MessageTable
            // 
            this.MessageTable.AutoSize = true;
            this.MessageTable.ColumnCount = 2;
            this.MessageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MessageTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MessageTable.Dock = System.Windows.Forms.DockStyle.Top;
            this.MessageTable.Location = new System.Drawing.Point(0, 0);
            this.MessageTable.Name = "MessageTable";
            this.MessageTable.RowCount = 1;
            this.MessageTable.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MessageTable.Size = new System.Drawing.Size(993, 0);
            this.MessageTable.TabIndex = 0;
            // 
            // Input_Panel
            // 
            this.Input_Panel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Input_Panel.Controls.Add(this.InputTablePanel);
            this.Input_Panel.Location = new System.Drawing.Point(3, 490);
            this.Input_Panel.Name = "Input_Panel";
            this.Input_Panel.Size = new System.Drawing.Size(993, 69);
            this.Input_Panel.TabIndex = 2;
            // 
            // InputTablePanel
            // 
            this.InputTablePanel.AutoSize = true;
            this.InputTablePanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.InputTablePanel.ColumnCount = 4;
            this.InputTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 85.01028F));
            this.InputTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.996575F));
            this.InputTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.996575F));
            this.InputTablePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.996575F));
            this.InputTablePanel.Controls.Add(this.Input_Box, 0, 0);
            this.InputTablePanel.Controls.Add(this.Emoji_Button, 1, 0);
            this.InputTablePanel.Controls.Add(this.File_Upload_Button, 2, 0);
            this.InputTablePanel.Controls.Add(this.Send_button, 3, 0);
            this.InputTablePanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputTablePanel.Location = new System.Drawing.Point(0, 0);
            this.InputTablePanel.Name = "InputTablePanel";
            this.InputTablePanel.RowCount = 1;
            this.InputTablePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.InputTablePanel.Size = new System.Drawing.Size(993, 69);
            this.InputTablePanel.TabIndex = 0;
            // 
            // Input_Box
            // 
            this.Input_Box.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Input_Box.Location = new System.Drawing.Point(4, 4);
            this.Input_Box.Multiline = true;
            this.Input_Box.Name = "Input_Box";
            this.Input_Box.Size = new System.Drawing.Size(833, 61);
            this.Input_Box.TabIndex = 0;
            // 
            // Emoji_Button
            // 
            this.Emoji_Button.BackgroundImage = global::Aegis.Properties.Resources._8666647_heart_icon;
            this.Emoji_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Emoji_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Emoji_Button.Location = new System.Drawing.Point(844, 4);
            this.Emoji_Button.Name = "Emoji_Button";
            this.Emoji_Button.Size = new System.Drawing.Size(43, 61);
            this.Emoji_Button.TabIndex = 1;
            this.Emoji_Button.UseVisualStyleBackColor = true;
            this.Emoji_Button.Click += new System.EventHandler(this.Emoji_Button_Click);
            // 
            // File_Upload_Button
            // 
            this.File_Upload_Button.BackgroundImage = global::Aegis.Properties.Resources._8666803_folder_documents_icon;
            this.File_Upload_Button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.File_Upload_Button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.File_Upload_Button.Location = new System.Drawing.Point(894, 4);
            this.File_Upload_Button.Name = "File_Upload_Button";
            this.File_Upload_Button.Size = new System.Drawing.Size(43, 61);
            this.File_Upload_Button.TabIndex = 2;
            this.File_Upload_Button.UseVisualStyleBackColor = true;
            this.File_Upload_Button.Click += new System.EventHandler(this.File_Upload_Button_Click);
            // 
            // Send_button
            // 
            this.Send_button.BackgroundImage = global::Aegis.Properties.Resources._8666722_log_in_icon;
            this.Send_button.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.Send_button.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Send_button.Location = new System.Drawing.Point(944, 4);
            this.Send_button.Name = "Send_button";
            this.Send_button.Size = new System.Drawing.Size(45, 61);
            this.Send_button.TabIndex = 3;
            this.Send_button.UseVisualStyleBackColor = true;
            this.Send_button.Click += new System.EventHandler(this.Send_button_Click);
            // 
            // Session_Label
            // 
            this.Session_Label.AutoSize = true;
            this.Session_Label.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Session_Label.Dock = System.Windows.Forms.DockStyle.Left;
            this.Session_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Session_Label.Location = new System.Drawing.Point(0, 0);
            this.Session_Label.Name = "Session_Label";
            this.Session_Label.Size = new System.Drawing.Size(0, 42);
            this.Session_Label.TabIndex = 4;
            // 
            // Message_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.ClientSize = new System.Drawing.Size(1001, 563);
            this.Controls.Add(this.tableLayout_Messager);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Message_Window";
            this.Text = "Message_Window";
            this.tableLayout_Messager.ResumeLayout(false);
            this.Message_Panel.ResumeLayout(false);
            this.Message_Panel.PerformLayout();
            this.Session_Panel.ResumeLayout(false);
            this.Session_Panel.PerformLayout();
            this.Input_Panel.ResumeLayout(false);
            this.Input_Panel.PerformLayout();
            this.InputTablePanel.ResumeLayout(false);
            this.InputTablePanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayout_Messager;
        public System.Windows.Forms.Panel Message_Panel;
        public System.Windows.Forms.Panel Input_Panel;
        public System.Windows.Forms.TextBox Input_Box;
        private System.Windows.Forms.TableLayoutPanel InputTablePanel;
        public System.Windows.Forms.TableLayoutPanel MessageTable;
        private Button Emoji_Button;
        private Button File_Upload_Button;
        private Button Send_button;
        private Button Session_Settings;
        private Panel Session_Panel;
        public Label Session_Label;
    }
}