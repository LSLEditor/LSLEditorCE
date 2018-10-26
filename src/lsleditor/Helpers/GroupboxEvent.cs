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
using System.Windows.Forms;

namespace LSLEditor
{
	/// <summary>
	/// Summary description for GroupboxTextbox.
	/// </summary>
	public class GroupboxEvent : System.Windows.Forms.GroupBox
	{
		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public GroupboxEvent(Point pt,string strName,string strArgs,System.EventHandler eventHandler)
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			this.Location = pt;

			string[] args = strArgs.Trim().Split(new char[] {','});
			int intX=5;
			int intY=5;

			if(args.Length>0)
				intY += 5;
			for(int intArgumentNumber=0;intArgumentNumber<args.Length;intArgumentNumber++)
			{
				string[] argument = args[intArgumentNumber].Trim().Split(new char[] {' '});
				if(argument.Length==2)
				{
					string strArgumentName = argument[1];
					string strArgumentType = argument[0];
					string strArgumentValue = "";
					switch(strArgumentType)
					{
						case "System.double":
						case "LSLEditor.SecondLife+Float":
							strArgumentValue = "1.0";
							break;
						case "LSLEditor.integer":
						case "LSLEditor.SecondLife+integer":
						case "System.Int32":
							strArgumentValue = "1";
							break;
						case "LSLEditor.SecondLife+String":
						case "System.String":
							strArgumentValue = "hello";
							break;
						case "LSLEditor.SecondLife+key":
							strArgumentValue = Guid.NewGuid().ToString();
							break;
						case "LSLEditor.SecondLife+rotation":
							strArgumentValue = "<0,0,0,1>";
							break;
						case "LSLEditor.SecondLife+vector":
							strArgumentValue = "<0,0,0>";
							break;
						case "LSLEditor.SecondLife+list":
							strArgumentValue = "";
							break;
						default:
							MessageBox.Show("GroupboxEvent->["+strArgumentType+"]["+strArgumentName+"]");
							strArgumentValue = "unknown";
							break;
					}

					GroupBox groupbox = new GroupBox();
					groupbox.Name = strName+"_"+intArgumentNumber;
					groupbox.Text = strArgumentName;
					groupbox.Location = new Point(5,intY);
					groupbox.Width = this.Width - 10;

					Control control = null;
					if (strName == "listen" && intArgumentNumber==0)
					{
						ComboBox comboBox = new ComboBox();
						comboBox.Text = "";
						control = comboBox;
					}
					else
					{
						TextBox textBox = new TextBox();
						textBox.Text = strArgumentValue;
						control = textBox;
					}
					control.Name = "textBox_" + strName + "_" + intArgumentNumber;
					control.Location = new Point(5, 15);
					groupbox.Controls.Add(control);
					groupbox.Height = 20 + control.Height;
					this.Controls.Add(groupbox);
					intY += groupbox.Height;
				}
				else
				{
					if(strArgs!="")
						MessageBox.Show("Argument must be 'type name' ["+strArgs+"]");
				}
			}

			intY += 5;

			Button button = new Button();
			button.Text = strName;
			button.Width = 130;
			button.Location = new Point(intX,intY);
			button.Click += eventHandler;
			this.Controls.Add(button);
			this.Height = intY + button.Height + 5;
		}

		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// GroupboxTextbox
			// 
			this.Name = "GroupboxTextbox";
			this.Size = new System.Drawing.Size(152, 96);

		}
		#endregion
	}
}
