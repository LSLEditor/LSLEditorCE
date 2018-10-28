// <copyright file="gpl-2.0.txt">
// ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden.
// The code was donated on 2010-04-28 by Alphons van der Heijden to Brandon 'Dimentox Travanti' Husbands &
// Malcolm J. Kudra, who in turn License under the GPLv2 in agreement with Alphons van der Heijden's wishes.
//
// The community would like to thank Alphons for all of his hard work, blood sweat and tears. Without his work
// the community would be stuck with crappy editors.
//
// The source code in this file ("Source Code") is provided by The LSLEditor Group to you under the terms of the GNU
// General Public License, version 2.0 ("GPL"), unless you have obtained a separate licensing agreement ("Other
// License"), formally executed by you and The LSLEditor Group.
// Terms of the GPL can be found in the gplv2.txt document.
//
// GPLv2 Header
// ************
// LSLEditor, a External editor for the LSL Language.
// Copyright (C) 2010 The LSLEditor Group.
//
// This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public
// License as published by the Free Software Foundation; either version 2 of the License, or (at your option) any
// later version.
//
// This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied
// warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License for more
// details.
//
// You should have received a copy of the GNU General Public License along with this program; if not, write to the Free
// Software Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********************************************************************************************************************
// The above copyright notice and this permission notice shall be included in copies or substantial portions of the
// Software.
// ********************************************************************************************************************
// </copyright>
//
// <summary>
//
//
// </summary>

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
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

		private readonly System.Diagnostics.Process curProc;
		private const int WM_NCACTIVATE = 0x0086;

		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_NCACTIVATE) {
				if (m.LParam != IntPtr.Zero) {
					m.WParam = new IntPtr(1);
				}
			} else {
				try { this.curProc.MaxWorkingSet = this.curProc.MaxWorkingSet; } catch { }
			}
			base.WndProc(ref m);
		}

		public Solution.SolutionExplorer SolutionExplorer {
			get {
				return this.m_SolutionExplorer;
			}
		}

		private void SetDefaultProperties()
		{
			if (Properties.Settings.Default.FontEditor == null) {
				Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);
			}

			if (Properties.Settings.Default.FontTooltips == null) {
				Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);
			}

			var strLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (Properties.Settings.Default.ProjectLocation?.Length == 0) {
				Properties.Settings.Default.ProjectLocation = strLocation;
			}
		}

		public LSLEditorForm(string[] args)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US", false);
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US", false);

			if (args.Length == 1) {
				if (args[0] == "/reset") {
					Properties.Settings.Default.Reset();
					Properties.Settings.Default.CallUpgrade = false;
				}
			}

			if (Properties.Settings.Default.CallUpgrade) {
				Properties.Settings.Default.Upgrade();
				Properties.Settings.Default.CallUpgrade = false;
			}

			// set some normal values on some properties
			this.SetDefaultProperties();

			this.curProc = System.Diagnostics.Process.GetCurrentProcess();

			this.InitializeComponent();

			this.m_SolutionExplorer = new LSLEditor.Solution.SolutionExplorer {
				parent = this
			};

			this.InitRecentFileList();
			this.InitRecentProjectList();
			this.InitPluginsList();

			this.SetupChildForms();

			try {
				this.Start(args);
			} catch (Exception exception) {
				MessageBox.Show("Error: " + OopsFormatter.ApplyFormatting(exception.Message), "Oops");
			}
		}

		public Form[] Children {
			get {
				if (this.IsMdiContainer) {
					return this.MdiChildren;
				}

				var children = new List<Form>();
				//TODO: Find Child forms
				//foreach (TabPage tabPage in this.tabControlExtended1.TabPages)
				//	children.Add(tabPage.Tag as Form);
				return children.ToArray();
			}
		}

		public Form ActiveMdiForm {
			get {
				Form form = null;
				if (this.IsMdiContainer) {
					form = this.ActiveMdiChild;
				} else {
					//TODO: Get Active Mdi Form
					//return null;
					//dockPanel.ActiveContent
					//	if (this.tabControlExtended1.SelectedTab == null)
					//		return null;
					//	return this.tabControlExtended1.SelectedTab.Tag as Form;
				}
				return form;
			}
		}

		public void ActivateMdiForm(Form form)
		{
			if (this.IsMdiContainer) {
				this.ActivateMdiChild(form);
			} else {
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
			if (this.IsMdiContainer) {
				//form.MdiParent = this;
				//form.Tag = null;
				//form.Show();
				//ActivateMdiChild(form);

				//TODO: add form in the right way
				form.Show(this.dockPanel);
			} else {
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
		private static void Main(string[] args)
		{
			Application.EnableVisualStyles();
			Application.DoEvents();
			Application.Run(new LSLEditorForm(args));
		}

		private void Start(string[] args)
		{
			const string fileFilterNotes = "Notecard files (*.txt)|*.txt|All files (*.*)|*.*";
			const string fileFilterScripts = "Secondlife script files (*.lsl;*.lsli)|*.lsl;*.lsli|All files (*.*)|*.*";
			const string fileFilterSolutions = "LSLEditor Solution File (*.sol)|*.sol|All Files (*.*)|*.*";

			this.ConfLSL = this.GetXmlFromResource(Properties.Settings.Default.ConfLSL);
			this.ConfCSharp = this.GetXmlFromResource(Properties.Settings.Default.ConfCSharp);

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

			var version = Assembly.GetExecutingAssembly().GetName().Version;
			this.Text += " " + version.Major + "." + version.Minor;

			if (System.Diagnostics.Debugger.IsAttached) {
				this.Text += " (ALPHA)";
			} else {
				if (Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).Contains("beta")) {
					this.Text += " (BETA)";
				}
			}

			//TODO: Fix close buttons on tabs
			// enables close buttons on tab
			//this.tabControlExtended1.SetDrawMode();
			//this.tabControlExtended1.OnTabClose += new EventHandler(tabControl1_OnTabClose);

			if (args.Length == 0) {
				this.NewFile();
			} else {
				if (args[0] == "/reset") {
					this.NewFile();
				} else {
					if (Path.GetExtension(args[0]) == ".sol") {
						this.SolutionExplorer.OpenSolution(args[0]);
					} else {
						var blnRun = false;
						foreach (var strFileName in args) {
							if (strFileName == "/run") {
								blnRun = true;
								continue;
							}
							var editForm = new EditForm(this);
							editForm.LoadFile(strFileName);
							editForm.TextBox.OnCursorPositionChanged += this.TextBox_OnCursorPositionChanged;
							this.AddForm(editForm);
						}
						if (blnRun) {
							this.stopToolStripMenuItem.Enabled = true;
							this.StartSimulator();
						}
					}
				}
			}
		}

		private void TextBox_OnCursorPositionChanged(object sender, SyntaxRichTextBox.CursorPositionEventArgs e)
		{
			var editForm = (EditForm)this.ActiveMdiForm;
			var expandedWarning = "";
			if (editForm != null && Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName)) {
				expandedWarning = "Warning: Editing in included sections will be erased when collapsing/saving!";
			}

			this.toolStripStatusLabel1.Text = string.Format("Ln {0,-10} Col {1,-10} Ch {2,-20} Ttl {3,-10} {4,-10} {5,-10} {6}",
				e.Line, e.Column, e.Char, e.Total, e.Insert ? "INS" : "OVR", e.Caps ? "CAP" : "", expandedWarning);
		}

		private XmlDocument GetXmlFromResource(string strName)
		{
			var xml = new XmlDocument();
			var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(this.GetType().Namespace + "." + strName);

			if (resource != null) {
				xml.Load(resource);
			}
			return xml;
		}

		private void NewNotecard()
		{
			var fullPathName = Properties.Settings.Default.ExampleNameNotecard;
			var editForm = new EditForm(this) {
				IsScript = false,
				FullPathName = fullPathName
			};
			editForm.TextBox.OnCursorPositionChanged += this.TextBox_OnCursorPositionChanged;
			editForm.Dirty = false;
			this.AddForm(editForm);
		}

		private void NewFile(bool isLSLI = false)
		{
			var fullPathName = isLSLI ? Properties.Settings.Default.ExampleNameLSLI : Properties.Settings.Default.ExampleName;
			var editForm = new EditForm(this) {
				SourceCode = Helpers.GetTemplate.Source()
			};
			editForm.TextBox.FormatDocument();
			editForm.TextBox.ClearUndoStack();
			editForm.FullPathName = fullPathName;
			editForm.TextBox.OnCursorPositionChanged += this.TextBox_OnCursorPositionChanged;
			editForm.Dirty = false;
			this.AddForm(editForm);
		}

		public EditForm OpenFile(string strPath, Guid guid, bool blnIsScript)
		{
			EditForm editForm = null;

			foreach (var child in this.Children) {
				var form = child as EditForm;
				if (form?.IsDisposed != false || form.IsNew) {
					continue;
				}
				if (form.FullPathName == strPath) {
					editForm = form;
					break;
				}
			}

			if (editForm == null) {
				editForm = new EditForm(this);
				editForm.TextBox.OnCursorPositionChanged += this.TextBox_OnCursorPositionChanged;
				editForm.guid = guid;
				editForm.IsScript = blnIsScript;
				editForm.LoadFile(strPath);
				this.AddForm(editForm);
				this.ActivateMdiForm(editForm);
			} else {
				this.ActivateMdiForm(editForm);
				if (editForm.Dirty) {
					var dialogResult = MessageBox.Show(@"Revert file """ + editForm.ScriptName + @""" to last saved state? Your changes will be lost!", "File has changed", MessageBoxButtons.OKCancel);
					if (dialogResult == DialogResult.OK) {
						editForm.LoadFile(strPath);
						editForm.TextBox.ClearUndoStack();
						editForm.Dirty = false;
					}
				}
			}

			this.UpdateRecentFileList(strPath);

			if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.Text)) {
				editForm.Text = Helpers.LSLIPathHelper.GetExpandedTabName(editForm.Text);
			}

			return editForm;
		}

		public void OpenFile(string strPath, Guid guid)
		{
			this.OpenFile(strPath, guid, true);
		}

		private void OpenFile(string strFileName)
		{
			this.OpenFile(strFileName, Guid.NewGuid(), true);
		}

		private string ClippedPath(string strPath)
		{
			if (string.IsNullOrEmpty(strPath)) {
				return string.Empty;
			}

			var pathClipLength = Properties.Settings.Default.PathClipLength;
			if (pathClipLength < 1) {
				pathClipLength = 1;
			}

			var strFullPath = Path.GetFullPath(strPath);
			var lstDirectories = new List<string>(strFullPath.Split('\\'));

			var intCount = 0;
			var intLength = strFullPath.Length - pathClipLength;
			if (intLength > 0 && lstDirectories.Count > 2) {
				intLength += 4;

				var index = 1;
				var intRemoveCount = 0;
				while (index < lstDirectories.Count - 1 && intCount < intLength) {
					intCount += lstDirectories[index].Length + 1;
					intRemoveCount++;
					index++;
				}

				if (intRemoveCount > 0 && intCount >= 4) {
					lstDirectories.Insert(1, "...");
					lstDirectories.RemoveRange(2, intRemoveCount);
				}
			}

			return string.Join("\\", lstDirectories.ToArray());
		}

		private void InitRecentFileList()
		{
			if (Properties.Settings.Default.RecentFileList == null) {
				Properties.Settings.Default.RecentFileList = new System.Collections.Specialized.StringCollection();
			}
			var intLen = Properties.Settings.Default.RecentFileList.Count;
			for (var intI = 0; intI < intLen; intI++) {
				var tsmi = new ToolStripMenuItem(this.ClippedPath(Properties.Settings.Default.RecentFileList[intI])) {
					Tag = Properties.Settings.Default.RecentFileList[intI]
				};
				this.recentFileToolStripMenuItem.DropDownItems.Add(tsmi);
			}
		}

		private void InitRecentProjectList()
		{
			if (Properties.Settings.Default.RecentProjectList == null) {
				Properties.Settings.Default.RecentProjectList = new System.Collections.Specialized.StringCollection();
			}
			var intLen = Properties.Settings.Default.RecentProjectList.Count;
			for (var intI = 0; intI < intLen; intI++) {
				var tsmi = new ToolStripMenuItem(this.ClippedPath(Properties.Settings.Default.RecentProjectList[intI])) {
					Tag = Properties.Settings.Default.RecentProjectList[intI]
				};
				this.recentProjectToolStripMenuItem.DropDownItems.Add(tsmi);
			}
		}

		private void InitPluginsList()
		{
			// erase old plugins
			for (var intI = this.toolsStripMenuItem.DropDownItems.Count - 1; intI > 0; intI--) {
				this.toolsStripMenuItem.DropDownItems.RemoveAt(intI);
			}

			if (Properties.Settings.Default.Plugins != null) {
				ToolStripMenuItem tsmi;
				var handler = new EventHandler(this.PluginsHandler);
				foreach (var strPlugin in Properties.Settings.Default.Plugins) {
					tsmi = strPlugin.IndexOf("lslint", StringComparison.OrdinalIgnoreCase) >= 0
						? new ToolStripMenuItem(strPlugin, null, handler, Keys.F7)
						: new ToolStripMenuItem(strPlugin, null, handler);
					this.toolsStripMenuItem.DropDownItems.Add(tsmi);
				}
			}
		}

		private void UpdateRecentFileList(string strPath)
		{
			var tsmi = new ToolStripMenuItem(this.ClippedPath(strPath)) {
				Tag = strPath
			};
			this.recentFileToolStripMenuItem.DropDownItems.Insert(0, tsmi);
			Properties.Settings.Default.RecentFileList.Insert(0, strPath);
			var intListLen = Properties.Settings.Default.RecentFileList.Count;
			for (var intI = intListLen - 1; intI > 0; intI--) {
				if (string.Equals(strPath, Properties.Settings.Default.RecentFileList[intI], StringComparison.OrdinalIgnoreCase)) {
					this.recentFileToolStripMenuItem.DropDownItems.RemoveAt(intI);
					Properties.Settings.Default.RecentFileList.RemoveAt(intI);
				}
			}
			var intLen = Properties.Settings.Default.RecentFileMax;
			if (this.recentFileToolStripMenuItem.DropDownItems.Count > intLen) {
				this.recentFileToolStripMenuItem.DropDownItems.RemoveAt(intLen);
				Properties.Settings.Default.RecentFileList.RemoveAt(intLen);
			}
		}

		public void UpdateRecentProjectList(string strPath, bool AddToList)
		{
			var tsmi = new ToolStripMenuItem(this.ClippedPath(strPath)) {
				Tag = strPath
			};

			if (AddToList) {
				this.recentProjectToolStripMenuItem.DropDownItems.Insert(0, tsmi);
				Properties.Settings.Default.RecentProjectList.Insert(0, strPath);
			}

			var intListLen = Properties.Settings.Default.RecentProjectList.Count;
			for (var intI = intListLen - 1; intI > 0; intI--) {
				if (string.Equals(strPath, Properties.Settings.Default.RecentProjectList[intI], StringComparison.OrdinalIgnoreCase)) {
					this.recentProjectToolStripMenuItem.DropDownItems.RemoveAt(intI);
					Properties.Settings.Default.RecentProjectList.RemoveAt(intI);
				}
			}

			var intLen = Properties.Settings.Default.RecentProjectMax;
			if (this.recentProjectToolStripMenuItem.DropDownItems.Count > intLen) {
				this.recentProjectToolStripMenuItem.DropDownItems.RemoveAt(intLen);
				Properties.Settings.Default.RecentProjectList.RemoveAt(intLen);
			}
		}

		private void ReadNoteFiles()
		{
			this.openNoteFilesDialog.Multiselect = true;
			if (this.openNoteFilesDialog.ShowDialog() == DialogResult.OK) {
				foreach (var strFileName in this.openNoteFilesDialog.FileNames) {
					if (File.Exists(strFileName)) {
						this.OpenFile(strFileName, Guid.NewGuid(), false);
					}
				}
			}
		}

		private void ReadScriptFiles()
		{
			this.openScriptFilesDialog.Multiselect = true;
			if (this.openScriptFilesDialog.ShowDialog() == DialogResult.OK) {
				foreach (var strFileName in this.openScriptFilesDialog.FileNames) {
					if (File.Exists(strFileName)) {
						this.OpenFile(strFileName, Guid.NewGuid());
					}
				}
			}
		}

		private void importExampleToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			browser.ShowWebBrowser("Import Examples", Properties.Settings.Default.Examples);
		}

		/***
		 * Return value indicate whether file was saved or not.
		 */
		public bool SaveFile(EditForm editForm, bool blnSaveAs)
		{
			var dialogresult = DialogResult.OK;
			if (blnSaveAs || editForm.IsNew) {
				this.ActivateMdiForm(editForm);

				var saveDialog = editForm.IsScript ? this.saveScriptFilesDialog : this.saveNoteFilesDialog;

				// Save as LSLI when it's an expanded LSL
				saveDialog.FileName = Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName)
					? Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName)
					: editForm.ScriptName;
				//saveDialog.FileName = editForm.FullPathName;
				var strExtension = Path.GetExtension(editForm.FullPathName);
				dialogresult = saveDialog.ShowDialog();
				if (dialogresult == DialogResult.OK) {
					editForm.FullPathName = saveDialog.FileName;
				}
			}
			if (dialogresult == DialogResult.OK) {
				editForm.SaveCurrentFile();
				this.UpdateRecentFileList(editForm.FullPathName);
				return true;
			}
			return false;
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				this.SaveFile(editForm, false);
				this.Focus();
			}
		}

		private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				this.SaveFile(editForm, true);
				this.Focus();
			}
		}

		private Browser GetBrowser()
		{
			if (this.browser?.IsDisposed != false) {
				this.browser = new Browser(this);
				if (Properties.Settings.Default.BrowserLocation != Point.Empty) {
					this.browser.StartPosition = FormStartPosition.Manual;
					this.browser.Location = Properties.Settings.Default.BrowserLocation;
				}
				if (Properties.Settings.Default.BrowserSize != Size.Empty) {
					this.browser.Size = Properties.Settings.Default.BrowserSize;
				}
				this.browser.SizeChanged += this.browser_SizeChanged;
				this.browser.LocationChanged += this.browser_LocationChanged;
				if (this.browserInWindowToolStripMenuItem.Checked) {
					this.browser.MdiParent = null;
					this.browser.Show();
				} else {
					this.AddForm(this.browser);
					//this.browser.MdiParent = this;
				}
			}
			this.browser.Activate();

			if (this.IsMdiContainer) {
				this.ActivateMdiChild(this.browser);
			} else {
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

		private void browser_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.BrowserLocation = this.browser.Location;
		}

		private void browser_SizeChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.BrowserSize = this.browser.Size;
		}

		#region printing
		private void pageSettingsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Initialize the dialog's PrinterSettings property to hold user
			// defined printer settings.
			this.pageSetupDialog1.PageSettings = new System.Drawing.Printing.PageSettings();

			// Initialize dialog's PrinterSettings property to hold user
			// set printer settings.
			this.pageSetupDialog1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

			//Do not show the network in the printer dialog.
			this.pageSetupDialog1.ShowNetwork = false;

			//Show the dialog storing the result.
			if (this.pageSetupDialog1.ShowDialog() != DialogResult.OK) {
				this.pageSetupDialog1.PageSettings = null;
				this.pageSetupDialog1.PrinterSettings = null;
			}
		}

		private void printPreviewtoolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				this.printerHelp(editForm);
				this.printer.PrintPreviewEditForm(editForm);
			}
		}

		private void printToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				this.printerHelp(editForm);
				this.printer.PrintEditForm(editForm);
			}
		}

		private void printerHelp(EditForm editForm)
		{
			this.printer = new Helpers.PrinterHelper(this.pageSetupDialog1) {
				Title = editForm.FullPathName,
				SubTitle = DateTime.Now.ToString("s"),
				Footer = this.Text
			};
		}
		#endregion

		private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.ToClipBoard();
			}
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			// Check if a LSLI or expanded LSL open is, and close that one as well
			this.Close();
		}

		private void undoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.Undo();
			}
		}

		private void redoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.Redo();
			}
		}

		private void cutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.Cut();
			}
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.Copy();
			}
		}

		private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.Paste();
			}
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.Delete();
			}
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.SelectAll();
			}
		}

		private void formatDocumentToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.FormatDocument();
			}
		}

		private void formatSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.AutoFormatSelectedText();
			}
		}

		private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				if (sender is ToolStripMenuItem tsmi) {
					tsmi.Checked = !tsmi.Checked;
					editForm.TextBox.WordWrap = tsmi.Checked;
				}
			}
		}

		private void startToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.stopToolStripMenuItem.Enabled = true;
			this.StartSimulator();
		}

		private void stopToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.stopToolStripMenuItem.Enabled = false;
			this.StopSimulator();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var about = new About(this);
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

			if (Properties.Settings.Default.FontEditor == null) {
				Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);
			}

			if (Properties.Settings.Default.FontTooltips == null) {
				Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);
			}

			toolTipFont = Properties.Settings.Default.FontTooltips;
			listBoxFont = Properties.Settings.Default.FontEditor;

			this.TooltipMouse.Font = toolTipFont;
			this.TooltipKeyboard.Font = toolTipFont;
			this.TooltipListBox.Font = toolTipFont;
			this.GListBoxWindow.Font = listBoxFont;

			foreach (var form in this.Children) {
				var editForm = form as EditForm;
				if (editForm?.IsDisposed != false) {
					continue;
				}
				editForm.SetFont();
			}
		}

		private void SetupChildForms()
		{
			var listBoxFont = Properties.Settings.Default.FontEditor;
			var toolTipFont = Properties.Settings.Default.FontTooltips;

			this.TooltipMouse = new TooltipWindow(this);
			this.TooltipKeyboard = new TooltipWindow(this);
			this.TooltipListBox = new TooltipWindow(this);
			this.GListBoxWindow = new GListBoxWindow(this);

			this.SetFontsOnWindows();

			//TODO: Fix new file drop down
			//this.solutionExplorer1.parent = this;
			//this.solutionExplorer1.CreateNewFileDrowDownMenu(this.addNewFileToolStripMenuItem);
			this.solutionExplorerToolStripMenuItem.Checked = Properties.Settings.Default.ShowSolutionExplorer;
			this.ShowSolutionExplorer(this.solutionExplorerToolStripMenuItem.Checked);

			this.llDialogForms = new List<llDialogForm>();
			this.llTextBoxForms = new List<llTextBoxForm>();
			this.PermissionForms = new List<PermissionsForm>();

			this.TooltipMouse.Tag = "";
			this.TooltipKeyboard.Tag = "";
			this.TooltipListBox.Tag = "";
			this.TooltipListBox.XOffset = 200; // TODO, afhankelijk van toegepaste font
			this.GListBoxWindow.Tag = "";

			// This order equals with the KeyWordTypeEnum in KeyWords.cs
			var imageList = new ImageList();
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

			this.Move += this.LSLEditorForm_SetPosition;
			this.Resize += this.LSLEditorForm_SetPosition;

			this.GListBoxWindow.GListBox.DoubleClick += this.GListBox_DoubleClick;
			this.GListBoxWindow.GListBox.SelectedIndexChanged += this.GListBox_SelectedIndexChanged;
		}

		private void GListBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.ShowTooltipOnListBox();
			}
		}

		private void GListBox_DoubleClick(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.InsertSelectedWord();
			}
		}

		private void LSLEditorForm_SetPosition(object sender, EventArgs e)
		{
			foreach (var pf in this.PermissionForms) {
				pf.Top = this.Top + 30;
				pf.Left = this.Right - pf.Width - 5;
			}
			foreach (var df in this.llDialogForms) {
				df.Top = this.Top + 30;
				df.Left = this.Right - df.Width - 5;
			}
			foreach (var tbf in this.llTextBoxForms) {
				tbf.Left = this.Left + this.Width / 2 - tbf.Width / 2;
				tbf.Top = this.Top + this.Height / 2 - tbf.Height / 2;
			}
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.SetPosition(Screen.PrimaryScreen.WorkingArea);
			}
		}

		private void LoadProperties()
		{
			try {
				//TODO: hmmm?
				//this.tabControlExtended1.Visible = false;

				// this.panel1.Visible = false; // Simulator
				// this.panel2.Visible = false; // right pane

				//string strVersion = Properties.Settings.Default.Version;

				var size = Properties.Settings.Default.LSLEditorSize;
				var location = Properties.Settings.Default.LSLEditorLocation;
				var rect = new Rectangle(location, size);
				if (Screen.PrimaryScreen.WorkingArea.Contains(rect)) {
					if (size.Width > 100 && size.Height > 100) {
						this.Location = location;
						this.Size = size;
					}
				}

				this.browserInWindowToolStripMenuItem.Checked = Properties.Settings.Default.BrowserInWindow;

				this.WikiSepBrowserstoolStripMenuItem.Checked = Properties.Settings.Default.WikiSeperateBrowser;
			} catch (Exception exception) {
				MessageBox.Show("Error Properties: " + OopsFormatter.ApplyFormatting(exception.Message), "Oops, but continue");
			}
		}

		// TODO cleanup multiple return points
		public bool CloseAllOpenWindows()
		{
			foreach (var form in this.Children) {
				var editForm = form as EditForm;
				if (editForm?.IsDisposed != false) {
					continue;
				}
				this.ActivateMdiForm(editForm);
				if (editForm.Dirty) {
					var scriptToSave = editForm.ScriptName;
					if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName)) {
						// Expanded scripts will always be saved as LSLI's
						scriptToSave = Helpers.LSLIPathHelper.CreateCollapsedScriptName(scriptToSave);
					}

					var dialogResult = MessageBox.Show(this, @"Save """ + scriptToSave + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
					if (dialogResult == DialogResult.Cancel) {
						return false;
					}
					if (dialogResult == DialogResult.Yes) {
						// TODO: Refactor saveDialog to be a property of the form
						var saveDialog = editForm.IsScript ? this.saveScriptFilesDialog : this.saveNoteFilesDialog;
						if (!this.SaveFile(editForm, false)) {
							return false;
						}
					}

					if (dialogResult == DialogResult.No) {
						editForm.Dirty = false;
					}

					// Delete expanded file when closing
					var expandedFile = Helpers.LSLIPathHelper.CreateExpandedPathAndScriptName(editForm.FullPathName);
					if (File.Exists(expandedFile)) {
						File.Delete(expandedFile);
					}
				}
				this.CloseActiveWindow();
			}
			return true;
		}

		// TODO cleanup multiple return points
		private void LSLEditorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			try {
				Properties.Settings.Default.LSLEditorSize = this.Size;
				Properties.Settings.Default.LSLEditorLocation = this.Location;

				Properties.Settings.Default.Save();

				if (this.IsMdiContainer) {
					// this is set by any EditForm close
					e.Cancel = this.CancelClosing;
				} else {
					e.Cancel = !this.CloseAllOpenWindows();
				}

				if (!e.Cancel && this.SolutionExplorer?.IsDisposed == false) {
					this.SolutionExplorer.CloseSolution();
				}
			} catch { }
		}

		private void helpKeywordToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				var strKeyWord = editForm.TextBox.GetCurrentKeyWord(false, out var dummy);
				this.ShowHelpOnKeyWord(strKeyWord);
			}
		}

		// TODO cleanup multiple return points
		private void ShowHelpOnKeyWord(string strKeyWord)
		{
			if (Properties.Settings.Default.HelpOffline) {
				var strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Settings.Default.HelpOfflineFile);
				if (!File.Exists(strHelpFile)) {
					if (MessageBox.Show("No Offline help, use Online Wiki?", "Offline Help fails", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK) {
						return;
					}
				} else {
					if (strKeyWord?.Length == 0) {
						Help.ShowHelp(this, strHelpFile);
					} else {
						Help.ShowHelp(this, strHelpFile, HelpNavigator.KeywordIndex, strKeyWord);
					}

					return;
				}
			}

			var strUrl = Properties.Settings.Default.Help + strKeyWord;

			if (strKeyWord?.Length == 0) {
				strKeyWord = "Help";
			}

			if (Properties.Settings.Default.WikiSeperateBrowser) {
				System.Diagnostics.Process.Start(strUrl);
			} else {
				var browser = this.GetBrowser();
				browser.ShowWebBrowser(strKeyWord, strUrl);
			}
		}

		private void indexToolStripMenuItem_Click(object sender, EventArgs e)
		{
			const string strKeyWord = "HomePage";
			this.ShowHelpOnKeyWord(strKeyWord);
		}

		private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			var strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			browser.ShowWebBrowser("Check for Updates", Properties.Settings.Default.Update + "?" + strVersion);
		}

		private void browserInWindowToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.browserInWindowToolStripMenuItem.Checked = !this.browserInWindowToolStripMenuItem.Checked;
			Properties.Settings.Default.BrowserInWindow = this.browserInWindowToolStripMenuItem.Checked;
		}

		private void InitSyntaxError()
		{
			// information out of every script
			if (this.SyntaxErrors == null) {
				this.SyntaxErrors = new SyntaxError();
				this.SyntaxErrors.OnSyntaxError += this.SyntaxErrors_OnSyntaxError;
				this.SyntaxErrors.Dock = DockStyle.Fill;
			}
			this.SyntaxErrors.Clear();
		}

		private void SyntaxErrors_OnSyntaxError(object sender, SyntaxError.SyntaxErrorEventArgs e)
		{
			EditForm editForm = null;
			foreach (var form in this.Children) {
				editForm = form as EditForm;
				if (editForm?.IsDisposed != false) {
					continue;
				}

				if (editForm.FullPathName == e.FullPathName) {
					if (!editForm.Visible) {
						if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName) && this.GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName)).Visible) {
							//SetReadOnly((EditForm) GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName)), true); // Doesn't seem to work? Why?
							var LSLIForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName));
							LSLIForm.Close();
						}
						editForm.Show();
					}
					this.ActivateMdiForm(editForm);
					editForm.TextBox.Goto(e.Line, e.Char);
					editForm.Focus();
					return;
				}
			}
			editForm = this.OpenFile(e.FullPathName, e.EditFormGuid, e.IsScript);
			editForm.TextBox.Goto(e.Line, e.Char);
			editForm.Focus();
		}

		private void StartSimulator()
		{
			if (this.SyntaxCheck(true)) {
				this.StopSimulator();

				this.SimulatorConsole = new SimulatorConsole(this.SolutionExplorer, this.Children);

				this.SimulatorConsole.Show(this.dockPanel);

				//TODO: Show Simulator Console somewhere
				//this.panel1.Controls.Clear();
				//this.panel1.Controls.Add(this.SimulatorConsole);
				//this.panel1.Visible = true;
				//this.splitter1.SplitPosition = Properties.Settings.Default.SimulatorSize.Height;
			}
		}

		public void StopSimulator()
		{
			//TODO: Hide simulator? Or we could keep it like the debug output in VS
			//this.panel1.Visible = false;
			if (this.SimulatorConsole != null) {
				this.SimulatorConsole.Stop();
				this.SimulatorConsole.Dispose();
			}
			this.SimulatorConsole = null;
		}

		/// <summary>
		/// When running an LSLI script, a related expanded LSL script or LSLI readonly may be opened. These should not be ran/checked for syntax.
		/// An LSLI script should also first be expanded to an LSL script before it checks for syntax.
		/// </summary>
		/// <param name="editForm"></param>
		/// <returns></returns>
		private EditForm SelectEditFormToRun(EditForm editForm)
		{
			if (Helpers.LSLIPathHelper.IsLSLI(editForm.ScriptName) && editForm.Visible && !this.IsReadOnly(editForm)) {
				// Open and hide or select the expanded LSLI form
				var expandedForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(editForm.ScriptName));
				if (expandedForm == null) {
					// Create the LSL
					this.ExpandForm(editForm);
					expandedForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(editForm.ScriptName));
					editForm = expandedForm;
				} else {
					this.ExpandForm(editForm);
					editForm.Close();
				}
			} else if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName)) {
				// NOTE: WAAROM COLLAPSED HIJ HEM EERST? ZO VERWIJDERD HIJ DE VERANDERINGEN IN DE EXPANDED INCLUDE SECTIONS
				//CollapseForm(editForm);
				//EditForm collapsedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName));
				//ExpandForm(collapsedForm);
				var expandedForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(editForm.ScriptName));
				editForm = expandedForm;
			}
			return editForm;
		}

		private bool SyntaxCheck(bool Silent)
		{
			//TODO: What do we hide on SyntaxCheck?
			//this.panel1.Visible = false;
			this.InitSyntaxError();

			foreach (var form in this.Children) {
				var editForm = form as EditForm;
				editForm = this.SelectEditFormToRun(editForm);

				if (editForm?.IsDisposed != false || !editForm.Visible || this.IsReadOnly(editForm)) {
					continue;
				}
				if (Properties.Settings.Default.AutoSaveOnDebug && editForm.Dirty) {
					this.SaveFile(editForm, false);
				}
				editForm.SyntaxCheck();
			}

			var blnResult = false;
			if (this.SyntaxErrors.HasErrors) {
				this.SyntaxErrors.Show(this.dockPanel);
				//TODO: Show errors somewhere in an output
				//this.panel1.Controls.Clear();
				//this.panel1.Controls.Add(this.SyntaxErrors);
				//this.panel1.Visible = true;
				//this.splitter1.SplitPosition = Properties.Settings.Default.SimulatorSize.Height;
			} else {
				if (!Silent) {
					MessageBox.Show("LSL Syntax seems OK", "LSL Syntax Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
				blnResult = true;
			}
			return blnResult;
		}

		private void releaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			var strExeFileName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
			browser.ShowWebBrowser("Release notes", "res://" + strExeFileName + "/" + Properties.Settings.Default.ReleaseNotes);
		}

		private void findForm_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.FindLocation = this.findWindow.Location;
		}

		private void gotoForm_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.GotoLocation = this.GotoWindow.Location;
		}

		private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
		{
			//TODO: Splitter moved? I Think this is depricated
			//if( this.splitter1.SplitPosition>50)
			//	Properties.Settings.Default.SimulatorSize = new Size(this.splitter1.Width, this.splitter1.SplitPosition);
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.GListBoxWindow.Visible = false;
			this.TooltipMouse.Visible = false;
			this.TooltipKeyboard.Visible = false;
			this.TooltipListBox.Visible = false;
		}

		private void tabControlExtended1_MouseDown(object sender, MouseEventArgs e)
		{
			if (sender is TabControl tabControl) {
				if (e.Button == MouseButtons.Right) {
					for (var intI = 0; intI < tabControl.TabCount; intI++) {
						var rt = tabControl.GetTabRect(intI);
						if (e.X > rt.Left && e.X < rt.Right
							&& e.Y > rt.Top && e.Y < rt.Bottom) {
							this.contextMenuStrip1.Tag = intI;
							this.contextMenuStrip1.Show(tabControl, new Point(e.X, e.Y));
						}
					}
				}
			}
		}

		private bool IsInSolutionExplorer(Guid guid)
		{
			var blnResult = true;
			if (this.SolutionExplorer?.IsDisposed != false || this.SolutionExplorer.GetKey(guid) == null) {
				blnResult = false;
			}
			return blnResult;
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
			var intTabToDelete = (int)sender;
			this.CloseTab(intTabToDelete);
		}

		private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var intTabToDelete = (int)this.contextMenuStrip1.Tag;
			this.CloseTab(intTabToDelete);
		}

		private void toolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				if (this.GotoWindow?.IsDisposed != false) {
					this.GotoWindow = new GotoWindow(this);
					if (Properties.Settings.Default.GotoLocation != Point.Empty) {
						this.GotoWindow.StartPosition = FormStartPosition.Manual;
						this.GotoWindow.Location = Properties.Settings.Default.GotoLocation;
					}
					this.GotoWindow.LocationChanged += this.gotoForm_LocationChanged;
				}
				this.GotoWindow.Show(this);
			}
		}

		public void CloseActiveWindow()
		{
			if (this.IsMdiContainer) {
				var editForm = this.ActiveMdiForm as EditForm;
				if (editForm?.IsDisposed == false) {
					editForm.Close();
				}
			} else {
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
			this.CloseActiveWindow();
		}

		private void WikiSepBrowserstoolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.WikiSepBrowserstoolStripMenuItem.Checked = !this.WikiSepBrowserstoolStripMenuItem.Checked;
			Properties.Settings.Default.WikiSeperateBrowser = this.WikiSepBrowserstoolStripMenuItem.Checked;
		}

		private void LSLEditorForm_Load(object sender, EventArgs e)
		{
			this.LoadProperties();
			if (Properties.Settings.Default.CheckForUpdates) {
				this.updateApplicationForm = new UpdateApplicationForm {
					Icon = this.Icon
				};
				this.updateApplicationForm.OnUpdateAvailable += this.updateApplicationForm_OnUpdateAvailable;
				this.updateApplicationForm.CheckForUpdate(false);
			}
		}

		private void updateApplicationForm_OnUpdateAvailable(object sender, EventArgs e)
		{
			this.updateApplicationForm.ShowDialog(this);
		}

		private void commentInToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.MultiLineComment(true);
			}
		}

		private void uncommentingSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.MultiLineComment(false);
			}
		}

		private void FindandReplace(bool blnReplaceAlso)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				if (this.findWindow?.IsDisposed != false) {
					this.findWindow = new FindWindow(this);
					this.findWindow.LocationChanged += this.findForm_LocationChanged;
				}
				var rect = new Rectangle(this.Location, this.Size);
				this.findWindow.Location = rect.Contains(Properties.Settings.Default.FindLocation)
					? Properties.Settings.Default.FindLocation
					: this.Location;
				this.findWindow.ReplaceAlso = blnReplaceAlso;
				this.findWindow.KeyWord = editForm.TextBox.GetCurrentKeyWord(false, out var dummy);
				if (!this.findWindow.Visible) {
					this.findWindow.Show(this);
				}
				this.findWindow.FindFocus();
			}
		}

		private void replaceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.FindandReplace(true);
		}

		private void findToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.FindandReplace(false);
		}

		private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.findWindow?.IsDisposed == false) {
				this.findWindow.Find();
			}
		}

		public void ShowSolutionExplorer(bool blnVisible)
		{
			//TODO: We need another way to activate the Solution Explorer
			//this.panel2.Visible = blnVisible;
			if (blnVisible) {
				this.m_SolutionExplorer.Show(this.dockPanel);
			} else {
				this.m_SolutionExplorer.Hide();
			}
			this.solutionExplorerToolStripMenuItem.Checked = blnVisible;
			//this.tabControlExtended1.Refresh();
		}

		private void solutionExplorerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.solutionExplorerToolStripMenuItem.Checked = !this.solutionExplorerToolStripMenuItem.Checked;
			Properties.Settings.Default.ShowSolutionExplorer = this.solutionExplorerToolStripMenuItem.Checked;
			this.ShowSolutionExplorer(this.solutionExplorerToolStripMenuItem.Checked);
		}

		private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NewFile();
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
			this.ReadNoteFiles();
		}

		private void openScriptFilesToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.ReadScriptFiles();
		}

		private void closeFileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.CloseActiveWindow();
		}

		private void recentFileToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (e.ClickedItem is ToolStripMenuItem tsmi) {
				this.fileStripMenuItem.HideDropDown();

				var strPath = tsmi.Tag.ToString();
				if (!File.Exists(strPath)) {
					var dialogResult = MessageBox.Show("File not found. Do you want to remove it from the recent list?", "File not found", MessageBoxButtons.YesNo);
					if (dialogResult == DialogResult.Yes) {
						this.recentFileToolStripMenuItem.DropDownItems.Remove(tsmi);
						Properties.Settings.Default.RecentFileList.Remove(strPath);
					}
					return;
				}

				var strExt = Path.GetExtension(strPath);
				var blnIsScript = strExt == ".lsl" || strExt == ".lsli";
				this.OpenFile(strPath, Guid.NewGuid(), blnIsScript);
			}
		}

		private void makeBugReporttoolStripMenuItem_Click(object sender, EventArgs e)
		{
			var bugReport = new BugReport.BugReportForm(this);
			bugReport.Show(this);
		}

		private void toolStripMenuItem4_Click(object sender, EventArgs e)
		{
			var uaf = new UpdateApplicationForm {
				Icon = this.Icon
			};
			uaf.CheckForUpdate(true);
			uaf.ShowDialog(this);
		}

		private void syntaxCheckerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SyntaxCheck(false);
		}

		#region SolutionExplorer
		private void openProjectSolutionToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.openSolutionFilesDialog.ShowDialog(this) == DialogResult.OK) {
				if (File.Exists(this.openSolutionFilesDialog.FileName)) {
					if (this.CloseAllOpenWindows()) {
						this.SolutionExplorer.OpenSolution(this.openSolutionFilesDialog.FileName);
					}
				}
			}
		}

		private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var np = new NewProject(this);
			if (np.ShowDialog(this) == DialogResult.OK) {
				this.CloseAllOpenWindows();
			}
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
			if (e.ClickedItem is ToolStripMenuItem tsmi) {
				this.fileStripMenuItem.HideDropDown();

				var strPath = tsmi.Tag.ToString();
				if (!File.Exists(strPath)) {
					var dialogResult = MessageBox.Show("Project not found. Do you want to remove it from the recent list?", "Project not found", MessageBoxButtons.YesNo);
					if (dialogResult == DialogResult.Yes) {
						this.recentProjectToolStripMenuItem.DropDownItems.Remove(tsmi);
						Properties.Settings.Default.RecentProjectList.Remove(strPath);
					}
					return;
				}

				if (this.CloseAllOpenWindows()) {
					this.SolutionExplorer.OpenSolution(strPath);
				}
			}
		}

		private void LSLEditorForm_DragEnter(object sender, DragEventArgs e)
		{
			e.Effect = e.Data.GetDataPresent(DataFormats.FileDrop) ? DragDropEffects.Copy : DragDropEffects.None;
		}

		private delegate void DelegateOpenFile(string s);

		private void LSLEditorForm_DragDrop(object sender, DragEventArgs e)
		{
			try {
				var delegateOpenFile = new DelegateOpenFile(this.OpenFile);
				var allFiles = (Array)e.Data.GetData(DataFormats.FileDrop);
				if (allFiles != null) {
					for (var intI = 0; intI < allFiles.Length; intI++) {
						var strFileName = allFiles.GetValue(intI).ToString();
						this.BeginInvoke(delegateOpenFile, new object[] { strFileName });
					}

					this.Activate(); // in the case Explorer overlaps this form
				}
			} catch {
				// Error in DragDrop function (dont show messagebox, explorer is waiting
			}
		}

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var to = new Tools.ToolsOptions();
			to.PropertiesChanged += new LSLEditor.Tools.ToolsOptions.PropertiesChangedHandler(this.to_PropertiesChanged);
			to.Icon = this.Icon;
			to.ShowDialog(this);
		}

		private void to_PropertiesChanged()
		{
			this.browserInWindowToolStripMenuItem.Checked = Properties.Settings.Default.BrowserInWindow;
			this.WikiSepBrowserstoolStripMenuItem.Checked = Properties.Settings.Default.WikiSeperateBrowser;

			this.SetFontsOnWindows();
			this.InitPluginsList();
		}

		private void toolStripMenuItem5_Click(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			browser.ShowWebBrowser("Donate", Properties.Settings.Default.DonateUrl);
		}

		private void LSLint()
		{
			this.InitSyntaxError();

			var lslint = new Plugins.LSLint();

			if (lslint.SyntaxCheck(this)) {
				if (lslint.HasErrors) {
					this.SyntaxErrors.Show(this.dockPanel);
				} else {
					MessageBox.Show("LSL Syntax seems OK", "LSLint Syntax Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}
			} else {
				MessageBox.Show("LSLint:" + lslint.ExceptionMessage, "LSLint plugin", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void Particles()
		{
			var particles = new Plugins.Particles(this);
		}

		private void SvnPlugin()
		{
			if (File.Exists(Svn.Executable)) {
				Properties.Settings.Default.VersionControl = true;
				Properties.Settings.Default.VersionControlSVN = true;
				Properties.Settings.Default.SvnExe = Svn.Executable;
				MessageBox.Show("SVN is installed and can be used in the solution explorer", "SVN information", MessageBoxButtons.OK, MessageBoxIcon.Information);
			} else {
				Properties.Settings.Default.VersionControl = false;
				Properties.Settings.Default.VersionControlSVN = false;
				MessageBox.Show("SVN is NOT installed (can not find svn binary)", "SVN information", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void GenericPlugin(string strPluginName)
		{
			var generic = new Plugins.Generic(this, strPluginName);
		}

		private void PluginsHandler(object sender, EventArgs e)
		{
			//TODO: What do we hide here?
			//this.panel1.Visible = false;

			if (sender is ToolStripMenuItem tsmi) {
				switch (tsmi.Text.ToLower()) {
					case "lslint":
						this.LSLint();
						break;
					case "particles":
						this.Particles();
						break;
					case "svn (version control)":
						this.SvnPlugin();
						break;
					default:
						this.GenericPlugin(tsmi.Text);
						//MessageBox.Show("Unknown plugin", "LSLEditor plugins warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
						break;
				}
			}
		}

		private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (var form in this.Children) {
				var editForm = form as EditForm;
				if (editForm?.IsDisposed != false) {
					continue;
				}
				if (editForm.Dirty) {
					this.SaveFile(editForm, false);
				}
			}
			// save it all, also solution explorer file
			if (this.SolutionExplorer != null & !this.SolutionExplorer.IsDisposed) {
				this.SolutionExplorer.SaveSolutionFile();
			}
		}

		private void SetupFileMenu()
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				this.saveToolStripMenuItem.Text = "Save " + editForm.ScriptName;
				this.saveScriptFilesDialog.FileName = editForm.ScriptName;
				this.saveToolStripMenuItem.Enabled = editForm.Dirty;
				this.closeFileToolStripMenuItem.Enabled = true;
			} else {
				this.closeFileToolStripMenuItem.Enabled = false;
			}
		}

		private void tabControlExtended1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.SetupFileMenu();
			if (this.ActiveMdiForm is EditForm ef) {
				ef.SetFocus();
			}
		}

		private void LSLEditorForm_MdiChildActivate(object sender, EventArgs e)
		{
			this.SetupFileMenu();
		}

		private void fileToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				this.toolStripMenuItem2.Enabled =
					Helpers.LSLIPathHelper.IsLSLI(editForm.ScriptName)
				 || Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName);
			}

			this.SetupFileMenu();
		}

		private void forumStripMenuItem_Click(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			browser.ShowWebBrowser("LSLEditor Forum", Properties.Settings.Default.ForumLSLEditor);
		}

		private void notecardToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NewNotecard();
		}

		private void toolStripMenuItem7_Click(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			browser.ShowWebBrowser("Contact Form", Properties.Settings.Default.ContactUrl);
		}

		private void toolStripMenuItem8_Click(object sender, EventArgs e)
		{
			//NativeHelper.SendMyKeys.PasteTextToApp("hello", "SecondLife", null);
			if (this.ActiveMdiForm is EditForm editForm) {
				editForm.TextBox.ToClipBoard();
				NativeHelper.SendMyKeys.ClipBoardToApp("SecondLife", null);
			}
		}

		private void outlineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//old
		}

		private void outlineToolStripMenuItem_CheckedChanged(object sender, EventArgs e)
		{
			foreach (var k in this.Children) {
				var editForm = k as EditForm;
				if (editForm == null) {
					editForm.splitContainer1.Panel2Collapsed = !this.outlineToolStripMenuItem.Checked;
				}
			}
		}

		private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
		{
			var browser = this.GetBrowser();
			browser.ShowWebBrowser("LSLEditor QA", Properties.Settings.Default.qasite);
		}

		/// <summary>
		/// Gets a form based on it's form.Text property.
		/// </summary>
		/// <param name="formName"></param>
		/// <returns>Returns null if not found</returns>
		public Form GetForm(string formName)
		{
			EditForm desirableForm = null;
			for (var i = 0; i < this.Children.Length; i++) {
				var form = this.Children[i];
				if (Helpers.LSLIPathHelper.TrimStarsAndWhiteSpace(form.Text) == formName) {
					desirableForm = (EditForm)form;
				}
			}

			return desirableForm;
		}

		/// <summary>
		/// Sets the readonly property of the textbox in the form
		/// </summary>
		/// <param name="form"></param>
		/// <param name="isReadOnly"></param>
		public void SetReadOnly(EditForm form, bool isReadOnly)
		{
			foreach (Control c in form.tabControl.SelectedTab.Controls) {
				if (c.GetType() == typeof(SplitContainer)) {
					var a = (NumberedTextBox.NumberedTextBoxUC)((SplitContainer)c).ActiveControl;
					if (a != null) {
						a.TextBox.ReadOnly = isReadOnly;
					}
				}
			}
		}

		/// <summary>
		/// Gets the forms readonly property and returns it.
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public bool IsReadOnly(EditForm form)
		{
			foreach (Control c in form.tabControl.SelectedTab.Controls) {
				if (c.GetType() == typeof(SplitContainer)) {
					var a = (NumberedTextBox.NumberedTextBoxUC)((SplitContainer)c).ActiveControl;
					return a != null ? a.TextBox.ReadOnly : true;
				}
			}
			return false;
		}

		/// <summary>
		/// Expands an editform and opens it. Hides the LSLI
		/// </summary>
		/// <param name="editForm"></param>
		public void ExpandForm(EditForm editForm)
		{
			if (editForm != null && Helpers.LSLIPathHelper.IsLSLI(editForm.ScriptName)) {
				var converter = new Helpers.LSLIConverter();
				var lsl = converter.ExpandToLSL(editForm);
				var file = Helpers.LSLIPathHelper.CreateExpandedPathAndScriptName(editForm.FullPathName);
				var oldExpandedForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(editForm.ScriptName)));

				// Check if the expanded form is already open. If so, then overwrite the content of it.
				if (oldExpandedForm != null)//
				{
					oldExpandedForm.SourceCode = lsl;
					//oldExpandedForm.TabIndex = editForm.TabIndex; // TODO: Keep tabIndex when expanding/collapsing the same
					oldExpandedForm.Show();
					this.SetReadOnly(oldExpandedForm, false);
					oldExpandedForm.Dirty = editForm.Dirty;
				} else { // If not already open 
					Helpers.LSLIPathHelper.DeleteFile(file);

					using (var sw = new StreamWriter(file)) {
						sw.Write(lsl);
					}

					Helpers.LSLIPathHelper.HideFile(file);

					var expandedForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.CreateExpandedScriptName(Path.GetFileName(file)));

					expandedForm?.Close();

					this.OpenFile(file);
					var lslForm = (EditForm)this.GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(file));
					lslForm.Dirty = editForm.Dirty;
				}
				editForm.Hide();
			}
		}

		// Expand to LSL button (F11)
		private void expandToLSLToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var editForm = this.ActiveMdiForm as EditForm;
			this.ExpandForm(editForm);
		}

		public void CollapseForm(EditForm editForm)
		{
			if (editForm != null && Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName)) {
				var converter = new Helpers.LSLIConverter();

				Helpers.LSLIPathHelper.DeleteFile(editForm.FullPathName);

				var lsli = converter.CollapseToLSLIFromEditform(editForm);
				var file = Helpers.LSLIPathHelper.CreateCollapsedPathAndScriptName(editForm.FullPathName);

				// Check if the LSLI form is already open (but hidden)
				if (this.GetForm(Path.GetFileName(file)) != null) {
					var LSLIform = (EditForm)this.GetForm(Path.GetFileName(file));
					LSLIform.SourceCode = lsli;
					LSLIform.Show();
					this.SetReadOnly(LSLIform, false);

					LSLIform.Dirty = editForm.Dirty;
				} else {
					this.OpenFile(file);
					var LSLIform = (EditForm)this.GetForm(Path.GetFileName(file));
					LSLIform.SourceCode = lsli;
					LSLIform.Dirty = editForm.Dirty;
				}

				// if readonly is open, close it
				this.GetForm(Path.GetFileName(file) + Helpers.LSLIPathHelper.READONLY_TAB_EXTENSION)?.Close();

				editForm.Hide();
			}
		}

		// Collapse to LSLI button (F10)
		private void CollapseToLSLIToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var editForm = this.ActiveMdiForm as EditForm;
			this.CollapseForm(editForm);
		}

		// View LSLI button (F12)
		private void viewLSLIToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.ActiveMdiForm is EditForm editForm) {
				if (Helpers.LSLIPathHelper.IsLSLI(editForm.Text)) {
					return;
				}

				var pathOfLSLI = Helpers.LSLIPathHelper.CreateCollapsedPathAndScriptName(editForm.FullPathName);

				if (File.Exists(pathOfLSLI)) {
					var tabText = Path.GetFileName(pathOfLSLI) + Helpers.LSLIPathHelper.READONLY_TAB_EXTENSION;

					// If old LSLI readonly is open
					var OldReadOnlyLSLIform = this.GetForm(tabText);

					OldReadOnlyLSLIform?.Close();

					this.OpenFile(pathOfLSLI);

					var lsliForm = (EditForm)this.GetForm(Path.GetFileName(pathOfLSLI));
					this.SetReadOnly(lsliForm, true);
					lsliForm.AutoScroll = true;
					lsliForm.Text = tabText;
				} else {
					MessageBox.Show("Error: No related LSLI file found. \n \"" + pathOfLSLI + "\"", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		// Export button (Ctrl+E)
		private void toolStripMenuItem2_Click(object sender, EventArgs e)
		{
			StreamWriter streamWriter;
			var saveFileDialog1 = new SaveFileDialog();
			var editForm = this.ActiveMdiForm as EditForm;

			saveFileDialog1.Filter = "Secondlife script files (*.lsl)|*.lsl";
			saveFileDialog1.FileName = Helpers.LSLIPathHelper.RemoveDotInFrontOfFilename(Helpers.LSLIPathHelper.RemoveExpandedSubExtension(
				Path.GetFileNameWithoutExtension(editForm.ScriptName))) + Helpers.LSLIConverter.LSL_EXT;
			saveFileDialog1.RestoreDirectory = true;
			saveFileDialog1.Title = "Export to LSL";

			if (saveFileDialog1.ShowDialog() == DialogResult.OK) {
				if ((streamWriter = new StreamWriter(saveFileDialog1.OpenFile())) != null) {
					var lsliConverter = new Helpers.LSLIConverter();

					var showBeginEnd = Properties.Settings.Default.ShowIncludeMetaData;
					streamWriter.Write(lsliConverter.ExpandToLSL(editForm, showBeginEnd));
					streamWriter.Close();
					this.OpenFile(Path.GetFullPath(saveFileDialog1.FileName));
				}
			}
		}

		// New LSLI script button (Ctrl+M)
		private void lSLIScriptToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.NewFile(true);
		}
	}
}
