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
                try { curProc.MaxWorkingSet = curProc.MaxWorkingSet; } catch { }
            }
            base.WndProc(ref m);
        }

        public Solution.SolutionExplorer SolutionExplorer => m_SolutionExplorer;

        private void SetDefaultProperties()
        {
            if (Properties.Settings.Default.FontEditor == null)
            {
                Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);
            }

            if (Properties.Settings.Default.FontTooltips == null)
            {
                Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);
            }

            var strLocation = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (Properties.Settings.Default.ProjectLocation == "")
            {
                Properties.Settings.Default.ProjectLocation = strLocation;
            }
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

            m_SolutionExplorer = new LSLEditor.Solution.SolutionExplorer
            {
                parent = this
            };

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
                if (IsMdiContainer)
                {
                    return MdiChildren;
                }

                var children = new List<Form>();
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
                Form form = null;
                if (IsMdiContainer)
                {
                    form = ActiveMdiChild;
                }
                else
                {
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
            if (IsMdiContainer)
            {
                ActivateMdiChild(form);
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
            if (IsMdiContainer)
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
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.DoEvents();
            Application.Run(new LSLEditorForm(args));
        }

        private void Start(string[] args)
        {
            var fileFilterNotes = "Notecard files (*.txt)|*.txt|All files (*.*)|*.*";
            var fileFilterScripts = "Secondlife script files (*.lsl;*.lsli)|*.lsl;*.lsli|All files (*.*)|*.*";
            var fileFilterSolutions = "LSLEditor Solution File (*.sol)|*.sol|All Files (*.*)|*.*";

            ConfLSL = GetXmlFromResource(Properties.Settings.Default.ConfLSL);
            ConfCSharp = GetXmlFromResource(Properties.Settings.Default.ConfCSharp);

            openNoteFilesDialog.FileName = "";
            openNoteFilesDialog.Filter = fileFilterNotes;
            openNoteFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;

            saveNoteFilesDialog.FileName = "";
            saveNoteFilesDialog.Filter = fileFilterNotes;
            saveNoteFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;


            openScriptFilesDialog.FileName = "";
            openScriptFilesDialog.Filter = fileFilterScripts;
            openScriptFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;

            saveScriptFilesDialog.FileName = "";
            saveScriptFilesDialog.Filter = fileFilterScripts;
            saveScriptFilesDialog.InitialDirectory = Properties.Settings.Default.WorkingDirectory;

            openSolutionFilesDialog.FileName = "";
            openSolutionFilesDialog.Filter = fileFilterSolutions;
            openSolutionFilesDialog.InitialDirectory = Properties.Settings.Default.ProjectLocation;
            openSolutionFilesDialog.Multiselect = false;


            var version = Assembly.GetExecutingAssembly().GetName().Version;
            Text += " " + version.Major + "." + version.Minor;

            if (System.Diagnostics.Debugger.IsAttached)
            {
                Text += " (ALPHA)";
            }
            else
            {
                if (Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).Contains("beta"))
                {
                    Text += " (BETA)";
                }
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
                        SolutionExplorer.OpenSolution(args[0]);
                    }
                    else
                    {
                        var blnRun = false;
                        foreach (var strFileName in args)
                        {
                            if (strFileName == "/run")
                            {
                                blnRun = true;
                                continue;
                            }
                            var editForm = new EditForm(this);
                            editForm.LoadFile(strFileName);
                            editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
                            AddForm(editForm);
                        }
                        if (blnRun)
                        {
                            stopToolStripMenuItem.Enabled = true;
                            StartSimulator();
                        }
                    }
                }
            }
        }

        private void TextBox_OnCursorPositionChanged(object sender, SyntaxRichTextBox.CursorPositionEventArgs e)
        {
            var editForm = (EditForm)ActiveMdiForm;
            var expandedWarning = "";
            if (editForm != null && Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
            {
                expandedWarning = "Warning: Editing in included sections will be erased when collapsing/saving!";
            }

            toolStripStatusLabel1.Text = string.Format("Ln {0,-10} Col {1,-10} Ch {2,-20} Ttl {3,-10} {4,-10} {5,-10} {6}",
                e.Line, e.Column, e.Char, e.Total, e.Insert ? "INS" : "OVR", e.Caps ? "CAP" : "", expandedWarning);
        }

        private XmlDocument GetXmlFromResource(string strName)
        {
            var xml = new XmlDocument();
            var resource = Assembly.GetExecutingAssembly().GetManifestResourceStream(GetType().Namespace + "." + strName);

            if (resource != null)
            {
                xml.Load(resource);
            }
            return xml;
        }

        private void NewNotecard()
        {
            var fullPathName = Properties.Settings.Default.ExampleNameNotecard;
            var editForm = new EditForm(this)
            {
                IsScript = false,
                FullPathName = fullPathName
            };
            editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
            editForm.Dirty = false;
            AddForm(editForm);
        }

        private void NewFile(bool isLSLI = false)
        {
            var fullPathName = isLSLI ? Properties.Settings.Default.ExampleNameLSLI : Properties.Settings.Default.ExampleName;
            var editForm = new EditForm(this)
            {
                SourceCode = Helpers.GetTemplate.Source()
            };
            editForm.TextBox.FormatDocument();
            editForm.TextBox.ClearUndoStack();
            editForm.FullPathName = fullPathName;
            editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
            editForm.Dirty = false;
            AddForm(editForm);
        }

        public EditForm OpenFile(string strPath, Guid guid, bool blnIsScript)
        {
            EditForm editForm = null;

            foreach (var child in Children)
            {
                var form = child as EditForm;
                if (form == null || form.IsDisposed || form.IsNew)
                {
                    continue;
                }
                if (form.FullPathName == strPath)
                {
                    editForm = form;
                    break;
                }
            }

            if (editForm == null)
            {
                editForm = new EditForm(this);
                editForm.TextBox.OnCursorPositionChanged += new SyntaxRichTextBox.CursorPositionChangedHandler(TextBox_OnCursorPositionChanged);
                editForm.guid = guid;
                editForm.IsScript = blnIsScript;
                editForm.LoadFile(strPath);
                AddForm(editForm);
                ActivateMdiForm(editForm);
            }
            else
            {
                ActivateMdiForm(editForm);
                if (editForm.Dirty)
                {
                    var dialogResult = MessageBox.Show(@"Revert file """ + editForm.ScriptName + @""" to last saved state? Your changes will be lost!", "File has changed", MessageBoxButtons.OKCancel);
                    if (dialogResult == DialogResult.OK)
                    {
                        editForm.LoadFile(strPath);
                        editForm.TextBox.ClearUndoStack();
                        editForm.Dirty = false;
                    }
                }
            }

            UpdateRecentFileList(strPath);

            if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.Text))
            {
                editForm.Text = Helpers.LSLIPathHelper.GetExpandedTabName(editForm.Text);
            }

            return editForm;
        }

        public void OpenFile(string strPath, Guid guid)
        {
            OpenFile(strPath, guid, true);
        }

        private void OpenFile(string strFileName)
        {
            OpenFile(strFileName, Guid.NewGuid(), true);
        }

        private string ClippedPath(string strPath)
        {
            if (string.IsNullOrEmpty(strPath))
            {
                return string.Empty;
            }

            var pathClipLength = Properties.Settings.Default.PathClipLength;
            if (pathClipLength < 1)
            {
                pathClipLength = 1;
            }

            var strFullPath = Path.GetFullPath(strPath);
            var lstDirectories = new List<string>(strFullPath.Split('\\'));

            var intCount = 0;
            var intLength = strFullPath.Length - pathClipLength;
            if (intLength > 0 && lstDirectories.Count > 2)
            {
                intLength += 4;

                var index = 1;
                var intRemoveCount = 0;
                while (index < lstDirectories.Count - 1 && intCount < intLength)
                {
                    intCount += lstDirectories[index].Length + 1;
                    intRemoveCount++;
                    index++;
                }

                if (intRemoveCount > 0 && intCount >= 4)
                {
                    lstDirectories.Insert(1, "...");
                    lstDirectories.RemoveRange(2, intRemoveCount);
                }
            }

            return string.Join("\\", lstDirectories.ToArray());
        }

        private void InitRecentFileList()
        {
            if (Properties.Settings.Default.RecentFileList == null)
            {
                Properties.Settings.Default.RecentFileList = new System.Collections.Specialized.StringCollection();
            }
            var intLen = Properties.Settings.Default.RecentFileList.Count;
            for (var intI = 0; intI < intLen; intI++)
            {
                var tsmi = new ToolStripMenuItem(ClippedPath(Properties.Settings.Default.RecentFileList[intI]))
                {
                    Tag = Properties.Settings.Default.RecentFileList[intI]
                };
                recentFileToolStripMenuItem.DropDownItems.Add(tsmi);
            }
        }

        private void InitRecentProjectList()
        {
            if (Properties.Settings.Default.RecentProjectList == null)
            {
                Properties.Settings.Default.RecentProjectList = new System.Collections.Specialized.StringCollection();
            }
            var intLen = Properties.Settings.Default.RecentProjectList.Count;
            for (var intI = 0; intI < intLen; intI++)
            {
                var tsmi = new ToolStripMenuItem(ClippedPath(Properties.Settings.Default.RecentProjectList[intI]))
                {
                    Tag = Properties.Settings.Default.RecentProjectList[intI]
                };
                recentProjectToolStripMenuItem.DropDownItems.Add(tsmi);
            }
        }

        private void InitPluginsList()
        {
            // erase old plugins
            for (var intI = toolsStripMenuItem.DropDownItems.Count - 1; intI > 0; intI--)
            {
                toolsStripMenuItem.DropDownItems.RemoveAt(intI);
            }

            if (Properties.Settings.Default.Plugins != null)
            {
                ToolStripMenuItem tsmi;
                var handler = new EventHandler(PluginsHandler);
                foreach (var strPlugin in Properties.Settings.Default.Plugins)
                {
                    if (strPlugin.ToLower().Contains("lslint"))
                    {
                        tsmi = new ToolStripMenuItem(strPlugin, null, handler, Keys.F7);
                    }
                    else
                    {
                        tsmi = new ToolStripMenuItem(strPlugin, null, handler);
                    }
                    toolsStripMenuItem.DropDownItems.Add(tsmi);
                }
            }
        }

        private void UpdateRecentFileList(string strPath)
        {
            var tsmi = new ToolStripMenuItem(ClippedPath(strPath))
            {
                Tag = strPath
            };
            recentFileToolStripMenuItem.DropDownItems.Insert(0, tsmi);
            Properties.Settings.Default.RecentFileList.Insert(0, strPath);
            var intListLen = Properties.Settings.Default.RecentFileList.Count;
            for (var intI = intListLen - 1; intI > 0; intI--)
            {
                if (strPath.ToLower() == Properties.Settings.Default.RecentFileList[intI].ToLower())
                {
                    recentFileToolStripMenuItem.DropDownItems.RemoveAt(intI);
                    Properties.Settings.Default.RecentFileList.RemoveAt(intI);
                }
            }
            var intLen = Properties.Settings.Default.RecentFileMax;
            if (recentFileToolStripMenuItem.DropDownItems.Count > intLen)
            {
                recentFileToolStripMenuItem.DropDownItems.RemoveAt(intLen);
                Properties.Settings.Default.RecentFileList.RemoveAt(intLen);
            }
        }

        public void UpdateRecentProjectList(string strPath, bool AddToList)
        {
            var tsmi = new ToolStripMenuItem(ClippedPath(strPath))
            {
                Tag = strPath
            };

            if (AddToList)
            {
                recentProjectToolStripMenuItem.DropDownItems.Insert(0, tsmi);
                Properties.Settings.Default.RecentProjectList.Insert(0, strPath);
            }

            var intListLen = Properties.Settings.Default.RecentProjectList.Count;
            for (var intI = intListLen - 1; intI > 0; intI--)
            {
                if (strPath.ToLower() == Properties.Settings.Default.RecentProjectList[intI].ToLower())
                {
                    recentProjectToolStripMenuItem.DropDownItems.RemoveAt(intI);
                    Properties.Settings.Default.RecentProjectList.RemoveAt(intI);
                }
            }

            var intLen = Properties.Settings.Default.RecentProjectMax;
            if (recentProjectToolStripMenuItem.DropDownItems.Count > intLen)
            {
                recentProjectToolStripMenuItem.DropDownItems.RemoveAt(intLen);
                Properties.Settings.Default.RecentProjectList.RemoveAt(intLen);
            }

        }

        private void ReadNoteFiles()
        {
            openNoteFilesDialog.Multiselect = true;
            if (openNoteFilesDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var strFileName in openNoteFilesDialog.FileNames)
                {
                    if (File.Exists(strFileName))
                    {
                        OpenFile(strFileName, Guid.NewGuid(), false);
                    }
                }
            }
        }

        private void ReadScriptFiles()
        {
            openScriptFilesDialog.Multiselect = true;
            if (openScriptFilesDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (var strFileName in openScriptFilesDialog.FileNames)
                {
                    if (File.Exists(strFileName))
                    {
                        OpenFile(strFileName, Guid.NewGuid());
                    }
                }
            }
        }

        private void importExampleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var browser = GetBrowser();
            browser.ShowWebBrowser("Import Examples", Properties.Settings.Default.Examples);
        }

        /***
         * Return value indicate whether file was saved or not.
         */
        public bool SaveFile(EditForm editForm, bool blnSaveAs)
        {
            var dialogresult = DialogResult.OK;
            if (blnSaveAs || editForm.IsNew)
            {
                ActivateMdiForm(editForm);

                var saveDialog = editForm.IsScript ? saveScriptFilesDialog : saveNoteFilesDialog;

                // Save as LSLI when it's an expanded LSL
                if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
                {
                    saveDialog.FileName = Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName);
                }
                else
                {
                    saveDialog.FileName = editForm.ScriptName;
                }
                //saveDialog.FileName = editForm.FullPathName;
                var strExtension = Path.GetExtension(editForm.FullPathName);
                dialogresult = saveDialog.ShowDialog();
                if (dialogresult == DialogResult.OK)
                {
                    editForm.FullPathName = saveDialog.FileName;
                }
            }
            if (dialogresult == DialogResult.OK)
            {
                editForm.SaveCurrentFile();
                UpdateRecentFileList(editForm.FullPathName);
                return true;
            }
            return false;
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                SaveFile(editForm, false);
                Focus();
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                SaveFile(editForm, true);
                Focus();
            }
        }

        private Browser GetBrowser()
        {
            if (browser == null || browser.IsDisposed)
            {
                browser = new Browser(this);
                if (Properties.Settings.Default.BrowserLocation != Point.Empty)
                {
                    browser.StartPosition = FormStartPosition.Manual;
                    browser.Location = Properties.Settings.Default.BrowserLocation;
                }
                if (Properties.Settings.Default.BrowserSize != Size.Empty)
                {
                    browser.Size = Properties.Settings.Default.BrowserSize;
                }
                browser.SizeChanged += new EventHandler(browser_SizeChanged);
                browser.LocationChanged += new EventHandler(browser_LocationChanged);
                if (browserInWindowToolStripMenuItem.Checked)
                {
                    browser.MdiParent = null;
                    browser.Show();
                }
                else
                {
                    AddForm(browser);
                    //this.browser.MdiParent = this;
                }
            }
            browser.Activate();

            if (IsMdiContainer)
            {
                ActivateMdiChild(browser);
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

            return browser;
        }

        private void browser_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.BrowserLocation = browser.Location;
        }

        private void browser_SizeChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.BrowserSize = browser.Size;
        }

        #region printing
        private void pageSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Initialize the dialog's PrinterSettings property to hold user
            // defined printer settings.
            pageSetupDialog1.PageSettings = new System.Drawing.Printing.PageSettings();

            // Initialize dialog's PrinterSettings property to hold user
            // set printer settings.
            pageSetupDialog1.PrinterSettings = new System.Drawing.Printing.PrinterSettings();

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
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                printerHelp(editForm);
                printer.PrintPreviewEditForm(editForm);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                printerHelp(editForm);
                printer.PrintEditForm(editForm);
            }
        }

        private void printerHelp(EditForm editForm)
        {
            printer = new Helpers.PrinterHelper(pageSetupDialog1)
            {
                Title = editForm.FullPathName,
                SubTitle = DateTime.Now.ToString("s"),
                Footer = Text
            };
        }
        #endregion

        private void copyToClipboardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.ToClipBoard();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Check if a LSLI or expanded LSL open is, and close that one as well
            Close();
        }

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.Undo();
            }
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.Redo();
            }
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.Cut();
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.Copy();
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.Paste();
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.Delete();
            }
        }

        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.SelectAll();
            }
        }

        private void formatDocumentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.FormatDocument();
            }
        }

        private void formatSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.AutoFormatSelectedText();
            }
        }


        private void wordWrapToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                var tsmi = sender as ToolStripMenuItem;
                if (tsmi != null)
                {
                    tsmi.Checked = !tsmi.Checked;
                    editForm.TextBox.WordWrap = tsmi.Checked;
                }
            }

        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopToolStripMenuItem.Enabled = true;
            StartSimulator();
        }

        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            stopToolStripMenuItem.Enabled = false;
            StopSimulator();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var about = new About(this);
            about.ShowDialog(this);
        }

        public void OnDirtyChanged(bool blnEnableSave)
        {
            saveToolStripMenuItem.Enabled = blnEnableSave;
        }

        private void SetFontsOnWindows()
        {
            Font listBoxFont;
            Font toolTipFont;

            if (Properties.Settings.Default.FontEditor == null)
            {
                Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);
            }

            if (Properties.Settings.Default.FontTooltips == null)
            {
                Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);
            }

            toolTipFont = Properties.Settings.Default.FontTooltips;
            listBoxFont = Properties.Settings.Default.FontEditor;

            TooltipMouse.Font = toolTipFont;
            TooltipKeyboard.Font = toolTipFont;
            TooltipListBox.Font = toolTipFont;
            GListBoxWindow.Font = listBoxFont;

            foreach (var form in Children)
            {
                var editForm = form as EditForm;
                if (editForm == null || editForm.IsDisposed)
                {
                    continue;
                }
                editForm.SetFont();
            }
        }

        private void SetupChildForms()
        {
            var listBoxFont = Properties.Settings.Default.FontEditor;
            var toolTipFont = Properties.Settings.Default.FontTooltips;

            TooltipMouse = new TooltipWindow(this);
            TooltipKeyboard = new TooltipWindow(this);
            TooltipListBox = new TooltipWindow(this);
            GListBoxWindow = new GListBoxWindow(this);

            SetFontsOnWindows();

            //TODO: Fix new file drop down
            //this.solutionExplorer1.parent = this;
            //this.solutionExplorer1.CreateNewFileDrowDownMenu(this.addNewFileToolStripMenuItem);
            solutionExplorerToolStripMenuItem.Checked = Properties.Settings.Default.ShowSolutionExplorer;
            ShowSolutionExplorer(solutionExplorerToolStripMenuItem.Checked);

            llDialogForms = new List<llDialogForm>();
            llTextBoxForms = new List<llTextBoxForm>();
            PermissionForms = new List<PermissionsForm>();

            TooltipMouse.Tag = "";
            TooltipKeyboard.Tag = "";
            TooltipListBox.Tag = "";
            TooltipListBox.XOffset = 200; // TODO, afhankelijk van toegepaste font
            GListBoxWindow.Tag = "";

            // This order equals with the KeyWordTypeEnum in KeyWords.cs
            var imageList = new ImageList();
            imageList.Images.Add(new Bitmap(GetType(), "Images.Unknown.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Functions.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Events.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Constants.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Class.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Vars.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Properties.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.States.gif"));

            GListBoxWindow.GListBox.ImageList = imageList;
            // this.tvOutline.ImageList = imageList;

            Move += new EventHandler(LSLEditorForm_SetPosition);
            Resize += new EventHandler(LSLEditorForm_SetPosition);

            GListBoxWindow.GListBox.DoubleClick += new EventHandler(GListBox_DoubleClick);
            GListBoxWindow.GListBox.SelectedIndexChanged += new EventHandler(GListBox_SelectedIndexChanged);
        }

        private void GListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.ShowTooltipOnListBox();
            }
        }

        private void GListBox_DoubleClick(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.InsertSelectedWord();
            }

        }

        private void LSLEditorForm_SetPosition(object sender, EventArgs e)
        {
            foreach (var pf in PermissionForms)
            {
                pf.Top = Top + 30;
                pf.Left = Right - pf.Width - 5;
            }
            foreach (var df in llDialogForms)
            {
                df.Top = Top + 30;
                df.Left = Right - df.Width - 5;
            }
            foreach (var tbf in llTextBoxForms)
            {
                tbf.Left = Left + Width / 2 - tbf.Width / 2;
                tbf.Top = Top + Height / 2 - tbf.Height / 2;
            }
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.SetPosition(Screen.PrimaryScreen.WorkingArea);
            }
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

                var size = Properties.Settings.Default.LSLEditorSize;
                var location = Properties.Settings.Default.LSLEditorLocation;
                var rect = new Rectangle(location, size);
                if (Screen.PrimaryScreen.WorkingArea.Contains(rect))
                {
                    if (size.Width > 100 && size.Height > 100)
                    {
                        Location = location;
                        Size = size;
                    }
                }

                browserInWindowToolStripMenuItem.Checked = Properties.Settings.Default.BrowserInWindow;

                WikiSepBrowserstoolStripMenuItem.Checked = Properties.Settings.Default.WikiSeperateBrowser;

            }
            catch (Exception exception)
            {
                MessageBox.Show("Error Properties: " + OopsFormatter.ApplyFormatting(exception.Message), "Oops, but continue");
            }
        }

        // TODO cleanup multiple return points
        public bool CloseAllOpenWindows()
        {
            foreach (var form in Children)
            {
                var editForm = form as EditForm;
                if (editForm == null || editForm.IsDisposed)
                {
                    continue;
                }
                ActivateMdiForm(editForm);
                if (editForm.Dirty)
                {
                    var scriptToSave = editForm.ScriptName;
                    if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
                    {
                        // Expanded scripts will always be saved as LSLI's
                        scriptToSave = Helpers.LSLIPathHelper.CreateCollapsedScriptName(scriptToSave);
                    }

                    var dialogResult = MessageBox.Show(this, @"Save """ + scriptToSave + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
                    if (dialogResult == DialogResult.Cancel)
                    {
                        return false;
                    }
                    if (dialogResult == DialogResult.Yes)
                    {
                        // TODO: Refactor saveDialog to be a property of the form
                        var saveDialog = editForm.IsScript ? saveScriptFilesDialog : saveNoteFilesDialog;
                        if (!SaveFile(editForm, false))
                        {
                            return false;
                        }
                    }

                    if (dialogResult == DialogResult.No)
                    {
                        editForm.Dirty = false;
                    }

                    // Delete expanded file when closing
                    var expandedFile = Helpers.LSLIPathHelper.CreateExpandedPathAndScriptName(editForm.FullPathName);
                    if (File.Exists(expandedFile))
                    {
                        File.Delete(expandedFile);
                    }
                }
                CloseActiveWindow();
            }
            return true;
        }

        // TODO cleanup multiple return points
        private void LSLEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                Properties.Settings.Default.LSLEditorSize = Size;
                Properties.Settings.Default.LSLEditorLocation = Location;

                Properties.Settings.Default.Save();

                if (IsMdiContainer)
                {
                    // this is set by any EditForm close
                    e.Cancel = CancelClosing;
                }
                else
                {
                    e.Cancel = !CloseAllOpenWindows();
                }

                if (!e.Cancel && SolutionExplorer != null && !SolutionExplorer.IsDisposed)
                {
                    SolutionExplorer.CloseSolution();
                }
            }
            catch { }
        }


        private void helpKeywordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                var strKeyWord = editForm.TextBox.GetCurrentKeyWord(false, out var dummy);
                ShowHelpOnKeyWord(strKeyWord);
            }
        }

        // TODO cleanup multiple return points
        private void ShowHelpOnKeyWord(string strKeyWord)
        {
            if (Properties.Settings.Default.HelpOffline)
            {
                var strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Settings.Default.HelpOfflineFile);
                if (!File.Exists(strHelpFile))
                {
                    if (MessageBox.Show("No Offline help, use Online Wiki?", "Offline Help fails", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                    {
                        return;
                    }
                }
                else
                {
                    if (strKeyWord == "")
                    {
                        Help.ShowHelp(this, strHelpFile);
                    }
                    else
                    {
                        Help.ShowHelp(this, strHelpFile, HelpNavigator.KeywordIndex, strKeyWord);
                    }

                    return;
                }
            }

            var strUrl = Properties.Settings.Default.Help + strKeyWord;

            if (strKeyWord == "")
            {
                strKeyWord = "Help";
            }

            if (Properties.Settings.Default.WikiSeperateBrowser)
            {
                System.Diagnostics.Process.Start(strUrl);
            }
            else
            {
                var browser = GetBrowser();
                browser.ShowWebBrowser(strKeyWord, strUrl);
            }
        }

        private void indexToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var strKeyWord = "HomePage";
            ShowHelpOnKeyWord(strKeyWord);
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var browser = GetBrowser();
            var strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            browser.ShowWebBrowser("Check for Updates", Properties.Settings.Default.Update + "?" + strVersion);
        }

        private void browserInWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            browserInWindowToolStripMenuItem.Checked = !browserInWindowToolStripMenuItem.Checked;
            Properties.Settings.Default.BrowserInWindow = browserInWindowToolStripMenuItem.Checked;
        }

        private void InitSyntaxError()
        {
            // information out of every script
            if (SyntaxErrors == null)
            {
                SyntaxErrors = new SyntaxError();
                SyntaxErrors.OnSyntaxError += new SyntaxError.SyntaxErrorHandler(SyntaxErrors_OnSyntaxError);
                SyntaxErrors.Dock = DockStyle.Fill;
            }
            SyntaxErrors.Clear();
        }

        private void SyntaxErrors_OnSyntaxError(object sender, SyntaxError.SyntaxErrorEventArgs e)
        {
            EditForm editForm = null;
            foreach (var form in Children)
            {
                editForm = form as EditForm;
                if (editForm == null || editForm.IsDisposed)
                {
                    continue;
                }

                if (editForm.FullPathName == e.FullPathName)
                {
                    if (!editForm.Visible)
                    {
                        if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName) && GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName)).Visible)
                        {
                            //SetReadOnly((EditForm) GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName)), true); // Doesn't seem to work? Why?
                            var LSLIForm = (EditForm)GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName));
                            LSLIForm.Close();
                        }
                        editForm.Show();
                    }
                    ActivateMdiForm(editForm);
                    editForm.TextBox.Goto(e.Line, e.Char);
                    editForm.Focus();
                    return;
                }
            }
            editForm = OpenFile(e.FullPathName, e.EditFormGuid, e.IsScript);
            editForm.TextBox.Goto(e.Line, e.Char);
            editForm.Focus();
        }

        private void StartSimulator()
        {
            if (SyntaxCheck(true))
            {
                if (SimulatorConsole != null)
                {
                    StopSimulator();
                }

                SimulatorConsole = new SimulatorConsole(SolutionExplorer, Children);

                SimulatorConsole.Show(dockPanel);

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
            if (SimulatorConsole != null)
            {
                SimulatorConsole.Stop();
                SimulatorConsole.Dispose();
            }
            SimulatorConsole = null;
        }

        /// <summary>
        /// When running an LSLI script, a related expanded LSL script or LSLI readonly may be opened. These should not be ran/checked for syntax.
        /// An LSLI script should also first be expanded to an LSL script before it checks for syntax.
        /// </summary>
        /// <param name="editForm"></param>
        /// <returns></returns>
        private EditForm SelectEditFormToRun(EditForm editForm)
        {
            if (Helpers.LSLIPathHelper.IsLSLI(editForm.ScriptName) && editForm.Visible && !IsReadOnly(editForm))
            {
                // Open and hide or select the expanded LSLI form
                var expandedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(editForm.ScriptName));
                if (expandedForm == null)
                {
                    // Create the LSL
                    ExpandForm(editForm);
                    expandedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(editForm.ScriptName));
                    editForm = expandedForm;
                }
                else
                {
                    ExpandForm(editForm);
                    editForm.Close();
                }
            }
            else if (Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
            {
                // NOTE: WAAROM COLLAPSED HIJ HEM EERST? ZO VERWIJDERD HIJ DE VERANDERINGEN IN DE EXPANDED INCLUDE SECTIONS
                //CollapseForm(editForm);
                //EditForm collapsedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.CreateCollapsedScriptName(editForm.ScriptName));
                //ExpandForm(collapsedForm);
                var expandedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(editForm.ScriptName));
                editForm = expandedForm;
            }
            return editForm;
        }

        private bool SyntaxCheck(bool Silent)
        {
            //TODO: What do we hide on SyntaxCheck?
            //this.panel1.Visible = false;
            InitSyntaxError();

            foreach (var form in Children)
            {
                var editForm = form as EditForm;
                editForm = SelectEditFormToRun(editForm);

                if (editForm == null || editForm.IsDisposed || !editForm.Visible || IsReadOnly(editForm))
                {
                    continue;
                }
                if (Properties.Settings.Default.AutoSaveOnDebug)
                {
                    if (editForm.Dirty)
                    {
                        SaveFile(editForm, false);
                    }
                }
                editForm.SyntaxCheck();
            }

            var blnResult = false;
            if (SyntaxErrors.HasErrors)
            {
                SyntaxErrors.Show(dockPanel);
                //TODO: Show errors somewhere in an output
                //this.panel1.Controls.Clear();
                //this.panel1.Controls.Add(this.SyntaxErrors);
                //this.panel1.Visible = true;
                //this.splitter1.SplitPosition = Properties.Settings.Default.SimulatorSize.Height;
            }
            else
            {
                if (!Silent)
                {
                    MessageBox.Show("LSL Syntax seems OK", "LSL Syntax Checker", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                blnResult = true;
            }
            return blnResult;
        }

        private void releaseNotesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var browser = GetBrowser();
            var strExeFileName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
            browser.ShowWebBrowser("Release notes", "res://" + strExeFileName + "/" + Properties.Settings.Default.ReleaseNotes);

        }

        private void findForm_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.FindLocation = findWindow.Location;
        }

        private void gotoForm_LocationChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.GotoLocation = GotoWindow.Location;
        }

        private void splitter1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            //TODO: Splitter moved? I Think this is depricated
            //if( this.splitter1.SplitPosition>50)
            //	Properties.Settings.Default.SimulatorSize = new Size(this.splitter1.Width, this.splitter1.SplitPosition);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GListBoxWindow.Visible = false;
            TooltipMouse.Visible = false;
            TooltipKeyboard.Visible = false;
            TooltipListBox.Visible = false;
        }

        private void tabControlExtended1_MouseDown(object sender, MouseEventArgs e)
        {
            var tabControl = sender as TabControl;
            if (tabControl != null)
            {
                if (e.Button == MouseButtons.Right)
                {
                    for (var intI = 0; intI < tabControl.TabCount; intI++)
                    {
                        var rt = tabControl.GetTabRect(intI);
                        if (e.X > rt.Left && e.X < rt.Right
                            && e.Y > rt.Top && e.Y < rt.Bottom)
                        {
                            contextMenuStrip1.Tag = intI;
                            contextMenuStrip1.Show(tabControl, new Point(e.X, e.Y));
                        }
                    }
                }
            }
        }

        private bool IsInSolutionExplorer(Guid guid)
        {
            var blnResult = true;
            if (SolutionExplorer == null || SolutionExplorer.IsDisposed || SolutionExplorer.GetKey(guid) == null)
            {
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
            CloseTab(intTabToDelete);
        }

        private void closeTabToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var intTabToDelete = (int)contextMenuStrip1.Tag;
            CloseTab(intTabToDelete);
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
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
        }


        public void CloseActiveWindow()
        {
            if (IsMdiContainer)
            {
                var editForm = ActiveMdiForm as EditForm;
                if (editForm != null && !editForm.IsDisposed)
                {
                    editForm.Close();
                }
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
            Properties.Settings.Default.WikiSeperateBrowser = WikiSepBrowserstoolStripMenuItem.Checked;
        }

        private void LSLEditorForm_Load(object sender, EventArgs e)
        {
            LoadProperties();
            if (Properties.Settings.Default.CheckForUpdates)
            {
                updateApplicationForm = new UpdateApplicationForm
                {
                    Icon = Icon
                };
                updateApplicationForm.OnUpdateAvailable += new EventHandler(updateApplicationForm_OnUpdateAvailable);
                updateApplicationForm.CheckForUpdate(false);
            }
        }

        private void updateApplicationForm_OnUpdateAvailable(object sender, EventArgs e)
        {
            updateApplicationForm.ShowDialog(this);
        }

        private void commentInToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.MultiLineComment(true);
            }

        }

        private void uncommentingSelectedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                editForm.TextBox.MultiLineComment(false);
            }

        }

        private void FindandReplace(bool blnReplaceAlso)
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                if (findWindow == null || findWindow.IsDisposed)
                {
                    findWindow = new FindWindow(this);
                    findWindow.LocationChanged += new EventHandler(findForm_LocationChanged);
                }
                var rect = new Rectangle(Location, Size);
                if (rect.Contains(Properties.Settings.Default.FindLocation))
                {
                    findWindow.Location = Properties.Settings.Default.FindLocation;
                }
                else
                {
                    findWindow.Location = Location;
                }
                findWindow.ReplaceAlso = blnReplaceAlso;
                findWindow.KeyWord = editForm.TextBox.GetCurrentKeyWord(false, out var dummy);
                if (findWindow.Visible == false)
                {
                    findWindow.Show(this);
                }
                findWindow.FindFocus();
            }
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
            {
                findWindow.Find();
            }
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
            solutionExplorerToolStripMenuItem.Checked = blnVisible;
            //this.tabControlExtended1.Refresh();
        }

        private void solutionExplorerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            solutionExplorerToolStripMenuItem.Checked = !solutionExplorerToolStripMenuItem.Checked;
            Properties.Settings.Default.ShowSolutionExplorer = solutionExplorerToolStripMenuItem.Checked;
            ShowSolutionExplorer(solutionExplorerToolStripMenuItem.Checked);
        }

        private void newFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile();
        }

        public void SolutionItemsFileMenu(bool blnVisible)
        {
            addToolStripMenuItem.Visible = blnVisible;
            addToolStripSeparator.Visible = blnVisible;
            closeSolutiontoolStripMenuItem.Enabled = blnVisible;
            addNewObjecttoolStripMenuItem.Enabled = blnVisible;
            addNewFileToolStripMenuItem.Enabled = blnVisible;
            newProjectToolStripMenuItem.Enabled = !blnVisible;
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
            var tsmi = e.ClickedItem as ToolStripMenuItem;
            if (tsmi != null)
            {
                fileStripMenuItem.HideDropDown();

                var strPath = tsmi.Tag.ToString();
                if (!File.Exists(strPath))
                {
                    var dialogResult = MessageBox.Show("File not found. Do you want to remove it from the recent list?", "File not found", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        recentFileToolStripMenuItem.DropDownItems.Remove(tsmi);
                        Properties.Settings.Default.RecentFileList.Remove(strPath);
                    }
                    return;
                }

                var strExt = Path.GetExtension(strPath);
                var blnIsScript = strExt == ".lsl" || strExt == ".lsli";
                OpenFile(strPath, Guid.NewGuid(), blnIsScript);
            }
        }

        private void makeBugReporttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            var bugReport = new BugReport.BugReportForm(this);
            bugReport.Show(this);
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            var uaf = new UpdateApplicationForm
            {
                Icon = Icon
            };
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
            if (openSolutionFilesDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (File.Exists(openSolutionFilesDialog.FileName))
                {
                    if (CloseAllOpenWindows())
                    {
                        SolutionExplorer.OpenSolution(openSolutionFilesDialog.FileName);
                    }
                }
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var np = new NewProject(this);
            if (np.ShowDialog(this) == DialogResult.OK)
            {
                CloseAllOpenWindows();
            }

        }

        private void closeSolutiontoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolutionExplorer.CloseSolution();
        }

        private void newProjectToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SolutionExplorer.AddNewProjectAction();
        }

        private void existingProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolutionExplorer.AddExistingProject();
        }

        private void addNewObjecttoolStripMenuItem_Click(object sender, EventArgs e)
        {
            SolutionExplorer.AddNewObject();
        }
        #endregion SolutionExplorer

        private void recentProjectToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var tsmi = e.ClickedItem as ToolStripMenuItem;
            if (tsmi != null)
            {
                fileStripMenuItem.HideDropDown();

                var strPath = tsmi.Tag.ToString();
                if (!File.Exists(strPath))
                {
                    var dialogResult = MessageBox.Show("Project not found. Do you want to remove it from the recent list?", "Project not found", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        recentProjectToolStripMenuItem.DropDownItems.Remove(tsmi);
                        Properties.Settings.Default.RecentProjectList.Remove(strPath);
                    }
                    return;
                }

                if (CloseAllOpenWindows())
                {
                    SolutionExplorer.OpenSolution(strPath);
                }
            }
        }

        private void LSLEditorForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private delegate void DelegateOpenFile(string s);
        private void LSLEditorForm_DragDrop(object sender, DragEventArgs e)
        {
            try
            {
                var delegateOpenFile = new DelegateOpenFile(OpenFile);
                var allFiles = (Array)e.Data.GetData(DataFormats.FileDrop);
                if (allFiles != null)
                {
                    for (var intI = 0; intI < allFiles.Length; intI++)
                    {
                        var strFileName = allFiles.GetValue(intI).ToString();
                        BeginInvoke(delegateOpenFile, new object[] { strFileName });
                    }

                    Activate(); // in the case Explorer overlaps this form
                }
            }
            catch
            {
                // Error in DragDrop function (dont show messagebox, explorer is waiting
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var to = new Tools.ToolsOptions();
            to.PropertiesChanged += new LSLEditor.Tools.ToolsOptions.PropertiesChangedHandler(to_PropertiesChanged);
            to.Icon = Icon;
            to.ShowDialog(this);
        }

        private void to_PropertiesChanged()
        {
            browserInWindowToolStripMenuItem.Checked = Properties.Settings.Default.BrowserInWindow;
            WikiSepBrowserstoolStripMenuItem.Checked = Properties.Settings.Default.WikiSeperateBrowser;

            SetFontsOnWindows();
            InitPluginsList();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            var browser = GetBrowser();
            browser.ShowWebBrowser("Donate", Properties.Settings.Default.DonateUrl);
        }

        private void LSLint()
        {
            InitSyntaxError();

            var lslint = new Plugins.LSLint();

            if (lslint.SyntaxCheck(this))
            {
                if (lslint.HasErrors)
                {
                    SyntaxErrors.Show(dockPanel);
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
            var particles = new Plugins.Particles(this);
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
            var generic = new Plugins.Generic(this, strPluginName);
        }


        private void PluginsHandler(object sender, EventArgs e)
        {
            //TODO: What do we hide here?
            //this.panel1.Visible = false;

            var tsmi = sender as ToolStripMenuItem;
            if (tsmi != null)
            {
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
        }

        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var form in Children)
            {
                var editForm = form as EditForm;
                if (editForm == null || editForm.IsDisposed)
                {
                    continue;
                }
                if (editForm.Dirty)
                {
                    SaveFile(editForm, false);
                }
            }
            // save it all, also solution explorer file
            if (SolutionExplorer != null & !SolutionExplorer.IsDisposed)
            {
                SolutionExplorer.SaveSolutionFile();
            }
        }

        private void SetupFileMenu()
        {
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
                saveToolStripMenuItem.Text = "Save " + editForm.ScriptName;
                saveScriptFilesDialog.FileName = editForm.ScriptName;
                saveToolStripMenuItem.Enabled = editForm.Dirty;
                closeFileToolStripMenuItem.Enabled = true;
            }
            else
            {
                closeFileToolStripMenuItem.Enabled = false;
            }
        }

        private void tabControlExtended1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetupFileMenu();
            var ef = ActiveMdiForm as EditForm;
            if (ef != null)
            {
                ef.SetFocus();
            }
        }

        private void LSLEditorForm_MdiChildActivate(object sender, EventArgs e)
        {
            SetupFileMenu();
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;

            if (editForm != null)
            {
                if (Helpers.LSLIPathHelper.IsLSLI(editForm.ScriptName) || Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
                {
                    toolStripMenuItem2.Enabled = true;
                }
                else
                {
                    toolStripMenuItem2.Enabled = false;
                }
            }

            SetupFileMenu();
        }

        private void forumStripMenuItem_Click(object sender, EventArgs e)
        {
            var browser = GetBrowser();
            browser.ShowWebBrowser("LSLEditor Forum", Properties.Settings.Default.ForumLSLEditor);
        }

        private void notecardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewNotecard();
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            var browser = GetBrowser();
            browser.ShowWebBrowser("Contact Form", Properties.Settings.Default.ContactUrl);
        }

        private void toolStripMenuItem8_Click(object sender, EventArgs e)
        {
            //NativeHelper.SendMyKeys.PasteTextToApp("hello", "SecondLife", null);
            var editForm = ActiveMdiForm as EditForm;
            if (editForm != null)
            {
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
            foreach (var k in Children)
            {
                var editForm = k as EditForm;
                if (editForm == null)
                {
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
        }

        private void toolStripMenuItem9_Click_1(object sender, EventArgs e)
        {
            var browser = GetBrowser();
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
            for (var i = 0; i < Children.Length; i++)
            {
                var form = Children[i];
                if (Helpers.LSLIPathHelper.TrimStarsAndWhiteSpace(form.Text) == formName)
                {
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
            foreach (Control c in form.tabControl.SelectedTab.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    var a = (NumberedTextBox.NumberedTextBoxUC)((SplitContainer)c).ActiveControl;
                    if (a != null)
                    {
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
            foreach (Control c in form.tabControl.SelectedTab.Controls)
            {
                if (c.GetType() == typeof(SplitContainer))
                {
                    var a = (NumberedTextBox.NumberedTextBoxUC)((SplitContainer)c).ActiveControl;
                    if (a != null)
                    {
                        return a.TextBox.ReadOnly;
                    }
                    else
                    {
                        return true;
                    }
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
            if (editForm != null && Helpers.LSLIPathHelper.IsLSLI(editForm.ScriptName))
            {
                var converter = new Helpers.LSLIConverter();
                var lsl = converter.ExpandToLSL(editForm);
                var file = Helpers.LSLIPathHelper.CreateExpandedPathAndScriptName(editForm.FullPathName);
                var oldExpandedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(Helpers.LSLIPathHelper.CreateExpandedScriptName(editForm.ScriptName)));

                // Check if the expanded form is already open. If so, then overwrite the content of it.
                if (oldExpandedForm != null)//
                {
                    oldExpandedForm.SourceCode = lsl;
                    //oldExpandedForm.TabIndex = editForm.TabIndex; // TODO: Keep tabIndex when expanding/collapsing the same
                    oldExpandedForm.Show();
                    SetReadOnly(oldExpandedForm, false);
                    oldExpandedForm.Dirty = editForm.Dirty;
                }
                else
                { // If not already open 
                    Helpers.LSLIPathHelper.DeleteFile(file);

                    using (var sw = new StreamWriter(file))
                    {
                        sw.Write(lsl);
                    }

                    Helpers.LSLIPathHelper.HideFile(file);

                    var expandedForm = (EditForm)GetForm(Helpers.LSLIPathHelper.CreateExpandedScriptName(Path.GetFileName(file)));

                    if (expandedForm != null)
                    {
                        expandedForm.Close();
                    }

                    OpenFile(file);
                    var lslForm = (EditForm)GetForm(Helpers.LSLIPathHelper.GetExpandedTabName(file));
                    lslForm.Dirty = editForm.Dirty;
                }
                editForm.Hide();
            }
        }

        // Expand to LSL button (F11)
        private void expandToLSLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            ExpandForm(editForm);
        }

        public void CollapseForm(EditForm editForm)
        {
            if (editForm != null && Helpers.LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
            {
                var converter = new Helpers.LSLIConverter();

                Helpers.LSLIPathHelper.DeleteFile(editForm.FullPathName);

                var lsli = converter.CollapseToLSLIFromEditform(editForm);
                var file = Helpers.LSLIPathHelper.CreateCollapsedPathAndScriptName(editForm.FullPathName);

                // Check if the LSLI form is already open (but hidden)
                if (GetForm(Path.GetFileName(file)) != null)
                {
                    var LSLIform = (EditForm)GetForm(Path.GetFileName(file));
                    LSLIform.SourceCode = lsli;
                    LSLIform.Show();
                    SetReadOnly(LSLIform, false);

                    LSLIform.Dirty = editForm.Dirty;
                }
                else
                {
                    OpenFile(file);
                    var LSLIform = (EditForm)GetForm(Path.GetFileName(file));
                    LSLIform.SourceCode = lsli;
                    LSLIform.Dirty = editForm.Dirty;
                }

                if (GetForm(Path.GetFileName(file) + Helpers.LSLIPathHelper.READONLY_TAB_EXTENSION) != null) // if readonly is open, close it
                {
                    GetForm(Path.GetFileName(file) + Helpers.LSLIPathHelper.READONLY_TAB_EXTENSION).Close();
                }
                editForm.Hide();
            }
        }

        // Collapse to LSLI button (F10)
        private void CollapseToLSLIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;
            CollapseForm(editForm);
        }

        // View LSLI button (F12)
        private void viewLSLIToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var editForm = ActiveMdiForm as EditForm;

            if (editForm != null)
            {
                if (Helpers.LSLIPathHelper.IsLSLI(editForm.Text))
                {
                    return;
                }

                var pathOfLSLI = Helpers.LSLIPathHelper.CreateCollapsedPathAndScriptName(editForm.FullPathName);

                if (File.Exists(pathOfLSLI))
                {
                    var tabText = Path.GetFileName(pathOfLSLI) + Helpers.LSLIPathHelper.READONLY_TAB_EXTENSION;

                    // If old LSLI readonly is open
                    var OldReadOnlyLSLIform = GetForm(tabText);

                    if (OldReadOnlyLSLIform != null)
                    {
                        OldReadOnlyLSLIform.Close();
                    }

                    OpenFile(pathOfLSLI);

                    var lsliForm = (EditForm)GetForm(Path.GetFileName(pathOfLSLI));
                    SetReadOnly(lsliForm, true);
                    lsliForm.AutoScroll = true;
                    lsliForm.Text = tabText;
                }
                else
                {
                    MessageBox.Show("Error: No related LSLI file found. \n \"" + pathOfLSLI + "\"", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Export button (Ctrl+E)
        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            StreamWriter streamWriter;
            var saveFileDialog1 = new SaveFileDialog();
            var editForm = ActiveMdiForm as EditForm;

            saveFileDialog1.Filter = "Secondlife script files (*.lsl)|*.lsl";
            saveFileDialog1.FileName = Helpers.LSLIPathHelper.RemoveDotInFrontOfFilename(Helpers.LSLIPathHelper.RemoveExpandedSubExtension(
                Path.GetFileNameWithoutExtension(editForm.ScriptName))) + Helpers.LSLIConverter.LSL_EXT;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.Title = "Export to LSL";

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if ((streamWriter = new StreamWriter(saveFileDialog1.OpenFile())) != null)
                {
                    var lsliConverter = new Helpers.LSLIConverter();

                    var showBeginEnd = Properties.Settings.Default.ShowIncludeMetaData;
                    streamWriter.Write(lsliConverter.ExpandToLSL(editForm, showBeginEnd));
                    streamWriter.Close();
                    OpenFile(Path.GetFullPath(saveFileDialog1.FileName));
                }
            }
        }

        // New LSLI script button (Ctrl+M)
        private void lSLIScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewFile(true);
        }
    }
}
