namespace LSLEditor
{
	partial class UpdateApplicationForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.labelHelpversionString = new System.Windows.Forms.Label();
			this.labelHelpFile = new System.Windows.Forms.Label();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.labelLatestVersionString = new System.Windows.Forms.Label();
			this.labelOurVersionString = new System.Windows.Forms.Label();
			this.labelLatestVersion = new System.Windows.Forms.Label();
			this.labelOurVersion = new System.Windows.Forms.Label();
			this.buttonUpdate = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.labelHelpversionString);
			this.groupBox1.Controls.Add(this.labelHelpFile);
			this.groupBox1.Controls.Add(this.progressBar1);
			this.groupBox1.Controls.Add(this.labelLatestVersionString);
			this.groupBox1.Controls.Add(this.labelOurVersionString);
			this.groupBox1.Controls.Add(this.labelLatestVersion);
			this.groupBox1.Controls.Add(this.labelOurVersion);
			this.groupBox1.Location = new System.Drawing.Point(8, 8);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(216, 128);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "version information";
			// 
			// labelHelpversionString
			// 
			this.labelHelpversionString.AutoSize = true;
			this.labelHelpversionString.Location = new System.Drawing.Point(104, 72);
			this.labelHelpversionString.Name = "labelHelpversionString";
			this.labelHelpversionString.Size = new System.Drawing.Size(0, 13);
			this.labelHelpversionString.TabIndex = 6;
			// 
			// labelHelpFile
			// 
			this.labelHelpFile.AutoSize = true;
			this.labelHelpFile.Location = new System.Drawing.Point(16, 72);
			this.labelHelpFile.Name = "labelHelpFile";
			this.labelHelpFile.Size = new System.Drawing.Size(48, 13);
			this.labelHelpFile.TabIndex = 5;
			this.labelHelpFile.Text = "Help file:";
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(16, 96);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(184, 15);
			this.progressBar1.TabIndex = 4;
			// 
			// labelLatestVersionString
			// 
			this.labelLatestVersionString.AutoSize = true;
			this.labelLatestVersionString.Location = new System.Drawing.Point(104, 48);
			this.labelLatestVersionString.Name = "labelLatestVersionString";
			this.labelLatestVersionString.Size = new System.Drawing.Size(0, 13);
			this.labelLatestVersionString.TabIndex = 3;
			// 
			// labelOurVersionString
			// 
			this.labelOurVersionString.AutoSize = true;
			this.labelOurVersionString.Location = new System.Drawing.Point(104, 24);
			this.labelOurVersionString.Name = "labelOurVersionString";
			this.labelOurVersionString.Size = new System.Drawing.Size(0, 13);
			this.labelOurVersionString.TabIndex = 2;
			// 
			// labelLatestVersion
			// 
			this.labelLatestVersion.AutoSize = true;
			this.labelLatestVersion.Location = new System.Drawing.Point(16, 48);
			this.labelLatestVersion.Name = "labelLatestVersion";
			this.labelLatestVersion.Size = new System.Drawing.Size(76, 13);
			this.labelLatestVersion.TabIndex = 1;
			this.labelLatestVersion.Text = "Latest version:";
			// 
			// labelOurVersion
			// 
			this.labelOurVersion.AutoSize = true;
			this.labelOurVersion.Location = new System.Drawing.Point(16, 24);
			this.labelOurVersion.Name = "labelOurVersion";
			this.labelOurVersion.Size = new System.Drawing.Size(69, 13);
			this.labelOurVersion.TabIndex = 0;
			this.labelOurVersion.Text = "Your version:";
			// 
			// buttonUpdate
			// 
			this.buttonUpdate.Location = new System.Drawing.Point(48, 144);
			this.buttonUpdate.Name = "buttonUpdate";
			this.buttonUpdate.Size = new System.Drawing.Size(75, 23);
			this.buttonUpdate.TabIndex = 1;
			this.buttonUpdate.Text = "Update";
			this.buttonUpdate.UseVisualStyleBackColor = true;
			this.buttonUpdate.Click += new System.EventHandler(this.buttonUpdate_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(136, 144);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(75, 23);
			this.buttonCancel.TabIndex = 2;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// UpdateApplicationForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(234, 177);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonUpdate);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "UpdateApplicationForm";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Update LSLEditor";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.UpdateApplicationForm_FormClosing);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label labelLatestVersionString;
		private System.Windows.Forms.Label labelOurVersionString;
		private System.Windows.Forms.Label labelLatestVersion;
		private System.Windows.Forms.Label labelOurVersion;
		private System.Windows.Forms.Button buttonUpdate;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Label labelHelpversionString;
		private System.Windows.Forms.Label labelHelpFile;
	}
}