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
	public partial class llDialogForm : Form
	{
		private SecondLifeHost host;
		private int Channel;
		private string ObjectName;
		private string OwnerName;
		private SecondLife.key id;

		public llDialogForm(SecondLifeHost host, SecondLife.String strObjectName, SecondLife.key id, SecondLife.String strOwner, SecondLife.String strMessage, SecondLife.list buttons, SecondLife.integer intChannel)
		{
			InitializeComponent();

			this.host = host;
			this.Channel = intChannel;
			this.OwnerName = strOwner;
			this.ObjectName = strObjectName;
			this.id = id;

			for (int intI = 1; intI <= 12; intI++) {
				Button button = this.Controls["Button" + intI] as Button;
				button.Visible = false;
			}

			this.label1.Text = strOwner + "'s '" + strObjectName + "'";
			this.label2.Text = strMessage.ToString().Replace("&", "&&");

			for (int intI = 1; intI <= buttons.Count; intI++) {
				Button button = this.Controls["Button" + intI] as Button;
				if (button == null)
					continue;
				button.Text = buttons[intI - 1].ToString().Replace("&", "&&");
				button.Visible = true;
				button.Click += new EventHandler(button_Click);
			}
		}

		void button_Click(object sender, EventArgs e)
		{
			Button button = sender as Button;
			if (button != null) {
				host.Chat(this, this.Channel, this.OwnerName, this.id, button.Text.Replace("&&", "&"), CommunicationType.Say);
				this.Close();
			}
		}

		private void button13_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}
