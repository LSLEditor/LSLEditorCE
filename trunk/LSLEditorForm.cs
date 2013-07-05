// /**
// ********
// *
// * ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon 'Dimentox Travanti' Husbands & Malcolm J. Kudra, who in turn License under the GPLv2.
// * In agreement with Alphons van der Heijden's wishes.
// *
// * The community would like to thank Alphons for all of his hard work, blood sweat and tears.
// * Without his work the community would be stuck with crappy editors.
// *
// * The source code in this file ("Source Code") is provided by The LSLEditor Group
// * to you under the terms of the GNU General Public License, version 2.0
// * ("GPL"), unless you have obtained a separate licensing agreement
// * ("Other License"), formally executed by you and The LSLEditor Group.  Terms of
// * the GPL can be found in the gplv2.txt document.
// *
// ********
// * GPLv2 Header
// ********
// * LSLEditor, a External editor for the LSL Language.
// * Copyright (C) 2010 The LSLEditor Group.
//
// * This program is free software; you can redistribute it and/or
// * modify it under the terms of the GNU General Public License
// * as published by the Free Software Foundation; either version 2
// * of the License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with this program; if not, write to the Free Software
// * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********
// *
// * The above copyright notice and this permission notice shall be included in all
// * copies or substantial portions of the Software.
// *
// ********
// */
using System;
using System.IO;
using System.Xml;

using System.Collections.Generic;

using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Globalization;
using System.Windows.Forms;
using System.Drawing.Printing;

using LSLEditor.Docking;
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

// At the bottom of the form there are


// 1) tabcontrol1 , holding tabbed documents
// 2) splitter1, for increasing simulator window
// 3) panel1 , holding simulator or listview for compiler errors

// righthand side

// 1) panel2 , for holding solution explorer

namespace LSLEditor
{
    public partial class LSLEditorForm : Form
    {
        public XmlDocument ConfLSL;
		public XmlDocument ConfCSharp;

		private Browser browser;
		private SimulatorConsole SimulatorConsole;


		public bool CancelClosing = false;

        public Solution.SolutionExplorer m_SolutionExplorer;

		public GListBoxWindow GListBoxWindow;
		public TooltipWindow TooltipMouse;
		public TooltipWindow TooltipKeyboard;
		public TooltipWindow TooltipListBox;

		public FindWindow findWindow;
		public GotoWindow GotoWindow;

		public List<llDialogForm> llDialogForms;
        public List<llTextBoxForm> llTextBoxForms;
		public List<PermissionsForm> PermissionForms;

		private UpdateApplicationForm updateApplicationForm;
        private Helpers.PrinterHelper printer;

		public SyntaxError SyntaxErrors;

		private System.Diagnostics.Process curProc;
		private const int WM_NCACTIVATE = 0x0086;
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_NCACTIVATE)
			{
				if (m.LParam != IntPtr.Zero)
				{
					m.WParam = new IntPtr(1);
				}
			}
			else
			{
				try { curProc.MaxWorkingSet = curProc.MaxWorkingSet; } catch {}
			}
			base.WndProc(ref m);
		}

		public Solution.SolutionExplorer SolutionExplorer
		{
			get
			{
                return this.m_SolutionExplorer;
			}
		}

		private void SetDefaultProperties()
		{
			if (Properties.Settings.Default.FontEditor == null)
				Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);

			if (Properties.Settings.Default.FontTooltips == null)
				Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);

			string strLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (Properties.Settings.Default.ProjectLocation == "")
				Properties.Settings.Default.ProjectLocation = strLocation;
		}

		public LSLEditorForm(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);

			if (args.Length == 1)
			{
				if (args[0] == "/reset")
				{
					Properties.Settings.Default.Reset();
					Properties.Settings.Default.CallUpgrade = false;
				}
			}

			if (Properties.Settings.Default.CallUpgrade)
			{
				Properties.Settings.Default.Upgrade();
				Properties.Settings.Default.CallUpgrade = false;
			}

			// set some normal values on some properties
			SetDefaultProperties();

			curProc = System.Diagnostics.Process.GetCurrentProcess();

			InitializeComponent();

            m_SolutionExplorer = new LSLEditor.Solution.SolutionExplorer();
            m_SolutionExplorer.parent = this;

			InitRecentFileList();
			InitRecentProjectList();
			InitPluginsList();

			SetupChildForms();

			try
			{
				Start(args);
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error: " + OopsFormatter.ApplyFormatting(exception.Message), "Oops");
			}
		}

		public Form[] Children
		{
			get
			{
				if (this.IsMdiContainer)
					return this.MdiChildren;

				List<Form> children = new List<Form>();
			//TODO: Find Child forms
                //foreach (TabPage tabPage in this.tabControlExtended1.TabPages)
				//	children.Add(tabPage.Tag as Form);
				return children.ToArray();
			}
		}

		public Form ActiveMdiForm
		{
			get
			{
				if (this.IsMdiContainer)
					return this.ActiveMdiChild;
				else
				{
                    //TODO: Get Active Mdi Form
                    return null;
                    //dockPanel.ActiveContent
				//	if (this.tabControlExtended1.SelectedTab == null)
				//		return null;
				//	return this.tabControlExtended1.SelectedTab.Tag as Form;
				}
			}
		}

		public void ActivateMdiForm(Form form)
		{
			if (this.IsMdiContainer)
			{
				this.ActivateMdiChild(form);
			}
			else
			{
                //TODO: Activate the right Mdi Form
				/*for (int intI = 0; intI < this.tabControlExtended1.TabCount; intI++)
				{
					TabPage tabPage = this.tabControlExtended1.TabPages[intI];
					EditForm f = tabPage.Tag as EditForm;
					if (f == form)
					{
						this.tabControlExtended1.SelectedIndex = intI;
						tabPage.Focus();
						break;
					}
				}*/
			}
		}

		public void AddForm(DockContent form)
		{
			if (this.IsMdiContainer)
			{
				//form.MdiParent = this;
				//form.Tag = null;
				//form.Show();
				//ActivateMdiChild(form);

                //TODO: add form in the right way
                form.Show(dockPanel);
			}
			else
			{



				//form.Visible = false;
				//form.MdiParent = null;
				//TabPage tabPage = new TabPage(form.Text+"   ");
				//tabPage.BackColor = Color.White;
				//for(int intI=form.Controls.Count-1;intI>=0;intI--)
				//	tabPage.Controls.Add(form.Controls[intI]);
				//tabPage.Tag = form;
				//form.Tag = tabPage;
				// Was already commented out //tabPage.Controls.Add(form.Controls[0]);


                //this.tabControlExtended1.TabPages.Add(tabPage);
				//this.tabControlExtended1.SelectedTab = tabPage;
			}
		}

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new LSLEditorForm(args));
		}

		private void Start(string[] args)
		{
            string fileFilterNotes = "Notecard files (*.txt)|*.txt|All files (*.*)|*.*";
            string fileFilterScripts = "Secondlife script files (*.lsl)|*.lsl|All files (*.*)|*.*";
            string fileFilterSolutions = "LSLEditor Solution File (*.sol)|*.sol|All Files (*.*)|*.*";

			this.ConfLSL = GetXmlFromResource(Properties.Settings.Default.ConfLSL);
			this.ConfCSharp = GetXmlFromResource(Properties.Settings.Default.ConfCSharp);

            this.openNoteFilesDialog.FileName = "";
			this.openNoteFilesDialog.Filter = fileFilterNotes;
			this.openNoteFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;

			this.saveNoteFilesDialog.FileName = "";
            this.saveNoteFilesDialog.Filter = fileFilterNotes;
			this.saveNoteFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;


			this.openScriptFilesDialog.FileName = "";
            this.openScriptFilesDialog.Filter = fileFilterScripts;
			this.openScriptFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;

			this.saveScriptFilesDialog.FileName = "";
            this.saveScriptFilesDialog.Filter = fileFilterScripts;
			this.saveScriptFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;

			this.openSolutionFilesDialog.FileName = "";
            this.openSolutionFilesDialog.Filter = fileFilterSolutions;
			this.openSolutionFilesDialog.InitialDirectory = Properties.Settings.Default.ProjectLocation;
			this.openSolutionFilesDialog.Multiselect = false;


			Version version = Assembly.GetExecutingAssembly().GetName().Version;
			this.Text += " " + version.Major + "." + version.Minor;

			if (System.Diagnostics.Debugger.IsAttached)
			{
				this.Text += " (ALPHA)";
			}
			else
			{
				if (Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).Contains("beta"))
					this.Text += " (BETA)";
			}

            //TODO: Fix close buttons on tabs
			// enables close buttons on tab
			//this.tabControlExtended1.SetDrawMode();
			//this.tabControlExtended1.OnTabClose += new EventHandler(tabControl1_OnTabClose);

			if (args.Length == 0)
			{
				NewFile();
			}
			else
			{
				if (args[0] == "/reset")
				{
					NewFile();
				}
				else
				{
					if (Path.GetExtension(args[0]) == ".sol")
					{
						this.SolutionExplorer.OpenSolution(args[0]);
					}
					else
					{
						bool blnRun = false;
						foreach (string strFileName in args)
						{
							if (strFileName == "/run")
							{
								blnRun = true;
								continue;
							}
							EditForm editForm = new EditForm(this);
                            editForm.LoadFile(strFileName);
                            editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
							AddForm(editForm);
						}
						if (blnRun)
						{
							this.stopToolStripMenuItem.Enabled = true;
							StartSimulator();
						}
					}
				}
			}
		}

		void TextBox_OnCursorPositionChanged(object sender, SyntaxRichTextBox.CursorPositionEventArgs e)
		{
			this.toolStripStatusLabel1.Text = string.Format("Ln {0,-10} Col {1,-10} Ch {2,-20} Ttl {3,-10} {4,-10} {5,-10}", e.Line, e.Column, e.Char, e.Total, e.Insert ? "INS" : "OVR", e.Caps ? "CAP" : "");
		}

		private XmlDocument GetXmlFromResource(string strName)
		{
			XmlDocument xml = new XmlDocument();
			Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + "."+strName);

			if (resource != null)
				xml.Load(resource);
			return xml;
		}

		private void NewNotecard()
		{
			EditForm editForm = new EditForm(this);
			editForm.IsScript = false;
			editForm.FullPathName = Properties.Settings.Default.ExampleNameNotecard;
			editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
			editForm.Dirty = false;
			AddForm(editForm);
		}

		private void NewFile()
		{
			EditForm editForm = new EditForm(this);
			editForm.SourceCode = Helpers.GetTemplate.Source();
			editForm.TextBox.FormatDocument();
			editForm.TextBox.ClearUndoStack();
			editForm.FullPathName = Properties.Settings.Default.ExampleName;
			editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
			AddForm(editForm);
		}

		public EditForm OpenFile(string strPath, Guid guid, bool blnIsScript)
		{
			EditForm editForm = null;
			if (this.Children.Length > 0)
			{
				editForm = this.Children[0] as EditForm;
				if (editForm != null && !editForm.IsDisposed)
				{
					if (editForm.ScriptName != Properties.Settings.Default.ExampleName || editForm.Dirty)
						editForm = null;
				}
			}

			if (editForm == null)
			{
				editForm = new EditForm(this);
				editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
				AddForm(editForm);
			}

			editForm.guid = guid;
			editForm.IsScript = blnIsScript;
			editForm.LoadFile(strPath);

			// 23 oct 2007
			ActivateMdiForm(editForm);

			UpdateRecentFileList(strPath);

			return editForm;
		}

		public void OpenFile(string strPath, Guid guid)
		{
			OpenFile(strPath, guid, true);
		}

		private void OpenFile(string strFileName)
		{
			OpenFile(strFileName, Guid.Empty , true);
		}

		private string ClippedPath(string strPath)
		{
			if (strPath.Length < Properties.Settings.Default.PathClipLength)
				return strPath;

			string strRoot = Path.GetPathRoot(strPath);
			string strTmp = "";
			while (strTmp.Length < Properties.Settings.Default.PathClipLength)
			{
				strTmp = Path.Combine(Path.GetFileName(strPath) , strTmp);
				strPath = Path.GetDirectoryName(strPath);
				if (strPath == strRoot || strPath == null)
					break;
			}
			return Path.Combine(Path.Combine(strRoot, "..."), strTmp);
		}

		private void InitRecentFileList()
		{
			if (Properties.Settings.Default.RecentFileList == null)
				Properties.Settings.Default.RecentFileList = new System.Collections.Specialized.StringCollection();
			int intLen = Properties.Settings.Default.RecentFileList.Count;
			for (int intI = 0; intI < intLen; intI++)
			{
				ToolStripMenuItem tsmi = new ToolStripMenuItem(ClippedPath(Properties.Settings.Default.RecentFileList[intI]));
				tsmi.Tag = Properties.Settings.Default.RecentFileList[intI];
				this.recentFileToolStripMenuItem.DropDownItems.Add(tsmi);
			}
		}

		private void InitRecentProjectList()
		{
			if (Properties.Settings.Default.RecentProjectList == null)
				Properties.Settings.Default.RecentProjectList = new System.Collections.Specialized.StringCollection();
			int intLen = Properties.Settings.Default.RecentProjectList.Count;
			for (int intI = 0; intI < intLen; intI++)
			{
				ToolStripMenuItem tsmi = new ToolStripMenuItem(ClippedPath(Properties.Settings.Default.RecentProjectList[intI]));
				tsmi.Tag = Properties.Settings.Default.RecentProjectList[intI];
				this.recentProjectToolStripMenuItem.DropDownItems.Add(tsmi);
			}
		}

		private void InitPluginsList()
		{
			// erase old plugins
			for (int intI = this.toolsStripMenuItem.DropDownItems.Count - 1; intI > 0; intI--)
				this.toolsStripMenuItem.DropDownItems.RemoveAt(intI);

			if (Properties.Settings.Default.Plugins == null)
				return;

			ToolStripMenuItem tsmi;
			EventHandler handler = new EventHandler(this.PluginsHandler);
			foreach (string strPlugin in Properties.Settings.Default.Plugins)
			{
				if(strPlugin.ToLower().Contains("lslint"))
					tsmi = new ToolStripMenuItem(strPlugin, null, handler, Keys.F7);
				else
					tsmi = new ToolStripMenuItem(strPlugin, null, handler);
				this.toolsStripMenuItem.DropDownItems.Add(tsmi);
			}
		}

		private void UpdateRecentFileList(string strPath)
		{
			ToolStripMenuItem tsmi = new ToolStripMenuItem(ClippedPath(strPath));
			tsmi.Tag = strPath;
			this.recentFileToolStripMenuItem.DropDownItems.Insert(0, tsmi);
			Properties.Settings.Default.RecentFileList.Insert(0, strPath);
			int intListLen = Properties.Settings.Default.RecentFileList.Count;
			for (int intI = intListLen - 1; intI > 0; intI--)
			{
				if (strPath.ToLower() == Properties.Settings.Default.RecentFileList[intI].ToLower())
				{
					this.recentFileToolStripMenuItem.DropDownItems.RemoveAt(intI);
					Properties.Settings.Default.RecentFileList.RemoveAt(intI);
				}
			}
			int intLen = Properties.Settings.Default.RecentFileMax;
			if (this.recentFileToolStripMenuItem.DropDownItems.Count > intLen)
			{
				this.recentFileToolStripMenuItem.DropDownItems.RemoveAt(intLen);
				Properties.Settings.Default.RecentFileList.RemoveAt(intLen);
			}
		}

		public void UpdateRecentProjectList(string strPath, bool AddToList)
		{
			ToolStripMenuItem tsmi = new ToolStripMenuItem(ClippedPath(strPath));
			tsmi.Tag = strPath;

			if (AddToList)
			{
				this.recentProjectToolStripMenuItem.DropDownItems.Insert(0, tsmi);
				Properties.Settings.Default.RecentProjectList.Insert(0, strPath);
			}

			int intListLen = Properties.Settings.Default.RecentProjectList.Count;
			for (int intI = intListLen - 1; intI > 0; intI--)
			{
				if (strPath.ToLower() == Properties.Settings.Default.RecentProjectList[intI].ToLower())
				{
					this.recentProjectToolStripMenuItem.DropDownItems.RemoveAt(intI);
					Properties.Settings.Default.RecentProjectList.RemoveAt(intI);
				}
			}

			int intLen = Properties.Settings.Default.RecentProjectMax;
			if (this.recentProjectToolStripMenuItem.DropDownItems.Count > intLen)
			{
				this.recentProjectToolStripMenuItem.DropDownItems.RemoveAt(intLen);
				Properties.Settings.Default.RecentProjectList.RemoveAt(intLen);
			}

		}

		private void ReadNoteFiles()
		{
			this.openNoteFilesDialog.Multiselect = true;
			if (this.openNoteFilesDialog.ShowDialog() == DialogResult.OK)
			{
				foreach (string strFileName in this.openNoteFilesDialog.FileNames)
				{
					if (File.Exists(strFileName))
					{
						OpenFile(strFileName, Guid.NewGuid(), false);
						UpdateRecentFileList(strFileName);
					}
				}
			}
		}

		private void ReadScriptFiles()
		{
			this.openScriptFilesDialog.Multiselect = true;
			if (this.openScriptFilesDialog.ShowDialog() == DialogResult.OK)
			{
				foreach (string strFileName in this.openScriptFilesDialog.FileNames)
				{
					if (File.Exists(strFileName))
					{
						OpenFile(strFileName, Guid.NewGuid());
						UpdateRecentFileList(strFileName);
					}
				}
			}
		}

		private void importExampleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Browser browser = GetBrowser();
			browser.ShowWebBrowser("Import Examples", Properties.Settings.Default.Examples);
		}

        /***
         * Return value indicate whether file was saved or not.
         */
		public bool SaveFile(EditForm editForm, bool blnSaveAs)
		{
			DialogResult dialogresult = DialogResult.OK;
			if (editForm.FullPathName == Properties.Settings.Default.ExampleName || blnSaveAs)
			{
                SaveFileDialog saveDialog = editForm.IsScript ? this.saveScriptFilesDialog : this.saveNoteFilesDialog;
                saveDialog.FileName = editForm.FullPathName;
				string strExtension = Path.GetExtension(editForm.FullPathName);
                dialogresult = saveDialog.ShowDialog();
				if (dialogresult == DialogResult.OK)
                    editForm.FullPathName = saveDialog.FileName;
			}
			if (dialogresult == DialogResult.OK)
			{
				editForm.SaveCurrentFile();
				UpdateRecentFileList(editForm.FullPathName);
				return true;
			}
			return false;
		}

		// save file of active MDI child, if any
		private bool SaveActiveFile()
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return false;
			// save as!!
            // TODO: Refactor saveDialog to be a property of the form
            SaveFileDialog saveDialog = editForm.IsScript ? this.saveScriptFilesDialog : this.saveNoteFilesDialog;
            return SaveFile(editForm, true);
		}


		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.SaveCurrentFile();
			this.Focus();
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveActiveFile();
		}

		private Browser GetBrowser()
		{
			if (this.browser == null || this.browser.IsDisposed)
			{
				this.browser = new Browser(this);
				if (Properties.Settings.Default.BrowserLocation != Point.Empty)
				{
					this.browser.StartPosition = FormStartPosition.Manual;
					this.browser.Location = Properties.Settings.Default.BrowserLocation;
				}
				if (Properties.Settings.Default.BrowserSize != Size.Empty)
					this.browser.Size = Properties.Settings.Default.BrowserSize;
				this.browser.SizeChanged += new EventHandler(browser_SizeChanged);
				this.browser.LocationChanged += new EventHandler(browser_LocationChanged);
				if (this.browserInWindowToolStripMenuItem.Checked)
				{
					this.browser.MdiParent = null;
					this.browser.Show();
				}
				else
				{
					AddForm(this.browser);
					//this.browser.MdiParent = this;
				}
			}
			this.browser.Activate();

			if (this.IsMdiContainer)
			{
				this.ActivateMdiChild(this.browser);
			}
			else
			{
                //TODO: find browser in childs
				/*for (int intI = 0; intI < this.tabControlExtended1.TabCount; intI++)
				{
					TabPage tabPage = this.tabControlExtended1.TabPages[intI];
					Browser b = tabPage.Tag as Browser;
					if (b == this.browser)
					{
						this.tabControlExtended1.SelectedIndex = intI;
						tabPage.Focus();
						break;
					}
				}*/
			}

			return this.browser;
		}

		void browser_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.BrowserLocation = browser.Location;
		}

		void browser_SizeChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.BrowserSize = browser.Size;
		}

		#region printing
		private void pageSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Initialize the dialog's PrinterSettings property to hold user
			// defined printer settings.
				pageSetupDialog1.PageSettings =
					new System.Drawing.Printing.PageSettings();

			// Initialize dialog's PrinterSettings property to hold user
			// set printer settings.
				pageSetupDialog1.PrinterSettings =
					new System.Drawing.Printing.PrinterSettings();

			//Do not show the network in the printer dialog.
			pageSetupDialog1.ShowNetwork = false;

			//Show the dialog storing the result.
			if (pageSetupDialog1.ShowDialog() != DialogResult.OK)
			{
				pageSetupDialog1.PageSettings = null;
				pageSetupDialog1.PrinterSettings = null;
			}
		}

		private void printPreviewtoolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                printerHelp(editForm);
                printer.PrintPreviewEditForm(editForm);
            }
		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                printerHelp(editForm);
                printer.PrintEditForm(editForm);
            }
		}

        private void printerHelp(EditForm editForm)
        {
            printer = new Helpers.PrinterHelper(pageSetupDialog1);
            printer.Title = editForm.FullPathName;
            printer.SubTitle = DateTime.Now.ToString("s");
            printer.Footer = this.Text;
        }
		#endregion

		private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.ToClipBoard();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.Undo();
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.Redo();
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.Cut();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.Copy();
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.Paste();
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.Delete();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.SelectAll();
		}

		private void formatDocumentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.FormatDocument();
		}

		private void formatSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.AutoFormatSelectedText();
		}


		private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm != null)
			{
				ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
				if (tsmi != null)
				{
					tsmi.Checked = !tsmi.Checked;
					editForm.TextBox.WordWrap = tsmi.Checked;
				}
			}

		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.stopToolStripMenuItem.Enabled = true;
			StartSimulator();
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.stopToolStripMenuItem.Enabled = false;
			StopSimulator();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			About about = new About(this);
			about.ShowDialog(this);
		}

		public void OnDirtyChanged(bool blnEnableSave)
		{
			this.saveToolStripMenuItem.Enabled = blnEnableSave;
		}

		private void SetFontsOnWindows()
		{
			Font listBoxFont;
			Font toolTipFont;

			if (Properties.Settings.Default.FontEditor == null)
				Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);

			if (Properties.Settings.Default.FontTooltips == null)
				Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);

			toolTipFont = Properties.Settings.Default.FontTooltips;
			listBoxFont = Properties.Settings.Default.FontEditor;

			this.TooltipMouse.Font = toolTipFont;
			this.TooltipKeyboard.Font = toolTipFont;
			this.TooltipListBox.Font = toolTipFont;
			this.GListBoxWindow.Font = listBoxFont;

			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				editForm.SetFont();
			}
		}

		private void SetupChildForms()
		{
			Font listBoxFont = Properties.Settings.Default.FontEditor;
			Font toolTipFont = Properties.Settings.Default.FontTooltips;

			this.TooltipMouse = new TooltipWindow(this);
			this.TooltipKeyboard = new TooltipWindow(this);
			this.TooltipListBox = new TooltipWindow(this);
			this.GListBoxWindow = new GListBoxWindow(this);

			SetFontsOnWindows();

            //TODO: Fix new file drop down
			//this.solutionExplorer1.parent = this;
			//this.solutionExplorer1.CreateNewFileDrowDownMenu(this.addNewFileToolStripMenuItem);
			this.solutionExplorerToolStripMenuItem.Checked = Properties.Settings.Default.ShowSolutionExplorer;
			ShowSolutionExplorer(this.solutionExplorerToolStripMenuItem.Checked);

			this.llDialogForms = new List<llDialogForm>();
            this.llTextBoxForms = new List<llTextBoxForm>();
			this.PermissionForms = new List<PermissionsForm>();

			this.TooltipMouse.Tag = "";
			this.TooltipKeyboard.Tag = "";
			this.TooltipListBox.Tag = "";
			this.TooltipListBox.XOffset = 200; // TODO, afhankelijk van toegepaste font
			this.GListBoxWindow.Tag = "";

            // This order equals with the KeyWordTypeEnum in KeyWords.cs
			ImageList imageList = new ImageList();
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Unknown.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Functions.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Events.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Constants.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Class.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Vars.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Properties.gif"));
            imageList.Images.Add(new Bitmap(this.GetType(), "Images.States.gif"));

			this.GListBoxWindow.GListBox.ImageList = imageList;
           // this.tvOutline.ImageList = imageList;

			this.Move += new EventHandler(LSLEditorForm_SetPosition);
			this.Resize += new EventHandler(LSLEditorForm_SetPosition);

			this.GListBoxWindow.GListBox.DoubleClick += new EventHandler(GListBox_DoubleClick);
			this.GListBoxWindow.GListBox.SelectedIndexChanged += new EventHandler(GListBox_SelectedIndexChanged);
		}

		void GListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm != null)
				editForm.TextBox.ShowTooltipOnListBox();
		}

		void GListBox_DoubleClick(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm != null)
				editForm.TextBox.InsertSelectedWord();

		}

		private void LSLEditorForm_SetPosition(object sender, EventArgs e)
		{
			foreach (PermissionsForm pf in this.PermissionForms)
			{
				pf.Top = this.Top + 30;
				pf.Left = this.Right - pf.Width - 5;
			}
			foreach(llDialogForm df in this.llDialogForms)
			{
				df.Top = this.Top + 30;
				df.Left = this.Right - df.Width - 5;
			}
            foreach(llTextBoxForm tbf in this.llTextBoxForms)
            {
                tbf.Left = this.Left + this.Width / 2 - tbf.Width / 2;
                tbf.Top = this.Top + this.Height / 2 - tbf.Height / 2;
            }
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm != null)
				editForm.TextBox.SetPosition(Screen.PrimaryScreen.WorkingArea);
		}

		private void LoadProperties()
		{
			try
			{
                //TODO: hmmm?
				//this.tabControlExtended1.Visible = false;

				// this.panel1.Visible = false; // Simulator
				// this.panel2.Visible = false; // right pane

				//string strVersion = Properties.Settings.Default.Version;

				Size size = Properties.Settings.Default.LSLEditorSize;
				Point location = Properties.Settings.Default.LSLEditorLocation;
				Rectangle rect = new Rectangle(location, size);
				if(Screen.PrimaryScreen.WorkingArea.Contains(rect))
				{
					if (size.Width > 100 && size.Height > 100)
					{
						this.Location = location;
						this.Size = size;
					}
				}

				this.browserInWindowToolStripMenuItem.Checked =
					Properties.Settings.Default.BrowserInWindow;

				this.WikiSepBrowserstoolStripMenuItem.Checked =
					Properties.Settings.Default.WikiSeperateBrowser;

			}
			catch(Exception exception)
			{
				MessageBox.Show("Error Properties: " + OopsFormatter.ApplyFormatting(exception.Message), "Oops, but continue");
			}
		}

		public bool CloseAllOpenWindows()
		{
			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				ActivateMdiForm(editForm);
				if (editForm.Dirty)
				{
					DialogResult dialogResult = MessageBox.Show(this, @"Save """ + editForm.ScriptName + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
					if (dialogResult == DialogResult.Cancel)
					{
						return false;
					}
					if (dialogResult == DialogResult.Yes)
					{
                        // TODO: Refactor saveDialog to be a property of the form
                        SaveFileDialog saveDialog = editForm.IsScript ? this.saveScriptFilesDialog : this.saveNoteFilesDialog;
						if(!SaveFile(editForm, false))
							return false;
					}

					if (dialogResult == DialogResult.No)
					{
						editForm.Dirty = false;
					}
				}
				CloseActiveWindow();
			}
			return true;
		}

		private void LSLEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				Properties.Settings.Default.LSLEditorSize = this.Size;
				Properties.Settings.Default.LSLEditorLocation = this.Location;

				Properties.Settings.Default.Save();

				if (this.IsMdiContainer)
				{
					// this is set by any EditForm close
					e.Cancel = this.CancelClosing;
					return;
				}

				if (this.SolutionExplorer != null & !this.SolutionExplorer.IsDisposed)
				{
					this.SolutionExplorer.CloseSolution();
				}

				e.Cancel = !CloseAllOpenWindows();

			}
			catch
			{
			}
		}


		private void helpKeywordToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;

			Point dummy;
			string strKeyWord = editForm.TextBox.GetCurrentKeyWord(false, out dummy);
			ShowHelpOnKeyWord(strKeyWord);
		}

		private void ShowHelpOnKeyWord(string strKeyWord)
		{
			if (Properties.Settings.Default.HelpOffline)
			{
				string strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Settings.Default.HelpOfflineFile);
				if (!File.Exists(strHelpFile))
				{
					if (MessageBox.Show("No Offline help, use Online Wiki?", "Offline Help fails", MessageBoxButtons.OKCancel,MessageBoxIcon.Question) != DialogResult.OK)
						return;
				}
				else
				{
					if (strKeyWord == "")
						Help.ShowHelp(this, strHelpFile);
					else
						Help.ShowHelp(this, strHelpFile, HelpNavigator.KeywordIndex, strKeyWord);
					return;
				}
			}

			string strUrl = Properties.Settings.Default.Help + strKeyWord;

			if (strKeyWord == "")
				strKeyWord = "Help";

			if (Properties.Settings.Default.WikiSeperateBrowser)
			{
				System.Diagnostics.Process.Start(strUrl);
			}
			else
			{
				Browser browser = GetBrowser();
				browser.ShowWebBrowser(strKeyWord, strUrl);
			}
		}

		private void indexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string strKeyWord = "HomePage";
			ShowHelpOnKeyWord(strKeyWord);
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Browser browser = GetBrowser();
			string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			browser.ShowWebBrowser("Check for Updates", Properties.Settings.Default.Update + strVersion);
		}

		private void browserInWindowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.browserInWindowToolStripMenuItem.Checked = !this.browserInWindowToolStripMenuItem.Checked;
			Properties.Settings.Default.BrowserInWindow = this.browserInWindowToolStripMenuItem.Checked;
		}

		private void InitSyntaxError()
		{
			// information out of every script
			if (this.SyntaxErrors == null)
			{
				this.SyntaxErrors = new SyntaxError();
				this.SyntaxErrors.OnSyntaxError += new SyntaxError.SyntaxErrorHandler(SyntaxErrors_OnSyntaxError);
				this.SyntaxErrors.Dock = DockStyle.Fill;
			}
			this.SyntaxErrors.Clear();
		}

		private void SyntaxErrors_OnSyntaxError(object sender, SyntaxError.SyntaxErrorEventArgs e)
		{
			EditForm editForm = null;
			foreach (Form form in this.Children)
			{
				editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				if (editForm.FullPathName == e.FullPathName)
				{
					ActivateMdiForm(editForm);
					editForm.TextBox.Goto(e.Line, e.Char);
					editForm.Focus();
					return;
				}
			}
			editForm = OpenFile(e.FullPathName,e.EditFormGuid,e.IsScript);
			editForm.TextBox.Goto(e.Line, e.Char);
			editForm.Focus();
		}

		private void StartSimulator()
		{
			if (!SyntaxCheck(true))
				return;

			this.SimulatorConsole = new SimulatorConsole(this.SolutionExplorer, this.Children);

            this.SimulatorConsole.Show(dockPanel);
            //TODO: Show Simulator Console somewhere
            //this.panel1.Controls.Clear();
			//this.panel1.Controls.Add(this.SimulatorConsole);
			//this.panel1.Visible = true;
			//this.splitter1.SplitPosition = Properties.Settings.Default.SimulatorSize.Height;
		}

		public void StopSimulator()
		{
            //TODO: Hide simulator? Or we could keep it like the debug output in VS
			//this.panel1.Visible = false;
			if (this.SimulatorConsole != null)
			{
				this.SimulatorConsole.Stop();
				this.SimulatorConsole.Dispose();
			}
			this.SimulatorConsole = null;
		}


		private bool SyntaxCheck(bool Silent)
		{
            //TODO: What do we hide on SyntaxCheck?
			//this.panel1.Visible = false;
			InitSyntaxError();

			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				if (Properties.Settings.Default.AutoSaveOnDebug)
				{
					if (editForm.Dirty)
						editForm.SaveCurrentFile();
				}
				editForm.SyntaxCheck();
			}

			if (this.SyntaxErrors.HasErrors)
			{
                this.SyntaxErrors.Show(dockPanel);
                //TODO: Show errors somewhere in an output
				//this.panel1.Controls.Clear();
				//this.panel1.Controls.Add(this.SyntaxErrors);
				//this.panel1.Visible = true;
				//this.splitter1.SplitPosition = Properties.Settings.Default.SimulatorSize.Height;
				return false;
			}
			else
			{
				if (!Silent)
					MessageBox.Show("LSL Syntax seems OK", "LSL Syntax Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return true;
			}
		}

		private void releaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Browser browser = GetBrowser();
			string strExeFileName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
			browser.ShowWebBrowser("Release notes", "res://" + strExeFileName + "/" + Properties.Settings.Default.ReleaseNotes);

		}

		void findForm_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.FindLocation = findWindow.Location;
		}

		void gotoForm_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.GotoLocation = GotoWindow.Location;
		}

		private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
		{
            //TODO: Splitter moved? I Think this is depricated
			//if( this.splitter1.SplitPosition>50)
			//	Properties.Settings.Default.SimulatorSize = new Size(this.splitter1.Width, this.splitter1.SplitPosition);
		}

		void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.GListBoxWindow.Visible = false;
			this.TooltipMouse.Visible = false;
			this.TooltipKeyboard.Visible = false;
			this.TooltipListBox.Visible = false;
		}

		private void tabControlExtended1_MouseDown(object sender, MouseEventArgs e)
		{
			TabControl tabControl = sender as TabControl;
			if (tabControl == null)
				return;
			if (e.Button == MouseButtons.Right)
			{
				for (int intI = 0; intI < tabControl.TabCount; intI++)
				{
					Rectangle rt = tabControl.GetTabRect(intI);
					if (e.X > rt.Left && e.X < rt.Right
						&& e.Y > rt.Top && e.Y < rt.Bottom)
					{
						this.contextMenuStrip1.Tag = intI;
						this.contextMenuStrip1.Show(tabControl, new Point(e.X, e.Y));
					}
				}
			}
		}

		private bool IsInSolutionExplorer(Guid guid)
		{
			if (this.SolutionExplorer == null || this.SolutionExplorer.IsDisposed)
				return false;
			if (this.SolutionExplorer.GetKey(guid) == null)
				return false;
			return true;

		}

		private void CloseTab(int intTabToDelete)
		{
            //TODO: Find a new way for closing tabs
            /*
			// reset toolstrip information
			this.toolStripStatusLabel1.Text = "";

			//int intTabToDelete = (int)this.contextMenuStrip1.Tag;

            if (intTabToDelete >= this.tabControlExtended1.TabCount)
				return;

            TabPage tabPage = this.tabControlExtended1.TabPages[intTabToDelete];

            if (tabPage.Text.Contains("Browser"))
			{
				this.browser.Dispose();
				this.browser = null;
				GC.Collect();
			}

			EditForm editForm = tabPage.Tag as EditForm;
			if (editForm != null && !editForm.IsDisposed)
			{
				if (editForm.Dirty)
				{
					this.ActivateMdiForm(editForm);
					DialogResult dialogResult = MessageBox.Show(this, @"Save """ + editForm.ScriptName + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
					if (dialogResult == DialogResult.Cancel)
						return;
					if (dialogResult == DialogResult.Yes)
					{
						if(IsInSolutionExplorer(editForm.guid))
						{
							editForm.SaveCurrentFile();
						}
						else
						{
							// save as
							if(!SaveFile(editForm, true))
								return;
						}
					}
				}
				editForm.Dispose();
				editForm = null;
			}
			this.tabControlExtended1.TabPages[intTabToDelete].Dispose();
            */
			GC.Collect();
		}

		private void tabControl1_OnTabClose(object sender, EventArgs e)
		{
			int intTabToDelete = (int)sender;
			CloseTab(intTabToDelete);
		}

		private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int intTabToDelete = (int)this.contextMenuStrip1.Tag;
			CloseTab(intTabToDelete);
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			if (GotoWindow == null || GotoWindow.IsDisposed)
			{
				GotoWindow = new GotoWindow(this);
				if (Properties.Settings.Default.GotoLocation != Point.Empty)
				{
					GotoWindow.StartPosition = FormStartPosition.Manual;
					GotoWindow.Location = Properties.Settings.Default.GotoLocation;
				}
				GotoWindow.LocationChanged += new EventHandler(gotoForm_LocationChanged);
			}
			GotoWindow.Show(this);
		}


		public void CloseActiveWindow()
		{
			if (this.IsMdiContainer)
			{
				if(this.ActiveMdiForm!=null && !this.ActiveMdiForm.IsDisposed)
					this.ActiveMdiForm.Close();
			}
			else
			{
                //TODO: Find a new way
				/*
                int intTabToClose = this.tabControlExtended1.SelectedIndex;
				if (intTabToClose >= 0)
					CloseTab(intTabToClose);

                 */
            }
		}

		private void closeActiveWindowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseActiveWindow();
		}

		private void WikiSepBrowserstoolStripMenuItem_Click(object sender, EventArgs e)
		{
			WikiSepBrowserstoolStripMenuItem.Checked = !WikiSepBrowserstoolStripMenuItem.Checked;
			Properties.Settings.Default.WikiSeperateBrowser = this.WikiSepBrowserstoolStripMenuItem.Checked;
		}

		private void LSLEditorForm_Load(object sender, EventArgs e)
		{
			LoadProperties();
			if (Properties.Settings.Default.CheckForUpdates)
			{
				updateApplicationForm = new UpdateApplicationForm();
				updateApplicationForm.Icon = this.Icon;
				updateApplicationForm.OnUpdateAvailable += new EventHandler(updateApplicationForm_OnUpdateAvailable);
				updateApplicationForm.CheckForUpdate(false);
			}
		}

		void updateApplicationForm_OnUpdateAvailable(object sender, EventArgs e)
		{
			updateApplicationForm.ShowDialog(this);
		}

		private void commentInToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.MultiLineComment(true);

		}

		private void uncommentingSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.MultiLineComment(false);

		}

		private void FindandReplace(bool blnReplaceAlso)
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			if (findWindow == null || findWindow.IsDisposed)
			{
				findWindow = new FindWindow(this);
				findWindow.LocationChanged += new EventHandler(findForm_LocationChanged);
			}
			Rectangle rect = new Rectangle(this.Location, this.Size);
			if (rect.Contains(Properties.Settings.Default.FindLocation))
				findWindow.Location = Properties.Settings.Default.FindLocation;
			else
				findWindow.Location = this.Location;
			Point dummy;
			findWindow.ReplaceAlso = blnReplaceAlso;
			findWindow.KeyWord = editForm.TextBox.GetCurrentKeyWord(false, out dummy);
			if (findWindow.Visible == false)
				findWindow.Show(this);
			findWindow.FindFocus();
		}

		private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindandReplace(true);
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e)
		{
			FindandReplace(false);
		}

		private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (findWindow != null && !findWindow.IsDisposed)
				findWindow.Find();
		}


		public void ShowSolutionExplorer(bool blnVisible)
		{
            //TODO: We need another way to activate the Solution Explorer
			//this.panel2.Visible = blnVisible;
            if (blnVisible)
            {
                m_SolutionExplorer.Show(dockPanel);
            }
            else
            {
                m_SolutionExplorer.Hide();
            }
			this.solutionExplorerToolStripMenuItem.Checked = blnVisible;
			//this.tabControlExtended1.Refresh();
		}

		private void solutionExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.solutionExplorerToolStripMenuItem.Checked = !this.solutionExplorerToolStripMenuItem.Checked;
			Properties.Settings.Default.ShowSolutionExplorer = this.solutionExplorerToolStripMenuItem.Checked;
			ShowSolutionExplorer(this.solutionExplorerToolStripMenuItem.Checked);
		}

		private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewFile();
		}

		public void SolutionItemsFileMenu(bool blnVisible)
		{
			this.addToolStripMenuItem.Visible = blnVisible;
			this.addToolStripSeparator.Visible = blnVisible;
			this.closeSolutiontoolStripMenuItem.Enabled = blnVisible;
			this.addNewObjecttoolStripMenuItem.Enabled = blnVisible;
			this.addNewFileToolStripMenuItem.Enabled = blnVisible;
			this.newProjectToolStripMenuItem.Enabled = !blnVisible;
		}

        private void openNoteFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadNoteFiles();
        }

        private void openScriptFilesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReadScriptFiles();
        }

		private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CloseActiveWindow();
		}

		private void recentFileToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripMenuItem tsmi = e.ClickedItem as ToolStripMenuItem;
			if (tsmi == null)
				return;

			this.fileStripMenuItem.HideDropDown();

			string strPath = tsmi.Tag.ToString();
			OpenFile(strPath, Guid.NewGuid());
			UpdateRecentFileList(strPath);
		}

		private void makeBugReporttoolStripMenuItem_Click(object sender, EventArgs e)
		{
			BugReport.BugReportForm bugReport = new BugReport.BugReportForm(this);
			bugReport.Show(this);
		}

		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			UpdateApplicationForm uaf = new UpdateApplicationForm();
			uaf.Icon = this.Icon;
			uaf.CheckForUpdate(true);
			uaf.ShowDialog(this);
		}

		private void syntaxCheckerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SyntaxCheck(false);
		}

		#region SolutionExplorer
		private void openProjectSolutionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.openSolutionFilesDialog.ShowDialog(this) == DialogResult.OK)
			{
				if (File.Exists(this.openSolutionFilesDialog.FileName))
				{
					if(CloseAllOpenWindows())
						this.SolutionExplorer.OpenSolution(this.openSolutionFilesDialog.FileName);
				}
			}
		}

		private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewProject np = new NewProject(this);
			if (np.ShowDialog(this) == DialogResult.OK)
				CloseAllOpenWindows();

		}

		private void closeSolutiontoolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SolutionExplorer.CloseSolution();
		}

		private void newProjectToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			this.SolutionExplorer.AddNewProjectAction();
		}

		private void existingProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SolutionExplorer.AddExistingProject();
		}

		private void addNewObjecttoolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SolutionExplorer.AddNewObject();
		}

		#endregion SolutionExplorer

		private void recentProjectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			ToolStripMenuItem tsmi = e.ClickedItem as ToolStripMenuItem;
			if (tsmi == null)
				return;

			this.fileStripMenuItem.HideDropDown();

			if (CloseAllOpenWindows())
			{
				string strPath = tsmi.Tag.ToString();
				this.SolutionExplorer.OpenSolution(strPath);
			}
		}

		private void LSLEditorForm_DragEnter(object sender, DragEventArgs e)
		{
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
				e.Effect = DragDropEffects.Copy;
			else
				e.Effect = DragDropEffects.None;
		}

		private delegate void DelegateOpenFile(string s);
		private void LSLEditorForm_DragDrop(object sender, DragEventArgs e)
		{
			try
			{
				DelegateOpenFile delegateOpenFile = new DelegateOpenFile(OpenFile);
				Array allFiles = (Array)e.Data.GetData(DataFormats.FileDrop);
				if (allFiles == null)
					return;

				for (int intI = 0; intI < allFiles.Length; intI++)
				{
					string strFileName = allFiles.GetValue(intI).ToString();
					this.BeginInvoke(delegateOpenFile, new object[] { strFileName });
				}

				this.Activate(); // in the case Explorer overlaps this form
			}
			catch
			{
				// Error in DragDrop function (dont show messagebox, explorer is waiting
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Tools.ToolsOptions to = new Tools.ToolsOptions();
			to.PropertiesChanged += new LSLEditor.Tools.ToolsOptions.PropertiesChangedHandler(to_PropertiesChanged);
			to.Icon = this.Icon;
			to.ShowDialog(this);
		}

		private void to_PropertiesChanged()
		{
			this.browserInWindowToolStripMenuItem.Checked = Properties.Settings.Default.BrowserInWindow;
			this.WikiSepBrowserstoolStripMenuItem.Checked = Properties.Settings.Default.WikiSeperateBrowser;

			SetFontsOnWindows();
			InitPluginsList();
		}

		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			Browser browser = GetBrowser();
			browser.ShowWebBrowser("Donate", Properties.Settings.Default.DonateUrl);
		}

		private void LSLint()
		{
			InitSyntaxError();

			Plugins.LSLint lslint = new Plugins.LSLint();

			if (lslint.SyntaxCheck(this))
			{
				if (lslint.HasErrors)
				{
					this.SyntaxErrors.Show(dockPanel);
				}
				else
				{
					MessageBox.Show("LSL Syntax seems OK", "LSLint Syntax Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			}
			else
			{
				MessageBox.Show("LSLint:" + lslint.ExceptionMessage, "LSLint plugin", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Particles()
		{
			Plugins.Particles particles = new Plugins.Particles(this);
		}

		private void SvnPlugin()
		{
			if (File.Exists(Svn.Executable))
			{
				Properties.Settings.Default.VersionControl = true;
				Properties.Settings.Default.VersionControlSVN = true;
				Properties.Settings.Default.SvnExe = Svn.Executable;
				MessageBox.Show("SVN is installed and can be used in the solution explorer", "SVN information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			else
			{
				Properties.Settings.Default.VersionControl = false;
				Properties.Settings.Default.VersionControlSVN = false;
				MessageBox.Show("SVN is NOT installed (can not find svn binary)", "SVN information", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void GenericPlugin(string strPluginName)
		{
			Plugins.Generic generic = new Plugins.Generic(this, strPluginName);
		}


		private void PluginsHandler(object sender, EventArgs e)
		{
            //TODO: What do we hide here?
			//this.panel1.Visible = false;

			ToolStripMenuItem tsmi = sender as ToolStripMenuItem;
			if (tsmi == null)
				return;

			switch (tsmi.Text.ToLower())
			{
				case "lslint":
					LSLint();
					break;
				case "particles":
					Particles();
					break;
				case "svn (version control)":
					SvnPlugin();
					break;
				default:
					GenericPlugin(tsmi.Text);
					//MessageBox.Show("Unknown plugin", "LSLEditor plugins warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
					break;
			}
		}

		private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				if (editForm.Dirty)
					editForm.SaveCurrentFile();
			}
			// save it all, also solution explorer file
			if (this.SolutionExplorer != null & !this.SolutionExplorer.IsDisposed)
				this.SolutionExplorer.SaveSolutionFile();
		}

		private void SetupFileMenu()
		{
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm != null)
			{
				this.saveToolStripMenuItem.Text = "Save " + editForm.ScriptName;
				this.saveScriptFilesDialog.FileName = editForm.ScriptName;
				this.saveToolStripMenuItem.Enabled = editForm.Dirty;
				this.closeFileToolStripMenuItem.Enabled = true;
			}
			else
			{
				this.closeFileToolStripMenuItem.Enabled = false;
			}
		}

		private void tabControlExtended1_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetupFileMenu();
			EditForm ef = this.ActiveMdiForm as EditForm;
			if (ef != null)
				ef.SetFocus();
		}

		private void LSLEditorForm_MdiChildActivate(object sender, EventArgs e)
		{
			SetupFileMenu();
		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SetupFileMenu();
		}

		private void forumStripMenuItem_Click(object sender, EventArgs e)
		{
			Browser browser = GetBrowser();
			browser.ShowWebBrowser("LSLEditor Forum", Properties.Settings.Default.ForumLSLEditor);
		}

		private void notecardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			NewNotecard();
		}

		private void toolStripMenuItem7_Click(object sender, EventArgs e)
		{
			Browser browser = GetBrowser();
			browser.ShowWebBrowser("Contact Form", Properties.Settings.Default.ContactUrl);
		}

		private void toolStripMenuItem8_Click(object sender, EventArgs e)
		{
			//NativeHelper.SendMyKeys.PasteTextToApp("hello", "SecondLife", null);
			EditForm editForm = this.ActiveMdiForm as EditForm;
			if (editForm == null)
				return;
			editForm.TextBox.ToClipBoard();
			NativeHelper.SendMyKeys.ClipBoardToApp("SecondLife", null);
		}

        private void outlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //old
        }

        private void outlineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
        {
            foreach (Form k in this.Children)
            {
                EditForm editForm = k as EditForm;
                if (editForm == null)
                    return;
                if (outlineToolStripMenuItem.Checked)
                {
                    editForm.splitContainer1.Panel2Collapsed = false;
                }
                else
                {
                    editForm.splitContainer1.Panel2Collapsed = true;
                }
            }
        }



        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            Browser browser = GetBrowser();
            browser.ShowWebBrowser("LSLEditor QA", Properties.Settings.Default.qasite);
        }


	}
}