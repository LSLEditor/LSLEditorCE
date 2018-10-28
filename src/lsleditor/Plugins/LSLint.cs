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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
/*
    <p>
        - Added: <b>Plugin support</b><br />
        At this moment only lslint.exe is supported! (get the windows version from <a href="http://w-hat.com/lslint/">http://w-hat.com/lslint/</a>)<br />
        Create a subdirectory 'plugins' in the directory where LSLEditor.exe resides<br />
        Copy lslint.exe into this plugins directory<br />
        Menu Tools - Options - Environment - Plugins - Check lslint checkbox<br />
        When hitting the OK button, Menu - Tools - lslint should appear<br />
    </p>
 
 */
namespace LSLEditor.Plugins
{
    internal class LSLint
    {
        public string ExceptionMessage;
        public bool HasErrors;

        public LSLint()
        {
            this.ExceptionMessage = "";
        }

        private string Execute(string strSourceCode)
        {
            var strAll = "";
            try
            {
                const string strPluginName = "lslint";
                const string strArguments = "";
                var process = new Process();

                var strDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var strPluginsDirectory = Path.Combine(strDirectory, "Plugins");
                var strProgram = Path.Combine(strPluginsDirectory, strPluginName + ".exe");

                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardError = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.FileName = strProgram;
                process.StartInfo.Arguments = strArguments;
                process.Start();
                process.StandardInput.Write(strSourceCode);
                process.StandardInput.Close();
                strAll = process.StandardError.ReadToEnd();
                process.WaitForExit();
            }
            catch (Exception exception)
            {
                this.ExceptionMessage += exception.Message + "\r\n";
            }
            return strAll;
        }

        private void RunOnce(ListView listView, EditForm editForm)
        {
            // ProjectName, ScriptName, 
            var strErrors = this.Execute(editForm.SourceCode);
            var sr = new StringReader(strErrors);
            var intNr = 1;
            while (true)
            {
                var strLine = sr.ReadLine();
                if (strLine == null)
                {
                    break;
                }

                strLine = strLine.Trim();

                if (strLine.StartsWith("TOTAL"))
                {
                    continue;
                }

                var strLineNr = "";
                var strColumnNr = "";
                var strErrorNr = "";

                var intImageIndex = -1;
                if (strLine.StartsWith("WARN"))
                {
                    intImageIndex = 1;
                }

                if (strLine.StartsWith("ERR"))
                {
                    intImageIndex = 0;
                }

                var intI = strLine.IndexOf("::");
                if (intI > 0)
                {
                    strLine = strLine.Substring(intI + 2).Trim();
                    intI = strLine.IndexOf(":");
                    if (intI > 0)
                    {
                        var strLineColumn = strLine.Substring(0, intI).Replace("(", "").Replace(")", "").Replace(" ", "");
                        var LineColumn = strLineColumn.Split(new char[] { ',' });
                        if (LineColumn.Length > 1)
                        {
                            int.TryParse(LineColumn[0], out var intLine);
                            int.TryParse(LineColumn[1], out var intColumn);

                            strLineNr = intLine.ToString();
                            strColumnNr = intColumn.ToString();
                            strErrorNr = intNr.ToString();
                        }
                        strLine = strLine.Substring(intI + 1).Trim();
                    }
                }

                var lvi = new ListViewItem(new string[] {
                        "",							// 0
                        strErrorNr,					// 1
                        strLine,					// 2
                        editForm.ScriptName,		// 3
                        strLineNr,					// 4
                        strColumnNr,				// 5
                        editForm.ProjectName ,		// 6
                        editForm.FullPathName,		// 7
                        editForm.guid.ToString(),	// 8
                        editForm.IsScript.ToString()// 9
                            }, intImageIndex);
                listView.Items.Add(lvi);
                if (strErrorNr != "")
                {
                    intNr++;
                }
            }
        }

        public bool SyntaxCheck(LSLEditorForm parent)
        {
            foreach (var form in parent.Children)
            {
                var editForm = form as EditForm;
                if (editForm?.IsDisposed != false)
                {
                    continue;
                }

                this.RunOnce(parent.SyntaxErrors.ListView, editForm);
            }
            this.HasErrors = (parent.SyntaxErrors.ListView.Items.Count != 0);
            return this.ExceptionMessage?.Length == 0;
        }
    }
}
