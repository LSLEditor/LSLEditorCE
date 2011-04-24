// /**
// ********
// *
// * ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon 'Dimentox Travanti' Husbands & Malcolm J. Kudra, who in turn License under the GPLv2.
// * In agreement to Alphons van der Heijden wishes.
// *
// * The community would like to thank Alphons for all of his hard work, blood sweat and tears.
// * Without his work the community would be stuck with crappy editors.
// *
// * The source code in this file ("Source Code") is provided by The LSLEditor Group
// * to you under the terms of the GNU General Public License, version 2.0
// * ("GPL"), unless you have obtained a separate licensing agreement
// * ("Other License"), formally executed by you and The LSLEditor Group.  Terms of
// * the GPL can be found in the gplv2.txt document.
// *
// ********
// * GPLv2 Header
// ********
// * LSLEditor, a External editor for the LSL Language.
// * Copyright (C) 2010 The LSLEditor Group.
// 
// * This program is free software; you can redistribute it and/or
// * modify it under the terms of the GNU General Public License
// * as published by the Free Software Foundation; either version 2
// * of the License, or (at your option) any later version.
// *
// * This program is distributed in the hope that it will be useful,
// * but WITHOUT ANY WARRANTY; without even the implied warranty of
// * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// * GNU General Public License for more details.
// *
// * You should have received a copy of the GNU General Public License
// * along with this program; if not, write to the Free Software
// * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
// ********
// *
// * The above copyright notice and this permission notice shall be included in all 
// * copies or substantial portions of the Software.
// *
// ********
// */
using System;
using System.Text.RegularExpressions;

namespace LSLEditor
{
	public partial class SecondLife
	{
		public struct Float
		{
			private object m_value;
			private Double value
			{
				get
				{
					if (m_value == null)
						m_value = (Double)0;
					return (Double)m_value;
				}
				set
				{
					m_value = value;
				}
			}
			public Float(Double a)
			{
				this.m_value = a;
			}

			#region public Float(string s)
			public Float(string s)
			{
				Regex r = new Regex(@"\A[ ]*(?<sign>[+-]?) 
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
                                )", RegexOptions.IgnoreCase | RegexOptions.IgnorePatternWhitespace);
				Match m = r.Match(s);
				Double mantissa = 0.0;
				if (r.Match(s).Groups["dec"].Value.Length > 0)
					mantissa = Convert.ToDouble(r.Match(s).Groups["dec"].Value);
				if (m.Groups["int"].Success)
				{
					if (m.Groups["int"].Value.Length > 0)
					{
						//                    System.Console.WriteLine("i:\t" + m.Groups["int"].Value); 
						mantissa = Convert.ToInt64(m.Groups["int"].Value, 16);
					}
					if (m.Groups["frac"].Value.Length > 0)
					{
						//                    System.Console.WriteLine("f:\t"+m.Groups["frac"].Value); 
						mantissa += Convert.ToInt64(m.Groups["frac"].Value, 16) / Math.Pow(16.0, m.Groups["frac"].Value.Length);
					}
					if (m.Groups["exp"].Value.Length > 0)
					{
						//                    System.Console.WriteLine("e:\t" + m.Groups["exp"].Value); 
						mantissa *= Math.Pow(2.0, Convert.ToInt64(m.Groups["exp"].Value, 10));
					}
				}
				if (m.Groups["sign"].Value == "-")
					this.m_value = -mantissa;
				else
					this.m_value = mantissa;
			}
#endregion

			public override int GetHashCode()
			{
				return (int)this.value % System.Int32.MaxValue;
			}

			public override string ToString()
			{
				return string.Format("{0:0.000000}",this.value);
			}

			public static explicit operator String(Float x)
			{
				return x.ToString();
			}

			public static implicit operator Double(Float x)
			{
				return x.value;
			}

			//public static implicit operator integer(Float x)
			//{
			//	return (int)x.value;
			//}

			public static explicit operator Float(string s)
			{
				return new Float(s);
			}

			public static explicit operator Float(String s)
			{
				return new Float(s.ToString());
			}

			public static implicit operator Float(Int32 x)
			{
				return new Float((Double)x);
			}

			// 6 jan 2008 , does this work????
			public static implicit operator Float(long x)
			{
				return new Float((int)(uint)x);
			}

			public static implicit operator Float(Double x)
			{
				return new Float(x);
			}

			public static implicit operator Float(integer x)
			{
				return new Float((Double)x);
			}

			public static Float operator *(Float a, Float b)
			{
				return new Float(a.value * b.value);
			}

			public static Float operator /(Float a, Float b)
			{
				return new Float(a.value / b.value);
			}

			public static Float operator ++(Float a)
			{
				return new Float(a.value + 1.0);
			}

			public static Float operator --(Float a)
			{
				return new Float(a.value - 1.0);
			}

			public static Float operator +(Float a, Float b)
			{
				return new Float(a.value + b.value);
			}

			public static Float operator -(Float a, Float b)
			{
				return new Float(a.value - b.value);
			}

			public static explicit operator bool(Float a)
			{
				return (Math.Abs(a.value) >= Double.Epsilon);
			}

			public static bool operator true(Float a)
			{
				return (Math.Abs(a.value) >= Double.Epsilon);
			}

			public static bool operator false(Float a)
			{
				return (Math.Abs(a.value) < Double.Epsilon);
			}

			public static bool operator ==(Float x, Float y)
			{
				return (Math.Abs(x.value - y.value) < Double.Epsilon);
			}

			public static bool operator !=(Float x, Float y)
			{
				return !(x == y);
			}

			public static int Compare(Float a, Float b)
			{
				if (a.value < b.value)
					return -1;
				if (a.value > b.value)
					return 1;
				return 0;
			}


			public override bool Equals(object o)
			{
				try
				{
					return (bool)(this == (Float)o);
				}
				catch
				{
					return false;
				}
			}

		}
	}
}
