using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

using System.Collections;

using Microsoft.Win32;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Printing;


// aximp is oude informatie, maar ik laat het er even instaan
// aximp %WINDIR%\System32\shdocvw.dll /out:"d:\temp\AxInterop.SHDocVw.dll" /keyfile:"D:\Documents and Settings\Administrator\Mijn documenten\Mijn keys\Test.snk"
// copieer de TWEE files AxInterop.SHDocVw.dll en SHDocVw.dll in de bin/Debug directory
// referentie maken naar die twee files

//
// Pre build command for using a LSLEditor.rc file containing: About.htm HTML "Resource/About.htm"
//
// "$(DevEnvDir)..\..\SDK\v2.0\bin\rc.exe" /r "$(ProjectDir)$(TargetName).rc"
//
// Project properties Application, resource file

namespace LSLEditor
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class Form1 : System.Windows.Forms.Form
	{
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.SaveFileDialog saveFileDialog1;
		private System.Windows.Forms.Panel panel1;
		private System.ComponentModel.IContainer components = null;
		private Compiler compiler;
		private bool FullDebug;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.ContextMenu contextMenu1;

		private System.Windows.Forms.MainMenu mainMenu1;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem3;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem menuItem8;
		private System.Windows.Forms.MenuItem menuItem9;
		private System.Windows.Forms.MenuItem menuItem10;
		private System.Windows.Forms.MenuItem menuItem11;
		private System.Windows.Forms.MenuItem menuItem12;
		private System.Windows.Forms.MenuItem menuItem13;
		private System.Windows.Forms.MenuItem menuItem14;
		private System.Windows.Forms.MenuItem menuItem15;
		private System.Windows.Forms.MenuItem menuItem16;
		private System.Windows.Forms.MenuItem menuItem17;
		private System.Windows.Forms.MenuItem menuItem18;
		private System.Windows.Forms.MenuItem menuItem19;
		private System.Windows.Forms.MenuItem menuItem20;
		private System.Windows.Forms.MenuItem menuItem21;
		private System.Windows.Forms.MenuItem menuItem22;
		private System.Windows.Forms.MenuItem menuItem23;
		private System.Windows.Forms.MenuItem menuItem24;
		private System.Windows.Forms.MenuItem menuItem25;
		private System.Windows.Forms.MenuItem menuItem26;
		private System.Windows.Forms.MenuItem menuItem27;
		private System.Windows.Forms.MenuItem menuItem28;
		private System.Windows.Forms.MenuItem menuItem29;
		private System.Windows.Forms.MenuItem menuItem30;
		private System.Windows.Forms.MenuItem menuItem31;
		private System.Windows.Forms.MenuItem menuItem32;
		private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
		private System.Windows.Forms.PrintDialog printDialog1;
		private System.Windows.Forms.MenuItem menuItem34;
		private System.Windows.Forms.MenuItem menuItem35;
		private System.Windows.Forms.MenuItem menuItem36;
		private TabPage tabPage1;
		private NumberedTextBox.NumberedTextBoxUC numberedTextBoxUC1;
		private MenuItem menuItem37;
		private MenuItem menuItem38;
		private MenuItem menuItem39;
		private MenuItem menuItem41;
		private MenuItem menuItem40;
		private MenuItem menuItem42;
		private MenuItem menuItemUploadScript;
		private StatusStrip statusStrip1;
		private ToolStripStatusLabel toolStripStatusLabel1;
		private System.Windows.Forms.MenuItem menuItem33;

		public Form1(string[] args)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			Start(args);
		}



		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
				{
					components.Dispose();
				}
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.mainMenu1 = new System.Windows.Forms.MainMenu(this.components);
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem11 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem41 = new System.Windows.Forms.MenuItem();
			this.menuItem40 = new System.Windows.Forms.MenuItem();
			this.menuItem42 = new System.Windows.Forms.MenuItem();
			this.menuItemUploadScript = new System.Windows.Forms.MenuItem();
			this.menuItem15 = new System.Windows.Forms.MenuItem();
			this.menuItem13 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem35 = new System.Windows.Forms.MenuItem();
			this.menuItem34 = new System.Windows.Forms.MenuItem();
			this.menuItem36 = new System.Windows.Forms.MenuItem();
			this.menuItem31 = new System.Windows.Forms.MenuItem();
			this.menuItem32 = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.menuItem8 = new System.Windows.Forms.MenuItem();
			this.menuItem24 = new System.Windows.Forms.MenuItem();
			this.menuItem23 = new System.Windows.Forms.MenuItem();
			this.menuItem25 = new System.Windows.Forms.MenuItem();
			this.menuItem22 = new System.Windows.Forms.MenuItem();
			this.menuItem21 = new System.Windows.Forms.MenuItem();
			this.menuItem20 = new System.Windows.Forms.MenuItem();
			this.menuItem26 = new System.Windows.Forms.MenuItem();
			this.menuItem28 = new System.Windows.Forms.MenuItem();
			this.menuItem27 = new System.Windows.Forms.MenuItem();
			this.menuItem37 = new System.Windows.Forms.MenuItem();
			this.menuItem38 = new System.Windows.Forms.MenuItem();
			this.menuItem39 = new System.Windows.Forms.MenuItem();
			this.menuItem9 = new System.Windows.Forms.MenuItem();
			this.menuItem12 = new System.Windows.Forms.MenuItem();
			this.menuItem14 = new System.Windows.Forms.MenuItem();
			this.menuItem33 = new System.Windows.Forms.MenuItem();
			this.menuItem29 = new System.Windows.Forms.MenuItem();
			this.menuItem30 = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.menuItem16 = new System.Windows.Forms.MenuItem();
			this.menuItem17 = new System.Windows.Forms.MenuItem();
			this.menuItem18 = new System.Windows.Forms.MenuItem();
			this.menuItem19 = new System.Windows.Forms.MenuItem();
			this.menuItem3 = new System.Windows.Forms.MenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.panel1 = new System.Windows.Forms.Panel();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.statusStrip1 = new System.Windows.Forms.StatusStrip();
			this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.contextMenu1 = new System.Windows.Forms.ContextMenu();
			this.menuItem10 = new System.Windows.Forms.MenuItem();
			this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
			this.printDialog1 = new System.Windows.Forms.PrintDialog();
			this.numberedTextBoxUC1 = new NumberedTextBox.NumberedTextBoxUC();
			this.panel1.SuspendLayout();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.statusStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenu1
			// 
			this.mainMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem1,
            this.menuItem8,
            this.menuItem12,
            this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem11,
            this.menuItem4,
            this.menuItem41,
            this.menuItem40,
            this.menuItem42,
            this.menuItemUploadScript,
            this.menuItem15,
            this.menuItem13,
            this.menuItem5,
            this.menuItem7,
            this.menuItem35,
            this.menuItem34,
            this.menuItem36,
            this.menuItem31,
            this.menuItem32,
            this.menuItem6});
			this.menuItem1.Text = "File";
			// 
			// menuItem11
			// 
			this.menuItem11.Index = 0;
			this.menuItem11.Text = "New";
			this.menuItem11.Click += new System.EventHandler(this.menuItem11_Click);
			// 
			// menuItem4
			// 
			this.menuItem4.Index = 1;
			this.menuItem4.Text = "Open...";
			this.menuItem4.Click += new System.EventHandler(this.menuItem4_Click);
			// 
			// menuItem41
			// 
			this.menuItem41.Index = 2;
			this.menuItem41.Text = "-";
			// 
			// menuItem40
			// 
			this.menuItem40.Index = 3;
			this.menuItem40.Text = "Import Example...";
			this.menuItem40.Click += new System.EventHandler(this.menuItem40_Click);
			// 
			// menuItem42
			// 
			this.menuItem42.Index = 4;
			this.menuItem42.Text = "-";
			// 
			// menuItemUploadScript
			// 
			this.menuItemUploadScript.Enabled = false;
			this.menuItemUploadScript.Index = 5;
			this.menuItemUploadScript.Text = "Upload Your LSL script...";
			this.menuItemUploadScript.Click += new System.EventHandler(this.menuItemUploadScript_Click);
			// 
			// menuItem15
			// 
			this.menuItem15.Index = 6;
			this.menuItem15.Text = "-";
			// 
			// menuItem13
			// 
			this.menuItem13.Index = 7;
			this.menuItem13.Shortcut = System.Windows.Forms.Shortcut.CtrlS;
			this.menuItem13.Text = "Save";
			this.menuItem13.Click += new System.EventHandler(this.menuItem13_Click);
			// 
			// menuItem5
			// 
			this.menuItem5.Index = 8;
			this.menuItem5.Text = "Save...";
			this.menuItem5.Click += new System.EventHandler(this.menuItem5_Click);
			// 
			// menuItem7
			// 
			this.menuItem7.Index = 9;
			this.menuItem7.Text = "-";
			// 
			// menuItem35
			// 
			this.menuItem35.Index = 10;
			this.menuItem35.Text = "Page Setup...";
			this.menuItem35.Click += new System.EventHandler(this.menuItem35_Click);
			// 
			// menuItem34
			// 
			this.menuItem34.Index = 11;
			this.menuItem34.Shortcut = System.Windows.Forms.Shortcut.CtrlP;
			this.menuItem34.Text = "Print";
			this.menuItem34.Click += new System.EventHandler(this.menuItem34_Click);
			// 
			// menuItem36
			// 
			this.menuItem36.Index = 12;
			this.menuItem36.Text = "-";
			// 
			// menuItem31
			// 
			this.menuItem31.Index = 13;
			this.menuItem31.Text = "Copy to clipboard";
			this.menuItem31.Click += new System.EventHandler(this.menuItem31_Click);
			// 
			// menuItem32
			// 
			this.menuItem32.Index = 14;
			this.menuItem32.Text = "-";
			// 
			// menuItem6
			// 
			this.menuItem6.Index = 15;
			this.menuItem6.Text = "Exit";
			this.menuItem6.Click += new System.EventHandler(this.menuItem6_Click);
			// 
			// menuItem8
			// 
			this.menuItem8.Index = 1;
			this.menuItem8.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem24,
            this.menuItem23,
            this.menuItem25,
            this.menuItem22,
            this.menuItem21,
            this.menuItem20,
            this.menuItem26,
            this.menuItem28,
            this.menuItem27,
            this.menuItem37,
            this.menuItem9});
			this.menuItem8.Text = "Edit";
			// 
			// menuItem24
			// 
			this.menuItem24.Index = 0;
			this.menuItem24.Shortcut = System.Windows.Forms.Shortcut.CtrlZ;
			this.menuItem24.Text = "Undo";
			this.menuItem24.Click += new System.EventHandler(this.menuItem24_Click);
			// 
			// menuItem23
			// 
			this.menuItem23.Enabled = false;
			this.menuItem23.Index = 1;
			this.menuItem23.Shortcut = System.Windows.Forms.Shortcut.CtrlY;
			this.menuItem23.Text = "Redo";
			this.menuItem23.Click += new System.EventHandler(this.menuItem23_Click);
			// 
			// menuItem25
			// 
			this.menuItem25.Index = 2;
			this.menuItem25.Text = "-";
			// 
			// menuItem22
			// 
			this.menuItem22.Index = 3;
			this.menuItem22.Shortcut = System.Windows.Forms.Shortcut.CtrlX;
			this.menuItem22.Text = "Cut";
			this.menuItem22.Click += new System.EventHandler(this.menuItem22_Click);
			// 
			// menuItem21
			// 
			this.menuItem21.Index = 4;
			this.menuItem21.Shortcut = System.Windows.Forms.Shortcut.CtrlC;
			this.menuItem21.Text = "Copy";
			this.menuItem21.Click += new System.EventHandler(this.menuItem21_Click);
			// 
			// menuItem20
			// 
			this.menuItem20.Index = 5;
			this.menuItem20.Shortcut = System.Windows.Forms.Shortcut.CtrlV;
			this.menuItem20.Text = "Paste";
			this.menuItem20.Click += new System.EventHandler(this.menuItem20_Click);
			// 
			// menuItem26
			// 
			this.menuItem26.Index = 6;
			this.menuItem26.Shortcut = System.Windows.Forms.Shortcut.Del;
			this.menuItem26.Text = "Delete";
			this.menuItem26.Click += new System.EventHandler(this.menuItem26_Click);
			// 
			// menuItem28
			// 
			this.menuItem28.Index = 7;
			this.menuItem28.Shortcut = System.Windows.Forms.Shortcut.CtrlA;
			this.menuItem28.Text = "Select All";
			this.menuItem28.Click += new System.EventHandler(this.menuItem28_Click);
			// 
			// menuItem27
			// 
			this.menuItem27.Index = 8;
			this.menuItem27.Text = "-";
			// 
			// menuItem37
			// 
			this.menuItem37.Index = 9;
			this.menuItem37.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem38,
            this.menuItem39});
			this.menuItem37.Text = "Advanced";
			// 
			// menuItem38
			// 
			this.menuItem38.Index = 0;
			this.menuItem38.Text = "Format Document";
			this.menuItem38.Click += new System.EventHandler(this.menuItem38_Click);
			// 
			// menuItem39
			// 
			this.menuItem39.Enabled = false;
			this.menuItem39.Index = 1;
			this.menuItem39.Text = "Format Selection";
			this.menuItem39.Click += new System.EventHandler(this.menuItem39_Click);
			// 
			// menuItem9
			// 
			this.menuItem9.Enabled = false;
			this.menuItem9.Index = 10;
			this.menuItem9.Text = "Word wrap";
			this.menuItem9.Click += new System.EventHandler(this.menuItem9_Click);
			// 
			// menuItem12
			// 
			this.menuItem12.Index = 2;
			this.menuItem12.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem14,
            this.menuItem33,
            this.menuItem29,
            this.menuItem30});
			this.menuItem12.Text = "Compiler";
			// 
			// menuItem14
			// 
			this.menuItem14.Index = 0;
			this.menuItem14.Shortcut = System.Windows.Forms.Shortcut.F5;
			this.menuItem14.Text = "Start";
			this.menuItem14.Click += new System.EventHandler(this.menuItem14_Click);
			// 
			// menuItem33
			// 
			this.menuItem33.Index = 1;
			this.menuItem33.Text = "Compiler Window";
			this.menuItem33.Click += new System.EventHandler(this.menuItem33_Click);
			// 
			// menuItem29
			// 
			this.menuItem29.Index = 2;
			this.menuItem29.Text = "-";
			// 
			// menuItem30
			// 
			this.menuItem30.Index = 3;
			this.menuItem30.Text = "Properties...";
			this.menuItem30.Click += new System.EventHandler(this.menuItem30_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 3;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem16,
            this.menuItem17,
            this.menuItem18,
            this.menuItem19,
            this.menuItem3});
			this.menuItem2.Text = "Help";
			// 
			// menuItem16
			// 
			this.menuItem16.Index = 0;
			this.menuItem16.Shortcut = System.Windows.Forms.Shortcut.F1;
			this.menuItem16.Text = "Index...";
			this.menuItem16.Click += new System.EventHandler(this.menuItem16_Click);
			// 
			// menuItem17
			// 
			this.menuItem17.Index = 1;
			this.menuItem17.Text = "-";
			// 
			// menuItem18
			// 
			this.menuItem18.Index = 2;
			this.menuItem18.Text = "Check for Updates";
			this.menuItem18.Click += new System.EventHandler(this.menuItem18_Click);
			// 
			// menuItem19
			// 
			this.menuItem19.Index = 3;
			this.menuItem19.Text = "-";
			// 
			// menuItem3
			// 
			this.menuItem3.Index = 4;
			this.menuItem3.Text = "About...";
			this.menuItem3.Click += new System.EventHandler(this.menuItem3_Click);
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.tabControl1);
			this.panel1.Controls.Add(this.statusStrip1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(592, 353);
			this.panel1.TabIndex = 2;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(592, 331);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseDown);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.Color.Transparent;
			this.tabPage1.Controls.Add(this.numberedTextBoxUC1);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(584, 305);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "New";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// statusStrip1
			// 
			this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
			this.statusStrip1.Location = new System.Drawing.Point(0, 331);
			this.statusStrip1.Name = "statusStrip1";
			this.statusStrip1.Size = new System.Drawing.Size(592, 22);
			this.statusStrip1.TabIndex = 2;
			this.statusStrip1.Text = "statusStrip1";
			// 
			// toolStripStatusLabel1
			// 
			this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
			this.toolStripStatusLabel1.Size = new System.Drawing.Size(19, 17);
			this.toolStripStatusLabel1.Text = "...";
			// 
			// contextMenu1
			// 
			this.contextMenu1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItem10});
			// 
			// menuItem10
			// 
			this.menuItem10.Index = 0;
			this.menuItem10.Text = "Close";
			this.menuItem10.Click += new System.EventHandler(this.menuItem10_Click);
			// 
			// numberedTextBoxUC1
			// 
			this.numberedTextBoxUC1.BackColor = System.Drawing.SystemColors.Control;
			this.numberedTextBoxUC1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.numberedTextBoxUC1.Location = new System.Drawing.Point(3, 3);
			this.numberedTextBoxUC1.Name = "numberedTextBoxUC1";
			this.numberedTextBoxUC1.Size = new System.Drawing.Size(578, 299);
			this.numberedTextBoxUC1.TabIndex = 1;
			// 
			// Form1
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
			this.ClientSize = new System.Drawing.Size(592, 353);
			this.Controls.Add(this.panel1);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.IsMdiContainer = true;
			this.Menu = this.mainMenu1;
			this.Name = "Form1";
			this.Text = "LSL-Editor";
			this.Closing += new System.ComponentModel.CancelEventHandler(this.Form1_Closing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.statusStrip1.ResumeLayout(false);
			this.statusStrip1.PerformLayout();
			this.ResumeLayout(false);

		}
		#endregion

		private void Start(string[] args)
		{
			this.FullDebug = true;

			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);

			string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.Text += " " + strVersion;

			// load the keywords and colors, give handle to codecompletion listBox
			this.numberedTextBoxUC1.TextBox.Init("ConfLSL.xml");

			if (args.Length == 0)
			{
				this.numberedTextBoxUC1.TextBox.GetExampleFile();
				this.saveFileDialog1.FileName = "new.lsl";
				this.menuItem13.Text = "Save new.lsl";
				this.menuItem13.Enabled = false;
			}
			else
			{
				this.numberedTextBoxUC1.TextBox.LoadFile(args[0]);
				this.tabControl1.TabPages[0].Text = Path.GetFileName(args[0]);
				this.menuItem13.Text = "Save " + Path.GetFileName(args[0]);
				this.menuItem13.Enabled = true;
				this.saveFileDialog1.FileName = args[0];
			}
			this.numberedTextBoxUC1.TextBox.ToolTipping = true;
			this.numberedTextBoxUC1.TextBox.isDirty = false;
		}

		// close application
		private void menuItem6_Click(object sender, System.EventArgs e)
		{
			this.Close();
		}

		// reading file
		private void menuItem4_Click(object sender, System.EventArgs e)
		{
			if (this.numberedTextBoxUC1.TextBox.isDirty)
			{
				DialogResult dialogResult = MessageBox.Show(this, @"Save """ + this.tabControl1.TabPages[0].Text + @"""?", "cap", MessageBoxButtons.YesNoCancel);
				if (dialogResult == DialogResult.Yes)
					dialogResult = SaveCurrentFile();
				if (dialogResult == DialogResult.Cancel)
					return;
			}
			this.openFileDialog1.Filter = "Secondlife script files (*.lsl)|*.lsl|All files (*.*)|*.*";
			if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				if (File.Exists(this.openFileDialog1.FileName))
				{
					this.numberedTextBoxUC1.TextBox.LoadFile(this.openFileDialog1.FileName);
					this.numberedTextBoxUC1.TextBox.isDirty = false;
					this.saveFileDialog1.FileName = this.openFileDialog1.FileName;
					this.tabControl1.TabPages[0].Text = Path.GetFileName(this.openFileDialog1.FileName);

					this.menuItem13.Text = "Save " + Path.GetFileName(this.openFileDialog1.FileName);
					this.menuItem13.Enabled = true;
					this.saveFileDialog1.FileName = this.openFileDialog1.FileName;
				}
			}
		}

		// save current file
		private DialogResult SaveCurrentFile()
		{
			this.saveFileDialog1.Filter = "Secondlife script files (*.lsl)|*.lsl|All files (*.*)|*.*";
			DialogResult dialogresult = this.saveFileDialog1.ShowDialog();
			if (dialogresult == DialogResult.OK)
			{
				this.numberedTextBoxUC1.TextBox.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
				this.tabControl1.TabPages[0].Text = Path.GetFileName(this.saveFileDialog1.FileName);
				this.menuItem13.Text = "Save " + Path.GetFileName(this.saveFileDialog1.FileName);
				this.menuItem13.Enabled = true;
				this.numberedTextBoxUC1.TextBox.isDirty = false;
			}
			return dialogresult;
		}

		private void menuItem5_Click(object sender, System.EventArgs e)
		{
			SaveCurrentFile();
		}

		// about
		private void menuItem3_Click(object sender, System.EventArgs e)
		{
			//About about = new About(this);
			//about.ShowDialog(this);
		}

		// toggle word wrap
		private void menuItem9_Click(object sender, System.EventArgs e)
		{
			this.menuItem9.Checked = !this.menuItem9.Checked;
			this.numberedTextBoxUC1.TextBox.WordWrap = (this.menuItem9.Checked);
		}

		private void StartCompiler()
		{
			compiler = new Compiler(this.FullDebug, "ConfLSL.xml", this.numberedTextBoxUC1.TextBox.Text);
			if (compiler.CompilerErrors != null)
			{
				MessageBox.Show(compiler.CompilerErrors, "compiler errors");
				return;
			}

			if (this.menuItem33.Checked)
			{
				compiler.ShowDialog(this);
			}
			else
			{
				TabPage tabPage = null;
				for (int intI = 0; intI < this.tabControl1.TabPages.Count; intI++)
				{
					if (this.tabControl1.TabPages[intI].Text == "Compiler")
					{
						tabPage = this.tabControl1.TabPages[intI];
						tabPage.Controls.Clear();
						this.tabControl1.SelectedIndex = intI;
						break;
					}
				}
				if (tabPage == null)
				{
					tabPage = new TabPage("Compiler");
					this.tabControl1.TabPages.Add(tabPage);
					this.tabControl1.SelectedIndex = this.tabControl1.TabCount - 1;
				}
				tabPage.Controls.Add(compiler.Controls[0]);
			}
		}

		// compiler
		private void menuItem14_Click(object sender, System.EventArgs e)
		{
			StartCompiler();
		}

		// Empty current script
		private void menuItem11_Click(object sender, System.EventArgs e)
		{
			this.tabControl1.TabPages[0].Text = "New";
			this.saveFileDialog1.FileName = "new.lsl";
			this.menuItem13.Text = "Save new.lsl";
			this.menuItem13.Enabled = false;
			this.numberedTextBoxUC1.TextBox.Clear();
			this.numberedTextBoxUC1.TextBox.SelectionStart = 0;
			this.numberedTextBoxUC1.TextBox.SelectionLength = 0;
			this.numberedTextBoxUC1.TextBox.SelectionColor = Color.Black;
			this.numberedTextBoxUC1.TextBox.isDirty = false;
		}

		private void Form1_Load(object sender, System.EventArgs e)
		{
			try
			{
				RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"software\hwh\lsleditor");
				if (regKey != null)
				{
					this.Width = (int)regKey.GetValue("Width");
					this.Height = (int)regKey.GetValue("Height");
					this.Left = (int)regKey.GetValue("Left");
					this.Top = (int)regKey.GetValue("Top");
					this.numberedTextBoxUC1.TextBox.WordWrap = (regKey.GetValue("WordWrap").ToString() == "True");
					this.menuItem9.Checked = this.numberedTextBoxUC1.TextBox.WordWrap;
					this.menuItem33.Checked = (regKey.GetValue("CompilerInWindow").ToString() == "True");
					regKey.GetValue("Unknown").ToString();
					this.FullDebug = true;
					this.menuItemUploadScript.Enabled = true;
				}
			}
			catch
			{
			}
			//this.Activate();
		}

		private void Form1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			RegistryKey regKey = Registry.CurrentUser.CreateSubKey(@"software\hwh\lsleditor");
			if (regKey != null)
			{
				regKey.SetValue("Width", this.Width);
				regKey.SetValue("Height", this.Height);
				regKey.SetValue("Top", this.Top);
				regKey.SetValue("Left", this.Left);
				regKey.SetValue("WordWrap", this.numberedTextBoxUC1.TextBox.WordWrap);
				regKey.SetValue("CompilerInWindow", this.menuItem33.Checked);
				regKey.Flush();
				regKey.Close();
			}
			if (this.numberedTextBoxUC1.TextBox.isDirty)
			{
				DialogResult dialogResult = MessageBox.Show(this, @"Save """ + this.tabControl1.TabPages[0].Text + @"""?", "cap", MessageBoxButtons.YesNoCancel);
				if (dialogResult == DialogResult.Yes)
					dialogResult = SaveCurrentFile();
				e.Cancel = (dialogResult == DialogResult.Cancel);
			}
		}

		// F1
		private void ShowHelp()
		{
			string strKeyWord = this.numberedTextBoxUC1.TextBox.GetCurrentKeyWord(false);

			string strUrl = this.numberedTextBoxUC1.TextBox.HelpUrl + "wakka.php?wakka=" + strKeyWord;

			if (strKeyWord == "")
				strKeyWord = "Help";

			ShowWebBrowser(strKeyWord, strUrl);
		}

		// show context menu for tab headers
		private void tabControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right)
			{
				for (int intI = 1; intI < tabControl1.TabCount; intI++)
				{
					Rectangle rt = tabControl1.GetTabRect(intI);
					if (e.X > rt.Left && e.X < rt.Right
						&& e.Y > rt.Top && e.Y < rt.Bottom)
					{
						this.contextMenu1.Show(this.tabControl1, new Point(e.X, e.Y));
					}
				}
			}
		}

		// close tab
		private void menuItem10_Click(object sender, System.EventArgs e)
		{
			if (this.tabControl1.SelectedIndex > 0)
				this.tabControl1.TabPages.RemoveAt(this.tabControl1.SelectedIndex);
		}

		private void menuItem13_Click(object sender, System.EventArgs e)
		{
			// save as current file
			this.numberedTextBoxUC1.TextBox.SaveFile(this.saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
			this.numberedTextBoxUC1.TextBox.isDirty = false;
		}

		private void menuItem16_Click(object sender, System.EventArgs e)
		{
			ShowHelp();
		}

		private void menuItem18_Click(object sender, System.EventArgs e)
		{
			ShowWebBrowser("Check for Updates", this.numberedTextBoxUC1.TextBox.UpdateUrl);
		}

		private void menuItem22_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.Cut();
		}

		private void menuItem21_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.Copy();
		}

		private void menuItem20_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.Paste();
		}

		private void menuItem26_Click(object sender, System.EventArgs e)
		{
			if (this.numberedTextBoxUC1.TextBox.SelectedText == "")
				this.numberedTextBoxUC1.TextBox.SelectionLength = 1;
			this.numberedTextBoxUC1.TextBox.SelectedText = "";
		}

		private void menuItem28_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.SelectAll();
		}

		private void menuItem24_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.UndoPlus();
		}

		private void menuItem23_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.Redo();
		}

		private void menuItem30_Click(object sender, System.EventArgs e)
		{
			SimProperties props = new SimProperties();
			props.Icon = this.Icon;
			props.ShowDialog(this);
		}

		private void menuItem31_Click(object sender, System.EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.ToClipBoard();
		}

		private void menuItem33_Click(object sender, System.EventArgs e)
		{
			this.menuItem33.Checked = !this.menuItem33.Checked;
		}

		private void menuItem35_Click(object sender, System.EventArgs e)
		{
			PrintDocument docPrn = new PrintDocument();
			docPrn.DocumentName = this.saveFileDialog1.FileName;
			this.pageSetupDialog1.Document = docPrn;
			this.pageSetupDialog1.ShowDialog();
		}

		private void menuItem34_Click(object sender, System.EventArgs e)
		{
			this.printDialog1.AllowPrintToFile = true;
			PrintDocument docPrn = new PrintDocument();
			docPrn.DocumentName = this.saveFileDialog1.FileName;
			this.printDialog1.Document = docPrn;
			if (this.printDialog1.ShowDialog() == DialogResult.OK)
			{
				try
				{
					docPrn.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);
					docPrn.Print();
				}
				catch
				{
					MessageBox.Show("Error While Printing", "Print Error");
				}
			}
		}

		private void printDoc_PrintPage(Object sender, PrintPageEventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.Print(0, this.numberedTextBoxUC1.TextBox.Text.Length, e);
		}

		private void menuItem38_Click(object sender, EventArgs e)
		{
			// Format Document
			AutoFormatter autoFormatter = new AutoFormatter();
			this.numberedTextBoxUC1.TextBox.Text =
				autoFormatter.ApplyFormatting(this.numberedTextBoxUC1.TextBox.Text);
		}

		private void menuItem39_Click(object sender, EventArgs e)
		{
			// Format Selection
			AutoFormatter autoFormatter = new AutoFormatter();
			this.numberedTextBoxUC1.TextBox.SelectedText =
				autoFormatter.ApplyFormatting(this.numberedTextBoxUC1.TextBox.SelectedText);
		}

		private void menuItem40_Click(object sender, EventArgs e)
		{
			ShowWebBrowser("Import Examples", this.numberedTextBoxUC1.TextBox.ExamplesUrl);
		}

		private void ShowWebBrowser(string strTabName, string strUrl)
		{
			TabPage tabPage = new TabPage(strTabName);

			WebBrowser axWebBrowser1 = new WebBrowser();
			tabPage.Controls.Add(axWebBrowser1);
			this.tabControl1.TabPages.Add(tabPage);
			this.tabControl1.SelectedIndex = this.tabControl1.TabCount - 1;

			axWebBrowser1.Dock = DockStyle.Fill;
			axWebBrowser1.StatusTextChanged += new EventHandler(axWebBrowser1_StatusTextChanged);
			axWebBrowser1.Navigating += new WebBrowserNavigatingEventHandler(axWebBrowser1_Navigating);
			axWebBrowser1.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(axWebBrowser1_DocumentCompleted);
			axWebBrowser1.Navigate(strUrl);
		}

		void axWebBrowser1_StatusTextChanged(object sender, EventArgs e)
		{
			WebBrowser axWebBrowser1 = sender as WebBrowser;
			this.toolStripStatusLabel1.Text = axWebBrowser1.StatusText;
		}


		void axWebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			string strUrl = e.Url.ToString();
			if (strUrl.EndsWith(".lsl"))
			{
				e.Cancel = true;
				if (MessageBox.Show("Import LSL script?", "Import script", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
					return;

				WebBrowser axWebBrowser1 = sender as WebBrowser;
				axWebBrowser1.Stop();

				// download the url
				this.numberedTextBoxUC1.TextBox.LoadFile(strUrl);
				this.numberedTextBoxUC1.TextBox.isDirty = true;

				// Delete webbrowser?
				// axWebBrowser1.Dispose();
				// this.tabControl1.TabPages.RemoveAt(this.tabControl1.TabCount - 1);

				int intI = strUrl.LastIndexOf("/");
				if (intI > 0)
				{
					string strName = strUrl.Substring(intI + 1);
					this.tabControl1.TabPages[0].Text = strName;
					this.menuItem13.Text = "Save " + strName;
					this.menuItem13.Enabled = false;
					this.saveFileDialog1.FileName = strName;
				}

				this.tabControl1.SelectedIndex = 0;
			}
		}

		void axWebBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			WebBrowser axWebBrowser1 = sender as WebBrowser;
			if (e.Url.ToString() == this.numberedTextBoxUC1.TextBox.UploadUrl)
			{
				HtmlElement element = axWebBrowser1.Document.GetElementById("SourceCode");
				if (element != null)
				{
					//if(element.InnerText.Trim()=="")
					element.InnerText = this.numberedTextBoxUC1.TextBox.Text;

					// Makepictures
					Size oldSize = this.Size;
					this.Size = new Size(640, 480);

					Bitmap bitmap;
					ImageManipulation.GetBitmapFromControl helper = new ImageManipulation.GetBitmapFromControl();

					this.tabControl1.SelectedIndex = 0;
					bitmap = helper.GetOctreeQuantizedBitmap(this, false);
					bitmap.Save(@"d:\temp\test1.gif", ImageFormat.Gif);

					element = axWebBrowser1.Document.GetElementById("File1");
					if (element != null)
					{
						element.SetAttribute("value", @"d:\temp\test1.gif");
						element.OuterHtml = @"<input name=""File1"" type=""file"" id=""File1"" size=""80"" value=""test""/>";
					}

					if (this.tabControl1.TabPages[1].Text.Contains("Compiler"))
					{
						this.tabControl1.SelectedIndex = 1;
						bitmap = helper.GetOctreeQuantizedBitmap(this, false);
						bitmap.Save(@"d:\temp\test2.gif", ImageFormat.Gif);

						element = axWebBrowser1.Document.GetElementById("File2");
						if (element != null)
							element.SetAttribute("value", @"d:\temp\test2.gif");
					}

					this.tabControl1.SelectedIndex = this.tabControl1.TabPages.Count - 1;
					this.Size = oldSize;
				}
			}
		}

		private void UploadScript()
		{
			//ShowWebBrowser("Upload", this.numberedTextBoxUC1.TextBox.UploadUrl);

			ArrayList pictures = new ArrayList();
			Size oldSize = this.Size;
			int intSelectedTab = this.tabControl1.SelectedIndex;
			this.Size = new Size(640, 480);

			Bitmap bitmap;
			ImageManipulation.GetBitmapFromControl helper = new ImageManipulation.GetBitmapFromControl();

			this.tabControl1.SelectedIndex = 0;
			bitmap = helper.GetOctreeQuantizedBitmap(this, false);
			string strTempFileName = Path.GetTempFileName() + ".gif";
			bitmap.Save(strTempFileName, ImageFormat.Gif);
			pictures.Add(strTempFileName);

			for (int intI = 1; intI < this.tabControl1.TabPages.Count; intI++)
			{
				if (this.tabControl1.TabPages[intI].Text.Contains("Compiler"))
				{
					this.tabControl1.SelectedIndex = intI;
					bitmap = helper.GetOctreeQuantizedBitmap(this, false);
					strTempFileName = Path.GetTempFileName() + ".gif";
					bitmap.Save(strTempFileName, ImageFormat.Gif);
					pictures.Add(strTempFileName);
					break;
				}
			}
			this.tabControl1.SelectedIndex = intSelectedTab;
			this.Size = oldSize;

			UploadExamle u = new UploadExamle(
				Path.GetFileName(this.saveFileDialog1.FileName),
				this.numberedTextBoxUC1.TextBox.UploadUrl,
				this.numberedTextBoxUC1.TextBox.Text,
				pictures);
			u.ShowDialog(this);

			// cleanup
			foreach (string strFileName in pictures)
			{
				File.Delete(strFileName);
			}
		}

		private void menuItemUploadScript_Click(object sender, EventArgs e)
		{
			UploadScript();
		}
	}
}
