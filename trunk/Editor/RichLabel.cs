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
using System.Globalization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LSLEditor
{
	public partial class RichLabel : UserControl
	{
		private Regex m_regex;

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				base.Text = value;
				this.Invalidate();
			}
		}

		public RichLabel()
		{
			InitializeComponent();

			this.SetStyle(
				ControlStyles.DoubleBuffer |
				ControlStyles.UserPaint |
				ControlStyles.AllPaintingInWmPaint,
				true);

			this.UpdateStyles();

			this.Text = "richLabel1";
			this.BackColor = Color.LightGoldenrodYellow;

			this.m_regex = new Regex(@"
(?:
	<(?:
		(?<startTag>[^>/\s]*)
		(?<attributes> [\s]+ (?<attName>[^=]*) = ""(?<attValue>[^""]*)"")*
	)>
| 
	<[/]* (?<endTag>[^>\s/]*) [/]*>
|
	(?<text>[^<]*) 
)
",
					RegexOptions.IgnorePatternWhitespace
					| RegexOptions.Compiled);

			//this.m_regex = new Regex(@"(?<text>\w*)");
		}

		private void SafePaint(PaintEventArgs e)
		{
			try
			{
				Graphics g = e.Graphics;

				e.Graphics.Clear(this.BackColor);

				Stack<String> fontFace = new Stack<String>();
				Stack<Single> fontSize = new Stack<Single>();
				Stack<Color> fontColor = new Stack<Color>();
				Stack<FontStyle> fontStyle = new Stack<FontStyle>();

				fontFace.Push(this.Font.Name);
				fontSize.Push(this.Font.Size);
				fontStyle.Push(this.Font.Style);
				fontColor.Push(this.ForeColor);

				float fltLineHeight = 0;
				float fltLineHeightMax = 0;
				float fltWidth = 0;
				float fltHeight = 0;

				PointF point = new PointF(this.Margin.Left, this.Margin.Top);

				string strLines = this.Text.Replace("\r", "").Replace("\n", "<br>");
				foreach (Match m in this.m_regex.Matches(strLines))
				{
					string strText = m.Groups["text"].Value.Replace("&lt;", "<").Replace("&gt;", ">");

					switch (m.Groups["startTag"].Value)
					{
						case "font":
							for (int intI = 0; intI < m.Groups["attName"].Captures.Count; intI++)
							{
								string strValue = m.Groups["attValue"].Captures[intI].Value;
								switch (m.Groups["attName"].Captures[intI].Value)
								{
									case "color":
										if (strValue.StartsWith("#"))
										{
											int intColor = 255;
											int.TryParse(strValue.Substring(1), NumberStyles.HexNumber, null, out intColor);
											fontColor.Push(Color.FromArgb(255, Color.FromArgb(intColor)));
										}
										else
										{
											fontColor.Push(Color.FromName(strValue));
										}
										break;
									case "face":
										fontFace.Push(strValue);
										break;
									case "size":
										float fltSize = 10.0F;
										float.TryParse(strValue, out fltSize);
										fontSize.Push(fltSize);
										break;
									default:
										break;
								}
							}
							break;
						case "b":
							fontStyle.Push(fontStyle.Peek() | FontStyle.Bold);
							break;
						case "u":
							fontStyle.Push(fontStyle.Peek() | FontStyle.Underline);
							break;
						case "i":
							fontStyle.Push(fontStyle.Peek() | FontStyle.Italic);
							break;
						case "s":
							fontStyle.Push(fontStyle.Peek() | FontStyle.Strikeout);
							break;
						case "br":
							point = new PointF(this.Margin.Left, point.Y + fltLineHeightMax);
							fltLineHeightMax = fltLineHeight;
							break;
						default:
							break;
					}
					switch (m.Groups["endTag"].Value)
					{
						case "font":
							if (fontColor.Count > 1)
								fontColor.Pop();
							if (fontSize.Count > 1)
								fontSize.Pop();
							if (fontFace.Count > 1)
								fontFace.Pop();
							break;
						case "b":
						case "u":
						case "i":
						case "s":
							if (fontStyle.Count > 1)
								fontStyle.Pop();
							break;
						case "br":
							point = new PointF(this.Margin.Left, point.Y + fltLineHeightMax);
							fltLineHeightMax = fltLineHeight;
							break;
						default:
							break;
					}

					if (strText.Length == 0)
						continue;

					Font fontTmp = new Font(fontFace.Peek(), fontSize.Peek(), fontStyle.Peek());
					Size rect = MeasureTextIncludingSpaces(g, strText, fontTmp); // TextRenderer.MeasureText(strText, fontTmp);
					PointF pointToDraw = new PointF(point.X, point.Y);

					point = new PointF(point.X + rect.Width, point.Y);
					fltWidth = Math.Max(fltWidth, point.X);
					fltHeight = Math.Max(fltHeight, point.Y + rect.Height);
					fltLineHeight = rect.Height;
					fltLineHeightMax = Math.Max(fltLineHeightMax, fltLineHeight);

					Brush brush = new SolidBrush(fontColor.Peek());
					g.DrawString(strText, fontTmp, brush, pointToDraw);
					brush.Dispose();
					fontTmp.Dispose();
				}
				int intWidth = (int)fltWidth + (Margin.Right <<1);
				int intHeight = (int)fltHeight + Margin.Bottom;
				this.Size = new Size(intWidth, intHeight);

				//System.Drawing.Drawing2D.GraphicsPath path = Editor.RoundCorners.RoundedRectangle(new Rectangle(this.Location, this.Size), 10);
				//g.DrawPath(new Pen(Color.Black,2F), path);
				//this.Region = Editor.RoundCorners.RoundedRegion(this.Size, 4);
			}
			catch
			{
			}
		}

		public static SizeF MeasureTextVisible(Graphics graphics, string text, Font font)
		{
			StringFormat format = new StringFormat();
			RectangleF rect = new RectangleF(0, 0, 4096, 1000);
			CharacterRange[] ranges = { new CharacterRange(0, text.Length) };
			format.SetMeasurableCharacterRanges(ranges);
			Region[] regions = graphics.MeasureCharacterRanges(text, font, rect, format);
			rect = regions[0].GetBounds(graphics);
			return new SizeF(rect.Width, rect.Height);
		}

		public static Size MeasureTextIncludingSpaces(Graphics graphics, string text, Font font)
		{
			SizeF sizePostfix = MeasureTextVisible(graphics, "|", font);
			SizeF size = MeasureTextVisible(graphics, text + "|", font);
			return new Size((int)(size.Width - sizePostfix.Width + 1), (int)(size.Height + 1));
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			SafePaint(e);
		}

		protected override void OnPaintBackground(PaintEventArgs e)
		{
			// base.OnPaintBackground(e);
		}

	}
}
