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
using System.Windows.Forms;

namespace LSLEditor.Editor
{
	public partial class Numbered : UserControl
	{
		public RichTextBox richTextBox1;
		private Brush brush;
		public float LineHeight;

		public Numbered()
		{
			InitializeComponent();

			this.SetStyle(
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint,
				true);

			this.UpdateStyles();

			brush = new SolidBrush(this.ForeColor);

			LineHeight = 0.0F;
		}

		private void updateNumberLabel(PaintEventArgs e)
		{
			if (this.brush == null)
				return;

			if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
				return;

			int delta = 0;
			int firstLine = 0;
			int lastLine = 10;
			Font font = this.Font;
            int selectedLineStart = -1, selectedLineEnd = -1;
			if (this.richTextBox1 == null)
			{
				LineHeight = 16.0F;
			}
			else
			{
                if (richTextBox1.SelectionStart != -1)
                {
                    selectedLineStart = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart);
                    selectedLineEnd = richTextBox1.GetLineFromCharIndex(richTextBox1.SelectionStart + richTextBox1.SelectionLength);
                }
				//we get index of first visible char and number of first visible line
				Point pos = new Point(0, 0);

				int firstIndex = this.richTextBox1.GetCharIndexFromPosition(pos);
				firstLine = this.richTextBox1.GetLineFromCharIndex(firstIndex);

				font = this.richTextBox1.Font;

				if (LineHeight < 0.01)
				{
					if (this.richTextBox1.Lines.Length > 1)
					{
						Point pos1 = this.richTextBox1.GetPositionFromCharIndex(this.richTextBox1.GetFirstCharIndexFromLine(1));
						LineHeight = pos1.Y;
					}
				}

				lastLine = Math.Min(this.richTextBox1.Lines.Length, 2 + firstLine + (int)(this.richTextBox1.ClientRectangle.Height / LineHeight));

				int intCharIndex = this.richTextBox1.GetCharIndexFromPosition(Point.Empty);
				delta = 1 + this.richTextBox1.GetPositionFromCharIndex(intCharIndex).Y % font.Height;
			}

			// here we go
			lastLine = Math.Max(lastLine, 1);

			Graphics g = e.Graphics;
			g.Clear(this.BackColor);
			if(this.richTextBox1==null)
				g.SetClip(new Rectangle(0, 0, this.Width, this.Height));
			else
				g.SetClip(new Rectangle(0, 0, this.Width, this.richTextBox1.ClientRectangle.Height));

			for (int i = firstLine; i < lastLine; i++)
				g.DrawString(string.Format("{0:0###}", i + 1), (i>=selectedLineStart && i<= selectedLineEnd) ? new Font(font,FontStyle.Bold) : font, brush,
					new PointF(0F, delta + (i - firstLine) * LineHeight) );
			//g.DrawLine(new Pen(brush), backBuffer.Width - 1, 0, backBuffer.Width - 1, backBuffer.Height);
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			updateNumberLabel(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			//base.OnPaintBackground(e);
		}

	}
}
