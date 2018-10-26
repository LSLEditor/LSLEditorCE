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
	public partial class TextEditorFontColors : UserControl, ICommit
	{
		private Font fontEditor;
		private Font fontTooltips;

		public TextEditorFontColors()
		{
			InitializeComponent();

			this.checkBox1.Checked = Properties.Settings.Default.SLColorScheme;

			fontEditor = Properties.Settings.Default.FontEditor;
			fontTooltips = Properties.Settings.Default.FontTooltips;

			ShowFonts();
		}

		private void ShowFonts()
		{
			this.label3.Text = string.Format("{0} / {1} / {2}",
				fontEditor.Name,
				fontEditor.Size,
				fontEditor.Style);

			this.label4.Text = string.Format("{0} / {1} / {2}",
				fontTooltips.Name,
				fontTooltips.Size,
				fontTooltips.Style);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			this.fontDialog1.FixedPitchOnly = true;
			this.fontDialog1.ShowEffects = false;
			this.fontDialog1.AllowScriptChange = false;
			this.fontDialog1.Font = Properties.Settings.Default.FontEditor;
			try
			{
				if (this.fontDialog1.ShowDialog(this) == DialogResult.OK)
				{
					fontEditor = this.fontDialog1.Font;
					ShowFonts();
				}
			}
			catch
			{
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			this.fontDialog1.FixedPitchOnly = false;
			this.fontDialog1.ShowEffects = false;
			this.fontDialog1.AllowScriptChange = false;
			this.fontDialog1.Font = Properties.Settings.Default.FontTooltips;
			if (this.fontDialog1.ShowDialog(this) == DialogResult.OK)
			{
				fontTooltips = this.fontDialog1.Font;
				ShowFonts();
			}
		}

		public void Commit()
		{
			Properties.Settings.Default.SLColorScheme = this.checkBox1.Checked;

			Properties.Settings.Default.FontEditor = fontEditor;
			Properties.Settings.Default.FontTooltips = fontTooltips;
		}


	}
}
