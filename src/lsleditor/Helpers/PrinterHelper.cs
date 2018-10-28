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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Windows.Forms;

namespace LSLEditor.Helpers
{
	/// <summary>
	/// Data Grid View Printer. Print functions for a datagridview, since MS
	/// didn't see fit to do it.
	/// </summary>
	class PrinterHelper
	{
		//---------------------------------------------------------------------
		// global variables
		//---------------------------------------------------------------------
		#region global variables

		// the data grid view we're printing
		EditForm editForm = null;
		int intCharFrom;
		int intCharTo;
		int intCharPrint;

		// print document
		PrintDocument docToPrint = null;

		// print dialogue
		PrintDialog pd = null;

		// print status items
		int fromPage = 0;
		int toPage = -1;

		// page formatting options
		int pageHeight = 0;
		int pageWidth = 0;
		int printWidth = 0;
		int CurrentPage = 0;
		PrintRange printRange;

		// calculated values
		private float footerHeight = 0;
		private float pagenumberHeight = 0;
		#endregion

		//---------------------------------------------------------------------
		// properties - settable by user
		//---------------------------------------------------------------------
		#region properties

		// Title
		#region title properties

		/// <summary>
		/// Title for this report. Default is empty.
		/// </summary>
		private String title;
		public String Title
		{
			get { return title; }
			set { title = value; docToPrint.DocumentName = title; }
		}

		/// <summary>
		/// Font for the title. Default is Tahoma, 18pt.
		/// </summary>
		private Font titlefont;
		public Font TitleFont
		{
			get { return titlefont; }
			set { titlefont = value; }
		}

		/// <summary>
		/// Foreground color for the title. Default is Black
		/// </summary>
		private Color titlecolor;
		public Color TitleColor
		{
			get { return titlecolor; }
			set { titlecolor = value; }
		}

		/// <summary>
		/// Allow the user to override the title string alignment. Default value is
		/// Alignment - Near;
		/// </summary>
		private StringAlignment titlealignment;
		public StringAlignment TitleAlignment
		{
			get { return titlealignment; }
			set { titlealignment = value; }
		}

		/// <summary>
		/// Allow the user to override the title string format flags. Default values
		/// are: FormatFlags - NoWrap, LineLimit, NoClip
		/// </summary>
		private StringFormatFlags titleformatflags;
		public StringFormatFlags TitleFormatFlags
		{
			get { return titleformatflags; }
			set { titleformatflags = value; }
		}
		#endregion

		// SubTitle
		#region subtitle properties

		/// <summary>
		/// SubTitle for this report. Default is empty.
		/// </summary>
		private String subtitle;
		public String SubTitle
		{
			get { return subtitle; }
			set { subtitle = value; }
		}

		/// <summary>
		/// Font for the subtitle. Default is Tahoma, 12pt.
		/// </summary>
		private Font subtitlefont;
		public Font SubTitleFont
		{
			get { return subtitlefont; }
			set { subtitlefont = value; }
		}

		/// <summary>
		/// Foreground color for the subtitle. Default is Black
		/// </summary>
		private Color subtitlecolor;
		public Color SubTitleColor
		{
			get { return subtitlecolor; }
			set { subtitlecolor = value; }
		}

		/// <summary>
		/// Allow the user to override the subtitle string alignment. Default value is
		/// Alignment - Near;
		/// </summary>
		private StringAlignment subtitlealignment;
		public StringAlignment SubTitleAlignment
		{
			get { return subtitlealignment; }
			set { subtitlealignment = value; }
		}

		/// <summary>
		/// Allow the user to override the subtitle string format flags. Default values
		/// are: FormatFlags - NoWrap, LineLimit, NoClip
		/// </summary>
		private StringFormatFlags subtitleformatflags;
		public StringFormatFlags SubTitleFormatFlags
		{
			get { return subtitleformatflags; }
			set { subtitleformatflags = value; }
		}
		#endregion

		// Footer
		#region footer properties

		/// <summary>
		/// footer for this report. Default is empty.
		/// </summary>
		private String footer;
		public String Footer
		{
			get { return footer; }
			set { footer = value; }
		}

		/// <summary>
		/// Font for the footer. Default is Tahoma, 10pt.
		/// </summary>
		private Font footerfont;
		public Font FooterFont
		{
			get { return footerfont; }
			set { footerfont = value; }
		}

		/// <summary>
		/// Foreground color for the footer. Default is Black
		/// </summary>
		private Color footercolor;
		public Color FooterColor
		{
			get { return footercolor; }
			set { footercolor = value; }
		}

		/// <summary>
		/// Allow the user to override the footer string alignment. Default value is
		/// Alignment - Center;
		/// </summary>
		private StringAlignment footeralignment;
		public StringAlignment FooterAlignment
		{
			get { return footeralignment; }
			set { footeralignment = value; }
		}

		/// <summary>
		/// Allow the user to override the footer string format flags. Default values
		/// are: FormatFlags - NoWrap, LineLimit, NoClip
		/// </summary>
		private StringFormatFlags footerformatflags;
		public StringFormatFlags FooterFormatFlags
		{
			get { return footerformatflags; }
			set { footerformatflags = value; }
		}

		private float footerspacing;
		public float FooterSpacing
		{
			get { return footerspacing; }
			set { footerspacing = value; }
		}
		#endregion

		// Page Numbering
		#region page number properties

		/// <summary>
		/// Include page number in the printout. Default is true.
		/// </summary>
		private bool pageno = true;
		public bool PageNumbers
		{
			get { return pageno; }
			set { pageno = value; }
		}

		/// <summary>
		/// Font for the page number, Default is Tahoma, 8pt.
		/// </summary>
		private Font pagenofont;
		public Font PageNumberFont
		{
			get { return pagenofont; }
			set { pagenofont = value; }
		}

		/// <summary>
		/// Text color (foreground) for the page number. Default is Black
		/// </summary>
		private Color pagenocolor;
		public Color PageNumberColor
		{
			get { return pagenocolor; }
			set { pagenocolor = value; }
		}

		/// <summary>
		/// Allow the user to override the page number string alignment. Default value is
		/// Alignment - Near;
		/// </summary>
		private StringAlignment pagenumberalignment;
		public StringAlignment PaageNumberAlignment
		{
			get { return pagenumberalignment; }
			set { pagenumberalignment = value; }
		}

		/// <summary>
		/// Allow the user to override the pagenumber string format flags. Default values
		/// are: FormatFlags - NoWrap, LineLimit, NoClip
		/// </summary>
		private StringFormatFlags pagenumberformatflags;
		public StringFormatFlags PageNumberFormatFlags
		{
			get { return pagenumberformatflags; }
			set { pagenumberformatflags = value; }
		}

		/// <summary>
		/// Allow the user to select whether to have the page number at the top or bottom
		/// of the page. Default is false: page numbers on the bottom of the page
		/// </summary>
		private bool pagenumberontop = false;
		public bool PageNumberInHeader
		{
			get { return pagenumberontop; }
			set { pagenumberontop = value; }
		}

		/// <summary>
		/// Should the page number be printed on a separate line, or printed on the
		/// same line as the header / footer? Default is false;
		/// </summary>
		private bool pagenumberonseparateline = false;
		public bool PaageNumberOnSeparateLine
		{
			get { return pagenumberonseparateline; }
			set { pagenumberonseparateline = value; }
		}


		#endregion

		// Page Level Properties
		#region page level properties

		/// <summary>
		/// Page margins override. Default is (60, 60, 60, 60)
		/// </summary>
		private Margins printmargins;
		public Margins PrintMargins
		{
			get { return printmargins; }
			set { printmargins = value; }
		}

		#endregion

		#endregion

		/// <summary>
		/// Constructor for PrinterHelper
		/// </summary>
		public PrinterHelper(PageSetupDialog pageSetupDialog)
		{
			// create print document
			docToPrint = new PrintDocument();
			docToPrint.PrintPage += new PrintPageEventHandler(printDoc_PrintPage);

			if (pageSetupDialog.PrinterSettings != null)
				docToPrint.PrinterSettings = pageSetupDialog.PrinterSettings;

			if (pageSetupDialog.PageSettings != null)
				docToPrint.DefaultPageSettings = pageSetupDialog.PageSettings;
			else
				docToPrint.DefaultPageSettings.Margins = new Margins(60, 80, 40, 40);

			printmargins = docToPrint.DefaultPageSettings.Margins;

			// set default fonts
			pagenofont = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
			pagenocolor = Color.Black;
			titlefont = new Font("Tahoma", 10, FontStyle.Bold, GraphicsUnit.Point);
			titlecolor = Color.Black;
			subtitlefont = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
			subtitlecolor = Color.Black;
			footerfont = new Font("Tahoma", 10, FontStyle.Regular, GraphicsUnit.Point);
			footercolor = Color.Black;

			// set default string formats
			titlealignment = StringAlignment.Near;
			titleformatflags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

			subtitlealignment = StringAlignment.Near;
			subtitleformatflags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

			footeralignment = StringAlignment.Near;
			footerformatflags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;

			pagenumberalignment = StringAlignment.Center;
			pagenumberformatflags = StringFormatFlags.NoWrap | StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
		}

		/// <summary>
		/// Start the printing process, print to a printer.
		/// </summary>
		/// <param name="editForm">The EditForm to print</param>
		/// NOTE: Any changes to this method also need to be done in PrintPreviewEditForm
		public void PrintEditForm(EditForm editForm)
		{
			saveFormData(editForm);
			setupPrintDialogue();

			// show print dialog
			if (pd.ShowDialog() == DialogResult.OK)
			{
				SetupPrint(pd);
				docToPrint.Print();
			}

		}

		/// <summary>
		/// Start the printing process, print to a print preview dialog
		/// </summary>
		/// <param name="editForm">The EditForm to print</param>
		/// NOTE: Any changes to this method also need to be done in PrintDataGridView
		public void PrintPreviewEditForm(EditForm editForm)
		{
			saveFormData(editForm);
			setupPrintDialogue();

			// show print dialog
			if (pd.ShowDialog() == DialogResult.OK)
			{
				SetupPrint(pd);
				PrintPreviewDialog ppdialog = new PrintPreviewDialog();
				ppdialog.Document = docToPrint;
				ppdialog.ShowDialog();
			}
		}

		/// <summary>
		/// Set up the print job. Save information from print dialog
		/// and print document for easy access. Also sets up the rows
		/// and columns that will be printed.
		/// </summary>
		/// <param name="pd">The print dialog the user just filled out</param>
		void SetupPrint(PrintDialog pd)
		{
			//-----------------------------------------------------------------
			// save data from print dialog and document
			//-----------------------------------------------------------------

			// check to see if we're doing landscape printing
			if (docToPrint.DefaultPageSettings.Landscape)
			{
				// landscape: switch width and height
				pageHeight = docToPrint.DefaultPageSettings.PaperSize.Width;
				pageWidth = docToPrint.DefaultPageSettings.PaperSize.Height;
			}
			else
			{
				// portrait: keep width and height as expected
				pageHeight = docToPrint.DefaultPageSettings.PaperSize.Height;
				pageWidth = docToPrint.DefaultPageSettings.PaperSize.Width;
			}

			// save printer margins and calc print width
			printmargins = docToPrint.DefaultPageSettings.Margins;
			printWidth = pageWidth - printmargins.Left - printmargins.Right;

			// save print range
			printRange = pd.PrinterSettings.PrintRange;

			// pages to print handles "some pages" option
			if (PrintRange.SomePages == printRange)
			{
				// set limits to only print some pages
				fromPage = pd.PrinterSettings.FromPage;
				toPage = pd.PrinterSettings.ToPage;
			}
			else
			{
				// set extremes so that we'll print all pages
				fromPage = 0;
				toPage = 2147483647;
			}

			//-----------------------------------------------------------------
			// set up the pages to print
			//-----------------------------------------------------------------

			// pages (handles "selection" and "current page" options
			if (PrintRange.Selection == printRange)
			{
				intCharPrint = this.editForm.TextBox.SelectionStart;
				intCharFrom = intCharPrint;
				intCharTo = intCharFrom + this.editForm.TextBox.SelectionLength;
			}
			else if (PrintRange.CurrentPage == printRange)
			{
			}
			// this is the default for print all
			else
			{
				intCharPrint = 0;
				intCharFrom = intCharPrint;
				intCharTo = this.editForm.TextBox.Text.Length;
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
			StringFormat format = new StringFormat();
			format.Trimming = StringTrimming.Word;
			format.Alignment = alignment;
			format.FormatFlags = flags;

			return format;
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
			Rectangle rect = new Rectangle(
				e.MarginBounds.Left,
				e.MarginBounds.Top + e.MarginBounds.Top,
				e.MarginBounds.Width,
				e.MarginBounds.Height - e.MarginBounds.Top - e.MarginBounds.Top);
			PrintPageEventArgs ee = new PrintPageEventArgs(e.Graphics, rect, e.PageBounds, e.PageSettings);
			// Print the content of RichTextBox. Store the last character printed.
			intCharFrom = editForm.TextBox.Print(intCharFrom, intCharTo, ee);

			// increment page number & check page range
			CurrentPage++;

			//-----------------------------------------------------------------
			// print headers
			//-----------------------------------------------------------------

			// reset printpos as it may have changed during the 'skip pages' routine just above.
			float printpos = printmargins.Top;

			// print page number if user selected it
			if (pagenumberontop)
			{
				// if we have a page number to print
				if (pageno)
				{
					// ... then print it
					printsection(e.Graphics, ref printpos, "Page " + CurrentPage.ToString(CultureInfo.CurrentCulture),
						pagenofont, pagenocolor, pagenumberalignment, pagenumberformatflags);

					// if the page number is not on a separate line, don't "use up" it's vertical space
					if (!pagenumberonseparateline)
						printpos -= pagenumberHeight;
				}
			}

			// print title if provided
			if (!String.IsNullOrEmpty(title))
				printsection(e.Graphics, ref printpos, title, titlefont,
					titlecolor, titlealignment, titleformatflags);

			// print subtitle if provided
			if (!String.IsNullOrEmpty(subtitle))
				printsection(e.Graphics, ref printpos, subtitle, subtitlefont,
					subtitlecolor, subtitlealignment, subtitleformatflags);

			//-----------------------------------------------------------------
			// print footer
			//-----------------------------------------------------------------
			printfooter(e.Graphics, ref printpos);

			// Check for more pages
			if (intCharFrom < intCharTo)
				e.HasMorePages = true;
			else
			{
				intCharFrom = intCharPrint; // reset
				CurrentPage = 0;
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
			StringFormat printformat = managestringformat(alignment, flags);

			// measure string
			SizeF printsize = g.MeasureString(text, font, printWidth, printformat);

			// build area to print within
			RectangleF printarea = new RectangleF((float)printmargins.Left, pos, (float)printWidth,
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
			pos = pageHeight - footerHeight - printmargins.Top - printmargins.Bottom;

			// add spacing
			pos += footerspacing;

			// print the footer
			printsection(g, ref pos, footer, footerfont,
				footercolor, footeralignment, footerformatflags);

			// print the page number if it's on the bottom.
			if (!pagenumberontop)
			{
				if (pageno)
				{
					pagenumberHeight = g.MeasureString("M", pagenofont).Height;
					// if the pageno is not on a separate line, push the print location up by its height.
					if (!pagenumberonseparateline)
						pos = pos - pagenumberHeight;

					// print the page number
					printsection(g, ref pos, "Page " + CurrentPage.ToString(CultureInfo.CurrentCulture),
						pagenofont, pagenocolor, pagenumberalignment, pagenumberformatflags);

				}
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
			pd = new PrintDialog();

			pd.Document = docToPrint;
			pd.AllowSelection = true;
			pd.AllowSomePages = false;
			pd.AllowCurrentPage = false;
			pd.AllowPrintToFile = false;
			pd.UseEXDialog = true;
		}
	}
}
