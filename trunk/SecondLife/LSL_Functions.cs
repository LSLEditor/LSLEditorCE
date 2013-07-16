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

namespace LSLEditor
{
	/// <summary>
	/// This part of the SecondLife class contains the LSL function definitions.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "These are all LSL functions, the documentation is in the LSL Wiki.")]
	public partial class SecondLife
	{
		public integer llAbs(integer fValue)
		{
			int intA = (int)Math.Abs((long)fValue);
			Verbose("llAbs({0})={1}", fValue, intA);
			return intA;
		}

		public Float llAcos(Float fValue)
		{
			double dblA = Math.Acos(fValue);
			Verbose("llAcos({0})={1}", fValue, dblA);
			return dblA;
		}

		public void llAddToLandBanList(key kID, Float fHours)
		{
			Verbose("llAddToLandBanList({0}, {1})", kID, fHours);
			if (htLandBanList.ContainsKey(kID)) {
				htLandBanList.Remove(kID);
			}
			htLandBanList.Add(kID, fHours);
		}

		public void llAddToLandPassList(key kID, Float fHours)
		{
			Verbose("llAddToLandPassList({0}, {1})", kID, fHours);
			if (htLandPassList.ContainsKey(kID)) {
				htLandPassList.Remove(kID);
			}
			htLandPassList.Add(kID, fHours);
		}

		public void llAdjustSoundVolume(Float fVolume)
		{
			Verbose("llAdjustSoundVolume({0}), fVolume");
			this.fVolume = fVolume;
		}

		public void llAllowInventoryDrop(integer iAllowDrop)
		{
			Verbose("llAllowInventoryDrop({0})", iAllowDrop);
			blnAllowDrop = (Boolean)iAllowDrop;
		}

		public Float llAngleBetween(rotation a, rotation b)
		{
			Float fResult = 0.0F;
			rotation r = b / a;												// calculate the rotation between the two arguments as quaternion
			double s2 = r.s * r.s;											// square of the s-element
			double v2 = (r.x * r.x) + (r.y * r.y) + (r.z * r.z);			// sum of the squares of the v-elements

			if (s2 < v2) {													// compare the s-component to the v-component
				fResult = 2.0 * Math.Acos(Math.Sqrt(s2 / (s2 + v2)));		// use arccos if the v-component is dominant
			} else if (v2 != 0) {											// make sure the v-component is non-zero
				fResult = 2.0 * Math.Asin(Math.Sqrt(v2 / (s2 + v2)));		// use arcsin if the s-component is dominant
			}
			return fResult; // one or both arguments are scaled too small to be meaningful, or the values are the same, so return zero
			// implementation taken from LSL Portal. http://wiki.secondlife.com/w/index.php?title=LlAngleBetween
		}

		public void llApplyImpulse(vector vForce, integer iLocal)
		{
			Verbose("llApplyImpulse({0}, {1}", vForce, iLocal);
		}

		public void llApplyRotationalImpulse(vector vForce, integer iLocal)
		{
			Verbose("llApplyRotationalImpulse({0}, {1})", vForce, iLocal);
		}

		public Float llAsin(Float fValue)
		{
			double dblA = Math.Asin(fValue);
			Verbose("llAsin({0})={1}", fValue, dblA);
			return dblA;
		}

		public Float llAtan2(Float y, Float x)
		{
			double dblA = Math.Atan2(y, x);
			Verbose("llAtan2({0}, {1})={2}", y, x, dblA);
			return dblA;
		}

		public void llAttachToAvatar(integer iAttachPoint)
		{
			Verbose("llAttachToAvatar({0})", iAttachPoint);
		}

		public void llAttachToAvatarTemp(integer iAttachPoint)
		{
			Verbose("llAttachToAvatarTemp({0})", iAttachPoint);
		}

		public key llAvatarOnLinkSitTarget(integer iLinkIndex)
		{
			key kLinkUUID = new key(Guid.NewGuid());
			Verbose("llAvatarOnLinkSitTarget({0}))={1}", iLinkIndex, kLinkUUID);
			return kLinkUUID;
		}

		public key llAvatarOnSitTarget()
		{
			key kLinkUUID = new key(Guid.NewGuid());
			Verbose("llAvatarOnSitTarget()={0}", kLinkUUID);
			return kLinkUUID;
		}

		public rotation llAxes2Rot(vector vForward, vector vLeft, vector vUp)
		{
			rotation rRot = rotation.ZERO_ROTATION;
			Verbose("llAxes2Rot({0}, {1}, {2})={3}", vForward, vLeft, +vUp, rRot);
			return rRot;
		}

		public rotation llAxisAngle2Rot(vector vAxis, Float fAngle)
		{
			vector vUnitAxis = llVecNorm(vAxis);
			double dSineHalfAngle = Math.Sin(fAngle / 2);
			double dCosineHalfAngle = Math.Cos(fAngle / 2);

			rotation rResult = new rotation(
				dSineHalfAngle * vUnitAxis.x,
				dSineHalfAngle * vUnitAxis.y,
				dSineHalfAngle * vUnitAxis.z,
				dCosineHalfAngle);

			Verbose("llAxisAngle2Rot({0}, {1})={2}", vAxis, fAngle, rResult);
			return rResult;
		}

		public integer llBase64ToInteger(String sString)
		{
			int iResult;

			try {
				string s = sString;
				byte[] data = new byte[4];

				if (s.Length > 1) {
					data[3] = (byte)(LookupBase64(s, 0) << 2);
					data[3] |= (byte)(LookupBase64(s, 1) >> 4);
				}

				if (s.Length > 2) {
					data[2] = (byte)((LookupBase64(s, 1) & 0xf) << 4);
					data[2] |= (byte)(LookupBase64(s, 2) >> 2);
				}

				if (s.Length > 3) {
					data[1] = (byte)((LookupBase64(s, 2) & 0x7) << 6);
					data[1] |= (byte)LookupBase64(s, 3);
				}

				if (s.Length > 5) {
					data[0] = (byte)(LookupBase64(s, 4) << 2);
					data[0] |= (byte)(LookupBase64(s, 5) >> 4);
				}

				iResult = BitConverter.ToInt32(data, 0);

				// 0 12 34 56
				// 1 78 12 34
				// 2 56 78 12
				// 3 34 56 78

				// 4 12 34 56
				// 5 78 12 34
				// 6 56 78 12
				// 7 34 56 78
			} catch {
				iResult = (new Random()).Next();
			}
			Verbose(@"llBase64ToInteger(""{0}"")={1}", sString, iResult);
			return iResult;
		}

		public String llBase64ToString(String sString)
		{
			string sResult = Base64ToString(sString.ToString());
			Verbose(@"llBase64ToString(""{0}"")=""{1}""", sString, sResult);
			return sResult;
		}

		public void llBreakAllLinks()
		{
			host.llBreakAllLinks();
			Verbose("llBreakAllLinks()");
		}

		public void llBreakLink(integer iLinkIndex)
		{
			host.llBreakLink(iLinkIndex);
			Verbose("llBreakLink({0})", iLinkIndex);
		}

		public list llCSV2List(String sString)
		{
			string sSource = sString;
			list lResult = new list();
			StringBuilder sb = new StringBuilder();
			int intWithinAngelBracket = 0;
			for (int intI = 0; intI < sSource.Length; intI++) {
				char chrC = sSource[intI];
				if (chrC == '<') {
					intWithinAngelBracket++;
				} else if (chrC == '>') {
					intWithinAngelBracket--;
				}

				if (intWithinAngelBracket == 0 && chrC == ',') {
					lResult.Add(sb.ToString());
					sb = new StringBuilder();
				} else {
					sb.Append(sSource[intI]);
				}
			}

			// dont forget the last one
			lResult.Add(sb.ToString());

			// remove the first space, if any
			for (int intI = 0; intI < lResult.Count; intI++) {
				string strValue = lResult[intI].ToString();
				if (strValue == "") {
					continue;
				}
				if (strValue[0] == ' ') {
					lResult[intI] = strValue.Substring(1);
				}
			}

			Verbose(@"llCSV2List(""{0}"")={1}", sSource, lResult.ToVerboseString());

			return lResult;
		}

		public list llCastRay(vector vStart, vector vEnd, list lOptions)
		{
			list lResult = new list();
			Verbose("llCastRay({0}, {1}, {2})={3}", vStart, vEnd, lOptions.ToVerboseString(), lResult.ToVerboseString());
			return lResult;
		}

		public integer llCeil(Float fValue)
		{
			int intA = (int)Math.Ceiling(fValue);
			Verbose("llCeiling({0})={1}", fValue, intA);
			return intA;
		}

		public void llClearCameraParams()
		{
			Verbose("llClearCameraParams()");
		}

		public integer llClearLinkMedia(integer iLink, integer iFace)
		{
			Verbose("llClearLinkMedia({0}, {1})={2}", iLink, iFace, true);
			return true;
		}

		public integer llClearPrimMedia(integer iFace)
		{
			integer iResult = 0;
			Verbose("llClearPrimMedia({0})={1}", iFace, iResult);
			return iResult;
		}

		public void llCloseRemoteDataChannel(key kChannel)
		{
			host.llCloseRemoteDataChannel(kChannel);
			Verbose("llCloseRemoteDataChannel({0})", kChannel);
		}

		public Float llCloud(vector vOffset)
		{
			Float fResult = 0.0F;
			Verbose("llCloud({0})={1}", vOffset, fResult);
			return fResult;
		}

		public void llCollisionFilter(String sName, key kID, integer iAccept)
		{
			Verbose(@"llCollisionFilter(""{0}"", {1}, {2})", sName, kID, iAccept);
		}

		public void llCollisionSound(String sImpactSound, Float fImpactVolume)
		{
			Verbose(@"llCollisionSound(""{0}"", {1})", sImpactSound, +fImpactVolume);
		}

		public void llCollisionSprite(String sImpactSprite)
		{
			Verbose(@"llCollisionSprite(""{0}"")", sImpactSprite);
		}

		public Float llCos(Float fTheta)
		{
			double dblA = Math.Cos(fTheta);
			Verbose("llCos({0})={1}", fTheta, dblA);
			return dblA;
		}

		public void llCreateCharacter(list lOptions)
		{
			Verbose("llCreateCharacter({0})", lOptions.ToVerboseString());
		}

		public void llCreateLink(key kID, integer iSimulator)
		{
			Verbose("llCreateLink({0}, {1})", kID, iSimulator);
		}

		public void llDeleteCharacter()
		{
			Verbose("llDeleteCharacter()");
		}

		public list llDeleteSubList(list lSource, integer iStart, integer iEnd)
		{
			int intLength = lSource.Count;

			int start = iStart;
			int end = iEnd;

			list src = new list(lSource);

			if (CorrectIt(intLength, ref start, ref end)) {
				if (start <= end) {
					for (int intI = start; intI <= end; intI++) {
						src[intI] = null;
					}
				} else { // excluding
					for (int intI = 0; intI <= end; intI++) {
						src[intI] = null;
					}
					for (int intI = start; intI < intLength; intI++) {
						src[intI] = null;
					}
				}
			}
			list lResult = new list();
			for (int intI = 0; intI < src.Count; intI++) {
				if (src[intI] != null) {
					lResult.Add(src[intI]);
				}
			}
			Verbose(string.Format("llDeleteSubList({0}, {1}, {2})={3}", lSource.ToVerboseString(), iStart, iEnd, lResult.ToVerboseString()));
			return lResult;
		}

		public String llDeleteSubString(String sSource, integer iStart, integer iEnd)
		{
			char[] src = sSource.ToString().ToCharArray();
			int start = iStart;
			int end = iEnd;

			int intLength = src.Length;

			if (CorrectIt(intLength, ref start, ref end)) {
				if (start <= end) {
					for (int intI = start; intI <= end; intI++) {
						src[intI] = '\0';
					}
				} else { // excluding
					for (int intI = 0; intI <= end; intI++) {
						src[intI] = '\0';
					}
					for (int intI = start; intI < intLength; intI++) {
						src[intI] = '\0';
					}
				}
			}
			StringBuilder result = new StringBuilder();
			for (int intI = 0; intI < src.Length; intI++) {
				if (src[intI] != '\0') {
					result.Append(src[intI]);
				}
			}

			Verbose(string.Format(@"llDeleteSubString(""{0}"", {1}, {2})=""{3}""", sSource, iStart, iEnd, result));
			return result.ToString();
		}

		public void llDetachFromAvatar()
		{
			Verbose("llDetachFromAvatar()");
		}

		public vector llDetectedGrab(integer iIndex)
		{
			vector vCoords = vector.ZERO_VECTOR;
			Verbose("llDetectedGrab({0})={1}", iIndex, vCoords);
			return vCoords;
		}

		public integer llDetectedGroup(integer iIndex)
		{
			integer iDetected = 0;
			Verbose("llDetectedGroup({0})={1}", iIndex, iDetected);
			return iDetected;
		}

		public key llDetectedKey(integer iIndex)
		{
			key kID = new key(Properties.Settings.Default.AvatarKey);
			Verbose("llDetectedKey({0})={1}", iIndex, kID);
			return kID;
		}

		public integer llDetectedLinkNumber(integer iLinkIndex)
		{
			integer iDetected = 0;
			Verbose("llDetectedLinkNumber({0})={1}", iLinkIndex, iDetected);
			return iDetected;
		}

		public String llDetectedName(integer iIndex)
		{
			string sResult = Properties.Settings.Default.AvatarName;
			Verbose(@"llDetectedName({0})=""{1}""", iIndex, sResult);
			return sResult;
		}

		public key llDetectedOwner(integer iIndex)
		{
			key kID = new key(Properties.Settings.Default.AvatarKey);
			Verbose("llDetectedOwner({0})={1}", iIndex, kID);
			return kID;
		}

		public vector llDetectedPos(integer iIndex)
		{
			vector vCoords = vector.ZERO_VECTOR;
			Verbose("llDetectedPos({0})={1}", iIndex, vCoords);
			return vCoords;
		}

		public rotation llDetectedRot(integer iIndex)
		{
			rotation rRotation = rotation.ZERO_ROTATION;
			Verbose("llDetectedRot({0})={1}", iIndex, rRotation);
			return rRotation;
		}

		public vector llDetectedTouchBinormal(integer iIndex)
		{
			vector vCoords = new vector();
			Verbose("llDetectedTouchBinormal({0})={1}", iIndex, vCoords);
			return vCoords;
		}

		public integer llDetectedTouchFace(integer iIndex)
		{
			integer iFace = 0;
			Verbose("llDetectedTouchFace({0})={1}", iIndex, iFace);
			return iFace;
		}

		public vector llDetectedTouchNormal(integer iIndex)
		{
			vector vNormal = new vector();
			Verbose("llDetectedTouchNormal({0})={1}", iIndex, vNormal);
			return vNormal;
		}

		public vector llDetectedTouchPos(integer iIndex)
		{
			vector vCoords = vector.ZERO_VECTOR;
			Verbose("llDetectedTouchPos({0})={1}", iIndex, vCoords);
			return vCoords;
		}

		public vector llDetectedTouchST(integer iIndex)
		{
			vector vCoords = vector.ZERO_VECTOR;
			Verbose("llDetectedTouchST({0})={1}", iIndex, vCoords);
			return vCoords;
		}

		public vector llDetectedTouchUV(integer iIndex)
		{
			vector vUV = vector.ZERO_VECTOR;
			Verbose("llDetectedTouchUV({0})={1}", iIndex, vUV);
			return vUV;
		}

		public integer llDetectedType(integer iIndex)
		{
			integer iResult = AGENT;
			Verbose("llDetectedType({0})={1}", iIndex, iResult);
			return iResult;
		}

		public vector llDetectedVel(integer iIndex)
		{
			vector vVelocity = vector.ZERO_VECTOR;
			Verbose("llDetectedVel({0})={1}", iIndex, vVelocity);
			return vVelocity;
		}

		public void llDialog(key kID, String sMessage, list lButtons, integer iChannel)
		{
			Verbose(@"llDialog({0}, ""{1}"", {2}, {3})", kID, sMessage, lButtons.ToVerboseString(), iChannel);
			host.llDialog(kID, sMessage, lButtons, iChannel);
		}

		public void llDie()
		{
			Verbose("llDie()");
			host.Die();
		}

		public String llDumpList2String(list lSource, String sSeparator)
		{
			StringBuilder result = new StringBuilder();
			for (int intI = 0; intI < lSource.Count; intI++) {
				if (intI > 0) {
					result.Append(sSeparator.ToString());
				}
				result.Append(lSource[intI].ToString());
			}
			Verbose(@"llDumpList2String({0},""{1}"")=""{2}""", lSource.ToVerboseString(), sSeparator, result.ToString());
			return result.ToString();
		}

		public integer llEdgeOfWorld(vector vPosition, vector vDirection)
		{
			integer iResult = 0;
			Verbose("llEdgeOfWorld({0}, {1})={2}", vPosition, vDirection, iResult);
			return iResult;
		}

		public void llEjectFromLand(key kID)
		{
			Verbose("llEjectFromLand({0})", kID);
		}

		public void llEmail(String sAddress, String sSubject, String sMessage)
		{
			Verbose(@"llEmail(""{0}"", ""{1}"", ""{2}"")", sAddress, sSubject, sMessage);
			host.Email(sAddress, sSubject, sMessage);
		}

		public String llEscapeURL(String sURL)
		{
			StringBuilder sb = new StringBuilder();
			byte[] data = Encoding.UTF8.GetBytes(sURL.ToString());
			for (int intI = 0; intI < data.Length; intI++) {
				byte chrC = data[intI];
				if ((chrC >= 'a' && chrC <= 'z') || (chrC >= 'A' && chrC <= 'Z') || (chrC >= '0' && chrC <= '9')) {
					sb.Append((char)chrC);
				} else {
					sb.AppendFormat("%{0:X2}", (int)chrC);
				}
			}
			Verbose(string.Format(@"EscapeURL(""{0}"")=""{1}""", sURL, sb.ToString()));
			return sb.ToString();
		}

		public rotation llEuler2Rot(vector v)
		{
			v /= 2.0;
			double ax = Math.Sin(v.x);
			double aw = Math.Cos(v.x);
			double by = Math.Sin(v.y);
			double bw = Math.Cos(v.y);
			double cz = Math.Sin(v.z);
			double cw = Math.Cos(v.z);
			rotation rRotation = new rotation(
				(aw * by * cz) + (ax * bw * cw),
				(aw * by * cw) - (ax * bw * cz),
				(aw * bw * cz) + (ax * by * cw),
				(aw * bw * cw) - (ax * by * cz));
			Verbose("llEuler2Rot({0})={1}", v, rRotation);
			return rRotation;
		}

		public void llEvade(key kTargetID, list lOptions)
		{
			Verbose("llEvade({0}, {1})", kTargetID, lOptions);
		}

		public void llExecCharacterCmd(integer iCommand, list lOptions)
		{
			Verbose("llExecCharacterCmd({0}, {1})", iCommand, lOptions.ToVerboseString());
		}

		public Float llFabs(Float fValue)
		{
			double dblA = Math.Abs(fValue);
			Verbose("llFabs({0})={1}", fValue, dblA);
			return dblA;
		}

		public void llFleeFrom(vector vSource, Float fDistance, list lOptions)
		{
			Verbose("llFleeFrom({0}, {1}, {2})", vSource, fDistance, lOptions.ToVerboseString());
		}

		public integer llFloor(Float fValue)
		{
			int intA = (int)Math.Floor(fValue);
			Verbose("llFloor({0})={1}", fValue, intA);
			return intA;
		}

		public void llForceMouselook(integer iMouselook)
		{
			Verbose("llForceMouselook({0})", iMouselook);
		}

		public Float llFrand(Float fMaximum)
		{
			double dblValue = fMaximum * rdmRandom.NextDouble();
			Verbose("llFrand({0})={1}", fMaximum, dblValue);
			return dblValue;
		}

		public key llGenerateKey()
		{
			key kID = new key(Guid.NewGuid());
			Verbose("llGenerateKey()={0}", kID);
			return kID;
		}

		public vector llGetAccel()
		{
			vector vAcceleration = vector.ZERO_VECTOR;
			Verbose("llGetAccel()={0}", vAcceleration);
			return vAcceleration;
		}

		public integer llGetAgentInfo(key kID)
		{
			integer iAgentFlags = 0;
			Verbose("llGetAgentInfo({0})={1}", kID, iAgentFlags);
			return iAgentFlags;
		}

		public String llGetAgentLanguage(key kID)
		{
			string sLanguageCode = "en-us";
			Verbose(@"llGetAgentLanguage({0})=""{1}""", kID, sLanguageCode);
			return sLanguageCode;
		}

		public list llGetAgentList(integer iScope, list lOptions)
		{
			list lAgents = new list();
			Verbose("llGetAgentList({0}, [{1}])={2}", iScope, lOptions, lAgents.ToVerboseString());
			return lAgents;
		}

		public vector llGetAgentSize(key kID)
		{
			vector vAgentSize = vector.ZERO_VECTOR;
			Verbose("llGetAgentSize({0})={1}", kID, vAgentSize);
			return vAgentSize;
		}

		public Float llGetAlpha(integer iFace)
		{
			Float fOpacity = 1.0F;
			Verbose("llGetAlpha({0})={1}", iFace, fOpacity);
			return fOpacity;
		}

		public Float llGetAndResetTime()
		{
			// get time
			double dblTime = llGetTime();
			Verbose("llGetAndResetTime()=" + dblTime);

			// reset time
			llResetTime();
			return dblTime;
		}

		public String llGetAnimation(key kID)
		{
			String sAnimation = "";
			Verbose(@"llGetAnimation({0})=""{1}""", kID, sAnimation);
			return sAnimation;
		}

		public list llGetAnimationList(key kID)
		{
			list lAnimationList = new list();
			Verbose("llGetAnimationList({0})={1}", kID, lAnimationList.ToVerboseString());
			return lAnimationList;
		}

		public String llGetAnimationOverride(String sAnimationState)
		{
			String sAnimation = "";
			Verbose(@"llGetAnimationOverride(""{0}"")=""{1}""", sAnimationState, sAnimation);
			return sAnimation;
		}

		public integer llGetAttached()
		{
			integer iAttachPoint = 0;
			Verbose("llGetAttached()={0}", iAttachPoint);
			return iAttachPoint;
		}

		public list llGetBoundingBox(key kID)
		{
			list lBoundingCoords = new list();
			Verbose("llGetBoundingBox({0})={1}", kID, lBoundingCoords.ToVerboseString());
			return lBoundingCoords;
		}

		public vector llGetCameraPos()
		{
			vector vCameraCoords = vector.ZERO_VECTOR;
			Verbose("llGetCameraPos()={0}", vCameraCoords);
			return vCameraCoords;
		}

		public rotation llGetCameraRot()
		{
			rotation rCameraRotation = rotation.ZERO_ROTATION;
			Verbose("llGetCameraRot()={0}", rCameraRotation);
			return rCameraRotation;
		}

		public vector llGetCenterOfMass()
		{
			vector vCenterOfMass = vector.ZERO_VECTOR;
			Verbose("llGetCenterOfMass()={0}", vCenterOfMass);
			return vCenterOfMass;
		}

		public list llGetClosestNavPoint(vector lPoint, list lOptions)
		{
			list lClosetNavPoint = new list();
			Verbose("llGetClosestNavPoint({0}, {1})={2}", lPoint, lOptions.ToVerboseString(), lClosetNavPoint.ToVerboseString());
			return lClosetNavPoint;
		}

		public vector llGetColor(integer iFace)
		{
			vector vColour = vector.ZERO_VECTOR;
			Verbose("llGetColor({0})={1}", iFace, vColour);
			return vColour;
		}

		public key llGetCreator()
		{
			key kResult = Properties.Settings.Default.AvatarKey;
			Verbose("llGetCreator()={0}", kResult);
			return kResult;
		}

		public String llGetDate()
		{
			string sResult = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd");
			Verbose("llGetDate()={0}", sResult);
			return sResult;
		}

		public string llGetDisplayName(key kAvatarID)
		{
			string sResult = "";
			Verbose("llGetDisplayName({0})={1}", kAvatarID, sResult);
			return sResult;
		}

		public Float llGetEnergy()
		{
			Float fResult = 1.23;
			Verbose("llGetEnergy()={0}", fResult);
			return fResult;
		}

		public string llGetEnv(string sDataRequest)
		{
			string sResult;
			switch (sDataRequest) {
				case "sim_channel":
					sResult = "Second Life Server";
					break;
				case "sim_version":
					sResult = "11.11.09.244706";
					break;
				default:
					sResult = "";
					break;
			}
			Verbose(@"llGetEnv(""{0}"")=""{1}""", sDataRequest, sResult);
			return sResult;
		}

		public vector llGetForce()
		{
			vector vForce = vector.ZERO_VECTOR;
			Verbose("llGetForce()={0}", vForce);
			return vForce;
		}

		public integer llGetFreeMemory()
		{
			integer iFreeMemory = 16000;
			Verbose("llGetFreeMemory()={0}", iFreeMemory);
			return iFreeMemory;
		}

		public integer llGetFreeURLs()
		{
			integer iFreeURLs = 0;
			Verbose("llGetFreeURLs()={0}", iFreeURLs);
			return iFreeURLs;
		}

		public Float llGetGMTclock()
		{
			Float fResult = DateTime.Now.ToUniversalTime().TimeOfDay.TotalSeconds;
			Verbose("llGetGMTclock()={0}", fResult);
			return fResult;
		}

		public vector llGetGeometricCenter()
		{
			vector vResult = ZERO_VECTOR;
			Verbose("llGetGeometricCenter()={0}", vResult);
			return vResult;
		}

		public String llGetHTTPHeader(key kRequestID, String sDesiredHeader)
		{
			String sResult = "not-implemented";
			Verbose(@"llGetHTTPHeader({0}, ""{1}"")=""{2}""", kRequestID, sDesiredHeader, sResult);
			return sResult;
		}

		public key llGetInventoryCreator(String sItem)
		{
			key kAvatarID = Properties.Settings.Default.AvatarKey;
			Verbose(@"llGetInventoryCreator(""{0}"")={1}", sItem, kAvatarID);
			return kAvatarID;
		}

		public key llGetInventoryKey(String sItemName)
		{
			key kID = host.GetInventoryKey(sItemName);
			Verbose(@"llGetInventoryKey(""{0}"")={1}", sItemName, kID);
			return kID;
		}

		public String llGetInventoryName(integer iItemType, integer iItemIndex)
		{
			string sItemName = host.GetInventoryName(iItemType, iItemIndex);
			Verbose(@"llGetInventoryName({0}, {1})=""{2}""", iItemType, iItemIndex, sItemName);
			return sItemName;
		}

		public integer llGetInventoryNumber(integer iType)
		{
			int iTypeCount = host.GetInventoryNumber(iType);
			Verbose("llGetInventoryNumber({0})={1}", iType, iTypeCount);
			return iTypeCount;
		}

		public integer llGetInventoryPermMask(String sItemName, integer iPermMask)
		{
			integer iPermissionState = 0;
			Verbose(@"llGetInventoryPermMask(""{0}"", {1})={2}", sItemName, iPermMask, iPermissionState);
			return iPermissionState;
		}

		public integer llGetInventoryType(String sItemName)
		{
			integer iItemType = host.GetInventoryType(sItemName);
			Verbose(@"llGetInventoryType(""{0}"")={1}", sItemName, iItemType);
			return iItemType;
		}

		public key llGetKey()
		{
			key kID = host.GetKey();
			Verbose(@"llGetKey()=""{0}""", kID.ToString());
			return kID;
		}

		public key llGetLandOwnerAt(vector vPosition)
		{
			key kAvatarID = new key(Guid.NewGuid());
			Verbose("llGetLandOwnerAt({0})={1}", vPosition, kAvatarID);
			return kAvatarID;
		}

		public key llGetLinkKey(integer iLinkIndex)
		{
			key kID = new key(Guid.NewGuid());
			Verbose("llGetLinkKey({0})={1}", iLinkIndex, kID);
			return kID;
		}

		public list llGetLinkMedia(integer iLinkIndex, integer iFace, list lParameters)
		{
			list lMediaList = new list();
			Verbose("llGetLinkMedia({0}, {1}, {2})={3}", iLinkIndex, iFace, lParameters.ToVerboseString(), lMediaList.ToVerboseString());
			return lMediaList;
		}

		public String llGetLinkName(integer iLinkIndex)
		{
			String sLinkName = "";
			Verbose(@"llGetLinkName({0})=""{1}""", iLinkIndex, sLinkName);
			return sLinkName;
		}

		public integer llGetLinkNumber()
		{
			integer iLinkIndex = 0;
			Verbose("llGetLinkNumber()={0}", iLinkIndex);
			return iLinkIndex;
		}

		public integer llGetLinkNumberOfSides(integer iLinkIndex)
		{
			integer iSides = 6;
			Verbose("llGetLinkNumberOfSides({0})={1}", iLinkIndex, iSides);
			return iSides;
		}

		public list llGetLinkPrimitiveParams(integer iLinkIndex, list lParametersRequested)
		{
			list lParametersReturned = new list();
			Verbose("llGetLinkPrimitiveParams({0}, {1})={2}", iLinkIndex, lParametersRequested.ToVerboseString(), lParametersReturned);
			return lParametersReturned;
		}

		public integer llGetListEntryType(list lSource, integer iIndex)
		{
			integer iEntryType;

			if (iIndex < 0) {
				iIndex = lSource.Count + iIndex;
			}
			if (iIndex >= lSource.Count || iIndex < 0) {
				iEntryType = 0;
			} else {
				switch (lSource[iIndex].GetType().ToString().Replace("LSLEditor.SecondLife+", "")) {
					case "System.Double":
					case "Float":
						iEntryType = TYPE_FLOAT;
						break;
					case "System.String":
					case "String":
						iEntryType = TYPE_STRING;
						break;
					case "System.Int32":
					case "integer":
						iEntryType = TYPE_INTEGER;
						break;
					case "key":
						iEntryType = TYPE_KEY;
						break;
					case "vector":
						iEntryType = TYPE_VECTOR;
						break;
					case "rotation":
						iEntryType = TYPE_ROTATION;
						break;
					default:
						iEntryType = TYPE_INVALID;
						break;
				}
			}
			Verbose("llGetListEntryType({0}, {1})={2}", lSource.ToVerboseString(), iIndex, iEntryType);
			return iEntryType;
		}

		public integer llGetListLength(list lSource)
		{
			integer iLength = lSource.Count;
			Verbose("llGetListLength({0})={1}", lSource.ToVerboseString(), iLength);
			return iLength;
		}

		public vector llGetLocalPos()
		{
			Verbose("llGetLocalPos()={0}", vPosition);
			return vPosition;
		}

		public rotation llGetLocalRot()
		{
			Verbose("llGetLocalRot()={0}", rRotationlocal);
			return rRotationlocal;
		}

		public Float llGetMass()
		{
			Float fMass = 1.23;
			Verbose("llGetMass()={0}", fMass);
			return fMass;
		}

		public Float llGetMassMKS()
		{
			Float fMass = 1.23;
			Verbose("llGetMassMKS()={0}", fMass);
			return fMass;
		}

		public integer llGetMemoryLimit()
		{
			integer iMaxMemory = 65536;
			Verbose("llGetMemoryLimit()={0}", iMaxMemory);
			return iMaxMemory;
		}

		public void llGetNextEmail(String sAddress, String sSubject)
		{
			Verbose(@"llGetNextEmail(""{0}"", ""{1}"")", sAddress, sSubject);
		}

		public key llGetNotecardLine(String sNoteName, integer iLineIndex)
		{
			key kID = host.GetNotecardLine(sNoteName, iLineIndex);
			Verbose(@"llGetNotecardLine(""{0}"", {1})={2}", sNoteName, iLineIndex, kID);
			return kID;
		}

		public key llGetNumberOfNotecardLines(String sNoteName)
		{
			key kID = host.GetNumberOfNotecardLines(sNoteName);
			Verbose(@"llGetNumberOfNotecardLines(""{0}"")={1}", sNoteName, kID);
			return kID;
		}

		public integer llGetNumberOfPrims()
		{
			integer iPrimCount = 10;
			Verbose("llGetNumberOfPrims()={0}", iPrimCount);
			return iPrimCount;
		}

		public integer llGetNumberOfSides()
		{
			integer iSideCount = 6;
			Verbose("llGetNumberOfSides()={0}", iSideCount);
			return iSideCount;
		}

		public String llGetObjectDesc()
		{
			string sDescription = host.GetObjectDescription();
			Verbose(@"llGetObjectDesc()=""{0}""", sDescription);
			return sDescription;
		}

		public list llGetObjectDetails(key kID, list lObjectFlags)
		{
			list lObjectDetails = new list();
			for (int intI = 0; intI < lObjectFlags.Count; intI++) {
				if (!(lObjectFlags[intI] is integer)) {
					continue;
				}
				switch ((int)(integer)lObjectFlags[intI]) {
					case OBJECT_NAME:
						lObjectDetails.Add((SecondLife.String)host.GetObjectName(new Guid(kID.guid)));
						break;
					case OBJECT_DESC:
						lObjectDetails.Add((SecondLife.String)host.GetObjectDescription(new Guid(kID.guid)));
						break;
					case OBJECT_POS:
						lObjectDetails.Add(llGetPos());
						break;
					case OBJECT_ROT:
						lObjectDetails.Add(llGetRot());
						break;
					case OBJECT_VELOCITY:
						lObjectDetails.Add(llGetVel());
						break;
					case OBJECT_OWNER:
						lObjectDetails.Add(llGetOwner());
						break;
					case OBJECT_GROUP:
						lObjectDetails.Add(OBJECT_UNKNOWN_DETAIL);
						break;
					case OBJECT_CREATOR:
						lObjectDetails.Add(llGetCreator());
						break;
					default:
						lObjectDetails.Add(OBJECT_UNKNOWN_DETAIL);
						break;
				}
			}
			Verbose(@"llGetObjectDetails({0}, {1})={2}", kID, lObjectFlags, lObjectDetails);
			return lObjectDetails;
		}

		public Float llGetObjectMass(key kID)
		{
			Float fMass = 0.0F;
			Verbose("llGetObjectMass({0})={1}", kID, fMass);
			return fMass;
		}

		public String llGetObjectName()
		{
			string sObjectName = host.GetObjectName();
			Verbose(@"llGetObjectName()=""{0}""", sObjectName);
			return sObjectName;
		}

		public integer llGetObjectPermMask(integer iRequestedMask)
		{
			integer iRetunedMaskState = 0;
			Verbose("llGetObjectPermMask({0})={1}", iRequestedMask, iRetunedMaskState);
			return iRetunedMaskState;
		}

		// added 4 mei 2007
		public integer llGetObjectPrimCount(key kID)
		{
			integer iPrimCount = 0;
			Verbose("llGetObjectPrimCount({0})={1}", kID, iPrimCount);
			return iPrimCount;
		}

		public vector llGetOmega()
		{
			vector vOmega = vector.ZERO_VECTOR;
			Verbose("llGetOmega()={0}", vOmega);
			return vOmega;
		}

		public key llGetOwner()
		{
			key kID = new key(Properties.Settings.Default.AvatarKey);
			Verbose("llGetOwner()={0}", kID);
			return kID;
		}

		public key llGetOwnerKey(key kID)
		{
			key kAvatarID = llGetOwner();	// This is incorrect, as the owner of this object may not be the owner of kID
			Verbose("llGetOwnerKey({0})={1}", kID, kAvatarID);
			return kAvatarID;
		}

		/*
		PARCEL_DETAILS_NAME  0  The name of the parcel.  63 Characters  string
		PARCEL_DETAILS_DESC  1  The description of the parcel.  127 Characters  string
		PARCEL_DETAILS_OWNER  2  The parcel owner's key.  (36 Characters)  key
		PARCEL_DETAILS_GROUP  3  The parcel group's key.  (36 Characters)  key
		PARCEL_DETAILS_AREA  4  The parcel's area, in sqm.  (5 Characters)  integer
		*/
		public list llGetParcelDetails(vector vPosition, list lRequestedDetails)
		{
			list lReturnedDetails = new list();
			for (int intI = 0; intI < lRequestedDetails.Count; intI++) {
				if (lRequestedDetails[intI] is integer) {
					switch ((int)(integer)lRequestedDetails[intI]) {
						case PARCEL_DETAILS_NAME:
							lReturnedDetails.Add(Properties.Settings.Default.ParcelName);
							break;
						case PARCEL_DETAILS_DESC:
							lReturnedDetails.Add(Properties.Settings.Default.ParcelDescription);
							break;
						case PARCEL_DETAILS_OWNER:
							lReturnedDetails.Add(new key(Properties.Settings.Default.ParcelOwner));
							break;
						case PARCEL_DETAILS_GROUP:
							lReturnedDetails.Add(new key(Properties.Settings.Default.ParcelGroup));
							break;
						case PARCEL_DETAILS_AREA:
							lReturnedDetails.Add(new integer(Properties.Settings.Default.ParcelArea));
							break;
						case PARCEL_DETAILS_ID:
							lReturnedDetails.Add(new integer(Properties.Settings.Default.ParcelID));
							break;
						case PARCEL_DETAILS_SEE_AVATARS:
							lReturnedDetails.Add(new integer(Properties.Settings.Default.ParcelSeeAvatars));
							break;
						default:
							break;
					}
				}
			}
			Verbose("llGetParcelDetails({0}, {1})={2}", vPosition, lRequestedDetails.ToVerboseString(), lReturnedDetails.ToVerboseString());
			return lReturnedDetails;
		}

		public integer llGetParcelFlags(vector vPosition)
		{
			integer iReturnedFlags = 0;
			Verbose("llGetParcelFlags({0})={1}", vPosition, iReturnedFlags);
			return iReturnedFlags;
		}

		public integer llGetParcelMaxPrims(vector vPosition, integer iSimWide)
		{
			integer iMaxPrims = 0;
			Verbose("llGetParcelMaxPrims({0}, {1})={2}", vPosition, iSimWide, iMaxPrims);
			return iMaxPrims;
		}

		public string llGetParcelMusicURL()
		{
			Verbose(@"llGetParcelMaxPrims()=""{0}""", sParcelMusicURL);
			return sParcelMusicURL;
		}

		public integer llGetParcelPrimCount(vector vPosition, integer iCategory, integer iSimWide)
		{
			integer iPrimCount = 0;
			Verbose("llGetParcelPrimCount({0}, {1}, {2})={3}", vPosition, iCategory, iSimWide, iPrimCount);
			return iPrimCount;
		}

		public list llGetParcelPrimOwners(vector vPosition)
		{
			list lOwners = new list(new object[] { Properties.Settings.Default.AvatarKey, 10 });
			Verbose("llGetParcelPrimOwners({0})={1}", vPosition, lOwners);
			return lOwners;
		}

		public integer llGetPermissions()
		{
			integer iPermissions = 0;
			Verbose("llGetPermissions()={0}", iPermissions);
			return iPermissions;
		}

		public key llGetPermissionsKey()
		{
			key kID = key.NULL_KEY;
			Verbose("llGetPermissionsKey()={0}", kID);
			return kID;
		}

		public list llGetPhysicsMaterial()
		{
			list lMaterials = new list();
			Verbose("llGetPhysicalMaterial()={0}", lMaterials);
			return lMaterials;
		}

		public vector llGetPos()
		{
			Verbose("llGetPos()={0}", vPosition);
			return vPosition;
		}

		public list llGetPrimMediaParams(integer iFace, list lDesiredParams)
		{
			list lReturnedParams = new list();
			Verbose("llGetPrimMediaParams({0}, {1})={2}", iFace, lDesiredParams.ToVerboseString(), lReturnedParams.ToVerboseString());
			return lReturnedParams;
		}

		public list llGetPrimitiveParams(list lDesiredParams)
		{
			list lReturnedParams = new list();
			Verbose("llGetPrimitiveParams({0})={1}", lDesiredParams.ToVerboseString(), lReturnedParams.ToVerboseString());
			return lReturnedParams;
		}

		// 334
		public integer llGetRegionAgentCount()
		{
			integer iAgentCount = 0;
			Verbose("llGetRegionAgentCount()={0}", iAgentCount);
			return iAgentCount;
		}

		public vector llGetRegionCorner()
		{
			System.Drawing.Point pRegionCorner = Properties.Settings.Default.RegionCorner;
			vector vRegionCorner = new vector(pRegionCorner.X, pRegionCorner.Y, 0);
			Verbose("llGetRegionCorner()={0}", vRegionCorner);
			return vRegionCorner;
		}

		public Float llGetRegionFPS()
		{
			Float fRegionFPS = Properties.Settings.Default.RegionFPS;
			Verbose("llGetRegionFPS()={0}", fRegionFPS);
			return fRegionFPS;
		}

		public integer llGetRegionFlags()
		{
			integer iRegionFlags = 0;
			Verbose("llGetRegionFlags()={0}", iRegionFlags);
			return iRegionFlags;
		}

		public String llGetRegionName()
		{
			String sRegionName = Properties.Settings.Default.RegionName;
			Verbose("llGetRegionName()={0}", sRegionName);
			return sRegionName;
		}

		public Float llGetRegionTimeDilation()
		{
			Float fTimeDilation = 0.9F;
			Verbose("llGetRegionTimeDilation()={0}", fTimeDilation);
			return fTimeDilation;
		}

		public vector llGetRootPosition()
		{
			vector vRootPosition = vector.ZERO_VECTOR;
			Verbose("llGetRootPosition()={0}", vRootPosition);
			return vRootPosition;
		}

		public rotation llGetRootRotation()
		{
			rotation vRootRotation = rotation.ZERO_ROTATION;
			Verbose("llGetRootRotation()={0}", vRootRotation);
			return vRootRotation;
		}

		public rotation llGetRot()
		{
			Verbose("llGetRot()={0}", rRotation);
			return rRotation;
		}

		public integer llGetSPMaxMemory()
		{
			integer iMaxSPMemory = 65536;
			Verbose("llGetSPMaxMemory()={0}", iMaxSPMemory);
			return iMaxSPMemory;
		}

		public vector llGetScale()
		{
			Verbose("llGetScale()=" + vScale);
			return vScale;
		}

		public String llGetScriptName()
		{
			string sScriptName = host.GetScriptName();
			Verbose("llGetScriptName()={0}", sScriptName);
			return sScriptName;
		}

		public integer llGetScriptState(String sScriptName)
		{
			integer iScriptState = 0;
			Verbose(@"llGetScriptState(""{0}"")={1}", sScriptName, iScriptState);
			return iScriptState;
		}

		public float llGetSimStats(integer iStatType)
		{
			float iSimStat = 0.0F;
			Verbose("llGetSimStats({0})={1}", iStatType, iSimStat);
			return iSimStat;
		}

		public String llGetSimulatorHostname()
		{
			String sSimHostName = "";
			Verbose(@"llGetSimulatorHostname()=""{0}""", sSimHostName);
			return sSimHostName;
		}

		public integer llGetStartParameter()
		{
			Verbose("llGetStartParameter()={0}" + iStartParameter);
			return iStartParameter;
		}

		public list llGetStaticPath(vector vStart, vector vEnd, Float fRadius, list lParameters)
		{
			list lReturn = new list();
			Verbose("llGetStaticPath({0}, {1}, {2}, {3})={4}", vStart, vEnd, fRadius, lParameters.ToVerboseString(), lReturn.ToVerboseString());
			return lReturn;
		}

		public integer llGetStatus(integer iRequestedStatus)
		{
			integer iStatus = 0;
			Verbose("llGetStatus({0})={1}", iRequestedStatus, iStatus);
			return iStatus;
		}

		public String llGetSubString(String sSource, integer iStart, integer iEnd)
		{
			string src = sSource;
			int start = iStart;
			int end = iEnd;

			StringBuilder sResult = new StringBuilder();

			int intLength = src.Length;

			if (CorrectIt(intLength, ref start, ref end)) {
				if (start <= end) {
					for (int intI = start; intI <= end; intI++) {
						sResult.Append(src[intI]);
					}
				} else { // excluding
					for (int intI = 0; intI <= end; intI++) {
						sResult.Append(src[intI]);
					}
					for (int intI = start; intI < intLength; intI++) {
						sResult.Append(src[intI]);
					}
				}
			}
			Verbose(string.Format(@"GetSubString(""{0}"", {1}, {2})=""{3}""", sSource, iStart, iEnd, sResult));
			return sResult.ToString();
		}

		public vector llGetSunDirection()
		{
			vector vSunDirection = vector.ZERO_VECTOR;
			Verbose("llGetSunDirection()={0}", vSunDirection);
			return vSunDirection;
		}

		public String llGetTexture(integer iFace)
		{
			String sTexture = "";
			Verbose(@"llGetTexture({0})=""{1}""", iFace, sTexture);
			return sTexture;
		}

		public vector llGetTextureOffset(integer iFace)
		{
			vector vOffset = vector.ZERO_VECTOR;
			Verbose("llGetTextureOffset({0})={1}", iFace, vOffset);
			return vOffset;
		}

		public Float llGetTextureRot(integer iFace)
		{
			Float fTextureRot = 0.0;
			Verbose("llGetTextureRot({0})={1}", iFace, fTextureRot);
			return fTextureRot;
		}

		public vector llGetTextureScale(integer iFace)
		{
			vector vScale = vector.ZERO_VECTOR;
			Verbose("llGetTextureScale({0})={1}", iFace, vScale);
			return vScale;
		}

		public Float llGetTime()
		{
			TimeSpan span = DateTime.Now.ToUniversalTime() - dtDateTimeScriptStarted;
			Verbose("llGetTime()={0}", span.TotalSeconds);
			return span.TotalSeconds;
		}

		public Float llGetTimeOfDay()
		{
			// dummy
			Float fSeconds = llGetTime();
			Verbose("llGetTimeOfDay()={0}", fSeconds);
			return fSeconds;
		}

		public string llGetTimestamp()
		{
			string sTimestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ", System.Globalization.DateTimeFormatInfo.InvariantInfo);
			Verbose(@"llGetTimestamp()=""{0}""", sTimestamp);
			return sTimestamp;
		}

		public vector llGetTorque()
		{
			vector vTorque = vector.ZERO_VECTOR;
			Verbose("llGetTorque()={0}", vTorque);
			return vTorque;
		}

		public integer llGetUnixTime()
		{
			DateTime dtUnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
			TimeSpan tsSeconds = DateTime.Now.ToUniversalTime() - dtUnixEpoch;
			Verbose("llGetUnixTime()={0}", (int)tsSeconds.TotalSeconds);
			return (int)tsSeconds.TotalSeconds;
		}

		public integer llGetUsedMemory()
		{
			integer iUsedMemory = 65536;
			Verbose("llGetUsedMemory()={0}", iUsedMemory);
			return iUsedMemory;
		}

		public string llGetUsername(key kAvatarID)
		{
			// TODO Find a dummy username.
			string sUserName = "";
			Verbose(@"llGetUsername({0})=""{1}""", kAvatarID, sUserName);
			return sUserName;
		}

		public vector llGetVel()
		{
			vector vVelocity = vector.ZERO_VECTOR;
			Verbose("llGetVel()={0}", vVelocity);
			return vVelocity;
		}

		public Float llGetWallclock()
		{
			Float fSeconds = (int)DateTime.Now.AddHours(-9.0).TimeOfDay.TotalSeconds;
			Verbose("llGetWallclock()={0}", fSeconds);
			return fSeconds;
		}

		public void llGiveInventory(key kID, String sItemName)
		{
			Verbose(@"llGiveInventory({0}, ""{1}"")", kID, sItemName);
		}

		public void llGiveInventoryList(key kID, String sDirectory, list lInventory)
		{
			Verbose(@"llGiveInventoryList({0}, ""{1}"", {2})", kID, sDirectory, lInventory.ToVerboseString());
		}

		public integer llGiveMoney(key kAvatarID, integer iAmount)
		{
			Verbose("llGiveMoney({0}, {1})=0", kAvatarID, iAmount);
			return 0;
		}

		public Float llGround(vector vOffset)
		{
			Float fHeight = 25.0;
			Verbose("llGround({0})={1}", vOffset, fHeight);
			return fHeight;
		}

		public vector llGroundContour(vector vOffset)
		{
			vector vContour = vector.ZERO_VECTOR;
			Verbose("llGroundContour({0})={1}", vOffset, vContour);
			return vContour;
		}

		public vector llGroundNormal(vector vOffset)
		{
			vector vGroundNormal = new vector(0.0, 0.0, 1.0);
			Verbose("llGroundNormal({0})={1}", vOffset, vGroundNormal);
			return vGroundNormal;
		}

		public void llGroundRepel(Float fHeight, integer iWater, Float fTau)
		{
			Verbose("llGroundRepel({0}, {1}, {2})", fHeight, iWater, fTau);
		}

		public vector llGroundSlope(vector vOffset)
		{
			vector vSlope = vector.ZERO_VECTOR;
			Verbose("llGroundSlope({0})={1}" + vOffset, vSlope);
			return vSlope;
		}

		public key llHTTPRequest(String sURL, list lParameters, String sBody)
		{
			key kRequestID = host.Http(sURL, lParameters, sBody);
			Verbose(@"llHTTPRequest(""{0}"", {1}, ""{2}"")=""{3}""", sURL, lParameters.ToVerboseString(), sBody, kRequestID);
			return kRequestID;
		}

		public void llHTTPResponse(key kRequestID, integer iStatus, String sBody)
		{
			Verbose(@"llHTTPResponse({0}, {1}, ""{2}"")", kRequestID, iStatus, sBody);
		}

		public String llInsertString(String sDestination, integer iIndex, String sSource)
		{
			string strDestination = sDestination;
			string strSource = sSource;
			int intPosition = iIndex;
			string sResult;

			if (intPosition < strDestination.Length) {
				sResult = strDestination.Substring(0, intPosition) + strSource + strDestination.Substring(intPosition);
			} else if (intPosition >= 0) {
				sResult = strDestination + strSource;
			} else {
				sResult = "**ERROR**";
			}
			Verbose(@"llInsertString(""{0}"", {1}, ""{2}"")=""{3}""", sDestination, iIndex, sSource, sResult);
			return sResult;
		}

		public void llInstantMessage(key kAvatarID, String sMessage)
		{
			Verbose(@"llInstantMessage({0}, ""{1}"")", kAvatarID, sMessage);
		}

		public String llIntegerToBase64(integer iNumber)
		{
			byte[] data = new byte[4];
			data[3] = (byte)(iNumber & 0xff);
			data[2] = (byte)((iNumber >> 8) & 0xff);
			data[1] = (byte)((iNumber >> 16) & 0xff);
			data[0] = (byte)((iNumber >> 24) & 0xff);
			string sResult = Convert.ToBase64String(data);
			Verbose(@"llIntegerToBase64({0})=""{1}""", iNumber, sResult);
			return sResult;
		}

		public list llJson2List(string sJSON)
		{
			// TODO implement conversion to list
			list lJSON = new list();
			Verbose("llJson2List({0})={1}", sJSON, lJSON);
			return lJSON;
		}

		public string llJsonGetValue(string sJSON, list lSpecifiers)
		{
			// TODO determine return value from list
			string sReturn = JSON_INVALID;
			Verbose("llJsonGetValue({0}, {1})= {2}", sJSON, lSpecifiers, sReturn);
			return sReturn;
		}

		public string llJsonSetValue(string sJSON, list lSpecifiers, string sValue)
		{
			// TODO determine return value
			string sReturn = JSON_INVALID;
			Verbose("llJsonGetValue({0}, {1}, {2})= {3}", sJSON, lSpecifiers, sValue, sReturn);
			return sReturn;
		}

		public string llJsonValueType(string sJSON, list lSpecifiers)
		{
			// TODO determine return value
			string sReturn = JSON_INVALID;
			Verbose("llJsonGetValue({0}, {1})= {2}", sJSON, lSpecifiers, sReturn);
			return sReturn;
		}

		public String llKey2Name(key kID)
		{
			string sAvatarName = "";
			if (Properties.Settings.Default.AvatarKey == kID) {
				sAvatarName = Properties.Settings.Default.AvatarName;
			}
			Verbose(@"llKey2Name({0})=""{1}""", kID, sAvatarName);
			return sAvatarName;
		}

		public void llLinkParticleSystem(integer iLink, list lParameters)
		{
			Verbose("llLinkParticleSystem({0}, {1})", iLink, lParameters.ToVerboseString());
		}

		public void llLinkSitTarget(integer iLinkIndex, vector vOffset, rotation rRotation)
		{
			Verbose("llLinkSitTarget({0}, {1}, {2})", iLinkIndex, vOffset, rRotation);
		}

		public String llList2CSV(list lSource)
		{
			StringBuilder sCSV = new StringBuilder();
			for (int intI = 0; intI < lSource.Count; intI++) {
				if (intI > 0) {
					sCSV.Append(", ");
				}
				sCSV.Append(lSource[intI].ToString());
			}
			Verbose(@"llList2CSV({0})=""{1}""", lSource.ToVerboseString(), sCSV.ToString());
			return sCSV.ToString();
		}

		public Float llList2Float(list lSource, integer iIndex)
		{
			Float fResult;
			if (iIndex < 0) {
				iIndex = lSource.Count + iIndex;
			}
			if (iIndex >= lSource.Count || iIndex < 0) {
				fResult = 0.0;
			} else {
				fResult = (Float)lSource[iIndex].ToString();
			}
			Verbose("llList2Float({0}, {1})={2}", lSource.ToVerboseString(), iIndex, fResult);
			return fResult;
		}

		public integer llList2Integer(list lSrc, integer iIndex)
		{
			integer iResult;
			if (iIndex < 0) {
				iIndex = lSrc.Count + iIndex;
			}
			if (iIndex >= lSrc.Count || iIndex < 0) {
				iResult = 0;
			} else {
				iResult = (integer)lSrc[iIndex].ToString();
			}
			Verbose("llList2Integer({0}, {1})={2}", lSrc.ToVerboseString(), iIndex, iResult);
			return iResult;
		}

		public string llList2Json(string sType, list lValues)
		{
			// TODO determine return value
			string sReturn = JSON_INVALID;
			Verbose(@"llList2Json({0}, {1})=""{2}""", sType, lValues.ToVerboseString(), sReturn);
			return sReturn;
		}

		public key llList2Key(list lSource, integer iIndex)
		{
			key kResult;
			if (iIndex < 0) { 
				iIndex = lSource.Count + iIndex;
			}
			if (iIndex >= lSource.Count || iIndex < 0) {
				kResult = key.NULL_KEY;
			} else {
				kResult = (key)lSource[iIndex].ToString();
			}
			Verbose("llList2Key({0}, {1})={2}", lSource.ToVerboseString(), iIndex, kResult);
			return kResult;
		}

		public list llList2List(list lSource, integer iStart, integer iEnd)
		{
			int iLength = lSource.Count;

			int start = iStart;
			int end = iEnd;

			list lResult = new list();

			if (CorrectIt(iLength, ref start, ref end)) {
				if (start <= end) {
					for (int intI = start; intI <= end; intI++) {
						lResult.Add(lSource[intI]);
					}
				} else { // excluding
					for (int intI = 0; intI <= end; intI++) {
						lResult.Add(lSource[intI]);
					}
					for (int intI = start; intI < iLength; intI++) {
						lResult.Add(lSource[intI]);
					}
				}
			}

			Verbose(string.Format("List2List({0}, {1}, {2})={3}", lSource.ToVerboseString(), iStart, iEnd, lResult.ToVerboseString()));
			return lResult;
		}

		public list llList2ListStrided(list lSource, integer iStart, integer iEnd, integer iStride)
		{
			int iLength = lSource.Count;

			int intStart = iStart;
			int intEnd = iEnd;

			list lTemp = new list();

			if (CorrectIt(iLength, ref intStart, ref intEnd)) {
				if (intStart <= intEnd) {
					for (int intI = intStart; intI <= intEnd; intI++) {
						lTemp.Add(lSource[intI]);
					}
				} else {	// excluding
					for (int intI = 0; intI <= intEnd; intI++) {
						lTemp.Add(lSource[intI]);
					}
					for (int intI = intStart; intI < iLength; intI++) {
						lTemp.Add(lSource[intI]);
					}
				}
			}
			list lResult = new list();
			string sRemark = "";
			if (iStride <= 0) {
				sRemark = " ** stride must be > 0 **";
			} else {
				if (intStart == 0) {
					for (int intI = 0; intI < lTemp.Count; intI += iStride) {
						lResult.Add(lTemp[intI]);
					}
				} else {
					for (int intI = iStride - 1; intI < lTemp.Count; intI += iStride) {
						lResult.Add(lTemp[intI]);
					}
				}
			}
			Verbose(@"llList2ListStrided({0}, {1}, {2}, {3})={4}{5}", lSource.ToVerboseString(), intStart, intEnd, iStride, lResult.ToVerboseString(), sRemark);
			return lResult;
		}

		public rotation llList2Rot(list lSource, integer iIndex)
		{
			rotation rResult;
			if (iIndex < 0) {
				iIndex = lSource.Count + iIndex;
			}
			if (iIndex >= lSource.Count || iIndex < 0) {
				rResult = ZERO_ROTATION;
			} else {
				if (lSource[iIndex] is rotation) {
					rResult = (rotation)lSource[iIndex];
				} else {
					rResult = rotation.ZERO_ROTATION;
				}
			}
			Verbose("llList2Rot({0}, {1})={2}", lSource.ToVerboseString(), iIndex, rResult);
			return rResult;
		}

		public String llList2String(list lSource, integer iIndex)
		{
			String sResult;
			if (iIndex < 0) {
				iIndex = lSource.Count + iIndex;
			}
			if (iIndex >= lSource.Count || iIndex < 0) {
				sResult = "";
			} else {
				sResult = (String)lSource[iIndex].ToString();
			}
			Verbose(@"llList2String({0}, {1})=""{2}""", lSource.ToVerboseString(), iIndex, sResult);
			return sResult;
		}

		public vector llList2Vector(list lSource, integer iIndex)
		{
			vector vResult;
			if (iIndex < 0) {
				iIndex = lSource.Count + iIndex;
			}
			if (iIndex >= lSource.Count || iIndex < 0) {
				vResult = ZERO_VECTOR;
			} else {
				if (lSource[iIndex] is vector) {
					vResult = (vector)lSource[iIndex];
				} else {
					vResult = vector.ZERO_VECTOR;
				}
			}
			Verbose("llList2Vector({0}, {1})={2}", lSource.ToVerboseString(), iIndex, vResult);
			return vResult;
		}

		public integer llListFindList(list lSource, list lMatch)
		{
			if (lSource.Count == 0) {
				return -1;
			}
			if (lMatch.Count == 0) {
				return 0;
			}
			if (lMatch.Count > lSource.Count) {
				return -1;
			}

			int iReturn = -1;
			for (int intI = 0; intI <= (lSource.Count - lMatch.Count); intI++) {
				if (lMatch[0].Equals(lSource[intI])) {
					bool blnOkay = true;
					for (int intJ = 1; intJ < lMatch.Count; intJ++) {
						if (!lMatch[intJ].Equals(lSource[intI + intJ])) {
							blnOkay = false;
							break;
						}
					}
					if (blnOkay) {
						iReturn = intI;
						break;
					}
				}
			}
			Verbose("llListFindList({0}, {1}={2}", lSource.ToVerboseString(), lMatch.ToVerboseString(), iReturn);
			return iReturn;
		}

		public list llListInsertList(list lDestination, list lSource, integer iIndex)
		{
			int intLength = lDestination.Count;
			list lResult = new list();
			if (iIndex < 0) {
				iIndex = lDestination.Count + iIndex;
			}

			for (int intI = 0; intI < Math.Min(iIndex, intLength); intI++) {
				lResult.Add(lDestination[intI]);
			}

			lResult.AddRange(lSource);

			for (int intI = Math.Max(0, iIndex); intI < intLength; intI++) {
				lResult.Add(lDestination[intI]);
			}

			Verbose("llListInsertList({0}, {1}, {2})={3}", lDestination.ToVerboseString(), lSource.ToVerboseString(), iIndex, lResult.ToVerboseString());
			return lResult;
		}

		public list llListRandomize(list lSource, integer iStride)
		{
			list lResult;
			ArrayList buckets = List2Buckets(lSource, iStride);
			if (buckets == null) {
				lResult = new list(lSource);
			} else {
				lResult = Buckets2List(RandomShuffle(buckets), iStride);
			}
			Verbose("llListRandomize({0}, {1})={2}", lSource.ToVerboseString(), iStride, lResult.ToVerboseString());
			return lResult;
		}

		// TODO check this!!!
		public list llListReplaceList(list lDestination, list lSource, integer iStart, integer iEnd)
		{
			int intLength = lDestination.Count;

			int intStart = iStart;
			int intEnd = iEnd;
			CorrectIt(intLength, ref intStart, ref intEnd);

			list lResult = new list();
			if (intStart <= intEnd) {
				for (int intI = 0; intI < intStart; intI++) {
					lResult.Add(lDestination[intI]);
				}
				lResult.AddRange(lSource);
				for (int intI = intEnd + 1; intI < intLength; intI++) {
					lResult.Add(lDestination[intI]);
				}
			} else {
				// where to add src?????
				for (int intI = intEnd; intI <= intStart; intI++) {
					lResult.Add(lDestination[intI]);
				}
			}
			Verbose("llListReplaceList({0}, {1}, {2}, {3}={4}", lDestination.ToVerboseString(), lSource.ToVerboseString(), intStart, intEnd, lResult.ToVerboseString());
			return lResult;
		}

		public list llListSort(list lSource, integer iStride, integer iAscending)
		{
			int intAscending = iAscending;
			int intStride = iStride;
			list lResult;
			ArrayList buckets = List2Buckets(lSource, intStride);
			if (buckets == null) {
				lResult = new list(lSource);
			} else {
				buckets.Sort(new BucketComparer(intAscending));
				lResult = Buckets2List(buckets, intStride);
			}
			Verbose("llListSort({0}, {1}, {2})={3}", lSource.ToVerboseString(), iStride, iAscending, lResult.ToVerboseString());
			return lResult;
		}

		/*
0 LIST_STAT_RANGE  Returns the range.
1 LIST_STAT_MIN  Retrieves the smallest number.
2 LIST_STAT_MAX  Retrieves the largest number.
3 LIST_STAT_MEAN  Retrieves the mean (average).
4 LIST_STAT_MEDIAN  Retrieves the median number.
5 LIST_STAT_STD_DEV  Calculates the standard deviation.
6 LIST_STAT_SUM  Calculates the sum.
7 LIST_STAT_SUM_SQUARES  Calculates the sum of the squares.
8 LIST_STAT_NUM_COUNT  Retrieves the amount of float and integer elements. Theoretically similar to llGetListLength, except LL states that this function ignores all non-integer and non-float elements. Therefore, this is a useful tool for mixed lists of numbers and non-numbers.
9 LIST_STAT_GEOMETRIC_MEAN  Calculates the geometric mean. All numbers must be greater than zero for this to work, according to LL.
		 */
		public Float llListStatistics(integer iOperation, list lInput)
		{
			double dResult = 0.0;
			double rmin, rmax;
			int operation = iOperation;
			List<double> input = GetListOfNumbers(lInput);
			if (input.Count > 0) {
				switch (operation) {
					case LIST_STAT_RANGE:
						rmin = double.MaxValue;
						rmax = double.MinValue;
						for (int intI = 0; intI < input.Count; intI++) {
							if (input[intI] < rmin) {
								rmin = input[intI];
							}
							if (input[intI] > rmax) {
								rmax = input[intI];
							}
						}
						dResult = rmax - rmin;
						break;
					case LIST_STAT_MIN:
						dResult = double.MaxValue;
						for (int intI = 0; intI < input.Count; intI++) {
							if (input[intI] < dResult) {
								dResult = input[intI];
							}
						}
						break;
					case LIST_STAT_MAX:
						dResult = double.MinValue;
						for (int intI = 0; intI < input.Count; intI++) {
							if (input[intI] > dResult) {
								dResult = input[intI];
							}
						}
						break;
					case LIST_STAT_MEAN:
						for (int intI = 0; intI < input.Count; intI++) {
							dResult += input[intI];
						}
						dResult = dResult / input.Count;
						break;
					case LIST_STAT_MEDIAN:
						input.Sort();
						if (Math.Ceiling(input.Count * 0.5) == input.Count * 0.5) {
							dResult = input[(int)((input.Count * 0.5) - 1)] + (input[(int)(input.Count * 0.5)] / 2);
						} else {
							dResult = input[((int)(Math.Ceiling(input.Count * 0.5))) - 1];
						}
						break;
					case LIST_STAT_STD_DEV:
						dResult = GetStandardDeviation(input.ToArray());
						break;
					case LIST_STAT_SUM:
						for (int intI = 0; intI < input.Count; intI++) {
							dResult += input[intI];
						}
						break;
					case LIST_STAT_SUM_SQUARES:
						for (int intI = 0; intI < input.Count; intI++) {
							dResult += input[intI] * input[intI];
						}
						////double av = GetAverage(input.ToArray());
						////for (int intI = 0; intI < input.Count; intI++)
						//	result += (av - input[intI]) * (av - input[intI]);
						break;
					case LIST_STAT_NUM_COUNT:
						dResult = input.Count;
						break;
					case LIST_STAT_GEOMETRIC_MEAN:
						for (int intI = 0; intI < input.Count; intI++) {
							input[intI] = Math.Log(input[intI]);
						}
						dResult = Math.Exp(GetAverage(input.ToArray()));
						break;
					default:
						break;
				}
			}
			Verbose("llListStatistics({0}, {1})={2}", iOperation, lInput.ToVerboseString(), dResult);
			return dResult;
		}

		public integer llListen(integer iChannel, String sName, key kID, String sText)
		{
			int intHandle = host.llListen(iChannel, sName, kID, sText);
			Verbose(@"llListen({0}, ""{1}"", {2}, {3})={4}", iChannel, sName, kID, sText, intHandle);
			return intHandle;
		}

		public void llListenControl(integer iHandle, integer iActive)
		{
			Verbose("llListenControl({0}, {1})", iHandle, iActive);
			host.llListenControl(iHandle, iActive);
		}

		public void llListenRemove(integer iHandle)
		{
			Verbose("llListenRemove({0})", iHandle);
			host.llListenRemove(iHandle);
		}

		public void llLoadURL(key kAvatarID, String sText, String sURL)
		{
			Verbose(@"llLoadURL({0}, ""{1}"", ""{2}"")", kAvatarID, sText, sURL);
			string strUrl = sURL.ToString();
			if (strUrl.StartsWith("http://")) {
				System.Diagnostics.Process.Start(strUrl);
			}
		}

		public Float llLog(Float fValue)
		{
			double dblA = 0.0;
			if (fValue > 0.0) {
				dblA = Math.Log(fValue);
			}
			Verbose("llLog({0})={1}", fValue, dblA);
			return dblA;
		}

		public Float llLog10(Float fValue)
		{
			double dblA = 0.0;
			if (fValue > 0.0) {
				dblA = Math.Log10(fValue);
			}
			Verbose("llLog10({0})={1}", fValue, dblA);
			return dblA;
		}

		public void llLookAt(vector vTarget, Float fStrength, Float fDamping)
		{
			Verbose("llLookAt({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", vTarget, fStrength, fDamping);
		}

		public void llLoopSound(String sSound, Float sVolume)
		{
			try {
				System.Media.SoundPlayer sp = host.GetSoundPlayer(sSound);
				sp.PlayLooping();
			} catch {
			}
			Verbose("llLoopSound({0}, {1})", sSound, sVolume);
		}

		public void llLoopSoundMaster(String sSound, Float fVolume)
		{
			try {
				System.Media.SoundPlayer sp = host.GetSoundPlayer(sSound);
				sp.PlayLooping();
			} catch {
			}
			Verbose("llLoopSoundMaster({0}, {1})", sSound, fVolume);
		}

		public void llLoopSoundSlave(String sSound, Float fVolume)
		{
			try {
				System.Media.SoundPlayer sp = host.GetSoundPlayer(sSound);
				sp.PlayLooping();
			} catch {
			}
			Verbose("llLoopSoundSlave({0}, {1})", sSound, fVolume);
		}

		// ok
		public String llMD5String(String sSource, integer iNonce)
		{
			System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
			byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(sSource + ":" + iNonce.ToString()));
			StringBuilder sbResult = new StringBuilder();
			foreach (byte hex in hash) {
				sbResult.Append(hex.ToString("x2"));						// convert to standard MD5 form
			}
			Verbose("llMD5String({0}, {1})={2}", sSource, iNonce, sbResult);
			return sbResult.ToString();
		}

		public void llMakeExplosion(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
		{
			Verbose(@"llMakeExplosion({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
		}

		public void llMakeFire(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
		{
			Verbose(@"llMakeFire({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
		}

		public void llMakeFountain(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
		{
			Verbose(@"llMakeFountain({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
		}

		public void llMakeSmoke(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
		{
			Verbose(@"llMakeSmoke({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
		}

		public void llManageEstateAccess(integer iAction, key kID)
		{
			Verbose("llManageEstateAccess({0}, {1}", iAction, kID);
		}

		public void llMapDestination(String sLandmark, vector vPosition, vector vLookat)
		{
			Verbose(@"llMapDestination({0}, {1}, {2})", sLandmark, vPosition, vLookat);
		}

		public void llMessageLinked(integer iLink, integer iValue, String sText, key kID)
		{
			Verbose(@"llMessageLinked({0}, {1}, ""{2}"", {3})", iLink, iValue, sText, kID);
			host.MessageLinked(iLink, iValue, sText, kID);
		}

		public void llMinEventDelay(Float fDelay)
		{
			Verbose("llMinEventDelay({0}", fDelay);
		}

		public integer llModPow(integer iValue1, integer iValue2, integer iModulus)
		{
			integer iResult = ModPow2(iValue1, iValue2, iModulus);
			Verbose("llModPow({0}, {1}, {2})={3}", iValue1, iValue2, iModulus, iResult);
			return iResult;
		}

		public void llModifyLand(integer iAction, integer iSize)
		{
			Verbose("llModifyLand({0}, {1})", iAction, iSize);
		}

		public void llMoveToTarget(vector vTarget, Float fTau)
		{
			Verbose("llMoveToTarget({0}, {1})", vTarget, fTau);
		}

		public void llNavigateTo(vector vLocation, list lOptions)
		{
			Verbose("llNavigateTo({0}, {1})", vLocation, lOptions);
		}

		public void llOffsetTexture(Float fOffsetS, Float fOffsetT, integer iFace)
		{
			Verbose("llOffsetTexture({0}, {1}, {2})", fOffsetS, fOffsetT, iFace);
		}

		public void llOpenRemoteDataChannel()
		{
			host.llOpenRemoteDataChannel();
			Verbose("llOpenRemoteDataChannel()");
		}

		public integer llOverMyLand(key kID)
		{
			integer iResult = integer.TRUE;
			Verbose("llOverMyLand({0})={1}", kID, iResult);
			return iResult;
		}

		public void llOwnerSay(String sText)
		{
			Chat(0, sText, CommunicationType.OwnerSay);
		}

		public void llParcelMediaCommandList(list lCommands)
		{
			Verbose("llParcelMediaCommandList({0})", lCommands.ToVerboseString());
		}

		public list llParcelMediaQuery(list lQuery)
		{
			list lResult = new list();
			Verbose("llParcelMediaQuery({0})={1})", lQuery.ToVerboseString(), lResult.ToVerboseString());
			return lResult;
		}

		// 21 sep 2007, check this
		public list llParseString2List(String sSource, list lSseparators, list lSpacers)
		{
			list lResult = ParseString(sSource, lSseparators, lSpacers, false);
			Verbose(@"llParseString2List(""{0}"", {1}, {2})={3}", sSource, lSseparators.ToVerboseString(), lSpacers.ToVerboseString(), lResult.ToVerboseString());
			return lResult;
		}

		// 21 sep 2007, check this, first check 3 oct 2007, last element=="" is added also
		public list llParseStringKeepNulls(String sSource, list lSeparators, list lSpacers)
		{
			list lResult = ParseString(sSource, lSeparators, lSpacers, true);
			Verbose(@"llParseStringKeepNulls(""{0}"", {1}, {2})={3}", sSource, lSeparators.ToVerboseString(), lSpacers.ToVerboseString(), lResult.ToVerboseString());
			return lResult;
		}

		public void llParticleSystem(list lParameters)
		{
			Verbose("llParticleSystem({0})", lParameters.ToVerboseString());
		}

		public void llPassCollisions(integer iPassFlag)
		{
			Verbose("llPassCollisions({0})", iPassFlag);
		}

		public void llPassTouches(integer iPassFlag)
		{
			Verbose("llPassTouches({0})", iPassFlag);
		}

		public void llPatrolPoints(list lPoints, list lOptions)
		{
			Verbose("llPatrolPoints({0}, {1})", lPoints, lOptions);
		}

		public void llPlaySound(String sSoundName, Float fVolume)
		{
			try {
				System.Media.SoundPlayer sp = host.GetSoundPlayer(sSoundName);
				sp.Play();
				Verbose("llPlaySound({0}, {1})", sSoundName, fVolume);
			} catch (Exception exception) {
				Verbose("llPlaySound({0}, {1}) **{2}", sSoundName, fVolume, exception.Message);
			}
		}

		public void llPlaySoundSlave(String sSoundName, Float fVolume)
		{
			try {
				System.Media.SoundPlayer sp = host.GetSoundPlayer(sSoundName);
				sp.Play();
			} catch {
			}
			Verbose("llPlaySoundSlave({0}, {1})", sSoundName, fVolume);
		}

		public void llPointAt(vector vTarget)
		{
			Verbose("llPointAt({0})", vTarget);
		}

		public Float llPow(Float fBase, Float fExponent)
		{
			double dblA = Math.Pow(fBase, fExponent);
			Verbose("llPow({0}, {1})={2}", fBase, fExponent, dblA);
			return dblA;
		}

		public void llPreloadSound(String sSoundName)
		{
			Verbose("llPreloadSound({0})", sSoundName);
		}

		public void llPursue(key kTargetID, list lOptions)
		{
			Verbose("llPursue({0}, {1})", kTargetID, lOptions);
		}

		public void llPushObject(key kID, vector vImpulse, vector vAngularImpulse, integer iLocalFlag)
		{
			Verbose("llPushObject({0}, {1}, {2}, {3})", kID, vImpulse, vAngularImpulse, iLocalFlag);
		}

		public void llRegionSay(integer iChannel, String sText)
		{
			if (iChannel != 0) {
				Chat(iChannel, sText, CommunicationType.RegionSay);
			}
			Verbose(@"llRegionSay({0}, ""{1}"")", iChannel, sText);
		}

		public void llRegionSayTo(key kTargetID, integer iChannel, String sText)
		{
			Verbose(@"llRegionSayTo({0}, {1}, ""{2}"")", kTargetID, iChannel, sText);
		}

		public void llReleaseCamera(key kID)
		{
			Verbose("llReleaseCamera({0})", kID);
		}

		public void llReleaseControls()
		{
			Verbose("llReleaseControls()");
			this.host.ReleaseControls();
		}

		public void llReleaseControls(key kAvatarID)
		{
			Verbose("llReleaseControls({0})", kAvatarID);
		}

		public void llReleaseURL(string sURL)
		{
			Verbose(@"llReleaseURL(""{0}"")", sURL);
		}

		public void llRemoteDataReply(key kChannel, key kMessageID, String sData, integer iData)
		{
			host.llRemoteDataReply(kChannel, kMessageID, sData, iData);
			Verbose("llRemoteDataReply({0}, {1}, {2}, {3})", kChannel, kMessageID, sData, iData);
		}

		public void llRemoteDataSetRegion()
		{
			Verbose("llRemoteDataSetRegion()");
		}

		public void llRemoteLoadScript(key kTargetID, String sName, integer iRunningFlag, integer iStartParameter)
		{
			Verbose(@"llRemoteLoadScript({0}, ""{1}"", {2}, {3})", kTargetID, sName, iRunningFlag, iStartParameter);
		}

		public void llRemoteLoadScriptPin(key kTargetID, String sName, integer iPIN, integer iRunningFlag, integer iStartParameter)
		{
			Verbose(@"llRemoteLoadScriptPin({0}, ""{1}"", {2}, {3}, {4})", kTargetID, sName, iPIN, iRunningFlag, iStartParameter);
		}

		public void llRemoveFromLandBanList(key kAvatarID)
		{
			Verbose("llRemoveFromLandBanList({0})", kAvatarID);
			if (htLandBanList.ContainsKey(kAvatarID)) {
				htLandBanList.Remove(kAvatarID);
			}
		}

		public void llRemoveFromLandPassList(key kAvatarID)
		{
			Verbose("llRemoveFromLandPassList({0})", kAvatarID);
			if (htLandPassList.ContainsKey(kAvatarID)) {
				htLandPassList.Remove(kAvatarID);
			}
		}

		public void llRemoveInventory(String sInventoryItem)
		{
			host.RemoveInventory(sInventoryItem);
			Verbose("llRemoveInventory({0})", sInventoryItem);
		}

		public void llRemoveVehicleFlags(integer iFlags)
		{
			Verbose("llRemoveVehicleFlags({0})", iFlags);
		}

		public key llRequestAgentData(key kID, integer iDataConstant)
		{
			key kResult = new key(Guid.NewGuid());

			string strData = "***";
			switch ((int)iDataConstant) {
				case DATA_ONLINE:
					break;
				case DATA_NAME:
					strData = Properties.Settings.Default.AvatarName;
					break;
				case DATA_BORN:
					strData = DateTime.Now.ToString("yyyy-MM-dd");
					break;
				case DATA_RATING:
					break;
				case DATA_PAYINFO:
					break;
				default:
					break;
			}
			host.ExecuteSecondLife("dataserver", kResult, (SecondLife.String)strData);
			return kResult;
		}

		public key llRequestDisplayName(key kAvatarID)
		{
			key kID = new key(Guid.NewGuid());
			string strData = "dummyDisplay Name";
			host.ExecuteSecondLife("dataserver", kID, (SecondLife.String)strData);
			Verbose("llRequestDisplayName({0})={1}", kAvatarID, kID);
			return kID;
		}

		public key llRequestInventoryData(String sName)
		{
			key kResult = new key(Guid.NewGuid());
			Verbose(@"llRequestInventoryData(""{0}"")={1}", sName, kResult);
			return kResult;
		}

		public void llRequestPermissions(key kAvatarID, integer iPermissionFlags)
		{
			Verbose("llRequestPermissions({0}, {1})", kAvatarID, iPermissionFlags);
			this.host.llRequestPermissions(kAvatarID, iPermissionFlags);
		}

		public key llRequestSecureURL()
		{
			key kResult = new key(Guid.NewGuid());
			Verbose("llRequestPermissions()={0}", kResult);
			return kResult;
		}

		public key llRequestSimulatorData(String sSimulator, integer iData)
		{
			key kResult = new key(Guid.NewGuid());
			Verbose(@"llRequestSimulatorData(""{0}"", {1})={2}", sSimulator, iData, kResult);
			return kResult;
		}

		public key llRequestURL()
		{
			key kResult = new key(Guid.NewGuid());
			Verbose("llRequestURL()={0}", kResult);
			return kResult;
		}

		public key llRequestUsername(key kAvatarID)
		{
			key kID = new key(Guid.NewGuid());
			string strData = "dummyUser Name";
			Verbose("llRequestDisplayName({0})={1}", kAvatarID, kID);
			host.ExecuteSecondLife("dataserver", kID, (SecondLife.String)strData);
			return kID;
		}

		public void llResetAnimationOverride(String sAnimationState)
		{
			Verbose("llResetAnimationOverride({0})", sAnimationState);
		}

		public void llResetLandBanList()
		{
			htLandBanList = new Hashtable();
			Verbose("llResetLandBanList()");
		}

		public void llResetLandPassList()
		{
			htLandPassList = new Hashtable();
			Verbose("llResetLandPassList()");
		}

		public void llResetOtherScript(String sScriptName)
		{
			Verbose("llResetOtherScript({0})", sScriptName);
		}

		public void llResetScript()
		{
			Verbose("llResetScript()");
			host.Reset();
			System.Threading.Thread.Sleep(1000);
			System.Windows.Forms.MessageBox.Show("If you see this, something wrong in llResetScript()", "Oops...");
		}

		public void llResetTime()
		{
			Verbose("llResetTime()");
			dtDateTimeScriptStarted = DateTime.Now.ToUniversalTime();
		}

		public integer llReturnObjectsByID(list lObjects)
		{
			integer iReturned = ERR_GENERIC;
			Verbose("llReturnObjectsByID({0})={1}", lObjects, iReturned);
			return iReturned;
		}

		public integer llReturnObjectsByOwner(key kID, integer iScope)
		{
			integer iReturned = ERR_GENERIC;
			Verbose("llReturnObjectsByOwner({0}, {1})={2}", kID, iScope, iReturned);
			return iReturned;
		}

		public void llRezAtRoot(String sInventoryItem, vector vPosition, vector vVelocity, rotation rRotation, integer iParameter)
		{
			Verbose(@"llRezAtRoot(""{0}"", {1}, {2}, {3}, {4})", sInventoryItem, vPosition, vVelocity, rRotation, iParameter);
		}

		public void llRezObject(String sInventoryItem, vector vPosition, vector vVelocity, rotation rRotation, integer iParameter)
		{
			Verbose(@"llRezObject(""{0}"", {1}, {2}, {3}, {4})", sInventoryItem, vPosition, vVelocity, rRotation, iParameter);
			object_rez(new key(Guid.NewGuid()));
			on_rez(iParameter);
		}

		public Float llRot2Angle(rotation rRotation)
		{
			Float fAngle = 0.0F;
			Verbose("llRot2Angle({0})={1}", rRotation, fAngle);
			return fAngle;
		}

		public vector llRot2Axis(rotation rRotation)
		{
			vector vAxis = vector.ZERO_VECTOR;
			Verbose("llRot2Axis({0})={1})", rRotation, vAxis);
			return vAxis;
		}

		public vector llRot2Euler(rotation rRotation)
		{
			// http://rpgstats.com/wiki/index.php?title=LibraryRotationFunctions
			rotation t = new rotation(rRotation.x * rRotation.x, rRotation.y * rRotation.y, rRotation.z * rRotation.z, rRotation.s * rRotation.s);
			double m = t.x + t.y + t.z + t.s;
			vector vEuler = new vector(0, 0, 0);
			if (m != 0) {
				double n = 2 * ((rRotation.y * rRotation.s) + (rRotation.x * rRotation.z));
				double p = (m * m) - (n * n);
				if (p > 0) {
					vEuler = new vector(
						Math.Atan2(2.0 * ((rRotation.x * rRotation.s) - (rRotation.y * rRotation.z)), -t.x - t.y + t.z + t.s),
						Math.Atan2(n, Math.Sqrt(p)),
						Math.Atan2(2.0 * ((rRotation.z * rRotation.s) - (rRotation.x * rRotation.y)), t.x - t.y - t.z + t.s));
				} else if (n > 0) {
					vEuler = new vector(0, PI_BY_TWO, Math.Atan2((rRotation.z * rRotation.s) + (rRotation.x * rRotation.y), 0.5 - t.x - t.z));
				} else {
					vEuler = new vector(0, -PI_BY_TWO, Math.Atan2((rRotation.z * rRotation.s) + (rRotation.x * rRotation.y), 0.5 - t.x - t.z));
				}
			}
			Verbose("llRot2Euler({0})={1}", rRotation, vEuler);
			return vEuler;
		}

		public vector llRot2Fwd(rotation rRotation)
		{
			vector v = new vector(1.0 / ((rRotation.x * rRotation.x) + (rRotation.y * rRotation.y) + (rRotation.z * rRotation.z) + (rRotation.s * rRotation.s)), 0, 0);
			vector vResult = v * rRotation;
			Verbose("llRot2Fwd({0})={1}", rRotation, vResult);
			return vResult;
		}

		public vector llRot2Left(rotation rRotation)
		{
			vector v = new vector(0, 1.0 / ((rRotation.x * rRotation.x) + (rRotation.y * rRotation.y) + (rRotation.z * rRotation.z) + (rRotation.s * rRotation.s)), 0);
			vector vResult = v * rRotation;
			Verbose("llRot2Left({0})={1}", rRotation, vResult);
			return vResult;
		}

		public vector llRot2Up(rotation rRotation)
		{
			vector v = new vector(0, 0, 1.0 / ((rRotation.x * rRotation.x) + (rRotation.y * rRotation.y) + (rRotation.z * rRotation.z) + (rRotation.s * rRotation.s)));
			vector vResult = v * rRotation;
			Verbose("llRot2Left({0})={1}", rRotation, vResult);
			return vResult;
		}

		public void llRotateTexture(Float fRadians, integer iFace)
		{
			Verbose("llRotateTexture({0}, {1})", fRadians, iFace);
		}

		public rotation llRotBetween(vector vDirection1, vector vDirection2)
		{
			rotation rResult = rotation.ZERO_ROTATION;
			Verbose("llRotBetween({0}, {1})={2}", vDirection1, vDirection2, rResult);
			return rResult;
		}

		public void llRotLookAt(rotation rRotation, Float fStrength, Float fDamping)
		{
			Verbose("llRotLookAt({0}, {1}, {2})", rRotation, fStrength, fDamping);
		}

		public integer llRotTarget(rotation rRotation, Float fError)
		{
			integer iHandle = 0;
			Verbose("llRotTarget({0}, {1})={2})", rRotation, fError, iHandle);
			return iHandle;
		}

		public void llRotTargetRemove(integer iHandle)
		{
			Verbose("llRotTargetRemove({0})", iHandle);
		}

		public integer llRound(Float fValue)
		{
			int intA = (int)Math.Round(fValue);
			Verbose("llRound({0})={1})=", fValue, intA);
			return intA;
		}

		public integer llSameGroup(key kID)
		{
			integer iFlag = 0;
			Verbose("llSameGroup({0})={1})", kID, iFlag);
			return iFlag;
		}

		public void llSay(integer iChannel, String sText)
		{
			Chat(iChannel, sText, CommunicationType.Say);
		}

		public void llScaleTexture(Float fScaleS, Float fScaleT, integer iFace)
		{
			Verbose("llScaleTexture({0}, {1}, {2})", fScaleS, fScaleT, iFace);
		}

		public integer llScriptDanger(vector vPosition)
		{
			integer iFlag = 0;
			Verbose("llScriptDanger({0})={1}", vPosition, iFlag);
			return iFlag;
		}

		public void llScriptProfiler(integer iState)
		{
			Verbose("llScriptProfiler({0})", iState);
		}

		public key llSendRemoteData(key kChannel, String sDestination, integer iData, String sData)
		{
			key kID = host.llSendRemoteData(kChannel, sDestination, iData, sData);
			Verbose("llSendRemoteData({0}, {1}, {2}, {3})={4}", kChannel, sDestination, iData, sData, kID);
			return kID;
		}

		public void llSensor(String sName, key kID, integer iType, Float fRange, Float fArc)
		{
			Verbose(@"llSensor(""{0}"", {1}, {2}, {3}, {4})", sName, kID, iType, fRange, fArc);
			host.sensor_timer.Stop();
			integer iTotalNumber = 1;
			host.ExecuteSecondLife("sensor", iTotalNumber);
		}

		public void llSensorRemove()
		{
			Verbose("llSensorRemove()");
			host.sensor_timer.Stop();
		}

		public void llSensorRepeat(String sName, key kID, integer iType, Float fRange, Float fArc, Float fRate)
		{
			Verbose(@"llSensorRepeat(""{0}"", {1}, {2}, {3}, {4}, {5})", sName, kID, iType, fRange, fArc, fRate);
			host.sensor_timer.Stop();
			if (fRate > 0) {
				host.sensor_timer.Interval = (int)Math.Round(fRate * 1000);
				host.sensor_timer.Start();
			}
		}

		public void llSetAlpha(Float fOpacity, integer iFace)
		{
			Verbose("llSetAlpha({0}, {1})", fOpacity, iFace);
		}

		public void llSetAngularVelocity(vector vForce, integer iLocal)
		{
			Verbose("llSetAngularVelocity({0}, {1})", vForce, iLocal);
		}

		public void llSetAnimationOverride(String sAnimationState, String sAnimation)
		{
			Verbose("llSetAnimationOverride({0}, {1})", sAnimationState, sAnimation);
		}

		public void llSetBuoyancy(Float fBuoyancy)
		{
			Verbose("llSetBuoyancy({0})", fBuoyancy);
		}

		public void llSetCameraAtOffset(vector vOffset)
		{
			Verbose("llSetCameraAtOffset({0})", vOffset);
		}

		public void llSetCameraEyeOffset(vector vOffset)
		{
			Verbose("llSetCameraEyeOffset({0})", vOffset);
		}

		public void llSetCameraParams(list lRules)
		{
			Verbose("llSetCameraParams({0})", lRules.ToVerboseString());
		}

		public void llSetClickAction(integer iAction)
		{
			Verbose("llSetClickAction({0})", iAction);
		}

		public void llSetColor(vector vColour, integer iFace)
		{
			Verbose("llSetColor({0}, {1})", vColour, iFace);
		}

		public void llSetContentType(key kHTTPRequestID, integer iContentType)
		{
			Verbose("llSetContentType({0}, {1})", kHTTPRequestID, iContentType);
		}

		public void llSetDamage(Float fDamage)
		{
			Verbose("llSetDamage({0})", fDamage);
		}

		public void llSetForce(vector vForce, integer iLocal)
		{
			Verbose("llSetForce({0}, {1})", vForce, iLocal);
		}

		public void llSetForceAndTorque(vector vForce, vector vTorque, integer iLocal)
		{
			Verbose("llSetForceAndTorque({0}, {1}, {2})", vForce, vTorque, iLocal);
		}

		public void llSetHoverHeight(Float fHeight, Float fWater, Float fTau)
		{
			Verbose("llSetHoverHeight({0}, {1}, {2})", fHeight, fWater, fTau);
		}

		public void llSetInventoryPermMask(String sItem, integer iMask, integer iValue)
		{
			Verbose(@"llSetInventoryPermMask(""{0}"", {1}, {2})", sItem, iMask, iValue);
		}

		public void llSetKeyframedMotion(list lKeyframes, list lOptions)
		{
			Verbose("llSetKeyframedMotion({0}, {1})", lKeyframes.ToVerboseString(), lOptions.ToVerboseString());
		}

		public void llSetLinkAlpha(integer iLinkIndex, Float fAlpha, integer iFace)
		{
			Verbose("llSetLinkAlpha({0}, {1}, {2})", iLinkIndex, fAlpha, iFace);
		}

		public void llSetLinkCamera(integer iLinkIndex, vector vEyeOffset, vector vLookOffset)
		{
			Verbose("llSetLinkCamera({0}, {1}, {2})", iLinkIndex, vEyeOffset, vLookOffset);
		}

		public void llSetLinkColor(integer iLinkIndex, vector vColour, integer iFace)
		{
			Verbose("llSetLinkColor({0}, {1}, {2})", iLinkIndex, vColour, iFace);
		}

		public integer llSetLinkMedia(integer iLinkIndex, integer iFace, list lParameters)
		{
			integer iResult = STATUS_OK;
			Verbose("llSetLinkMedia({0}, {1}, {2})={3}", iLinkIndex, iFace, lParameters.ToVerboseString(), iResult);
			return iResult;
		}

		public void llSetLinkPrimitiveParams(integer iLinkIndex, list lRules)
		{
			Verbose("llSetLinkPrimitiveParams({0}, {1})", iLinkIndex, lRules.ToVerboseString());
		}

		public void llSetLinkPrimitiveParamsFast(integer iLinkIndex, list lRules)
		{
			Verbose("llSetLinkPrimitiveParamsFast({0}, {1})", iLinkIndex, lRules.ToVerboseString());
		}

		public void llSetLinkTexture(integer iLinkIndex, String sTexture, integer iFace)
		{
			Verbose(@"llSetLinkTexture({0}, ""{1}"", {2})", iLinkIndex, sTexture, iFace);
		}

		public void llSetLinkTextureAnim(integer iLinkIndex, integer iMode, integer iFace, integer iSizeX, integer iSizeY, Float fStart, Float fLength, Float fRate)
		{
			Verbose("llSetLinkTextureAnim({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", iLinkIndex, iMode, iFace, iSizeX, iSizeY, fStart, fLength, fRate);
		}

		public void llSetLocalRot(rotation rRotation)
		{
			this.rRotationlocal = rRotation;
			Verbose("llSetLocalRot({0})", rRotation);
		}

		public integer llSetMemoryLimit(integer iLimit)
		{
			integer iResult = true;
			Verbose("llSetMemoryLimit({0})=(1)", iLimit, iResult);
			return iResult;
		}

		public void llSetObjectDesc(String sDescription)
		{
			Verbose(@"llSetObjectDesc(""{0}"")", sDescription);
			host.SetObjectDescription(sDescription);
		}

		public void llSetObjectName(String sName)
		{
			Verbose(@"llSetObjectName(""{0}"")", sName);
			host.SetObjectName(sName);
		}

		public void llSetObjectPermMask(integer iMaskConstant, integer iValue)
		{
			Verbose("llSetObjectPermMask({0}, {1})", iMaskConstant, iValue);
		}

		public void llSetParcelMusicURL(String sURL)
		{
			Verbose(@"llSetParcelMusicURL(""{0}"")", sURL);
			sParcelMusicURL = sURL;
		}

		public void llSetPayPrice(integer iPrice, list lButtons)
		{
			Verbose("llSetPayPrice({0}, {1})", iPrice, lButtons.ToVerboseString());
		}

		public void llSetPhysicsMaterial(integer iMaterialBits, Float fGravityMultiplier, Float fRestitution, Float fFriction, Float fDensity)
		{
			Verbose("llSetPhysicsMaterial({0}, {1}, {2}, {3}, {4})", iMaterialBits, fGravityMultiplier, fRestitution, fFriction, fDensity);
		}

		public void llSetPos(vector vPosition)
		{
			Verbose("llSetPos({0})", vPosition);
			this.vPosition = vPosition;
		}

		public integer llSetPrimMediaParams(integer iFace, list lParameters)
		{
			integer iResult = 0;
			Verbose("llSetPrimMediaParams({0}, {1})={2}", iFace, lParameters.ToVerboseString(), iResult);
			return iResult;
		}

		public void llSetPrimitiveParams(list lRule)
		{
			Verbose("llSetPrimitiveParams({0})", lRule.ToVerboseString());
		}

		public integer llSetRegionPos(vector vPosition)
		{
			integer iResult = true;
			Verbose("llSetRegionPos({0})={1}", vPosition, iResult);
			this.vPosition = vPosition;
			return iResult;
		}

		public void llSetRemoteScriptAccessPin(integer iPIN)
		{
			Verbose("llSetRemoteScriptAccessPin({0})", iPIN);
		}

		public void llSetRot(rotation rRotation)
		{
			Verbose("llSetRot({0})", rRotation);
			this.rRotation = rRotation;
		}

		public void llSetScale(vector vScale)
		{
			Verbose("llSetScale({0})", vScale);
			this.vScale = vScale;
		}

		public void llSetScriptState(String sName, integer iRunState)
		{
			Verbose(@"llSetScriptState(""{0}"", {1})", sName, iRunState);
		}

		public void llSetSitText(String sText)
		{
			Verbose(@"llSetSitText(""{0}"")", sText);
			sSitText = sText;
		}

		public void llSetSoundQueueing(integer iQueueFlag)
		{
			Verbose("llSetSoundQueueing({0})", iQueueFlag);
		}

		public void llSetSoundRadius(Float fRadius)
		{
			fSoundRadius = fRadius;
			Verbose("llSetSoundRadius({0})", fRadius);
		}

		public void llSetStatus(integer iStatusConstant, integer iValue)
		{
			Verbose("llSetStatus({0}, {1})", iStatusConstant, iValue);
		}

		public void llSetText(String sText, vector vColour, Float fOpacity)
		{
			Verbose(@"llSetText(""{0}"", {1}, {2})", sText, vColour, fOpacity);
		}

		public void llSetTexture(String sTextureName, integer iFace)
		{
			Verbose(@"llSetTexture(""{0}"", {1})", sTextureName, iFace);
		}

		public void llSetTextureAnim(integer iMode, integer iFace, integer iSizeX, integer iSizeY, Float fStart, Float fLength, Float fRate)
		{
			Verbose("llSetTextureAnim({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", iMode, iFace, iSizeX, iSizeY, fStart, fLength, fRate);
		}

		public void llSetTimerEvent(Float fSeconds)
		{
			Verbose("llSetTimerEvent({0})", fSeconds);
			host.timer.Stop();
			if (fSeconds > 0) {
				host.timer.Interval = (int)Math.Round(fSeconds * 1000);
				host.timer.Start();
			}
		}

		public void llSetTorque(vector vTorque, integer iLocal)
		{
			Verbose("llSetTorque({0}, {1})", vTorque, iLocal);
		}

		public void llSetTouchText(String sText)
		{
			Verbose(@"llSetTouchText(""{0}"")", sText);
		}

		public void llSetVehicleFlags(integer iVehicleFlagConstants)
		{
			Verbose("llSetVehicleFlags({0})", iVehicleFlagConstants);
		}

		public void llSetVehicleFloatParam(integer iParameterName, Float fParameterValue)
		{
			Verbose("llSetVehicledoubleParam({0}, {1})", iParameterName, fParameterValue);
		}

		public void llSetVehicleRotationParam(integer iParameterName, rotation fParameterValue)
		{
			Verbose("llSetVehicleRotationParam({0}, {1})", iParameterName, fParameterValue);
		}

		public void llSetVehicleType(integer iTypeConstant)
		{
			Verbose("llSetVehicleType({0})", iTypeConstant);
		}

		public void llSetVehicleVectorParam(integer iParameterName, vector vParameterValue)
		{
			Verbose("llSetVehicleVectorParam({0}, {1})", iParameterName, vParameterValue);
		}

		public void llSetVelocity(vector vForce, integer iLocal)
		{
			Verbose("llSetVelocity({0}, {1})", vForce, iLocal);
		}

		public String llSHA1String(String sSource)
		{
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(sSource.ToString());
			System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
			string sHash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
			Verbose(@"llSHA1String(""{0"")=""{1}""", sSource, sHash);
			return sHash;
		}

		public void llShout(integer iChannel, String sText)
		{
			Chat(iChannel, sText, CommunicationType.Shout);
		}

		public Float llSin(Float fTheta)
		{
			double dblA = Math.Sin(fTheta);
			Verbose("llSin({0})={1}", fTheta, dblA);
			return dblA;
		}

		public void llSitTarget(vector vOffset, rotation rRotation)
		{
			Verbose("llSitTarget({0}, {1})", vOffset, rRotation);
		}

		public void llSleep(Float fSeconds)
		{
			Verbose("llSleep({0})", fSeconds);
			System.Threading.Thread.Sleep((int)Math.Round(fSeconds * 1000));
		}

		public Float llSqrt(Float fValue)
		{
			double dblA = Math.Sqrt(fValue);
			Verbose("llSqrt({0})={1}", fValue, dblA);
			return dblA;
		}

		public void llStartAnimation(String sAnimationName)
		{
			Verbose(@"llStartAnimation(""{0}"")", sAnimationName);
		}

		public void llStopAnimation(String sAnimationName)
		{
			Verbose(@"llStopAnimation(""{0}"")", sAnimationName);
		}

		public void llStopHover()
		{
			Verbose("llStopHover()");
		}

		public void llStopLookAt()
		{
			Verbose("llStopLookAt()");
		}

		public void llStopMoveToTarget()
		{
			Verbose("llStopMoveToTarget()");
		}

		public void llStopPointAt()
		{
			Verbose("llStopPointAt()");
		}

		public void llStopSound()
		{
			Verbose("llStopSound()");
		}

		public integer llStringLength(String sSource)
		{
			int intLength = ((string)sSource).Length;
			Verbose(@"llStringLength(""{0}"")={1}", sSource, intLength);
			return intLength;
		}

		public String llStringToBase64(String sText)
		{
			string sResult = StringToBase64(sText.ToString());
			Verbose(@"llStringToBase64(""{0}"")=""{1}""", sText, sResult);
			return sResult;
		}

		public String llStringTrim(String sText, integer iTrimType)
		{
			string strResult = sText.ToString();

			if ((iTrimType & STRING_TRIM_HEAD) != 0) {
				strResult = strResult.TrimStart();
			}

			if ((iTrimType & STRING_TRIM_TAIL) != 0) {
				strResult = strResult.TrimEnd();
			}

			Verbose(@"llStringTrim(""{0}"", {1})=""{2}""", sText, iTrimType, strResult);
			return strResult;
		}

		public integer llSubStringIndex(String sSource, String sPattern)
		{
			int intIndex = ((string)sSource).IndexOf((string)sPattern);
			Verbose("llSubStringIndex({0}, {1})={2}", sSource, sPattern, intIndex);
			return intIndex;
		}

		public void llTakeCamera(key kAvatarID)
		{
			Verbose("llTakeCamera({0})", kAvatarID);
		}

		public void llTakeControls(integer iControls, integer iAcceptFlag, integer iPassOnFlag)
		{
			Verbose("llTakeControls({0}, {1}), {2})", iControls, iAcceptFlag, iPassOnFlag);
			this.host.TakeControls(iControls, iAcceptFlag, iPassOnFlag);
		}

		public Float llTan(Float fTheta)
		{
			double dblA = Math.Tan(fTheta);
			Verbose("llTan({0})={1}", fTheta, dblA);
			return dblA;
		}

		public integer llTarget(vector vPosition, Float fRange)
		{
			integer iResult = 0;
			Verbose("llTarget({0}, {1})={2}", vPosition, fRange, iResult);
			return iResult;
		}

		public void llTargetOmega(vector vAxis, Float fSpinRate, Float fGain)
		{
			Verbose("llTargetOmega({0}, {1}, {2})", vAxis, fSpinRate, fGain);
		}

		public void llTargetRemove(integer iTargetHandle)
		{
			Verbose("llTargetRemove({0})", iTargetHandle);
		}

		public void llTeleportAgent(key kAvatarID, String sLandmarkName, vector vLandingPoint, vector vLookAtPoint)
		{
			Verbose(@"llTeleportAgentHome({0}, ""{1}"", {2}, {3})", kAvatarID, sLandmarkName, vLandingPoint, vLookAtPoint);
		}

		public void llTeleportAgentGlobalCoords(key kAvatarID, vector vGlobalPosition, vector vRegionPosition, vector iLookAtPoint)
		{
			Verbose("llTeleportAgentHome({0}, {1}, {2}, {3})", kAvatarID, vGlobalPosition, vRegionPosition, iLookAtPoint);
		}

		public void llTeleportAgentHome(key kAvatarID)
		{
			Verbose("llTeleportAgentHome({0})", kAvatarID);
		}

		public void llTextBox(key kAvatar, String sText, integer iChannel)
		{
			Verbose(@"llTextBox({0}, ""{1}"", {2})", kAvatar, sText, iChannel);
			host.llTextBox(kAvatar, sText, iChannel);
		}

		public String llToLower(String sText)
		{
			string strTemp = ((string)sText).ToLower();
			Verbose(@"llToLower(""{0}"")=""{1}""", sText, strTemp);
			return strTemp;
		}

		public String llToUpper(String sText)
		{
			string strTemp = ((string)sText).ToUpper();
			Verbose(@"llToUpper(""{0}"")=""{1}""", sText, strTemp);
			return strTemp;
		}

		public key llTransferLindenDollars(key kPayee, integer iAmount)
		{
			key kID = new key(Guid.NewGuid());
			string strData = kPayee.ToString() + "," + iAmount.ToString();
			Verbose("llTransferLindenDollars({0}, {1})", kPayee, iAmount);
			host.ExecuteSecondLife("transaction_result", kID, true, (SecondLife.String)strData);
			return kID;
		}

		public void llTriggerSound(String sSoundName, Float fVolume)
		{
			Verbose(@"llTriggerSound(""{0}"", {1})", sSoundName, fVolume);
		}

		public void llTriggerSoundLimited(String sSoundName, Float fVolume, vector vBoxNE, vector vBoxSW)
		{
			Verbose("llTriggerSoundLimited({0}, {1}, {2}, {3})", sSoundName, fVolume, vBoxNE, vBoxSW);
		}

		public String llUnescapeURL(String sURL)
		{
			byte[] data = Encoding.UTF8.GetBytes(sURL.ToString());
			List<byte> list = new List<byte>();
			for (int intI = 0; intI < data.Length; intI++) {
				byte chrC = data[intI];
				if (chrC == (byte)'%') {
					if (intI < (data.Length - 2)) {
						list.Add((byte)
							(HexToInt(data[intI + 1]) << 4
							| HexToInt(data[intI + 2])));
					}
					intI += 2;
				} else {
					list.Add(chrC);
				}
			}
			data = list.ToArray();
			int intLen = Array.IndexOf(data, (byte)0x0);
			if (intLen < 0) {
				intLen = data.Length;
			}
			string strTmp = Encoding.UTF8.GetString(data, 0, intLen);
			Verbose(string.Format(@"llUnescapeURL(""{0}"")=""{1}""", sURL, strTmp));
			return strTmp;
		}

		public void llUnSit(key kAvatarID)
		{
			Verbose("llUnSit({0})", kAvatarID);
		}

		public void llUpdateCharacter(list lOptions)
		{
			Verbose("llUpdateCharacter({0})", lOptions.ToVerboseString());
		}

		public Float llVecDist(vector vPostionA, vector vPositionB)
		{
			vector vecValue = new vector(vPostionA.x - vPositionB.x, vPostionA.y - vPositionB.y, vPostionA.z - vPositionB.z);
			double dblMag = llVecMag(vecValue);
			Verbose("llVecDist({0}, {1})={2}", vPostionA, vPositionB, dblMag);
			return dblMag;
		}

		public Float llVecMag(vector vVector)
		{
			double dblValue = Math.Sqrt((vVector.x * vVector.x) + (vVector.y * vVector.y) + (vVector.z * vVector.z));
			Verbose("llVecMag({0})={1}", vVector, dblValue);
			return dblValue;
		}

		public vector llVecNorm(vector vVector)
		{
			double dblMag = llVecMag(vVector);
			vector vecValue = new vector(vVector.x / dblMag, vVector.y / dblMag, vVector.z / dblMag);
			Verbose("llVecNorm({0})={1}", vVector, vecValue);
			return vecValue;
		}

		public void llVolumeDetect(integer iDetectFlag)
		{
			Verbose("llVolumeDetect({0})", iDetectFlag);
		}

		public void llWanderWithin(vector vOrigin, vector vDistance, list lOptions)
		{
			Verbose("llWanderWithin({0}, {1}, {2})", vOrigin, vDistance, lOptions.ToVerboseString());
		}

		public Float llWater(vector vOffset)
		{
			Float fWaterLevel = 0.0F;
			Verbose("llWater({0})={1}", vOffset, fWaterLevel);
			return fWaterLevel;
		}

		public void llWhisper(integer iChannel, String sText)
		{
			Chat(iChannel, sText, CommunicationType.Whisper);
		}

		public vector llWind(vector vOffset)
		{
			vector vDirection = vector.ZERO_VECTOR;
			Verbose("llWind({0})={1}", vOffset, vDirection);
			return vDirection;
		}

		public String llXorBase64(String sText1, String sText2)
		{
			string sResult = "";
			Verbose(@"llXorBase64(""{0}"",""{1}"")=""{2}""", sText1, sText2, sResult);
			return sResult;
		}

		public String llXorBase64Strings(String sText1, String sText2)
		{
			string sResult = "";
			Verbose(@"llXorBase64Strings(""{0}"",""{1}"")=""{2}""", sText1, sText2, sResult);
			return sResult;
		}

		public String llXorBase64StringsCorrect(String sText1, String sText2)
		{
			string strS1 = Base64ToString(sText1.ToString());
			string strS2 = Base64ToString(sText2.ToString());
			int intLength = strS1.Length;
			if (strS2.Length == 0) {
				strS2 = " ";
			}
			while (strS2.Length < intLength) {
				strS2 += strS2;
			}
			strS2 = strS2.Substring(0, intLength);
			StringBuilder sb = new StringBuilder();
			for (int intI = 0; intI < intLength; intI++) {
				sb.Append((char)(strS1[intI] ^ strS2[intI]));
			}
			string sResult = StringToBase64(sb.ToString());
			Verbose(@"llXorBase64StringsCorrect(""{0}"",""{1}"")=""{2}""", sText1, sText2, sResult);
			return sResult;
		}
	}
}
