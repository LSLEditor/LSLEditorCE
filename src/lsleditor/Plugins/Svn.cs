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
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;

using Microsoft.Win32;

using System.IO;
using System.Diagnostics;

namespace LSLEditor
{
	class Svn
	{
		public string Output;
		public string Error;

		// HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\App Paths\svn.exe
		// HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\App Paths\svn.exe
		public static string Executable
		{
			get
			{
				try
				{
					string strKey = @"Software\Microsoft\Windows\CurrentVersion\App Paths\svn.exe";
					RegistryKey key = Registry.CurrentUser.OpenSubKey(strKey, false);
					if (key == null)
						return null;
					string strExe = key.GetValue("").ToString();
					key.Close();
					return strExe;
				}
				catch
				{
					return null;
				}
			}
		}

		public static bool IsInstalled
		{
			get
			{
				return (Svn.Executable != null);
			}
		}

		public bool Execute(string Arguments, bool ShowOutput, bool ShowError)
		{
			this.Error = "";
			this.Output = "";

			Process p = new Process();
			StreamWriter sw;
			StreamReader sr;
			StreamReader err;

			p.StartInfo.FileName = Properties.Settings.Default.SvnExe;
			p.StartInfo.Arguments = Arguments;
			p.StartInfo.RedirectStandardError = true;
			p.StartInfo.RedirectStandardInput = true;
			p.StartInfo.RedirectStandardOutput = true;
			p.StartInfo.UseShellExecute = false;
			p.StartInfo.CreateNoWindow = true;

			p.Start();

			sw = p.StandardInput;
			sr = p.StandardOutput;
			err = p.StandardError;

			sw.AutoFlush = true;

			StringBuilder sb = new StringBuilder();

			while (sr.Peek() >= 0)
			{
				char[] buffer = new char[1024];
				sr.Read(buffer, 0, buffer.Length);
				sb.Append(new string(buffer));
			}
			sw.Close();
			this.Output = sb.ToString();

			sb = new StringBuilder();
			while (err.Peek() >= 0)
			{
				char[] buffer = new char[4096];
				err.Read(buffer, 0, buffer.Length);
				sb.Append(new string(buffer));
			}
			this.Error = sb.ToString();

			if (this.Error != "" && ShowError)
				MessageBox.Show(this.Error, "SVN Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			
			if (this.Output != "" && ShowOutput)
					MessageBox.Show(this.Output, "SVN Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

			return (this.Error == "");
		}
	}
}
