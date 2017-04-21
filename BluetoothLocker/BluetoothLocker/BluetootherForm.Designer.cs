namespace BluetoothLocker
{
    partial class BluetootherForm
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
            this.discoverBtn = new System.Windows.Forms.Button();
            this.devicesDropDown = new System.Windows.Forms.ComboBox();
            this.startStopBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.exitBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // discoverBtn
            // 
            this.discoverBtn.Location = new System.Drawing.Point(12, 34);
            this.discoverBtn.Name = "discoverBtn";
            this.discoverBtn.Size = new System.Drawing.Size(75, 23);
            this.discoverBtn.TabIndex = 0;
            this.discoverBtn.Text = "Discover";
            this.discoverBtn.UseVisualStyleBackColor = true;
            this.discoverBtn.Click += new System.EventHandler(this.discover_Click);
            // 
            // devicesDropDown
            // 
            this.devicesDropDown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.devicesDropDown.FormattingEnabled = true;
            this.devicesDropDown.Location = new System.Drawing.Point(93, 35);
            this.devicesDropDown.Name = "devicesDropDown";
            this.devicesDropDown.Size = new System.Drawing.Size(224, 21);
            this.devicesDropDown.TabIndex = 2;
            this.devicesDropDown.SelectedIndexChanged += new System.EventHandler(this.devicesDropDown_SelectedIndexChanged);
            // 
            // startStopBtn
            // 
            this.startStopBtn.Enabled = false;
            this.startStopBtn.Location = new System.Drawing.Point(12, 83);
            this.startStopBtn.Name = "startStopBtn";
            this.startStopBtn.Size = new System.Drawing.Size(219, 23);
            this.startStopBtn.TabIndex = 3;
            this.startStopBtn.Text = "Start using selected device for security";
            this.startStopBtn.UseVisualStyleBackColor = true;
            this.startStopBtn.Click += new System.EventHandler(this.startStopBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(305, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Select a device in range which will be used to lock the program";
            // 
            // exitBtn
            // 
            this.exitBtn.Location = new System.Drawing.Point(242, 83);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 5;
            this.exitBtn.Text = "Exit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // BluetootherForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(329, 121);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.startStopBtn);
            this.Controls.Add(this.devicesDropDown);
            this.Controls.Add(this.discoverBtn);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "BluetootherForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bluetoother";
            this.Shown += new System.EventHandler(this.BluetootherForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button discoverBtn;
        private System.Windows.Forms.ComboBox devicesDropDown;
        private System.Windows.Forms.Button startStopBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button exitBtn;
    }
}

