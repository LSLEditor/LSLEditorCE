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
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace LSLEditor
{
	public partial class TooltipWindow : Form
	{
		public string Wild;
		public int XOffset = 0;
		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.richLabel1.Text = value;
			}
		}

		public TooltipWindow(Form parent)
		{
			InitializeComponent();

			this.Wild = "";

			this.Owner = parent;
			this.richLabel1.BorderStyle = BorderStyle.FixedSingle;
			//this.richLabel1.BackColor = Color.LightGoldenrodYellow;

			this.BackColor = Color.LightGoldenrodYellow;
			//this.richLabel1.BorderStyle = BorderStyle.None;

			this.richLabel1.Tag = "";
			this.richLabel1.Text = "";
		}

		public void SetPosition(Rectangle rect, SyntaxRichTextBox syntaxRichTextBox)
		{
			//Rectangle rect = Screen.PrimaryScreen.WorkingArea;
			Point p = syntaxRichTextBox.GetPositionFromCharIndex(syntaxRichTextBox.SelectionStart);

			p = new Point(p.X - 20 + this.XOffset + 2 , p.Y + 2 + syntaxRichTextBox.LineHeight);

			Rectangle client = Screen.PrimaryScreen.WorkingArea; //  syntaxRichTextBox.ClientRectangle;
			if (p.X < client.Left || p.Y < client.Top || p.X > client.Width || p.Y > client.Height)
			{
				this.Visible = false;
				return;
			}

			Point screen = syntaxRichTextBox.PointToScreen(p);

			//if ((screen.Y + this.Height) > rect.Height)
			//	screen = RichTextBox.PointToScreen(new Point(p.X - 20 + this.XOffset, p.Y - this.Height));

			if (screen.Y > rect.Bottom)
			{
				this.Visible = false;
				return;
				//screen.Y = rect.Bottom;
			}

			if (screen.X > rect.Right)
			{
				this.Visible = false;
				return;
				//screen.X = rect.Right;
			}

			if (screen.X < rect.Left)
			{
				this.Visible = false;
				return;
				//screen.X = rect.Left;
			}

			if ((screen.Y) < rect.Top)
			{
				this.Visible = false;
				return;
				//screen.Y = rect.Top;
			} 
			
			this.Location = screen;
		}

		public void SetPosition(SyntaxRichTextBox syntaxRichTextBox, Point p)
		{
			Rectangle rect = Screen.PrimaryScreen.WorkingArea;
			Point screen = syntaxRichTextBox.PointToScreen(new Point(p.X - 20 , p.Y + syntaxRichTextBox.LineHeight));
			if ((screen.Y + this.Height) > rect.Height)
				screen = syntaxRichTextBox.PointToScreen(new Point(p.X - 20, p.Y - this.Height));

			if ((screen.X + this.Width) > rect.Width)
				screen.X = rect.Width - this.Width;

			if (screen.X < rect.Left)
				screen.X = rect.Left;

			if (screen.Y < rect.Top)
				screen.Y = rect.Top;

			this.Location = screen;
		}

		private void richLabel1_Resize(object sender, EventArgs e)
		{
			this.Size = this.richLabel1.Size;

		}

	}
}