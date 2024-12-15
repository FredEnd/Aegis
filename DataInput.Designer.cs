namespace Aegis
{
    partial class DataInput
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
            this.DataInputTable1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.Cancel = new System.Windows.Forms.Button();
            this.EnterButton = new System.Windows.Forms.Button();
            this.EnterBox = new System.Windows.Forms.TextBox();
            this.DataInputTable1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // DataInputTable1
            // 
            this.DataInputTable1.ColumnCount = 1;
            this.DataInputTable1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DataInputTable1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.DataInputTable1.Controls.Add(this.label1, 0, 0);
            this.DataInputTable1.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.DataInputTable1.Controls.Add(this.EnterBox, 0, 1);
            this.DataInputTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DataInputTable1.Location = new System.Drawing.Point(0, 0);
            this.DataInputTable1.Name = "DataInputTable1";
            this.DataInputTable1.RowCount = 3;
            this.DataInputTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.77778F));
            this.DataInputTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 84.22222F));
            this.DataInputTable1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 105F));
            this.DataInputTable1.Size = new System.Drawing.Size(800, 450);
            this.DataInputTable1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(794, 54);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter your code for other session";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.Cancel, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.EnterButton, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 347);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(794, 100);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // Cancel
            // 
            this.Cancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Cancel.Location = new System.Drawing.Point(400, 3);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(391, 94);
            this.Cancel.TabIndex = 2;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // EnterButton
            // 
            this.EnterButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnterButton.Location = new System.Drawing.Point(3, 3);
            this.EnterButton.Name = "EnterButton";
            this.EnterButton.Size = new System.Drawing.Size(391, 94);
            this.EnterButton.TabIndex = 1;
            this.EnterButton.Text = "Enter";
            this.EnterButton.UseVisualStyleBackColor = true;
            this.EnterButton.Click += new System.EventHandler(this.EnterButton_ClickAsync);
            // 
            // EnterBox
            // 
            this.EnterBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EnterBox.Location = new System.Drawing.Point(3, 57);
            this.EnterBox.Multiline = true;
            this.EnterBox.Name = "EnterBox";
            this.EnterBox.Size = new System.Drawing.Size(794, 284);
            this.EnterBox.TabIndex = 3;
            // 
            // DataInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.DataInputTable1);
            this.Name = "DataInput";
            this.Text = "DataInput";
            this.DataInputTable1.ResumeLayout(false);
            this.DataInputTable1.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel DataInputTable1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button EnterButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox EnterBox;
    }
}