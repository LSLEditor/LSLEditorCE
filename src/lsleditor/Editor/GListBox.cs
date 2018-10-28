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
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace LSLEditor
{
	public partial class GListBox : ListBox
	{
		public ImageList ImageList;

		public GListBox(IContainer container)
		{
			container.Add(this);

			this.InitializeComponent();

			// Set owner draw mode
			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.ImageList = new ImageList();
		}

		public GListBox()
		{
			this.InitializeComponent();

			// Set owner draw mode
			this.DrawMode = DrawMode.OwnerDrawFixed;
			this.ImageList = new ImageList();
		}

		protected override void OnDrawItem(DrawItemEventArgs e)
		{
			try {
				GListBoxItem item;
				var bounds = new Rectangle(e.Bounds.X + e.Bounds.Height, e.Bounds.Y, e.Bounds.Width - e.Bounds.Height - 1, e.Bounds.Height);
				item = (GListBoxItem)this.Items[e.Index];
				if (item.ImageIndex != -1) {
					e.Graphics.FillRectangle(new SolidBrush(this.BackColor), bounds);
					if ((e.State & DrawItemState.Selected) == DrawItemState.Selected) {
						e.Graphics.FillRectangle(SystemBrushes.Highlight, bounds);
					}

					e.Graphics.DrawImage(this.ImageList.Images[item.ImageIndex], bounds.Left - bounds.Height, bounds.Top, bounds.Height, bounds.Height);
					e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
						bounds.Left, bounds.Top);
				} else {
					e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(e.ForeColor),
						bounds.Left, bounds.Top);
				}
			} catch {
				e.DrawBackground();
				e.DrawFocusRectangle();
				if (e.Index != -1) {
					try {
						e.Graphics.DrawString(this.Items[e.Index].ToString(), e.Font,
							new SolidBrush(e.ForeColor), e.Bounds.Left, e.Bounds.Top);
					} catch {
					}
				} else {
					e.Graphics.DrawString(this.Text, e.Font, new SolidBrush(e.ForeColor),
						e.Bounds.Left, e.Bounds.Top);
				}
			}
			base.OnDrawItem(e);
		}
	}//End of GListBox class

	// GListBoxItem class 
	public class GListBoxItem
	{
		// properties 
		public string Text { get; set; }
		public int ImageIndex { get; set; }

		//constructor
		public GListBoxItem(string text, int index)
		{
			this.Text = text;
			this.ImageIndex = index;
		}

		public GListBoxItem(string text) : this(text, -1) { }

		public GListBoxItem() : this("") { }

		public override string ToString()
		{
			return this.Text;
		}
	}//End of GListBoxItem class
}
