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
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace LSLEditor.Helpers
{
	class CompilerHelper
	{
		private static int FindDefaultLineNumber(string strCode)
		{
			StringReader sr = new StringReader(strCode);
			int intI = 0;
			while (true)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;
				if (strLine.StartsWith("class State_default"))
					return intI;
				intI++;
			}
			return intI;
		}

		public static Assembly CompileCSharp(EditForm editForm, string CSharpCode)
		{
			// Code compiler and provider
			CodeDomProvider cc = new CSharpCodeProvider();

			// Compiler parameters
			CompilerParameters cp = new CompilerParameters();

			// Sept 15, 2007 -> 3, 30 oct 2007 -> 4
			if(!Properties.Settings.Default.SkipWarnings)
				cp.WarningLevel = 4;

			// Add common assemblies
			cp.ReferencedAssemblies.Add("System.dll");
			cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");

			// LSLEditor.exe contains all base SecondLife class stuff
			cp.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

			// Compiling to memory
			cp.GenerateInMemory = true;

			// Does this work?
			cp.IncludeDebugInformation = true;

			// Wrap strCSharpCode into my namespace
			string strRunTime = "namespace LSLEditor\n{\n";
			strRunTime += CSharpCode;
			strRunTime += "\n}\n";

			int intDefaultLineNumber = FindDefaultLineNumber(strRunTime);

			// Here we go
			CompilerResults cr = cc.CompileAssemblyFromSource(cp, strRunTime);

			// get the listview to store some errors
			ListView ListViewErrors = editForm.parent.SyntaxErrors.ListView;

			// Console.WriteLine(cr.Errors.HasWarnings.ToString());
			// Check for compilation errors...
			if (ListViewErrors != null && (cr.Errors.HasErrors || cr.Errors.HasWarnings))
			{
				int intNr = 1;
				foreach (CompilerError err in cr.Errors)
				{
					int intLine = err.Line;
					if (intLine < intDefaultLineNumber)
						intLine -= 4;
					else
						intLine -= 5;
					string strError = OopsFormatter.ApplyFormatting(err.ErrorText);
					int intImageIndex = 0;
					if (err.IsWarning)
						intImageIndex = 1;
					ListViewItem lvi = new ListViewItem(new string[] {
						"",						// 0
						intNr.ToString(),		// 1
						strError,				// 2
						editForm.ScriptName,	// 3
						intLine.ToString(),		// 4
						err.Column.ToString(),	// 5
						editForm.ProjectName ,	// 6
						editForm.FullPathName,	// 7
						editForm.guid.ToString(),// 8
						editForm.IsScript.ToString()// 9
							} , intImageIndex);
					ListViewErrors.Items.Add(lvi);
					intNr++;
				}
				return null;
			}
			return cr.CompiledAssembly;
		}
	}
}
