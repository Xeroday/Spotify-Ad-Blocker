namespace EZBlocker
{
    partial class Main
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.BlockButton = new System.Windows.Forms.Button();
            this.OpenButton = new System.Windows.Forms.Button();
            this.AutoAddCheck = new System.Windows.Forms.CheckBox();
            this.MainTimer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // BlockButton
            // 
            this.BlockButton.Location = new System.Drawing.Point(12, 36);
            this.BlockButton.Name = "BlockButton";
            this.BlockButton.Size = new System.Drawing.Size(140, 36);
            this.BlockButton.TabIndex = 0;
            this.BlockButton.Text = "Add Current to Blocklist";
            this.BlockButton.UseVisualStyleBackColor = true;
            this.BlockButton.Click += new System.EventHandler(this.BlockButton_Click);
            // 
            // OpenButton
            // 
            this.OpenButton.Location = new System.Drawing.Point(158, 36);
            this.OpenButton.Name = "OpenButton";
            this.OpenButton.Size = new System.Drawing.Size(138, 36);
            this.OpenButton.TabIndex = 1;
            this.OpenButton.Text = "Open Blocklist";
            this.OpenButton.UseVisualStyleBackColor = true;
            // 
            // AutoAddCheck
            // 
            this.AutoAddCheck.AutoSize = true;
            this.AutoAddCheck.Checked = true;
            this.AutoAddCheck.CheckState = System.Windows.Forms.CheckState.Checked;
            this.AutoAddCheck.Location = new System.Drawing.Point(12, 87);
            this.AutoAddCheck.Name = "AutoAddCheck";
            this.AutoAddCheck.Size = new System.Drawing.Size(211, 17);
            this.AutoAddCheck.TabIndex = 2;
            this.AutoAddCheck.Text = "AutoAdd Ads to Blocklist (Experimental)";
            this.AutoAddCheck.UseVisualStyleBackColor = true;
            this.AutoAddCheck.CheckedChanged += new System.EventHandler(this.AutoAddCheck_CheckedChanged);
            // 
            // MainTimer
            // 
            this.MainTimer.Enabled = true;
            this.MainTimer.Interval = 1000;
            this.MainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 373);
            this.Controls.Add(this.AutoAddCheck);
            this.Controls.Add(this.OpenButton);
            this.Controls.Add(this.BlockButton);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Main";
            this.RightToLeftLayout = true;
            this.Text = "EZBlocker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BlockButton;
        private System.Windows.Forms.Button OpenButton;
        private System.Windows.Forms.CheckBox AutoAddCheck;
        private System.Windows.Forms.Timer MainTimer;
    }
}

