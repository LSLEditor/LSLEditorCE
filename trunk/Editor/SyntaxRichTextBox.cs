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
using System.Xml;
using System.Text;
using System.Drawing;

using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Printing;
using System.Text.RegularExpressions;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace LSLEditor
{
	public delegate void IsDirtyHandler(object sender, EventArgs e);

	public class SyntaxRichTextBox : RichTextBox
	{
		private const int WM_SETREDRAW = 0x000B;
		private const int WM_USER = 0x400;
		private const int EM_GETEVENTMASK = (WM_USER + 59);
		private const int EM_SETEVENTMASK = (WM_USER + 69);

		// Scroll position
		[DllImport("user32.dll")]
		private static extern int SendMessage(IntPtr hwndLock, Int32 wMsg, Int32 wParam, ref Point pt);

		// Anti flicker
		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

		// Tabs
		[DllImport("User32", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);

		private bool m_Dirty;

		private bool blnEscape;

		private bool blnInsert;

		// for tooltipping
		private bool m_blnToolTipping;
		private System.Windows.Forms.Timer timer1;
		private GListBoxWindow GListBoxWindow;
		private TooltipWindow TooltipMouse;
		private TooltipWindow TooltipKeyboard;
		private TooltipWindow TooltipListBox;

		private Point OldMouseLocation;

		private int intKeyWordLength;

		// needed for flicker-free updates
		private int intUpdate;
		private IntPtr eventMask;

		private int intOldLines;

		// colored words
		private KeyWords keyWords;

		private Helpers.CodeCompletion codeCompletion;

		// after clicking on error
		private int HighLightLine;

		// bracket highlighting
		private List<int> HighLightList;
        public EditForm p;
		public float CharWidth;
		public int LineHeight;

		// undo
		private struct UndoElement
		{
			public int SelectionStart;
			public int SelectionLength;
			public string SelectedText;
			public string RedoText;
			public UndoElement(int SelectionStart, string SelectedText, int SelectionLength, string RedoText)
			{
				this.SelectionStart = SelectionStart;
				this.SelectionLength = SelectionLength;
				this.SelectedText = SelectedText;
				this.RedoText = RedoText;
			}
		}
		private Stack<UndoElement> UndoStack;
		private Stack<UndoElement> RedoStack;

		#region printing
		//Convert the unit used by the .NET framework (1/100 inch) 
		//and the unit used by Win32 API calls (twips 1/1440 inch)
		private const double anInch = 14.4;

		[StructLayout(LayoutKind.Sequential)]
		private struct RECT
		{
			public int Left;
			public int Top;
			public int Right;
			public int Bottom;
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct CHARRANGE
		{
			public int cpMin;	//First character of range (0 for start of doc)
			public int cpMax;	//Last character of range (-1 for end of doc)
		}

		[StructLayout(LayoutKind.Sequential)]
		private struct FORMATRANGE
		{
			public IntPtr hdc;			//Actual DC to draw on
			public IntPtr hdcTarget;	//Target DC for determining text formatting
			public RECT rc;				//Region of the DC to draw to (in twips)
			public RECT rcPage;			//Region of the whole DC (page size) (in twips)
			public CHARRANGE chrg;		//Range of text to draw (see earlier declaration)
		}

		private const int EM_FORMATRANGE = WM_USER + 57;

		[DllImport("user32", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

		// Render the contents of the RichTextBox for printing
		// Return the last character printed + 1 (printing start from this point for next page)
		public int Print(int charFrom, int charTo, PrintPageEventArgs e)
		{
			//Calculate the area to render and print
			RECT rectToPrint;
			rectToPrint.Top = (int)(e.MarginBounds.Top * anInch);
			rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * anInch);
			rectToPrint.Left = (int)(e.MarginBounds.Left * anInch);
			rectToPrint.Right = (int)(e.MarginBounds.Right * anInch);

			//Calculate the size of the page
			RECT rectPage;
			rectPage.Top = (int)(e.PageBounds.Top * anInch);
			rectPage.Bottom = (int)(e.PageBounds.Bottom * anInch);
			rectPage.Left = (int)(e.PageBounds.Left * anInch);
			rectPage.Right = (int)(e.PageBounds.Right * anInch);

			IntPtr hdc = e.Graphics.GetHdc();

			FORMATRANGE fmtRange;
			fmtRange.chrg.cpMax = charTo;	//Indicate character from to character to 
			fmtRange.chrg.cpMin = charFrom;
			fmtRange.hdc = hdc;				//Use the same DC for measuring and rendering
			fmtRange.hdcTarget = hdc;		//Point at printer hDC
			fmtRange.rc = rectToPrint;		//Indicate the area on page to print
			fmtRange.rcPage = rectPage;		//Indicate size of page

			IntPtr wparam = new IntPtr(1);

			//Get the pointer to the FORMATRANGE structure in memory
			IntPtr lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
			Marshal.StructureToPtr(fmtRange, lparam, false);

			//Send the rendered data for printing 
			IntPtr res = SendMessage(Handle, EM_FORMATRANGE, wparam, lparam);

			//Free the block of memory allocated
			Marshal.FreeCoTaskMem(lparam);

			//Release the device context handle obtained by a previous call
			e.Graphics.ReleaseHdc(hdc);

			//Return last + 1 character printer
			return res.ToInt32();
		}
		#endregion

		public event IsDirtyHandler OnDirtyChanged;
		public event EventHandler OnPaintNumbers;

		public delegate void CursorPositionChangedHandler(object sender, CursorPositionEventArgs e);
		public event CursorPositionChangedHandler OnCursorPositionChanged;
		public class CursorPositionEventArgs : EventArgs
		{
			public int Line;
			public int Column;
			public int Char;
			public int Total;
			public bool Insert;
			public bool Caps;
			public CursorPositionEventArgs(int intLine, int intColumn, int intChar, int intTotal, bool blnInsert, bool blnCaps)
			{
				this.Line = intLine;
				this.Column = intColumn;
				this.Char = intChar;
				this.Total = intTotal;
				this.Insert = blnInsert;
				this.Caps = blnCaps;
			}
		}

		public SyntaxRichTextBox()
		{
			this.intUpdate = 0;
			this.eventMask = IntPtr.Zero;
			this.m_blnToolTipping = false;

			this.intKeyWordLength = 0;

			this.AcceptsTab = true;
			this.BorderStyle = BorderStyle.None;
			this.DetectUrls = false;
			this.Dirty = false;

			this.ScrollBars = RichTextBoxScrollBars.ForcedBoth;
			this.WordWrap = false;

			this.FontChanged += new EventHandler(SyntaxRichTextBox_FontChanged);

			this.MouseMove += new MouseEventHandler(SyntaxRichTextBox_MouseMove);

			this.VScroll += new EventHandler(SyntaxRichTextBox_Position);
			this.HScroll += new EventHandler(SyntaxRichTextBox_Position);

			this.HighLightLine = -1;
			this.HighLightList = new List<int>();

			this.UndoStack = new Stack<UndoElement>();
			this.RedoStack = new Stack<UndoElement>();

			this.blnEscape = false;

			this.blnInsert = true;

			this.intOldLines = -1;

			this.CharWidth = 10; //TODO
			this.LineHeight = 16;

			this.OldMouseLocation = Point.Empty;

			this.codeCompletion = new Helpers.CodeCompletion();

			this.HideSelection = false;

			// are these of any use?
			//this.DoubleBuffered = true;
			//SetStyle(ControlStyles.AllPaintingInWmPaint, true);
			//SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
			//SetStyle(ControlStyles.Opaque, true);
		}
        public void setEditform(EditForm ed)
        {
            p = ed;
        }
		public void Init(LSLEditorForm parent, XmlDocument xml)
		{
			if (parent != null)
			{
				this.ToolTipping = true;
				this.GListBoxWindow = parent.GListBoxWindow;
				this.TooltipMouse = parent.TooltipMouse;
				this.TooltipKeyboard = parent.TooltipKeyboard;
				this.TooltipListBox = parent.TooltipListBox;
             //   p = this.Parent as EditForm;
               // p = parent; moved to edit form
			}

			string ColorScheme = "color";
			if (this.ToolTipping)
			{
				if (Properties.Settings.Default.SLColorScheme)
				{
					ColorScheme = "sl" + ColorScheme;
				}

				XmlNode BackgroundColorNode = xml.SelectSingleNode("/Conf");
				if (BackgroundColorNode != null)
				{
					Color bgColor = Color.FromArgb(255, Color.FromArgb(int.Parse(BackgroundColorNode.Attributes[ColorScheme].InnerText.Replace("#", ""), System.Globalization.NumberStyles.HexNumber)));
					this.BackColor = bgColor;
				}
			}
			keyWords = new KeyWords(ColorScheme, xml);
		}

		private void MeasureFont()
		{
			Size size = TextRenderer.MeasureText("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", this.Font);
			CharWidth = (int)(size.Width / 50.0F);
			LineHeight = size.Height;
		}

		void SyntaxRichTextBox_FontChanged(object sender, EventArgs e)
		{
			this.SelectAll();

			TabStops();

			// recolor all
			ColorLine(0, this.Text.Length);
		}

		private void TabStops()
		{
			MeasureFont();

			const int EM_SETTABSTOPS = 0xCB;

			int intNumberOfChars = Properties.Settings.Default.TabStops;
			int[] tabs = new int[30]; // TODO
			float fltTabWidth = intNumberOfChars * CharWidth;

			float totalWidth = 0.0F;
			for (int intI = 0; intI < tabs.Length; intI++)
			{
				totalWidth += fltTabWidth;
				tabs[intI] = (int)totalWidth;
			}

			SendMessage(this.Handle, EM_SETTABSTOPS, 0, null);
			SendMessage(this.Handle, EM_SETTABSTOPS, tabs.Length, tabs);

			this.SelectionTabs = tabs;
		}

		private void PaintNumbers()
		{
			if (OnPaintNumbers != null)
				OnPaintNumbers(null, null);
		}

		protected override void OnVScroll(EventArgs e)
		{
			base.OnVScroll(e);
			PaintNumbers();
		}


		private Point RTBScrollPos
		{
			get
			{
				const int EM_GETSCROLLPOS = 0x0400 + 221;
				Point pt = new Point();

				SendMessage(this.Handle, EM_GETSCROLLPOS, 0, ref pt);
				return pt;
			}
			set
			{
				const int EM_SETSCROLLPOS = 0x0400 + 222;

				SendMessage(this.Handle, EM_SETSCROLLPOS, 0, ref value);
			}
		}


		public bool Dirty
		{
			get
			{
				return m_Dirty;
			}
			set
			{
				if (this.m_Dirty == value)
					return;
				this.m_Dirty = value;

				if (OnDirtyChanged == null)
					return;

				OnDirtyChanged(this, new EventArgs());
			}
		}

		private void SyntaxRichTextBox_Position(object sender, EventArgs e)
		{
			SetPosition(Screen.GetWorkingArea(this));
		}

		private void SyntaxRichTextBox_MouseMove(object sender, MouseEventArgs e)
		{
			if (!ToolTipping)
				return;

			if (this.timer1 == null)
				return;

			this.timer1.Stop();
			if (OldMouseLocation != e.Location)
			{
				OldMouseLocation = e.Location;
				this.timer1.Start();
			}
		}

		private int GetLine()
		{
			return 1 + this.GetLineFromCharIndex(this.SelectionStart);
		}

		private int GetChar()
		{
			return 1 + this.SelectionStart - this.GetFirstCharIndexOfCurrentLine();
		}

		private int GetColumn()
		{
			Point p1 = this.GetPositionFromCharIndex(this.GetFirstCharIndexOfCurrentLine());
			Point p2 = this.GetPositionFromCharIndex(this.SelectionStart);
			int intColumn = (int)((p2.X - p1.X) / this.CharWidth);
			return 1+ intColumn;
		}

		private int GetTotal()
		{
			return this.Text.Length;
		}

		public void Goto(int Line, int Char)
		{
			BeginUpdate();
			LineDownlighting();

			try
			{
				int intLine = Line - 1;
				if (intLine < 0)
					intLine = 0;
				if (intLine >= this.Lines.Length)
					intLine = this.Lines.Length - 1;

				if (intLine >= 0 && intLine < this.Lines.Length)
				{
					int intLength = this.Lines[intLine].Length;
					int intStart = GetFirstCharIndexFromLine(intLine);
					int intIndex = intStart + Char - 1;
					HighLightLine = intLine;
					this.SelectionStart = intStart;
					this.SelectionLength = intLength;
					this.SelectionBackColor = Color.LightBlue;
					this.SelectionLength = 0;
					this.SelectionStart = intIndex;
				}
			}
			catch
			{
			}

			EndUpdate(false);
			this.Focus();
		}

		protected override void OnSelectionChanged(EventArgs e)
		{
			base.OnSelectionChanged(e);
			if (!IsUpdating)
				CursorPositionChanged(); // show cursor position
		}

		private bool IsUpdating
		{
			get
			{
				return (intUpdate > 0);
			}
		}

		Point backupScrollPoint;
		private void BeginUpdate()
		{
			++intUpdate;
			if (intUpdate > 1) // once is enough
				return;

			backupScrollPoint = this.RTBScrollPos;
			// Disable redrawing
			SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

			// Disable generating events
			eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
		}

		private void EndUpdate(bool update_outline)
		{
            if(update_outline) doOutline();

			intUpdate = Math.Max(0, intUpdate - 1);

			if (intUpdate > 0) // only the last one
				return;

			// Enable events
			SendMessage(this.Handle, EM_SETEVENTMASK, 0, eventMask);

			// Enable redrawing
			SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

			// also draw when linenumbers are shifted
			if (intOldLines != this.Lines.Length || backupScrollPoint != this.RTBScrollPos)
			{
				intOldLines = this.Lines.Length;
				PaintNumbers();
			}
			Invalidate();
		}

		private bool IsAlpha(char chrC)
		{
			return (char.IsLetterOrDigit(chrC) || chrC == '_');
		}

		// used for F1 key, help and tooltipping, use Mouse of begin-selection
		public string GetCurrentKeyWord(bool blnUserMousePosition, out Point PositionFirstCharacter)
		{
			PositionFirstCharacter = Point.Empty;

			int intIndex = this.SelectionStart;
			if (blnUserMousePosition)
			{
				Point mp = this.PointToClient(Control.MousePosition);
				intIndex = this.GetCharIndexFromPosition(mp);

				// check if mouse is in region of index
				Point mpControle = this.GetPositionFromCharIndex(intIndex);
				if (Math.Abs(mp.X - mpControle.X) > 5 || Math.Abs(mp.Y - mpControle.Y) > this.LineHeight)
					return "";
			}

			int intLineNr = this.GetLineFromCharIndex(intIndex);
			int intFirstChar = this.GetFirstCharIndexFromLine(intLineNr);
			int intColumn = intIndex - intFirstChar;

			string strLine = this.Lines[intLineNr];

			int intStart = intColumn;
			while ((intStart > 0) && (IsAlpha(strLine[intStart - 1])))
				intStart--;

			// 22 jun 2007
			PositionFirstCharacter = this.GetPositionFromCharIndex(intFirstChar + intStart + 2);

			int intEnd = intColumn;
			while ((intEnd < strLine.Length) && (IsAlpha(strLine[intEnd])))
				intEnd++;

			// Calculate the length of the keyword.
			int intLength = intEnd - intStart;

			// return the keyword
			return strLine.Substring(intStart, intLength);
		}

		private string GetCurrentFunction(out int Argument, out bool DoWild)
		{
			DoWild = false;
			Argument = 0;
			int intIndex = this.SelectionStart;
			int intLineNumber = this.GetLineFromCharIndex(intIndex);
			int intStartChar = this.GetFirstCharIndexFromLine(intLineNumber);

			int intColumn = intIndex - intStartChar;

			string strLine = this.Lines[intLineNumber];

			// Find the end of the current function
			int intEnd = intColumn - 1;
			while (intEnd > 0 &&
				strLine[intEnd] != '(' &&
				strLine[intEnd] != ')' &&
				strLine[intEnd] != '\n')
			{
				if (strLine[intEnd] == ',')
				{
					Argument++;
					if (Argument == 1)
						DoWild = (intColumn - intEnd) == 1;
				}
				intEnd--;
			}
			if (intEnd <= 0)
				return "";

			if (strLine[intEnd] != '(')
				return "";

			if (Argument == 0)
				DoWild = (intColumn - intEnd) == 1;

			intEnd--;

			// Find the begin of the current function.
			int intStart = intEnd;
			while ((intStart > 0) && (IsAlpha(strLine[intStart])))
				intStart--;

			// Calculate the length of the function.
			int intLength = intEnd - intStart;

			// return the Function name
			return strLine.Substring(intStart + 1, intLength);
		}

		private string GetNewWhiteSpace(int intOffset)
		{
			int intIndex = this.GetLineFromCharIndex(this.SelectionStart);
			return AutoFormatter.GetNewWhiteSpace(this.Lines, intIndex + intOffset);
		}

		private void AutoFormatFromLineToLine(int intLineStart, int intLineEnd)
		{
			BeginUpdate();
			int intStart = this.GetFirstCharIndexFromLine(intLineStart);
			int intLength = this.GetFirstCharIndexFromLine(intLineEnd) +
				this.Lines[intLineEnd].Length + 1 - intStart;
			this.SelectionStart = intStart;
			this.SelectionLength = intLength;
			AutoFormat(true);
			EndUpdate(true);
		}

		private void ProcessEnter()
		{
			BeginUpdate();
			this.ColoredText = "\n";
			if (ToolTipping)
			{
				if (Properties.Settings.Default.Indent)
				{
					if (Properties.Settings.Default.IndentFullAuto)
					{
						int intLine = this.GetLineFromCharIndex(this.SelectionStart);
						AutoFormatFromLineToLine(intLine - 1, intLine - 1);
						this.ColoredText = GetNewWhiteSpace(-1);
					}
					else if (Properties.Settings.Default.IndentCursorPlacement)
					{
						this.ColoredText = GetNewWhiteSpace(-1);
					}
				}
			}
			EndUpdate(true);
		}

		private void SmartIndenting()
		{
			int intFirstLine, intC;
			int intLastLine = this.GetLineFromCharIndex(this.SelectionStart);
			int intColumn = this.SelectionStart - this.GetFirstCharIndexFromLine(intLastLine);

			intColumn--; // because we are at position '}'

			if (MatchingBracket(intLastLine, intColumn, '}', '{', -1, out intFirstLine, out intC))
			{
				if ((intLastLine - intFirstLine) >= 1)
					AutoFormatFromLineToLine(intFirstLine + 1, intLastLine);
			}
		}

		private void ProcessBraces(char keyChar)
		{
			BeginUpdate();
			int intStart = this.SelectionStart;

			this.ColoredText = keyChar.ToString();

			if (Properties.Settings.Default.Indent)
			{
				int intLength = this.Text.Length;

				if (Properties.Settings.Default.IndentFullAuto)
				{
					if (keyChar == '}')
					{
						SmartIndenting();
					}
					else
					{
						int intCurrentLine = this.GetLineFromCharIndex(this.SelectionStart);
						AutoFormatFromLineToLine(intCurrentLine, intCurrentLine);
					}
				}

				int intDelta = 1 + this.Text.Length - intLength;

				this.SelectionStart = intStart + intDelta;

				BracketHighlichting();
			}

			EndUpdate(false);
		}

		public void Delete()
		{
			BeginUpdate();
			if (this.SelectionLength == 0)
				this.SelectionLength = 1;
			this.ColoredText = "";
			EndUpdate(true);
			OnTextChanged(null);
		}

		public new void Cut()
		{
			BeginUpdate();
			this.Copy();
			this.ColoredText = "";
			EndUpdate(true);
			OnTextChanged(null);
		}


		private void SaveUndo(UndoElement el)
		{
			if (RedoStack.Count > 0)
				RedoStack.Clear();
			UndoStack.Push(el);
		}

		public new void Redo()
		{
			if (RedoStack.Count == 0)
				return;

			MakeAllInvis();

			BeginUpdate();

			this.Dirty = true;

			UndoElement el = RedoStack.Pop();

			this.SelectionStart = el.SelectionStart;
			this.SelectionLength = el.SelectedText.Length;
			this.SelectedText = el.RedoText;

			el.RedoText = null; // dont need it anymore
			UndoStack.Push(el);

			int intStartLine = this.GetLineFromCharIndex(el.SelectionStart);
			int intStart = this.GetFirstCharIndexFromLine(intStartLine);

			int intStopLine = this.GetLineFromCharIndex(el.SelectionStart + el.SelectedText.Length);
			if (intStopLine < this.Lines.Length)
			{
				int intLengthStopLine = this.Lines[intStopLine].Length;
				int intStop = this.GetFirstCharIndexFromLine(intStopLine) + intLengthStopLine;

				ColorLine(intStart, intStop - intStart);
			}

			EndUpdate(true);
			this.Focus();
		}

		public new void Undo()
		{
			if (UndoStack.Count == 0)
				return;

			MakeAllInvis();

			BeginUpdate();

			UndoElement el = UndoStack.Pop();

			this.Dirty = (UndoStack.Count != 0);

			this.SelectionStart = el.SelectionStart;
			this.SelectionLength = el.SelectionLength;
			el.RedoText = this.SelectedText; // save redo text!!!!
			this.SelectedText = el.SelectedText;

			RedoStack.Push(el);

			int intStartLine = this.GetLineFromCharIndex(el.SelectionStart);
			int intStart = this.GetFirstCharIndexFromLine(intStartLine);

			int intStopLine = this.GetLineFromCharIndex(el.SelectionStart + el.SelectedText.Length);
			if (intStopLine < this.Lines.Length)
			{
				int intLengthStopLine = this.Lines[intStopLine].Length;
				int intStop = this.GetFirstCharIndexFromLine(intStopLine) + intLengthStopLine;

				ColorLine(intStart, intStop - intStart);
			}

			EndUpdate(true);
			this.Focus();
		}

		public void ToClipBoard()
		{
			bool backup = Properties.Settings.Default.SL4SpacesIndent;
			Properties.Settings.Default.SL4SpacesIndent = true;
			string strFormattedText = AutoFormatter.ApplyFormatting(0, this.Text);
			Properties.Settings.Default.SL4SpacesIndent = backup;

			try
			{
				Clipboard.SetDataObject(strFormattedText, true);
			}
			catch(Exception exception)
			{
				// error
				Console.WriteLine(exception.Message);
			}
		}

		public void ReplaceSelectedText(string strReplacement)
		{
			this.blnEscape = true; // prevent windows popping up
			this.ColoredText = strReplacement;
		}

		public void MakeAllInvis()
		{
			if (!ToolTipping)
				return;
			this.TooltipKeyboard.Visible = false;
			this.TooltipListBox.Visible = false;
			this.TooltipMouse.Visible = false;
			this.GListBoxWindow.Visible = false;
			this.Focus();
		}

		private void LineDownlighting()
		{
			if (this.HighLightLine < 0)
				return;

			// doof line
			BeginUpdate();
			int intBackupSelectionStart = this.SelectionStart;
			int intBackupSelectionLength = this.SelectionLength;
			this.SelectionStart = this.GetFirstCharIndexFromLine(this.HighLightLine);
			this.SelectionLength = this.Lines[this.HighLightLine].Length;
			this.SelectionBackColor = this.BackColor;
			this.SelectionStart = intBackupSelectionStart;
			this.SelectionLength = intBackupSelectionLength;
			this.HighLightLine = -1;
			EndUpdate(false);
		}

		private void CursorPositionChanged()
		{
			BracketHighlichting();

			LineDownlighting();

			if (OnCursorPositionChanged != null)
				OnCursorPositionChanged(this, new CursorPositionEventArgs(this.GetLine(), this.GetColumn(), this.GetChar(), this.GetTotal(), this.blnInsert, Control.IsKeyLocked(Keys.CapsLock)));
		}

		public new void Paste()
		{
			ResetHighlighting();

			// First try with Unicode
			if (Clipboard.GetDataObject().GetDataPresent(DataFormats.UnicodeText, true))
			{
				string strTextToPaste = Clipboard.GetDataObject().GetData(DataFormats.UnicodeText, true).ToString().Replace("\r", "");
				this.ColoredText = strTextToPaste;
			} // failing that try ANSI text.
			else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text, true))
			{
				string strTextToPaste = Clipboard.GetDataObject().GetData(DataFormats.Text, true).ToString().Replace("\r", "");
				this.ColoredText = strTextToPaste;
			}
		}

		private void GoEnd(bool blnShift)
		{
			if (this.Lines.Length == 0)
				return;
			int intLast = this.SelectionStart + this.SelectionLength;
			int intLine = this.GetLineFromCharIndex(intLast);
			int intColumn = intLast - this.GetFirstCharIndexFromLine(intLine);
			string strLine = this.Lines[intLine];
			int intAdd = strLine.Length - intColumn;
			if (blnShift)
				this.SelectionLength += intAdd;
			else
			{
				this.SelectionStart = intLast+intAdd;
				this.SelectionLength = 0;
			}
		}

		private void GoHome(bool blnShift)
		{
			if (this.Lines.Length == 0)
				return;
			int intLine = this.GetLineFromCharIndex(this.SelectionStart);
			string strLine = this.Lines[intLine];
			for (int intColumn = 0; intColumn <= strLine.Length; intColumn++)
			{
				// now we are at the front of the line, ex whitespace
				if (intColumn == strLine.Length || strLine[intColumn] > ' ')
				{
					int intStart = this.GetFirstCharIndexFromLine(intLine) + intColumn;
					if (intStart == this.SelectionStart) // already here
						intStart = this.GetFirstCharIndexFromLine(intLine);
					// intStart has now got the new start position
					if (blnShift)
					{
						int intAddToLength = this.SelectionStart - intStart;
						this.SelectionStart = intStart;
						// can + of -
						this.SelectionLength = Math.Max(0, this.SelectionLength + intAddToLength);
					}
					else
					{
						this.SelectionStart = intStart;
						this.SelectionLength = 0;
					}
					return;
				}
			}
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			base.OnKeyDown(e);

			if (this.TooltipMouse != null)
			{
				if (this.TooltipMouse.Visible)
				{
					this.TooltipMouse.Visible = false;
					this.Focus();
				}
			}

			if (this.GListBoxWindow != null && this.GListBoxWindow.Visible)
			{
				this.GListBoxWindow.KeyDownHandler(e);
			}
			else
			{
				if (e.KeyCode == Keys.Tab)
				{
					int intLineStart = this.GetLineFromCharIndex(this.SelectionStart);
					int intLineStop = this.GetLineFromCharIndex(this.SelectionStart + this.SelectionLength);
					if (intLineStart != intLineStop)
						MultiLineTab(!e.Shift);
					else
						SingleLineTab(!e.Shift);
					e.Handled = true;
				}
			}

			if (e.KeyCode == Keys.Insert)
			{
				blnInsert = !blnInsert;
				CursorPositionChanged();
			}

			if (e.KeyCode == Keys.Left ||
				e.KeyCode == Keys.Right ||
				e.KeyCode == Keys.End ||
				e.KeyCode == Keys.Home)
				MakeAllInvis();

			if (e.KeyCode == Keys.Home)
			{
				if (!e.Control)
				{
					GoHome(e.Shift);
					e.Handled = true;
				}
			}

			if (e.KeyCode == Keys.End)
			{
				if (!e.Control)
				{
					GoEnd(e.Shift);
					e.Handled = true;
				}
			}

			if (e.KeyCode == Keys.Delete)
			{
				if (e.Control)
				{
					int intWissel = 0;
					int intIndex = this.SelectionStart;
					if (intIndex < this.Text.Length)
					{
						bool blnWhiteSpace = (this.Text[intIndex] <= ' ');

						while (intIndex < this.Text.Length)
						{
							if (this.Text[intIndex] > ' ')
							{
								if (blnWhiteSpace)
								{
									blnWhiteSpace = false;
									intWissel++;
								}
							}
							else // whitespace
							{
								if (!blnWhiteSpace)
								{
									blnWhiteSpace = true;
									intWissel++;
								}
							}
							if (intWissel == 2)
								break;
							intIndex++;
						}
						this.SelectionLength = intIndex - this.SelectionStart;
					}
				} // control
				else
				{
					if (e.Shift)
					{
						if (this.SelectionLength == 0)
						{
							if (this.Lines.Length > 0)
							{
								BeginUpdate();
								this.SelectionStart = this.GetFirstCharIndexOfCurrentLine();
								int intLine = this.GetLineFromCharIndex(this.SelectionStart);
								this.SelectionLength = this.Lines[intLine].Length + 1;
								EndUpdate(true);
							}
						}
						this.Copy();
					} // shift
					else
					{
						if (this.SelectionLength == 0)
							this.SelectionLength = 1;
					} // no shift
				}
				this.Delete();
				e.Handled = true;
			}

			if (e.KeyCode == Keys.X && e.Control)
			{
				this.Cut();
				e.Handled = true;
			}

			if (this.TooltipKeyboard != null)
			{
				if (this.TooltipKeyboard.Visible)
				{
					if (e.KeyCode == Keys.Up ||
						e.KeyCode == Keys.Down)
					{
						this.TooltipKeyboard.Visible = false;
						this.Focus();
					}
				}
			}

			// 16 maart 2008
			if (e.KeyCode == Keys.Back && e.Alt)
				this.Undo();

			if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Back)
				e.Handled = true;

			if (e.Control)
			{
				if (e.KeyCode == Keys.R)
				{
					e.Handled = true;
				}
				if (e.KeyCode == Keys.V)
				{
					this.Paste();
					e.Handled = true;
				}
				if (e.KeyCode == Keys.C)
				{
					this.Copy();
					e.Handled = true;
				}
			}

			if (e.KeyData == Keys.CapsLock)
				CursorPositionChanged();
		}

		private void ProcessBackSpace()
		{
			if (this.SelectionStart == 0 && this.SelectionLength == 0)
				return;

			BeginUpdate();
			if (this.SelectionStart > 0 && this.SelectionLength == 0)
				this.SelectionStart--;
			if (this.SelectionLength == 0)
				this.SelectionLength = 1;
			this.ColoredText = "";
			EndUpdate(true);
			OnTextChanged(null);
			CursorPositionChanged();
		}

		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			ResetHighlighting();

			char keyChar = e.KeyChar;

			switch (e.KeyChar)
			{
				case '\r': // Return
					if (this.GListBoxWindow.Visible)
						InsertSelectedWord();
					else
						ProcessEnter();
					e.Handled = true;
					break;
				case '\t': // TAB
					if (this.GListBoxWindow.Visible)
						InsertSelectedWord();
					e.Handled = true;
					break;
				case '{':
				case '}':
					ProcessBraces(keyChar);
					e.Handled = true;
					break;
				case '\b':
					ProcessBackSpace();
					MakeAllInvis();
					e.Handled = true;
					break;
				case (char)27:
					this.blnEscape = !this.blnEscape;
					MakeAllInvis();
					break;
				default:
					if (keyChar >= 32)
					{
						if (this.SelectionLength != 0)
							this.Delete();
						SaveUndo(new UndoElement(this.SelectionStart, "", 1, null));
					}
					if (keyChar > 32 && !Char.IsLetterOrDigit(keyChar))
						this.blnEscape = false;
					break;
			}
			base.OnKeyPress(e);
		}

		// event generated, make list of codecompletion words
		private void CodeCompletion(string strKeyWord, bool IsRegularExpression)
		{
			if (!ToolTipping)
				return;

			intKeyWordLength = strKeyWord.Length;

			if (IsRegularExpression)
				intKeyWordLength = 0; // else it would delete typed keyword

			if (strKeyWord.Length == 0 || this.blnEscape)
			{
				this.TooltipListBox.Visible = false;
				this.GListBoxWindow.Visible = false;
				this.Focus();
				return;
			}

			List<KeyWordInfo> list;

			if (Properties.Settings.Default.CodeCompletion)
				list = keyWords.KeyWordSearch(strKeyWord, IsRegularExpression);
			else
				list = new List<KeyWordInfo>();

			if (Properties.Settings.Default.CodeCompletionUserVar && !IsRegularExpression)
				codeCompletion.CodeCompletionUserVar(strKeyWord, this.Text, this.SelectionStart, list);

			if (list.Count == 0)
			{
				this.TooltipListBox.Visible = false;
				this.GListBoxWindow.Visible = false;
				this.Focus();
				return;
			}

			if (list.Count == 1)
			{
				// single keyword has been typed, make listbox invisible
				if (list[0].name == strKeyWord)
				{
					/*
					// this autocompletes 1 word, backspace is a problem, use escape
					this.GListBoxWindow.GListBox.Items.Clear();
					KeyWordInfo keyWordInfo = list[0];
					GListBoxItem glbi = new GListBoxItem(keyWordInfo.name, (int)keyWordInfo.type);
					this.GListBoxWindow.GListBox.Items.Add(glbi);

					this.GListBoxWindow.GListBox.SelectedIndex = 0;
					InsertSelectedWord();
					 */

					if (strKeyWord == "else") // else autocompletion enoys people!!! TODO
					{
						this.TooltipListBox.Visible = false;
						this.GListBoxWindow.Visible = false;
						this.Focus();
						return;
					}
				}
			}

			// Hide argument typing
			this.TooltipKeyboard.Visible = false;
			this.Focus();

			this.GListBoxWindow.GListBox.Items.Clear();
			foreach (KeyWordInfo keyWordInfo in list)
			{
				GListBoxItem glbi = new GListBoxItem(keyWordInfo.name, (int)keyWordInfo.type);
				this.GListBoxWindow.GListBox.Items.Add(glbi);
			}
			this.GListBoxWindow.GListBox.SelectedIndex = 0;
			this.GListBoxWindow.GListBox.Height = Math.Min(list.Count + 1, 11) * this.GListBoxWindow.GListBox.ItemHeight;
			this.GListBoxWindow.SetPosition(Screen.GetWorkingArea(this), this);

			// Just in case it pops up
			this.TooltipListBox.SetPosition(Screen.GetWorkingArea(this), this);

			this.GListBoxWindow.Visible = true;
			this.Focus();
		}

		private void SelectWordByDoubleClick()
		{
			int intSelectionStartBackup = this.SelectionStart;

			int intLineNumber = this.GetLineFromCharIndex(intSelectionStartBackup);
			int intFirstChar = this.GetFirstCharIndexFromLine(intLineNumber);
			int intColumn = intSelectionStartBackup - intFirstChar;

			string strLine = this.Lines[intLineNumber];

			// start or ending in underscore then expand
			int intStart = intColumn;
			while ((intStart > 0) && IsAlpha(strLine[intStart - 1]))
				intStart--;

			int intEnd = intColumn;
			while ((intEnd < strLine.Length) && IsAlpha(strLine[intEnd]))
				intEnd++;

			if (intStart != intColumn || intEnd != (intStart + this.SelectionLength))
			{
				this.SelectionStart = intFirstChar + intStart;
				this.SelectionLength = intEnd - intStart;
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown(e);

			BracketHighlichting();

			// this sucks!!!!
			this.AutoWordSelection = true;
			this.AutoWordSelection = Properties.Settings.Default.AutoWordSelection;

			if (this.ToolTipping)
				MakeAllInvis();

			if (e.Clicks == 2)
				SelectWordByDoubleClick();
		}

		private void ShowKeyBoardHint()
		{
			if (!ToolTipping)
				return;

			if (this.GListBoxWindow.Visible)
				return;

			if (this.blnEscape)
				return;

			int intArgument;
			bool DoWild;
			string strFunction = GetCurrentFunction(out intArgument, out DoWild);

			if (strFunction == "")
			{
				this.TooltipKeyboard.Visible = false;
				this.Focus();
				return;
			}

			SetArgumentListOnRichLabel(this.TooltipKeyboard, strFunction, intArgument);
			if (DoWild && this.TooltipKeyboard.Wild != "")
			{
				CodeCompletion(this.TooltipKeyboard.Wild , true);
				this.Focus();
				return;
			}

			if (this.TooltipKeyboard.Text == "")
			{
				this.TooltipKeyboard.Visible = false;
			}
			else
			{
				this.TooltipKeyboard.SetPosition(Screen.GetWorkingArea(this), this);
				this.TooltipKeyboard.Visible = true;
				//this.SelectionColor = this.ForeColor;
			}
			this.Focus();
		}

		private bool IsInComment(string strLine, int intIndex)
		{
			bool blnInString = false;
			for (int intI = 0; intI < intIndex; intI++)
			{
				char chrC = strLine[intI];
				if (chrC == '"')
					blnInString = !blnInString;
				if (blnInString)
				{
					if (chrC == '\\')
						intI++;
					continue;
				}
				if (chrC == '/')
				{
					if ((intI + 1) < intIndex)
						if (strLine[intI + 1] == '/')
							return true;
				}
			}
			return false;
		}

		private bool IsInString(string strLine, int intIndex)
		{
			int intQuotes = 0;
			for (int intI = intIndex - 1; intI >= 0; intI--)
			{
				char chrC = strLine[intI];
				if (chrC == '"')
				{
					if ((intI - 1) >= 0)
						if (strLine[intI - 1] != '\\')
							intQuotes++;
				}
			}
			return ((intQuotes % 2) != 0);
		}

		/// 
		/// OnTextChanged
		///
		protected override void OnTextChanged(EventArgs e)
		{
			if (IsUpdating)
				return;

			if (this.Lines.Length == 0)
			{
				TabStops();
				return;
			}

			this.Dirty = true;

			int intSelectionStartBackup = this.SelectionStart;

			int intLineNumber = this.GetLineFromCharIndex(intSelectionStartBackup);
			int intFirstChar = this.GetFirstCharIndexFromLine(intLineNumber);
			int intColumn = intSelectionStartBackup - intFirstChar;

			string strLine = this.Lines[intLineNumber];

			ColorLine(intFirstChar, strLine.Length);

			if (IsInComment(strLine, intColumn))
				return;

			if (IsInString(strLine, intColumn))
				return;

			// not in comment and not in string, find the start of a word
			int intStart = intColumn;
			while ((intStart > 0) && IsAlpha(strLine[intStart - 1]))
				intStart--;

			string strTypedWord = strLine.Substring(intStart, intColumn - intStart);

			CodeCompletion(strTypedWord , false);

			ShowKeyBoardHint();
		}

		/// 
		/// Color a line
		/// 
		private void ColorLine(int intStart, int intLength)
		{
			if (!ToolTipping)
				return;

			if (intLength <= 0)
				return;

			BeginUpdate();

			// Backup position
			int SelectionStartBackup = this.SelectionStart;

			// get the line
			string strLine = this.Text.Substring(intStart, intLength);

			// highlight all keywords
			foreach (Match m in keyWords.Matches(strLine))
			{
				Group g = m.Groups[1];
				string strKeyWord = g.Value;

				this.SelectionStart = intStart + g.Index;
				this.SelectionLength = g.Length;

				// normal keywords
				if (keyWords.ContainsKeyWord(strKeyWord))
					this.SelectionColor = keyWords.GetColorFromKeyWordList(strKeyWord);
				else
					this.SelectionColor = keyWords.GetColorFromRegex(strKeyWord);
			}

			// Restore position
			this.SelectionLength = 0;
			this.SelectionStart = SelectionStartBackup;
			this.SelectionColor = this.ForeColor;

			EndUpdate(false);
		}

		private int AutoFormat(bool OnlySelectedText)
		{
			int intTabs;
			if (OnlySelectedText)
			{
				string strW = GetNewWhiteSpace(-1);
				intTabs = (int)(strW.Length / AutoFormatter.GetTab().Length);
			}
			else
			{
				this.SelectionStart = 0;
				this.SelectionLength = this.Text.Length;
				intTabs = 0;
			}
			string strFormattedText = AutoFormatter.ApplyFormatting(intTabs, this.SelectedText);

			this.ColoredText = strFormattedText;

			return strFormattedText.Length;
		}

		public void AutoFormatSelectedText()
		{
			BeginUpdate();
			Point scrollPoint = this.RTBScrollPos;
			int intBackupStart = this.SelectionStart;
			int intSelectionLength = AutoFormat(true);
			this.SelectionStart = intBackupStart;
			this.SelectionLength = intSelectionLength;
			this.RTBScrollPos = scrollPoint;
			EndUpdate(false);
		}

		public void FormatDocument()
		{
			BeginUpdate();
			Point scrollPoint = this.RTBScrollPos;
			int intLine = this.GetLineFromCharIndex(this.SelectionStart);
			AutoFormat(false);
			this.SelectionStart = Math.Max(0,this.GetFirstCharIndexFromLine(intLine));
			this.RTBScrollPos = scrollPoint;
			EndUpdate(false);
		}

		public void ClearUndoStack()
		{
			UndoStack.Clear();
			Dirty = false;
		}

		public override string Text
		{
			get
			{
				return base.Text;
			}
			set
			{
				BeginUpdate();
				int intBackup = this.SelectionStart;
				this.SelectionStart = 0;
				this.SelectionLength = this.Text.Length; //TODO
				this.ColoredText = value;
				this.SelectionStart = intBackup;
				EndUpdate(true);
			}
		}

		private string GetWhiteSpaceFromCurrentLine()
		{
			int intIndex = this.GetLineFromCharIndex(this.SelectionStart);
			if (intIndex < this.Lines.Length)
				return AutoFormatter.GetWhiteSpaceFromLine(this.Lines[intIndex]);
			return "";
		}

		// code-completion word is selected
		// also comes from mainform (doubleclick), must be public
		public void InsertSelectedWord()
		{
			this.GListBoxWindow.Visible = false;
			this.TooltipListBox.Visible = false;

			if (this.GListBoxWindow.GListBox.SelectedIndex < 0)
				return;

			int intOffset = 0;

			GListBoxItem glbi = this.GListBoxWindow.Selected;
			string strSelected = glbi.ToString();
			if (strSelected == "")
			{
				this.Focus();
				return;
			}

			if (glbi.ImageIndex == (int)KeyWordTypeEnum.Events
				|| strSelected == "default" && this.Text.IndexOf("default")<0)
			{
				if (strSelected != "default")
					strSelected = keyWords.GetEvent(strSelected);

				string strW = GetWhiteSpaceFromCurrentLine();
				strSelected += "\n" + strW + "{\n" + strW + AutoFormatter.GetTab() + "\n" + strW + "}";
				intOffset = -(strW.Length + 2);
			}
			else if (glbi.ImageIndex == (int)KeyWordTypeEnum.Functions)
			{
				if(keyWords.GetNumberOfArguments(strSelected) == 0)
					strSelected += "()";
				else
					strSelected += "(";
			}
			else if (glbi.ImageIndex == (int)KeyWordTypeEnum.Properties)
			{
				strSelected = "\"" + strSelected.Trim() +"\"";
			}

			BeginUpdate();
			this.SelectionStart -= intKeyWordLength;
			this.SelectionLength = intKeyWordLength;
			this.ColoredText = strSelected;

			this.SelectionStart += intOffset;
			EndUpdate(true);

			// if any
			ShowKeyBoardHint();

			this.Focus();
		}

		// this comes from the main form
		public void ShowTooltipOnListBox()
		{
			string strSelected = this.GListBoxWindow.Selected.ToString();
			string strDescription = keyWords.GetDescription(strSelected);
			if (strDescription != "")
			{
				this.TooltipListBox.Tag = strSelected;
				this.TooltipListBox.Text = strDescription;
				this.TooltipListBox.XOffset = this.GListBoxWindow.Width;
				this.TooltipListBox.SetPosition(Screen.GetWorkingArea(this), this);
				this.TooltipListBox.Visible = true;
				this.Focus();
			}
			else
			{
				this.TooltipListBox.Visible = false;
				this.Focus();
			}
		}

		// Tooltip mouse by (Windows.Forms) timer
		private void timer1_Tick(object sender, EventArgs e)
		{
			this.timer1.Stop();

			if (this.Disposing || this.IsDisposed)
				return;

			if (!this.Visible)
				return;

			if (!ToolTipping)
				return;

			if (!Properties.Settings.Default.ToolTip)
				return;

			if (Control.MouseButtons != MouseButtons.None)
				return;

			Point point;
			string strKeyWord = this.GetCurrentKeyWord(true, out point);

			if (strKeyWord == "")
			{
				this.TooltipMouse.Visible = false;
				return;
			}

			if (this.TooltipMouse.Tag.ToString() != strKeyWord)
			{
				this.TooltipMouse.Tag = strKeyWord;
				this.TooltipMouse.Text = keyWords.GetDescription(strKeyWord);
			}

			if (this.TooltipMouse.Text == "")
			{
				this.TooltipMouse.Visible = false;
				return;
			}

			this.TooltipMouse.SetPosition(this, point);
			this.TooltipMouse.Visible = true;
			this.Focus();
		}

		public bool ToolTipping
		{
			get
			{
				return m_blnToolTipping;
			}
			set
			{
				m_blnToolTipping = value;
				if (m_blnToolTipping)
				{
					this.timer1 = new System.Windows.Forms.Timer();
					this.timer1.Interval = Properties.Settings.Default.ToolTipDelay;
					this.timer1.Tick += new EventHandler(timer1_Tick);
				}
			}
		}

		public void SetPosition(Rectangle rect)
		{
			if (!ToolTipping)
				return;
			if (this.GListBoxWindow.Visible)
				this.GListBoxWindow.SetPosition(rect, this);
			if (this.TooltipListBox.Visible)
				this.TooltipListBox.SetPosition(rect, this);
			if (this.TooltipKeyboard.Visible)
				this.TooltipKeyboard.SetPosition(rect, this);
		}

		private void SetArgumentListOnRichLabel(TooltipWindow window, string strKeyWord, int intArgument)
		{
			if (window.Tag.ToString() == (strKeyWord + intArgument))
				return; // cached information

			string strWild = "";
			window.Wild = "";
			window.Tag = (strKeyWord + intArgument);
			window.Text = keyWords.GetFunctionAndHiglightArgument(strKeyWord, intArgument, out strWild);
			if(Properties.Settings.Default.CodeCompletionArguments)
				window.Wild = strWild;
		}


		public void Goto(int intLine)
		{
			this.SelectionStart = this.GetFirstCharIndexFromLine(intLine - 1);
			this.Focus();
		}

		public void MultiLineComment(bool blnAdd)
		{
			BeginUpdate();

			ResetHighlighting();

			string strW = GetNewWhiteSpace(-1);
			int intTabs = (int)(strW.Length / AutoFormatter.GetTab().Length);

			int intLastLine = this.GetLineFromCharIndex(this.SelectionStart + this.SelectionLength);

			int intLine = this.GetLineFromCharIndex(this.SelectionStart);
			this.SelectionStart = this.GetFirstCharIndexFromLine(intLine);
			int intLength = 0;
			do
			{
				intLength += this.Lines[intLine].Length + 1;
				intLine++;
			} while (intLine <= intLastLine);
			this.SelectionLength = intLength;

			string strSelectedText = AutoFormatter.MultiLineComment(blnAdd, intTabs, this.SelectedText);

			int intBackup = this.SelectionStart;
			this.ColoredText = strSelectedText;
			this.SelectionStart = intBackup;
			this.SelectionLength = strSelectedText.Length;
			EndUpdate(true);
		}

		public void MultiLineTab(bool blnAdd)
		{
			string strSelectedText = AutoFormatter.MultiLineTab(blnAdd, this.SelectedText);

			int intBackup = this.SelectionStart;
			this.ColoredText = strSelectedText;
			this.SelectionStart = intBackup;
			this.SelectionLength = strSelectedText.Length;
		}

		private void SingleLineTab(bool blnAdd)
		{
			if (blnAdd)
			{
				this.SelectedText = AutoFormatter.GetTab();
			}
			else
			{
				// Shift tab on single line
				string strTab = AutoFormatter.GetTab();
				if (this.SelectionStart > strTab.Length)
				{
					string strBefore = this.Text.Substring(this.SelectionStart - strTab.Length, strTab.Length);
					if (strBefore == strTab)
					{
						int intBackupLength = this.SelectionLength;
						this.SelectionStart -= strTab.Length;
						this.SelectionLength = strTab.Length;
						this.SelectedText = "";
						this.SelectionLength = intBackupLength;
					}
					else
					{
						// space remove? TODO
					}
				}
			}
		}

		private void HighLightCharAt(int intIndex, Color color)
		{
			this.SelectionStart = intIndex;
			this.SelectionLength = 1;
			this.SelectionBackColor = color;

			if (color != this.BackColor)
				HighLightList.Add(intIndex);
		}

		private void HighLightCharAt(int intLine, int intColumn, Color color)
		{
			int intIndex = this.GetFirstCharIndexFromLine(intLine) + intColumn;
			HighLightCharAt(intIndex, color);
		}

		private bool MatchingBracket(int intCurrentLine, int intCurrentColumn, char chrOpening, char chrClosing, int intDirection, out int intLine, out int intColumn)
		{
			if (this.Lines.Length == 0)
			{
				intLine = 0;
				intColumn = 0;
				return false;
			}
			intLine = intCurrentLine;
			intColumn = intCurrentColumn;
			string strLine = AutoFormatter.RemoveComment(this.Lines[intLine]);

			int intNumber = 1;
			bool blnWithinString = false;
			while (true)
			{
				intColumn += intDirection;
				while (intColumn < 0)
				{
					intLine--;
					if (intLine < 0)
						return false; // nothing to do
					strLine = AutoFormatter.RemoveComment(this.Lines[intLine]); // get new previous line
					intColumn = strLine.Length - 1; // place on end of line
				}

				while (intColumn >= strLine.Length)
				{
					intLine++;
					if (intLine >= this.Lines.Length)
						return false; // nothing to do
					strLine = AutoFormatter.RemoveComment(this.Lines[intLine]); // get new previous line
					intColumn = 0; // place on begin of line
				}

				char chrC = strLine[intColumn];

				if (chrC == '"')
				{
					if (intDirection < 0 && intColumn > 0)
					{
						if (strLine[intColumn - 1] == '\\')
						{
							intColumn += intDirection;
							continue;
						}
					}
					blnWithinString = !blnWithinString;
				}

				if (blnWithinString)
				{
					if (intDirection > 0 && chrC == '\\')
						intColumn += intDirection;
					continue;
				}

				if (chrC == chrOpening)
					intNumber++;
				else if (chrC == chrClosing)
				{
					intNumber--;
					if (intNumber == 0)
						return true;
				}
			}
		}

		private void ResetHighlighting()
		{
			if (HighLightList.Count == 0)
				return;

			BeginUpdate();

			Point scrollPoint = this.RTBScrollPos;

			int intBackupStart = this.SelectionStart;
			int intBackupLength = this.SelectionLength;
			foreach (int intIndex in HighLightList)
				HighLightCharAt(intIndex, this.BackColor);
			HighLightList.Clear();
			this.SelectionStart = intBackupStart;
			this.SelectionLength = intBackupLength;

			this.RTBScrollPos = scrollPoint;

			EndUpdate(false);
		}

		private void BracketHighlichting()
		{
			if (this.TextLength == 0)
				return;
			if (this.SelectionLength != 0)
				return;

			BeginUpdate();

			Point scrollPoint = this.RTBScrollPos;

			int intSelectionStart = this.SelectionStart;

			// ResetHighlichting without the overhead
			if (HighLightList.Count > 0)
			{
				foreach (int intI in HighLightList)
					HighLightCharAt(intI, this.BackColor);
				HighLightList.Clear();
			}

			int intIndex;
			int intType;

			const string Uppers = "<({[";
			const string Downers = ">)}]";

			int intLine = this.GetLineFromCharIndex(intSelectionStart);
			int intColumn = intSelectionStart - this.GetFirstCharIndexFromLine(intLine);

			string strLine = AutoFormatter.RemoveComment(this.Lines[intLine]);

			if (intColumn <= strLine.Length)
			{
				intIndex = intColumn - 1;
				if (intIndex >= 0)
				{
					intType = Downers.IndexOf(strLine[intIndex]);
					if (intType >= 0)
					{
						int intL, intC;
						if (MatchingBracket(intLine, intIndex, Downers[intType], Uppers[intType], -1, out intL, out intC))
						{
							HighLightCharAt(intLine, intIndex, Properties.Settings.Default.BracketHighlight);
							HighLightCharAt(intL, intC, Properties.Settings.Default.BracketHighlight);
						}
					}
				}

				intIndex = intColumn;
				if (intIndex < strLine.Length)
				{
					intType = Uppers.IndexOf(strLine[intIndex]);
					if (intType >= 0)
					{
						int intL, intC;
						if (MatchingBracket(intLine, intIndex, Uppers[intType], Downers[intType], 1, out intL, out intC))
						{
							HighLightCharAt(intLine, intIndex, Properties.Settings.Default.BracketHighlight);
							HighLightCharAt(intL, intC, Properties.Settings.Default.BracketHighlight);
						}
					}
				}
			}

			this.SelectionStart = intSelectionStart;
			this.SelectionLength = 0;

			this.RTBScrollPos = scrollPoint;

			EndUpdate(false);
		}

		public override string SelectedText
		{
			get
			{
				return base.SelectedText;
			}
			set
			{
				if (!IsUpdating)
					SaveUndo(new UndoElement(this.SelectionStart, this.SelectedText, value.Length, null));
				base.SelectedText = value;
			}
		}

		private string ColoredText
		{
			set
			{
				BeginUpdate();
				int intFirstCharOfStartLine = this.GetFirstCharIndexOfCurrentLine();
				int intBackup = this.SelectionStart;
				if (this.SelectedText != value)
				{
					this.Dirty = true;
					string strTmp = value.Replace("\r", "");
					SaveUndo(new UndoElement(this.SelectionStart, this.SelectedText, strTmp.Length, null));
					this.SelectedText = strTmp;
					if (this.Lines.Length > 0)
					{
						int intEndLine = this.GetLineFromCharIndex(intBackup + strTmp.Length);
						int intLastCharOfEndLine = this.GetFirstCharIndexFromLine(intEndLine) + this.Lines[intEndLine].Length;
						int intLength = intLastCharOfEndLine - intFirstCharOfStartLine;
						ColorLine(intFirstCharOfStartLine, intLength);
						this.SelectionStart = intBackup + strTmp.Length;
					}
				}
				else
				{
					this.SelectionLength = 0;
					this.SelectionStart += value.Length;
				}
				EndUpdate(true);
			}
		}
        public void doOutline()
        {
            //TODO: finish the outline class and such
            //      still a work in progress trying to figure out exactly how i wanna do this.

            int len = this.Lines.Length;
            if (len < 1)
            {
                return;
            }
            Dictionary<int, Helpers.OutlineHelper> list = new Dictionary<int, LSLEditor.Helpers.OutlineHelper>();
            string ttext = this.Text;
            ttext.Replace("\r\n", "");
            using (StringReader reader = new StringReader(ttext))
            {
                string line;

                int lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {

                    line = line.Trim();
                    line = line.Split('(')[0];
                    string[] words = line.Split(' ');
                    foreach (string word in words)
                    {
                        //Debug.WriteLine("ww:" + word);
                        if (keyWords.ContainsKeyWord(word))
                        {
                            KeyWordInfo k = keyWords.GetKeyWordInfo(word);
                          //  Debug.WriteLine("w:" + word);
                            //Debug.WriteLine("k:" + k.type);
                            if (!list.ContainsKey(lineNumber))
                            {
                                switch (k.type)
                                {

                                    case KeyWordTypeEnum.Functions:
                                        list.Add(lineNumber, new LSLEditor.Helpers.OutlineHelper(k, lineNumber));
                                        // Debug.WriteLine(k);
                                        break;
                                    case KeyWordTypeEnum.Events:
                                        list.Add(lineNumber, new LSLEditor.Helpers.OutlineHelper(k, lineNumber));
                                        break;
                                    case KeyWordTypeEnum.Constants:
                                        list.Add(lineNumber, new LSLEditor.Helpers.OutlineHelper(k, lineNumber));
                                        break;
                                    case KeyWordTypeEnum.Class:
                                        list.Add(lineNumber, new LSLEditor.Helpers.OutlineHelper(k, lineNumber));
                                        break;
                                    case KeyWordTypeEnum.Vars:
                                        list.Add(lineNumber, new LSLEditor.Helpers.OutlineHelper(k, lineNumber));
                                        break;
                                    case KeyWordTypeEnum.States:
                                        list.Add(lineNumber, new LSLEditor.Helpers.OutlineHelper(k, lineNumber));
                                        break;
                                    default:
                                        //Debug.WriteLine(k);
                                        break;
                                }
                            }
                        }
                    }

                    lineNumber++;

                }
            }
            //TODO: parse dict and create the outline in the treeview
            //WILL SOMEONE PLEASE FUCKING FINISH THIS!

            if (p != null) //It gives the parent-scriptwindow as null when you try to run it
            {
                p.tvOutline.BeginUpdate();
                p.tvOutline.Nodes.Clear();
                TreeNode lastState = null;
                TreeNode lastEvent = null;
                TreeNode lastScope = null;

                foreach (LSLEditor.Helpers.OutlineHelper k in list.Values)
                {
                    switch (k.info.type)
                    {
                        case KeyWordTypeEnum.States:
                            lastState = createOutlineNode(k);
                            lastScope = lastState;
                            p.tvOutline.Nodes.Add(lastState);
                            break;
                        case KeyWordTypeEnum.Events:
                            if (lastState != null) //we need a state for every event!
                            {
                                lastEvent = createOutlineNode(k);
                                lastScope = lastEvent;
                                lastState.Nodes.Add(lastEvent);
                            }
                            break;
                        case KeyWordTypeEnum.Functions:
                            if (lastScope != null)
                            {
                                lastScope.Nodes.Add(createOutlineNode(k));
                            }
                            break;
                        case KeyWordTypeEnum.Class:
                            if (lastScope != null)
                            {
                                lastScope.Nodes.Add(createOutlineNode(k));
                            }
                            else
                            {
                                p.tvOutline.Nodes.Add(createOutlineNode(k));
                            }
                            break;
                        default:
                            p.tvOutline.Nodes.Add(createOutlineNode(k));
                            break;
                    }
                }
                p.tvOutline.EndUpdate();
                // p.tvOutline.Nodes.Add(states);
                p.tvOutline.ExpandAll();

            }
        }

        TreeNode createOutlineNode(Helpers.OutlineHelper ohOutline)
        {
            TreeNode result = null;
            int ImageKey = (int)ohOutline.info.type;
            result = new TreeNode(string.Format("{0} [{1}]", ohOutline.info.name, ohOutline.line + 1), ImageKey, ImageKey);
            result.Tag = ohOutline;
            return result;
        }

		public void SaveCurrentFile(string strPath)
		{
			try
			{
				Encoding enc = null;
				if (!Directory.Exists(Path.GetDirectoryName(strPath)))
					Directory.CreateDirectory(Path.GetDirectoryName(strPath));
				switch (Properties.Settings.Default.OutputFormat)
				{
					case "UTF8":
						enc = Encoding.UTF8;
						break;
					case "Unicode":
						enc = Encoding.Unicode;
						break;
					case "BigEndianUnicode":
						enc = Encoding.BigEndianUnicode;
						break;
					default:
						enc = Encoding.Default;
						break;
				}
                StreamWriter sw;
                if (enc != Encoding.UTF8)
                {
                    sw = new StreamWriter(strPath, false, enc);
                }
                else
                {
                    sw = new StreamWriter(strPath);
                }
				sw.Write(this.Text);
				sw.Close();
				//this.SaveFile(strPath, RichTextBoxStreamType.PlainText);
				this.Dirty = false;
			}
			catch // (Exception exception)
			{
				MessageBox.Show("Error saving file, check pathname: " + strPath, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public new void LoadFile(string path)
		{
			if (path.StartsWith("http://"))
			{
				System.Net.WebClient webClient = new System.Net.WebClient();
				this.Text = webClient.DownloadString(path);
			}
			else
			{
				if (File.Exists(path))
				{
					StreamReader sr = new StreamReader(path, Encoding.Default);
					this.Text = sr.ReadToEnd();
					sr.Close();
				}
			}
			// Fresh files can not be dirty
			ClearUndoStack();
		}
	}
}
