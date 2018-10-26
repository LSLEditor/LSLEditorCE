namespace LSLEditor.Tools
{
	partial class RuntimeExternal
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.ProxyPassword = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ProxyUserid = new System.Windows.Forms.TextBox();
			this.ProxyServer = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.label6);
			this.groupBox3.Controls.Add(this.ProxyPassword);
			this.groupBox3.Controls.Add(this.label5);
			this.groupBox3.Controls.Add(this.ProxyUserid);
			this.groupBox3.Controls.Add(this.ProxyServer);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Location = new System.Drawing.Point(0, 0);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(392, 96);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Proxy";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(8, 64);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(53, 13);
			this.label6.TabIndex = 7;
			this.label6.Text = "Password";
			// 
			// ProxyPassword
			// 
			this.ProxyPassword.Location = new System.Drawing.Point(88, 64);
			this.ProxyPassword.Name = "ProxyPassword";
			this.ProxyPassword.PasswordChar = '*';
			this.ProxyPassword.Size = new System.Drawing.Size(152, 20);
			this.ProxyPassword.TabIndex = 6;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(8, 40);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(37, 13);
			this.label5.TabIndex = 5;
			this.label5.Text = "Userid";
			// 
			// ProxyUserid
			// 
			this.ProxyUserid.Location = new System.Drawing.Point(88, 40);
			this.ProxyUserid.Name = "ProxyUserid";
			this.ProxyUserid.Size = new System.Drawing.Size(152, 20);
			this.ProxyUserid.TabIndex = 3;
			// 
			// ProxyServer
			// 
			this.ProxyServer.Location = new System.Drawing.Point(88, 16);
			this.ProxyServer.Name = "ProxyServer";
			this.ProxyServer.Size = new System.Drawing.Size(208, 20);
			this.ProxyServer.TabIndex = 2;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(8, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(38, 13);
			this.label3.TabIndex = 0;
			this.label3.Text = "Server";
			// 
			// RuntimeInternal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox3);
			this.Name = "RuntimeInternal";
			this.Size = new System.Drawing.Size(392, 272);
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox ProxyPassword;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox ProxyUserid;
		private System.Windows.Forms.TextBox ProxyServer;
		private System.Windows.Forms.Label label3;

	}
}
