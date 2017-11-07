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
using System.IO;
using System.Xml;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using LSLEditor.Docking;
using LSLEditor.Helpers;
using System.Collections.Generic;

namespace LSLEditor
{
	public partial class EditForm : DockContent
	{
		public RuntimeConsole runtime;

        public List<string> verboseQueue = new List<string>();

        private string m_FullPathName;
		private Guid m_Guid;
		// private bool sOutline = true;
		public LSLEditorForm parent;
		public Encoding encodedAs = null;

		private const int WM_NCACTIVATE = 0x0086;
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_NCACTIVATE) {
				if (m.LParam != IntPtr.Zero) {
					m.WParam = new IntPtr(1);
				} else {
					this.numberedTextBoxUC1.TextBox.MakeAllInvis();
				}
			}
			try { base.WndProc(ref m); } catch { }
		}

		public SyntaxRichTextBox TextBox
		{
			get
			{
				return this.numberedTextBoxUC1.TextBox;
			}
		}

		public XmlDocument ConfLSL
		{
			get
			{
				return this.parent.ConfLSL;
			}
		}

		public XmlDocument ConfCSharp
		{
			get
			{
				return this.parent.ConfCSharp;
			}
		}

		public Guid guid
		{
			get
			{
				return m_Guid;
			}
			set
			{
				this.m_Guid = value;
			}
		}

		public bool IsScript
		{
			get
			{
				return this.TextBox.ToolTipping;
			}
			set
			{
				if (value) {
					this.tabPage1.Text = "Script";
				} else {
					this.tabPage1.Text = "Text";
				}
				this.TextBox.ToolTipping = value;
			}
		}

		public EditForm(LSLEditorForm lslEditorForm)
		{
			InitializeComponent();

			this.guid = new Guid();

			this.components = new System.ComponentModel.Container();

			this.Icon = lslEditorForm.Icon;
			this.parent = lslEditorForm;
			this.numberedTextBoxUC1.TextBox.setEditform(this);
			this.numberedTextBoxUC1.TextBox.Init(this.parent, this.parent.ConfLSL);
			this.numberedTextBoxUC1.TextBox.OnDirtyChanged += new IsDirtyHandler(TextBox_OnDirtyChanged);

			this.Move += new EventHandler(EditForm_Position);
			this.Resize += new EventHandler(EditForm_Position);

			this.Layout += new LayoutEventHandler(EditForm_Layout);
			ImageList imageList = new ImageList();
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Unknown.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Functions.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Events.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Constants.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Class.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Vars.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.Properties.gif"));
			imageList.Images.Add(new Bitmap(this.GetType(), "Images.States.gif"));

			this.tvOutline.ImageList = imageList;
			if (lslEditorForm.outlineToolStripMenuItem.Checked) {
				splitContainer1.Panel2Collapsed = false;
			} else {
				splitContainer1.Panel2Collapsed = true;
			}
			SetFont();
		}

		public void SetFont()
		{
			this.numberedTextBoxUC1.Font = Properties.Settings.Default.FontEditor;
		}

		void EditForm_Layout(object sender, LayoutEventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized) {
				this.numberedTextBoxUC1.TextBox.MakeAllInvis();
			}
		}

		void EditForm_Position(object sender, EventArgs e)
		{
			//this.numberedTextBoxUC1.TextBox.SetPosition(this.MdiParent.RectangleToScreen(this.MdiParent.ClientRectangle));
		}

		void TextBox_OnDirtyChanged(object sender, EventArgs e)
		{
            if(parent.IsReadOnly(this))
            {
                Dirty = false;
                return;
            }
            if(this.Text == null || this.ScriptName == null)
            {
                this.Text = this.ScriptName;
            }
            if (LSLIPathHelper.GetExpandedTabName(this.ScriptName) == this.Text)
            {
                if (this.numberedTextBoxUC1.TextBox.Dirty)
                {
                    this.Text = this.Text.Trim() + "*  ";

                }
            }
            else
            {
                this.Text = this.ScriptName;
			    if (this.numberedTextBoxUC1.TextBox.Dirty) {
				    this.Text = this.Text.Trim() + "*  ";
			    } else {
				    this.Text = this.Text.Trim() + "   ";
			    }
            }

            TabPage tabPage = this.Tag as TabPage;
			if (tabPage != null) {
				tabPage.Text = this.Text;
			}

			this.parent.OnDirtyChanged(this.numberedTextBoxUC1.TextBox.Dirty);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
            this.Close();
		}

		public string FullPathName
		{
			get
			{
				return this.m_FullPathName;
			}
			set
			{
				this.m_FullPathName = value;
				string strDirectory = Path.GetDirectoryName(this.m_FullPathName);
				if (Directory.Exists(strDirectory)) {
					Properties.Settings.Default.WorkingDirectory = strDirectory;
				} else {
					if (!Directory.Exists(Properties.Settings.Default.WorkingDirectory)) {
						Properties.Settings.Default.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
					}
					this.m_FullPathName = Path.Combine(Properties.Settings.Default.WorkingDirectory, this.m_FullPathName);
				}
				this.Text = this.ScriptName;
				TabPage tabPage = this.Tag as TabPage;
				if (tabPage != null) {
					tabPage.Text = this.Text + "   ";
				}
			}
		}

		public string ScriptName
		{
			get
			{
				return Path.GetFileName(this.m_FullPathName);
			}
		}

		public string ProjectName
		{
			get
			{
				return parent.SolutionExplorer.GetProjectName(this.guid);
			}
		}

		public string SourceCode
		{
			get
			{
				return this.numberedTextBoxUC1.TextBox.Text;
			}
			set
			{
				this.numberedTextBoxUC1.TextBox.Text = value;
			}
		}

		private int PercentageIndentTab()
		{
			int intResult;
			int intSpaces = 0;
			int intTabs = 0;
			StringReader sr = new StringReader(this.TextBox.Text);
			while (true) {
				string strLine = sr.ReadLine();
				if (strLine == null) break;
				if (strLine.Length == 0) continue;
				if (strLine[0] == ' ') {
					intSpaces++;
				} else if (strLine[0] == '\t') {
					intTabs++;
				}
			}
			if (intTabs == 0 && intSpaces == 0) {
				intResult = 50;
			} else {
				intResult = (int)Math.Round((100.0 * intTabs) / (intTabs + intSpaces));
			}
			return intResult;
		}

		public void LoadFile(string strPath)
		{
			if (strPath.StartsWith("http://")) {
				this.FullPathName = Path.GetFileName(strPath);
			} else {
				this.FullPathName = strPath;
			}
			this.encodedAs = this.numberedTextBoxUC1.TextBox.LoadFile(strPath);

			if (this.IsScript) {

				if (Properties.Settings.Default.IndentAutoCorrect) {
					this.TextBox.FormatDocument();
					this.TextBox.ClearUndoStack();
				} else {
					if (Properties.Settings.Default.IndentWarning) {
						if ((PercentageIndentTab() > 50 && Properties.Settings.Default.SL4SpacesIndent) ||
							(PercentageIndentTab() < 50 && !Properties.Settings.Default.SL4SpacesIndent)) {
							if (MessageBox.Show("Indent scheme differs from settings\nDo you want to correct it?\nIt can also be corrected by pressing Ctrl-D or turn on Autocorrection (tools menu)", "Indent Warning!!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
								this.TextBox.FormatDocument();
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
            if (!LSLIPathHelper.IsExpandedLSL(strPath))
            {
                this.FullPathName = strPath;
                Encoding encodeAs = this.encodedAs;
                if (this.IsScript && encodeAs == null)
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

                this.numberedTextBoxUC1.TextBox.SaveCurrentFile(strPath, encodeAs);
                this.encodedAs = encodeAs;

            } else if (LSLIPathHelper.IsExpandedLSL(strPath))
            {
                string LSLIfilePath = LSLIPathHelper.CreateCollapsedPathAndScriptName(strPath);
                // Check if an LSLI version of this script exists
                if (File.Exists(LSLIfilePath))
                {
                    // Save the LSLI file as well
                    File.WriteAllText(LSLIfilePath, LSLIConverter.CollapseToLSLI(this.numberedTextBoxUC1.TextBox.Text));
                    EditForm form = null;

                    // If it's currently open, then refresh it
                    for (int i = 0; i < Application.OpenForms.Count; i++)
                    {
                        Form openForm = Application.OpenForms[i];
                        string filename = LSLIPathHelper.TrimStarsAndWhiteSpace(openForm.Text);
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
                this.numberedTextBoxUC1.TextBox.Dirty = false;
                this.Text = LSLIPathHelper.GetExpandedTabName(strPath);
            }
        }

		public void SaveCurrentFile()
		{
			this.SaveCurrentFile(this.FullPathName);
		}

		public bool Dirty
		{
			get
			{
				return this.numberedTextBoxUC1.TextBox.Dirty;
			}
			set
			{
				this.numberedTextBoxUC1.TextBox.Dirty = value;
			}
		}

		public TabControl tabControl
		{
			get
			{
				return this.tabControl1;
			}
		}

		public void SetFocus()
		{
			this.numberedTextBoxUC1.TextBox.Focus();
		}

		public void StopCompiler()
		{
			this.numberedTextBoxUC1.TextBox.MakeAllInvis();

			if (runtime != null) {
				this.components.Remove(runtime);
				if (!runtime.IsDisposed) {
					runtime.Dispose();
				}
				runtime = null;
			}

			for (int intI = this.tabControl1.TabPages.Count - 1; intI > 0; intI--) {
				this.tabControl1.TabPages.RemoveAt(intI);
			}
		}

		public SecondLifeHost.SecondLifeHostChatHandler ChatHandler;
		public SecondLifeHost.SecondLifeHostMessageLinkedHandler MessageLinkedHandler;

		public bool StartCompiler()
		{
			bool blnResult = false;
			//if (this.disableCompilesyntaxCheckToolStripMenuItem.Checked)
			//	return false;

			if (this.IsScript) {
				StopCompiler();

				if (this.parent != null) {
					runtime = new RuntimeConsole(this.parent);

					// for disposing
					this.components.Add(runtime);

                    foreach (string message in verboseQueue)
                    {
                        runtime.VerboseConsole(message);

                        if (message.StartsWith("Error: "))
                        {
                            StopCompiler();
                            this.tabControl1.SelectedIndex = 0;
                            verboseQueue = new List<string>();
                            return false;
                        }
                    }

					if (!runtime.Compile(this)) {
						this.tabControl1.SelectedIndex = 0;
						return false;
					}


                    TabPage tabPage = new TabPage("Debug");
					tabPage.Controls.Add(runtime);
					this.tabControl1.TabPages.Add(tabPage);
					this.tabControl1.SelectedIndex = 1;
					blnResult = true;
				}
			}
			return blnResult;
		}

		public bool SyntaxCheck()
		{
			bool blnResult = false;
			//if (this.disableCompilesyntaxCheckToolStripMenuItem.Checked)
			//	return false;

			if (this.IsScript) {
                string lsl = SourceCode;

                // If it is LSLI, it needs to import scripts first, before it recognizes imported functions
                if (LSLIPathHelper.IsLSLI(this.FullPathName))
                {
                    LSLIConverter converter = new LSLIConverter();
                    lsl = converter.ExpandToLSL(this);
                }

				LSL2CSharp translator = new LSL2CSharp(ConfLSL);
				string strCSharp = translator.Parse(lsl);

				if (System.Diagnostics.Debugger.IsAttached) {
					for (int intI = this.tabControl1.TabPages.Count - 1; intI > 0; intI--) {
						this.tabControl1.TabPages.RemoveAt(intI);
					}

					// TODO
					TabPage tabPage = new TabPage("C#");
					NumberedTextBox.NumberedTextBoxUC numberedTextBoxUC1 = new NumberedTextBox.NumberedTextBoxUC();
					numberedTextBoxUC1.TextBox.Init(null, this.ConfCSharp);
					numberedTextBoxUC1.TextBox.Text = strCSharp;
					numberedTextBoxUC1.TextBox.ReadOnly = true;
					numberedTextBoxUC1.Dock = DockStyle.Fill;
					tabPage.Controls.Add(numberedTextBoxUC1);
					this.tabControl.TabPages.Add(tabPage);
				}
				blnResult = (null != CompilerHelper.CompileCSharp(this, strCSharp));
			}
			return blnResult;
		}

		public int Find(string strSearch, int intStart, int intEnd, RichTextBoxFinds options)
		{
			intStart = this.numberedTextBoxUC1.TextBox.Find(strSearch, intStart, intEnd, options);
			this.numberedTextBoxUC1.TextBox.Focus();
			return intStart;
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.MakeAllInvis();
		}

		private void EditForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.parent.CancelClosing = false;
			if (this.Dirty) {
                string scriptToSave = ScriptName;
                if (LSLIPathHelper.IsExpandedLSL(ScriptName))
                {
                    // Expanded scripts will always be saved as LSLI's
                    scriptToSave = LSLIPathHelper.CreateCollapsedScriptName(scriptToSave);
                }

                DialogResult dialogResult = MessageBox.Show(this, @"Save """ + scriptToSave + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
				if (dialogResult == DialogResult.Yes) {
					e.Cancel = !this.parent.SaveFile(this, false);
				} else {
					e.Cancel = (dialogResult == DialogResult.Cancel);
                }
			}

            if (!e.Cancel)
            {
                // Close related readonly's if this is an expanded script
                if (LSLIPathHelper.IsExpandedLSL(ScriptName))
                {
                    // Check if a LSLI readonly is open
                    EditForm readOnlyLSLI = (EditForm)parent.GetForm(Path.GetFileName(
                        LSLIPathHelper.CreateCollapsedScriptName(ScriptName)) + " (Read Only)");

                    if (readOnlyLSLI != null)
                    {
                        readOnlyLSLI.Close();
                    }
                }

                // Delete expanded file when closing
                string expandedFile = LSLIPathHelper.CreateExpandedPathAndScriptName(FullPathName);
                if (File.Exists(expandedFile))
                {
                    File.Delete(expandedFile);
                }
            }
            this.parent.CancelClosing = e.Cancel;
		}

		private void disableCompilesyntaxCheckToolStripMenuItem_Click(object sender, EventArgs e)
		{
			//this.disableCompilesyntaxCheckToolStripMenuItem.Checked = !this.disableCompilesyntaxCheckToolStripMenuItem.Checked;
		}

		private void tvOutline_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
		{
			this.parent.BeginInvoke(new TreeNodeMouseClickEventHandler(
				delegate(object sender2, TreeNodeMouseClickEventArgs e2)
				{
					if (e.Node.Tag is Helpers.OutlineHelper) {
						Helpers.OutlineHelper ohOutline = (Helpers.OutlineHelper)e.Node.Tag;
						if (ohOutline.line < this.TextBox.Lines.Length) {
							//editForm.Focus();
							//editForm.TextBox.Select();
							//editForm.TextBox.Goto(ohOutline.line + 1);

							//TextBox.Focus();
							this.TextBox.Select();
							this.TextBox.SelectionStart = this.TextBox.GetFirstCharIndexFromLine(ohOutline.line);


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
			this.tvOutline.ExpandAll();
        }
    }
}
