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
using System.Windows.Forms.VisualStyles;

namespace System.Windows.Forms
{
	public partial class TabControlExtended : System.Windows.Forms.TabControl
	{
		private int HoverIndex;
		private bool Extended;

		public event EventHandler OnTabClose;

		public TabControlExtended()
		{
			InitializeComponent();
		}

		public TabControlExtended(IContainer container)
		{
			InitializeComponent();

			container.Add(this);
		}

		public void SetDrawMode()
		{
			try
			{
				HoverIndex = 0;
				VisualStyleRenderer render = new VisualStyleRenderer(VisualStyleElement.Tab.Pane.Normal);
				this.DrawMode = TabDrawMode.OwnerDrawFixed;
				this.SetStyle(ControlStyles.UserPaint, true);
				this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
				this.SetStyle(ControlStyles.DoubleBuffer, true);
				this.SetStyle(ControlStyles.ResizeRedraw, true);
				this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
				this.Extended = true;
			}
			catch
			{
				this.Extended = false;
				this.DrawMode = TabDrawMode.Normal;
			}
		}

/*
protected override bool ProcessMnemonic(char charCode)
{
	foreach (TabPage p in this.TabPages)
	{
		if (Control.IsMnemonic(charCode, p.Text))
		{
			this.SelectedTab = p;
			this.Focus();
			return true;
		}
	}
	return false;
}
*/

		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);
			if (!this.Extended)
				return;
			MyPaint(e);
		}

		protected override void OnSelectedIndexChanged(EventArgs e)
		{
			this.HoverIndex = -1;
			base.OnSelectedIndexChanged(e);
		}

		private void MyPaint(PaintEventArgs e)
		{
			if (!this.Visible)
				return;

			Graphics g = e.Graphics;

			Rectangle displayRectangle = this.DisplayRectangle;

			Size borderSize = SystemInformation.Border3DSize;
			displayRectangle.Inflate(borderSize.Width << 1, borderSize.Height << 1);

			VisualStyleRenderer render = new VisualStyleRenderer(VisualStyleElement.Tab.Pane.Normal);

			render.DrawBackground(g, displayRectangle);

			for (int intI = 0; intI < this.TabCount; intI++)
				if (intI != this.SelectedIndex)
					DrawTab(g, intI);
			if (this.SelectedIndex >= 0)
				DrawTab(g, this.SelectedIndex);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			base.OnMouseMove(e);
			if (!Extended)
				return;

			Point p = e.Location;
			for (int intI = 0; intI < this.TabCount; intI++)
			{
				Rectangle rectangle = GetTabRect(intI);
				if (rectangle.Contains(p))
				{
					HoverIndex = intI;
					this.Invalidate();
				}
			}
		}

		protected override void OnMouseLeave(EventArgs e)
		{
			base.OnMouseLeave(e);
			if (!Extended)
				return;

			HoverIndex = this.SelectedIndex;
			this.Invalidate();
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);
			if (!Extended)
				return;

			Point p = e.Location;
			Rectangle rectangle = this.GetTabRect(this.SelectedIndex);
			if (rectangle.Contains(p))
			{
				Rectangle closeImage = new Rectangle(rectangle.Right - 15 - 5, 5, 15, 15);
				if (closeImage.Contains(p))
				{
					if (OnTabClose != null)
						OnTabClose(this.SelectedIndex, new EventArgs());
					else
						this.TabPages.RemoveAt(this.SelectedIndex);
				}
			}
		}

		private void DrawTab(Graphics g, int intIndex)
		{
			Font font;
			Bitmap bitmap;

			Rectangle recBounds = this.GetTabRect(intIndex);
			RectangleF tabTextArea = (RectangleF)this.GetTabRect(intIndex);

			TabPage tabPage = this.TabPages[intIndex];

			Size borderSize = SystemInformation.Border3DSize;

			VisualStyleRenderer render;
			if (this.SelectedIndex == intIndex)
			{
				font = new Font(this.Font, FontStyle.Bold);
				Point p = this.PointToClient(Control.MousePosition);
				Rectangle closeImage = new Rectangle(recBounds.Right - 15 - 5, 5, 15, 15);
				if (closeImage.Contains(p))
					bitmap = new Bitmap(Type.GetType("LSLEditor.LSLEditorForm"), "Images.Close-Active.gif");
				else
					bitmap = new Bitmap(Type.GetType("LSLEditor.LSLEditorForm"), "Images.Close-Inactive.gif");
				recBounds.X -= borderSize.Width;
				recBounds.Y -= borderSize.Height;
				recBounds.Width += borderSize.Width << 1;
				recBounds.Height += borderSize.Height;
				render = new VisualStyleRenderer(VisualStyleElement.Tab.TabItem.Pressed);
				Rectangle clipper = new Rectangle(recBounds.X, recBounds.Y, recBounds.Width, recBounds.Height - 1);
				render.DrawBackground(g, recBounds, clipper);
			}
			else
			{
				font = new Font(this.Font, FontStyle.Regular);
				if (this.HoverIndex == intIndex)
				{
					render = new VisualStyleRenderer(VisualStyleElement.Tab.TopTabItem.Hot);
					bitmap = new Bitmap(Type.GetType("LSLEditor.LSLEditorForm"), "Images.Close-Active.gif");
				}
				else
				{
					render = new VisualStyleRenderer(VisualStyleElement.Tab.TabItem.Normal);
					bitmap = new Bitmap(Type.GetType("LSLEditor.LSLEditorForm"), "Images.Close-Disabled.gif");
				}
				recBounds.Height -= borderSize.Height;
				render.DrawBackground(g, recBounds);
			}

			SolidBrush br = new SolidBrush(tabPage.ForeColor);
			//Console.WriteLine("["+tabPage.Text+"]");
			g.DrawString(tabPage.Text, font, br, tabTextArea.Left + 2, tabTextArea.Top + 3);
			font.Dispose();
			g.DrawImage(bitmap, new Point((int)tabTextArea.Right - bitmap.Width - 5, 5));
		}

	}
}
