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
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
	public interface ICommit
	{
		void Commit();
	}

	public partial class EnvironmentGeneral : UserControl, ICommit
	{
		public EnvironmentGeneral()
		{
			this.InitializeComponent();

			this.radioButton1.Checked = Properties.Settings.Default.TabbedDocument;
			this.radioButton2.Checked = !Properties.Settings.Default.TabbedDocument;

			this.textBox1.Text = Properties.Settings.Default.RecentFileMax.ToString();
			this.textBox2.Text = Properties.Settings.Default.RecentProjectMax.ToString();

			this.checkBox1.Checked = Properties.Settings.Default.CheckForUpdates;
			this.checkBox2.Checked = Properties.Settings.Default.DeleteOldFiles;
			this.radioButton3.Checked = Properties.Settings.Default.CheckEveryDay;
			this.radioButton4.Checked = Properties.Settings.Default.CheckEveryWeek;

			this.checkBox1_CheckedChanged(null, null);
		}

		public void Commit()
		{
			Properties.Settings.Default.TabbedDocument = this.radioButton1.Checked;

			if (int.TryParse(this.textBox1.Text, out var intValue)) {
				Properties.Settings.Default.RecentFileMax = intValue;
			}

			if (int.TryParse(this.textBox2.Text, out intValue)) {
				Properties.Settings.Default.RecentProjectMax = intValue;
			}

			Properties.Settings.Default.CheckForUpdates = this.checkBox1.Checked;
			Properties.Settings.Default.CheckEveryDay = this.radioButton3.Checked;
			Properties.Settings.Default.CheckEveryWeek = this.radioButton4.Checked;
			Properties.Settings.Default.DeleteOldFiles = this.checkBox2.Checked;
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			this.radioButton3.Enabled = this.checkBox1.Checked;
			this.radioButton4.Enabled = this.checkBox1.Checked;
			this.groupBox4.Enabled = this.checkBox1.Checked;
		}
	}
}
