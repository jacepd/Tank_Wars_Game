
namespace View
{
    partial class Form1
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
            this.ServerNameTextbox = new System.Windows.Forms.TextBox();
            this.ServerConnectButton = new System.Windows.Forms.Button();
            this.PlayerNameTextbox = new System.Windows.Forms.TextBox();
            this.ServerNameLabel = new System.Windows.Forms.Label();
            this.PlayerNameLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ServerNameTextbox
            // 
            this.ServerNameTextbox.Location = new System.Drawing.Point(85, 13);
            this.ServerNameTextbox.Name = "ServerNameTextbox";
            this.ServerNameTextbox.Size = new System.Drawing.Size(100, 22);
            this.ServerNameTextbox.TabIndex = 0;
            // 
            // ServerConnectButton
            // 
            this.ServerConnectButton.Location = new System.Drawing.Point(471, 13);
            this.ServerConnectButton.Name = "ServerConnectButton";
            this.ServerConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ServerConnectButton.TabIndex = 1;
            this.ServerConnectButton.Text = "Connect";
            this.ServerConnectButton.UseVisualStyleBackColor = true;
            this.ServerConnectButton.Click += new System.EventHandler(this.ServerConnectButton_Click);
            // 
            // PlayerNameTextbox
            // 
            this.PlayerNameTextbox.Location = new System.Drawing.Point(286, 14);
            this.PlayerNameTextbox.Name = "PlayerNameTextbox";
            this.PlayerNameTextbox.Size = new System.Drawing.Size(100, 22);
            this.PlayerNameTextbox.TabIndex = 2;
            // 
            // ServerNameLabel
            // 
            this.ServerNameLabel.AutoSize = true;
            this.ServerNameLabel.Location = new System.Drawing.Point(25, 13);
            this.ServerNameLabel.Name = "ServerNameLabel";
            this.ServerNameLabel.Size = new System.Drawing.Size(54, 17);
            this.ServerNameLabel.TabIndex = 3;
            this.ServerNameLabel.Text = "Server:";
            // 
            // PlayerNameLabel
            // 
            this.PlayerNameLabel.AutoSize = true;
            this.PlayerNameLabel.Location = new System.Drawing.Point(231, 16);
            this.PlayerNameLabel.Name = "PlayerNameLabel";
            this.PlayerNameLabel.Size = new System.Drawing.Size(49, 17);
            this.PlayerNameLabel.TabIndex = 4;
            this.PlayerNameLabel.Text = "Name:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.PlayerNameLabel);
            this.Controls.Add(this.ServerNameLabel);
            this.Controls.Add(this.PlayerNameTextbox);
            this.Controls.Add(this.ServerConnectButton);
            this.Controls.Add(this.ServerNameTextbox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ServerNameTextbox;
        private System.Windows.Forms.Button ServerConnectButton;
        private System.Windows.Forms.TextBox PlayerNameTextbox;
        private System.Windows.Forms.Label ServerNameLabel;
        private System.Windows.Forms.Label PlayerNameLabel;
    }
}

