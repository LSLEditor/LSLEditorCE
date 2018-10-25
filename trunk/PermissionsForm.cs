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
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor
{
	public partial class PermissionsForm : Form
	{
		private SecondLifeHost host;
		private SecondLife.String ObjectName;
		private SecondLife.String OwnerName;
		private SecondLife.key id;
		private SecondLife.key agent;
		private SecondLife.integer intPermissions;

		public PermissionsForm(SecondLifeHost host, string strObjectName, SecondLife.key id, string strOwner, SecondLife.key agent, int intPermissions)
		{
			InitializeComponent();

			this.host = host;
			this.OwnerName = strOwner;
			this.ObjectName = strObjectName;
			this.agent = agent;
			this.id = id;
			this.intPermissions = intPermissions;

			StringBuilder sb = new StringBuilder();

			if ((intPermissions & SecondLife.PERMISSION_DEBIT) == SecondLife.PERMISSION_DEBIT) {
				sb.AppendLine("Take Linden dollars (L$) from you");
			}

			if ((intPermissions & SecondLife.PERMISSION_TAKE_CONTROLS) == SecondLife.PERMISSION_TAKE_CONTROLS) {
				sb.AppendLine("Act on your control inputs");
			}

			if ((intPermissions & SecondLife.PERMISSION_TRIGGER_ANIMATION) == SecondLife.PERMISSION_TRIGGER_ANIMATION) {
				sb.AppendLine("Animate your avatar");
			}

			if ((intPermissions & SecondLife.PERMISSION_ATTACH) == SecondLife.PERMISSION_ATTACH) {
				sb.AppendLine("Attach to your avatar");
			}

			if ((intPermissions & SecondLife.PERMISSION_CHANGE_LINKS) == SecondLife.PERMISSION_CHANGE_LINKS) {
				sb.AppendLine("Link and delink from other objects");
			}

			if ((intPermissions & SecondLife.PERMISSION_TRACK_CAMERA) == SecondLife.PERMISSION_TRACK_CAMERA) {
				sb.AppendLine("Track your camera");
			}

			if ((intPermissions & SecondLife.PERMISSION_CONTROL_CAMERA) == SecondLife.PERMISSION_CONTROL_CAMERA) {
				sb.AppendLine("Control your camera");
			}

			this.label1.Text = "'" + strObjectName + "', an object owned by '" + strOwner + "',\nwould like to:";
			this.label2.Text = sb.ToString();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.host.SetPermissions(new SecondLife.integer(0));
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.host.SetPermissions(this.intPermissions);
			this.Close();
		}
	}
}
