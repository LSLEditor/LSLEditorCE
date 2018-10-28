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
// About.cs
// Provides the code for the About dialogue
// </summary>

using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace LSLEditor
{
	/// <summary>
	/// About dialogue box form.
	/// </summary>
	public partial class About : Form
	{
		/// <summary>
		/// Initialises a new instance of the <see cref="About" /> class.
		/// </summary>
		/// <param name="parent">The parent form.</param>
		[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed.")]
		public About(LSLEditorForm parent)
		{
			this.InitializeComponent();

			this.Icon = parent.Icon;

			string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			this.label2.Text = strVersion;
		}

		/// <summary>
		/// OK/Close button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Link to SourceForge page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(Properties.Settings.Default.ContactUrl);
		}

		/// <summary>
		/// Loads the page.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void About_Load(object sender, EventArgs e)
		{
			string strExeFileName = Path.GetFileName(Assembly.GetExecutingAssembly().CodeBase);
			this.webBrowser1.Navigate("res://" + strExeFileName + "/" + Properties.Settings.Default.About);
		}
	}
}