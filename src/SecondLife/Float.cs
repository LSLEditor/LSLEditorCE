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
// Float.cs
//
// </summary>

using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace LSLEditor
{
	/// <summary>
	/// This part of the SecondLife class defines the Float struct.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed.")]
	public partial class SecondLife
	{
		/// <summary>
		/// A struct for Float objects.
		/// </summary>
		public struct Float
		{
			/// <summary>
			/// Stores the Float value.
			/// </summary>
			private object objValue;

			/// <summary>
			/// Gets or sets the value of the Float.
			/// </summary>
			/// <value>Gets or sets the objValue data member.</value>
			private Double value
			{
				get
				{
					if (objValue == null) {
						objValue = (Double)0;
					}
					return (Double)objValue;
				}

				set
				{
					objValue = value;
				}
			}

			/// <summary>
			/// Initialises a new instance of the <see cref="Float"/> struct.
			/// </summary>
			/// <param name="a"></param>
			public Float(Double a)
			{
				this.objValue = a;
			}

			#region public Float(string s)
			/// <summary>
			/// Initialises a new instance of the <see cref="Float"/> struct.
			/// </summary>
			/// <param name="s"></param>
			public Float(string s)
			{
				Regex r = new Regex(
					@"\A[ ]*(?<sign>[+-]?)
                        (?:
                            (?:
                                0x
                                (?:
                                    (?:
                                        (?<int>[0-9a-f]*)
                                        (?:
                                            (?:\.
                                                (?<frac>[0-9a-f]*)
                                            )
                                            |
                                        )
                                    )
                                    (?:
                                        (?:p
                                            (?<exp>[-+]?[\d]*)
                                        )
                                        |
                                    )
                                )
                            )
                            |
                            (?<dec>
                                [\d]*[.]?[\d]*
                                (?:
                                    (?:
                                        e[+-]?[\d]*
                                    )
                                    |
                                )
                            )
                        )",
						RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
				Match m = r.Match(s);
				Double mantissa = 0.0;
				if (r.Match(s).Groups["dec"].Value.Length > 0) {
					mantissa = Convert.ToDouble(r.Match(s).Groups["dec"].Value);
				}
				if (m.Groups["int"].Success) {
					if (m.Groups["int"].Value.Length > 0) {
						////System.Console.WriteLine("i:\t" + m.Groups["int"].Value);
						mantissa = Convert.ToInt64(m.Groups["int"].Value, 16);
					}
					if (m.Groups["frac"].Value.Length > 0) {
						////System.Console.WriteLine("f:\t"+m.Groups["frac"].Value);
						mantissa += Convert.ToInt64(m.Groups["frac"].Value, 16) / Math.Pow(16.0, m.Groups["frac"].Value.Length);
					}
					if (m.Groups["exp"].Value.Length > 0) {
						////System.Console.WriteLine("e:\t" + m.Groups["exp"].Value);
						mantissa *= Math.Pow(2.0, Convert.ToInt64(m.Groups["exp"].Value, 10));
					}
				}
				if (m.Groups["sign"].Value == "-") {
					this.objValue = -mantissa;
				} else {
					this.objValue = mantissa;
				}
			}
			#endregion

			/// <summary>
			/// No idea of the intention here ;-).
			/// </summary>
			/// <returns>The Float value as a 32 bit number.</returns>
			public override int GetHashCode()
			{
				return (int)this.value % System.Int32.MaxValue;
			}

			/// <summary>
			/// Converts the Float to a string representation.
			/// </summary>
			/// <returns>String representation of the Float.</returns>
			public override string ToString()
			{
				return string.Format("{0:0.000000}", this.value);
			}

			/// <summary>
			/// Converts the Float to a string representation.
			/// </summary>
			/// <param name="x"></param>
			/// <returns>String representation of the Float.</returns>
			public static explicit operator String(Float x)
			{
				return x.ToString();
			}

			/// <summary>
			/// Converts the Float to a string representation.
			/// </summary>
			/// <param name="x"></param>
			/// <returns>String representation of the Float.</returns>
			public static implicit operator Double(Float x)
			{
				return x.value;
			}

			////public static implicit operator integer(Float x)
			////{
			////	return (int)x.value;
			////}

			/// <summary>
			/// Creates a new instance of a Float from a string.
			/// </summary>
			/// <param name="s"></param>
			/// <returns>A new Float instance.</returns>
			public static explicit operator Float(string s)
			{
				return new Float(s);
			}

			/// <summary>
			/// Creates a new instance of a Float from a String.
			/// </summary>
			/// <param name="s"></param>
			/// <returns>A new Float instance.</returns>
			public static explicit operator Float(String s)
			{
				return new Float(s.ToString());
			}

			/// <summary>
			/// Creates a new instance of a Float from an Int32 .
			/// </summary>
			/// <param name="x"></param>
			/// <returns>A new Float instance.</returns>
			public static implicit operator Float(Int32 x)
			{
				return new Float((Double)x);
			}

			/// <summary>
			/// Creates a new instance of a Float from a long.
			/// </summary>
			/// <param name="x"></param>
			/// <returns>A new Float instance.</returns>
			public static implicit operator Float(long x)
			{
				return new Float((int)(uint)x);	// 6 jan 2008 , does this work????
			}

			/// <summary>
			/// Creates a new instance of a Float from a Double.
			/// </summary>
			/// <param name="x"></param>
			/// <returns>A new Float instance.</returns>
			public static implicit operator Float(Double x)
			{
				return new Float(x);
			}

			/// <summary>
			/// Creates a new instance of a Float from an integer type.
			/// </summary>
			/// <param name="x"></param>
			/// <returns>A new Float instance.</returns>
			public static implicit operator Float(integer x)
			{
				return new Float((Double)x);
			}

			/// <summary>
			/// Calculates the result of multiplying a and b.
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <returns>The product of a and b.</returns>
			public static Float operator *(Float a, Float b)
			{
				return new Float(a.value * b.value);
			}

			/// <summary>
			/// Calculates the result of dividing a by b.
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <returns>The result of dividing a by b.</returns>
			public static Float operator /(Float a, Float b)
			{
				return new Float(a.value / b.value);
			}

			/// <summary>
			/// Calculates the result of incrementing the Float's value.
			/// </summary>
			/// <param name="a"></param>
			/// <returns>The result of incrementing the Float's value.</returns>
			public static Float operator ++(Float a)
			{
				return new Float(a.value + 1.0);
			}

			/// <summary>
			/// Calculates the result of decrementing the Float's value.
			/// </summary>
			/// <param name="a"></param>
			/// <returns>The result of decrementing the Float's value.</returns>
			public static Float operator --(Float a)
			{
				return new Float(a.value - 1.0);
			}

			/// <summary>
			/// Calculates the result of adding a to b.
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <returns>The result of adding a to b.</returns>
			public static Float operator +(Float a, Float b)
			{
				return new Float(a.value + b.value);
			}

			/// <summary>
			/// Calculates the result of subtracting a from b.
			/// </summary>
			/// <param name="a"></param>
			/// <param name="b"></param>
			/// <returns>The result of subtracting a from b.</returns>
			public static Float operator -(Float a, Float b)
			{
				return new Float(a.value - b.value);
			}

			/// <summary>
			/// Casts the Float to a boolean.
			/// </summary>
			/// <param name="a"></param>
			/// <returns>A boolean value.</returns>
			public static explicit operator bool(Float a)
			{
				return Math.Abs(a.value) >= Double.Epsilon;
			}

			/// <summary>
			/// Defines a boolean true value.
			/// </summary>
			/// <param name="a"></param>
			/// <returns>A boolean indicating whether the value is considered true.</returns>
			public static bool operator true(Float a)
			{
				return Math.Abs(a.value) >= Double.Epsilon;
			}

			/// <summary>
			/// Defines a boolean false value.
			/// </summary>
			/// <param name="a"></param>
			/// <returns>A boolean indicating whether the value is considered false.</returns>
			public static bool operator false(Float a)
			{
				return Math.Abs(a.value) < Double.Epsilon;
			}

			/// <summary>
			/// Defines the equality operator.
			/// </summary>
			/// <param name="x">First operand.</param>
			/// <param name="y">Second operand.</param>
			/// <returns>A boolean value indicating equality.</returns>
			public static bool operator ==(Float x, Float y)
			{
				return Math.Abs(x.value - y.value) < Double.Epsilon;
			}

			/// <summary>
			/// Defines the inequality operator.
			/// </summary>
			/// <param name="x">First operand.</param>
			/// <param name="y">Second operand.</param>
			/// <returns>A boolean value indicating inequality.</returns>
			public static bool operator !=(Float x, Float y)
			{
				return !(x == y);
			}

			/// <summary>
			/// Compares two Floats.
			/// </summary>
			/// <param name="a">First operand.</param>
			/// <param name="b">Second operand.</param>
			/// <returns>An integer (-1, 0, or 1), indicating whether the first item is less than, same as, or greater than the second.</returns>
			public static int Compare(Float a, Float b)
			{
				int intResult = 0;
				if (a.value < b.value) {
					intResult = -1;
				} else if (a.value > b.value) {
					intResult = 1;
				}
				return intResult;
			}

			/// <summary>
			/// Defines the Equals operator.
			/// </summary>
			/// <param name="o"></param>
			/// <returns>A boolean value indicating equality.</returns>
			public override bool Equals(object o)
			{
				bool blnResult;
				try {
					blnResult = (bool)(this == (Float)o);
				} catch {
					blnResult = false;
				}
				return blnResult;
			}
		}
	}
}
