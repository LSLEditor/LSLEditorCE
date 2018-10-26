namespace LSLEditor
{
	partial class GListBoxWindow
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
			this.gListBox1 = new LSLEditor.GListBox(this.components);
			this.SuspendLayout();
			// 
			// gListBox1
			// 
			this.gListBox1.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.gListBox1.FormattingEnabled = true;
			this.gListBox1.Location = new System.Drawing.Point(0, 0);
			this.gListBox1.Name = "gListBox1";
			this.gListBox1.Size = new System.Drawing.Size(192, 134);
			this.gListBox1.TabIndex = 0;
			this.gListBox1.Resize += new System.EventHandler(this.gListBox1_Resize);
			// 
			// GListBoxWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(212, 156);
			this.Controls.Add(this.gListBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Name = "GListBoxWindow";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
			this.Text = "GListBoxWindow";
			this.ResumeLayout(false);

		}

		#endregion

		private GListBox gListBox1;
	}
}