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
// This class is used to convert LSLI to LSL and the other way around.
// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace LSLEditor.Helpers
{
    class LSLIConverter
    {
        private EditForm editForm;
        private const string BEGIN = "//@BEGIN";
        private const string END = "//@END";
        private static List<string> validExtensions = new List<string>(3) { "lsl", "lsli", "LSL", "LSLI" };

        public const string EXPANDED_SUBEXT = ".expanded";
        public const string LSL_EXT = ".lsl";
        public const string LSLI_EXT = ".lsli";

        public LSLIConverter()
        {

        }

        /// <summary>
        /// Creates a new path and name from the original path and name based on the editForm.
        /// E.g. turns path/to/file.lsli into path/to/file.expanded.lsl
        /// </summary>
        /// <returns>New path and scriptname</returns>
        public string CreateExpandedPathAndScriptName()
        {
            string nameExpanded = editForm.Text.Remove(editForm.ScriptName.Length - 4, 4).TrimEnd(' ') + EXPANDED_SUBEXT + LSL_EXT;
            nameExpanded = nameExpanded.IndexOf('*') > -1 ? nameExpanded.Remove(nameExpanded.IndexOf('*'), 1) : nameExpanded;
            return editForm.FullPathName.Remove(editForm.FullPathName.Length - 4, 4) + EXPANDED_SUBEXT + LSL_EXT;
        }

        /// <summary>
        /// Creates a relative path between two paths
        /// </summary>
        /// <param name="filespec">The file or folder to create a relative path towards</param>
        /// <param name="folder">The base folder</param>
        /// <returns></returns>
        private string GetRelativePath(string filespec, string folder)
        {
            filespec = Path.GetFullPath(filespec);
            if(validExtensions.Contains(filespec.Substring(filespec.LastIndexOf(".") + 1)))
            {
                int lastIndexOfSeperator = filespec.LastIndexOf('\\') > filespec.LastIndexOf('/') ? filespec.LastIndexOf('\\') : filespec.LastIndexOf('/');
                filespec = filespec.Remove(lastIndexOfSeperator);
            }
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            string relativePath = Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));

            if (relativePath.Substring(relativePath.Length - 3) != Path.DirectorySeparatorChar.ToString())
            {
                relativePath += Path.DirectorySeparatorChar;
            }

            return relativePath;
        }

        /// <summary>
        /// Creates a new line in the context after another line
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newLine"></param>
        /// <param name="lineBefore"></param>
        /// <returns>Context with the new line</returns>
        private StringBuilder WriteAfterLine(StringBuilder context, string newLine, string lineBefore)
        {
            int includeIndex = context.ToString().LastIndexOf(lineBefore) + lineBefore.Length;

            string hasSeperator = lineBefore.Substring(lineBefore.Length - 1, 1);
            if (hasSeperator != "\n")
            {
                newLine = "\n" + newLine;
            }

            hasSeperator = newLine.Substring(newLine.Length - 1, 1);
            if (hasSeperator != "\n")
            {
                newLine += "\n";
            }

            context.Insert(includeIndex, newLine);
            return context;
        }
        
        /// <summary>
        /// Imports scripts from //@include statements
        /// </summary>
        /// <param name="strC">Sourcecode to work with</param>
        /// <param name="pathOfScript">Path of the source code of the script</param>
        /// <returns>Sourcecode with imported scripts</returns>
        private string ImportScripts(string strC, string pathOfScript)
        {
            StringBuilder sb = new StringBuilder(strC);
            string includeMatch = "([\n]|^)+//@include\\(\".*?\"\\)(\\s\\w)?"; // OLD (\n|^)+//@include\\(\".*?\"\\)(\\s?)+(\n|$) MATCHES ONLY 1 INCLUDE
            MatchCollection mIncludes = Regex.Matches(strC, includeMatch); // Find includes
            foreach (Match m in mIncludes)
            {
                string line = m.Value;

                string pathOfInclude = Regex.Match(line, "\".*?\"").Value.Trim('"');
                int lastIndexOfLine = line.LastIndexOf("\n") > -1 && line.LastIndexOf("\n") > line.LastIndexOf(")")
                    ? line.LastIndexOf("\n") + 1 : line.Length;
                
                // Trim end of string if
                if (line.Substring(line.Length - 1, 1) != "\n" && line.Substring(line.Length - 1, 1) != ")")
                {
                    line = line.Remove(line.Length - 1);
                }

                string ext = pathOfInclude.Substring(pathOfInclude.LastIndexOf(".") + 1);

                if (validExtensions.Contains(ext))
                {
                    // If path is relative
                    if(!Path.IsPathRooted(pathOfInclude))
                    {
                        pathOfInclude = GetRelativePath(pathOfScript, Environment.CurrentDirectory) + pathOfInclude;
                    }

                    sb = this.WriteAfterLine(sb, BEGIN, line);

                    string script = "";
                    if (File.Exists(pathOfInclude))
                    {
                        // Insert included script
                        using (StreamReader sr = new StreamReader(pathOfInclude))
                        {
                            string scriptRaw = sr.ReadToEnd();

                            // If there are includes in the included script
                            if (Regex.IsMatch(strC, includeMatch))
                            {
                                // TODO: Kijk voor alle matches, niet alleen 1 match AT: VOLGENS MIJ GEBEURD DIT AL?
                                //Match mInclude = Regex.Match(strC, includeMatch); // Find includes
                                //string IncludePath = Regex.Match(mInclude.ToString(), "\".*?\"").Value.Trim('"');

                                // Then import these scripts too
                                script = "\n" + ImportScripts(scriptRaw, pathOfInclude) + "\n";
                            }
                        }
                    }
                    
                    this.WriteAfterLine(sb, script, BEGIN + "\n");

                    this.WriteAfterLine(sb, END, script);
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Main function of the class. Call this to expand LSLI to LSL
        /// </summary>
        /// <param name="editForm"></param>
        /// <returns>LSL</returns>
        public string ExpandToLSL(EditForm editForm)
        {
            this.editForm = editForm;
            string sourceCode = ImportScripts(editForm.SourceCode, editForm.FullPathName);
            return sourceCode;
        }
    }
}
