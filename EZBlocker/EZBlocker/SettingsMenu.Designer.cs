
namespace EZBlocker
{
    partial class SettingsMenu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsMenu));
            this.SpotifyFileLocationTitle = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.FileSelectorButton = new System.Windows.Forms.Button();
            this.fileLocation = new System.Windows.Forms.TextBox();
            this.TestSpotifyExeButton = new System.Windows.Forms.Button();
            this.DefaultPathButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SpotifyFileLocationTitle
            // 
            this.SpotifyFileLocationTitle.AutoSize = true;
            this.SpotifyFileLocationTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpotifyFileLocationTitle.Location = new System.Drawing.Point(9, 13);
            this.SpotifyFileLocationTitle.Name = "SpotifyFileLocationTitle";
            this.SpotifyFileLocationTitle.Size = new System.Drawing.Size(119, 15);
            this.SpotifyFileLocationTitle.TabIndex = 0;
            this.SpotifyFileLocationTitle.Text = "Spotify.exe Location:";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FileSelectorButton
            // 
            this.FileSelectorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FileSelectorButton.Location = new System.Drawing.Point(11, 31);
            this.FileSelectorButton.Name = "FileSelectorButton";
            this.FileSelectorButton.Size = new System.Drawing.Size(65, 23);
            this.FileSelectorButton.TabIndex = 1;
            this.FileSelectorButton.Text = global::EZBlocker.Properties.strings.FileSelectorText;
            this.FileSelectorButton.UseVisualStyleBackColor = true;
            this.FileSelectorButton.Click += new System.EventHandler(this.FileSelectorButton_Click);
            // 
            // fileLocation
            // 
            this.fileLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fileLocation.Location = new System.Drawing.Point(78, 32);
            this.fileLocation.Name = "fileLocation";
            this.fileLocation.Size = new System.Drawing.Size(211, 21);
            this.fileLocation.TabIndex = 2;
            this.fileLocation.Text = global::EZBlocker.Properties.Settings.Default.SpotifyPath;
            // 
            // TestSpotifyExeButton
            // 
            this.TestSpotifyExeButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TestSpotifyExeButton.Location = new System.Drawing.Point(11, 59);
            this.TestSpotifyExeButton.Name = "TestSpotifyExeButton";
            this.TestSpotifyExeButton.Size = new System.Drawing.Size(128, 23);
            this.TestSpotifyExeButton.TabIndex = 3;
            this.TestSpotifyExeButton.Text = global::EZBlocker.Properties.strings.TestSpotifyExeButtonText;
            this.TestSpotifyExeButton.UseVisualStyleBackColor = true;
            this.TestSpotifyExeButton.Click += new System.EventHandler(this.TestSpotifyExeButton_Click);
            // 
            // DefaultPathButton
            // 
            this.DefaultPathButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DefaultPathButton.Location = new System.Drawing.Point(145, 59);
            this.DefaultPathButton.Name = "DefaultPathButton";
            this.DefaultPathButton.Size = new System.Drawing.Size(144, 23);
            this.DefaultPathButton.TabIndex = 4;
            this.DefaultPathButton.Text = global::EZBlocker.Properties.strings.DefaultPathButtonText;
            this.DefaultPathButton.UseVisualStyleBackColor = true;
            this.DefaultPathButton.Click += new System.EventHandler(this.DefaultPathButton_Click);
            // 
            // SettingsMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(305, 97);
            this.Controls.Add(this.DefaultPathButton);
            this.Controls.Add(this.TestSpotifyExeButton);
            this.Controls.Add(this.fileLocation);
            this.Controls.Add(this.FileSelectorButton);
            this.Controls.Add(this.SpotifyFileLocationTitle);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "SettingsMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EZBlocker - Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SpotifyFileLocationTitle;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button FileSelectorButton;
        private System.Windows.Forms.TextBox fileLocation;
        private System.Windows.Forms.Button TestSpotifyExeButton;
        private System.Windows.Forms.Button DefaultPathButton;
    }
}