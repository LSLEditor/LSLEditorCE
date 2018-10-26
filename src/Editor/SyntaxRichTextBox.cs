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
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using LSLEditor.Helpers;

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
        [DllImport("user32")]
        private static extern int SendMessage(IntPtr hwndLock, int wMsg, int wParam, ref Point pt);

        // Anti flicker
        [DllImport("user32", CharSet = CharSet.Auto)]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, int wParam, IntPtr lParam);

        // Tabs
        [DllImport("user32", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(IntPtr h, int msg, int wParam, int[] lParam);

        private bool m_Dirty;

        private bool blnEscape;

        private bool blnInsert;

        // for tooltipping
        private bool m_blnToolTipping;
        private Timer timer1;
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

        private readonly CodeCompletion codeCompletion;

        // after clicking on error
        private int HighLightLine;

        // bracket highlighting
        private readonly List<int> HighLightList;
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

        private readonly Stack<UndoElement> UndoStack;
        private readonly Stack<UndoElement> RedoStack;

        #region printing
        //Convert the unit used by the .NET framework (1/100 inch) 
        //and the unit used by Win32 API calls (twips 1/1440 inch)
        private const double AnInch = 14.4;

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
            public int cpMin;   //First character of range (0 for start of doc)
            public int cpMax;   //Last character of range (-1 for end of doc)
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct FORMATRANGE
        {
            public IntPtr hdc;          //Actual DC to draw on
            public IntPtr hdcTarget;    //Target DC for determining text formatting
            public RECT rc;             //Region of the DC to draw to (in twips)
            public RECT rcPage;         //Region of the whole DC (page size) (in twips)
            public CHARRANGE chrg;      //Range of text to draw (see earlier declaration)
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
            rectToPrint.Top = (int)(e.MarginBounds.Top * AnInch);
            rectToPrint.Bottom = (int)(e.MarginBounds.Bottom * AnInch);
            rectToPrint.Left = (int)(e.MarginBounds.Left * AnInch);
            rectToPrint.Right = (int)(e.MarginBounds.Right * AnInch);

            //Calculate the size of the page
            RECT rectPage;
            rectPage.Top = (int)(e.PageBounds.Top * AnInch);
            rectPage.Bottom = (int)(e.PageBounds.Bottom * AnInch);
            rectPage.Left = (int)(e.PageBounds.Left * AnInch);
            rectPage.Right = (int)(e.PageBounds.Right * AnInch);

            var hdc = e.Graphics.GetHdc();

            FORMATRANGE fmtRange;
            fmtRange.chrg.cpMax = charTo;   //Indicate character from to character to 
            fmtRange.chrg.cpMin = charFrom;
            fmtRange.hdc = hdc;             //Use the same DC for measuring and rendering
            fmtRange.hdcTarget = hdc;       //Point at printer hDC
            fmtRange.rc = rectToPrint;      //Indicate the area on page to print
            fmtRange.rcPage = rectPage;     //Indicate size of page

            var wparam = new IntPtr(1);

            //Get the pointer to the FORMATRANGE structure in memory
            var lparam = Marshal.AllocCoTaskMem(Marshal.SizeOf(fmtRange));
            Marshal.StructureToPtr(fmtRange, lparam, false);

            //Send the rendered data for printing 
            var res = SendMessage(this.Handle, EM_FORMATRANGE, wparam, lparam);

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

            this.FontChanged += this.SyntaxRichTextBox_FontChanged;

            this.MouseMove += this.SyntaxRichTextBox_MouseMove;

            this.VScroll += this.SyntaxRichTextBox_Position;
            this.HScroll += this.SyntaxRichTextBox_Position;

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

            this.codeCompletion = new CodeCompletion();

            this.HideSelection = false;

            // are these of any use?
            //this.DoubleBuffered = true;
            //SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            //SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            //SetStyle(ControlStyles.Opaque, true);
        }

        public void setEditform(EditForm ed)
        {
            this.p = ed;
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
                // p = this.Parent as EditForm;
                // p = parent; moved to edit form
            }

            var ColorScheme = "color";
            if (this.ToolTipping)
            {
                if (Properties.Settings.Default.SLColorScheme)
                {
                    ColorScheme = "sl" + ColorScheme;
                }

                var BackgroundColorNode = xml.SelectSingleNode("/Conf");
                if (BackgroundColorNode != null)
                {
                    this.BackColor = Color.FromArgb(255, Color.FromArgb(
                        int.Parse(BackgroundColorNode.Attributes[ColorScheme].InnerText.Replace("#", ""),
                        System.Globalization.NumberStyles.HexNumber)));
                }
            }
            this.keyWords = new KeyWords(ColorScheme, xml);
        }

        private void MeasureFont()
        {
            var size = TextRenderer.MeasureText("MMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMMM", this.Font);
            this.CharWidth = (int)(size.Width / 50.0F);
            this.LineHeight = size.Height;
        }

        private void SyntaxRichTextBox_FontChanged(object sender, EventArgs e)
        {
            this.SelectAll();

            this.TabStops();

            // recolor all
            this.ColorLine(0, this.Text.Length);
        }

        private void TabStops()
        {
            this.MeasureFont();

            const int EM_SETTABSTOPS = 0xCB;

            var intNumberOfChars = Properties.Settings.Default.TabStops;
            var tabs = new int[30]; // TODO
            var fltTabWidth = intNumberOfChars * this.CharWidth;

            var totalWidth = 0.0F;
            for (var intI = 0; intI < tabs.Length; intI++)
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
            OnPaintNumbers?.Invoke(null, null);
        }

        protected override void OnVScroll(EventArgs e)
        {
            base.OnVScroll(e);
            this.PaintNumbers();
        }

        private Point RTBScrollPos
        {
            get
            {
                const int EM_GETSCROLLPOS = 0x0400 + 221;
                var pt = new Point();

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
                return this.m_Dirty;
            }
            set
            {
                if (this.m_Dirty == value)
                {
                    return;
                }

                this.m_Dirty = value;

                OnDirtyChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void SyntaxRichTextBox_Position(object sender, EventArgs e)
        {
            this.SetPosition(Screen.GetWorkingArea(this));
        }

        private void SyntaxRichTextBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!this.ToolTipping)
            {
                return;
            }

            if (this.timer1 == null)
            {
                return;
            }

            this.timer1.Stop();
            if (this.OldMouseLocation != e.Location)
            {
                this.OldMouseLocation = e.Location;
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
            var p1 = this.GetPositionFromCharIndex(this.GetFirstCharIndexOfCurrentLine());
            var p2 = this.GetPositionFromCharIndex(this.SelectionStart);
            var intColumn = (int)((p2.X - p1.X) / this.CharWidth);
            return 1 + intColumn;
        }

        private int GetTotal()
        {
            return this.Text.Length;
        }

        public void Goto(int Line, int Char)
        {
            this.BeginUpdate();
            this.LineDownlighting();

            try
            {
                var intLine = Line - 1;
                if (intLine < 0)
                {
                    intLine = 0;
                }

                if (intLine >= this.Lines.Length)
                {
                    intLine = this.Lines.Length - 1;
                }

                if (intLine >= 0 && intLine < this.Lines.Length)
                {
                    var intLength = this.Lines[intLine].Length;
                    var intStart = this.GetFirstCharIndexFromLine(intLine);
                    var intIndex = intStart + Char - 1;
                    this.HighLightLine = intLine;
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

            this.EndUpdate(false);
            this.Focus();
        }

        protected override void OnSelectionChanged(EventArgs e)
        {
            base.OnSelectionChanged(e);
            if (!this.IsUpdating)
            {
                this.CursorPositionChanged(); // show cursor position
            }
        }

        private bool IsUpdating
        {
            get
            {
                return this.intUpdate > 0;
            }
        }

        private Point backupScrollPoint;

        private void BeginUpdate()
        {
            ++this.intUpdate;
            if (this.intUpdate > 1) // once is enough
            {
                return;
            }

            this.backupScrollPoint = this.RTBScrollPos;
            // Disable redrawing
            SendMessage(this.Handle, WM_SETREDRAW, 0, IntPtr.Zero);

            // Disable generating events
            this.eventMask = SendMessage(this.Handle, EM_GETEVENTMASK, 0, IntPtr.Zero);
        }

        private void EndUpdate(bool update_outline)
        {
            if (update_outline)
            {
                this.doOutline();
            }

            this.intUpdate = Math.Max(0, this.intUpdate - 1);

            if (this.intUpdate > 0) // only the last one
            {
                return;
            }

            // Enable events
            SendMessage(this.Handle, EM_SETEVENTMASK, 0, this.eventMask);

            // Enable redrawing
            SendMessage(this.Handle, WM_SETREDRAW, 1, IntPtr.Zero);

            // also draw when linenumbers are shifted
            if (this.intOldLines != this.Lines.Length || this.backupScrollPoint != this.RTBScrollPos)
            {
                this.intOldLines = this.Lines.Length;
                this.PaintNumbers();
            }
            this.Invalidate();
        }

        private bool IsAlpha(char chrC)
        {
            return char.IsLetterOrDigit(chrC) || chrC == '_';
        }

        // used for F1 key, help and tooltipping, use Mouse of begin-selection
        public string GetCurrentKeyWord(bool blnUserMousePosition, out Point PositionFirstCharacter)
        {
            PositionFirstCharacter = Point.Empty;

            var intIndex = this.SelectionStart;
            if (blnUserMousePosition)
            {
                var mp = this.PointToClient(Control.MousePosition);
                intIndex = this.GetCharIndexFromPosition(mp);

                // check if mouse is in region of index
                var mpControle = this.GetPositionFromCharIndex(intIndex);
                if (Math.Abs(mp.X - mpControle.X) > 5 || Math.Abs(mp.Y - mpControle.Y) > this.LineHeight)
                {
                    return "";
                }
            }

            var intLineNr = this.GetLineFromCharIndex(intIndex);
            var intFirstChar = this.GetFirstCharIndexFromLine(intLineNr);
            var intColumn = intIndex - intFirstChar;

            var strLine = this.Lines[intLineNr];

            var intStart = intColumn;
            while ((intStart > 0) && this.IsAlpha(strLine[intStart - 1]))
            {
                intStart--;
            }

            // 22 jun 2007
            PositionFirstCharacter = this.GetPositionFromCharIndex(intFirstChar + intStart + 2);

            var intEnd = intColumn;
            while ((intEnd < strLine.Length) && this.IsAlpha(strLine[intEnd]))
            {
                intEnd++;
            }

            // Calculate the length of the keyword.
            var intLength = intEnd - intStart;

            // return the keyword
            return strLine.Substring(intStart, intLength);
        }

        private string GetCurrentFunction(out int Argument, out bool DoWild)
        {
            DoWild = false;
            Argument = 0;
            var intIndex = this.SelectionStart;
            var intLineNumber = this.GetLineFromCharIndex(intIndex);
            var intStartChar = this.GetFirstCharIndexFromLine(intLineNumber);

            var intColumn = intIndex - intStartChar;

            var strLine = this.Lines[intLineNumber];

            // Find the end of the current function
            var intEnd = intColumn - 1;
            while (intEnd > 0 && strLine[intEnd] != '(' && strLine[intEnd] != ')' && strLine[intEnd] != '\n')
            {
                if (strLine[intEnd] == ',')
                {
                    Argument++;
                    if (Argument == 1)
                    {
                        DoWild = (intColumn - intEnd) == 1;
                    }
                }
                intEnd--;
            }
            if (intEnd <= 0)
            {
                return "";
            }

            if (strLine[intEnd] != '(')
            {
                return "";
            }

            if (Argument == 0)
            {
                DoWild = (intColumn - intEnd) == 1;
            }

            intEnd--;

            // Find the begin of the current function.
            var intStart = intEnd;
            while ((intStart > 0) && this.IsAlpha(strLine[intStart]))
            {
                intStart--;
            }

            // Calculate the length of the function.
            var intLength = intEnd - intStart;

            // return the Function name
            return strLine.Substring(intStart + 1, intLength);
        }

        private string GetNewWhiteSpace(int intOffset)
        {
            var intIndex = this.GetLineFromCharIndex(this.SelectionStart);
            return AutoFormatter.GetNewWhiteSpace(this.Lines, intIndex + intOffset);
        }

        private void AutoFormatFromLineToLine(int intLineStart, int intLineEnd)
        {
            this.BeginUpdate();
            var intStart = this.GetFirstCharIndexFromLine(intLineStart);
            var intLength = this.GetFirstCharIndexFromLine(intLineEnd) + this.Lines[intLineEnd].Length + 1 - intStart;
            this.SelectionStart = intStart;
            this.SelectionLength = intLength;
            this.AutoFormat(true);
            this.EndUpdate(true);
        }

        private void ProcessEnter()
        {
            this.BeginUpdate();
            this.ColoredText = "\n";
            if (this.ToolTipping && Properties.Settings.Default.Indent)
            {
                if (Properties.Settings.Default.IndentFullAuto)
                {
                    var intLine = this.GetLineFromCharIndex(this.SelectionStart);
                    this.AutoFormatFromLineToLine(intLine - 1, intLine - 1);
                    this.ColoredText = this.GetNewWhiteSpace(-1);
                }
                else if (Properties.Settings.Default.IndentCursorPlacement)
                {
                    this.ColoredText = this.GetNewWhiteSpace(-1);
                }
            }
            this.EndUpdate(true);
        }

        private void SmartIndenting()
        {
            var intLastLine = this.GetLineFromCharIndex(this.SelectionStart);
            var intColumn = this.SelectionStart - this.GetFirstCharIndexFromLine(intLastLine);

            intColumn--; // because we are at position '}'

            if (this.MatchingBracket(intLastLine, intColumn, '}', '{', -1, out var intFirstLine, out var intC)
            && (intLastLine - intFirstLine) >= 1)
            {
                this.AutoFormatFromLineToLine(intFirstLine + 1, intLastLine);
            }
        }

        private void ProcessBraces(char keyChar)
        {
            this.BeginUpdate();
            var intStart = this.SelectionStart;

            this.ColoredText = keyChar.ToString();

            if (Properties.Settings.Default.Indent)
            {
                var intLength = this.Text.Length;

                if (Properties.Settings.Default.IndentFullAuto)
                {
                    if (keyChar == '}')
                    {
                        this.SmartIndenting();
                    }
                    else
                    {
                        var intCurrentLine = this.GetLineFromCharIndex(this.SelectionStart);
                        this.AutoFormatFromLineToLine(intCurrentLine, intCurrentLine);
                    }
                }

                var intDelta = 1 + this.Text.Length - intLength;

                this.SelectionStart = intStart + intDelta;

                this.BracketHighlichting();
            }

            this.EndUpdate(false);
        }

        public void Delete()
        {
            this.BeginUpdate();
            if (this.SelectionLength == 0)
            {
                this.SelectionLength = 1;
            }

            this.ColoredText = "";
            this.EndUpdate(true);
            this.OnTextChanged(null);
        }

        public new void Cut()
        {
            this.BeginUpdate();
            this.Copy();
            this.ColoredText = "";
            this.EndUpdate(true);
            this.OnTextChanged(null);
        }

        private void SaveUndo(UndoElement el)
        {
            if (this.RedoStack.Count > 0)
            {
                this.RedoStack.Clear();
            }

            this.UndoStack.Push(el);
        }

        public new void Redo()
        {
            if (this.RedoStack.Count == 0)
            {
                return;
            }

            this.MakeAllInvis();

            this.BeginUpdate();

            this.Dirty = true;

            var el = this.RedoStack.Pop();

            this.SelectionStart = el.SelectionStart;
            this.SelectionLength = el.SelectedText.Length;
            this.SelectedText = el.RedoText;

            el.RedoText = null; // dont need it anymore
            this.UndoStack.Push(el);

            var intStartLine = this.GetLineFromCharIndex(el.SelectionStart);
            var intStart = this.GetFirstCharIndexFromLine(intStartLine);

            var intStopLine = this.GetLineFromCharIndex(el.SelectionStart + el.SelectedText.Length);
            if (intStopLine < this.Lines.Length)
            {
                var intLengthStopLine = this.Lines[intStopLine].Length;
                var intStop = this.GetFirstCharIndexFromLine(intStopLine) + intLengthStopLine;

                this.ColorLine(intStart, intStop - intStart);
            }

            this.EndUpdate(true);
            this.Focus();
        }

        public new void Undo()
        {
            if (this.UndoStack.Count == 0)
            {
                return;
            }

            this.MakeAllInvis();

            this.BeginUpdate();

            var el = this.UndoStack.Pop();

            this.Dirty = (this.UndoStack.Count != 0);

            this.SelectionStart = el.SelectionStart;
            this.SelectionLength = el.SelectionLength;
            el.RedoText = this.SelectedText; // save redo text!!!!
            this.SelectedText = el.SelectedText;

            this.RedoStack.Push(el);

            var intStartLine = this.GetLineFromCharIndex(el.SelectionStart);
            var intStart = this.GetFirstCharIndexFromLine(intStartLine);

            var intStopLine = this.GetLineFromCharIndex(el.SelectionStart + el.SelectedText.Length);
            if (intStopLine < this.Lines.Length)
            {
                var intLengthStopLine = this.Lines[intStopLine].Length;
                var intStop = this.GetFirstCharIndexFromLine(intStopLine) + intLengthStopLine;

                this.ColorLine(intStart, intStop - intStart);
            }

            this.EndUpdate(true);
            this.Focus();
        }

        public void ToClipBoard()
        {
            var backup = Properties.Settings.Default.SL4SpacesIndent;
            Properties.Settings.Default.SL4SpacesIndent = true;
            var strFormattedText = AutoFormatter.ApplyFormatting(0, this.Text);
            Properties.Settings.Default.SL4SpacesIndent = backup;

            try
            {
                Clipboard.SetDataObject(strFormattedText, true);
            }
            catch (Exception exception)
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
            if (!this.ToolTipping)
            {
                return;
            }

            this.TooltipKeyboard.Visible = false;
            this.TooltipListBox.Visible = false;
            this.TooltipMouse.Visible = false;
            this.GListBoxWindow.Visible = false;
            this.Focus();
        }

        private void LineDownlighting()
        {
            if (this.HighLightLine < 0)
            {
                return;
            }

            // doof line
            this.BeginUpdate();
            var intBackupSelectionStart = this.SelectionStart;
            var intBackupSelectionLength = this.SelectionLength;
            this.SelectionStart = this.GetFirstCharIndexFromLine(this.HighLightLine);
            this.SelectionLength = this.Lines[this.HighLightLine].Length;
            this.SelectionBackColor = this.BackColor;
            this.SelectionStart = intBackupSelectionStart;
            this.SelectionLength = intBackupSelectionLength;
            this.HighLightLine = -1;
            this.EndUpdate(false);
        }

        private void CursorPositionChanged()
        {
            this.BracketHighlichting();

            this.LineDownlighting();

            OnCursorPositionChanged?.Invoke(this, new CursorPositionEventArgs(this.GetLine(), this.GetColumn(), this.GetChar(), this.GetTotal(), this.blnInsert, Control.IsKeyLocked(Keys.CapsLock)));
        }

        public new void Paste()
        {
            this.ResetHighlighting();

            // First try with Unicode
            if (Clipboard.GetDataObject().GetDataPresent(DataFormats.UnicodeText, true))
            {
                this.ColoredText = Clipboard.GetDataObject().GetData(DataFormats.UnicodeText, true).ToString().Replace("\r", "");
            } // failing that try ANSI text.
            else if (Clipboard.GetDataObject().GetDataPresent(DataFormats.Text, true))
            {
                this.ColoredText = Clipboard.GetDataObject().GetData(DataFormats.Text, true).ToString().Replace("\r", "");
#if DEBUG
                // TODO Add code to show encoding used in a dialogue or the status bar.
#endif
            }
        }

        private void GoEnd(bool blnShift)
        {
            if (this.Lines.Length == 0)
            {
                return;
            }

            var intLast = this.SelectionStart + this.SelectionLength;
            var intLine = this.GetLineFromCharIndex(intLast);
            var intColumn = intLast - this.GetFirstCharIndexFromLine(intLine);
            var strLine = this.Lines[intLine];
            var intAdd = strLine.Length - intColumn;
            if (blnShift)
            {
                this.SelectionLength += intAdd;
            }
            else
            {
                this.SelectionStart = intLast + intAdd;
                this.SelectionLength = 0;
            }
        }

        private void GoHome(bool blnShift)
        {
            if (this.Lines.Length == 0)
            {
                return;
            }

            var intLine = this.GetLineFromCharIndex(this.SelectionStart);
            var strLine = this.Lines[intLine];
            for (var intColumn = 0; intColumn <= strLine.Length; intColumn++)
            {
                // now we are at the front of the line, ex whitespace
                if (intColumn == strLine.Length || strLine[intColumn] > ' ')
                {
                    var intStart = this.GetFirstCharIndexFromLine(intLine) + intColumn;
                    if (intStart == this.SelectionStart) // already here
                    {
                        intStart = this.GetFirstCharIndexFromLine(intLine);
                    }
                    // intStart has now got the new start position
                    if (blnShift)
                    {
                        var intAddToLength = this.SelectionStart - intStart;
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

            if (this.TooltipMouse?.Visible == true)
            {
                this.TooltipMouse.Visible = false;
                this.Focus();
            }

            if (this.GListBoxWindow?.Visible == true)
            {
                this.GListBoxWindow.KeyDownHandler(e);
            }
            else if (e.KeyCode == Keys.Tab)
            {
                var intLineStart = this.GetLineFromCharIndex(this.SelectionStart);
                var intLineStop = this.GetLineFromCharIndex(this.SelectionStart + this.SelectionLength);
                if (intLineStart != intLineStop)
                {
                    this.MultiLineTab(!e.Shift);
                }
                else
                {
                    this.SingleLineTab(!e.Shift);
                }

                e.Handled = true;
            }

            if (e.KeyCode == Keys.Insert)
            {
                this.blnInsert = !this.blnInsert;
                this.CursorPositionChanged();
            }

            if (e.KeyCode == Keys.Left
             || e.KeyCode == Keys.Right
             || e.KeyCode == Keys.End
             || e.KeyCode == Keys.Home)
            {
                this.MakeAllInvis();
            }

            if (e.KeyCode == Keys.Home && !e.Control)
            {
                this.GoHome(e.Shift);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.End && !e.Control)
            {
                this.GoEnd(e.Shift);
                e.Handled = true;
            }

            if (e.KeyCode == Keys.Delete)
            {
                if (e.Control)
                {
                    var intWissel = 0;
                    var intIndex = this.SelectionStart;
                    if (intIndex < this.Text.Length)
                    {
                        var blnWhiteSpace = (this.Text[intIndex] <= ' ');

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
                            {
                                break;
                            }

                            intIndex++;
                        }
                        this.SelectionLength = intIndex - this.SelectionStart;
                    }
                } // control
                else if (e.Shift)
                {
                    if (this.SelectionLength == 0 && this.Lines.Length > 0)
                    {
                        this.BeginUpdate();
                        this.SelectionStart = this.GetFirstCharIndexOfCurrentLine();
                        var intLine = this.GetLineFromCharIndex(this.SelectionStart);
                        this.SelectionLength = this.Lines[intLine].Length + 1;
                        this.EndUpdate(true);
                    }
                    this.Copy();
                } // shift
                else if (this.SelectionLength == 0)
                {
                    this.SelectionLength = 1;
                }
                this.Delete();
                e.Handled = true;
            }

            if (e.KeyCode == Keys.X && e.Control)
            {
                this.Cut();
                e.Handled = true;
            }

            if (this.TooltipKeyboard?.Visible == true)
            {
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down)
                {
                    this.TooltipKeyboard.Visible = false;
                    this.Focus();
                }
            }

            if (e.KeyCode == Keys.Back && e.Alt)
            {
                this.Undo();
            }

            if (e.KeyCode == Keys.Return || e.KeyCode == Keys.Back)
            {
                e.Handled = true;
            }

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
            {
                this.CursorPositionChanged();
            }
        }

        private void ProcessBackSpace()
        {
            if (this.SelectionStart == 0 && this.SelectionLength == 0)
            {
                return;
            }

            this.BeginUpdate();
            if (this.SelectionStart > 0 && this.SelectionLength == 0)
            {
                this.SelectionStart--;
            }

            if (this.SelectionLength == 0)
            {
                this.SelectionLength = 1;
            }

            this.ColoredText = "";
            this.EndUpdate(true);
            this.OnTextChanged(null);
            this.CursorPositionChanged();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            this.ResetHighlighting();

            var keyChar = e.KeyChar;

            switch (e.KeyChar)
            {
                case '\r': // Return
                    if (this.GListBoxWindow.Visible)
                    {
                        this.InsertSelectedWord();
                    }
                    else
                    {
                        this.ProcessEnter();
                    }

                    e.Handled = true;
                    break;
                case '\t': // TAB
                    if (this.GListBoxWindow.Visible)
                    {
                        this.InsertSelectedWord();
                    }

                    e.Handled = true;
                    break;
                case '{':
                case '}':
                    this.ProcessBraces(keyChar);
                    e.Handled = true;
                    break;
                case '\b':
                    this.ProcessBackSpace();
                    this.MakeAllInvis();
                    e.Handled = true;
                    break;
                case (char)27:
                    this.blnEscape = !this.blnEscape;
                    this.MakeAllInvis();
                    break;
                default:
                    if (keyChar >= 32)
                    {
                        if (this.SelectionLength != 0)
                        {
                            this.Delete();
                        }

                        this.SaveUndo(new UndoElement(this.SelectionStart, "", 1, null));
                    }
                    if (keyChar > 32 && !char.IsLetterOrDigit(keyChar))
                    {
                        this.blnEscape = false;
                    }

                    break;
            }
            base.OnKeyPress(e);
        }

        // event generated, make list of codecompletion words
        private void CodeCompletion(string strKeyWord, bool IsRegularExpression)
        {
            if (!this.ToolTipping)
            {
                return;
            }

            this.intKeyWordLength = strKeyWord.Length;

            if (IsRegularExpression)
            {
                this.intKeyWordLength = 0; // else it would delete the word typed so far
            }

            if (strKeyWord.Length == 0 || this.blnEscape)
            {
                this.TooltipListBox.Visible = false;
                this.GListBoxWindow.Visible = false;
                this.Focus();
                return;
            }

            var list = Properties.Settings.Default.CodeCompletion
                ? this.keyWords.KeyWordSearch(strKeyWord, IsRegularExpression)
                : new List<KeyWordInfo>();
            if (Properties.Settings.Default.CodeCompletionUserVar && !IsRegularExpression)
            {
                this.codeCompletion.CodeCompletionUserVar(strKeyWord, this.Text, this.SelectionStart, list);
            }

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

                    if (strKeyWord == "else") // else autocompletion annoys people!!! TODO
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
            foreach (var keyWordInfo in list)
            {
                var glbi = new GListBoxItem(keyWordInfo.name, (int)keyWordInfo.type);
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
            var intSelectionStartBackup = this.SelectionStart;

            var intLineNumber = this.GetLineFromCharIndex(intSelectionStartBackup);
            var intFirstChar = this.GetFirstCharIndexFromLine(intLineNumber);
            var intColumn = intSelectionStartBackup - intFirstChar;

            var strLine = this.Lines[intLineNumber];

            // start or ending in underscore then expand
            var intStart = intColumn;
            while ((intStart > 0) && this.IsAlpha(strLine[intStart - 1]))
            {
                intStart--;
            }

            var intEnd = intColumn;
            while ((intEnd < strLine.Length) && this.IsAlpha(strLine[intEnd]))
            {
                intEnd++;
            }

            if (intStart != intColumn || intEnd != (intStart + this.SelectionLength))
            {
                this.SelectionStart = intFirstChar + intStart;
                this.SelectionLength = intEnd - intStart;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            this.BracketHighlichting();

            // this sucks!!!!
            this.AutoWordSelection = true;
            this.AutoWordSelection = Properties.Settings.Default.AutoWordSelection;

            if (this.ToolTipping)
            {
                this.MakeAllInvis();
            }

            if (e.Clicks == 2)
            {
                this.SelectWordByDoubleClick();
            }
        }

        private void ShowKeyBoardHint()
        {
            if (!this.ToolTipping)
            {
                return;
            }

            if (this.GListBoxWindow.Visible)
            {
                return;
            }

            if (this.blnEscape)
            {
                return;
            }
            var strFunction = this.GetCurrentFunction(out var intArgument, out var DoWild);

            if (strFunction?.Length == 0)
            {
                this.TooltipKeyboard.Visible = false;
                this.Focus();
                return;
            }

            this.SetArgumentListOnRichLabel(this.TooltipKeyboard, strFunction, intArgument);
            if (DoWild && this.TooltipKeyboard.Wild != "")
            {
                this.CodeCompletion(this.TooltipKeyboard.Wild, true);
                this.Focus();
                return;
            }

            if (this.TooltipKeyboard.Text?.Length == 0)
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
            var blnInString = false;
            for (var intI = 0; intI < intIndex; intI++)
            {
                var chrC = strLine[intI];
                if (chrC == '"')
                {
                    blnInString = !blnInString;
                }

                if (blnInString)
                {
                    if (chrC == '\\')
                    {
                        intI++;
                    }

                    continue;
                }
                if (chrC == '/' && (intI + 1) < intIndex && strLine[intI + 1] == '/')
                {
                    return true;
                }
            }
            return false;
        }

        private bool IsInString(string strLine, int intIndex)
        {
            var intQuotes = 0;
            for (var intI = intIndex - 1; intI >= 0; intI--)
            {
                var chrC = strLine[intI];
                if (chrC == '"' && (intI - 1) >= 0 && strLine[intI - 1] != '\\')
                {
                    intQuotes++;
                }
            }
            return intQuotes % 2 != 0;
        }

        protected override void OnTextChanged(EventArgs e)
        {
            if (this.IsUpdating)
            {
                return;
            }

            if (this.Lines.Length == 0)
            {
                this.TabStops();
                return;
            }

            this.Dirty = true;

            var intSelectionStartBackup = this.SelectionStart;

            var intLineNumber = this.GetLineFromCharIndex(intSelectionStartBackup);
            var intFirstChar = this.GetFirstCharIndexFromLine(intLineNumber);
            var intColumn = intSelectionStartBackup - intFirstChar;

            var strLine = this.Lines[intLineNumber];

            this.ColorLine(intFirstChar, strLine.Length);

            if (this.IsInComment(strLine, intColumn))
            {
                return;
            }

            if (this.IsInString(strLine, intColumn))
            {
                return;
            }

            // not in comment and not in string, find the start of a word
            var intStart = intColumn;
            while ((intStart > 0) && this.IsAlpha(strLine[intStart - 1]))
            {
                intStart--;
            }

            var strTypedWord = strLine.Substring(intStart, intColumn - intStart);

            this.CodeCompletion(strTypedWord, false);

            this.ShowKeyBoardHint();
        }

        private void ColorLine(int intStart, int intLength)
        {
            if (!this.ToolTipping)
            {
                return;
            }

            if (intLength <= 0)
            {
                return;
            }

            this.BeginUpdate();

            // Backup position
            var SelectionStartBackup = this.SelectionStart;

            // get the line
            var strLine = this.Text.Substring(intStart, intLength);

            // highlight all keywords
            foreach (Match m in this.keyWords.Matches(strLine))
            {
                var g = m.Groups[1];
                var strKeyWord = g.Value;

                this.SelectionStart = intStart + g.Index;
                this.SelectionLength = g.Length;

                // normal keywords
                this.SelectionColor = this.keyWords.ContainsKeyWord(strKeyWord)
                    ? this.keyWords.GetColorFromKeyWordList(strKeyWord)
                    : this.keyWords.GetColorFromRegex(strKeyWord);
            }

            // Restore position
            this.SelectionLength = 0;
            this.SelectionStart = SelectionStartBackup;
            this.SelectionColor = this.ForeColor;

            this.EndUpdate(false);
        }

        private int AutoFormat(bool OnlySelectedText)
        {
            int intTabs;
            if (OnlySelectedText)
            {
                var strW = this.GetNewWhiteSpace(-1);
                intTabs = strW.Length / AutoFormatter.GetTab().Length;
            }
            else
            {
                this.SelectionStart = 0;
                this.SelectionLength = this.Text.Length;
                intTabs = 0;
            }
            var strFormattedText = AutoFormatter.ApplyFormatting(intTabs, this.SelectedText);

            this.ColoredText = strFormattedText;

            return strFormattedText.Length;
        }

        public void AutoFormatSelectedText()
        {
            this.BeginUpdate();
            var scrollPoint = this.RTBScrollPos;
            var intBackupStart = this.SelectionStart;
            var intSelectionLength = this.AutoFormat(true);
            this.SelectionStart = intBackupStart;
            this.SelectionLength = intSelectionLength;
            this.RTBScrollPos = scrollPoint;
            this.EndUpdate(false);
        }

        public void FormatDocument()
        {
            this.BeginUpdate();
            var scrollPoint = this.RTBScrollPos;
            var intLine = this.GetLineFromCharIndex(this.SelectionStart);
            this.AutoFormat(false);
            this.SelectionStart = Math.Max(0, this.GetFirstCharIndexFromLine(intLine));
            this.RTBScrollPos = scrollPoint;
            this.EndUpdate(false);
        }

        public void ClearUndoStack()
        {
            this.UndoStack.Clear();
            this.Dirty = false;
        }

        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                this.BeginUpdate();
                var intBackup = this.SelectionStart;
                this.SelectionStart = 0;
                this.SelectionLength = this.Text.Length; //TODO
                this.ColoredText = value;
                this.SelectionStart = intBackup;
                this.EndUpdate(true);
            }
        }

        private string GetWhiteSpaceFromCurrentLine()
        {
            var intIndex = this.GetLineFromCharIndex(this.SelectionStart);
            if (intIndex < this.Lines.Length)
            {
                return AutoFormatter.GetWhiteSpaceFromLine(this.Lines[intIndex]);
            }

            return "";
        }

        // code-completion word is selected
        // also comes from mainform (doubleclick), must be public
        public void InsertSelectedWord()
        {
            this.GListBoxWindow.Visible = false;
            this.TooltipListBox.Visible = false;

            if (this.GListBoxWindow.GListBox.SelectedIndex < 0)
            {
                return;
            }

            var intOffset = 0;

            var glbi = this.GListBoxWindow.Selected;
            var strSelected = glbi.ToString();
            if (strSelected?.Length == 0)
            {
                this.Focus();
                return;
            }

            if (glbi.ImageIndex == (int)KeyWordTypeEnum.Events || (strSelected == "default" && this.Text.IndexOf("default") < 0))
            {
                if (strSelected != "default")
                {
                    strSelected = this.keyWords.GetEvent(strSelected);
                }

                var strW = this.GetWhiteSpaceFromCurrentLine();
                strSelected += "\n" + strW + "{\n" + strW + AutoFormatter.GetTab() + "\n" + strW + "}";
                intOffset = -(strW.Length + 2);
            }
            else if (glbi.ImageIndex == (int)KeyWordTypeEnum.Functions)
            {
                if (this.keyWords.GetNumberOfArguments(strSelected) == 0)
                {
                    strSelected += "()";
                }
                else
                {
                    strSelected += "(";
                }
            }
            else if (glbi.ImageIndex == (int)KeyWordTypeEnum.Properties)
            {
                strSelected = "\"" + strSelected.Trim() + "\"";
            }

            this.BeginUpdate();
            this.SelectionStart -= this.intKeyWordLength;
            this.SelectionLength = this.intKeyWordLength;
            this.ColoredText = strSelected;

            this.SelectionStart += intOffset;
            this.EndUpdate(true);

            // if any
            this.ShowKeyBoardHint();

            this.Focus();
        }

        // this comes from the main form
        public void ShowTooltipOnListBox()
        {
            var strSelected = this.GListBoxWindow.Selected.ToString();
            var strDescription = this.keyWords.GetDescription(strSelected);
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
            {
                return;
            }

            if (!this.Visible)
            {
                return;
            }

            if (!this.ToolTipping)
            {
                return;
            }

            if (!Properties.Settings.Default.ToolTip)
            {
                return;
            }

            if (MouseButtons != MouseButtons.None)
            {
                return;
            }

            var strKeyWord = this.GetCurrentKeyWord(true, out var point);

            if (strKeyWord?.Length == 0)
            {
                this.TooltipMouse.Visible = false;
                return;
            }

            if (this.TooltipMouse.Tag.ToString() != strKeyWord)
            {
                this.TooltipMouse.Tag = strKeyWord;
                this.TooltipMouse.Text = this.keyWords.GetDescription(strKeyWord);
            }

            if (this.TooltipMouse.Text?.Length == 0)
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
                return this.m_blnToolTipping;
            }
            set
            {
                this.m_blnToolTipping = value;
                if (this.m_blnToolTipping)
                {
                    this.timer1 = new Timer
                    {
                        Interval = Properties.Settings.Default.ToolTipDelay
                    };
                    this.timer1.Tick += this.timer1_Tick;
                }
            }
        }

        public void SetPosition(Rectangle rect)
        {
            if (!this.ToolTipping)
            {
                return;
            }

            if (this.GListBoxWindow.Visible)
            {
                this.GListBoxWindow.SetPosition(rect, this);
            }

            if (this.TooltipListBox.Visible)
            {
                this.TooltipListBox.SetPosition(rect, this);
            }

            if (this.TooltipKeyboard.Visible)
            {
                this.TooltipKeyboard.SetPosition(rect, this);
            }
        }

        private void SetArgumentListOnRichLabel(TooltipWindow window, string strKeyWord, int intArgument)
        {
            if (window.Tag.ToString() == (strKeyWord + intArgument))
            {
                return; // cached information
            }

            window.Wild = "";
            window.Tag = (strKeyWord + intArgument);
            window.Text = this.keyWords.GetFunctionAndHiglightArgument(strKeyWord, intArgument, out var strWild);
            if (Properties.Settings.Default.CodeCompletionArguments)
            {
                window.Wild = strWild;
            }
        }

        public void Goto(int intLine)
        {
            this.SelectionStart = this.GetFirstCharIndexFromLine(intLine - 1);
            this.Focus();
        }

        public void MultiLineComment(bool blnAdd)
        {
            this.BeginUpdate();

            this.ResetHighlighting();

            var strW = this.GetNewWhiteSpace(-1);
            var intTabs = strW.Length / AutoFormatter.GetTab().Length;

            var intLastLine = this.GetLineFromCharIndex(this.SelectionStart + this.SelectionLength);

            var intLine = this.GetLineFromCharIndex(this.SelectionStart);
            this.SelectionStart = this.GetFirstCharIndexFromLine(intLine);
            var intLength = 0;
            do
            {
                intLength += this.Lines[intLine].Length + 1;
                intLine++;
            } while (intLine <= intLastLine);
            this.SelectionLength = intLength;

            var strSelectedText = AutoFormatter.MultiLineComment(blnAdd, intTabs, this.SelectedText);

            var intBackup = this.SelectionStart;
            this.ColoredText = strSelectedText;
            this.SelectionStart = intBackup;
            this.SelectionLength = strSelectedText.Length;
            this.EndUpdate(true);
        }

        public void MultiLineTab(bool blnAdd)
        {
            var strSelectedText = AutoFormatter.MultiLineTab(blnAdd, this.SelectedText);

            var intBackup = this.SelectionStart;
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
                var strTab = AutoFormatter.GetTab();
                if (this.SelectionStart > strTab.Length)
                {
                    var strBefore = this.Text.Substring(this.SelectionStart - strTab.Length, strTab.Length);
                    if (strBefore == strTab)
                    {
                        var intBackupLength = this.SelectionLength;
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
            {
                this.HighLightList.Add(intIndex);
            }
        }

        private void HighLightCharAt(int intLine, int intColumn, Color color)
        {
            var intIndex = this.GetFirstCharIndexFromLine(intLine) + intColumn;
            this.HighLightCharAt(intIndex, color);
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
            var strLine = AutoFormatter.RemoveComment(this.Lines[intLine]);

            var intNumber = 1;
            var blnWithinString = false;
            while (true)
            {
                intColumn += intDirection;
                while (intColumn < 0)
                {
                    intLine--;
                    if (intLine < 0)
                    {
                        return false; // nothing to do
                    }

                    strLine = AutoFormatter.RemoveComment(this.Lines[intLine]); // get new previous line
                    intColumn = strLine.Length - 1; // place on end of line
                }

                while (intColumn >= strLine.Length)
                {
                    intLine++;
                    if (intLine >= this.Lines.Length)
                    {
                        return false; // nothing to do
                    }

                    strLine = AutoFormatter.RemoveComment(this.Lines[intLine]); // get new previous line
                    intColumn = 0; // place on begin of line
                }

                var chrC = strLine[intColumn];

                if (chrC == '"')
                {
                    if (intDirection < 0 && intColumn > 0
                     && strLine[intColumn - 1] == '\\')
                    {
                        intColumn += intDirection;
                        continue;
                    }
                    blnWithinString = !blnWithinString;
                }

                if (blnWithinString)
                {
                    if (intDirection > 0 && chrC == '\\')
                    {
                        intColumn += intDirection;
                    }

                    continue;
                }

                if (chrC == chrOpening)
                {
                    intNumber++;
                }
                else if (chrC == chrClosing)
                {
                    intNumber--;
                    if (intNumber == 0)
                    {
                        return true;
                    }
                }
            }
        }

        private void ResetHighlighting()
        {
            if (this.HighLightList.Count == 0)
            {
                return;
            }

            this.BeginUpdate();

            var scrollPoint = this.RTBScrollPos;

            var intBackupStart = this.SelectionStart;
            var intBackupLength = this.SelectionLength;
            foreach (var intIndex in this.HighLightList)
            {
                this.HighLightCharAt(intIndex, this.BackColor);
            }

            this.HighLightList.Clear();
            this.SelectionStart = intBackupStart;
            this.SelectionLength = intBackupLength;

            this.RTBScrollPos = scrollPoint;

            this.EndUpdate(false);
        }

        private void BracketHighlichting()
        {
            if (this.TextLength == 0)
            {
                return;
            }

            if (this.SelectionLength != 0)
            {
                return;
            }

            this.BeginUpdate();

            var scrollPoint = this.RTBScrollPos;

            var intSelectionStart = this.SelectionStart;

            // ResetHighlichting without the overhead
            if (this.HighLightList.Count > 0)
            {
                foreach (var intI in this.HighLightList)
                {
                    this.HighLightCharAt(intI, this.BackColor);
                }

                this.HighLightList.Clear();
            }

            int intIndex;
            int intType;

            const string Uppers = "<({[";
            const string Downers = ">)}]";

            var intLine = this.GetLineFromCharIndex(intSelectionStart);
            var intColumn = intSelectionStart - this.GetFirstCharIndexFromLine(intLine);

            var strLine = AutoFormatter.RemoveComment(this.Lines[intLine]);

            if (intColumn <= strLine.Length)
            {
                intIndex = intColumn - 1;
                if (intIndex >= 0)
                {
                    intType = Downers.IndexOf(strLine[intIndex]);
                    if (intType >= 0 && this.MatchingBracket(intLine, intIndex, Downers[intType], Uppers[intType], -1, out var intL, out var intC))
                    {
                        this.HighLightCharAt(intLine, intIndex, Properties.Settings.Default.BracketHighlight);
                        this.HighLightCharAt(intL, intC, Properties.Settings.Default.BracketHighlight);
                    }
                }

                intIndex = intColumn;
                if (intIndex < strLine.Length)
                {
                    intType = Uppers.IndexOf(strLine[intIndex]);
                    if (intType >= 0 && this.MatchingBracket(intLine, intIndex, Uppers[intType], Downers[intType], 1, out var intL, out var intC))
                    {
                        this.HighLightCharAt(intLine, intIndex, Properties.Settings.Default.BracketHighlight);
                        this.HighLightCharAt(intL, intC, Properties.Settings.Default.BracketHighlight);
                    }
                }
            }

            this.SelectionStart = intSelectionStart;
            this.SelectionLength = 0;

            this.RTBScrollPos = scrollPoint;

            this.EndUpdate(false);
        }

        public override string SelectedText
        {
            get
            {
                return base.SelectedText;
            }
            set
            {
                if (!this.IsUpdating)
                {
                    this.SaveUndo(new UndoElement(this.SelectionStart, this.SelectedText, value.Length, null));
                }

                base.SelectedText = value;
            }
        }

        private string ColoredText
        {
            set
            {
                this.BeginUpdate();
                var intFirstCharOfStartLine = this.GetFirstCharIndexOfCurrentLine();
                var intBackup = this.SelectionStart;
                if (this.SelectedText != value)
                {
                    this.Dirty = true;
                    var strTmp = value.Replace("\r", "");
                    this.SaveUndo(new UndoElement(this.SelectionStart, this.SelectedText, strTmp.Length, null));
                    this.SelectedText = strTmp;
                    if (this.Lines.Length > 0)
                    {
                        var intEndLine = this.GetLineFromCharIndex(intBackup + strTmp.Length);
                        var intLastCharOfEndLine = this.GetFirstCharIndexFromLine(intEndLine) + this.Lines[intEndLine].Length;
                        var intLength = intLastCharOfEndLine - intFirstCharOfStartLine;
                        this.ColorLine(intFirstCharOfStartLine, intLength);
                        this.SelectionStart = intBackup + strTmp.Length;
                    }
                }
                else
                {
                    this.SelectionLength = 0;
                    this.SelectionStart += value.Length;
                }
                this.EndUpdate(true);
            }
        }

        public void doOutline()
        {
            //TODO: finish the outline class and such
            //      still a work in progress trying to figure out exactly how i wanna do this.

            var len = this.Lines.Length;
            if (len < 1)
            {
                return;
            }
            var list = new Dictionary<int, LSLEditor.Helpers.OutlineHelper>();
            var ttext = this.Text;
            ttext.Replace("\r\n", "");
            using (var reader = new StringReader(ttext))
            {
                string line;

                var lineNumber = 0;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();
                    line = line.Split('(')[0];
                    foreach (var word in line.Split(' '))
                    {
                        //Debug.WriteLine("ww:" + word);
                        if (this.keyWords.ContainsKeyWord(word))
                        {
                            var k = this.keyWords.GetKeyWordInfo(word);
                            //  Debug.WriteLine("w:" + word);
                            //Debug.WriteLine("k:" + k.type);
                            if (!list.ContainsKey(lineNumber))
                            {
                                switch (k.type)
                                {
                                    case KeyWordTypeEnum.Functions:
                                    case KeyWordTypeEnum.Events:
                                    case KeyWordTypeEnum.Constants:
                                    case KeyWordTypeEnum.Class:
                                    case KeyWordTypeEnum.Vars:
                                    case KeyWordTypeEnum.States:
                                        list.Add(lineNumber, new OutlineHelper(k, lineNumber));
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

            if (this.p != null) //It gives the parent-scriptwindow as null when you try to run it
            {
                this.p.tvOutline.BeginUpdate();
                this.p.tvOutline.Nodes.Clear();
                TreeNode lastState = null;
                TreeNode lastEvent = null;
                TreeNode lastScope = null;

                foreach (var k in list.Values)
                {
                    switch (k.info.type)
                    {
                        case KeyWordTypeEnum.States:
                            lastState = this.createOutlineNode(k);
                            lastScope = lastState;
                            this.p.tvOutline.Nodes.Add(lastState);
                            break;
                        case KeyWordTypeEnum.Events:
                            if (lastState != null) //we need a state for every event!
                            {
                                lastEvent = this.createOutlineNode(k);
                                lastScope = lastEvent;
                                lastState.Nodes.Add(lastEvent);
                            }
                            break;
                        case KeyWordTypeEnum.Functions:
                            if (lastScope != null)
                            {
                                lastScope.Nodes.Add(this.createOutlineNode(k));
                            }
                            break;
                        case KeyWordTypeEnum.Class:
                            if (lastScope != null)
                            {
                                lastScope.Nodes.Add(this.createOutlineNode(k));
                            }
                            else
                            {
                                this.p.tvOutline.Nodes.Add(this.createOutlineNode(k));
                            }
                            break;
                        default:
                            this.p.tvOutline.Nodes.Add(this.createOutlineNode(k));
                            break;
                    }
                }
                this.p.tvOutline.EndUpdate();
                // p.tvOutline.Nodes.Add(states);
                this.p.tvOutline.ExpandAll();
            }
        }

        private TreeNode createOutlineNode(Helpers.OutlineHelper ohOutline)
        {
            var ImageKey = (int)ohOutline.info.type;
            return new TreeNode(string.Format("{0} [{1}]", ohOutline.info.name, ohOutline.line + 1), ImageKey, ImageKey)
            {
                Tag = ohOutline
            };
        }

        public void SaveCurrentFile(string strPath, Encoding enc)
        {
            try
            {
                //Encoding enc = null;
                if (!Directory.Exists(Path.GetDirectoryName(strPath)))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(strPath));
                }

                /*
                {
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
                }
                else
                {
                    enc = Encoding.UTF8;
                }
                 * */

                var sw = enc != Encoding.UTF8
                    ? new StreamWriter(strPath, false, enc)
                    : new StreamWriter(strPath);
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

        public new Encoding LoadFile(string path)
        {
            Encoding fileEncoding = null;
            if (path.StartsWith("http://"))
            {
                var webClient = new System.Net.WebClient();
                this.Text = webClient.DownloadString(path);
            }
            else
            {
                if (File.Exists(path))
                {
                    // TODO needs to be refactored to read the file in once and pass the byte array to be checked.
                    fileEncoding = TextFileEncodingDetector.DetectTextFileEncoding(path, Encoding.UTF8);
                    try
                    {
                        var sr = new StreamReader(path, fileEncoding);
                        this.Text = sr.ReadToEnd();
                        sr.Close();
                    }
                    catch
                    {
                    }
                }
            }
            // Fresh files can not be dirty
            this.ClearUndoStack();
            return fileEncoding;
        }
    }
}
