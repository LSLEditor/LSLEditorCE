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
using System.Text.RegularExpressions;

namespace LSLEditor
{
	public partial class SecondLife
	{
		public struct rotation
		{
			private object m_x, m_y, m_z, m_s;

			public Float x
			{
				get
				{
					if (m_x == null) m_x = (Float)0;
					return (Float)m_x;
				}
				set
				{
					m_x = value;
				}
			}

			public Float y
			{
				get
				{
					if (m_y == null) m_y = (Float)0;
					return (Float)m_y;
				}
				set
				{
					m_y = value;
				}
			}

			public Float z
			{
				get
				{
					if (m_z == null) m_z = (Float)0;
					return (Float)m_z;
				}
				set
				{
					m_z = value;
				}
			}

			public Float s
			{
				get
				{
					if (m_s == null) m_s = (Float)0;
					return (Float)m_s;
				}
				set
				{
					m_s = value;
				}
			}
			public static readonly rotation ZERO_ROTATION;
			public rotation(double x, double y, double z, double s)
			{
				this.m_x = (Float)x;
				this.m_y = (Float)y;
				this.m_z = (Float)z;
				this.m_s = (Float)s;
			}

			public static explicit operator rotation(String s)
			{
				return new rotation(s.ToString());
			}

			public static explicit operator rotation(string s)
			{
				return new rotation(s);
			}

			public rotation(string a)
			{
				this.m_x = (Float)0;
				this.m_y = (Float)0;
				this.m_z = (Float)0;
				this.m_s = (Float)0;
				Regex regex = new Regex(
					@"<(?<x>[^,]*),(?<y>[^,]*),(?<z>[^,]*),(?<s>[^,]*)>",
					RegexOptions.IgnorePatternWhitespace |
					RegexOptions.Compiled);
				Match m = regex.Match(a);
				if (m.Success) {
					this.m_x = new Float(m.Groups["x"].Value);
					this.m_y = new Float(m.Groups["y"].Value);
					this.m_z = new Float(m.Groups["z"].Value);
					this.m_s = new Float(m.Groups["s"].Value);
				}
			}

			static rotation()
			{
				ZERO_ROTATION = new rotation(0, 0, 0, 1);
			}

			public override string ToString()
			{
				return string.Format(new System.Globalization.CultureInfo("en-us"), "<{0:0.00000}, {1:0.00000}, {2:0.00000}, {3:0.00000}>", (double)this.x, (double)this.y, (double)this.z, (double)this.s);
			}

			public static explicit operator String(rotation rot)
			{
				if ((object)rot == null) {
					return ZERO_ROTATION.ToString();
				} else {
					return rot.ToString();
				}
			}

			// 23 feb 2008
			public static rotation operator -(rotation r)
			{
				return new rotation(-r.x, -r.y, -r.z, -r.s);
			}
			// 23 feb 2008
			public static rotation operator +(rotation r)
			{
				return new rotation(r.x, r.y, r.z, r.s);
			}


			public static rotation operator *(rotation q, rotation r)
			{
				rotation rot = new rotation(
					r.s * q.x - r.z * q.y + r.y * q.z + r.x * q.s,
					r.s * q.y + r.z * q.x + r.y * q.s - r.x * q.z,
					r.s * q.z + r.z * q.s - r.y * q.x + r.x * q.y,
					r.s * q.s - r.z * q.z - r.y * q.y - r.x * q.x);
				return rot;
			}

			public static rotation operator /(rotation q, rotation r)
			{ // 23 feb 2008
				rotation rot = new rotation(
					r.s * q.x + r.z * q.y - r.y * q.z - r.x * q.s,
					r.s * q.y - r.z * q.x - r.y * q.s + r.x * q.z,
					r.s * q.z - r.z * q.s + r.y * q.x - r.x * q.y,
					r.s * q.s + r.z * q.z + r.y * q.y + r.x * q.x);
				return rot;
			}

			public static rotation operator +(rotation q, rotation r)
			{
				return new rotation(
					q.x + r.x,
					q.y + r.y,
					q.z + r.z,
					q.s + r.s
				);
			}

			public static rotation operator -(rotation q, rotation r)
			{
				return new rotation(
					q.x - r.x,
					q.y - r.y,
					q.z - r.z,
					q.s - r.s
				);
			}

			public const double EqualityTolerence = 1e-14; //Double.Epsilon;

			public static bool operator ==(rotation r1, rotation r2)
			{
				bool bReturn = true;
				if ((object)r1 == null) r1 = ZERO_ROTATION;
				if ((object)r2 == null) r2 = ZERO_ROTATION;
				if (Math.Abs(r1.x - r2.x) > EqualityTolerence) {
					bReturn = false;
				} else if (Math.Abs(r1.y - r2.y) > EqualityTolerence) {
					bReturn = false;
				} else if (Math.Abs(r1.z - r2.z) > EqualityTolerence) {
					bReturn = false;
				} else if (Math.Abs(r1.s - r2.s) > EqualityTolerence) {
					bReturn = false;
				}
				return bReturn;
			}

			public static bool operator !=(rotation r, rotation s)
			{
				return !(r == s);
			}

			public static bool operator true(rotation r)
			{
				if ((object)r == null) {
					return false;
				}
				if (r.x == 0 && r.y == 0 && r.z == 0 && r.s == 1) {
					return false;
				}
				return true;
			}

			public static bool operator false(rotation r)
			{
				if ((object)r == null) {
					return true;
				}
				if (r.x == 0 && r.y == 0 && r.z == 0 && r.s == 1) {
					return true;
				}
				return false;
			}


			public override int GetHashCode()
			{
				return base.GetHashCode();
			}

			public override bool Equals(object obj)
			{
				try {
					return (this == (rotation)obj);
				} catch {
					return false;
				}
			}

		}
	}
}
