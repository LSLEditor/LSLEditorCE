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
using System.Reflection;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
	public partial class EnvironmentHelp : UserControl, ICommit
	{
		public EnvironmentHelp()
		{
			InitializeComponent();

			this.radioButton1.Checked = Properties.Settings.Default.HelpOnline;
			this.radioButton2.Checked = Properties.Settings.Default.HelpOffline;

			this.radioButton3.Checked = Properties.Settings.Default.WikiSeperateBrowser;
			this.radioButton4.Checked = !Properties.Settings.Default.WikiSeperateBrowser;

			this.checkBox1.Checked = Properties.Settings.Default.HelpNewTab;

			this.checkBox2.Checked = Properties.Settings.Default.ToolTip;

			this.textBox1.Text = Properties.Settings.Default.Help;

			radioButton1_CheckedChanged(null, null);

			radioButton4_CheckedChanged(null, null);
		}

		public void Commit()
		{
			Properties.Settings.Default.HelpOnline = this.radioButton1.Checked;
			Properties.Settings.Default.HelpOffline= this.radioButton2.Checked;
			Properties.Settings.Default.WikiSeperateBrowser = this.radioButton3.Checked;

			Properties.Settings.Default.HelpNewTab = this.checkBox1.Checked;
			Properties.Settings.Default.ToolTip = this.checkBox2.Checked;

			Properties.Settings.Default.Help = this.textBox1.Text;
		}

		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (this.radioButton2.Checked)
			{
				this.textBox1.Enabled = false;
				string strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
					Properties.Settings.Default.HelpOfflineFile);
				if (!File.Exists(strHelpFile))
				{
					if (MessageBox.Show("Help file does not exist, would you like to download it?", "Download Helpfile", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
					{
						UpdateApplicationForm updater = new UpdateApplicationForm();
						//updater.Icon = this.Icon; // TODO!!
						updater.CheckForHelpFile();
						updater.ShowDialog(this);
						return;
					}
				}

			}
		}

		private void radioButton1_CheckedChanged(object sender, EventArgs e)
		{
			this.textBox1.Enabled = this.radioButton1.Checked;
			this.groupBox1.Enabled = this.radioButton1.Checked;
			this.button1.Enabled = this.radioButton1.Checked;
			this.button2.Enabled = this.radioButton1.Checked;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.textBox1.Text = Properties.Settings.Default.Help1;
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.textBox1.Text = Properties.Settings.Default.Help2;
		}

		private void radioButton4_CheckedChanged(object sender, EventArgs e)
		{
			this.checkBox1.Enabled = this.radioButton4.Checked;
		}

	}
}
