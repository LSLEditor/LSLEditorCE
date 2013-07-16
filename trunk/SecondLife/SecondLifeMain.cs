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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

/*
// (C) 2006,2007 Alphons van der Heijden
// mail: alphons@heijden.com
 */

[module: SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed.")]

namespace LSLEditor
{
	/// <summary>
	/// Enumeration of Communication Types that the host understands.
	/// </summary>
	public enum CommunicationType
	{
		/// <summary>
		/// Indicates the message can be heard only within 5m.
		/// </summary>
		Whisper, 

		/// <summary>
		/// Indicates the message can be heard only within 20m.
		/// </summary>
		Say, 

		/// <summary>
		/// Indicates the message can be heard only within 100m.
		/// </summary>
		Shout, 

		/// <summary>
		/// Indicates the message can be only heard by the owner any where within the same simulator.
		/// </summary>
		OwnerSay, 

		/// <summary>
		/// Indicates the message can be heard any where within the same simulator.
		/// </summary>
		RegionSay, 

		/// <summary>
		/// Indicates the message can be heard any where within the same simulator, by a specific object.
		/// </summary>
		RegionSayTo
	}

	/// <summary>
	/// Partial definition of SecondLife class to handle running scripts.
	/// </summary>
	public partial class SecondLife
	{
		/// <summary>
		/// Holds the host object.
		/// </summary>
		private SecondLifeHost slhHost;

		#region members
		/// <summary>
		/// Random generator.
		/// </summary>
		private Random rdmRandom;

		/// <summary>
		/// Holds the time of the script starting execution.
		/// </summary>
		private DateTime dtDateTimeScriptStarted;

		/// <summary>
		/// Contains a boolean value indicating whether this object accepts other items to be dropped into it.
		/// </summary>
		private bool blnAllowDrop = false;

		/// <summary>
		/// Contains a list of keys of avatars that may enter a parcel.
		/// </summary>
		private Hashtable htLandPassList;

		/// <summary>
		/// Contains a list of keys of avatars that may NOT enter a parcel.
		/// </summary>
		private Hashtable htLandBanList;

		/// <summary>
		/// Volume of sound played by this prim.
		/// </summary>
		private Float fVolume;

		/// <summary>
		/// Name of the object/prim.
		/// </summary>
		private String sObjectName;

		/// <summary>
		/// URL for parcel's music stream.
		/// </summary>
		private String sParcelMusicURL;

		/// <summary>
		/// Position of object/prim  placement in a simulator.
		/// </summary>
		private vector vPosition;

		/// <summary>
		/// Rotation of the object/prim's placement in the simulator.
		/// </summary>
		private rotation rRotation;

		/// <summary>
		/// Local rotation of the prim with respect to the object root.
		/// </summary>
		private rotation rRotationlocal;

		/// <summary>
		/// Scale of the object/prim.
		/// </summary>
		private vector vScale;

		/// <summary>
		/// Text for the "Sit" entry on object menu.
		/// </summary>
		private String sSitText;

		/// <summary>
		/// Radius that sound may be heard around the object/prim.
		/// </summary>
		private Float fSoundRadius;

		/// <summary>
		/// Region Coordinates of the simulator's bottom-left corner.
		/// </summary>
		private vector vRegionCorner;

		/// <summary>
		/// Parameter passed to the script on start up.
		/// </summary>
		private integer iStartParameter;

		#endregion

		#region Constructor
		/// <summary>
		/// Initialises a new instance of the <see cref="SecondLife"/> class.
		/// </summary>
		public SecondLife()
		{
			this.host = null;
			rdmRandom = new Random();
			dtDateTimeScriptStarted = DateTime.Now.ToUniversalTime();
			htLandPassList = new Hashtable();
			htLandBanList = new Hashtable();
			fVolume = 0.0;
			sObjectName = null;
			vPosition = new vector(127, 128, 20);
			rRotation = rotation.ZERO_ROTATION;
			rRotationlocal = rotation.ZERO_ROTATION;
			vScale = vector.ZERO_VECTOR;
			sSitText = "sittext";
			fSoundRadius = 1.0;
			iStartParameter = 0;

			vRegionCorner = vector.ZERO_VECTOR;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Gets the position of the object within the simulator.
		/// </summary>
		/// <value>Gets the vPosition data member.</value>
		public vector GetLocalPos
		{
			// TODO change this to use a call to llGetLocalPos specifying NOT to be Verbose. After all functions have been changed to allow verbose/silent option.
			get { return vPosition; }
		}
		
		/// <summary>
		/// Gets or sets the SecondLifeHost object.
		/// </summary>
		/// <value>The host property gets/sets the slhHost data member.</value>
		public SecondLifeHost host
		{
			get { return this.slhHost; }
			set { this.slhHost = value; }
		}
		#endregion
		
		#region internal routines
		/// <summary>
		/// Outputs messages during execution of the script.
		/// </summary>
		/// <param name="strLine">The text (with possible placeholders) to be output.</param>
		/// <param name="parameters">Values to be put into corresponding placeholders.</param>
		private void Verbose(string strLine, params object[] parameters)
		{
			if (parameters.Length == 0) {
				host.VerboseMessage(strLine);
			} else {
				this.host.VerboseMessage(string.Format(strLine, parameters));
			}
		}

		/// <summary>
		/// Wrapper to call the SecondLifeHost.Chat method.
		/// </summary>
		/// <param name="iChannel">Channel for message to be sent on.</param>
		/// <param name="sText">Text of message to send.</param>
		/// <param name="ctHow">Type of communication (from CommunicationType enumerator).</param>
		private void Chat(integer iChannel, string sText, CommunicationType ctHow)
		{
			this.host.Chat(this.host, iChannel, this.host.GetObjectName(), this.host.GetKey(), sText, ctHow);
		}

		/// <summary>
		/// Method to trigger change in script state.
		/// </summary>
		/// <param name="strStateName">Name of state to switch to.</param>
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
		/// <summary>
		/// Takes the possibly negative value list indexes and corrects them to be absolute indexes.
		/// </summary>
		/// <param name="intLength">Length of list.</param>
		/// <param name="intStart">Start index.</param>
		/// <param name="intEnd">End index.</param>
		/// <returns>True, unless an error occurred, then false.</returns>
		private bool CorrectIt(int intLength, ref int intStart, ref int intEnd)
		{
			bool bResult = true;
			if (intStart < 0) {
				intStart = intLength + intStart;
			}

			if (intEnd < 0) {
				intEnd = intLength + intEnd;
			}

			if (intStart <= intEnd) {
				if (intStart >= intLength) {
					bResult = false;
				}
				if (intEnd < 0) {
					bResult = false;
				}
			}

			intStart = Math.Max(0, intStart);
			intEnd = Math.Min(intLength - 1, intEnd);

			return bResult;
		}

		/// <summary>
		/// Randomly rearranges the list of items.
		/// </summary>
		/// <param name="alCollection">List to shuffle.</param>
		/// <returns>The reordered list.</returns>
		public ArrayList RandomShuffle(ArrayList alCollection)
		{
			/* We have to copy all items anyway, and there isn't a way to produce the items
			   on the fly that is linear. So copying to an array and shuffling it is as efficient as we can get.
			 */

			if (alCollection == null) {
				throw new ArgumentNullException("collection");
			}

			int intCount = alCollection.Count;
			for (int i = intCount - 1; i >= 1; --i) {
				// Pick an random number 0 through i inclusive.
				int j = rdmRandom.Next(i + 1);

				// Swap array[i] and array[j]
				object temp = alCollection[i];
				alCollection[i] = alCollection[j];
				alCollection[j] = temp;
			}
			return alCollection;
		}

		/// <summary>
		/// Convert a list into an array of buckets containing Stride elements.
		/// </summary>
		/// <param name="lSource">List of items.</param>
		/// <param name="intStride">Number of element for each bucket.</param>
		/// <returns>The list separated into elements of Stride length.</returns>
		private ArrayList List2Buckets(list lSource, int intStride)
		{
			ArrayList alBuckets = null;
			if ((lSource.Count % intStride) == 0 && intStride <= lSource.Count) {
				alBuckets = new ArrayList();
				for (int intI = 0; intI < lSource.Count; intI += intStride) {
					object[] bucket = new object[intStride];
					for (int intJ = 0; intJ < intStride; intJ++) {
						bucket[intJ] = lSource[intI + intJ];
					}
					alBuckets.Add(bucket);
				}
			}
			return alBuckets;
		}

		/// <summary>
		/// Converts buckets back into a list.
		/// </summary>
		/// <param name="alBuckets">Array of buckets.</param>
		/// <param name="intStride">Stride value.</param>
		/// <returns>The items recombined into a simple list.</returns>
		private list Buckets2List(ArrayList alBuckets, int intStride)
		{
			object[] items = new object[alBuckets.Count * intStride];
			for (int intI = 0; intI < alBuckets.Count; intI++) {
				for (int intJ = 0; intJ < intStride; intJ++) {
					items[(intI * intStride) + intJ] = ((object[])alBuckets[intI])[intJ];
				}
			}
			return new list(items);
		}

		/// <summary>
		/// Implements the Comparer Interface for our custom types (Float, Integer, and String).
		/// </summary>
		private class BucketComparer : IComparer
		{
			/// <summary>
			/// True/false parameter indicating whether the comparison should be done in ascending or descending order.
			/// </summary>
			private integer iAscending;

			/// <summary>
			/// Initialises a new instance of the <see cref="BucketComparer"/> class.
			/// </summary>
			/// <param name="ascending"></param>
			[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1642:ConstructorSummaryDocumentationMustBeginWithStandardText", Justification = "Reviewed.")]
			public BucketComparer(integer ascending)
			{
				this.iAscending = ascending;
			}

			/// <summary>
			/// Perform the comparison between two objects.
			/// </summary>
			/// <param name="x">First object.</param>
			/// <param name="y">Second object.</param>
			/// <returns>An integer (-1, 0, or 1), indicating whether the first item is less than, same as, or greater than the second.</returns>
			public int Compare(object x, object y)
			{
				int iResult = 0;
				object objA, objB;

				object[] xx = x as object[];
				object[] yy = y as object[];

				if (iAscending == TRUE) {
					objA = xx[0];
					objB = yy[0];
				} else {
					objB = xx[0];
					objA = yy[0];
				}

				string strType = objA.GetType().ToString();

				if (((objA is string) && (objB is string)) || ((objA is SecondLife.String) && (objB is SecondLife.String))) {
					iResult = string.Compare(objA.ToString(), objB.ToString());
				} else if ((objA is SecondLife.integer) && (objB is SecondLife.integer)) {
					iResult = SecondLife.integer.Compare((SecondLife.integer)objA, (SecondLife.integer)objB);
				} else if ((objA is SecondLife.Float) && (objB is SecondLife.Float)) {
					iResult = SecondLife.Float.Compare((SecondLife.Float)objA, (SecondLife.Float)objB);
				}

				return iResult;
			}
		}
		#endregion

		#region String Functions

		/// <summary>
		/// Takes a string and splits it into a list based upon separators (which are not kept) and spacers (which are).
		/// </summary>
		/// <param name="sSource">The source text.</param>
		/// <param name="lSeparators">Separators to split on.</param>
		/// <param name="lSpacers">Spacers to split on.</param>
		/// <param name="blnKeepNulls">True/false indicating whether nulls are kept in the resulting list.</param>
		/// <returns>A new list of the split string.</returns>
		private list ParseString(String sSource, list lSeparators, list lSpacers, bool blnKeepNulls)
		{
			list lResult = new list();
			int intFrom = 0;
			string s = sSource;
			for (int intI = 0; intI < s.Length; intI++) {
				string strTmp = s.Substring(intI);
				bool blnFound = false;
				for (int intJ = 0; intJ < lSeparators.Count; intJ++) {
					string strSeparator = lSeparators[intJ].ToString();
					if (strSeparator.Length == 0) {
						continue; // check this
					}
					if (strTmp.IndexOf(strSeparator) == 0) {
						string strBefore = s.Substring(intFrom, intI - intFrom);
						if (strBefore != "" || blnKeepNulls) {
							lResult.Add(strBefore);
						}
						intI += strSeparator.Length - 1;
						intFrom = intI + 1;
						blnFound = true;
						break;
					}
				}

				if (blnFound) {
					continue;
				}

				for (int intJ = 0; intJ < lSpacers.Count; intJ++) {
					string strSpacer = lSpacers[intJ].ToString();
					if (strSpacer.Length == 0) {
						continue; // check this
					}
					if (strTmp.IndexOf(strSpacer) == 0) {
						string strBefore = s.Substring(intFrom, intI - intFrom);
						if (strBefore != "" || blnKeepNulls) {
							lResult.Add(strBefore);
						}
						lResult.Add(strSpacer);
						intI += strSpacer.Length - 1;
						intFrom = intI + 1;
						break;
					}
				}
			}
			string strLast = s.Substring(intFrom);
			if (strLast != "" || blnKeepNulls) {
				lResult.Add(strLast);
			}
			return lResult;
		}

		/// <summary>
		/// Convert string to Base64 representation.
		/// </summary>
		/// <param name="strText">Source string.</param>
		/// <returns>Base64 encoding of the source string.</returns>
		private string StringToBase64(string strText)
		{
			byte[] data = Encoding.UTF8.GetBytes(strText);
			return Convert.ToBase64String(data);
		}

		/// <summary>
		/// Converts Base64 encoded string back to UTF-8.
		/// </summary>
		/// <param name="strText">Base64 encoded string.</param>
		/// <returns>UTF-8 string.</returns>
		private string Base64ToString(string strText)
		{
			byte[] data = Convert.FromBase64String(strText);
			int intLen = Array.IndexOf(data, (byte)0x00);
			if (intLen < 0) {
				intLen = data.Length;
			}
			return Encoding.UTF8.GetString(data, 0, intLen);
		}

		/// <summary>
		/// Performs the lookup.
		/// </summary>
		/// <param name="s">String source.</param>
		/// <param name="intIndex">Byte within string.</param>
		/// <returns>The Base64 value of the element.</returns>
		private int LookupBase64(string s, int intIndex)
		{
			int intReturn = 0;
			if (intIndex < s.Length) {
				intReturn = FastLookupBase64[s[intIndex]];
				if (intReturn == 0) {
					if (s[intIndex] != 'A') {
						throw new Exception();
					}
				}
			} else {
				intReturn = 0;
			}
			return intReturn;
		}

		/// <summary>
		/// Pre-generated matrix for quicker lookup.
		/// </summary>
////[SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204:StaticElementsMustAppearBeforeInstanceElements", Justification = "Stays with other string functions.")]
		private static readonly int[] FastLookupBase64 =
			{ // 0   1   2   3   4   5   6   7   8   9   A   B   C   D   E   F
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // 00
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // 10
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 62,  0,  0,  0, 63,	  // 20
				52, 53, 54, 55, 56, 57, 58, 59, 60, 61,  0,  0,  0,  0,  0,  0,	  // 30
				 0,  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14,	  // 40
				15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25,  0,  0,  0,  0,  0,	  // 50
				 0, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40,	  // 60
				41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51,  0,  0,  0,  0,  0,	  // 70
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // 80
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // 90
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // A0
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // B0
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // C0
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // D0
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	  // E0
				 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0 }; // F0
		#endregion

		#region Math Functions

		/// <summary>
		/// Performs (iA ** iB) % iC.
		/// </summary>
		/// <param name="iA">Base value.</param>
		/// <param name="iB">Exponent value .</param>
		/// <param name="iC">Modulus value.</param>
		/// <returns>The calculated result.</returns>
		private integer ModPow1(integer iA, integer iB, integer iC)
		{
			return (int)Math.Pow((int)iA, (int)iB & (int)0xffff) % (int)iC;
		}

		/// <summary>
		/// Performs the llModPow2() function.
		/// </summary>
		/// <param name="iValueX">Value one.</param>
		/// <param name="iValueY">Value two.</param>
		/// <param name="iModulus">The modulus value.</param>
		/// <returns>The calculated result.</returns>
		private integer ModPow2(integer iValueX, integer iValueY, integer iModulus)
		{
			integer iResult = 0;
			if (iValueX != 0) {
				integer k = 1 + (int)Math.Ceiling(Math.Log(Math.Abs(iValueX)) / 0.69314718055994530941723212145818);	// ceil(log2(x))
				integer w = 32;
				integer p = w / k;
				integer r = iValueY / p;
				integer f = iValueY % p;
				integer z = 1;
				if (r) {
					z = ModPow2(ModPow1(iValueX, p, iModulus), r, iModulus);
				}
				if (f) {
					z = (z * ModPow1(iValueX, f, iModulus)) % iModulus;
				}
				iResult = z;
			}
			return iResult;
		}
		#endregion Math Functions

		/// <summary>
		/// Used by llListStatistics in generating the statistics.
		/// </summary>
		/// <param name="lInput">List of numbers (mixed types possible) to use.</param>
		/// <returns>The numbers added together as a Float type.</returns>
		private List<double> GetListOfNumbers(list lInput)
		{
			List<double> lResult = new List<double>();
			for (int intI = 0; intI < lInput.Count; intI++) {
				object objI = lInput[intI];
				string strType = objI.GetType().ToString().Replace("LSLEditor.SecondLife+", "");
				switch (strType) {
					case "Float":
						lResult.Add(Convert.ToDouble((Float)objI));
						break;
					case "System.Int32":
						lResult.Add(Convert.ToDouble((int)objI));
						break;
					case "System.Double":
						lResult.Add(Convert.ToDouble((double)objI));
						break;
					case "integer":
						lResult.Add(Convert.ToDouble((integer)objI));
						break;
					default:
						break;
				}
			}
			return lResult;
		}

		/// <summary>
		/// Calculates the mean of the supplied numbers.
		/// </summary>
		/// <param name="data">Array of numbers.</param>
		/// <returns>The mean of the supplied numbers as a double type.</returns>
		private double GetAverage(double[] data)
		{
			try {
				double dblDataTotal = 0;
				for (int i = 0; i < data.Length; i++) {
					dblDataTotal += data[i];
				}
				return SafeDivide(dblDataTotal, data.Length);
			} catch (Exception) {
				throw;
			}
		}

		/// <summary>
		/// Calculates standard deviation.
		/// </summary>
		/// <param name="dblNumbers">Array of numbers to work with.</param>
		/// <returns>The standard deviation of the supplied numbers as a double type.</returns>
		public double GetStandardDeviation(double[] dblNumbers)
		{
			double dblSum = 0.0, dblSumOfSqrs = 0.0;
			for (int i = 0; i < dblNumbers.Length; i++) {
				dblSum += dblNumbers[i];
				dblSumOfSqrs += Math.Pow(dblNumbers[i], 2);
			}
			double dblTopSum = (dblNumbers.Length * dblSumOfSqrs) - Math.Pow(dblSum, 2);
			double dblN = (double)dblNumbers.Length;
			return Math.Sqrt(dblTopSum / (dblN * (dblN - 1)));
		}

		/// <summary>
		/// Performs division, only if both values are non-zero.
		/// </summary>
		/// <param name="dblValue1">First number.</param>
		/// <param name="dblValue2">Second number.</param>
		/// <returns>The result of the division, or zero if not performed.</returns>
		private double SafeDivide(double dblValue1, double dblValue2)
		{
			double dblResult = 0.0F;
			try {
				if ((dblValue1 != 0) && (dblValue2 != 0)) {
					dblResult = dblValue1 / dblValue2;
				}
			} catch {
			}
			return dblResult;
		}

		/// <summary>
		/// Takes a hexadecimal representation of a number and coverts it to an integer.
		/// </summary>
		/// <param name="b">Byte to convert.</param>
		/// <returns>Integer value of the supplied byte.</returns>
		private byte HexToInt(byte b)
		{
			byte bResult;
			if (b >= '0' && b <= '9') {
				bResult = (byte)(b - '0');
			} else if ((b >= 'a' && b <= 'f') || (b >= 'A' && b <= 'F')) {
				bResult = (byte)((b & 0x5f) - 0x37);
			} else {
				bResult = 0; // error
			}
			return bResult;
		}
		#endregion
	}
}
