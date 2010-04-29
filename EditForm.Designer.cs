namespace LSLEditor
{
	partial class EditForm
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.disableCompilesyntaxCheckToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.numberedTextBoxUC1 = new NumberedTextBox.NumberedTextBoxUC();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.ContextMenuStrip = this.contextMenuStrip1;
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(551, 323);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.numberedTextBoxUC1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(543, 297);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Script";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.disableCompilesyntaxCheckToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(225, 26);
			// 
			// disableCompilesyntaxCheckToolStripMenuItem
			// 
			this.disableCompilesyntaxCheckToolStripMenuItem.Name = "disableCompilesyntaxCheckToolStripMenuItem";
			this.disableCompilesyntaxCheckToolStripMenuItem.Size = new System.Drawing.Size(224, 22);
			this.disableCompilesyntaxCheckToolStripMenuItem.Text = "Disable compile/syntax check";
			this.disableCompilesyntaxCheckToolStripMenuItem.Click += new System.EventHandler(this.disableCompilesyntaxCheckToolStripMenuItem_Click);
			// 
			// numberedTextBoxUC1
			// 
			this.numberedTextBoxUC1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.numberedTextBoxUC1.Location = new System.Drawing.Point(3, 3);
			this.numberedTextBoxUC1.Name = "numberedTextBoxUC1";
			this.numberedTextBoxUC1.Size = new System.Drawing.Size(537, 291);
			this.numberedTextBoxUC1.TabIndex = 4;
			// 
			// EditForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(551, 323);
			this.Controls.Add(this.tabControl1);
			this.Name = "EditForm";
			this.Text = "EditForm";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditForm_FormClosing);
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private NumberedTextBox.NumberedTextBoxUC numberedTextBoxUC1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem disableCompilesyntaxCheckToolStripMenuItem;


	}
}