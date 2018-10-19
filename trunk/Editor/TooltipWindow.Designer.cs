namespace LSLEditor
{
	partial class TooltipWindow
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
			this.richLabel1 = new LSLEditor.RichLabel();
			this.SuspendLayout();
			// 
			// richLabel1
			// 
			this.richLabel1.BackColor = System.Drawing.Color.LightGoldenrodYellow;
			this.richLabel1.Location = new System.Drawing.Point(0, 0);
			this.richLabel1.Name = "richLabel1";
			this.richLabel1.Size = new System.Drawing.Size(52, 20);
			this.richLabel1.TabIndex = 0;
			this.richLabel1.Resize += new System.EventHandler(this.richLabel1_Resize);
			// 
			// TooltipWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(105, 42);
			this.Controls.Add(this.richLabel1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "TooltipWindow";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "GListBoxWindow";
			this.ResumeLayout(false);

		}

		#endregion

		private RichLabel richLabel1;
	}
}