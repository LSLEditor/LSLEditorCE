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

using System.Collections.Generic;
using System.Drawing;
using System.Text.RegularExpressions;

namespace LSLEditor.Helpers
{
	class CodeCompletion
	{
		Regex regex;

		public CodeCompletion()
		{
			this.regex = new Regex(
				@"
\b(?<type>integer|float|string|vector|rotation|state|key|list)\s
(?>
  \s* (?<name>[\w]*) \s*  
  (?>\= \s* 
    (?:
      (?>
              [^(),;]+
         |    \( (?<nr>)
         |    \) (?<-nr>)
    )*
    (?(nr)(?!))
  )
  \s*)? [,;)]
)*
"
				,
				RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
		}

		public void CodeCompletionUserVar(string strKeyWord, string strTextIn, int intStart, List<KeyWordInfo> list)
		{
			if (intStart == 0)
				return;
			string strText = strTextIn.Substring(0, intStart);
			string strLowerCaseKeyWord = strKeyWord.ToLower();
			foreach (Match m in regex.Matches(strText))
			{
				if (m.Groups.Count == 4)
				{
					string strType = m.Groups[1].ToString();
					foreach (Capture cap in m.Groups[2].Captures)
					{
						string strName = cap.ToString();
						if (strName.ToLower().StartsWith(strLowerCaseKeyWord))
						{
							KeyWordInfo ki = new KeyWordInfo(KeyWordTypeEnum.Vars, strName, Color.Red);
							if (!list.Contains(ki))
								list.Add(ki);
						}
						if (strType == "list" || strType == "vector" || strType == "rotation")
							break;
					}
				}
			}
		}
	}
}
