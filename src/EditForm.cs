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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using LSLEditor.Docking;
using LSLEditor.Helpers;

namespace LSLEditor
{
    public partial class EditForm : DockContent
    {
        public RuntimeConsole runtime;

        public List<string> verboseQueue = new List<string>();

        private bool m_IsNew;
        private string m_FullPathName;
        private Guid m_Guid;
        // private bool sOutline = true;
        public LSLEditorForm parent;
        public Encoding encodedAs = null;

        private const int WM_NCACTIVATE = 0x0086;
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_NCACTIVATE)
            {
                if (m.LParam != IntPtr.Zero)
                {
                    m.WParam = new IntPtr(1);
                }
                else
                {
                    numberedTextBoxUC1.TextBox.MakeAllInvis();
                }
            }
            try { base.WndProc(ref m); } catch { }
        }

        public SyntaxRichTextBox TextBox => numberedTextBoxUC1.TextBox;

        public XmlDocument ConfLSL => parent.ConfLSL;

        public XmlDocument ConfCSharp => parent.ConfCSharp;

        public Guid guid
        {
            get => m_Guid;
            set => m_Guid = value;
        }

        public bool IsScript
        {
            get => TextBox.ToolTipping;
            set
            {
                if (value)
                {
                    tabPage1.Text = "Script";
                }
                else
                {
                    tabPage1.Text = "Text";
                }
                TextBox.ToolTipping = value;
            }
        }

        public EditForm(LSLEditorForm lslEditorForm)
        {
            InitializeComponent();

            guid = new Guid();

            components = new System.ComponentModel.Container();

            Icon = lslEditorForm.Icon;
            parent = lslEditorForm;
            numberedTextBoxUC1.TextBox.setEditform(this);
            numberedTextBoxUC1.TextBox.Init(parent, parent.ConfLSL);
            numberedTextBoxUC1.TextBox.OnDirtyChanged += new IsDirtyHandler(TextBox_OnDirtyChanged);

            Move += new EventHandler(EditForm_Position);
            Resize += new EventHandler(EditForm_Position);

            Layout += new LayoutEventHandler(EditForm_Layout);
            var imageList = new ImageList();
            imageList.Images.Add(new Bitmap(GetType(), "Images.Unknown.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Functions.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Events.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Constants.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Class.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Vars.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.Properties.gif"));
            imageList.Images.Add(new Bitmap(GetType(), "Images.States.gif"));

            tvOutline.ImageList = imageList;
            if (lslEditorForm.outlineToolStripMenuItem.Checked)
            {
                splitContainer1.Panel2Collapsed = false;
            }
            else
            {
                splitContainer1.Panel2Collapsed = true;
            }
            SetFont();
        }

        public void SetFont()
        {
            numberedTextBoxUC1.Font = Properties.Settings.Default.FontEditor;
        }

        private void EditForm_Layout(object sender, LayoutEventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                numberedTextBoxUC1.TextBox.MakeAllInvis();
            }
        }

        private void EditForm_Position(object sender, EventArgs e)
        {
            //this.numberedTextBoxUC1.TextBox.SetPosition(this.MdiParent.RectangleToScreen(this.MdiParent.ClientRectangle));
        }

        private void TextBox_OnDirtyChanged(object sender, EventArgs e)
        {
            if (parent.IsReadOnly(this))
            {
                Dirty = false;
                return;
            }
            if (Text == null || ScriptName == null)
            {
                Text = ScriptName;
            }
            if (LSLIPathHelper.GetExpandedTabName(ScriptName) == Text)
            {
                if (numberedTextBoxUC1.TextBox.Dirty)
                {
                    Text = Text.Trim() + "*  ";

                }
            }
            else
            {
                Text = ScriptName;
                if (numberedTextBoxUC1.TextBox.Dirty)
                {
                    Text = Text.Trim() + "*  ";
                }
                else
                {
                    Text = Text.Trim() + "   ";
                }
            }

            var tabPage = Tag as TabPage;
            if (tabPage != null)
            {
                tabPage.Text = Text;
            }

            parent.OnDirtyChanged(numberedTextBoxUC1.TextBox.Dirty);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        public string FullPathName
        {
            get => m_FullPathName;
            set
            {
                m_FullPathName = value;
                var strDirectory = Path.GetDirectoryName(m_FullPathName);
                if (Directory.Exists(strDirectory))
                {
                    Properties.Settings.Default.WorkingDirectory = strDirectory;
                }
                else
                {
                    if (!Directory.Exists(Properties.Settings.Default.WorkingDirectory))
                    {
                        Properties.Settings.Default.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    }
                    m_IsNew = true;
                    m_FullPathName = Path.Combine(Properties.Settings.Default.WorkingDirectory, m_FullPathName);
                }
                Text = ScriptName;
                var tabPage = Tag as TabPage;
                if (tabPage != null)
                {
                    tabPage.Text = Text + "   ";
                }
            }
        }

        public bool IsNew => m_IsNew;

        public string ScriptName => Path.GetFileName(m_FullPathName);

        public string ProjectName => parent.SolutionExplorer.GetProjectName(guid);

        public string SourceCode
        {
            get => numberedTextBoxUC1.TextBox.Text;
            set => numberedTextBoxUC1.TextBox.Text = value;
        }

        private int PercentageIndentTab()
        {
            int intResult;
            var intSpaces = 0;
            var intTabs = 0;
            var sr = new StringReader(TextBox.Text);
            while (true)
            {
                var strLine = sr.ReadLine();
                if (strLine == null)
                {
                    break;
                }

                if (strLine.Length == 0)
                {
                    continue;
                }

                if (strLine[0] == ' ')
                {
                    intSpaces++;
                }
                else if (strLine[0] == '\t')
                {
                    intTabs++;
                }
            }
            if (intTabs == 0 && intSpaces == 0)
            {
                intResult = 50;
            }
            else
            {
                intResult = (int)Math.Round((100.0 * intTabs) / (intTabs + intSpaces));
            }
            return intResult;
        }

        public void LoadFile(string strPath)
        {
            if (strPath.StartsWith("http://"))
            {
                FullPathName = Path.GetFileName(strPath);
            }
            else
            {
                FullPathName = strPath;
            }
            encodedAs = numberedTextBoxUC1.TextBox.LoadFile(strPath);

            if (IsScript)
            {

                if (Properties.Settings.Default.IndentAutoCorrect)
                {
                    TextBox.FormatDocument();
                    TextBox.ClearUndoStack();
                }
                else
                {
                    if (Properties.Settings.Default.IndentWarning)
                    {
                        if ((PercentageIndentTab() > 50 && Properties.Settings.Default.SL4SpacesIndent) ||
                            (PercentageIndentTab() < 50 && !Properties.Settings.Default.SL4SpacesIndent))
                        {
                            if (MessageBox.Show("Indent scheme differs from settings\nDo you want to correct it?\nIt can also be corrected by pressing Ctrl-D or turn on Autocorrection (tools menu)", "Indent Warning!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                            {
                                TextBox.FormatDocument();
                                //this.TextBox.ClearUndoStack();
                            }
                        }
                    }
                }
            }
        }

        public void SaveCurrentFile(string strPath)
        {
            // Check if this is an expanded.lsl
            if (LSLIPathHelper.IsExpandedLSL(strPath))
            {
                var LSLIfilePath = LSLIPathHelper.CreateCollapsedPathAndScriptName(strPath);
                // Check if an LSLI version of this script exists
                if (File.Exists(LSLIfilePath))
                {
                    // Save the LSLI file as well
                    File.WriteAllText(LSLIfilePath, LSLIConverter.CollapseToLSLI(numberedTextBoxUC1.TextBox.Text));
                    EditForm form = null;

                    // If it's currently open, then refresh it
                    for (var i = 0; i < Application.OpenForms.Count; i++)
                    {
                        var openForm = Application.OpenForms[i];
                        var filename = LSLIPathHelper.TrimStarsAndWhiteSpace(openForm.Text);
                        if (filename == Path.GetFileName(LSLIfilePath))
                        {
                            form = (EditForm)openForm;
                        }
                    }

                    if (form != null && form.Enabled)
                    {
                        parent.OpenFile(LSLIfilePath, Guid.NewGuid(), true);
                        form.Close();
                    }
                }
                numberedTextBoxUC1.TextBox.Dirty = false;
                Text = LSLIPathHelper.GetExpandedTabName(strPath);
            }
            else
            {
                FullPathName = strPath;
                var encodeAs = encodedAs;
                if (IsScript && encodeAs == null)
                {
                    switch (Properties.Settings.Default.OutputFormat)
                    {
                        case "UTF8":
                            encodeAs = Encoding.UTF8;
                            break;
                        case "Unicode":
                            encodeAs = Encoding.Unicode;
                            break;
                        case "BigEndianUnicode":
                            encodeAs = Encoding.BigEndianUnicode;
                            break;
                        default:
                            encodeAs = Encoding.Default;
                            break;
                    }
                }
                else if (encodeAs == null)
                {
                    encodeAs = Encoding.UTF8;
                }

                numberedTextBoxUC1.TextBox.SaveCurrentFile(strPath, encodeAs);
                encodedAs = encodeAs;
            }
            m_IsNew = false;
        }

        public void SaveCurrentFile()
        {
            SaveCurrentFile(FullPathName);
        }

        public bool Dirty
        {
            get => numberedTextBoxUC1.TextBox.Dirty;
            set => numberedTextBoxUC1.TextBox.Dirty = value;
        }

        public TabControl tabControl => tabControl1;

        public void SetFocus()
        {
            numberedTextBoxUC1.TextBox.Focus();
        }

        public void StopCompiler()
        {
            numberedTextBoxUC1.TextBox.MakeAllInvis();

            if (runtime != null)
            {
                components.Remove(runtime);
                if (!runtime.IsDisposed)
                {
                    runtime.Dispose();
                }
                runtime = null;
            }

            for (var intI = tabControl1.TabPages.Count - 1; intI > 0; intI--)
            {
                tabControl1.TabPages.RemoveAt(intI);
            }
        }

        public SecondLifeHost.SecondLifeHostChatHandler ChatHandler;
        public SecondLifeHost.SecondLifeHostMessageLinkedHandler MessageLinkedHandler;

        public bool StartCompiler()
        {
            var blnResult = false;
            //if (this.disableCompilesyntaxCheckToolStripMenuItem.Checked)
            //	return false;

            if (IsScript)
            {
                StopCompiler();

                if (parent != null)
                {
                    runtime = new RuntimeConsole(parent);

                    // for disposing
                    components.Add(runtime);

                    foreach (var message in verboseQueue)
                    {
                        runtime.VerboseConsole(message);

                        if (message.StartsWith("Error: "))
                        {
                            StopCompiler();
                            tabControl1.SelectedIndex = 0;
                            verboseQueue = new List<string>();
                            return false;
                        }
                    }

                    if (!runtime.Compile(this))
                    {
                        tabControl1.SelectedIndex = 0;
                        return false;
                    }


                    var tabPage = new TabPage("Debug");
                    tabPage.Controls.Add(runtime);
                    tabControl1.TabPages.Add(tabPage);
                    tabControl1.SelectedIndex = 1;
                    blnResult = true;
                }
            }
            return blnResult;
        }

        public bool SyntaxCheck()
        {
            var blnResult = false;
            //if (this.disableCompilesyntaxCheckToolStripMenuItem.Checked)
            //	return false;

            if (IsScript)
            {
                var lsl = SourceCode;

                // If it is LSLI, it needs to import scripts first, before it recognizes imported functions
                if (LSLIPathHelper.IsLSLI(FullPathName))
                {
                    var converter = new LSLIConverter();
                    lsl = converter.ExpandToLSL(this);
                }

                var translator = new LSL2CSharp(ConfLSL);
                var strCSharp = translator.Parse(lsl);

                if (System.Diagnostics.Debugger.IsAttached)
                {
                    for (var intI = tabControl1.TabPages.Count - 1; intI > 0; intI--)
                    {
                        tabControl1.TabPages.RemoveAt(intI);
                    }

                    // TODO
                    var tabPage = new TabPage("C#");
                    var numberedTextBoxUC1 = new NumberedTextBox.NumberedTextBoxUC();
                    numberedTextBoxUC1.TextBox.Init(null, ConfCSharp);
                    numberedTextBoxUC1.TextBox.Text = strCSharp;
                    numberedTextBoxUC1.TextBox.ReadOnly = true;
                    numberedTextBoxUC1.Dock = DockStyle.Fill;
                    tabPage.Controls.Add(numberedTextBoxUC1);
                    tabControl.TabPages.Add(tabPage);
                }
                blnResult = (null != CompilerHelper.CompileCSharp(this, strCSharp));
            }
            return blnResult;
        }

        public int Find(string strSearch, int intStart, int intEnd, RichTextBoxFinds options)
        {
            intStart = numberedTextBoxUC1.TextBox.Find(strSearch, intStart, intEnd, options);
            numberedTextBoxUC1.TextBox.Focus();
            return intStart;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            numberedTextBoxUC1.TextBox.MakeAllInvis();
        }

        private void EditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            parent.CancelClosing = false;
            parent.ActivateMdiForm(this);
            if (Dirty)
            {
                var scriptToSave = ScriptName;
                if (LSLIPathHelper.IsExpandedLSL(ScriptName))
                {
                    // Expanded scripts will always be saved as LSLI's
                    scriptToSave = LSLIPathHelper.CreateCollapsedScriptName(scriptToSave);
                }

                var dialogResult = MessageBox.Show(this, @"Save """ + scriptToSave + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
                if (dialogResult == DialogResult.Yes)
                {
                    e.Cancel = !parent.SaveFile(this, false);
                }
                else
                {
                    e.Cancel = (dialogResult == DialogResult.Cancel);
                }
            }

            if (!e.Cancel)
            {
                // Close related readonly's if this is an expanded script
                if (LSLIPathHelper.IsExpandedLSL(ScriptName))
                {
                    // Check if a LSLI readonly is open
                    var readOnlyLSLI = (EditForm)parent.GetForm(Path.GetFileName(LSLIPathHelper.GetReadOnlyTabName(ScriptName)));

                    if (readOnlyLSLI != null)
                    {
                        readOnlyLSLI.Close();
                    }
                }

                if (!parent.IsReadOnly(this)) // If this is not a readonly (LSLI)
                {
                    // Delete expanded file when closing
                    var expandedFile = LSLIPathHelper.CreateExpandedPathAndScriptName(FullPathName);
                    var expandedForm = (EditForm)parent.GetForm(LSLIPathHelper.GetExpandedTabName(Path.GetFileName(expandedFile)));

                    if (expandedForm != null && !LSLIPathHelper.IsExpandedLSL(ScriptName))
                    {
                        expandedForm.Close();
                    }

                    if (File.Exists(expandedFile))
                    {
                        File.Delete(expandedFile);
                    }
                }
            }
            parent.CancelClosing = e.Cancel;
        }

        private void disableCompilesyntaxCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.disableCompilesyntaxCheckToolStripMenuItem.Checked = !this.disableCompilesyntaxCheckToolStripMenuItem.Checked;
        }

        private void tvOutline_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            parent.BeginInvoke(new TreeNodeMouseClickEventHandler(
                delegate (object sender2, TreeNodeMouseClickEventArgs e2)
                {
                    if (e.Node.Tag is Helpers.OutlineHelper)
                    {
                        var ohOutline = (Helpers.OutlineHelper)e.Node.Tag;
                        if (ohOutline.line < TextBox.Lines.Length)
                        {
                            //editForm.Focus();
                            //editForm.TextBox.Select();
                            //editForm.TextBox.Goto(ohOutline.line + 1);

                            //TextBox.Focus();
                            TextBox.Select();
                            TextBox.SelectionStart = TextBox.GetFirstCharIndexFromLine(ohOutline.line);


                        }
                    }
                }), sender, e);
        }

        private void tvOutline_AfterSelect(object sender, TreeViewEventArgs e)
        {

            //this.TextBox.Select
        }

        private void splitContainer1_Click(object sender, EventArgs e)
        {

        }

        private void tvOutline_VisibleChanged(object sender, EventArgs e)
        {
            tvOutline.ExpandAll();
        }
    }
}
