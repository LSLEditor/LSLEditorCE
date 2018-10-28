namespace LSLEditor.BugReport
{
	partial class BugReportForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.panel2 = new System.Windows.Forms.Panel();
			this.button4 = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.listView1 = new System.Windows.Forms.ListView();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.panel1 = new System.Windows.Forms.Panel();
			this.button3 = new System.Windows.Forms.Button();
			this.progressBar1 = new System.Windows.Forms.ProgressBar();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.splitter2 = new System.Windows.Forms.Splitter();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.splitter1 = new System.Windows.Forms.Splitter();
			this.listView2 = new System.Windows.Forms.ListView();
			this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
			this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.panel2.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.textBox3);
			this.groupBox1.Controls.Add(this.panel2);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(3, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Padding = new System.Windows.Forms.Padding(8);
			this.groupBox1.Size = new System.Drawing.Size(712, 330);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "report";
			// 
			// textBox3
			// 
			this.textBox3.AcceptsReturn = true;
			this.textBox3.AcceptsTab = true;
			this.textBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox3.Location = new System.Drawing.Point(8, 160);
			this.textBox3.Multiline = true;
			this.textBox3.Name = "textBox3";
			this.textBox3.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox3.Size = new System.Drawing.Size(696, 162);
			this.textBox3.TabIndex = 4;
			// 
			// panel2
			// 
			this.panel2.Controls.Add(this.button4);
			this.panel2.Controls.Add(this.label6);
			this.panel2.Controls.Add(this.label5);
			this.panel2.Controls.Add(this.label4);
			this.panel2.Controls.Add(this.label3);
			this.panel2.Controls.Add(this.label1);
			this.panel2.Controls.Add(this.textBox1);
			this.panel2.Controls.Add(this.listView1);
			this.panel2.Controls.Add(this.textBox2);
			this.panel2.Controls.Add(this.label2);
			this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
			this.panel2.Location = new System.Drawing.Point(8, 21);
			this.panel2.Name = "panel2";
			this.panel2.Size = new System.Drawing.Size(696, 139);
			this.panel2.TabIndex = 6;
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(312, 112);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(75, 24);
			this.button4.TabIndex = 9;
			this.button4.Text = "check all";
			this.button4.UseVisualStyleBackColor = true;
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(0, 80);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(191, 13);
			this.label6.TabIndex = 8;
			this.label6.Text = "and will not be made public in any way!";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(0, 64);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(217, 13);
			this.label5.TabIndex = 7;
			this.label5.Text = "Uploaded files will only be used for bugfixing,";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(208, 8);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(98, 13);
			this.label4.TabIndex = 6;
			this.label4.Text = "Files to be included";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(0, 104);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(130, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "Description of the problem";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(0, 8);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(35, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Name";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(40, 8);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(168, 20);
			this.textBox1.TabIndex = 1;
			// 
			// listView1
			// 
			this.listView1.CheckBoxes = true;
			this.listView1.Location = new System.Drawing.Point(312, 8);
			this.listView1.Name = "listView1";
			this.listView1.Size = new System.Drawing.Size(248, 104);
			this.listView1.TabIndex = 3;
			this.listView1.UseCompatibleStateImageBehavior = false;
			this.listView1.View = System.Windows.Forms.View.Details;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(40, 40);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(168, 20);
			this.textBox2.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(0, 40);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "Email";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(8, 8);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(726, 402);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.groupBox1);
			this.tabPage1.Controls.Add(this.panel1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(718, 376);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "New bug report";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.button3);
			this.panel1.Controls.Add(this.progressBar1);
			this.panel1.Controls.Add(this.button2);
			this.panel1.Controls.Add(this.button1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.panel1.Location = new System.Drawing.Point(3, 333);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(712, 40);
			this.panel1.TabIndex = 0;
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(504, 8);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(75, 23);
			this.button3.TabIndex = 7;
			this.button3.Text = "Close";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// progressBar1
			// 
			this.progressBar1.Location = new System.Drawing.Point(8, 8);
			this.progressBar1.Name = "progressBar1";
			this.progressBar1.Size = new System.Drawing.Size(280, 23);
			this.progressBar1.TabIndex = 0;
			// 
			// button2
			// 
			this.button2.Enabled = false;
			this.button2.Location = new System.Drawing.Point(416, 8);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 6;
			this.button2.Text = "Cancel";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.button2_Click);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(296, 8);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(112, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "Send bug report";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.textBox5);
			this.tabPage2.Controls.Add(this.splitter2);
			this.tabPage2.Controls.Add(this.textBox4);
			this.tabPage2.Controls.Add(this.splitter1);
			this.tabPage2.Controls.Add(this.listView2);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(718, 376);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Bug tracker";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// textBox5
			// 
			this.textBox5.AcceptsReturn = true;
			this.textBox5.AcceptsTab = true;
			this.textBox5.Dock = System.Windows.Forms.DockStyle.Fill;
			this.textBox5.Location = new System.Drawing.Point(267, 131);
			this.textBox5.Multiline = true;
			this.textBox5.Name = "textBox5";
			this.textBox5.ReadOnly = true;
			this.textBox5.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox5.Size = new System.Drawing.Size(448, 242);
			this.textBox5.TabIndex = 10;
			// 
			// splitter2
			// 
			this.splitter2.Dock = System.Windows.Forms.DockStyle.Top;
			this.splitter2.Location = new System.Drawing.Point(267, 128);
			this.splitter2.Name = "splitter2";
			this.splitter2.Size = new System.Drawing.Size(448, 3);
			this.splitter2.TabIndex = 3;
			this.splitter2.TabStop = false;
			// 
			// textBox4
			// 
			this.textBox4.AcceptsReturn = true;
			this.textBox4.AcceptsTab = true;
			this.textBox4.Dock = System.Windows.Forms.DockStyle.Top;
			this.textBox4.Location = new System.Drawing.Point(267, 3);
			this.textBox4.Multiline = true;
			this.textBox4.Name = "textBox4";
			this.textBox4.ReadOnly = true;
			this.textBox4.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.textBox4.Size = new System.Drawing.Size(448, 125);
			this.textBox4.TabIndex = 9;
			// 
			// splitter1
			// 
			this.splitter1.Location = new System.Drawing.Point(264, 3);
			this.splitter1.Name = "splitter1";
			this.splitter1.Size = new System.Drawing.Size(3, 370);
			this.splitter1.TabIndex = 1;
			this.splitter1.TabStop = false;
			// 
			// listView2
			// 
			this.listView2.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
			this.listView2.ContextMenuStrip = this.contextMenuStrip1;
			this.listView2.Dock = System.Windows.Forms.DockStyle.Left;
			this.listView2.FullRowSelect = true;
			this.listView2.HideSelection = false;
			this.listView2.Location = new System.Drawing.Point(3, 3);
			this.listView2.Name = "listView2";
			this.listView2.Size = new System.Drawing.Size(261, 370);
			this.listView2.TabIndex = 8;
			this.listView2.UseCompatibleStateImageBehavior = false;
			this.listView2.View = System.Windows.Forms.View.Details;
			this.listView2.SelectedIndexChanged += new System.EventHandler(this.listView2_SelectedIndexChanged);
			// 
			// columnHeader1
			// 
			this.columnHeader1.Text = "Number";
			this.columnHeader1.Width = 85;
			// 
			// columnHeader2
			// 
			this.columnHeader2.Text = "Date/Time";
			this.columnHeader2.Width = 145;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(117, 26);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(116, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// timer1
			// 
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// BugReportForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(742, 418);
			this.Controls.Add(this.tabControl1);
			this.Name = "BugReportForm";
			this.Padding = new System.Windows.Forms.Padding(8);
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "LSLEditor bug report";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.panel2.ResumeLayout(false);
			this.panel2.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.tabPage2.PerformLayout();
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.ListView listView1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.TextBox textBox3;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.ProgressBar progressBar1;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Splitter splitter2;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.Splitter splitter1;
		private System.Windows.Forms.ListView listView2;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.ColumnHeader columnHeader2;
	}
}