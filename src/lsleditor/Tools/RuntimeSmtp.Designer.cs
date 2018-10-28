namespace LSLEditor.Tools
{
	partial class RuntimeSmtp
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
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.radioButton6 = new System.Windows.Forms.RadioButton();
			this.radioButton5 = new System.Windows.Forms.RadioButton();
			this.radioButton4 = new System.Windows.Forms.RadioButton();
			this.label12 = new System.Windows.Forms.Label();
			this.radioButton3 = new System.Windows.Forms.RadioButton();
			this.radioButton2 = new System.Windows.Forms.RadioButton();
			this.radioButton1 = new System.Windows.Forms.RadioButton();
			this.label11 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.SmtpPassword = new System.Windows.Forms.TextBox();
			this.EmailAddress = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.SmtpUserid = new System.Windows.Forms.TextBox();
			this.EmailServer = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox2.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.label4);
			this.groupBox2.Controls.Add(this.EmailAddress);
			this.groupBox2.Controls.Add(this.EmailServer);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(392, 80);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Email";
			// 
			// radioButton6
			// 
			this.radioButton6.AutoSize = true;
			this.radioButton6.Enabled = false;
			this.radioButton6.Location = new System.Drawing.Point(256, 96);
			this.radioButton6.Name = "radioButton6";
			this.radioButton6.Size = new System.Drawing.Size(80, 17);
			this.radioButton6.TabIndex = 19;
			this.radioButton6.Text = "Anonymous";
			this.radioButton6.UseVisualStyleBackColor = true;
			// 
			// radioButton5
			// 
			this.radioButton5.AutoSize = true;
			this.radioButton5.Enabled = false;
			this.radioButton5.Location = new System.Drawing.Point(192, 96);
			this.radioButton5.Name = "radioButton5";
			this.radioButton5.Size = new System.Drawing.Size(63, 17);
			this.radioButton5.TabIndex = 18;
			this.radioButton5.Text = "External";
			this.radioButton5.UseVisualStyleBackColor = true;
			// 
			// radioButton4
			// 
			this.radioButton4.AutoSize = true;
			this.radioButton4.Enabled = false;
			this.radioButton4.Location = new System.Drawing.Point(192, 120);
			this.radioButton4.Name = "radioButton4";
			this.radioButton4.Size = new System.Drawing.Size(91, 17);
			this.radioButton4.TabIndex = 17;
			this.radioButton4.Text = "DIGEST-MD5";
			this.radioButton4.UseVisualStyleBackColor = true;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(24, 96);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(52, 13);
			this.label12.TabIndex = 16;
			this.label12.Text = "Auth type";
			// 
			// radioButton3
			// 
			this.radioButton3.AutoSize = true;
			this.radioButton3.Enabled = false;
			this.radioButton3.Location = new System.Drawing.Point(96, 120);
			this.radioButton3.Name = "radioButton3";
			this.radioButton3.Size = new System.Drawing.Size(82, 17);
			this.radioButton3.TabIndex = 15;
			this.radioButton3.Text = "CRAM-MD5";
			this.radioButton3.UseVisualStyleBackColor = true;
			// 
			// radioButton2
			// 
			this.radioButton2.AutoSize = true;
			this.radioButton2.Location = new System.Drawing.Point(144, 96);
			this.radioButton2.Name = "radioButton2";
			this.radioButton2.Size = new System.Drawing.Size(51, 17);
			this.radioButton2.TabIndex = 14;
			this.radioButton2.Text = "Login";
			this.radioButton2.UseVisualStyleBackColor = true;
			// 
			// radioButton1
			// 
			this.radioButton1.AutoSize = true;
			this.radioButton1.Checked = true;
			this.radioButton1.Location = new System.Drawing.Point(96, 96);
			this.radioButton1.Name = "radioButton1";
			this.radioButton1.Size = new System.Drawing.Size(48, 17);
			this.radioButton1.TabIndex = 13;
			this.radioButton1.TabStop = true;
			this.radioButton1.Text = "Plain";
			this.radioButton1.UseVisualStyleBackColor = true;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(24, 24);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(158, 13);
			this.label11.TabIndex = 12;
			this.label11.Text = "leave blank for normal operation";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(24, 72);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(53, 13);
			this.label9.TabIndex = 11;
			this.label9.Text = "Password";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(8, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(72, 13);
			this.label4.TabIndex = 4;
			this.label4.Text = "Email address";
			// 
			// SmtpPassword
			// 
			this.SmtpPassword.Location = new System.Drawing.Point(104, 72);
			this.SmtpPassword.Name = "SmtpPassword";
			this.SmtpPassword.PasswordChar = '*';
			this.SmtpPassword.Size = new System.Drawing.Size(152, 20);
			this.SmtpPassword.TabIndex = 10;
			// 
			// EmailAddress
			// 
			this.EmailAddress.Location = new System.Drawing.Point(88, 40);
			this.EmailAddress.Name = "EmailAddress";
			this.EmailAddress.Size = new System.Drawing.Size(208, 20);
			this.EmailAddress.TabIndex = 3;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(24, 48);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(37, 13);
			this.label10.TabIndex = 9;
			this.label10.Text = "Userid";
			// 
			// SmtpUserid
			// 
			this.SmtpUserid.Location = new System.Drawing.Point(104, 48);
			this.SmtpUserid.Name = "SmtpUserid";
			this.SmtpUserid.Size = new System.Drawing.Size(152, 20);
			this.SmtpUserid.TabIndex = 8;
			// 
			// EmailServer
			// 
			this.EmailServer.Location = new System.Drawing.Point(88, 16);
			this.EmailServer.Name = "EmailServer";
			this.EmailServer.Size = new System.Drawing.Size(208, 20);
			this.EmailServer.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(8, 16);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(69, 13);
			this.label2.TabIndex = 0;
			this.label2.Text = "SMTP server";
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.radioButton6);
			this.groupBox1.Controls.Add(this.radioButton1);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.radioButton5);
			this.groupBox1.Controls.Add(this.radioButton2);
			this.groupBox1.Controls.Add(this.SmtpPassword);
			this.groupBox1.Controls.Add(this.radioButton4);
			this.groupBox1.Controls.Add(this.radioButton3);
			this.groupBox1.Controls.Add(this.SmtpUserid);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Location = new System.Drawing.Point(0, 88);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(392, 152);
			this.groupBox1.TabIndex = 20;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "SMTP Authentication";
			// 
			// RuntimeInternal
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.groupBox2);
			this.Name = "RuntimeInternal";
			this.Size = new System.Drawing.Size(392, 272);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox EmailAddress;
		private System.Windows.Forms.TextBox EmailServer;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.RadioButton radioButton6;
		private System.Windows.Forms.RadioButton radioButton5;
		private System.Windows.Forms.RadioButton radioButton4;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.RadioButton radioButton3;
		private System.Windows.Forms.RadioButton radioButton2;
		private System.Windows.Forms.RadioButton radioButton1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox SmtpPassword;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox SmtpUserid;
		private System.Windows.Forms.GroupBox groupBox1;

	}
}
