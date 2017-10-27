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
// This class is used to help with paths and LSLI files.
// </summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace LSLEditor.Helpers
{
    static class LSLIPathHelper
    {

        /// <summary>
        /// Checks if a filename is LSLI
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsLSLI(string filename)
        {
            filename = TrimStarsAndWhiteSpace(filename);
            return Path.GetExtension(filename).ToLower() == LSLIConverter.LSLI_EXT;
        }

        /// <summary>
        /// Checks if a filename is an expanded LSL file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static bool IsExpandedLSL(string filename)
        {
            filename = TrimStarsAndWhiteSpace(filename);
            return filename.EndsWith(LSLIConverter.EXPANDED_SUBEXT + LSLIConverter.LSL_EXT);
        }

        /// <summary>
        /// Creates a LSLI scriptname from a filename.
        /// </summary>
        /// <returns></returns>
        public static string CreateCollapsedScriptName(string filename)
        {
            string nameCollapsed = RemoveDotInFrontOfFilename(Path.GetFileNameWithoutExtension(RemoveExpandedSubExtension(filename)) + LSLIConverter.LSLI_EXT);
            return nameCollapsed;
        }
        
        private static string RemoveExtension(string filename)
        {
            //filename = filename.Contains(LSLIConverter.EXPANDED_SUBEXT) ? filename.Replace(LSLIConverter.EXPANDED_SUBEXT, "") : filename; // Is nu een aparte functie voor
            filename = TrimStarsAndWhiteSpace(filename.Remove(filename.LastIndexOf(Path.GetExtension(filename))));
            return filename;
        }

        public static string RemoveExpandedSubExtension(string filename)
        {
            if (filename.Contains(LSLIConverter.EXPANDED_SUBEXT))
            {
                return filename.Replace(LSLIConverter.EXPANDED_SUBEXT, "");
            }

            return filename;
        }

        /// <summary>
        /// Creates a new path and name from the given filename.
        /// E.g. turns path/to/file.expanded.lsl into path/to/file.lsli
        /// </summary>
        /// <returns></returns>
        public static string CreateCollapsedPathAndScriptName(string filename)
        {
            return RemoveDotInFrontOfFilename(RemoveExtension(RemoveExpandedSubExtension(filename)) + LSLIConverter.LSLI_EXT);
        }

        /// <summary>
        /// Creates a new path and name from the original path and name based on the editForm.
        /// E.g. turns path/to/file.lsli into path/to/.file.expanded.lsl
        /// </summary>
        /// <returns></returns>
        public static string CreateExpandedPathAndScriptName(string path)
        {
            return PutDotInFrontOfFilename(RemoveExtension(path) + LSLIConverter.EXPANDED_SUBEXT + LSLIConverter.LSL_EXT);
        }

        /// <summary>
        /// Creates an expanded scriptname out of the given filename.
        /// </summary>
        /// <returns></returns>
        public static string CreateExpandedScriptName(string filename)
        {
            string nameExpanded = "";
            if (filename != null)
            {
                nameExpanded = Path.GetFileNameWithoutExtension(filename) + LSLIConverter.EXPANDED_SUBEXT + LSLIConverter.LSL_EXT;
            }
            
            return PutDotInFrontOfFilename(TrimStarsAndWhiteSpace(nameExpanded));
        }

        private static string PutDotInFrontOfFilename(string filename)
        {
            int afterLastIndexOfSeperator = (filename.LastIndexOf('\\') > filename.LastIndexOf('/') ? filename.LastIndexOf('\\') : filename.LastIndexOf('/')) + 1;

            if (filename.Substring(afterLastIndexOfSeperator, 1) == ".")
            {
                return filename;
            }

            filename = filename.Insert(afterLastIndexOfSeperator, ".");
            return filename;
        }

        public static string RemoveDotInFrontOfFilename(string filename)
        {
            int afterLastIndexOfSeperator = (filename.LastIndexOf('\\') > filename.LastIndexOf('/') ? filename.LastIndexOf('\\') : filename.LastIndexOf('/')) + 1;

            if (filename.Substring(afterLastIndexOfSeperator, 1) != ".")
            {
                return filename;
            }

            filename = filename.Remove(afterLastIndexOfSeperator, 1);
            return filename;
        }

        /// <summary>
        /// "Hides" the file in the folder
        /// </summary>
        /// <param name="path"></param>
        public static void HideFile(string path)
        {
            File.SetAttributes(path, File.GetAttributes(path) | FileAttributes.Hidden);
        }

        /// <summary>
        /// First checks if the file exists, then deletes it
        /// </summary>
        /// <param name="path"></param>
        public static void DeleteFile(string path)
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public static string TrimStarsAndWhiteSpace(string str)
        {
            return str.Trim(' ').TrimEnd('*');
        }

        public static string GetExpandedTabName(string path)
        {
            if (path == null) return "";
            return RemoveDotInFrontOfFilename(Path.GetFileNameWithoutExtension(RemoveExpandedSubExtension(path)) + LSLIConverter.LSLI_EXT + " (Expanded LSL)");
        }
    }
}
