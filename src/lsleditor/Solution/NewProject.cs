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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Reflection;
using System.Windows.Forms;

namespace LSLEditor
{
	public partial class NewProject : Form
	{
		private LSLEditorForm parent;

		public NewProject(LSLEditorForm parent)
		{
			InitializeComponent();

			this.checkBox2.Visible = Svn.IsInstalled;

			this.parent = parent;

			this.textBox2.Text = Properties.Settings.Default.ProjectLocation;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}



		private void button3_Click(object sender, EventArgs e)
		{
			if (this.checkBox2.Checked)
				Checkout();
			else
				CreateNew();
		}

		private void Checkout()
		{
			Svn svn = new Svn();
			string strSvnRepository = this.textBox1.Text.Trim();
			string strLocalDirectory = this.textBox2.Text.Trim();
			string strSolutionName = this.textBox3.Text.Trim();

			strLocalDirectory = Path.Combine(strLocalDirectory, strSolutionName);

			strSvnRepository = strSvnRepository.TrimEnd(new char[] {'/'});
			if (!strSvnRepository.EndsWith(strSolutionName))
				strSvnRepository += "/"+strSolutionName;
			strSvnRepository += "/";

			string strSolFile = strSvnRepository + strSolutionName + ".sol";
			if (!svn.Execute("list \"" + strSolFile +"\"",false,true))
				return;

			if (!svn.Execute("checkout \"" + strSvnRepository + "\" \"" + strLocalDirectory + "\"",false,true))
				return;

			// Into solution directory
			string strLocalSolFile = Path.Combine(strLocalDirectory, strSolutionName + ".sol");
			if(!Directory.Exists(strLocalDirectory))
			{
				MessageBox.Show("Can't find directory " + strLocalDirectory, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (!File.Exists(strLocalSolFile))
			{
				MessageBox.Show("Can't find solution file " + strLocalSolFile, "Checkout Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (parent.SolutionExplorer.OpenSolution(strLocalSolFile))
			{
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void CreateNew()
		{
			if (this.parent.SolutionExplorer.CreateNew(this.textBox1.Text, this.textBox2.Text, this.textBox3.Text, this.checkBox1.Checked))
			{
				this.parent.ShowSolutionExplorer(true);
				this.parent.UpdateRecentProjectList(this.parent.SolutionExplorer.GetCurrentSolutionPath(),true);
				this.DialogResult = DialogResult.OK;
				this.Close();
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			if (this.folderBrowserDialog1.ShowDialog(this) == DialogResult.OK)
			{
				this.textBox2.Text = this.folderBrowserDialog1.SelectedPath;
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			if (this.checkBox2.Checked)
			{
				this.label1.Text = "SVN Path";
				this.textBox1.Text = "";
			}
			else
			{
				this.label1.Text = "Name";
				this.textBox1.Text = "Project";
			}
		}
	}
}