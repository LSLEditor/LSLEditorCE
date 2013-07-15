// /**
// ********
// *
// * ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon 'Dimentox Travanti' Husbands & Malcolm J. Kudra, who in turn License under the GPLv2.
// * In agreement with Alphons van der Heijden's wishes.
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
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;


//
// (C) 2006,2007 Alphons van der Heijden
// mail: alphons@heijden.com
//

namespace LSLEditor
{
	public enum CommunicationType
	{
		Whisper, Say, Shout, OwnerSay, RegionSay, RegionSayTo
	}

    public partial class SecondLife
    {
        // Make friends with my host
        public SecondLifeHost host;

        #region members
        // Random generator
        private Random m_random;

        private DateTime m_DateTimeScriptStarted;

        private Boolean m_AllowDrop = false;
        private Hashtable m_LandPassList;

        private Hashtable m_LandBanList;

        private Float m_Volume;

        private String m_ObjectName;
		private String m_ParcelMusicURL;
        private vector m_pos;
        private rotation m_rot;
        private rotation m_rotlocal;
        private vector m_scale;
        private String m_SitText;
        private Float m_SoundRadius;

        private vector m_RegionCorner;

        private integer m_start_parameter;

        #endregion

		#region Properties

		public vector GetLocalPos
		{
			// TODO change this to use a call to llGetLocalPos specifying NOT to be Verbose. After all functions have been changed to allow verbose/silent option.
			get { return m_pos; }
		}

		#endregion

		#region constructor
		public SecondLife()
        {
            host = null;
            m_random = new Random();
            m_DateTimeScriptStarted = DateTime.Now.ToUniversalTime();
            m_LandPassList = new Hashtable();
            m_LandBanList = new Hashtable();
            m_Volume = 0.0;
            m_ObjectName = null;
            m_pos = new vector(127, 128, 20);
            m_rot = rotation.ZERO_ROTATION;
            m_rotlocal = rotation.ZERO_ROTATION;
            m_scale = vector.ZERO_VECTOR;
            m_SitText = "sittext";
            m_SoundRadius = 1.0;
            m_start_parameter = 0;

            m_RegionCorner = vector.ZERO_VECTOR;
        }
        #endregion

        #region internal routines
        private void Verbose(string strLine, params object[] parameters)
        {
            if (parameters.Length == 0)
                host.VerboseMessage(strLine);
            else
                host.VerboseMessage(string.Format(strLine, parameters));
        }

        private void Chat(integer channel, string message, CommunicationType how)
        {
            host.Chat(host, channel, host.GetObjectName(), host.GetKey(), message, how);
        }

        public void state(string strStateName)
        {
            Verbose("state->" + strStateName);
            host.State(strStateName, false);
            System.Threading.Thread.Sleep(1000);
            System.Windows.Forms.MessageBox.Show("If you see this, something is wrong", "Oops...");
        }
        #endregion

        #region Helper Functions

        #region List Functions

        private bool CorrectIt(int length, ref int start, ref int end)
        {
            if (start < 0)
                start = length + start;

            if (end < 0)
                end = length + end;

            if (start <= end)
            {
                if (start >= length)
                    return false;
                if (end < 0)
                    return false;
            }

            start = Math.Max(0, start);
            end = Math.Min(length - 1, end);

            return true;
        }

        public ArrayList RandomShuffle(ArrayList collection)
        {
            // We have to copy all items anyway, and there isn't a way to produce the items
            // on the fly that is linear. So copying to an array and shuffling it is as efficient as we can get.

            if (collection == null)
                throw new ArgumentNullException("collection");

            int count = collection.Count;
            for (int i = count - 1; i >= 1; --i)
            {
                // Pick an random number 0 through i inclusive.
                int j = m_random.Next(i + 1);

                // Swap array[i] and array[j]
                object temp = collection[i];
                collection[i] = collection[j];
                collection[j] = temp;
            }
            return collection;
        }

        private ArrayList List2Buckets(list src, int stride)
        {
            ArrayList buckets = null;
            if ((src.Count % stride) == 0 && stride <= src.Count)
            {
                buckets = new ArrayList();
                for (int intI = 0; intI < src.Count; intI += stride)
                {
                    object[] bucket = new object[stride];
                    for (int intJ = 0; intJ < stride; intJ++)
                        bucket[intJ] = src[intI + intJ];
                    buckets.Add(bucket);
                }
            }
            return buckets;
        }

        private list Buckets2List(ArrayList buckets, int stride)
        {
            object[] items = new object[buckets.Count * stride];
            for (int intI = 0; intI < buckets.Count; intI++)
            {
                for (int intJ = 0; intJ < stride; intJ++)
                    items[intI * stride + intJ] = ((object[])buckets[intI])[intJ];
            }
            return new list(items);
        }

        private class BucketComparer : IComparer
        {
            private integer ascending;
            public BucketComparer(integer ascending)
            {
                this.ascending = ascending;
            }
            public int Compare(object x, object y)
            {
                object[] xx = x as object[];
                object[] yy = y as object[];

                object A, B;

                if (ascending == TRUE)
                {
                    A = xx[0];
                    B = yy[0];
                }
                else
                {
                    B = xx[0];
                    A = yy[0];
                }

                string strType = A.GetType().ToString();

                if (((A is string) && (B is string)) ||
                    ((A is SecondLife.String) && (B is SecondLife.String)))
                    return string.Compare(A.ToString(), B.ToString());

                if ((A is SecondLife.integer) && (B is SecondLife.integer))
                    return SecondLife.integer.Compare((SecondLife.integer)A, (SecondLife.integer)B);

                if ((A is SecondLife.Float) && (B is SecondLife.Float))
                    return SecondLife.Float.Compare((SecondLife.Float)A, (SecondLife.Float)B);

                return 0;
            }
        }
        #endregion

        #region String Functions
        private list ParseString(String src, list separators, list spacers, bool blnKeepNulls)
        {
            list result = new list();
            int intFrom = 0;
            string s = src;
            for (int intI = 0; intI < s.Length; intI++)
            {
                string strTmp = s.Substring(intI);
                bool blnFound = false;
                for (int intJ = 0; intJ < separators.Count; intJ++)
                {
                    string strSeparator = separators[intJ].ToString();
                    if (strSeparator.Length == 0)
                        continue; // check this
                    if (strTmp.IndexOf(strSeparator) == 0)
                    {
                        string strBefore = s.Substring(intFrom, intI - intFrom);
                        if (strBefore != "" || blnKeepNulls)
                            result.Add(strBefore);
                        intI += strSeparator.Length - 1;
                        intFrom = intI + 1;
                        blnFound = true;
                        break;
                    }
                }

                if (blnFound)
                    continue;

                for (int intJ = 0; intJ < spacers.Count; intJ++)
                {
                    string strSpacer = spacers[intJ].ToString();
                    if (strSpacer.Length == 0)
                        continue; // check this
                    if (strTmp.IndexOf(strSpacer) == 0)
                    {
                        string strBefore = s.Substring(intFrom, intI - intFrom);
                        if (strBefore != "" || blnKeepNulls)
                            result.Add(strBefore);
                        result.Add(strSpacer);
                        intI += strSpacer.Length - 1;
                        intFrom = intI + 1;
                        break;
                    }
                }
            }
            string strLast = s.Substring(intFrom);
            if (strLast != "" || blnKeepNulls)
                result.Add(strLast);
            return result;
        }

        private string StringToBase64(string str)
        {
            byte[] data = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(data);
        }

        private string Base64ToString(string str)
        {
            byte[] data = Convert.FromBase64String(str);
            int intLen = Array.IndexOf(data, (byte)0x00);
            if (intLen < 0)
                intLen = data.Length;
            return Encoding.UTF8.GetString(data, 0, intLen);
        }

        private int LookupBase64(string s, int intIndex)
        {
            if (intIndex < s.Length)
            {
                int intReturn = FastLookupBase64[s[intIndex]];
                if (intReturn == 0)
                    if (s[intIndex] != 'A')
                        throw new Exception();
                return intReturn;
            }
            else
                return 0;
        }

		static readonly int[] FastLookupBase64 =
			{//  0  1  2  3  4  5  6  7  8  9  A  B  C  D  E  F
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// 00
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// 10
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,62, 0, 0, 0,63,	// 20
				52,53,54,55,56,57,58,59,60,61, 0, 0, 0, 0, 0, 0,	// 30
				 0, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,	// 40
				15,16,17,18,19,20,21,22,23,24,25, 0, 0, 0, 0, 0,	// 50
				 0,26,27,28,29,30,31,32,33,34,35,36,37,38,39,40,	// 60
				41,42,43,44,45,46,47,48,49,50,51, 0, 0, 0, 0, 0,	// 70
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// 80
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// 90
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// A0
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// B0
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// C0
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// D0
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,	// E0
				 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};	// F0
        #endregion

        #region Math Functions


        private integer ModPow1(integer a, integer b, integer c)
        {
            return (int)Math.Pow((int)a, (int)b & (int)0xffff) % (int)c;
        }

        private integer ModPow2(integer x, integer y, integer m)
        {
            if (!x) return 0;
            integer k = 1 + (int)Math.Ceiling(Math.Log(Math.Abs(x)) / 0.69314718055994530941723212145818);//ceil(log2(x))
            integer w = 32;
            integer p = w / k;
            integer r = y / p;
            integer f = y % p;
            integer z = 1;
            if (r) z = ModPow2(ModPow1(x, p, m), r, m);
            if (f) z = (z * ModPow1(x, f, m)) % m;
            return z;
        }

        #endregion Math Functions

        private List<double> GetListOfNumbers(list input)
        {
            List<double> result = new List<double>();
            for (int intI = 0; intI < input.Count; intI++)
            {
                object objI = input[intI];
                string strType = objI.GetType().ToString().Replace("LSLEditor.SecondLife+", "");
                switch (strType)
                {
                    case "Float":
                        result.Add(Convert.ToDouble((Float)objI));
                        break;
                    case "System.Int32":
                        result.Add(Convert.ToDouble((int)objI));
                        break;
                    case "System.Double":
                        result.Add(Convert.ToDouble((double)objI));
                        break;
                    case "integer":
                        result.Add(Convert.ToDouble((integer)objI));
                        break;
                    default:
                        break;
                }
            }
            return result;
        }

        private double GetAverage(double[] data)
        {
            try
            {
                double DataTotal = 0;
                for (int i = 0; i < data.Length; i++)
                {
                    DataTotal += data[i];
                }
                return SafeDivide(DataTotal, data.Length);
            }
            catch (Exception) { throw; }
        }

        public double GetStandardDeviation(double[] num)
        {
            double Sum = 0.0, SumOfSqrs = 0.0;
            for (int i = 0; i < num.Length; i++)
            {
                Sum += num[i];
                SumOfSqrs += Math.Pow(num[i], 2);
            }
            double topSum = (num.Length * SumOfSqrs) - (Math.Pow(Sum, 2));
            double n = (double)num.Length;
            return Math.Sqrt(topSum / (n * (n - 1)));
        }

        private double SafeDivide(double value1, double value2)
        {
            double ret = 0;
            try
            {
                if ((value1 == 0) || (value2 == 0)) { return ret; }
                ret = value1 / value2;
            }
            catch { }
            return ret;
        }

        private byte HexToInt(byte b)
        {
            if (b >= '0' && b <= '9')
                return (byte)(b - '0');
            else if ((b >= 'a' && b <= 'f') || (b >= 'A' && b <= 'F'))
                return (byte)((b & 0x5f) - 0x37);
            else
                return 0; // error
        }
        #endregion

   }
}
