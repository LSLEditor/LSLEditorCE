// /**
// ********
// *
// * ORIGIONAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon'Dimentox Travanti' Husbands & Malcolm J. Kudra which in turn Liscense under the GPLv2.
// * In agreement to Alphons van der Heijden wishes.
// *
// * The community would like to thank Alphons for all of his hard work, blood sweat and tears.
// * Without his work the community would be stuck with crappy editors.
// *
// * The source code in this file ("Source Code") is provided by The LSLEditor Group
// * to you under the terms of the GNU General Public License, version 2.0
// * ("GPL"), unless you have obtained a separate licensing agreement
// * ("Other License"), formally executed by you and Linden Lab.  Terms of
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
using System.Drawing;
using System.Windows.Forms;

using LSLEditor.Helpers;

namespace LSLEditor
{
	public partial class EditForm : Form
	{
		public RuntimeConsole runtime;

		private string m_FullPathName;
		private Guid m_Guid;

		public LSLEditorForm parent;

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
					this.numberedTextBoxUC1.TextBox.MakeAllInvis();
				}
			}
			try { base.WndProc(ref m); } catch {}
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
				if(value)
					this.tabPage1.Text = "Script";
				else
					this.tabPage1.Text = "Text";
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

			this.numberedTextBoxUC1.TextBox.Init(this.parent, this.parent.ConfLSL);
			this.numberedTextBoxUC1.TextBox.OnDirtyChanged += new IsDirtyHandler(TextBox_OnDirtyChanged);

			this.Move += new EventHandler(EditForm_Position);
			this.Resize += new EventHandler(EditForm_Position);

			this.Layout += new LayoutEventHandler(EditForm_Layout);

			SetFont();
		}

		public void SetFont()
		{
			this.numberedTextBoxUC1.Font = Properties.Settings.Default.FontEditor;
		}

		void EditForm_Layout(object sender, LayoutEventArgs e)
		{
			if (this.WindowState == FormWindowState.Minimized)
				this.numberedTextBoxUC1.TextBox.MakeAllInvis();
		}

		void EditForm_Position(object sender, EventArgs e)
		{
			this.numberedTextBoxUC1.TextBox.SetPosition(this.MdiParent.RectangleToScreen(this.MdiParent.ClientRectangle));
		}

		void TextBox_OnDirtyChanged(object sender, EventArgs e)
		{
			this.Text = this.ScriptName;
			if (this.numberedTextBoxUC1.TextBox.Dirty)
				this.Text = this.Text.Trim()+"*  ";
			else
				this.Text = this.Text.Trim()+"   ";
			TabPage tabPage = this.Tag as TabPage;
			if (tabPage != null)
				tabPage.Text = this.Text;
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
				if (Directory.Exists(strDirectory))
				{
					Properties.Settings.Default.WorkingDirectory = strDirectory;
				}
				else
				{
					if(!Directory.Exists(Properties.Settings.Default.WorkingDirectory))
						Properties.Settings.Default.WorkingDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
					this.m_FullPathName = Path.Combine(Properties.Settings.Default.WorkingDirectory, this.m_FullPathName);
				}
				this.Text = this.ScriptName;
				TabPage tabPage = this.Tag as TabPage;
				if (tabPage != null)
					tabPage.Text = this.Text + "   ";
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
			int intSpaces = 0;
			int intTabs = 0;
			StringReader sr = new StringReader(this.TextBox.Text);
			while (true)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;
				if (strLine.Length == 0)
					continue;
				if (strLine[0] == ' ')
					intSpaces++;
				else if (strLine[0] == '\t')
					intTabs++;
			}
			if (intTabs == 0 && intSpaces==0)
				return 50;
			return (int)Math.Round((100.0 * intTabs) / (intTabs + intSpaces));
		}

		public void LoadFile(string strPath)
		{
			if(strPath.StartsWith("http://"))
				this.FullPathName = Path.GetFileName(strPath);
			else
				this.FullPathName = strPath;
			this.numberedTextBoxUC1.TextBox.LoadFile(strPath);

			if (!this.IsScript)
				return;

			if (Properties.Settings.Default.IndentAutoCorrect)
			{
				this.TextBox.FormatDocument();
				this.TextBox.ClearUndoStack();
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
							this.TextBox.FormatDocument();
							//this.TextBox.ClearUndoStack();
						}
					}
				}
			}
		}

		public void SaveCurrentFile(string strPath)
		{
			this.FullPathName = strPath;
			this.numberedTextBoxUC1.TextBox.SaveCurrentFile(strPath);
		}

		public void SaveCurrentFile()
		{
			this.numberedTextBoxUC1.TextBox.SaveCurrentFile(this.FullPathName);
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

			if (runtime != null)
			{
				this.components.Remove(runtime);
				if (!runtime.IsDisposed)
					runtime.Dispose();
				runtime = null;
			}
			
			for (int intI = this.tabControl1.TabPages.Count - 1; intI > 0; intI--)
			{
				this.tabControl1.TabPages.RemoveAt(intI);
			}
		}

		public SecondLifeHost.SecondLifeHostChatHandler ChatHandler;
		public SecondLifeHost.SecondLifeHostMessageLinkedHandler MessageLinkedHandler;

		public bool StartCompiler()
		{
			if (this.disableCompilesyntaxCheckToolStripMenuItem.Checked)
				return false;

			if (!this.IsScript)
				return false;

			StopCompiler();

			if (this.parent == null)
				return false;

			runtime = new RuntimeConsole(this.parent);

			// for disposing
			this.components.Add(runtime);

			if (!runtime.Compile(this))
			{
				this.tabControl1.SelectedIndex = 0;
				return false;
			}

			TabPage tabPage = new TabPage("Debug");
			tabPage.Controls.Add(runtime);
			this.tabControl1.TabPages.Add(tabPage);
			this.tabControl1.SelectedIndex = 1;
			return true;
		}

		public bool SyntaxCheck()
		{
			if (this.disableCompilesyntaxCheckToolStripMenuItem.Checked)
				return false;

			if (!this.IsScript)
				return false;

			LSL2CSharp translator = new LSL2CSharp(ConfLSL);
			string strCSharp = translator.Parse(SourceCode);

			if (System.Diagnostics.Debugger.IsAttached)
			{
				for (int intI = this.tabControl1.TabPages.Count - 1; intI > 0; intI--)
				{
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

			return (null != CompilerHelper.CompileCSharp(this, strCSharp));
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
			if (this.Dirty)
			{
				DialogResult dialogResult = MessageBox.Show(this, @"Save """ + this.ScriptName + @"""?", "File has changed", MessageBoxButtons.YesNoCancel);
				if (dialogResult == DialogResult.Yes)
					e.Cancel = this.parent.SaveFile(this,false);
				else
					e.Cancel = (dialogResult == DialogResult.Cancel);
			}
			this.parent.CancelClosing = e.Cancel;
		}

		private void disableCompilesyntaxCheckToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.disableCompilesyntaxCheckToolStripMenuItem.Checked = !this.disableCompilesyntaxCheckToolStripMenuItem.Checked;
		}
	}
}