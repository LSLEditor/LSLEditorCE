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
using System.Windows.Forms;

namespace LSLEditor.Helpers
{
    class LSLIConverter
    {
        private EditForm editForm;
        private const string BEGIN = "//@BEGIN";
        private const string END = "//@END";
        private static List<string> validExtensions = new List<string>() { "lsl", "lsli", ".lsl", ".lsli" }; // TODO: Optimize dit...

        public const string EXPANDED_SUBEXT = ".expanded";
        public const string LSL_EXT = ".lsl";
        public const string LSLI_EXT = ".lsli";
        
        private const string INCLUDE_REGEX = "(\n|^)//@include\\(\".*?\"\\)";
        private const string BEGIN_REGEX = "(\n|^)" + BEGIN; //"(\n|^)//@BEGIN"
        private const string END_REGEX = "(\n|^)" + END;

        private List<string> implementedIncludes = new List<string>();
        private int includeDepth = 0;

        public LSLIConverter()
        {

        }

        /// <summary>
        /// Searches for a file with one of the validExtensions based on a name or path.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>File path</returns>
        private string SearchFile(string file)
        {
            // TODO: Check of settings IncludeFromFolder aanstaat

            if (File.Exists(file))
            {
                return file;
            }

            if (Path.GetExtension(file) == "")
            {
                List<string> extensions = validExtensions.Where(e => e[0] == '.').ToList();
                string pFile = "";

                foreach (string extension in extensions)
                {
                    pFile = file + extension;

                    if (File.Exists(pFile)) {
                        return pFile;
                    }
                }
            }

            return "";
        }

        /// <summary>
        /// Creates a relative path between two paths
        /// </summary>
        /// <param name="filespec">The file or folder to create a relative path towards</param>
        /// <param name="folder">The base folder</param>
        /// <returns></returns>
        private string GetRelativePath(string filespec, string folder)
        {
            filespec = Path.GetFullPath(filespec).ToLower();
            if(validExtensions.Contains(filespec.Substring(filespec.LastIndexOf(".") + 1)))
            {
                int lastIndexOfSeperator = filespec.LastIndexOf('\\') > filespec.LastIndexOf('/') ? filespec.LastIndexOf('\\') : filespec.LastIndexOf('/');
                filespec = filespec.Remove(lastIndexOfSeperator);
            }
            Uri pathUri = new Uri(filespec);

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
        /// This is a hack to get the correct line, since problems arose in WriteAfterLine when inserting index-based
        /// </summary>
        /// <param name="lineBefore"></param>
        /// <returns></returns>
        private int GetCorrectIndexOfLine(string lineBefore, string context) // TODO
        {
            //int correctIndex = -1;
            //if(lineBefore.Trim('\n') == BEGIN)
            //{
            //    correctIndex = context.Where(l => l).LastIndexOf(lineBefore);
            //}
            return context.LastIndexOf(lineBefore);
        }

        /// <summary>
        /// Creates a new line in the context after another line
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newLine"></param>
        /// <param name="lineBefore"></param>
        /// <returns>Context with the new line</returns>
        private StringBuilder WriteAfterLine(StringBuilder context, string newLine, string lineBefore) // TODO: HIJ MOET KIJKEN NAAR DE INDEX VAN LINEBEFORE, NIET ZELF DE INDEX OPZOEKEN
        {
            string ctx = context.ToString();
            //int lastIndexOfLineBefore = ctx.LastIndexOf(lineBefore);
            int lastIndexOfLineBefore = GetCorrectIndexOfLine(lineBefore, ctx);
            int includeIndex = lastIndexOfLineBefore + lineBefore.Length;

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
            string test = context.ToString(); // Debug only
            return context;
        }

        /// <summary>
        /// Creates a new line in the context after a specified index
        /// </summary>
        /// <param name="context"></param>
        /// <param name="newLine"></param>
        /// <param name="indexOfNewLine"></param>
        /// <returns>Context with the new line</returns>
        //private StringBuilder WriteAfterLine(StringBuilder context, string newLine, int indexOfNewLine) // DEZE IS BETER, MAAR IN PRAKTIJK WERKT NIET...
        //{
        //    string ctx = context.ToString();
        //    int includeIndex = indexOfNewLine;

        //    string hasSeperator = context.ToString().Substring(indexOfNewLine - 1, 1);
        //    if (hasSeperator != "\n")
        //    {
        //        newLine = "\n" + newLine;
        //    }

        //    hasSeperator = newLine.Substring(newLine.Length - 1, 1);
        //    if (hasSeperator != "\n")
        //    {
        //        newLine += "\n";
        //    }

        //    context.Insert(includeIndex, newLine);
        //    string test = context.ToString();
        //    return context;
        //}

        /// <summary>
        /// Imports scripts from //@include statements
        /// </summary>
        /// <param name="strC">Sourcecode</param>
        /// <param name="pathOfScript">Path of the source code of the script</param>
        /// <returns>Sourcecode with imported scripts</returns>
        private string ImportScripts(string strC, string pathOfScript) // TODO: Lange functie, kan ik deze opsplitten?
        {
            if(!LSLIPathHelper.IsLSLI(pathOfScript))
            {
                // If it's not LSLI extension it can't import a script
                return strC;
            }

            StringBuilder sb = new StringBuilder(strC);
            MatchCollection mIncludes = Regex.Matches(strC, INCLUDE_REGEX); // Find includes

            foreach (Match m in mIncludes)
            {
                if (this.includeDepth == 0)
                {
                    this.implementedIncludes = new List<string>();
                }
                includeDepth++;

                string contentAfterMatchValue = strC.Substring(m.Index + m.Value.Length);
                int indexOfNewLine = contentAfterMatchValue.IndexOf('\n') + m.Index + m.Value.Length + 1; // Index of the first occurence of \n after this match
                string line = strC.Substring(m.Index, indexOfNewLine - m.Index); // Get full line

                int lineNumber = strC.Take(indexOfNewLine - 1).Count(c => c == '\n') + 1;

                string pathOfIncludeOriginal = Regex.Match(line, "\".*?\"").Value.Trim('"');
                string pathOfInclude = pathOfIncludeOriginal;
                string ext = Path.GetExtension(pathOfInclude).ToLower();

                if ((validExtensions.Contains(ext) || ext == "") && !LSLIPathHelper.CreateExpandedScriptName(pathOfScript).Contains(pathOfInclude)
                    && !pathOfScript.Contains(pathOfInclude))
                {
                    // If path is relative
                    if (!Path.IsPathRooted(pathOfInclude))
                    {
                        pathOfInclude = GetRelativePath(pathOfScript, Environment.CurrentDirectory) + pathOfInclude;
                    }

                    pathOfInclude = SearchFile(pathOfInclude);

                    if (pathOfInclude != "" && !this.implementedIncludes.Contains(Path.GetFullPath(pathOfInclude)))
                    {
                        sb = this.WriteAfterLine(sb, BEGIN, line);

                        // Insert included script
                        string script = "// Empty script\n";
                        using (StreamReader sr = new StreamReader(pathOfInclude))
                        {
                            this.implementedIncludes.Add(Path.GetFullPath(pathOfInclude));
                            string scriptRaw = sr.ReadToEnd();
                            scriptRaw = scriptRaw.Replace("\n", "\n\t");

                            // If there are includes in the included script
                            if (Regex.IsMatch(scriptRaw, INCLUDE_REGEX))
                            {
                                // Then import these scripts too
                                script = "\n" + ImportScripts(scriptRaw, pathOfInclude) + "\n";
                            } else if(scriptRaw != "" && scriptRaw != " ")
                            {
                                script = scriptRaw + "\n";
                            }
                        }
                    
                        this.WriteAfterLine(sb, script, BEGIN + "\n");

                        this.WriteAfterLine(sb, END, script);
                    }
                    else if (pathOfInclude != "" && this.implementedIncludes.Contains(Path.GetFullPath(pathOfInclude)))
                    {
                        string message = "Error: Recursive include loop detected: \"" + Path.GetFullPath(pathOfInclude) +
                            "\". In script \""
                            + Path.GetFileName(pathOfScript) + "\". Line " + lineNumber + ".";

                        if (!editForm.verboseQueue.Contains(message))
                        {
                            MessageBox.Show(message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            editForm.verboseQueue.Add(message);
                        }
                    } else
                    {
                        string correctPath = Path.GetFullPath(GetRelativePath(pathOfScript, Environment.CurrentDirectory) + pathOfIncludeOriginal);
                        string message = "Error: Unable to find file \"" + correctPath +
                            "\". In script \"" + Path.GetFileName(pathOfScript) + "\". Line " + lineNumber + ".";
                        
                        if (!editForm.verboseQueue.Contains(message))
                        {
                            MessageBox.Show(message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            editForm.verboseQueue.Add(message);
                        }
                    }
                }
                includeDepth--;
                if(this.implementedIncludes.Count > 0)
                {
                    this.implementedIncludes.Remove(this.implementedIncludes.Last());
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Removes included scripts
        /// </summary>
        /// <param name="strC">Sourcecode</param>
        /// <returns>Sourcecode without imported scripts</returns>
        private static string RemoveScripts(string strC)
        {
            StringBuilder sb = new StringBuilder(strC);
            
            int indexOfFirstBeginStatement = -1;
            uint depth = 0;
            int readIndex = 0;

            using (StringReader sr = new StringReader(strC))
            {
                int amountOfLines = strC.Split('\n').Length;
                for (int i = 1; i < amountOfLines; i++)
                {
                    string line = sr.ReadLine();

                    if (Regex.IsMatch(line, BEGIN_REGEX))
                    {
                        if (depth == 0)
                        {
                            indexOfFirstBeginStatement = readIndex;
                        }
                        depth++;
                    }

                    readIndex += line.Length + 1;

                    if (Regex.IsMatch(line, END_REGEX))
                    {
                        depth--;

                        if (depth == 0)
                        {
                            sb.Remove(indexOfFirstBeginStatement, (readIndex - indexOfFirstBeginStatement));
                            readIndex -= readIndex - indexOfFirstBeginStatement;
                            indexOfFirstBeginStatement = -1;
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// Call this to collapse LSL to LSLI
        /// </summary>
        /// <param name="editform"></param>
        /// <returns>LSLI</returns>
        public static string CollapseToLSLI(string source)
        {
            string sourceCode = RemoveScripts(source);
            return sourceCode;
        }

        /// <summary>
        /// Call this to collapse LSL to LSLI
        /// </summary>
        /// <param name="editform"></param>
        /// <returns>LSLI</returns>
        public string CollapseToLSLIFromEditform(EditForm editform)
        {
            this.editForm = editform;
            return CollapseToLSLI(editform.SourceCode);
        }

        /// <summary>
        /// Call this to collapse LSL to LSLI
        /// </summary>
        /// <param name="editform"></param>
        /// <returns>LSLI</returns>
        public static string CollapseToLSLIFromPath(string path)
        {
            string sourceCode = "";
            using(StreamReader sr = new StreamReader(path))
            {
                sourceCode = sr.ReadToEnd();
            }

            return CollapseToLSLI(sourceCode);
        }

        /// <summary>
        /// Call this to expand LSLI to LSL
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
