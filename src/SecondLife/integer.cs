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
        public struct integer
        {
            public static readonly integer FALSE = new integer(0);
            public static readonly integer TRUE = new integer(1);

            private object m_value;

            private int value
            {
                get
                {
                    if (this.m_value == null)
                    {
                        this.m_value = 0;
                    }

                    return (int)this.m_value;
                }
                set
                {
                    this.m_value = value;
                }
            }

            // Constructor.
            public integer(int value)
            {
                this.m_value = value;
            }

            public integer(string s)
            {
                var f = new Float(s);
                this.m_value = (int)f;
            }

            // Implicit conversion from bool to integer. Maps true to 
            // integer.TRUE and false to integer.FALSE:
            public static implicit operator integer(bool x)
            {
                return x ? TRUE : FALSE;
            }

            public static implicit operator integer(int x)
            {
                return new integer(x);
            }

            public static implicit operator integer(uint x)
            {
                return new integer((int)x);
            }

            // 3 oct 2007
            public static implicit operator integer(long x)
            {
                return new integer((int)(uint)x);
            }

            // 15 aug 2007 made explicit
            public static explicit operator integer(double x)
            {
                return new integer((int)x);
            }

            // 15 aug 2007 made explicit
            public static explicit operator integer(Float x)
            {
                return new integer((int)x);
            }

            public static explicit operator integer(String x)
            {
                return new integer(x.ToString());
            }

            // 6 jan 2008, BlandWanderer thingy
            public static explicit operator integer(string x)
            {
                var f = new Float(x);
                return new integer((int)(uint)f);
            }

            // Explicit conversion from integer to bool.
            // returns true when non zero else false
            public static explicit operator bool(integer x)
            {
                return x.value != 0;
            }

            // Logical negation (NOT) operator
            public static integer operator !(integer x)
            {
                return x.value == 0 ? TRUE : FALSE;
            }

            // Bitwise AND operator
            public static integer operator &(integer x, integer y)
            {
                return new integer(x.value & y.value);
            }

            // Bitwise OR operator
            public static integer operator |(integer x, integer y)
            {
                return new integer(x.value | y.value);
            }

            // Bitwise Not operator
            public static integer operator ~(integer a)
            {
                return new integer(~a.value);
            }

            // Bitwise Xor operator
            public static integer operator ^(integer a, integer b)
            {
                return new integer(a.value ^ b.value);
            }

            // Bitwise Shift right (b must be int)
            public static integer operator >>(integer a, int b)
            {
                return new integer(a.value >> b);
            }

            // Bitwise Shift left (b must be int)
            public static integer operator <<(integer a, int b)
            {
                return new integer(a.value << b);
            }

            public static integer operator %(integer a, integer b)
            {
                return new integer(a.value % b.value);
            }

            // Definitely true operator. Returns true if the operand is 
            // !=0, false otherwise:
            public static bool operator true(integer x)
            {
                return x.value != 0;
            }

            // Definitely false operator. Returns true if the operand is 
            // ==0, false otherwise:
            public static bool operator false(integer x)
            {
                return x.value == 0;
            }

            public static explicit operator String(integer x)
            {
                return x.value.ToString();
            }

            // this one is needed by the compiler
            public static implicit operator int(integer x)
            {
                return x.value;
            }

            public static integer operator +(integer a, integer b)
            {
                return new integer(a.value + b.value);
            }

            public static integer operator -(integer a, integer b)
            {
                return new integer(a.value - b.value);
            }

            public static integer operator -(integer a, bool b)
            {
                return a - (integer)b;
            }

            public static integer operator +(integer a, bool b)
            {
                return a + (integer)b;
            }

            // 6 jan 2008
            public static integer operator *(integer a, integer b)
            {
                return new integer(a.value * b.value);
            }

            // 23 feb 2008
            public static integer operator *(integer a, Float b)
            {
                var result = a.value * b;
                return new integer((int)result);
            }

            // 6 jan 2008
            public static bool operator ==(integer a, integer b)
            {
                return a.value == b.value;
            }

            // 6 jan 2008
            public static bool operator !=(integer a, integer b)
            {
                return a.value != b.value;
            }

            public static int Compare(integer a, integer b)
            {
                if (a.value < b.value)
                {
                    return -1;
                }
                else if (a.value > b.value)
                {
                    return 1;
                }
                return 0;
            }

            // Override the Object.Equals(object obj) method:
            public override bool Equals(object obj)
            {
                try
                {
                    return this == (integer)obj;
                }
                catch
                {
                    return false;
                }
            }

            // Override the Object.GetHashCode() method:
            public override int GetHashCode()
            {
                return this.value;
            }

            // Override the ToString method to convert integer to a string:
            public override string ToString()
            {
                return this.value.ToString();
            }
        }
    }
}
