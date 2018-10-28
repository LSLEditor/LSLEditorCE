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
// BugReportForm.cs
//
// </summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using LSLEditor.org.lsleditor.www;

namespace LSLEditor.BugReport
{
	public partial class BugReportForm : Form
	{
		private bool blnComplete;
		private UploadBugReport ubr;

		private readonly LSLEditorForm parent;

		public BugReportForm(LSLEditorForm parent)
		{
			this.InitializeComponent();
			this.parent = parent;
			this.Icon = parent.Icon;
			this.textBox1.Text = Properties.Settings.Default.AvatarName;
			this.textBox2.Text = Properties.Settings.Default.EmailAddress;
			this.listView1.Columns.Add("ScriptName", this.listView1.Width - 30);
			foreach (var form in parent.Children) {
				var editForm = form as EditForm;
				if (editForm?.IsDisposed != false) {
					continue;
				}

				var lvi = new ListViewItem(editForm.ScriptName) {
					Checked = false
				};
				this.listView1.Items.Add(lvi);
			}
			this.ShowBugReportsList();
		}

		private void ShowBugReportsList()
		{
			if (Properties.Settings.Default.Bugreports == null) {
				return;
			}

			this.listView2.Items.Clear();
			foreach (var Handle in Properties.Settings.Default.Bugreports) {
				if (!long.TryParse(Handle, out var result)) {
					continue;
				}

				result *= (long)1e7;
				var dateTime = new DateTime(result);
				var lvi = new ListViewItem(Handle);
				lvi.SubItems.Add(dateTime.ToString());
				lvi.Tag = Handle;
				this.listView2.Items.Add(lvi);
			}
		}

		//close
		private void button3_Click(object sender, EventArgs e)
		{
			this.timer1.Stop();
			this.ubr?.Stop();

			this.Close();
		}

		// cancel
		private void button2_Click(object sender, EventArgs e)
		{
			this.button2.Enabled = false;
			this.timer1.Stop();
			this.progressBar1.Value = 0;
			this.ubr?.Stop();

			this.button1.Enabled = true;
		}

		private void button1_Click(object sender, EventArgs e)
		{
			var strMessage = this.textBox3.Text.Trim();
			if (strMessage?.Length == 0) {
				MessageBox.Show("The bug report is empty(?!), it is not send!", "Bug report", MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			this.ubr = new UploadBugReport();
			this.button1.Enabled = false;
			this.button2.Enabled = true;
			this.blnComplete = false;
			this.timer1.Start();
			this.ubr.OnComplete += this.ubr_OnComplete;
			var list = new List<UploadBugReport.FileToUpload>();
			var sb = new StringBuilder();

			Properties.Settings.Default.AvatarName = this.textBox1.Text;
			Properties.Settings.Default.EmailAddress = this.textBox2.Text;

			sb.AppendFormat("Version: {0} {1}\r\n",
				this.parent.Text,
				Assembly.GetExecutingAssembly().GetName().Version.ToString());
			sb.AppendFormat("Name: {0}\r\n", this.textBox1.Text);
			sb.AppendFormat("Email: {0}\r\n\r\n", this.textBox2.Text);
			sb.Append(this.textBox3.Text);
			list.Add(new UploadBugReport.FileToUpload("bugreport.txt", sb.ToString()));
			foreach (ListViewItem lvi in this.listView1.Items) {
				if (!lvi.Checked) {
					continue;
				}

				var strScriptName = lvi.Text;
				string strBody = null;
				foreach (var form in this.parent.Children) {
					var editForm = form as EditForm;
					if (editForm?.IsDisposed != false) {
						continue;
					}

					if (editForm.ScriptName == strScriptName) {
						strBody = editForm.SourceCode;
					}
				}
				if (strBody != null) {
					list.Add(new UploadBugReport.FileToUpload(strScriptName, strBody));
				}
			}
			this.ubr.UploadAsync(list, this.progressBar1);
		}

		private void ubr_OnComplete(object sender, UploadBugReport.UploadCompleteEventArgs e)
		{
			if (e.TotalBytes == -1) {
				MessageBox.Show("There is something wrong. Your bug report has not been sent!!", "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			} else {
				MessageBox.Show("Your bug report has been sent (" + e.TotalBytes + " bytes)", "Ready", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			this.blnComplete = true;
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (this.blnComplete) {
				this.timer1.Stop();

				this.button1.Enabled = true;
				this.button2.Enabled = false;

				this.progressBar1.Value = 0;
				this.textBox3.Clear();
				this.ShowBugReportsList();
				this.tabControl1.SelectedIndex = 1;
			}

			if (this.ubr != null) {
				if (!this.ubr.blnRunning) {
					this.button1.Enabled = true;
					this.timer1.Stop();
				}
			}
		}

		private void listView2_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listView2.SelectedItems.Count == 0) {
				return;
			}

			var lvi = this.listView2.SelectedItems[0];
			var Handle = lvi.Tag.ToString();

			var service1 = new Service1();
			service1.GetStatusCompleted += this.service1_GetStatusCompleted;
			service1.GetStatusAsync(Handle, Handle);
		}

		private void service1_GetStatusCompleted(object sender, GetStatusCompletedEventArgs e)
		{
			this.textBox5.Clear();
			var Handle = e.UserState.ToString();
			if (e.Error != null) {
				this.textBox4.Text = "Bug report [" + Handle + "] not available (at this time)";
				return;
			}

			var strResult = e.Result;
			if (strResult == null) {
				this.textBox5.Text = "Bug report [" + Handle + "] does not exist (anymore)";
				return;
			}

			this.textBox5.Text = strResult.Replace("\n", "\r\n");

			var service1 = new Service1();
			service1.GetBugReportCompleted += this.service1_GetBugReportCompleted;
			service1.GetBugReportAsync(Handle, Handle);
		}

		private void service1_GetBugReportCompleted(object sender, GetBugReportCompletedEventArgs e)
		{
			this.textBox4.Clear();
			var Handle = e.UserState.ToString();

			if (e.Error != null) {
				this.textBox4.Text = "Bug report [" + Handle + "] not available (at this time)";
				return;
			}

			var strResult = e.Result;
			if (strResult == null) {
				this.textBox4.Text = "Bug report [" + Handle + "] does not extist (anymore)";
				return;
			}

			this.textBox4.Text = strResult.Replace("\n", "\r\n");
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.listView2.SelectedItems.Count == 0) {
				return;
			}

			if (MessageBox.Show("Delete " + this.listView2.SelectedItems.Count + " bugreports?", "Delete Bugreports", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK) {
				for (var intI = 0; intI < this.listView2.SelectedItems.Count; intI++) {
					var lvi = this.listView2.SelectedItems[intI];
					var Handle = lvi.Tag.ToString();
					Properties.Settings.Default.Bugreports.Remove(Handle);
				}
				this.textBox4.Clear();
				this.textBox5.Clear();
			}
			this.ShowBugReportsList();
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (this.button4.Text.Contains("uncheck")) {
				foreach (ListViewItem lvi in this.listView1.Items) {
					lvi.Checked = false;
				}

				this.button4.Text = "check all";
			} else {
				foreach (ListViewItem lvi in this.listView1.Items) {
					lvi.Checked = true;
				}

				this.button4.Text = "uncheck all";
			}
		}
	}
}
