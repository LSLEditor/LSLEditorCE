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
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
	public partial class TextEditorGeneral : UserControl, ICommit
	{
		public TextEditorGeneral()
		{
			InitializeComponent();

			this.checkBox1.Checked = Properties.Settings.Default.IndentWarning;

			this.checkBox2.Checked = Properties.Settings.Default.Indent;
			checkBox2_CheckedChanged(null, null);

			this.radioButton1.Checked = Properties.Settings.Default.IndentFullAuto;
			this.radioButton2.Checked = Properties.Settings.Default.IndentCursorPlacement;

			this.radioButton3.Checked = !Properties.Settings.Default.SL4SpacesIndent;
			this.radioButton4.Checked = Properties.Settings.Default.SL4SpacesIndent;

			this.checkBox3.Checked = Properties.Settings.Default.AutoWordSelection;

			this.checkBox4.Checked = Helpers.FileAssociator.IsAssociated(".lsl");

			this.checkBox5.Checked = Properties.Settings.Default.IndentAutoCorrect;


			switch(Properties.Settings.Default.OutputFormat)
			{
				case "Unicode":
					this.radioButton6.Checked = true;
					break;
				case "BigEndianUnicode":
					this.radioButton7.Checked = true;
					break;
				case "UTF8":
					this.radioButton8.Checked = true;
					break;
				default: // ANSI
					this.radioButton5.Checked = true;
					break;
			}
		}

		public void Commit()
		{
			Properties.Settings.Default.IndentWarning = this.checkBox1.Checked;
			Properties.Settings.Default.Indent = this.checkBox2.Checked;
			Properties.Settings.Default.AutoWordSelection = this.checkBox3.Checked;
			Properties.Settings.Default.SL4SpacesIndent = this.radioButton4.Checked;
			Properties.Settings.Default.IndentAutoCorrect = this.checkBox5.Checked;
			Properties.Settings.Default.IndentFullAuto = this.radioButton1.Checked;
			Properties.Settings.Default.IndentCursorPlacement = this.radioButton2.Checked;

			if (this.radioButton5.Checked)
				Properties.Settings.Default.OutputFormat = "ANSI";
			else if (this.radioButton6.Checked)
				Properties.Settings.Default.OutputFormat = "Unicode";
			else if (this.radioButton7.Checked)
				Properties.Settings.Default.OutputFormat = "BigEndianUnicode";
			else if (this.radioButton8.Checked)
				Properties.Settings.Default.OutputFormat = "UTF8";

			if (this.checkBox4.Checked)
			{
				if(!
				Helpers.FileAssociator.Associate(".lsl", "LSLEditorScript", "SecondLife script file for LSLEditor", Assembly.GetExecutingAssembly().Location, 0))
					MessageBox.Show("File association can not be made (needs administrative access)", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);

			}
			else
			{
				if (Helpers.FileAssociator.IsAssociated(".lsl"))
				{
					if(!Helpers.FileAssociator.DeAssociate(".lsl", "LSLEditorScript"))
						MessageBox.Show("File association can not be unmade (needs administrative access)", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void checkBox2_CheckedChanged(object sender, EventArgs e)
		{
			this.radioButton1.Enabled = this.checkBox2.Checked;
			this.radioButton2.Enabled = this.checkBox2.Checked;
		}

	}
}
