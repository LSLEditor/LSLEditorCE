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
using System.Windows.Forms;

namespace LSLEditor.Tools
{
	public partial class VersionControlGeneral : UserControl, ICommit
	{
		public VersionControlGeneral()
		{
			InitializeComponent();

            this.groupBox1.Enabled = true;//Properties.Settings.Default.VersionControl;

			this.checkBox1.Checked = Properties.Settings.Default.VersionControlSVN;
			this.checkBox1.CheckedChanged += new EventHandler(checkBox1_CheckedChanged);
			checkBox1_CheckedChanged(null, null);
			this.textBox1.Text = Properties.Settings.Default.SvnExe;
			this.textBox2.Text = Properties.Settings.Default.SvnUserid;
			this.textBox3.Text = Properties.Settings.Default.SvnPassword;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.textBox1.Enabled = this.checkBox1.Checked;
		}

		public void Commit()
		{
			Properties.Settings.Default.SvnExe = this.textBox1.Text;
			Properties.Settings.Default.SvnUserid = this.textBox2.Text;
			Properties.Settings.Default.SvnPassword = this.textBox3.Text;

			Properties.Settings.Default.VersionControlSVN = this.checkBox1.Checked;
		}


		private void button3_Click(object sender, EventArgs e)
		{
			this.openFileDialog1.FileName = "svn.exe";
			this.openFileDialog1.Filter = "Executables (*.exe)|*.exe|All files (*.*)|*.*";
			if (this.openFileDialog1.ShowDialog(this) == DialogResult.OK)
			{
				if (File.Exists(this.openFileDialog1.FileName))
					this.textBox1.Text = this.openFileDialog1.FileName;
			}
		}

	}
}
