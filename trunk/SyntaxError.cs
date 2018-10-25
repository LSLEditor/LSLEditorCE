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
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using LSLEditor.Docking;

namespace LSLEditor
{
	public partial class SyntaxError : DockContent
	{
		public SyntaxError()
		{
			InitializeComponent();
		}

		public void Clear()
		{
			this.listView1.Items.Clear();
		}

		public ListView ListView
		{
			get
			{
				return this.listView1;
			}
		}

		public bool HasErrors
		{
			get
			{
				return (this.listView1.Items.Count > 0);
			}
		}

		public class SyntaxErrorEventArgs : EventArgs
		{
			public string FullPathName;
			public Guid EditFormGuid;
			public bool IsScript;
			public int Line;
			public int Char;
			public SyntaxErrorEventArgs(string strPath, Guid guid, bool isScript, int intLine, int intChar)
			{
				this.FullPathName = strPath;
				this.IsScript = isScript;
				this.EditFormGuid = guid;
				this.Line = intLine;
				this.Char = intChar;
			}
		}

		public delegate void SyntaxErrorHandler(object sender, SyntaxErrorEventArgs e);

		public event SyntaxErrorHandler OnSyntaxError;

		private void DoSyntaxError(string strPath, Guid guid, bool isScript, int intLine, int intChar)
		{
			if(OnSyntaxError != null)
				OnSyntaxError(this, new SyntaxErrorEventArgs(strPath, guid, isScript, intLine, intChar));
		}

		private void listView1_DoubleClick(object sender, EventArgs e)
		{
			foreach (ListViewItem lvi in this.listView1.Items)
				lvi.BackColor = this.listView1.BackColor;

			if (this.listView1.SelectedItems.Count == 1)
			{
				ListViewItem lvi = this.listView1.SelectedItems[0];
				lvi.BackColor = SystemColors.Control;
				int intLine, intChr;
				if (int.TryParse(lvi.SubItems[4].Text, out intLine) &&
					int.TryParse(lvi.SubItems[5].Text, out intChr))
				{
					string strPath = lvi.SubItems[7].Text;
					Guid guid = new Guid(lvi.SubItems[8].Text);
					bool isScript = Convert.ToBoolean(lvi.SubItems[9].Text);
					DoSyntaxError(strPath,guid,isScript, intLine, intChr);
				}
			}
		}

		private void listView1_Resize(object sender, EventArgs e)
		{
			if (this.listView1.Columns.Count == 0)
				return;
			int intWidth = 0;
			for (int intI = 0; intI < this.listView1.Columns.Count; intI++)
			{
				if (intI != 2)
					intWidth += this.listView1.Columns[intI].Width;
			}
			this.listView1.Columns[2].Width = this.listView1.Width - intWidth - 50;
		}

		private void CopyToClipboard(bool blnDescriptionOnly)
		{
			if (this.listView1.SelectedItems.Count > 0)
			{
				StringBuilder sb = new StringBuilder();
				//sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n", this.listView1.Columns[1].Text, this.listView1.Columns[2].Text, this.listView1.Columns[3].Text, this.listView1.Columns[4].Text, this.listView1.Columns[5].Text, this.listView1.Columns[6].Text);
				foreach (ListViewItem lvi in this.listView1.SelectedItems)
				{
					if(blnDescriptionOnly)
						sb.AppendFormat("{0}\r\n", lvi.SubItems[2].Text);
					else
						sb.AppendFormat("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\r\n", lvi.SubItems[1].Text, lvi.SubItems[2].Text, lvi.SubItems[3].Text, lvi.SubItems[4].Text, lvi.SubItems[5].Text, lvi.SubItems[6].Text);
				}
				Clipboard.SetDataObject(sb.ToString(), true);
			}
		}

		private void listView1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control)
			{
				if (e.KeyCode == Keys.A)
				{
					foreach (ListViewItem lvi in this.listView1.Items)
						lvi.Selected = true;
				}
				if (e.KeyCode == Keys.C)
				{
					CopyToClipboard(false);
				}
			}
		}

		private void copyLineToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopyToClipboard(false);
		}

		private void copyDescriptionOnlyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			CopyToClipboard(true);
		}
	}
}
