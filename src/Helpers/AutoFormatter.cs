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
using System.Text;

using System.Collections.Generic;

namespace LSLEditor
{
	class AutoFormatter
	{
		public static string GetTab()
		{
			if (Properties.Settings.Default.SL4SpacesIndent)
				return "    ";
			else
				return "\t";
		}

		private static int CountParenthesis(string strLine)
		{
			int intParenthesis=0;
			bool blnWithinString = false;
			for (int intI = 0; intI < strLine.Length; intI++)
			{
				if (strLine[intI] == '"')
					blnWithinString = !blnWithinString;
				if (blnWithinString)
				{
					if (strLine[intI] == '\\')
						intI++;
					continue;
				}
				if (strLine[intI] == '/')
				{
					if(intI<(strLine.Length-1))
					if (strLine[intI + 1] == '/')
						break;
				}
				if (strLine[intI] == '{')
					intParenthesis++;
				if (strLine[intI] == '}')
					intParenthesis--;
			}
			return intParenthesis;
		}

		private static int GetTabCountFromWhiteSpace(string strWhiteSpace)
		{
			int intSpaces = 0;
			for (int intI = 0; intI < strWhiteSpace.Length; intI++)
			{
				if(strWhiteSpace[intI]==' ')
					intSpaces++;
				if (strWhiteSpace[intI] == '\t')
					intSpaces = (int)(6 * (intSpaces / 6)) + 6 - (intSpaces % 6);
			}
			if (Properties.Settings.Default.SL4SpacesIndent)
				return intSpaces / 4;
			else
				return intSpaces / 6;
		}

		public static string GetWhiteSpaceFromLine(string strLine)
		{
			StringBuilder sb = new StringBuilder();
			for (int intI = 0; intI < strLine.Length; intI++)
			{
				if (strLine[intI] > ' ')
					break;
				sb.Append(strLine[intI]);
			}
			return sb.ToString();
		}

		private static int GetTabCountFromLine(string strLine, out int intOnce)
		{
			StringBuilder sb = new StringBuilder();
			int intCountParenthesis = CountParenthesis(strLine);
			if(intCountParenthesis<0)
				intCountParenthesis = 0;
			for (int intI = 0; intI < strLine.Length; intI++)
			{
				if (strLine[intI] > ' ')
					break;
				sb.Append(strLine[intI]);
			}

			intOnce = 0;

			strLine = TrimCommentTrim(strLine);

			int intLength = strLine.Length;
			if (intLength > 0)
			{
				char chrLastChar = strLine[intLength - 1];
				if (strLine[0] == '{' || chrLastChar == ';' || chrLastChar == '{' || chrLastChar == '}')
					intOnce = 0;
				else
					intOnce = 1;
				// this only valid for typing
				if (intCountParenthesis == 0 && chrLastChar == '{')
					intCountParenthesis++;
			}

			return GetTabCountFromWhiteSpace(sb.ToString()) + intCountParenthesis + intOnce;
		}

		public static string RemoveComment(string strLine)
		{
            bool blnWithinString = false;
            for (int intI = 0; intI < (strLine.Length - 1); intI++)
            {
                char chrC = strLine[intI];
                if (chrC == '"')
                    blnWithinString = !blnWithinString;
                if (blnWithinString)
                {
                    if (chrC == '\\')
                        intI++;
                    continue;
                }
                if (chrC != '/')
                    continue;
                if (strLine[intI + 1] == '/')
                {
                    //if(strLine.IndexOf("@include") != intI + 2)
                    //{
                        strLine = strLine.Substring(0, intI);
                    //}
                    break;
                }
            }
            return strLine;
		}

		public static string RemoveCommentsFromLines(string strLines)
		{
			StringBuilder sb = new StringBuilder();
			StringReader sr = new StringReader(strLines);
			while (true)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;
				sb.AppendLine(RemoveComment(strLine));
			}
			return sb.ToString();
		}

		private static string TrimCommentTrim(string strLine)
		{
			return RemoveComment(strLine).Trim();
		}

		public static string GetNewWhiteSpace(string[] lines, int intIndex)
		{
			int intTab = 0;
			int intOnce = 0;
			string strLine = "";
			StringBuilder sb = new StringBuilder();

			while (intIndex>=0 && intIndex<lines.Length)
			{
				strLine = lines[intIndex];
				if (TrimCommentTrim(strLine).Length > 0)
				{
					intTab = GetTabCountFromLine(strLine, out intOnce);
					break;
				}
				intIndex--;
			}

			if (TrimCommentTrim(strLine) != "{")
			{
				intIndex--;
				while (intIndex >= 0 && intIndex < lines.Length)
				{
					strLine = lines[intIndex];
					if (TrimCommentTrim(strLine).Length > 0)
					{
						GetTabCountFromLine(strLine, out intOnce);
						break;
					}
					intIndex--;
				}
			}

			for (int intI = 0; intI < (intTab - intOnce); intI++)
				sb.Append(AutoFormatter.GetTab());

			return sb.ToString();
		}

		public static string ApplyFormatting(int intTab, string strInput)
		{
			StringBuilder sb = new StringBuilder();
			StringReader sr = new StringReader(strInput);

			Stack<int> stack = new Stack<int>();
			stack.Push(intTab);

			int intTemp = 0;
			while (true)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;

				// trim whitespace, this is a clean line
				strLine = strLine.Trim();

				// empty lines do not contain tabs
				if (strLine.Length == 0)
				{
					sb.Append('\n');
					continue;
				}

				// print current line, on current indent level
				int intCorrection = 0;
				if (strLine[0] == '{' || strLine[0] == '}')
					intCorrection--;
				for (int intI = 0; intI < (intTab + intTemp + intCorrection); intI++)
					sb.Append(GetTab());
				sb.Append(strLine);
				sb.Append('\n');


				// calculate next indent level
				strLine = TrimCommentTrim(strLine);

				int intParenthesis = CountParenthesis(strLine);

				if (intParenthesis > 0)
				{
					for (int intP = 0; intP < intParenthesis; intP++)
					{
						stack.Push(intTab);
						if (strLine != "{")
							intTab++;
					}
					intTab += intTemp;
					intTemp = 0;
				}
				else if (intParenthesis < 0)
				{
					if (stack.Count > 0)
						intTab = stack.Pop();
					intTemp = 0;
				}
				else
				{
					if (strLine.Length > 0)
					{
						char chrFirstChar = strLine[0];
						char chrLastChar = strLine[strLine.Length - 1];
						intTemp++;

						if (chrFirstChar == '|' || chrLastChar == '|')
							intTemp = 1;
						if (chrFirstChar == '+' || chrLastChar == '+')
							intTemp = 1;
						if (chrFirstChar == '-' || chrLastChar == '-')
							intTemp = 1;
						if (chrLastChar == ',' || chrLastChar == ',')
							intTemp = 1;
						if (chrLastChar == ';' || chrLastChar == '}')
							intTemp = 0;
					}
				}
			}

			if (!strInput.EndsWith("\n"))
				return sb.ToString().TrimEnd(new char[] { '\n' });
			else
				return sb.ToString();
		}

		public static string MultiLineTab(bool blnAdd, string strText)
		{
			string strPrefix = GetTab();
			StringBuilder sb = new StringBuilder();
			StringReader sr = new StringReader(strText);
			while (true)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;
				if (blnAdd)
				{
					sb.Append(strPrefix);
				}
				else
				{
					if (strLine.StartsWith(strPrefix))
						strLine = strLine.Substring(strPrefix.Length);
				}
				sb.Append(strLine);
				sb.Append('\n');
			}
			return sb.ToString();
		}


		public static string MultiLineComment(bool blnAdd, int intTab, string strText)
		{
			string strPrefix = "//";
			StringBuilder sb = new StringBuilder();
			StringReader sr = new StringReader(strText);
			while (true)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;

				if (blnAdd)
				{
					sb.Append(strPrefix);
				}
				else
				{
					strLine = strLine.Trim();
					if (strLine.StartsWith(strPrefix))
						strLine = strLine.Substring(strPrefix.Length);
				}
				sb.Append(strLine);
				sb.Append('\n');
			}
			return ApplyFormatting(intTab,sb.ToString());
		}

	}
}
