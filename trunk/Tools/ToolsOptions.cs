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
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace LSLEditor.Tools
{
	public partial class ToolsOptions : Form
	{
		public delegate void PropertiesChangedHandler();
		public event PropertiesChangedHandler PropertiesChanged;

		public ToolsOptions()
		{
			InitializeComponent();

			LoadTreeView();
		}

		private XmlDocument GetXmlFromResource(string strName)
		{
			XmlDocument xml = new XmlDocument();
			Stream resource = Assembly.GetExecutingAssembly().GetManifestResourceStream("LSLEditor." + strName);

			if (resource != null)
				xml.Load(resource);
			return xml;
		}

		private void LoadTreeView()
		{
			XmlDocument xml = GetXmlFromResource(Properties.Settings.Default.ToolsOptions);
			RecursiveLoad(this.treeView1.Nodes, xml.SelectSingleNode("/root"));
		}

		private void RecursiveLoad(TreeNodeCollection nodes, XmlNode xmlParentNode)
		{
			foreach (XmlNode xmlNode in xmlParentNode.SelectNodes("./*"))
			{
				string strName = xmlNode.Attributes["name"].Value;
				string strUserControl = xmlNode.Attributes["usercontrol"].Value;
				TreeNode tn = new TreeNode(strName, 0, 0);
				tn.Tag = strUserControl;
				nodes.Add(tn);
				RecursiveLoad(tn.Nodes, xmlNode);
			}
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			string strUserControl = "LSLEditor.Tools." + e.Node.Tag.ToString();
			bool blnFound = false;
			foreach (Control control in this.panel1.Controls)
			{
				control.Visible = (strUserControl == control.ToString());
				if (control.Visible)
					blnFound = true;
			}
			if (blnFound)
				return;
			//create control and add to panel1
			Control newControl = Assembly.GetExecutingAssembly().CreateInstance(strUserControl) as Control;
			if (newControl != null)
			{
				this.panel1.Controls.Add(newControl);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			// ok
			foreach (Control control in this.panel1.Controls)
			{
				((ICommit)control).Commit();
			}

			// save properties
			Properties.Settings.Default.Save();

			// notify parent
			if (PropertiesChanged != null)
				PropertiesChanged();

			this.Close();
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Reset all properties?", "Reset properties", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
			{
				this.panel1.Controls.Clear();

				Properties.Settings.Default.Reset();
				Properties.Settings.Default.CallUpgrade = false;

				if (Properties.Settings.Default.FontEditor == null)
					Properties.Settings.Default.FontEditor = new Font("Courier New", 9.75F, FontStyle.Regular);

				if (Properties.Settings.Default.FontTooltips == null)
					Properties.Settings.Default.FontTooltips = new Font(SystemFonts.MessageBoxFont.Name, 9.75F, FontStyle.Regular);
			}
		}

		private void ToolsOptions_Load(object sender, EventArgs e)
		{
			this.treeView1.ExpandAll();
			this.treeView1.SelectedNode = this.treeView1.Nodes[0];
			this.treeView1.Focus();
		}
	}
}