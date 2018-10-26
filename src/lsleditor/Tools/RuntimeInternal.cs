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
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace LSLEditor.Tools
{
	public partial class RuntimeInternal : UserControl, ICommit
	{
		public RuntimeInternal()
		{
			InitializeComponent();

			this.AvatarName.Text = Properties.Settings.Default.AvatarName;
			this.AvatarKey.Text = Properties.Settings.Default.AvatarKey;

			this.RegionName.Text = Properties.Settings.Default.RegionName;
			this.RegionFPS.Text = Properties.Settings.Default.RegionFPS.ToString();

			this.XSecondLifeShard.Text = Properties.Settings.Default.XSecondLifeShard;

			this.textBoxParcelName.Text = Properties.Settings.Default.ParcelName;
			this.textBoxParcelDescription.Text = Properties.Settings.Default.ParcelDescription;
			this.textBoxParcelOwner.Text = Properties.Settings.Default.ParcelOwner;
			this.textBoxParcelGroup.Text = Properties.Settings.Default.ParcelGroup;
			this.textBoxParcelArea.Text = Properties.Settings.Default.ParcelArea.ToString();
		}

		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			SearchForKey();
		}

		private void SearchForKey()
		{
			bool Searching = true;
			bool XmlVersion = false;
			try
			{
				string[] arName = this.AvatarName.Text.Split(' ');
				string strFirst = arName[0].Trim().ToLower();
				string strLast = arName[1].Trim().ToLower();
				string strCachePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\SecondLife\cache\name.cache";

				StreamReader sr = new StreamReader(strCachePath);
				while (Searching)
				{
					string strLine = sr.ReadLine();
					if (strLine == null)
						break;
					if (strLine.Contains("<llsd>"))
					{
						XmlVersion = true;
						break;
					}
					strLine = strLine.ToLower();
					if (strLine.IndexOf(strFirst) > 0)
					{
						if (strLine.IndexOf(strLast) > 0)
						{
							string strXoredKey = strLine.Split(new char[] { '\t' })[0].Trim();
							Guid g1 = new Guid(strXoredKey);
							Guid g2 = Guid.Empty;
							byte[] b1 = g1.ToByteArray();
							byte[] b2 = g2.ToByteArray();
							for (int intI = 0; intI < b1.Length; intI++)
								b2[intI] = (byte)(b1[intI] ^ 0x33);
							g2 = new Guid(b2);
							this.AvatarKey.Text = g2.ToString();
							Searching = false;
						}
					}
				}
				sr.Close();

				if (XmlVersion)
				{
					XmlDocument xmlDocument = new XmlDocument();
					xmlDocument.Load(strCachePath);
					// Who has made this key f*cking xml file??? it s*cks bigtime
					XmlNode xmlNodeMapFirst = xmlDocument.SelectSingleNode("//map[translate(string[1],'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" + strFirst + "']");
					XmlNode xmlNodeMapLast = xmlDocument.SelectSingleNode("//map[translate(string[2],'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz') = '" + strLast + "']");
					if (xmlNodeMapFirst != null && xmlNodeMapLast != null)
					{
						if (xmlNodeMapFirst == xmlNodeMapLast)
						{
							// previous sibbeling key is the key
							// Please, please, this meshed up my day, 
							// USE HIERARCHY PARENT-CHILD NOT SIBLINGS!!
							this.AvatarKey.Text = xmlNodeMapFirst.PreviousSibling.InnerText;
							Searching = false;
						}
					}
				}
			}
			catch
			{
			}
			if (Searching)
				this.AvatarKey.Text = "Name not found in Cache";
		}

		public void Commit()
		{
			Properties.Settings.Default.AvatarName = this.AvatarName.Text;
			Properties.Settings.Default.AvatarKey = this.AvatarKey.Text;

			Properties.Settings.Default.XSecondLifeShard = this.XSecondLifeShard.Text;

			Properties.Settings.Default.RegionName = this.RegionName.Text;
			double dblA;
			if(double.TryParse(this.RegionFPS.Text, out dblA))
				Properties.Settings.Default.RegionFPS = dblA;

			Properties.Settings.Default.ParcelName = this.textBoxParcelName.Text;
			Properties.Settings.Default.ParcelDescription = this.textBoxParcelDescription.Text;
			Properties.Settings.Default.ParcelOwner = this.textBoxParcelOwner.Text;
			Properties.Settings.Default.ParcelGroup = this.textBoxParcelGroup.Text;

			int intArea;
			if(int.TryParse(this.textBoxParcelArea.Text, out intArea))
				Properties.Settings.Default.ParcelArea = intArea;
			else
				Properties.Settings.Default.ParcelArea = 512;
		}

	}
}
