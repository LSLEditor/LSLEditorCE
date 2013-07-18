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
using System.Text; // StringBuilder
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace LSLEditor
{
	public partial class FindWindow : Form
	{
		private LSLEditorForm lslEditForm;
		private int intStart;
		private int intEnd;

		private bool m_ReplaceAlso;

		public FindWindow(LSLEditorForm lslEditForm)
		{
			InitializeComponent();
			this.lslEditForm = lslEditForm;
			this.intStart = 0;
			this.intEnd = 0;
		}

		public bool ReplaceAlso
		{
			get
			{
				return m_ReplaceAlso;
			}
			set
			{
				m_ReplaceAlso = value;
				this.groupBox2.Enabled = m_ReplaceAlso;
				this.Replace.Enabled = m_ReplaceAlso;
				this.ReplaceAll.Enabled = m_ReplaceAlso;

				if (m_ReplaceAlso)
					this.Text = "Find and Replace";
				else
					this.Text = "Find";
			}
		}

		public string KeyWord
		{
			set
			{
				this.label1.Text = ""; // clear out message
				if (value != "") {
					this.comboBoxFind.Text = value;
				} else {
					if (this.comboBoxFind.Items.Count > 0) {
						this.comboBoxFind.SelectedIndex = this.comboBoxFind.Items.Count - 1;
					}
				}
			}
		}

		private bool UpdateComboBox(ComboBox comboBox)
		{
			string strText = comboBox.Text;
			bool Found = false;

			foreach (string strC in comboBox.Items) {
				if (strC == strText) {
					Found = true;
					break;
				}
			}

			if (!Found) {
				comboBox.Items.Add(strText);
			}
			return Found;
		}

		public void Find()
		{
			this.label1.Text = "";
			EditForm editForm = this.lslEditForm.ActiveMdiForm as EditForm;
			if (editForm != null) {
				if (!UpdateComboBox(this.comboBoxFind)) {
					editForm.TextBox.SelectionLength = 0;
					editForm.TextBox.SelectionStart = 0;
				}

				RichTextBoxFinds options = RichTextBoxFinds.None;

				if (this.checkBoxMatchCase.Checked) options |= RichTextBoxFinds.MatchCase;
				if (this.checkBoxReverse.Checked) options |= RichTextBoxFinds.Reverse;
				if (this.checkBoxWholeWord.Checked) options |= RichTextBoxFinds.WholeWord;

				if (this.checkBoxReverse.Checked) {
					intStart = 0; // start cant change ;-)
					intEnd = editForm.TextBox.SelectionStart;
				} else {
					intStart = editForm.TextBox.SelectionStart + editForm.TextBox.SelectionLength;
					if (intStart == editForm.TextBox.Text.Length) {
						intStart = 0;
					}
					intEnd = editForm.TextBox.Text.Length - 1; // length can change!!
				}

				string strFind = this.comboBoxFind.Text;
				int intIndex = editForm.Find(strFind, intStart, intEnd, options);
				if (intIndex < 0) {
					this.label1.Text = "Not found...";
					return;
				}
			}
		}

		private void FindNext_Click(object sender, EventArgs e)
		{
			Find();
			this.Focus();
		}

		private void comboBoxFind_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Return) {
				if (this.Replace.Enabled) {
					this.comboBoxReplace.Focus();
					e.SuppressKeyPress = true;
				} else {
					Find();
					e.SuppressKeyPress = true;
				}
			}
		}

		private void Replace_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.lslEditForm.ActiveMdiForm as EditForm;
			if (editForm != null) {
				UpdateComboBox(this.comboBoxReplace);

				if (editForm.TextBox.SelectionLength > 0) {
					string strReplacement = this.comboBoxReplace.Text;
					editForm.TextBox.ReplaceSelectedText(strReplacement);
				}

				Find();
				this.Focus();
			}
		}

		// WildCardToRegex not used!!
		private string WildCardToRegex(string strWildCard)
		{
			StringBuilder sb = new StringBuilder(strWildCard.Length + 8);
			for (int intI = 0; intI < strWildCard.Length; intI++) {
				char chrC = strWildCard[intI];
				switch (chrC) {
					case '*':
						sb.Append(".*");
						break;
					case '?':
						sb.Append(".");
						break;
					case '\\':
						intI++;
						if (intI < strWildCard.Length)
							sb.Append(Regex.Escape(strWildCard[intI].ToString()));
						break;
					default:
						sb.Append(Regex.Escape(chrC.ToString()));
						break;
				}
			}
			return sb.ToString();
		}

		private void ReplaceAll_Click(object sender, EventArgs e)
		{
			EditForm editForm = this.lslEditForm.ActiveMdiForm as EditForm;
			if (editForm == null) {
				UpdateComboBox(this.comboBoxReplace);

				string strPattern;
				string strFind = Regex.Escape(this.comboBoxFind.Text);
				string strReplacement = this.comboBoxReplace.Text;
				string strSourceCode = editForm.SourceCode;

				RegexOptions regexOptions = RegexOptions.Compiled;
				if (!this.checkBoxMatchCase.Checked) {
					regexOptions |= RegexOptions.IgnoreCase;
				}
				if (this.checkBoxWholeWord.Checked) {
					strPattern = @"\b" + strFind + @"\b";
				} else {
					strPattern = strFind;
				}

				Regex regex = new Regex(strPattern, regexOptions);

				int intCount = 0;
				foreach (Match m in regex.Matches(strSourceCode)) {
					if (m.Value.Length > 0) {
						intCount++;
					}
				}
				if (intCount == 0) {
					MessageBox.Show("No matches found");
				} else {
					if (MessageBox.Show("There are " + intCount + " occurences, replace them all?", "Find and Replace", MessageBoxButtons.YesNoCancel) == DialogResult.Yes) {
						editForm.SourceCode = regex.Replace(strSourceCode, strReplacement);
					}
				}
				this.Focus();
			}
		}

		private void FindWindow_FormClosing(object sender, FormClosingEventArgs e)
		{
			lslEditForm.TopMost = true; // 15 nove 2007
			this.Visible = false;
			e.Cancel = true;
			lslEditForm.TopMost = false;// 15 nove 2007
		}

		private void FindWindow_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyData == Keys.Escape) {
				this.Visible = false;
				e.SuppressKeyPress = true;
				e.Handled = true;
			}

			if (e.KeyCode == Keys.Return) {
				Find();
				e.SuppressKeyPress = true;
				this.Focus();
			}

			if (e.KeyCode == Keys.F3) {
				Find();
				e.SuppressKeyPress = true;
				this.Focus();
			}


		}

		public void FindFocus()
		{
			this.comboBoxFind.Focus();
		}
	}
}
