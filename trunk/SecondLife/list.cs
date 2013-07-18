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
using System.Text;
using System.Collections;

namespace LSLEditor
{
	public partial class SecondLife
	{
		public struct list
		{
			private System.Collections.ArrayList value;

			public list(object[] args)
			{
				this.value = new ArrayList();
				foreach (object objA in args)
					this.Add(objA);
			}

			public list(list a)
			{
				this.value = new ArrayList();
				this.value.AddRange(a.ToArray());
			}

			public int Count
			{
				get
				{
					if (this.value == null) this.value = new ArrayList();
					return this.value.Count;
				}
			}

			public void AddRange(list c)
			{
				if (this.value == null) this.value = new ArrayList();
				this.value.AddRange(c.ToArray());
			}

			public void Add(object value)
			{
				if (this.value == null) this.value = new ArrayList();
				string strType = value.GetType().ToString();
				if (value is string) {
					this.value.Add((String)value.ToString());
				} else if (value is int) {
					this.value.Add(new integer((int)value));
				} else if (value is uint) {
					this.value.Add(new integer((int)(uint)value));
				} else if (value is double) {
					this.value.Add(new Float((double)value));
				} else {
					this.value.Add(value);
				}
			}

			public object this[int index]
			{
				get
				{
					if (this.value == null) this.value = new ArrayList();
					return this.value[index];
				}
				set
				{
					if (this.value == null) this.value = new ArrayList();
					this.value[index] = value;
				}
			}

			public void Insert(int index, object value)
			{
				if (this.value == null) this.value = new ArrayList();

				if (this.value == null) this.value = new ArrayList();
				string strType = value.GetType().ToString();
				if (value is string) {
					this.value.Insert(index, (String)value.ToString());
				} else if (value is int) {
					this.value.Insert(index, new integer((int)value));
				} else if (value is uint) {
					this.value.Insert(index, new integer((int)(uint)value));
				} else if (value is double) {
					this.value.Insert(index, new Float((double)value));
				} else {
					this.value.Insert(index, value);
				}
			}

			public object[] ToArray()
			{
				if (this.value == null) this.value = new ArrayList();
				return this.value.ToArray();
			}

			public static list operator +(list a, list b)
			{
				list l = new list();
				if ((object)a != null) l.AddRange(a);
				if ((object)b != null) l.AddRange(b);
				return l;
			}

			public static list operator +(object b, list a)
			{
				list l = new list();
				if ((object)a != null) l.AddRange(a);
				l.Insert(0, b);
				return l;
			}

			public static list operator +(list a, object b)
			{
				list l = new list();
				if ((object)a != null) l.AddRange(a);
				l.Add(b);
				return l;
			}

			public static explicit operator list(string a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}
			public static explicit operator list(String a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(integer a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(key a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(vector a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(rotation a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(uint a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(int a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static explicit operator list(double a)
			{
				list l = new list();
				l.Add(a);
				return l;
			}

			public static integer operator ==(list l, list m)
			{
				int iResult = TRUE;
				if (l.Count != m.Count) {
					iResult = FALSE;
				} else {
					for (int intI = 0; intI < l.Count; intI++) {
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
				int intDifferent = 0;
				if (m.Count == 0) {// shortcut
					intDifferent = l.Count;
				} else {
					for (int intI = 0; intI < l.Count; intI++) {
						bool blnFound = false;
						for (int intJ = 0; intJ < m.Count; intJ++) {
							if (l[intI].Equals(m[intJ])) {
								blnFound = true;
								break;
							}
						}
						if (!blnFound) intDifferent++;
					}
				}
				return intDifferent;
			}

			public static bool operator true(list x)
			{
				return (object)x == null ? false : (x.value.Count != 0);
			}

			// Definitely false operator. Returns true if the operand is 
			// ==0, false otherwise:
			public static bool operator false(list x)
			{
				return (object)x == null ? true : (x.value.Count == 0);
			}


			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				int intResult = (this == (list)obj);
				return (intResult == 1);
			}

			public string ToVerboseString()
			{
				if (this.value == null) {
					this.value = new ArrayList();
				}

				StringBuilder sb = new StringBuilder();
				sb.Append('[');
				for (int intI = 0; intI < this.value.Count; intI++) {
					if (intI > 0) sb.Append(',');
					if ((this.value[intI] is string) && Properties.Settings.Default.QuotesListVerbose) {
						sb.Append("\"" + this.value[intI].ToString() + "\"");
					} else {
						sb.Append(this.value[intI].ToString());
					}
				}
				sb.Append(']');
				return sb.ToString();
			}

			public override string ToString()
			{
				if (this.value == null) this.value = new ArrayList();
				StringBuilder sb = new StringBuilder();
				for (int intI = 0; intI < this.value.Count; intI++) {
					if (this.value[intI] is vector) {
						vector v = (vector)this.value[intI];
						sb.AppendFormat(new System.Globalization.CultureInfo("en-us"), "<{0:0.000000}, {1:0.000000}, {2:0.000000}>", (double)v.x, (double)v.y, (double)v.z);
					} else if (this.value[intI] is rotation) {
						rotation r = (rotation)this.value[intI];
						sb.AppendFormat(new System.Globalization.CultureInfo("en-us"), "<{0:0.000000}, {1:0.000000}, {2:0.000000}, {3:0.000000}>", (double)r.x, (double)r.y, (double)r.z, (double)r.s);
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
