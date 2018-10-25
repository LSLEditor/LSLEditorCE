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
// Browser.cs
//
// </summary>

using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using LSLEditor.Docking;

namespace LSLEditor
{
	public partial class Browser : DockContent
	{
		private LSLEditorForm lslEditorForm;

		public Browser(LSLEditorForm lslEditorForm)
		{
			InitializeComponent();

			this.Icon = lslEditorForm.Icon;
			this.lslEditorForm = lslEditorForm;

			// enables close buttons on tabs
			this.tabControl1.SetDrawMode();
		}

		private void axWebBrowser1_StatusTextChanged(object sender, EventArgs e)
		{
			WebBrowser axWebBrowser1 = sender as WebBrowser;
			ToolStripStatusLabel status = axWebBrowser1.Tag as ToolStripStatusLabel;
			if (status != null) {
				status.Text = axWebBrowser1.StatusText;
			}
		}

		private void axWebBrowser1_Navigating(object sender, WebBrowserNavigatingEventArgs e)
		{
			string strUrl = e.Url.ToString();
			if (strUrl.EndsWith(".lsl")) {
				e.Cancel = true;
				if (MessageBox.Show("Import LSL script?", "Import script", MessageBoxButtons.OKCancel) != DialogResult.Cancel) {
					WebBrowser axWebBrowser1 = sender as WebBrowser;
					axWebBrowser1.Stop();

					this.lslEditorForm.OpenFile(strUrl, Guid.NewGuid());
				}
			}
		}

		public void ShowWebBrowser(string strTabName, string strUrl)
		{
			WebBrowser axWebBrowser1 = null;
			try {
				if (!Properties.Settings.Default.HelpNewTab) {
					TabPage tabPage = this.tabControl1.TabPages[0];
					tabPage.Text = strTabName + "    ";
					axWebBrowser1 = tabPage.Controls[0] as WebBrowser;
				}
			} catch { }

			if (axWebBrowser1 == null) {
				TabPage tabPage = new TabPage(strTabName + "    ");
				tabPage.BackColor = Color.White;

				axWebBrowser1 = new WebBrowser();

				ToolStripStatusLabel toolStripStatusLabel1 = new ToolStripStatusLabel();
				StatusStrip statusStrip1 = new StatusStrip();

				statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
				statusStrip1.Location = new System.Drawing.Point(0, 318);
				statusStrip1.Name = "statusStrip1";
				statusStrip1.Size = new System.Drawing.Size(584, 22);
				statusStrip1.TabIndex = 0;
				statusStrip1.Text = "statusStrip1";

				toolStripStatusLabel1.Name = "toolStripStatusLabel1";
				toolStripStatusLabel1.Size = new System.Drawing.Size(109, 17);
				toolStripStatusLabel1.Text = "toolStripStatusLabel1";

				tabPage.Controls.Add(axWebBrowser1);

				tabPage.Controls.Add(statusStrip1);

				this.tabControl1.TabPages.Add(tabPage);
				this.tabControl1.SelectedIndex = this.tabControl1.TabCount - 1;

				// reference
				axWebBrowser1.Tag = toolStripStatusLabel1;

				axWebBrowser1.Dock = DockStyle.Fill;
				axWebBrowser1.StatusTextChanged += new EventHandler(axWebBrowser1_StatusTextChanged);
				axWebBrowser1.Navigating += new WebBrowserNavigatingEventHandler(axWebBrowser1_Navigating);
			}
			axWebBrowser1.Navigate(strUrl);
		}

		private void closeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			int intTabToClose = (int)this.contextMenuStrip1.Tag;
			if (intTabToClose < this.tabControl1.TabCount) {
				this.tabControl1.TabPages.RemoveAt(intTabToClose);
			}
		}

		private void tabControl1_MouseDown(object sender, MouseEventArgs e)
		{
			TabControl tabControl = sender as TabControl;
			if (tabControl != null) {
				if (e.Button == MouseButtons.Right) {
					for (int intI = 0; intI < tabControl.TabCount; intI++) {
						Rectangle rt = tabControl.GetTabRect(intI);
						if (e.X > rt.Left && e.X < rt.Right
							&& e.Y > rt.Top && e.Y < rt.Bottom) {
							this.contextMenuStrip1.Tag = intI;
							this.contextMenuStrip1.Show(this.tabControl1, new Point(e.X, e.Y));
						}
					}
				}
			}
		}
	}
}
