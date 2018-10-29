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
// vector.cs
//
// </summary>

using System;
using System.Text.RegularExpressions;

namespace LSLEditor
{
	public partial class SecondLife
	{
		public struct vector
		{
			private object m_x, m_y, m_z;

			public Float x
			{
				get {
					if (this.m_x == null) {
						this.m_x = (Float)0;
					}

					return (Float)this.m_x;
				}
				set {
					this.m_x = value;
				}
			}

			public Float y
			{
				get {
					if (this.m_y == null) {
						this.m_y = (Float)0;
					}

					return (Float)this.m_y;
				}
				set {
					this.m_y = value;
				}
			}

			public Float z
			{
				get {
					if (this.m_z == null) {
						this.m_z = (Float)0;
					}

					return (Float)this.m_z;
				}
				set {
					this.m_z = value;
				}
			}

			public static readonly vector ZERO_VECTOR;

			public vector(double x, double y, double z)
			{
				this.m_x = (Float)x;
				this.m_y = (Float)y;
				this.m_z = (Float)z;
			}

			public vector(string a)
			{
				this.m_x = (Float)0;
				this.m_y = (Float)0;
				this.m_z = (Float)0;
				var regex = new Regex(
					"<(?<x>[^,]*),(?<y>[^,]*),(?<z>[^,]*)>",
					RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled);
				var m = regex.Match(a);
				if (m.Success) {
					this.m_x = new Float(m.Groups["x"].Value);
					this.m_y = new Float(m.Groups["y"].Value);
					this.m_z = new Float(m.Groups["z"].Value);
				}
			}

			static vector()
			{
				ZERO_VECTOR = new vector(0, 0, 0);
			}

			public static explicit operator vector(string s)
			{
				return new vector(s);
			}

			public static explicit operator vector(String s)
			{
				return new vector(s.ToString());
			}

			private double SumSqrs()
			{
				return this.x * this.x + this.y * this.y + this.z * this.z;
			}

			public static vector operator +(vector vector1, vector vector2)
			{
				return new vector(vector1.x + vector2.x, vector1.y + vector2.y, vector1.z + vector2.z);
			}

			public static vector operator -(vector vector1, vector vector2)
			{
				return new vector(vector1.x - vector2.x, vector1.y - vector2.y, vector1.z - vector2.z);
			}

			public static vector operator -(vector vector1)
			{
				return new vector(-vector1.x, -vector1.y, -vector1.z);
			}

			public static vector operator +(vector vector1)
			{
				return new vector(vector1.x, vector1.y, vector1.z);
			}

			public static double operator *(vector vector1, vector vector2)
			{
				return vector1.x * vector2.x + vector1.y * vector2.y + vector1.z * vector2.z;
			}

			public static vector operator /(vector v1, double dbl1)
			{
				return new vector(v1.x / dbl1, v1.y / dbl1, v1.z / dbl1);
			}

			public static vector operator *(vector v1, double dbl1)
			{
				return new vector(v1.x * dbl1, v1.y * dbl1, v1.z * dbl1);
			}

			public static vector operator *(double dbl1, vector v1)
			{
				return v1 * dbl1;
			}

			public static vector operator %(vector vector1, vector vector2)
			{
				return new vector(
					vector1.y * vector2.z - vector1.z * vector2.y,
					vector1.z * vector2.x - vector1.x * vector2.z,
					vector1.x * vector2.y - vector1.y * vector2.x);
			}

			public static vector operator *(vector v, rotation r)
			{
				return new vector(
					(r.s * r.s + r.x * r.x - r.y * r.y - r.z * r.z) * v.x + 2 * (r.x * r.y - r.s * r.z) * v.y + 2 * (r.s * r.y + r.x * r.z) * v.z,
					2 * (r.s * r.z + r.x * r.y) * v.x + (r.s * r.s - r.x * r.x + r.y * r.y - r.z * r.z) * v.y + 2 * (r.y * r.z - r.s * r.x) * v.z,
					2 * (r.x * r.z - r.s * r.y) * v.x + 2 * (r.s * r.x + r.y * r.z) * v.y + (r.s * r.s - r.x * r.x - r.y * r.y + r.z * r.z) * v.z);
			}

			public static vector operator /(vector v, rotation r)
			{
				var inverseR = new rotation(-r.x, -r.y, -r.z, r.s);
				return v * inverseR;
			}

			public static bool operator <(vector v1, vector v2)
			{
				return v1.SumSqrs() < v2.SumSqrs();
			}

			public static bool operator <=(vector v1, vector v2)
			{
				return v1.SumSqrs() <= v2.SumSqrs();
			}

			public static bool operator >(vector v1, vector v2)
			{
				return v1.SumSqrs() > v2.SumSqrs();
			}

			public static bool operator >=(vector v1, vector v2)
			{
				return v1.SumSqrs() >= v2.SumSqrs();
			}

			public const double EqualityTolerence = 1e-14;

			public static bool operator ==(vector v1, vector v2)
			{
				if ((object)v1 == null) {
					v1 = ZERO_VECTOR;
				}

				if ((object)v2 == null) {
					v2 = ZERO_VECTOR;
				}

				if (Math.Abs(v1.x - v2.x) > EqualityTolerence) {
					return false;
				} else if (Math.Abs(v1.y - v2.y) > EqualityTolerence) {
					return false;
				} else if (Math.Abs(v1.z - v2.z) > EqualityTolerence) {
					return false;
				}
				return true;
			}

			public static bool operator !=(vector v1, vector v2)
			{
				return !(v1 == v2);
			}

			public static bool operator true(vector v)
			{
				return (object)v != null && (v.x != 0 || v.y != 0 || v.z != 0);
			}

			public static bool operator false(vector v)
			{
				return (object)v == null || (v.x == 0 && v.y == 0 && v.z == 0);
			}

			public override string ToString()
			{
				return string.Format(new System.Globalization.CultureInfo("en-us"), "<{0:0.00000}, {1:0.00000}, {2:0.00000}>", (double)this.x, (double)this.y, (double)this.z);
			}

			public static explicit operator String(vector v)
			{
				if ((object)v == null) {
					return ZERO_VECTOR.ToString();
				} else {
					return v.ToString();
				}
			}

			public override int GetHashCode()
			{
				return (int)((this.x + this.y + this.z) % int.MaxValue);
			}

			public override bool Equals(object obj)
			{
				try {
					return this == (vector)obj;
				} catch {
					return false;
				}
			}
		}
	}
}
