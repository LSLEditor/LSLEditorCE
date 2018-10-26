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

namespace LSLEditor
{
	public partial class SecondLife
	{
		public struct String
		{
			private string value;

			public String(string s)
			{
				this.value = s;
			}

			public static implicit operator String(string s)
			{
				return new String(s);
			}

			// 3 oct 2007
			public static explicit operator String(bool a)
			{
				return a ? "1" : "0";
			}

			// 24 augustus 2007 
			public static explicit operator String(int i)
			{
				return new String(i.ToString());
			}

			public static explicit operator String(long i)
			{
				return new String(((int)(uint)i).ToString());
			}

			public static explicit operator String(float i)
			{
				return new String(string.Format("{0:0.000000}", i));
			}

			public static implicit operator string(String s)
			{
				return s.ToString();
			}

			public static implicit operator bool(String s)
			{
				return (s.value == null) ? false : (s.value.Length != 0);
			}

			public static bool operator ==(String x, String y)
			{
				return (x.ToString() == y.ToString());
			}

			public static bool operator !=(String x, String y)
			{
				return !(x == y);
			}

			// Public overrides

			public override bool Equals(object o)
			{
				try {
					return (bool)(this.value == o.ToString());
				} catch {
					return false;
				}
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override string ToString()
			{
				if (this.value == null) {
					this.value = "";
				}
				return this.value;
			}

		}
	}
}
