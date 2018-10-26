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
using System.Drawing;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LSLEditor
{
	public enum KeyWordTypeEnum : int
	{
		Unknown = 0,
		Functions = 1,
		Events = 2,
		Constants = 3,
		Class = 4,
		Vars = 5,
		Properties = 6,
        States = 7
	}

	public struct KeyWordInfo
	{
		public KeyWordTypeEnum type;
		public string name;
		public Color color;
		public KeyWordInfo(KeyWordTypeEnum type, string name, Color color)
		{
			this.type = type;
			this.name = name;
			this.color = color;
		}
	}

	class KeyWords
	{
		private ArrayList m_RegexColorList;
		private Regex m_regexSplitter;
		private Hashtable m_KeyWordColorList;
		private XmlDocument xml;

		private void MakeSplitter(string strRegexSplitter)
		{
			m_regexSplitter = new Regex(
				@"\s*(" + strRegexSplitter + @"|\w+|.)",
				RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Compiled
				);
		}

		private struct RegexColor
		{
			public Regex regex;
			public Color color;
			public RegexColor(string strRegex, Color color)
			{
				this.regex = new Regex(strRegex,
					RegexOptions.IgnorePatternWhitespace
					| RegexOptions.Compiled
					);
				this.color = color;
			}
		}

		public KeyWords(string ColorScheme, XmlDocument xml)
		{
			this.m_RegexColorList = new ArrayList();
			this.m_KeyWordColorList = new Hashtable();
			this.xml = xml;

			string strRegexSplitter = "";

			foreach (XmlNode words in xml.SelectNodes("//Words"))
			{
				Color color = Color.FromArgb(int.Parse(words.Attributes[ColorScheme].InnerText.Replace("#", ""), System.Globalization.NumberStyles.HexNumber));
				KeyWordTypeEnum type = KeyWordTypeEnum.Unknown;
				if (words.Attributes["icon"] != null)
				{
					switch (words.Attributes["icon"].InnerText)
					{
						case "Functions":
							type = KeyWordTypeEnum.Functions;
							break;
						case "Events":
							type = KeyWordTypeEnum.Events;
							break;
						case "Constants":
							type = KeyWordTypeEnum.Constants;
							break;
						case "Class":
							type = KeyWordTypeEnum.Class;
							break;
						case "Vars":
							type = KeyWordTypeEnum.Vars;
							break;
                        case "States":
                            type = KeyWordTypeEnum.States;
                            break;
						case "Enum":
							if (!Properties.Settings.Default.CodeCompletionAnimation)
								continue;
							type = KeyWordTypeEnum.Properties;
							break;
						default:
							type = KeyWordTypeEnum.Unknown;
							break;
					}
				}
				foreach (XmlNode word in words.SelectNodes(".//Word"))
				{
					if (word.Attributes["name"].InnerText == "regex")
					{
						string strRegex = word.InnerText;
						AddRegex(strRegex, color);
						if (strRegexSplitter != "")
							strRegexSplitter += "|";
						strRegexSplitter += strRegex;
					}
					else
					{
						AddKeyword(type, word.Attributes["name"].InnerText, color);
					}
				}
			}
			MakeSplitter(strRegexSplitter);
		}

		private void AddRegex(string strRegex, Color color)
		{
			m_RegexColorList.Add(new RegexColor(strRegex, color));
		}

		private void AddKeyword(KeyWordTypeEnum type, string name, Color color)
		{
			m_KeyWordColorList.Add(name, new KeyWordInfo(type, name, color));
		}

		public Color GetColorFromRegex(string strKeyWord)
		{
			// specials
			Color color = Color.Black;
			foreach (RegexColor regexColor in m_RegexColorList)
			{
				Match mm = regexColor.regex.Match(strKeyWord);
				if (mm.Success)
				{
					// must be exact match
					if (mm.Index != 0)
						continue;
					color = regexColor.color;
					break;
				}
			}
			return color;
		}

		public KeyWordInfo GetKeyWordInfo(string strWord)
		{
			return (KeyWordInfo)this.m_KeyWordColorList[strWord];
		}

		public bool ContainsKeyWord(string strWord)
		{
			return m_KeyWordColorList.ContainsKey(strWord);
		}

		public Color GetColorFromKeyWordList(string strKeyWord)
		{
			if (ContainsKeyWord(strKeyWord))
				return GetKeyWordInfo(strKeyWord).color;
			else
				return Color.Black;
		}

		public MatchCollection Matches(string strLine)
		{
			return m_regexSplitter.Matches(strLine);
		}

		public List<KeyWordInfo> KeyWordSearch(string strKeyWord, bool IsRegularExpression)
		{
			List<KeyWordInfo> list = new List<KeyWordInfo>();
			string strLowerCaseKeyWord = strKeyWord.ToLower();
			int intLen = strLowerCaseKeyWord.Length;
			foreach (string strKey in m_KeyWordColorList.Keys)
			{
				if (IsRegularExpression)
				{
					if(new Regex("^"+strKeyWord+"$").Match(strKey).Success)
						list.Add(GetKeyWordInfo(strKey));
				}
				else
				{
					if (strKey.Length < intLen)
						continue;
					if (strKey.Substring(0, intLen).ToLower() == strLowerCaseKeyWord)
						list.Add(GetKeyWordInfo(strKey));
				}
			}
			return list;
		}

		public string GetDescription(string strKeyWord)
		{
			if(ContainsKeyWord(strKeyWord))
			{
				XmlNode xmlNode = xml.SelectSingleNode("//Word[@name='"+strKeyWord+"']");
				if(xmlNode!=null)
				{
					if (xmlNode.ChildNodes.Count == 0)
						return "";
					string strText = xmlNode.InnerXml;
					if (strText == "")
						return strKeyWord;
					else
					{
						int intArgument = strText.IndexOf("<Argument");
						if (intArgument > 0)
							strText = strText.Substring(0, intArgument);
						StringBuilder sb = new StringBuilder();
						StringReader sr = new StringReader(strText);
						while (true)
						{
							string strLine = sr.ReadLine();
							if (strLine == null)
								break;
							sb.AppendLine(strLine.Trim());
						}
						return sb.ToString();
					}
				}
			}
			return "";
		}

		private string MakeArgumentBold(string strText, int intArgument)
		{
			Regex regex = new Regex(
@"(?<prefix>[^(]* \( )
  (?:
      (?<argument> [^,\)]*)  (?"
	+ @"<seperator>[\,\)])+
  )*
(?<postfix>.*) 
",
	RegexOptions.IgnorePatternWhitespace);
			Match match = regex.Match(strText);
			StringBuilder sb = new StringBuilder();
			sb.Append(match.Groups["prefix"].Value);
			for (int intI = 0; intI < match.Groups["argument"].Captures.Count; intI++)
			{
				if (intI == intArgument)
				{
					sb.Append(@"<b><font color=""red"">");
					sb.Append(match.Groups["argument"].Captures[intI].Value);
					sb.Append("</font></b>");
				}
				else
				{
					sb.Append(match.Groups["argument"].Captures[intI].Value);
				}
				sb.Append(match.Groups["seperator"].Captures[intI].Value);
			}
			sb.Append(match.Groups["postfix"].Value);
			sb.Append('\n');
			return sb.ToString();
		}

		private string GetFirstLineOfKeyWord(string strWord, out XmlNode xmlNode)
		{
			xmlNode = null;

			if (!ContainsKeyWord(strWord))
				return "";

			xmlNode = xml.SelectSingleNode("//Word[@name='" + strWord + "']");
			if (xmlNode == null)
				return "";

			if (xmlNode.ChildNodes.Count == 0)
				return "";

			string strText = xmlNode.ChildNodes[0].InnerText;
			if (strText == "")
				return "";

			strText = strText.TrimStart();
			int intI = strText.IndexOf("\r");
			if (intI > 0)
				strText = strText.Substring(0, intI);

			return strText;
		}

		public int GetNumberOfArguments(string strWord)
		{
			XmlNode xmlNode;
			string strFirstLine = GetFirstLineOfKeyWord(strWord, out xmlNode);

			if (strFirstLine == "")
				return -1;

			Regex regex = new Regex(
@"(?<prefix>[^(]* \( )
  (?:
      (?<argument> [^,\)]*)  (?"
	+ @"<seperator>[\,\)])+
  )*
(?<postfix>.*) 
",
				RegexOptions.IgnorePatternWhitespace);
			Match match = regex.Match(strFirstLine);

			// nr not 1, return nr
			int intNr = match.Groups["argument"].Captures.Count;
			if(intNr != 1)
				return intNr;

			// nr = 1 can be void, returns also 0
			if(match.Groups["argument"].Captures[0].Value == "void")
				return 0;

			// not void, return 1
			return 1;
		}

		public string GetFunctionAndHiglightArgument(string strWord, int intArgument, out string strWild)
		{
			XmlNode xmlNode;
			string strFirstLine = GetFirstLineOfKeyWord(strWord,out xmlNode);

			strWild = "";

			if (strFirstLine == "")
				return "";

			string strRichText = MakeArgumentBold(strFirstLine, intArgument);

			if (xmlNode == null)
				return strRichText;

			XmlNodeList nodeList = xmlNode.SelectNodes("./Argument");
			if (intArgument < nodeList.Count)
			{
				XmlNode xmlNodeArgument = nodeList[intArgument];
				if(xmlNodeArgument.Attributes["wild"]!=null)
					strWild = xmlNodeArgument.Attributes["wild"].Value;
				string strName = xmlNodeArgument.Attributes["name"].Value;
				string strDescription = "\n" + @"<b><font color=""red"">" + strName + ":</font></b>";
				strRichText += strDescription + " " + xmlNodeArgument.InnerText + "\n";
			}
			return strRichText;
		}

		public string GetEvent(string strEventName)
		{
			string strReturn = strEventName + "(";

			XmlNode xmlNode;
			string strFirstLine = GetFirstLineOfKeyWord(strEventName, out xmlNode);

			if (strFirstLine == "")
				return strReturn;

			return strFirstLine.Replace(";", "").Replace("void", "");
		}

	}
}
