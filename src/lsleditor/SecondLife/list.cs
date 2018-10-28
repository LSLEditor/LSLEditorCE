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
using System.Text;

namespace LSLEditor
{
	public partial class SecondLife
	{
		public struct list
		{
			private ArrayList _value;

			private ArrayList value {
				get {
					return this._value ?? (this._value = new ArrayList());
				}
			}

			public int Count {
				get {
					return this.value.Count;
				}
			}

			public list(object[] args)
			{
				this._value = new ArrayList();
				foreach (var arg in args) {
					this.Add(arg);
				}
			}

			public list(list l)
			{
				this._value = new ArrayList();
				this.AddRange(l);
			}

			public void AddRange(list l)
			{
				this.value.AddRange(l.ToArray());
			}

			public void Add(object value)
			{
				switch (value) {
					case string x1:
						this.value.Add(new String(x1));
						break;
					case int x2:
						this.value.Add(new integer(x2));
						break;
					case uint x3:
						this.value.Add(new integer((int)x3));
						break;
					case double x4:
						this.value.Add(new Float(x4));
						break;
					default:
						this.value.Add(value);
						break;
				}
			}

			public void Insert(int index, object value)
			{
				switch (value) {
					case string x1:
						this.value.Insert(index, new String(x1));
						break;
					case int x2:
						this.value.Insert(index, new integer(x2));
						break;
					case uint x3:
						this.value.Insert(index, new integer((int)x3));
						break;
					case double x4:
						this.value.Insert(index, new Float(x4));
						break;
					default:
						this.value.Insert(index, value);
						break;
				}
			}

			public object[] ToArray()
			{
				return this.value.ToArray();
			}

			public object this[int index] {
				get {
					return this.value[index];
				}
				set {
					this.value[index] = value;
				}
			}

			public static list operator +(list a, list b)
			{
				var l = new list();
				if ((object)a != null) {
					l.AddRange(a);
				}
				if ((object)b != null) {
					l.AddRange(b);
				}
				return l;
			}

			public static list operator +(object b, list a)
			{
				var l = new list();
				if ((object)a != null) {
					l.AddRange(a);
				}
				l.Insert(0, b);
				return l;
			}

			public static list operator +(list a, object b)
			{
				var l = new list();
				if ((object)a != null) {
					l.AddRange(a);
				}
				l.Add(b);
				return l;
			}

			public static explicit operator list(string a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(String a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(integer a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(key a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(vector a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(rotation a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(uint a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(int a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(double a)
			{
				var l = new list();
				l.Add(a);
				return l;
			}

			public static integer operator ==(list l, list m)
			{
				var iResult = TRUE;
				if (l.Count != m.Count) {
					iResult = FALSE;
				} else {
					for (var intI = 0; intI < l.Count; intI++) {
						if (!l[intI].Equals(m[intI])) {
							iResult = FALSE;
							break;
						}
					}
				}
				return iResult;
			}

			public static integer operator !=(list l, list m)
			{
				var intDifferent = 0;
				if (m.Count == 0) {// shortcut
					intDifferent = l.Count;
				} else {
					for (var intI = 0; intI < l.Count; intI++) {
						var blnFound = false;
						for (var intJ = 0; intJ < m.Count; intJ++) {
							if (l[intI].Equals(m[intJ])) {
								blnFound = true;
								break;
							}
						}
						if (!blnFound) {
							intDifferent++;
						}
					}
				}
				return intDifferent;
			}

			public static bool operator true(list x)
			{
				return (object)x != null && x.value.Count > 0;
			}

			public static bool operator false(list x)
			{
				return (object)x == null || x.value.Count == 0;
			}

			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				int intResult = this == (list)obj;
				return intResult == 1;
			}

			public string ToVerboseString()
			{
				var sb = new StringBuilder();
				sb.Append('[');
				for (var intI = 0; intI < this.value.Count; intI++) {
					if (intI > 0) {
						sb.Append(',');
					}

					if ((this.value[intI] is String) && Properties.Settings.Default.QuotesListVerbose) {
						sb.Append("\"").Append(this.value[intI].ToString()).Append("\"");
					} else {
						sb.Append(this.value[intI].ToString());
					}
				}
				sb.Append(']');
				return sb.ToString();
			}

			public override string ToString()
			{
				var sb = new StringBuilder();
				for (var intI = 0; intI < this.value.Count; intI++) {
					if (this.value[intI] is vector v) {
						sb.AppendFormat(new System.Globalization.CultureInfo("en-us"),
							"<{0:0.000000}, {1:0.000000}, {2:0.000000}>",
							(double)v.x, (double)v.y, (double)v.z);
					} else if (this.value[intI] is rotation r) {
						sb.AppendFormat(new System.Globalization.CultureInfo("en-us"),
							"<{0:0.000000}, {1:0.000000}, {2:0.000000}, {3:0.000000}>",
							(double)r.x, (double)r.y, (double)r.z, (double)r.s);
					} else {
						sb.Append(this.value[intI].ToString());
					}
				}
				return sb.ToString();
			}

			public static explicit operator String(list l)
			{
				return (object)l == null ? "" : l.ToString();
			}
		}
	}
}
