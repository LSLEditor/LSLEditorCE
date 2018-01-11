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
// Created by Jasper Wiggerink
// 13-11-2017
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
        private const string BEGIN = "//#BEGIN";
        private const string END = "//#END";
        private const string INCLUDE = "//#include";

        public const string EXPANDED_SUBEXT = ".expanded";
        public const string LSL_EXT = ".lsl";
        public const string LSLI_EXT = ".lsli";
        public static List<string> validExtensions = new List<string>() { LSLI_EXT, LSL_EXT };
        
        private const string INCLUDE_REGEX = "(\n|^)\\s*" + INCLUDE + "\\(\".*?\"\\).*";
        private const string BEGIN_REGEX = "(\\s+|^)" + BEGIN;
        private const string END_REGEX = "(\\s+|^)" + END;
        private const string EMPTY_OR_WHITESPACE_REGEX = "^\\s*$";
        private const string PATH_OF_INCLUDE_REGEX = "\".*?\"";

        private const string EMPTY_SCRIPT = "// Empty script\n";

        private List<string> implementedIncludes = new List<string>();
        private int includeDepth = 0;

        public LSLIConverter()
        {

        }

        /// <summary>
        /// Searches for a file with one of the validExtensions based on a name or path. Also searches in the IncludeDirectories
        /// </summary>
        /// <param name="file"></param>
        /// <returns>File path</returns>
        private static string SearchFile(string file)
        {
            // Search in optional include directories
            foreach (string directory in Properties.Settings.Default.IncludeDirectories)
            {
                string pFile;
                if (file.ToLower().Contains(directory.ToLower()))
                {
                    pFile = file;
                }
                else
                {
                    pFile = directory + file;
                }

                if (File.Exists(pFile))
                {
                    return pFile;
                }

                if (Path.GetExtension(file) == "")
                {
                    foreach (string extension in validExtensions)
                    {
                        if (file.Contains(directory))
                        {
                            pFile = file + extension;
                        }
                        else
                        {
                            pFile = directory + file + extension;
                        }

                        if (File.Exists(pFile))
                        {
                            return pFile;
                        }
                    }
                }
            }

            // Search for file relative to the script
            if (File.Exists(file))
            {
                return file;
            }

            if (Path.GetExtension(file) == "")
            {
                string pFile = "";

                foreach (string extension in validExtensions)
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
        /// Returns the path of the file
        /// </summary>
        /// <param name="pathOfInclude"></param>
        /// <param name="pathOfScript"></param>
        /// <returns></returns>
        private string GetFilePath(string pathOfInclude, string pathOfScript)
        {
            // Step 1 (optional). Search from include directories
            // Step 2. Search from relative path from script

            string pathOfIncludeOriginal = pathOfInclude;
            pathOfInclude = SearchFile(pathOfInclude);

            if (pathOfInclude == "")
            {
                // If path is relative and no includedirectories
                if (!Path.IsPathRooted(pathOfIncludeOriginal))
                {
                    pathOfInclude = LSLIPathHelper.GetRelativePath(pathOfScript, Environment.CurrentDirectory) + pathOfIncludeOriginal;
                }
                else if (this.implementedIncludes.Count > 0) // If there are already includes, the relative path is already correct
                {
                    pathOfInclude = Path.GetDirectoryName(this.implementedIncludes.LastOrDefault()) + '\\' + pathOfIncludeOriginal;
                }
                else
                {
                    pathOfInclude = pathOfIncludeOriginal;
                }

                // If path is absolute it will stay the pathOfInclude
                pathOfInclude = SearchFile(pathOfInclude);
            }

            return pathOfInclude;
        }

        /// <summary>
        /// Finds all indexes of a value in a string
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static List<int> AllIndexesOf(string str, string value)
        {
            if (!String.IsNullOrEmpty(value))
            {
                List<int> indexes = new List<int>();
                for (int index = 0; ; index += value.Length)
                {
                    index = str.IndexOf(value, index);
                    if (index == -1)
                        return indexes;
                    indexes.Add(index);
                }
            }
            return null;
        }

        /// <summary>
        /// Compares 2 paths and returns true if they are different, false if they're the same.
        /// Warning: This doesn't compare extensions.
        /// </summary>
        /// <param name="pathOfInclude"></param>
        /// <param name="pathOfScript"></param>
        /// <returns></returns>
        public static bool IsDifferentScript(string pathOfInclude, string pathOfScript)
        {
            string pathOfScriptNoExt = LSLIPathHelper.RemoveExpandedSubExtension(Path.GetFileNameWithoutExtension(pathOfScript));
            string pathOfIncludeNoExt = LSLIPathHelper.RemoveExpandedSubExtension(Path.GetFileNameWithoutExtension(pathOfInclude));

            // Compare paths
            return !pathOfScriptNoExt.EndsWith(pathOfIncludeNoExt);
        }

        /// <summary>
        /// This is a hack to get the correct line, since problems arose in WriteAfterLine when inserting index-based.
        /// It checks for each occurance of lineBefore (when it's an include statement) if it has a BEGIN after it.
        /// </summary>
        /// <param name="lineBefore"></param>
        /// <returns></returns>
        private int GetCorrectIndexOfLine(string lineBefore, string context)
        {
            string trimmedLine = Regex.Replace(lineBefore, @"\s+", "");
            string matchString = Regex.Match(trimmedLine, INCLUDE_REGEX).ToString();

            // Tussen de één na laatste en de laatste moet de include statement staan, of na de laatste
            int lastButOneNewLineIndex = lineBefore.TrimEnd('\n').LastIndexOf('\n') > -1 ? lineBefore.TrimEnd('\n').LastIndexOf('\n') : 0;
            string lineBeforeAfterLastButOneNewLine = lineBefore.Substring(lastButOneNewLineIndex).TrimEnd('\n'); // Best variable name ever?

            if (Regex.IsMatch(lineBefore.TrimEnd('\n'), INCLUDE_REGEX)
                && Regex.IsMatch(lineBeforeAfterLastButOneNewLine, INCLUDE_REGEX)) // Line before this line is an include statement, that means this is a BEGIN statement //lineBefore.TrimEnd('\n').EndsWith(matchString)
            {
                // Get all matches with this linebefore
                List<int> allIndexes = AllIndexesOf(context, lineBefore);

                foreach (int index in allIndexes)
                {
                    // Check wether there is already a begin statement
                    string targetText = context.Substring(index + lineBefore.Length); // This is the text after lineBefore
                    targetText = Regex.Replace(targetText, @"\s+", "");

                    if (targetText.StartsWith(BEGIN)) // If the targetted text starts with BEGIN, then we should keep searching
                    {
                        continue;
                    } else
                    {
                        return index; // Found a free spot! Return the index
                    }
                }

            }
            return context.LastIndexOf(lineBefore); // If the lineBefore is not an include statement, simply return the last index of it.
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
            string ctx = context.ToString();
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
            return context;
        }

        /// <summary>
        /// Shows an 'Oops...' messagebox with the message and verboses it.
        /// </summary>
        /// <param name="message"></param>
        private void ShowError(string message)
        {
            if (!editForm.verboseQueue.Contains(message))
            {
                MessageBox.Show(message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
                editForm.verboseQueue.Add(message);
            }
        }

        /// <summary>
        /// Returns the amount of tabs for an include depth
        /// </summary>
        /// <param name="includeDepth"></param>
        /// <param name="OneLess"></param>
        /// <returns></returns>
        private string GetTabsForIncludeDepth(int includeDepth, bool OneLess = false)
        {
            string tabs = "";
            if(OneLess && includeDepth != 0)
            {
                includeDepth--;
            }

            for(int i = 0; i < includeDepth; i++)
            {
                tabs += "\t";
            }
            return tabs;
        }

        /// <summary>
        /// Returns the amount of tabs in front of an include statement
        /// </summary>
        /// <param name="includeLine"></param>
        /// <returns></returns>
        private string GetTabsForIndentedInclude(string includeLine, int includeDepth, bool isDebug = false)
        {
            if(includeLine.Contains('\t'))
            {
                int includeIndex = Regex.Match(includeLine, INCLUDE).Index;

                string beforeInclude = includeLine.Substring(0, includeIndex);

                if(beforeInclude.Contains('\n'))
                {
                    // Last '\n' before includeIndex
                    int lastIndexNewLine = beforeInclude.LastIndexOf('\n');
                    beforeInclude = beforeInclude.Substring(lastIndexNewLine, beforeInclude.Length - lastIndexNewLine);
                }

                int tabCount = 0;

                // Count the tabs between the start of the line and the begin of the include.
                if (isDebug)
                {
                    tabCount = beforeInclude.Count(f => f == '\t') - (includeDepth - 1); // The tabcount should be without the includeDepth, because this was already added.
                } else
                {
                    tabCount = beforeInclude.Count(f => f == '\t');
                }

                string tabs = "";
                for (int i = 0; i < tabCount; i++)
                {
                    tabs += "\t";
                }
                return tabs;
            }
            return "";
        }

        /// <summary>
        /// Returns the amount of spaces in front of an include statement
        /// </summary>
        /// <param name="includeLine"></param>
        /// <returns></returns>
        private string GetSpacesForIndentedInclude(string includeLine, int includeDepth, bool isDebug = false)
        {
            if (includeLine.Contains(" "))
            {
                int includeIndex = Regex.Match(includeLine, INCLUDE).Index;

                string beforeInclude = includeLine.Substring(0, includeIndex);
                int spaceCount = 0;

                // Count the space between the start of the line and the begin of the include.
                if (isDebug)
                {
                    // The spacecount should be without the includeDepth, because this was already added.
                    spaceCount = beforeInclude.Count(f => f == ' ') - (includeDepth - 1); 
                }
                else
                {
                    spaceCount = beforeInclude.Count(f => f == ' ');
                }

                string spaces = "";
                for (int i = 0; i < spaceCount; i++)
                {
                    spaces += " ";
                }
                return spaces;
            }
            return "";
        }

        /// <summary>
        /// Returns the amount of tabs/spaces for an include script
        /// </summary>
        /// <param name="includeLine"></param>
        /// <param name="includeDepth"></param>
        /// <param name="OneLess"></param>
        /// <returns></returns>
        private string GetTabsForIncludeScript(string includeLine, int includeDepth, bool OneLess = false)
        {
            string includeDepthTabs = GetTabsForIncludeDepth(includeDepth, OneLess);
            string indentedIncludeTabs = GetTabsForIndentedInclude(includeLine, includeDepth, true);
            string spacesForIndentedInclude = GetSpacesForIndentedInclude(includeLine, includeDepth, true);
            
            return includeDepthTabs + indentedIncludeTabs + spacesForIndentedInclude;
        }
        
        /// <summary>
        /// Returns the line of the match within a context
        /// </summary>
        /// <param name="context"></param>
        /// <param name="m"></param>
        /// <returns></returns>
        private string GetLineOfMatch(string context, Match m)
        {
            string contentAfterMatchValue = context.Substring(m.Index + m.Value.Length);
            int indexOfNewLine = contentAfterMatchValue.IndexOf('\n') + m.Index + m.Value.Length + 1; // Index of the first occurence of \n after this match
            return context.Substring(m.Index, indexOfNewLine - m.Index); // Get full line
        }

        /// <summary>
        /// Inserts an included script and writes the expanded script for export.
        /// </summary>
        /// <param name="pathOfInclude"></param>
        /// <param name="sb"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private StringBuilder WriteExportScript(string pathOfInclude, StringBuilder sb, string includeLine)
        {
            string script = GetTabsForIndentedInclude(includeLine, includeDepth) + EMPTY_SCRIPT;
            using (StreamReader sr = new StreamReader(pathOfInclude))
            {
                this.implementedIncludes.Add(Path.GetFullPath(pathOfInclude));
                string scriptRaw = sr.ReadToEnd();
                scriptRaw = GetTabsForIndentedInclude(includeLine, includeDepth) + scriptRaw.Replace("\n", "\n" + GetTabsForIndentedInclude(includeLine, includeDepth));

                // If there are includes in the included script
                if (Regex.IsMatch(scriptRaw, INCLUDE_REGEX))
                {
                    // Then import these scripts too
                    script = ImportScripts(scriptRaw, pathOfInclude, false) + "\n";
                }
                else if (!Regex.IsMatch(scriptRaw, EMPTY_OR_WHITESPACE_REGEX))
                {
                    script = scriptRaw + "\n";
                }
            }
            this.WriteAfterLine(sb, script, includeLine);
            string ctx = sb.ToString();
            return new StringBuilder(ctx.Remove(ctx.IndexOf(includeLine.TrimStart('\n')), includeLine.TrimStart('\n').Length)); // Deletes the include statement
        }

        /// <summary>
        /// Inserts an included script and writes it to the expanded script for debug.
        /// </summary>
        /// <param name="pathOfInclude"></param>
        /// <param name="sb"></param>
        /// <param name="line"></param>
        /// <returns></returns>
        private StringBuilder WriteDebugScript(string pathOfInclude, StringBuilder sb, string includeLine)
        {
            sb = this.WriteAfterLine(sb, GetTabsForIncludeScript(includeLine, includeDepth, true) + BEGIN, includeLine);

            // Insert included script
            string script = GetTabsForIncludeScript(includeLine, includeDepth) + EMPTY_SCRIPT;

            using (StreamReader sr = new StreamReader(pathOfInclude))
            {
                this.implementedIncludes.Add(Path.GetFullPath(pathOfInclude));
                string scriptRaw = sr.ReadToEnd();
                scriptRaw = GetTabsForIncludeScript(includeLine, includeDepth) + scriptRaw.Replace("\n", "\n" + GetTabsForIncludeScript(includeLine, includeDepth));

                // If there are includes in the included script
                if (Regex.IsMatch(scriptRaw, INCLUDE_REGEX))
                {
                    // Then import these scripts too
                    script = "\n" + ImportScripts(scriptRaw, pathOfInclude) + "\n";
                }
                else if (!Regex.IsMatch(scriptRaw, EMPTY_OR_WHITESPACE_REGEX))// Check if its not empty or whitespace
                {
                    script = scriptRaw + "\n";
                }
            }

            this.WriteAfterLine(sb, script, BEGIN + "\n");
            this.WriteAfterLine(sb, GetTabsForIncludeScript(includeLine, includeDepth, true) + END, script);
            return sb;
        }

        /// <summary>
        /// Imports scripts from //#include statements
        /// </summary>
        /// <param name="strC">Sourcecode</param>
        /// <param name="pathOfScript">Path of the source code of the script</param>
        /// <returns>Sourcecode with imported scripts</returns>
        private string ImportScripts(string strC, string pathOfScript, bool ShowBeginEnd = true)
        {
            if(!LSLIPathHelper.IsLSLI(pathOfScript))
            {
                // If it's not an LSLI script, it can't import a script
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

                string line = GetLineOfMatch(strC, m);
                int lineNumber = strC.Take(m.Index + line.Length - 1).Count(c => c == '\n') + 1;

                string pathOfIncludeOriginal = Regex.Match(line, PATH_OF_INCLUDE_REGEX).Value.Trim('"');
                string pathOfInclude = pathOfIncludeOriginal;
                string ext = Path.GetExtension(pathOfInclude).ToLower();

                if ((validExtensions.Contains(ext) || ext == "") && IsDifferentScript(pathOfInclude, pathOfScript))
                {
                    pathOfInclude = GetFilePath(pathOfInclude, pathOfScript);

                    if (pathOfInclude != "" && !this.implementedIncludes.Contains(Path.GetFullPath(pathOfInclude)))
                    {
                        if (!ShowBeginEnd)
                        {
                            sb = WriteExportScript(pathOfInclude, sb, line);
                        }
                        else
                        {
                            sb = WriteDebugScript(pathOfInclude, sb, line);
                        }
                    }
                    else if (pathOfInclude != "" && this.implementedIncludes.Contains(Path.GetFullPath(pathOfInclude)))
                    {
                        string message = "Error: Recursive include loop detected: \"" + Path.GetFullPath(pathOfInclude) +
                            "\". In script \""
                            + Path.GetFileName(pathOfScript) + "\". Line " + lineNumber + ".";

                        ShowError(message);
                    } else
                    {
                        string relativeToPathOfScript = LSLIPathHelper.GetRelativePath(pathOfScript, Environment.CurrentDirectory);
                        string correctPath = Path.GetFullPath(relativeToPathOfScript) + pathOfIncludeOriginal;
                        string message = "Error: Unable to find file \"" + correctPath +
                            "\". In script \"" + Path.GetFileName(pathOfScript) + "\". Line " + lineNumber + ".";

                        ShowError(message);
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
        public string ExpandToLSL(EditForm editForm, bool ShowBeginEnd = true)
        {
            editForm.verboseQueue = new List<string>();
            this.editForm = editForm;
            string strC = editForm.SourceCode;
            string fullPathName = editForm.FullPathName;

            if (LSLIPathHelper.IsExpandedLSL(editForm.ScriptName))
            {
                // Collapse first, to ensure it is expanded showing or not showing begin/end.
                strC = CollapseToLSLI(strC);
                // Mimic LSLI file
                fullPathName = LSLIPathHelper.CreateCollapsedPathAndScriptName(fullPathName);
            }

            string sourceCode = ImportScripts(strC, fullPathName, ShowBeginEnd);
            return sourceCode;
        }
    }
}
