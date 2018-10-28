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
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace LSLEditor
{
	internal class LSL2CSharp
	{
		public List<string> States;
		private readonly XmlDocument xml;

		public LSL2CSharp(XmlDocument xml)
		{
			this.xml = xml;
			this.States = new List<string>();
		}

		private string CorrectGlobalEvaluator(Match m)
		{
			var strPrefix = m.Groups["prefix"].Value;
			var strPublic = m.Groups["public"].Value;
			var strPostfix = m.Groups["postfix"].Value;
			if (strPublic.EndsWith(";")) {
				return strPrefix + "public static " + strPublic + strPostfix;
			}
			// has to be static,
			// because the vars must keep their values between state changes

			// 22 june 2007, added
			var regex = new Regex(@"\w*", RegexOptions.IgnorePatternWhitespace);
			var intCount = 0;
			for (var pm = regex.Match(strPublic); pm.Success; pm = pm.NextMatch()) {
				if (pm.Value.Length > 0) {
					intCount++;
				}

				if (intCount > 1) {
					break;
				}
			}
			if (intCount == 1) {
				return strPrefix + "public void " + strPublic + strPostfix;
			} else {
				return strPrefix + "public " + strPublic + strPostfix;
			}
		}

		private string CorrectGlobal(string strC)
		{
			var regex = new Regex(
				@"(?<prefix>\s*)
(?:
   (?<public>[^{};]*;)

|

(?<public>[^{;(]*)
   (?<postfix>
 \(  [^{]*  \)  \s*
 \{
	(?>
		[^{}]+ 
		|    \{ (?<number>)
		|    \} (?<-number>)
	)*
	(?(number)(?!))
\}
)
)",
				RegexOptions.IgnorePatternWhitespace);
			return regex.Replace(strC, new MatchEvaluator(this.CorrectGlobalEvaluator));
		}

		private string RemoveComments(string strC)
		{
			if (Properties.Settings.Default.CommentCStyle) {
				var intI = strC.IndexOf("/*");
				while (intI > 0) {
					var intJ = strC.IndexOf("*/", intI);
					if (intJ < 0) {
						break;
					}

					strC = strC.Remove(intI, intJ - intI + 2);
					intI = strC.IndexOf("/*");
				}
			}
			return AutoFormatter.RemoveCommentsFromLines(strC);
		}

		private string CorrectStates(string strC, string strGlobalClass)
		{
			// Default state
			var regex = new Regex(@"^\s*(default)(\W)",
				RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			strC = regex.Replace(strC, "class State_$1 : " + strGlobalClass + "$2");

			// Other states
			regex = new Regex(
				@"^state\s+([^\s]*)(\s*\{)",
				RegexOptions.Multiline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			var m = regex.Match(strC);
			while (m.Success) {
				var strStateName = m.Groups[1].ToString();
				this.States.Add(strStateName);
				m = m.NextMatch();
			}
			strC = regex.Replace(strC, "class State_$1 : " + strGlobalClass + "$2");

			var strGlobal = "";

			if (!Properties.Settings.Default.StatesInGlobalFunctions) {
				var intDefault = strC.IndexOf("class State_default");
				if (intDefault >= 0) {
					strGlobal = strC.Substring(0, intDefault);
					strC = strC.Substring(intDefault);
				}
			}

			// State change, excluding global functions
			regex = new Regex(
				@"(\s+)state\s+(\w+)(\s*;)",
				RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			return strGlobal + regex.Replace(strC, @"$1state(""$2"")$3");
		}

		private string PreCorrectReservedWords(string strC)
		{
			#region All PreCorrect reserved C# words
			var regex = new Regex(@"(\b)(public
|		class
|		override
|		namespace
|		void
|		SecondLife
|		GlobalClass
|		static
|		goto
|		String
|		Float

)(\b)",
			 RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			#endregion
			return regex.Replace(strC, "$1_$2$3");
		}

		private string CorrectReservedWords(string strC)
		{
			#region All reserved C# words
			var regex = new Regex(@"(\b)(new
|		abstract
|		as
|		base
|		bool
|		break
|		byte
|		case
|		catch
|		char
|		checked
|		const
|		continue
|		decimal
|		delegate
|		double
|		enum
|		event
|		explicit
|		extern
|		false
|		finally
|		fixed
|		foreach
|		implicit
|		in
|		int
|		interface
|		internal
|		is
|		lock
|		long
|		new
|		null
|		object
|		operator
|		out
|		params
|		private
|		protected
|		readonly
|		ref
|		sbyte
|		sealed 
|		short
|		sizeof
|		stackalloc
|		struct
|		switch
|		this
|		throw
|		true 
|		try
|		typeof
|		uint
|		ulong
|		unchecked 
|		unsafe
|		ushort
|		using
|		virtual

)(\b)",
			 RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			#endregion
			return regex.Replace(strC, "$1_$2$3");
		}

		private string CorrectEvent(string strC, string strName)
		{
			var regex = new Regex(
				@"([^\w_])" + strName + @"(\s*)\(",
				RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			return regex.Replace(strC, "$1public override void " + strName + "$2(");
		}

		private string CorrectEvents(string strC)
		{
			var words = this.xml.SelectSingleNode("//Words[@name='Appendix B. Events']");
			foreach (XmlNode xmlNode in words.SelectNodes(".//Word")) {
				var strName = xmlNode.Attributes["name"].InnerText;
				strC = this.CorrectEvent(strC, strName);
			}
			return strC;
		}

		// old vector parser
		// <([^<>,;]*),([^<>,;]*),([^<>,;]*)>
		private string CorrectVector(string strC)
		{
			var regex = new Regex(@"
<
   (?<vector_x>
	  (?>
			  [^=(),>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
,
   (?<vector_y>
	  (?>
			  [^=(),>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
,
   (?<vector_z>
	  (?>
			  [^=()>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
>
",
				RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			return regex.Replace(strC, "new vector(${vector_x},${vector_y},${vector_z})");
		}

		// old rotation
		// <([^<>,;]*),([^<>,;]*),([^<>,;]*),([^<>,;]*)>
		private string CorrectRotation(string strC)
		{
			var regex = new Regex(@"
<
   (?<rotation_x>
	  (?>
			  [^=(),>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
,
   (?<rotation_y>
	  (?>
			  [^=(),>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
,
   (?<rotation_z>
	  (?>
			  [^=(),>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
,
   (?<rotation_s>
	  (?>
			  [^=()>]+
		 |    \( (?<nr>)
		 |    \) (?<-nr>)
		 |    ,
	  )*
	  (?(nr)(?!))
   )
>
",
				RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			return regex.Replace(strC, "new rotation(${rotation_x},${rotation_y},${rotation_z},${rotation_s})");
		}

		private string CorrectQuaternion(string strC)
		{
			var regex = new Regex(@"(\b)quaternion(\b)",
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
			return regex.Replace(strC, "$1rotation$2");
		}

		private string CorrectListsEvaluator(Match m)
		{
			var strValue = m.Value;
			return "new list(new object[] {" + this.CorrectLists(strValue.Substring(1, strValue.Length - 2)) + "})";
		}

		private string CorrectLists(string strC)
		{
			var regex = new Regex(
				@"
\[
	(?>
		[^\[\]]+ 
		|    \[ (?<number>)
		|    \] (?<-number>)
	)*
	(?(number)(?!))
\]
",
				RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
			return regex.Replace(strC, new MatchEvaluator(this.CorrectListsEvaluator));
		}

		// changed 16 aug 2007
		private string CorrectJump(string strC)
		{
			// jump -> goto
			var regex = new Regex(
				@"(\b)jump(\s+)([^;]*;)",
				RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);
			strC = regex.Replace(strC, "$1goto$2_$3");

			// @label; -> label:;
			regex = new Regex(
				@"@\s*([a-z0-9_]+)\s*;",
				RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
			return regex.Replace(strC, "_$1:;");
		}

		private string RemoveQuotedStrings(string strC, out List<string> h)
		{
			h = new List<string>();
			var sb = new StringBuilder();
			StringBuilder QuotedString = null;
			for (var intI = 0; intI < strC.Length; intI++) {
				var chrC = strC[intI];
				if (chrC == '"') {
					if (QuotedString != null) {
						// end of a quoted string
						sb.Append('"');
						sb.Append(h.Count.ToString());
						sb.Append('"');
						h.Add(QuotedString.ToString());
						QuotedString = null;
						continue;
					} else {
						if (chrC == '"') {
							// start of a new quoted string
							QuotedString = new StringBuilder();
							continue;
						}
						// it was just a newline char, and not in a string
					}
				}

				if (QuotedString == null) {
					sb.Append(chrC);
				} else {
					if (chrC == '\n') {
						QuotedString.Append('\\');
						chrC = 'n';
					}
					if (chrC != '\\') {
						QuotedString.Append(chrC);
					} else // it is a backslash
					  {
						intI++;
						chrC = strC[intI];
						if (chrC == 't') // tabs are 4 spaces in SL world!!
						{
							QuotedString.Append("    ");
						} else // nope, it is no tab, just output it all
						  {
							QuotedString.Append('\\');
							QuotedString.Append(chrC);
						}
					}
				}
			}
			return sb.ToString();
		}

		private string InsertQuotedStrings(string strC, List<string> h)
		{
			var sb = new StringBuilder();
			StringBuilder QuotedString = null;
			for (var intI = 0; intI < strC.Length; intI++) {
				var chrC = strC[intI];
				if (chrC == '"') {
					if (QuotedString == null) {
						QuotedString = new StringBuilder();
					} else {
						sb.Append('"');
						// State("default") is not a number, result of 'CorrectStates'
						if (int.TryParse(QuotedString.ToString(), out var intNumber)) {
							sb.Append(h[intNumber]);
						} else {
							sb.Append(QuotedString.ToString());
						}

						sb.Append('"');
						QuotedString = null;
					}
					continue;
				}

				if (QuotedString == null) {
					sb.Append(chrC);
				} else {
					QuotedString.Append(chrC);
				}
			}
			return sb.ToString();
		}

		private string MakeGlobalAndLocal(string strC)
		{
			var regexDefault = new Regex(@"^\s*(default)\W",
				RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Multiline
				| RegexOptions.Compiled);

			var matchDefault = regexDefault.Match(strC);

			string strGlobal;
			int intDefaultIndex;
			if (matchDefault.Groups.Count == 2) {
				this.States.Add("default");
				intDefaultIndex = matchDefault.Groups[1].Index;
				strGlobal = this.CorrectGlobal(strC.Substring(0, intDefaultIndex));
			} else {
				intDefaultIndex = 0;
				strGlobal = "";
			}
			return "class GlobalClass : SecondLife\n{\n" + strGlobal + "}\n" + strC.Substring(intDefaultIndex);
		}

		private string Capitalize(string strC, string strName)
		{
			var regex = new Regex(@"(\W)" + strName + @"(\W)",
				RegexOptions.IgnorePatternWhitespace
				| RegexOptions.Multiline
				| RegexOptions.Compiled);
			var strCap = strName[0].ToString().ToUpper() + strName.Substring(1);
			return regex.Replace(strC, "$1" + strCap + "$2");
		}

		private string RemoveSingleQuotes(string strC)
		{
			if (Properties.Settings.Default.SingleQuote) {
				return strC.Replace("'", "");
			} else {
				return strC;
			}
		}

		/// <summary>
		/// This Class translates LSL script into CSharp code
		/// </summary>
		/// <param name="strLSLCode">LSL scripting code</param>
		/// <returns>CSHarp code</returns>
		public string Parse(string strLSLCode)
		{
			const string strGlobalClass = "GlobalClass";

			var strC = strLSLCode;

			strC = this.RemoveComments(strC);

			strC = this.RemoveQuotedStrings(strC, out var quotedStrings);

			strC = this.RemoveSingleQuotes(strC);

			strC = this.PreCorrectReservedWords(strC); // Experimental

			strC = this.MakeGlobalAndLocal(strC);

			strC = this.CorrectJump(strC);
			strC = this.CorrectEvents(strC);

			strC = this.Capitalize(strC, "float");
			strC = this.Capitalize(strC, "string"); //  llList2string is also translated

			strC = this.CorrectStates(strC, strGlobalClass);

			strC = this.CorrectReservedWords(strC); // Experimental

			strC = this.CorrectRotation(strC);
			strC = this.CorrectQuaternion(strC);
			strC = this.CorrectVector(strC);
			strC = this.CorrectLists(strC);

			strC = this.InsertQuotedStrings(strC, quotedStrings);

			return strC;
		}
	}
}
