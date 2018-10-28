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
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Text;
using System.Windows.Forms;

namespace LSLEditor.Helpers
{
    /// <summary>
    /// Data Grid View Printer. Print functions for a datagridview, since MS
    /// didn't see fit to do it.
    /// </summary>
    internal class PrinterHelper
    {
        //---------------------------------------------------------------------
        // global variables
        //---------------------------------------------------------------------
        #region global variables

        // the data grid view we're printing
        private EditForm editForm = null;
        private int intCharFrom;
        private int intCharTo;
        private int intCharPrint;

        // print document
        private readonly PrintDocument docToPrint = null;

        // print dialogue
        private PrintDialog pd = null;

        // print status items
        private int fromPage = 0;
        private int toPage = -1;

        // page formatting options
        private int pageHeight = 0;
        private int pageWidth = 0;
        private int printWidth = 0;
        private int CurrentPage = 0;
        private PrintRange printRange;

        // calculated values
        private readonly float footerHeight = 0;
        private float pagenumberHeight = 0;
        #endregion

        //---------------------------------------------------------------------
        // properties - settable by user
        //---------------------------------------------------------------------
        #region properties

        // Title
        #region title properties

        private string title;

        /// <summary>
        /// Title for this report. Default is empty.
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; this.docToPrint.DocumentName = this.title; }
        }

        /// <summary>
        /// Font for the title. Default is Tahoma, 18pt.
        /// </summary>
        public Font TitleFont { get; set; }

        /// <summary>
        /// Foreground color for the title. Default is Black
        /// </summary>
        public Color TitleColor { get; set; }

        /// <summary>
        /// Allow the user to override the title string alignment. Default value is
        /// Alignment - Near;
        /// </summary>
        public StringAlignment TitleAlignment { get; set; }

        /// <summary>
        /// Allow the user to override the title string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags TitleFormatFlags { get; set; }

        #endregion

        #region subtitle properties

        /// <summary>
        /// SubTitle for this report. Default is empty.
        /// </summary>
        public string SubTitle { get; set; }

        /// <summary>
        /// Font for the subtitle. Default is Tahoma, 12pt.
        /// </summary>
        public Font SubTitleFont { get; set; }

        /// <summary>
        /// Foreground color for the subtitle. Default is Black
        /// </summary>
        public Color SubTitleColor { get; set; }

        /// <summary>
        /// Allow the user to override the subtitle string alignment. Default value is
        /// Alignment - Near;
        /// </summary>
        public StringAlignment SubTitleAlignment { get; set; }

        /// <summary>
        /// Allow the user to override the subtitle string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags SubTitleFormatFlags { get; set; }

        #endregion

        #region footer properties

        /// <summary>
        /// footer for this report. Default is empty.
        /// </summary>
        public string Footer { get; set; }

        /// <summary>
        /// Font for the footer. Default is Tahoma, 10pt.
        /// </summary>
        public Font FooterFont { get; set; }

        /// <summary>
        /// Foreground color for the footer. Default is Black
        /// </summary>
        public Color FooterColor { get; set; }

        /// <summary>
        /// Allow the user to override the footer string alignment. Default value is
        /// Alignment - Center;
        /// </summary>
        public StringAlignment FooterAlignment { get; set; }

        /// <summary>
        /// Allow the user to override the footer string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags FooterFormatFlags { get; set; }

        public float FooterSpacing { get; set; }

        #endregion

        // Page Numbering
        #region page number properties

        /// <summary>
        /// Include page number in the printout. Default is true.
        /// </summary>
        public bool PageNumbers { get; set; } = true;

        /// <summary>
        /// Font for the page number, Default is Tahoma, 8pt.
        /// </summary>
        public Font PageNumberFont { get; set; }

        /// <summary>
        /// Text color (foreground) for the page number. Default is Black
        /// </summary>
        public Color PageNumberColor { get; set; }

        /// <summary>
        /// Allow the user to override the page number string alignment. Default value is
        /// Alignment - Near;
        /// </summary>
        public StringAlignment PaageNumberAlignment { get; set; }

        /// <summary>
        /// Allow the user to override the pagenumber string format flags. Default values
        /// are: FormatFlags - NoWrap, LineLimit, NoClip
        /// </summary>
        public StringFormatFlags PageNumberFormatFlags { get; set; }

        /// <summary>
        /// Allow the user to select whether to have the page number at the top or bottom
        /// of the page. Default is false: page numbers on the bottom of the page
        /// </summary>
        public bool PageNumberInHeader { get; set; } = false;

        /// <summary>
        /// Should the page number be printed on a separate line, or printed on the
        /// same line as the header / footer? Default is false;
        /// </summary>
        public bool PaageNumberOnSeparateLine { get; set; } = false;

        #endregion

        // Page Level Properties
        #region page level properties

        /// <summary>
        /// Page margins override. Default is (60, 60, 60, 60)
        /// </summary>
        public Margins PrintMargins { get; set; }

        #endregion

        #endregion

        /// <summary>
        /// Constructor for PrinterHelper
        /// </summary>
        public PrinterHelper(PageSetupDialog pageSetupDialog)
        {
            // create print document
            this.docToPrint = new PrintDocument();
            this.docToPrint.PrintPage += this.printDoc_PrintPage;

            if (pageSetupDialog.PrinterSettings != null)
            {
                this.docToPrint.PrinterSettings = pageSetupDialog.PrinterSettings;
            }

            if (pageSetupDialog.PageSettings != null)
            {
                this.docToPrint.DefaultPageSettings = pageSetupDialog.PageSettings;
            }
            else
            {
                this.docToPrint.DefaultPageSettings.Margins = new Margins(60, 80, 40, 40);
            }

            this.PrintMargins = this.docToPrint.DefaultPageSettings.Margins;

            // set default fonts
            this.PageNumberFont = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
            this.PageNumberColor = Color.Black;
            this.TitleFont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);
            this.TitleColor = Color.Black;
            this.SubTitleFont = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
            this.SubTitleColor = Color.Black;
            this.FooterFont = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
            this.FooterColor = Color.Black;

            // set default string formats
            this.TitleAlignment = StringAlignment.Near;
            this.TitleFormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            this.SubTitleAlignment = StringAlignment.Near;
            this.SubTitleFormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            this.FooterAlignment = StringAlignment.Near;
            this.FooterFormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

            this.PaageNumberAlignment = StringAlignment.Center;
            this.PageNumberFormatFlags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
        }

        /// <summary>
        /// Start the printing process, print to a printer.
        /// </summary>
        /// <param name="editForm">The EditForm to print</param>
        /// NOTE: Any changes to this method also need to be done in PrintPreviewEditForm
        public void PrintEditForm(EditForm editForm)
        {
            this.saveFormData(editForm);
            this.setupPrintDialogue();

            // show print dialog
            if (this.pd.ShowDialog() == DialogResult.OK)
            {
                this.SetupPrint(this.pd);
                this.docToPrint.Print();
            }
        }

        /// <summary>
        /// Start the printing process, print to a print preview dialog
        /// </summary>
        /// <param name="editForm">The EditForm to print</param>
        /// NOTE: Any changes to this method also need to be done in PrintDataGridView
        public void PrintPreviewEditForm(EditForm editForm)
        {
            this.saveFormData(editForm);
            this.setupPrintDialogue();

            // show print dialog
            if (this.pd.ShowDialog() == DialogResult.OK)
            {
                this.SetupPrint(this.pd);
                var ppdialog = new PrintPreviewDialog
                {
                    Document = this.docToPrint
                };
                ppdialog.ShowDialog();
            }
        }

        /// <summary>
        /// Set up the print job. Save information from print dialog
        /// and print document for easy access. Also sets up the rows
        /// and columns that will be printed.
        /// </summary>
        /// <param name="pd">The print dialog the user just filled out</param>
        private void SetupPrint(PrintDialog pd)
        {
            //-----------------------------------------------------------------
            // save data from print dialog and document
            //-----------------------------------------------------------------

            // check to see if we're doing landscape printing
            if (this.docToPrint.DefaultPageSettings.Landscape)
            {
                // landscape: switch width and height
                this.pageHeight = this.docToPrint.DefaultPageSettings.PaperSize.Width;
                this.pageWidth = this.docToPrint.DefaultPageSettings.PaperSize.Height;
            }
            else
            {
                // portrait: keep width and height as expected
                this.pageHeight = this.docToPrint.DefaultPageSettings.PaperSize.Height;
                this.pageWidth = this.docToPrint.DefaultPageSettings.PaperSize.Width;
            }

            // save printer margins and calc print width
            this.PrintMargins = this.docToPrint.DefaultPageSettings.Margins;
            this.printWidth = this.pageWidth - this.PrintMargins.Left - this.PrintMargins.Right;

            // save print range
            this.printRange = pd.PrinterSettings.PrintRange;

            // pages to print handles "some pages" option
            if (PrintRange.SomePages == this.printRange)
            {
                // set limits to only print some pages
                this.fromPage = pd.PrinterSettings.FromPage;
                this.toPage = pd.PrinterSettings.ToPage;
            }
            else
            {
                // set extremes so that we'll print all pages
                this.fromPage = 0;
                this.toPage = 2147483647;
            }

            //-----------------------------------------------------------------
            // set up the pages to print
            //-----------------------------------------------------------------

            // pages (handles "selection" and "current page" options
            if (PrintRange.Selection == this.printRange)
            {
                this.intCharPrint = this.editForm.TextBox.SelectionStart;
                this.intCharFrom = this.intCharPrint;
                this.intCharTo = this.intCharFrom + this.editForm.TextBox.SelectionLength;
            }
            else if (PrintRange.CurrentPage == this.printRange)
            {
            }
            else
            {
                // this is the default for print all
                this.intCharPrint = 0;
                this.intCharFrom = this.intCharPrint;
                this.intCharTo = this.editForm.TextBox.Text.Length;
            }
        }

        /// <summary>
        /// Centralize the string format settings. Does the work of checking for user
        /// overrides, and if they're not present, setting the cell alignment to match
        /// (somewhat) the source control's string alignment.
        /// </summary>
        /// <param name="alignment">String alignment</param>
        /// <param name="flags">String format flags</param>
        /// <param name="controlstyle">DataGridView style to apply (if available)</param>
        /// <param name="overrideformat">True if user overrode alignment or flags</param>
        /// <returns></returns>
        private static StringFormat managestringformat(StringAlignment alignment, StringFormatFlags flags)
        {
            // start with the provided
            return new StringFormat
            {
                Trimming = StringTrimming.Word,
                Alignment = alignment,
                FormatFlags = flags
            };
        }

        /// <summary>
        /// PrintPage event handler. This routine prints one page. It will
        /// skip non-printable pages if the user selected the "some pages" option
        /// on the print dialog.
        /// </summary>
        /// <param name="sender">default object from windows</param>
        /// <param name="e">Event info from Windows about the printing</param>
        private void printDoc_PrintPage(object sender, PrintPageEventArgs e)
        {
            // adjust printing region, make space for headers and footers
            var rect = new Rectangle(
                e.MarginBounds.Left,
                e.MarginBounds.Top + e.MarginBounds.Top,
                e.MarginBounds.Width,
                e.MarginBounds.Height - e.MarginBounds.Top - e.MarginBounds.Top);
            var ee = new PrintPageEventArgs(e.Graphics, rect, e.PageBounds, e.PageSettings);
            // Print the content of RichTextBox. Store the last character printed.
            this.intCharFrom = this.editForm.TextBox.Print(this.intCharFrom, this.intCharTo, ee);

            // increment page number & check page range
            this.CurrentPage++;

            //-----------------------------------------------------------------
            // print headers
            //-----------------------------------------------------------------

            // reset printpos as it may have changed during the 'skip pages' routine just above.
            float printpos = this.PrintMargins.Top;

            // print page number if user selected it
            if (this.PageNumberInHeader)
            {
                // if we have a page number to print
                if (this.PageNumbers)
                {
                    // ... then print it
                    this.printsection(e.Graphics, ref printpos, "Page " + this.CurrentPage.ToString(CultureInfo.CurrentCulture),
                        this.PageNumberFont, this.PageNumberColor, this.PaageNumberAlignment, this.PageNumberFormatFlags);

                    // if the page number is not on a separate line, don't "use up" it's vertical space
                    if (!this.PaageNumberOnSeparateLine)
                    {
                        printpos -= this.pagenumberHeight;
                    }
                }
            }

            // print title if provided
            if (!string.IsNullOrEmpty(this.title))
            {
                this.printsection(e.Graphics, ref printpos, this.title, this.TitleFont,
                    this.TitleColor, this.TitleAlignment, this.TitleFormatFlags);
            }

            // print subtitle if provided
            if (!string.IsNullOrEmpty(this.SubTitle))
            {
                this.printsection(e.Graphics, ref printpos, this.SubTitle, this.SubTitleFont,
                    this.SubTitleColor, this.SubTitleAlignment, this.SubTitleFormatFlags);
            }

            //-----------------------------------------------------------------
            // print footer
            //-----------------------------------------------------------------
            this.printfooter(e.Graphics, ref printpos);

            // Check for more pages
            if (this.intCharFrom < this.intCharTo)
            {
                e.HasMorePages = true;
            }
            else
            {
                this.intCharFrom = this.intCharPrint; // reset
                this.CurrentPage = 0;
                e.HasMorePages = false;
            }
        }

        /// <summary>
        /// Print a header or footer section. Used for page numbers and titles
        /// </summary>
        /// <param name="g">Graphic context to print in</param>
        /// <param name="pos">Track vertical space used; 'y' location</param>
        /// <param name="text">String to print</param>
        /// <param name="font">Font to use for printing</param>
        /// <param name="color">Color to print in</param>
        /// <param name="alignment">Alignment - print to left, center or right</param>
        /// <param name="flags">String format flags</param>
        /// <param name="useroverride">True if the user overrode the alignment or flags</param>
        private void printsection(Graphics g, ref float pos, string text,
            Font font, Color color, StringAlignment alignment, StringFormatFlags flags)
        {
            // string formatting setup
            var printformat = managestringformat(alignment, flags);

            // measure string
            var printsize = g.MeasureString(text, font, this.printWidth, printformat);

            // build area to print within
            var printarea = new RectangleF(this.PrintMargins.Left, pos, this.printWidth,
               printsize.Height);

            // do the actual print
            g.DrawString(text, font, new SolidBrush(color), printarea, printformat);

            // track "used" vertical space
            pos += printsize.Height;
        }

        /// <summary>
        /// Print the footer. This handles the footer spacing, and printing the page number
        /// at the bottom of the page (if the page number is not in the header).
        /// </summary>
        /// <param name="g">Graphic context to print in</param>
        /// <param name="pos">Track vertical space used; 'y' location</param>
        private void printfooter(Graphics g, ref float pos)
        {
            // print last footer. Note: need to force printpos to the bottom of the page
            // as we may have run out of data anywhere on the page
            pos = this.pageHeight - this.footerHeight - this.PrintMargins.Top - this.PrintMargins.Bottom;

            // add spacing
            pos += this.FooterSpacing;

            // print the footer
            this.printsection(g, ref pos, this.Footer, this.FooterFont,
                this.FooterColor, this.FooterAlignment, this.FooterFormatFlags);

            // print the page number if it's on the bottom.
            if (!this.PageNumberInHeader && this.PageNumbers)
            {
                this.pagenumberHeight = g.MeasureString("M", this.PageNumberFont).Height;
                // if the pageno is not on a separate line, push the print location up by its height.
                if (!this.PaageNumberOnSeparateLine)
                {
                    pos -= this.pagenumberHeight;
                }

                // print the page number
                this.printsection(g, ref pos, "Page " + this.CurrentPage.ToString(CultureInfo.CurrentCulture),
                    this.PageNumberFont, this.PageNumberColor, this.PaageNumberAlignment, this.PageNumberFormatFlags);
            }
        }

        private void saveFormData(EditForm editForm)
        {
            // save the datagridview we're printing
            this.editForm = editForm;
            this.intCharFrom = 0;
            this.intCharPrint = 0;
            this.intCharTo = editForm.TextBox.Text.Length;
        }

        private void setupPrintDialogue()
        {
            // create new print dialog
            this.pd = new PrintDialog
            {
                Document = this.docToPrint,
                AllowSelection = true,
                AllowSomePages = false,
                AllowCurrentPage = false,
                AllowPrintToFile = false,
                UseEXDialog = true
            };
        }
    }
}
