// /**
// ********
// *
// * ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon 'Dimentox Travanti' Husbands & Malcolm J. Kudra, who in turn License under the GPLv2.
// * In agreement with Alphons van der Heijden's wishes.
// *
// * The community would like to thank Alphons for all of his hard work, blood sweat and tears.
// * Without his work the community would be stuck with crappy editors.
// *
// * The source code in this file ("Source Code") is provided by The LSLEditor Group
// * to you under the terms of the GNU General Public License, version 2.0
// * ("GPL"), unless you have obtained a separate licensing agreement
// * ("Other License"), formally executed by you and The LSLEditor Group.  Terms of
// * the GPL can be found in the gplv2.txt document.
// *
// ********
// * GPLv2 Header
// ********
// * LSLEditor, a External editor for the LSL Language.
// * Copyright (C) 2010 The LSLEditor Group.
// 
// * This program is free software; you can redistribute it and/or
// * modify it under the terms of the GNU General Public License
// * as published by the Free Software Foundation; either version 2
// * of the License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with this program; if not, write to the Free Software
// * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********
// *
// * The above copyright notice and this permission notice shall be included in all 
// * copies or substantial portions of the Software.
// *
// ********
// */
using System;
using System.IO;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using System.Collections.Specialized;

namespace LSLEditor.Tools
{
	public partial class EnvironmentPlugins : UserControl, ICommit
	{
		private int intX,intY;

		public EnvironmentPlugins()
		{
			InitializeComponent();

			ShowPlugins();
		}

		private void ShowPlugin(string strName)
		{
			CheckBox checkBox = new CheckBox();
			checkBox.AutoSize = true;
			checkBox.Text = strName;
			checkBox.Name = "Plugin_" + strName;
			checkBox.Tag = strName;
			checkBox.Checked = Properties.Settings.Default.Plugins.Contains(checkBox.Tag.ToString());
			checkBox.Location = new Point(intX, intY);
			this.groupBox1.Controls.Add(checkBox);
			intY += 20;
			this.label1.Visible = false;
		}

		private void ShowPlugins()
		{
			if (Properties.Settings.Default.Plugins == null)
				Properties.Settings.Default.Plugins = new StringCollection();

			string strDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			string strPluginsDirectory = Path.Combine(strDirectory, "Plugins");

			intX = 20;
			intY = 20;

			if (Directory.Exists(strPluginsDirectory))
			{
				foreach (string strFilePath in Directory.GetFiles(strPluginsDirectory, "*.exe"))
				{
					ShowPlugin(Path.GetFileNameWithoutExtension(strFilePath));
				}
			}
			if (Svn.IsInstalled)
			{
				if(!Properties.Settings.Default.Plugins.Contains("SVN (Version control)"))
					Properties.Settings.Default.Plugins.Add("SVN (Version control)");
				ShowPlugin("SVN (Version control)");
			}

			this.groupBox1.Height = intY + 20;
		}

		public void Commit()
		{
			Properties.Settings.Default.Plugins = new StringCollection();

			bool SvnPlugin = false;
			foreach (Control control in this.groupBox1.Controls)
			{
				CheckBox checkBox = control as CheckBox;
				if (checkBox == null)
					continue;
				if (checkBox.Checked)
				{
					string strPluginName = checkBox.Tag.ToString();
					Properties.Settings.Default.Plugins.Add(strPluginName);
					if (strPluginName.ToLower().Contains("svn"))
						SvnPlugin = true;
				}
			}
			if (!SvnPlugin)
			{
				Properties.Settings.Default.VersionControl = false;
				Properties.Settings.Default.VersionControlSVN = false;
			}
		}
	}
}
