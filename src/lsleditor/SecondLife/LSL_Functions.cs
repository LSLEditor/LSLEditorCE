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
// LSL_Functions.cs
//
// </summary>

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
            var intA = (int)Math.Abs((long)fValue);
            this.Verbose("llAbs({0})={1}", fValue, intA);
            return intA;
        }

        public Float llAcos(Float fValue)
        {
            var dblA = Math.Acos(fValue);
            this.Verbose("llAcos({0})={1}", fValue, dblA);
            return dblA;
        }

        public void llAddToLandBanList(key kID, Float fHours)
        {
            this.Verbose("llAddToLandBanList({0}, {1})", kID, fHours);
            if (this.htLandBanList.ContainsKey(kID))
            {
                this.htLandBanList.Remove(kID);
            }
            this.htLandBanList.Add(kID, fHours);
        }

        public void llAddToLandPassList(key kID, Float fHours)
        {
            this.Verbose("llAddToLandPassList({0}, {1})", kID, fHours);
            if (this.htLandPassList.ContainsKey(kID))
            {
                this.htLandPassList.Remove(kID);
            }
            this.htLandPassList.Add(kID, fHours);
        }

        public void llAdjustSoundVolume(Float fVolume)
        {
            this.Verbose("llAdjustSoundVolume({0}), fVolume");
            this.fVolume = fVolume;
        }

        public void llAllowInventoryDrop(integer iAllowDrop)
        {
            this.Verbose("llAllowInventoryDrop({0})", iAllowDrop);
            this.blnAllowDrop = (bool)iAllowDrop;
        }

        public Float llAngleBetween(rotation a, rotation b)
        {
            Float fResult = 0.0F;
            var r = b / a;													// calculate the rotation between the two arguments as quaternion
            double s2 = r.s * r.s;											// square of the s-element
            double v2 = (r.x * r.x) + (r.y * r.y) + (r.z * r.z);			// sum of the squares of the v-elements

            if (s2 < v2)
            {																// compare the s-component to the v-component
                fResult = 2.0 * Math.Acos(Math.Sqrt(s2 / (s2 + v2)));		// use arccos if the v-component is dominant
            }
            else if (v2 != 0)
            {																// make sure the v-component is non-zero
                fResult = 2.0 * Math.Asin(Math.Sqrt(v2 / (s2 + v2)));		// use arcsin if the s-component is dominant
            }
            return fResult; // one or both arguments are scaled too small to be meaningful, or the values are the same, so return zero
            // implementation taken from LSL Portal. http://wiki.secondlife.com/w/index.php?title=LlAngleBetween
        }

        public void llApplyImpulse(vector vForce, integer iLocal)
        {
            this.Verbose("llApplyImpulse({0}, {1}", vForce, iLocal);
        }

        public void llApplyRotationalImpulse(vector vForce, integer iLocal)
        {
            this.Verbose("llApplyRotationalImpulse({0}, {1})", vForce, iLocal);
        }

        public Float llAsin(Float fValue)
        {
            var dblA = Math.Asin(fValue);
            this.Verbose("llAsin({0})={1}", fValue, dblA);
            return dblA;
        }

        public Float llAtan2(Float y, Float x)
        {
            var dblA = Math.Atan2(y, x);
            this.Verbose("llAtan2({0}, {1})={2}", y, x, dblA);
            return dblA;
        }

        public void llAttachToAvatar(integer iAttachPoint)
        {
            this.Verbose("llAttachToAvatar({0})", iAttachPoint);
        }

        public void llAttachToAvatarTemp(integer iAttachPoint)
        {
            this.Verbose("llAttachToAvatarTemp({0})", iAttachPoint);
        }

        public key llAvatarOnLinkSitTarget(integer iLinkIndex)
        {
            var kLinkUUID = new key(Guid.NewGuid());
            this.Verbose("llAvatarOnLinkSitTarget({0}))={1}", iLinkIndex, kLinkUUID);
            return kLinkUUID;
        }

        public key llAvatarOnSitTarget()
        {
            var kLinkUUID = new key(Guid.NewGuid());
            this.Verbose("llAvatarOnSitTarget()={0}", kLinkUUID);
            return kLinkUUID;
        }

        public rotation llAxes2Rot(vector vForward, vector vLeft, vector vUp)
        {
            var rRot = rotation.ZERO_ROTATION;
            this.Verbose("llAxes2Rot({0}, {1}, {2})={3}", vForward, vLeft, +vUp, rRot);
            return rRot;
        }

        public rotation llAxisAngle2Rot(vector vAxis, Float fAngle)
        {
            var vUnitAxis = this.llVecNorm(vAxis);
            var dSineHalfAngle = Math.Sin(fAngle / 2);
            var dCosineHalfAngle = Math.Cos(fAngle / 2);

            var rResult = new rotation(
                dSineHalfAngle * vUnitAxis.x,
                dSineHalfAngle * vUnitAxis.y,
                dSineHalfAngle * vUnitAxis.z,
                dCosineHalfAngle);

            this.Verbose("llAxisAngle2Rot({0}, {1})={2}", vAxis, fAngle, rResult);
            return rResult;
        }

        public integer llBase64ToInteger(String sString)
        {
            int iResult;

            try
            {
                string s = sString;
                var data = new byte[4];

                if (s.Length > 1)
                {
                    data[3] = (byte)(this.LookupBase64(s, 0) << 2);
                    data[3] |= (byte)(this.LookupBase64(s, 1) >> 4);
                }

                if (s.Length > 2)
                {
                    data[2] = (byte)((this.LookupBase64(s, 1) & 0xf) << 4);
                    data[2] |= (byte)(this.LookupBase64(s, 2) >> 2);
                }

                if (s.Length > 3)
                {
                    data[1] = (byte)((this.LookupBase64(s, 2) & 0x7) << 6);
                    data[1] |= (byte)this.LookupBase64(s, 3);
                }

                if (s.Length > 5)
                {
                    data[0] = (byte)(this.LookupBase64(s, 4) << 2);
                    data[0] |= (byte)(this.LookupBase64(s, 5) >> 4);
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
            }
            catch
            {
                iResult = (new Random()).Next();
            }
            this.Verbose(@"llBase64ToInteger(""{0}"")={1}", sString, iResult);
            return iResult;
        }

        public String llBase64ToString(String sString)
        {
            var sResult = this.Base64ToString(sString.ToString());
            this.Verbose(@"llBase64ToString(""{0}"")=""{1}""", sString, sResult);
            return sResult;
        }

        public void llBreakAllLinks()
        {
            this.host.llBreakAllLinks();
            this.Verbose("llBreakAllLinks()");
        }

        public void llBreakLink(integer iLinkIndex)
        {
            this.host.llBreakLink(iLinkIndex);
            this.Verbose("llBreakLink({0})", iLinkIndex);
        }

        public list llCSV2List(String sString)
        {
            string sSource = sString;
            var lResult = new list();
            var sb = new StringBuilder();
            var intWithinAngelBracket = 0;
            for (var intI = 0; intI < sSource.Length; intI++)
            {
                var chrC = sSource[intI];
                if (chrC == '<')
                {
                    intWithinAngelBracket++;
                }
                else if (chrC == '>')
                {
                    intWithinAngelBracket--;
                }

                if (intWithinAngelBracket == 0 && chrC == ',')
                {
                    lResult.Add(sb.ToString());
                    sb = new StringBuilder();
                }
                else
                {
                    sb.Append(sSource[intI]);
                }
            }

            // dont forget the last one
            lResult.Add(sb.ToString());

            // remove the first space, if any
            for (var intI = 0; intI < lResult.Count; intI++)
            {
                var strValue = lResult[intI].ToString();
                if (strValue?.Length == 0)
                {
                    continue;
                }
                if (strValue[0] == ' ')
                {
                    lResult[intI] = strValue.Substring(1);
                }
            }

            this.Verbose(@"llCSV2List(""{0}"")={1}", sSource, lResult);

            return lResult;
        }

        public list llCastRay(vector vStart, vector vEnd, list lOptions)
        {
            var lResult = new list();
            this.Verbose("llCastRay({0}, {1}, {2})={3}", vStart, vEnd, lOptions, lResult);
            return lResult;
        }

        public integer llCeil(Float fValue)
        {
            var intA = (int)Math.Ceiling(fValue);
            this.Verbose("llCeiling({0})={1}", fValue, intA);
            return intA;
        }

        public void llClearCameraParams()
        {
            this.Verbose("llClearCameraParams()");
        }

        public integer llClearLinkMedia(integer iLink, integer iFace)
        {
            this.Verbose("llClearLinkMedia({0}, {1})={2}", iLink, iFace, true);
            return true;
        }

        public integer llClearPrimMedia(integer iFace)
        {
            integer iResult = 0;
            this.Verbose("llClearPrimMedia({0})={1}", iFace, iResult);
            return iResult;
        }

        public void llCloseRemoteDataChannel(key kChannel)
        {
            this.host.llCloseRemoteDataChannel(kChannel);
            this.Verbose("llCloseRemoteDataChannel({0})", kChannel);
        }

        public Float llCloud(vector vOffset)
        {
            Float fResult = 0.0F;
            this.Verbose("llCloud({0})={1}", vOffset, fResult);
            return fResult;
        }

        public void llCollisionFilter(String sName, key kID, integer iAccept)
        {
            this.Verbose(@"llCollisionFilter(""{0}"", {1}, {2})", sName, kID, iAccept);
        }

        public void llCollisionSound(String sImpactSound, Float fImpactVolume)
        {
            this.Verbose(@"llCollisionSound(""{0}"", {1})", sImpactSound, +fImpactVolume);
        }

        public void llCollisionSprite(String sImpactSprite)
        {
            this.Verbose(@"llCollisionSprite(""{0}"")", sImpactSprite);
        }

        public Float llCos(Float fTheta)
        {
            var dblA = Math.Cos(fTheta);
            this.Verbose("llCos({0})={1}", fTheta, dblA);
            return dblA;
        }

        public void llCreateCharacter(list lOptions)
        {
            this.Verbose("llCreateCharacter({0})", lOptions);
        }

        public void llCreateLink(key kID, integer iSimulator)
        {
            this.Verbose("llCreateLink({0}, {1})", kID, iSimulator);
        }

        public void llDeleteCharacter()
        {
            this.Verbose("llDeleteCharacter()");
        }

        public list llDeleteSubList(list lSource, integer iStart, integer iEnd)
        {
            var intLength = lSource.Count;

            int start = iStart;
            int end = iEnd;

            var src = new list(lSource);

            if (this.CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (var intI = start; intI <= end; intI++)
                    {
                        src[intI] = null;
                    }
                }
                else
                { // excluding
                    for (var intI = 0; intI <= end; intI++)
                    {
                        src[intI] = null;
                    }
                    for (var intI = start; intI < intLength; intI++)
                    {
                        src[intI] = null;
                    }
                }
            }
            var lResult = new list();
            for (var intI = 0; intI < src.Count; intI++)
            {
                if (src[intI] != null)
                {
                    lResult.Add(src[intI]);
                }
            }
            this.Verbose(string.Format("llDeleteSubList({0}, {1}, {2})={3}", lSource, iStart, iEnd, lResult));
            return lResult;
        }

        public String llDeleteSubString(String sSource, integer iStart, integer iEnd)
        {
            var src = sSource.ToString().ToCharArray();
            int start = iStart;
            int end = iEnd;

            var intLength = src.Length;

            if (this.CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (var intI = start; intI <= end; intI++)
                    {
                        src[intI] = '\0';
                    }
                }
                else
                { // excluding
                    for (var intI = 0; intI <= end; intI++)
                    {
                        src[intI] = '\0';
                    }
                    for (var intI = start; intI < intLength; intI++)
                    {
                        src[intI] = '\0';
                    }
                }
            }
            var result = new StringBuilder();
            for (var intI = 0; intI < src.Length; intI++)
            {
                if (src[intI] != '\0')
                {
                    result.Append(src[intI]);
                }
            }

            this.Verbose(string.Format(@"llDeleteSubString(""{0}"", {1}, {2})=""{3}""", sSource, iStart, iEnd, result));
            return result.ToString();
        }

        public void llDetachFromAvatar()
        {
            this.Verbose("llDetachFromAvatar()");
        }

        public vector llDetectedGrab(integer iIndex)
        {
            var vCoords = vector.ZERO_VECTOR;
            this.Verbose("llDetectedGrab({0})={1}", iIndex, vCoords);
            return vCoords;
        }

        public integer llDetectedGroup(integer iIndex)
        {
            integer iDetected = 0;
            this.Verbose("llDetectedGroup({0})={1}", iIndex, iDetected);
            return iDetected;
        }

        public key llDetectedKey(integer iIndex)
        {
            var kID = new key(Properties.Settings.Default.AvatarKey);
            this.Verbose("llDetectedKey({0})={1}", iIndex, kID);
            return kID;
        }

        public integer llDetectedLinkNumber(integer iLinkIndex)
        {
            integer iDetected = 0;
            this.Verbose("llDetectedLinkNumber({0})={1}", iLinkIndex, iDetected);
            return iDetected;
        }

        public String llDetectedName(integer iIndex)
        {
            var sResult = Properties.Settings.Default.AvatarName;
            this.Verbose(@"llDetectedName({0})=""{1}""", iIndex, sResult);
            return sResult;
        }

        public key llDetectedOwner(integer iIndex)
        {
            var kID = new key(Properties.Settings.Default.AvatarKey);
            this.Verbose("llDetectedOwner({0})={1}", iIndex, kID);
            return kID;
        }

        public vector llDetectedPos(integer iIndex)
        {
            var vCoords = vector.ZERO_VECTOR;
            this.Verbose("llDetectedPos({0})={1}", iIndex, vCoords);
            return vCoords;
        }

        public rotation llDetectedRot(integer iIndex)
        {
            var rRotation = rotation.ZERO_ROTATION;
            this.Verbose("llDetectedRot({0})={1}", iIndex, rRotation);
            return rRotation;
        }

        public vector llDetectedTouchBinormal(integer iIndex)
        {
            var vCoords = new vector();
            this.Verbose("llDetectedTouchBinormal({0})={1}", iIndex, vCoords);
            return vCoords;
        }

        public integer llDetectedTouchFace(integer iIndex)
        {
            integer iFace = 0;
            this.Verbose("llDetectedTouchFace({0})={1}", iIndex, iFace);
            return iFace;
        }

        public vector llDetectedTouchNormal(integer iIndex)
        {
            var vNormal = new vector();
            this.Verbose("llDetectedTouchNormal({0})={1}", iIndex, vNormal);
            return vNormal;
        }

        public vector llDetectedTouchPos(integer iIndex)
        {
            var vCoords = vector.ZERO_VECTOR;
            this.Verbose("llDetectedTouchPos({0})={1}", iIndex, vCoords);
            return vCoords;
        }

        public vector llDetectedTouchST(integer iIndex)
        {
            var vCoords = vector.ZERO_VECTOR;
            this.Verbose("llDetectedTouchST({0})={1}", iIndex, vCoords);
            return vCoords;
        }

        public vector llDetectedTouchUV(integer iIndex)
        {
            var vUV = vector.ZERO_VECTOR;
            this.Verbose("llDetectedTouchUV({0})={1}", iIndex, vUV);
            return vUV;
        }

        public integer llDetectedType(integer iIndex)
        {
            integer iResult = AGENT;
            this.Verbose("llDetectedType({0})={1}", iIndex, iResult);
            return iResult;
        }

        public vector llDetectedVel(integer iIndex)
        {
            var vVelocity = vector.ZERO_VECTOR;
            this.Verbose("llDetectedVel({0})={1}", iIndex, vVelocity);
            return vVelocity;
        }

        public void llDialog(key kID, String sMessage, list lButtons, integer iChannel)
        {
            this.Verbose(@"llDialog({0}, ""{1}"", {2}, {3})", kID, sMessage, lButtons, iChannel);
            this.host.llDialog(kID, sMessage, lButtons, iChannel);
        }

        public void llDie()
        {
            this.Verbose("llDie()");
            this.host.Die();
        }

        public String llDumpList2String(list lSource, String sSeparator)
        {
            var result = new StringBuilder();
            for (var intI = 0; intI < lSource.Count; intI++)
            {
                if (intI > 0)
                {
                    result.Append(sSeparator.ToString());
                }
                result.Append(lSource[intI].ToString());
            }
            this.Verbose(@"llDumpList2String({0},""{1}"")=""{2}""", lSource, sSeparator, result.ToString());
            return result.ToString();
        }

        public integer llEdgeOfWorld(vector vPosition, vector vDirection)
        {
            integer iResult = 0;
            this.Verbose("llEdgeOfWorld({0}, {1})={2}", vPosition, vDirection, iResult);
            return iResult;
        }

        public void llEjectFromLand(key kID)
        {
            this.Verbose("llEjectFromLand({0})", kID);
        }

        public void llEmail(String sAddress, String sSubject, String sMessage)
        {
            this.Verbose(@"llEmail(""{0}"", ""{1}"", ""{2}"")", sAddress, sSubject, sMessage);
            this.host.Email(sAddress, sSubject, sMessage);
        }

        public String llEscapeURL(String sURL)
        {
            var sb = new StringBuilder();
            var data = Encoding.UTF8.GetBytes(sURL.ToString());
            for (var intI = 0; intI < data.Length; intI++)
            {
                var chrC = data[intI];
                if ((chrC >= 'a' && chrC <= 'z') || (chrC >= 'A' && chrC <= 'Z') || (chrC >= '0' && chrC <= '9'))
                {
                    sb.Append((char)chrC);
                }
                else
                {
                    sb.AppendFormat("%{0:X2}", (int)chrC);
                }
            }
            this.Verbose(string.Format(@"EscapeURL(""{0}"")=""{1}""", sURL, sb.ToString()));
            return sb.ToString();
        }

        public rotation llEuler2Rot(vector v)
        {
            v /= 2.0;
            var ax = Math.Sin(v.x);
            var aw = Math.Cos(v.x);
            var by = Math.Sin(v.y);
            var bw = Math.Cos(v.y);
            var cz = Math.Sin(v.z);
            var cw = Math.Cos(v.z);
            var rRotation = new rotation(
                (aw * by * cz) + (ax * bw * cw),
                (aw * by * cw) - (ax * bw * cz),
                (aw * bw * cz) + (ax * by * cw),
                (aw * bw * cw) - (ax * by * cz));
            this.Verbose("llEuler2Rot({0})={1}", v, rRotation);
            return rRotation;
        }

        public void llEvade(key kTargetID, list lOptions)
        {
            this.Verbose("llEvade({0}, {1})", kTargetID, lOptions);
        }

        public void llExecCharacterCmd(integer iCommand, list lOptions)
        {
            this.Verbose("llExecCharacterCmd({0}, {1})", iCommand, lOptions);
        }

        public Float llFabs(Float fValue)
        {
            var dblA = Math.Abs(fValue);
            this.Verbose("llFabs({0})={1}", fValue, dblA);
            return dblA;
        }

        public void llFleeFrom(vector vSource, Float fDistance, list lOptions)
        {
            this.Verbose("llFleeFrom({0}, {1}, {2})", vSource, fDistance, lOptions);
        }

        public integer llFloor(Float fValue)
        {
            var intA = (int)Math.Floor(fValue);
            this.Verbose("llFloor({0})={1}", fValue, intA);
            return intA;
        }

        public void llForceMouselook(integer iMouselook)
        {
            this.Verbose("llForceMouselook({0})", iMouselook);
        }

        public Float llFrand(Float fMaximum)
        {
            double dblValue = fMaximum * this.rdmRandom.NextDouble();
            this.Verbose("llFrand({0})={1}", fMaximum, dblValue);
            return dblValue;
        }

        public key llGenerateKey()
        {
            var kID = new key(Guid.NewGuid());
            this.Verbose("llGenerateKey()={0}", kID);
            return kID;
        }

        public vector llGetAccel()
        {
            var vAcceleration = vector.ZERO_VECTOR;
            this.Verbose("llGetAccel()={0}", vAcceleration);
            return vAcceleration;
        }

        public integer llGetAgentInfo(key kID)
        {
            integer iAgentFlags = 0;
            this.Verbose("llGetAgentInfo({0})={1}", kID, iAgentFlags);
            return iAgentFlags;
        }

        public String llGetAgentLanguage(key kID)
        {
            const string sLanguageCode = "en-us";
            this.Verbose(@"llGetAgentLanguage({0})=""{1}""", kID, sLanguageCode);
            return sLanguageCode;
        }

        public list llGetAgentList(integer iScope, list lOptions)
        {
            var lAgents = new list();
            this.Verbose("llGetAgentList({0}, [{1}])={2}", iScope, lOptions, lAgents);
            return lAgents;
        }

        public vector llGetAgentSize(key kID)
        {
            var vAgentSize = vector.ZERO_VECTOR;
            this.Verbose("llGetAgentSize({0})={1}", kID, vAgentSize);
            return vAgentSize;
        }

        public Float llGetAlpha(integer iFace)
        {
            Float fOpacity = 1.0F;
            this.Verbose("llGetAlpha({0})={1}", iFace, fOpacity);
            return fOpacity;
        }

        public Float llGetAndResetTime()
        {
            // get time
            double dblTime = this.llGetTime();
            this.Verbose("llGetAndResetTime()=" + dblTime);

            // reset time
            this.llResetTime();
            return dblTime;
        }

        public String llGetAnimation(key kID)
        {
            String sAnimation = "";
            this.Verbose(@"llGetAnimation({0})=""{1}""", kID, sAnimation);
            return sAnimation;
        }

        public list llGetAnimationList(key kID)
        {
            var lAnimationList = new list();
            this.Verbose("llGetAnimationList({0})={1}", kID, lAnimationList);
            return lAnimationList;
        }

        public String llGetAnimationOverride(String sAnimationState)
        {
            String sAnimation = "";
            this.Verbose(@"llGetAnimationOverride(""{0}"")=""{1}""", sAnimationState, sAnimation);
            return sAnimation;
        }

        public integer llGetAttached()
        {
            integer iAttachPoint = 0;
            this.Verbose("llGetAttached()={0}", iAttachPoint);
            return iAttachPoint;
        }

        public list llGetBoundingBox(key kID)
        {
            var lBoundingCoords = new list();
            this.Verbose("llGetBoundingBox({0})={1}", kID, lBoundingCoords);
            return lBoundingCoords;
        }

        public vector llGetCameraPos()
        {
            var vCameraCoords = vector.ZERO_VECTOR;
            this.Verbose("llGetCameraPos()={0}", vCameraCoords);
            return vCameraCoords;
        }

        public rotation llGetCameraRot()
        {
            var rCameraRotation = rotation.ZERO_ROTATION;
            this.Verbose("llGetCameraRot()={0}", rCameraRotation);
            return rCameraRotation;
        }

        public vector llGetCenterOfMass()
        {
            var vCenterOfMass = vector.ZERO_VECTOR;
            this.Verbose("llGetCenterOfMass()={0}", vCenterOfMass);
            return vCenterOfMass;
        }

        public list llGetClosestNavPoint(vector lPoint, list lOptions)
        {
            var lClosetNavPoint = new list();
            this.Verbose("llGetClosestNavPoint({0}, {1})={2}", lPoint, lOptions, lClosetNavPoint);
            return lClosetNavPoint;
        }

        public vector llGetColor(integer iFace)
        {
            var vColour = vector.ZERO_VECTOR;
            this.Verbose("llGetColor({0})={1}", iFace, vColour);
            return vColour;
        }

        public key llGetCreator()
        {
            key kResult = Properties.Settings.Default.AvatarKey;
            this.Verbose("llGetCreator()={0}", kResult);
            return kResult;
        }

        public String llGetDate()
        {
            var sResult = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd");
            this.Verbose("llGetDate()={0}", sResult);
            return sResult;
        }

        public string llGetDisplayName(key kAvatarID)
        {
            const string sResult = "";
            this.Verbose("llGetDisplayName({0})={1}", kAvatarID, sResult);
            return sResult;
        }

        public Float llGetEnergy()
        {
            Float fResult = 1.23;
            this.Verbose("llGetEnergy()={0}", fResult);
            return fResult;
        }

        public string llGetEnv(string sDataRequest)
        {
            string sResult;
            switch (sDataRequest)
            {
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
            this.Verbose(@"llGetEnv(""{0}"")=""{1}""", sDataRequest, sResult);
            return sResult;
        }

        public vector llGetForce()
        {
            var vForce = vector.ZERO_VECTOR;
            this.Verbose("llGetForce()={0}", vForce);
            return vForce;
        }

        public integer llGetFreeMemory()
        {
            integer iFreeMemory = 16000;
            this.Verbose("llGetFreeMemory()={0}", iFreeMemory);
            return iFreeMemory;
        }

        public integer llGetFreeURLs()
        {
            integer iFreeURLs = 0;
            this.Verbose("llGetFreeURLs()={0}", iFreeURLs);
            return iFreeURLs;
        }

        public Float llGetGMTclock()
        {
            Float fResult = DateTime.Now.ToUniversalTime().TimeOfDay.TotalSeconds;
            this.Verbose("llGetGMTclock()={0}", fResult);
            return fResult;
        }

        public vector llGetGeometricCenter()
        {
            var vResult = ZERO_VECTOR;
            this.Verbose("llGetGeometricCenter()={0}", vResult);
            return vResult;
        }

        public String llGetHTTPHeader(key kRequestID, String sDesiredHeader)
        {
            String sResult = "not-implemented";
            this.Verbose(@"llGetHTTPHeader({0}, ""{1}"")=""{2}""", kRequestID, sDesiredHeader, sResult);
            return sResult;
        }

        public key llGetInventoryCreator(String sItem)
        {
            key kAvatarID = Properties.Settings.Default.AvatarKey;
            this.Verbose(@"llGetInventoryCreator(""{0}"")={1}", sItem, kAvatarID);
            return kAvatarID;
        }

        public key llGetInventoryKey(String sItemName)
        {
            var kID = this.host.GetInventoryKey(sItemName);
            this.Verbose(@"llGetInventoryKey(""{0}"")={1}", sItemName, kID);
            return kID;
        }

        public String llGetInventoryName(integer iItemType, integer iItemIndex)
        {
            string sItemName = this.host.GetInventoryName(iItemType, iItemIndex);
            this.Verbose(@"llGetInventoryName({0}, {1})=""{2}""", iItemType, iItemIndex, sItemName);
            return sItemName;
        }

        public integer llGetInventoryNumber(integer iType)
        {
            int iTypeCount = this.host.GetInventoryNumber(iType);
            this.Verbose("llGetInventoryNumber({0})={1}", iType, iTypeCount);
            return iTypeCount;
        }

        public integer llGetInventoryPermMask(String sItemName, integer iPermMask)
        {
            integer iPermissionState = 0;
            this.Verbose(@"llGetInventoryPermMask(""{0}"", {1})={2}", sItemName, iPermMask, iPermissionState);
            return iPermissionState;
        }

        public integer llGetInventoryType(String sItemName)
        {
            var iItemType = this.host.GetInventoryType(sItemName);
            this.Verbose(@"llGetInventoryType(""{0}"")={1}", sItemName, iItemType);
            return iItemType;
        }

        public key llGetKey()
        {
            var kID = this.host.GetKey();
            this.Verbose(@"llGetKey()=""{0}""", kID.ToString());
            return kID;
        }

        public key llGetLandOwnerAt(vector vPosition)
        {
            var kAvatarID = new key(Guid.NewGuid());
            this.Verbose("llGetLandOwnerAt({0})={1}", vPosition, kAvatarID);
            return kAvatarID;
        }

        public key llGetLinkKey(integer iLinkIndex)
        {
            var kID = new key(Guid.NewGuid());
            this.Verbose("llGetLinkKey({0})={1}", iLinkIndex, kID);
            return kID;
        }

        public list llGetLinkMedia(integer iLinkIndex, integer iFace, list lParameters)
        {
            var lMediaList = new list();
            this.Verbose("llGetLinkMedia({0}, {1}, {2})={3}", iLinkIndex, iFace, lParameters, lMediaList);
            return lMediaList;
        }

        public String llGetLinkName(integer iLinkIndex)
        {
            String sLinkName = "";
            this.Verbose(@"llGetLinkName({0})=""{1}""", iLinkIndex, sLinkName);
            return sLinkName;
        }

        public integer llGetLinkNumber()
        {
            integer iLinkIndex = 0;
            this.Verbose("llGetLinkNumber()={0}", iLinkIndex);
            return iLinkIndex;
        }

        public integer llGetLinkNumberOfSides(integer iLinkIndex)
        {
            integer iSides = 6;
            this.Verbose("llGetLinkNumberOfSides({0})={1}", iLinkIndex, iSides);
            return iSides;
        }

        public list llGetLinkPrimitiveParams(integer iLinkIndex, list lParametersRequested)
        {
            var lParametersReturned = new list();
            this.Verbose("llGetLinkPrimitiveParams({0}, {1})={2}", iLinkIndex, lParametersRequested, lParametersReturned);
            return lParametersReturned;
        }

        public integer llGetListEntryType(list lSource, integer iIndex)
        {
            integer iEntryType;

            if (iIndex < 0)
            {
                iIndex = lSource.Count + iIndex;
            }
            if (iIndex >= lSource.Count || iIndex < 0)
            {
                iEntryType = 0;
            }
            else
            {
                switch (lSource[iIndex].GetType().ToString().Replace("LSLEditor.SecondLife+", ""))
                {
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
            this.Verbose("llGetListEntryType({0}, {1})={2}", lSource, iIndex, iEntryType);
            return iEntryType;
        }

        public integer llGetListLength(list lSource)
        {
            integer iLength = lSource.Count;
            this.Verbose("llGetListLength({0})={1}", lSource, iLength);
            return iLength;
        }

        public vector llGetLocalPos()
        {
            this.Verbose("llGetLocalPos()={0}", this.vPosition);
            return this.vPosition;
        }

        public rotation llGetLocalRot()
        {
            this.Verbose("llGetLocalRot()={0}", this.rRotationlocal);
            return this.rRotationlocal;
        }

        public Float llGetMass()
        {
            Float fMass = 1.23;
            this.Verbose("llGetMass()={0}", fMass);
            return fMass;
        }

        public Float llGetMassMKS()
        {
            Float fMass = 1.23;
            this.Verbose("llGetMassMKS()={0}", fMass);
            return fMass;
        }

        public integer llGetMemoryLimit()
        {
            integer iMaxMemory = 65536;
            this.Verbose("llGetMemoryLimit()={0}", iMaxMemory);
            return iMaxMemory;
        }

        public void llGetNextEmail(String sAddress, String sSubject)
        {
            this.Verbose(@"llGetNextEmail(""{0}"", ""{1}"")", sAddress, sSubject);
        }

        public key llGetNotecardLine(String sNoteName, integer iLineIndex)
        {
            var kID = this.host.GetNotecardLine(sNoteName, iLineIndex);
            this.Verbose(@"llGetNotecardLine(""{0}"", {1})={2}", sNoteName, iLineIndex, kID);
            return kID;
        }

        public key llGetNumberOfNotecardLines(String sNoteName)
        {
            var kID = this.host.GetNumberOfNotecardLines(sNoteName);
            this.Verbose(@"llGetNumberOfNotecardLines(""{0}"")={1}", sNoteName, kID);
            return kID;
        }

        public integer llGetNumberOfPrims()
        {
            integer iPrimCount = 10;
            this.Verbose("llGetNumberOfPrims()={0}", iPrimCount);
            return iPrimCount;
        }

        public integer llGetNumberOfSides()
        {
            integer iSideCount = 6;
            this.Verbose("llGetNumberOfSides()={0}", iSideCount);
            return iSideCount;
        }

        public String llGetObjectDesc()
        {
            var sDescription = this.host.GetObjectDescription();
            this.Verbose(@"llGetObjectDesc()=""{0}""", sDescription);
            return sDescription;
        }

        public list llGetObjectDetails(key kID, list lObjectFlags)
        {
            var lObjectDetails = new list();
            for (var intI = 0; intI < lObjectFlags.Count; intI++)
            {
                if (!(lObjectFlags[intI] is integer))
                {
                    continue;
                }
                switch ((integer)lObjectFlags[intI])
                {
                    case OBJECT_NAME:
                        lObjectDetails.Add((SecondLife.String)this.host.GetObjectName(new Guid(kID.guid)));
                        break;
                    case OBJECT_DESC:
                        lObjectDetails.Add((SecondLife.String)this.host.GetObjectDescription(new Guid(kID.guid)));
                        break;
                    case OBJECT_POS:
                        lObjectDetails.Add(this.llGetPos());
                        break;
                    case OBJECT_ROT:
                        lObjectDetails.Add(this.llGetRot());
                        break;
                    case OBJECT_VELOCITY:
                        lObjectDetails.Add(this.llGetVel());
                        break;
                    case OBJECT_OWNER:
                        lObjectDetails.Add(this.llGetOwner());
                        break;
                    case OBJECT_GROUP:
                        lObjectDetails.Add(OBJECT_UNKNOWN_DETAIL);
                        break;
                    case OBJECT_CREATOR:
                        lObjectDetails.Add(this.llGetCreator());
                        break;
                    default:
                        lObjectDetails.Add(OBJECT_UNKNOWN_DETAIL);
                        break;
                }
            }
            this.Verbose("llGetObjectDetails({0}, {1})={2}", kID, lObjectFlags, lObjectDetails);
            return lObjectDetails;
        }

        public Float llGetObjectMass(key kID)
        {
            Float fMass = 0.0F;
            this.Verbose("llGetObjectMass({0})={1}", kID, fMass);
            return fMass;
        }

        public String llGetObjectName()
        {
            var sObjectName = this.host.GetObjectName();
            this.Verbose(@"llGetObjectName()=""{0}""", sObjectName);
            return sObjectName;
        }

        public integer llGetObjectPermMask(integer iRequestedMask)
        {
            integer iRetunedMaskState = 0;
            this.Verbose("llGetObjectPermMask({0})={1}", iRequestedMask, iRetunedMaskState);
            return iRetunedMaskState;
        }

        // added 4 mei 2007
        public integer llGetObjectPrimCount(key kID)
        {
            integer iPrimCount = 0;
            this.Verbose("llGetObjectPrimCount({0})={1}", kID, iPrimCount);
            return iPrimCount;
        }

        public vector llGetOmega()
        {
            var vOmega = vector.ZERO_VECTOR;
            this.Verbose("llGetOmega()={0}", vOmega);
            return vOmega;
        }

        public key llGetOwner()
        {
            var kID = new key(Properties.Settings.Default.AvatarKey);
            this.Verbose("llGetOwner()={0}", kID);
            return kID;
        }

        public key llGetOwnerKey(key kID)
        {
            var kAvatarID = this.llGetOwner();	// This is incorrect, as the owner of this object may not be the owner of kID
            this.Verbose("llGetOwnerKey({0})={1}", kID, kAvatarID);
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
            var lReturnedDetails = new list();
            for (var intI = 0; intI < lRequestedDetails.Count; intI++)
            {
                if (lRequestedDetails[intI] is integer)
                {
                    switch ((integer)lRequestedDetails[intI])
                    {
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
            this.Verbose("llGetParcelDetails({0}, {1})={2}", vPosition, lRequestedDetails, lReturnedDetails);
            return lReturnedDetails;
        }

        public integer llGetParcelFlags(vector vPosition)
        {
            integer iReturnedFlags = 0;
            this.Verbose("llGetParcelFlags({0})={1}", vPosition, iReturnedFlags);
            return iReturnedFlags;
        }

        public integer llGetParcelMaxPrims(vector vPosition, integer iSimWide)
        {
            integer iMaxPrims = 0;
            this.Verbose("llGetParcelMaxPrims({0}, {1})={2}", vPosition, iSimWide, iMaxPrims);
            return iMaxPrims;
        }

        public string llGetParcelMusicURL()
        {
            this.Verbose(@"llGetParcelMaxPrims()=""{0}""", this.sParcelMusicURL);
            return this.sParcelMusicURL;
        }

        public integer llGetParcelPrimCount(vector vPosition, integer iCategory, integer iSimWide)
        {
            integer iPrimCount = 0;
            this.Verbose("llGetParcelPrimCount({0}, {1}, {2})={3}", vPosition, iCategory, iSimWide, iPrimCount);
            return iPrimCount;
        }

        public list llGetParcelPrimOwners(vector vPosition)
        {
            var lOwners = new list(new object[] { Properties.Settings.Default.AvatarKey, 10 });
            this.Verbose("llGetParcelPrimOwners({0})={1}", vPosition, lOwners);
            return lOwners;
        }

        public integer llGetPermissions()
        {
            integer iPermissions = 0;
            this.Verbose("llGetPermissions()={0}", iPermissions);
            return iPermissions;
        }

        public key llGetPermissionsKey()
        {
            var kID = key.NULL_KEY;
            this.Verbose("llGetPermissionsKey()={0}", kID);
            return kID;
        }

        public list llGetPhysicsMaterial()
        {
            var lMaterials = new list();
            this.Verbose("llGetPhysicalMaterial()={0}", lMaterials);
            return lMaterials;
        }

        public vector llGetPos()
        {
            this.Verbose("llGetPos()={0}", this.vPosition);
            return this.vPosition;
        }

        public list llGetPrimMediaParams(integer iFace, list lDesiredParams)
        {
            var lReturnedParams = new list();
            this.Verbose("llGetPrimMediaParams({0}, {1})={2}", iFace, lDesiredParams, lReturnedParams);
            return lReturnedParams;
        }

        public list llGetPrimitiveParams(list lDesiredParams)
        {
            var lReturnedParams = new list();
            this.Verbose("llGetPrimitiveParams({0})={1}", lDesiredParams, lReturnedParams);
            return lReturnedParams;
        }

        // 334
        public integer llGetRegionAgentCount()
        {
            integer iAgentCount = 0;
            this.Verbose("llGetRegionAgentCount()={0}", iAgentCount);
            return iAgentCount;
        }

        public vector llGetRegionCorner()
        {
            var pRegionCorner = Properties.Settings.Default.RegionCorner;
            var vRegionCorner = new vector(pRegionCorner.X, pRegionCorner.Y, 0);
            this.Verbose("llGetRegionCorner()={0}", vRegionCorner);
            return vRegionCorner;
        }

        public Float llGetRegionFPS()
        {
            Float fRegionFPS = Properties.Settings.Default.RegionFPS;
            this.Verbose("llGetRegionFPS()={0}", fRegionFPS);
            return fRegionFPS;
        }

        public integer llGetRegionFlags()
        {
            integer iRegionFlags = 0;
            this.Verbose("llGetRegionFlags()={0}", iRegionFlags);
            return iRegionFlags;
        }

        public String llGetRegionName()
        {
            String sRegionName = Properties.Settings.Default.RegionName;
            this.Verbose("llGetRegionName()={0}", sRegionName);
            return sRegionName;
        }

        public Float llGetRegionTimeDilation()
        {
            Float fTimeDilation = 0.9F;
            this.Verbose("llGetRegionTimeDilation()={0}", fTimeDilation);
            return fTimeDilation;
        }

        public vector llGetRootPosition()
        {
            var vRootPosition = vector.ZERO_VECTOR;
            this.Verbose("llGetRootPosition()={0}", vRootPosition);
            return vRootPosition;
        }

        public rotation llGetRootRotation()
        {
            var vRootRotation = rotation.ZERO_ROTATION;
            this.Verbose("llGetRootRotation()={0}", vRootRotation);
            return vRootRotation;
        }

        public rotation llGetRot()
        {
            this.Verbose("llGetRot()={0}", this.rRotation);
            return this.rRotation;
        }

        public integer llGetSPMaxMemory()
        {
            integer iMaxSPMemory = 65536;
            this.Verbose("llGetSPMaxMemory()={0}", iMaxSPMemory);
            return iMaxSPMemory;
        }

        public vector llGetScale()
        {
            this.Verbose("llGetScale()=" + this.vScale);
            return this.vScale;
        }

        public String llGetScriptName()
        {
            var sScriptName = this.host.GetScriptName();
            this.Verbose("llGetScriptName()={0}", sScriptName);
            return sScriptName;
        }

        public integer llGetScriptState(String sScriptName)
        {
            integer iScriptState = 0;
            this.Verbose(@"llGetScriptState(""{0}"")={1}", sScriptName, iScriptState);
            return iScriptState;
        }

        public float llGetSimStats(integer iStatType)
        {
            const float iSimStat = 0.0F;
            this.Verbose("llGetSimStats({0})={1}", iStatType, iSimStat);
            return iSimStat;
        }

        public String llGetSimulatorHostname()
        {
            String sSimHostName = "";
            this.Verbose(@"llGetSimulatorHostname()=""{0}""", sSimHostName);
            return sSimHostName;
        }

        public integer llGetStartParameter()
        {
            this.Verbose("llGetStartParameter()={0}" + this.iStartParameter);
            return this.iStartParameter;
        }

        public list llGetStaticPath(vector vStart, vector vEnd, Float fRadius, list lParameters)
        {
            var lReturn = new list();
            this.Verbose("llGetStaticPath({0}, {1}, {2}, {3})={4}", vStart, vEnd, fRadius, lParameters, lReturn);
            return lReturn;
        }

        public integer llGetStatus(integer iRequestedStatus)
        {
            integer iStatus = 0;
            this.Verbose("llGetStatus({0})={1}", iRequestedStatus, iStatus);
            return iStatus;
        }

        public String llGetSubString(String sSource, integer iStart, integer iEnd)
        {
            string src = sSource;
            int start = iStart;
            int end = iEnd;

            var sResult = new StringBuilder();

            var intLength = src.Length;

            if (this.CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (var intI = start; intI <= end; intI++)
                    {
                        sResult.Append(src[intI]);
                    }
                }
                else
                { // excluding
                    for (var intI = 0; intI <= end; intI++)
                    {
                        sResult.Append(src[intI]);
                    }
                    for (var intI = start; intI < intLength; intI++)
                    {
                        sResult.Append(src[intI]);
                    }
                }
            }
            this.Verbose(string.Format(@"GetSubString(""{0}"", {1}, {2})=""{3}""", sSource, iStart, iEnd, sResult));
            return sResult.ToString();
        }

        public vector llGetSunDirection()
        {
            var vSunDirection = vector.ZERO_VECTOR;
            this.Verbose("llGetSunDirection()={0}", vSunDirection);
            return vSunDirection;
        }

        public String llGetTexture(integer iFace)
        {
            String sTexture = "";
            this.Verbose(@"llGetTexture({0})=""{1}""", iFace, sTexture);
            return sTexture;
        }

        public vector llGetTextureOffset(integer iFace)
        {
            var vOffset = vector.ZERO_VECTOR;
            this.Verbose("llGetTextureOffset({0})={1}", iFace, vOffset);
            return vOffset;
        }

        public Float llGetTextureRot(integer iFace)
        {
            Float fTextureRot = 0.0;
            this.Verbose("llGetTextureRot({0})={1}", iFace, fTextureRot);
            return fTextureRot;
        }

        public vector llGetTextureScale(integer iFace)
        {
            var vScale = vector.ZERO_VECTOR;
            this.Verbose("llGetTextureScale({0})={1}", iFace, vScale);
            return vScale;
        }

        public Float llGetTime()
        {
            var span = DateTime.Now.ToUniversalTime() - this.dtDateTimeScriptStarted;
            this.Verbose("llGetTime()={0}", span.TotalSeconds);
            return span.TotalSeconds;
        }

        public Float llGetTimeOfDay()
        {
            // dummy
            var fSeconds = this.llGetTime();
            this.Verbose("llGetTimeOfDay()={0}", fSeconds);
            return fSeconds;
        }

        public string llGetTimestamp()
        {
            var sTimestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            this.Verbose(@"llGetTimestamp()=""{0}""", sTimestamp);
            return sTimestamp;
        }

        public vector llGetTorque()
        {
            var vTorque = vector.ZERO_VECTOR;
            this.Verbose("llGetTorque()={0}", vTorque);
            return vTorque;
        }

        public integer llGetUnixTime()
        {
            var dtUnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            var tsSeconds = DateTime.Now.ToUniversalTime() - dtUnixEpoch;
            this.Verbose("llGetUnixTime()={0}", (int)tsSeconds.TotalSeconds);
            return (int)tsSeconds.TotalSeconds;
        }

        public integer llGetUsedMemory()
        {
            integer iUsedMemory = 65536;
            this.Verbose("llGetUsedMemory()={0}", iUsedMemory);
            return iUsedMemory;
        }

        public string llGetUsername(key kAvatarID)
        {
            // TODO Find a dummy username.
            const string sUserName = "";
            this.Verbose(@"llGetUsername({0})=""{1}""", kAvatarID, sUserName);
            return sUserName;
        }

        public vector llGetVel()
        {
            var vVelocity = vector.ZERO_VECTOR;
            this.Verbose("llGetVel()={0}", vVelocity);
            return vVelocity;
        }

        public Float llGetWallclock()
        {
            Float fSeconds = (int)DateTime.Now.AddHours(-9.0).TimeOfDay.TotalSeconds;
            this.Verbose("llGetWallclock()={0}", fSeconds);
            return fSeconds;
        }

        public void llGiveInventory(key kID, String sItemName)
        {
            this.Verbose(@"llGiveInventory({0}, ""{1}"")", kID, sItemName);
        }

        public void llGiveInventoryList(key kID, String sDirectory, list lInventory)
        {
            this.Verbose(@"llGiveInventoryList({0}, ""{1}"", {2})", kID, sDirectory, lInventory);
        }

        public integer llGiveMoney(key kAvatarID, integer iAmount)
        {
            this.Verbose("llGiveMoney({0}, {1})=0", kAvatarID, iAmount);
            return 0;
        }

        public Float llGround(vector vOffset)
        {
            Float fHeight = 25.0;
            this.Verbose("llGround({0})={1}", vOffset, fHeight);
            return fHeight;
        }

        public vector llGroundContour(vector vOffset)
        {
            var vContour = vector.ZERO_VECTOR;
            this.Verbose("llGroundContour({0})={1}", vOffset, vContour);
            return vContour;
        }

        public vector llGroundNormal(vector vOffset)
        {
            var vGroundNormal = new vector(0.0, 0.0, 1.0);
            this.Verbose("llGroundNormal({0})={1}", vOffset, vGroundNormal);
            return vGroundNormal;
        }

        public void llGroundRepel(Float fHeight, integer iWater, Float fTau)
        {
            this.Verbose("llGroundRepel({0}, {1}, {2})", fHeight, iWater, fTau);
        }

        public vector llGroundSlope(vector vOffset)
        {
            var vSlope = vector.ZERO_VECTOR;
            this.Verbose("llGroundSlope({0})={1}" + vOffset, vSlope);
            return vSlope;
        }

        public key llHTTPRequest(String sURL, list lParameters, String sBody)
        {
            var kRequestID = this.host.Http(sURL, lParameters, sBody);
            this.Verbose(@"llHTTPRequest(""{0}"", {1}, ""{2}"")=""{3}""", sURL, lParameters, sBody, kRequestID);
            return kRequestID;
        }

        public void llHTTPResponse(key kRequestID, integer iStatus, String sBody)
        {
            this.Verbose(@"llHTTPResponse({0}, {1}, ""{2}"")", kRequestID, iStatus, sBody);
        }

        public String llInsertString(String sDestination, integer iIndex, String sSource)
        {
            string strDestination = sDestination;
            string strSource = sSource;
            int intPosition = iIndex;
            string sResult;

            if (intPosition < strDestination.Length)
            {
                sResult = strDestination.Substring(0, intPosition) + strSource + strDestination.Substring(intPosition);
            }
            else if (intPosition >= 0)
            {
                sResult = strDestination + strSource;
            }
            else
            {
                sResult = "**ERROR**";
            }
            this.Verbose(@"llInsertString(""{0}"", {1}, ""{2}"")=""{3}""", sDestination, iIndex, sSource, sResult);
            return sResult;
        }

        public void llInstantMessage(key kAvatarID, String sMessage)
        {
            this.Verbose(@"llInstantMessage({0}, ""{1}"")", kAvatarID, sMessage);
        }

        public String llIntegerToBase64(integer iNumber)
        {
            var data = new byte[4];
            data[3] = (byte)(iNumber & 0xff);
            data[2] = (byte)((iNumber >> 8) & 0xff);
            data[1] = (byte)((iNumber >> 16) & 0xff);
            data[0] = (byte)((iNumber >> 24) & 0xff);
            var sResult = Convert.ToBase64String(data);
            this.Verbose(@"llIntegerToBase64({0})=""{1}""", iNumber, sResult);
            return sResult;
        }

        public list llJson2List(string sJSON)
        {
            // TODO implement conversion to list
            var lJSON = new list();
            this.Verbose("llJson2List({0})={1}", sJSON, lJSON);
            return lJSON;
        }

        public string llJsonGetValue(string sJSON, list lSpecifiers)
        {
            // TODO determine return value from list
            const string sReturn = JSON_INVALID;
            this.Verbose("llJsonGetValue({0}, {1})= {2}", sJSON, lSpecifiers, sReturn);
            return sReturn;
        }

        public string llJsonSetValue(string sJSON, list lSpecifiers, string sValue)
        {
            // TODO determine return value
            const string sReturn = JSON_INVALID;
            this.Verbose("llJsonGetValue({0}, {1}, {2})= {3}", sJSON, lSpecifiers, sValue, sReturn);
            return sReturn;
        }

        public string llJsonValueType(string sJSON, list lSpecifiers)
        {
            // TODO determine return value
            const string sReturn = JSON_INVALID;
            this.Verbose("llJsonGetValue({0}, {1})= {2}", sJSON, lSpecifiers, sReturn);
            return sReturn;
        }

        public String llKey2Name(key kID)
        {
            var sAvatarName = "";
            if (Properties.Settings.Default.AvatarKey == kID)
            {
                sAvatarName = Properties.Settings.Default.AvatarName;
            }
            this.Verbose(@"llKey2Name({0})=""{1}""", kID, sAvatarName);
            return sAvatarName;
        }

        public void llLinkParticleSystem(integer iLink, list lParameters)
        {
            this.Verbose("llLinkParticleSystem({0}, {1})", iLink, lParameters);
        }

        public void llLinkSitTarget(integer iLinkIndex, vector vOffset, rotation rRotation)
        {
            this.Verbose("llLinkSitTarget({0}, {1}, {2})", iLinkIndex, vOffset, rRotation);
        }

        public String llList2CSV(list lSource)
        {
            var sCSV = new StringBuilder();
            for (var intI = 0; intI < lSource.Count; intI++)
            {
                if (intI > 0)
                {
                    sCSV.Append(", ");
                }
                sCSV.Append(lSource[intI].ToString());
            }
            this.Verbose(@"llList2CSV({0})=""{1}""", lSource, sCSV.ToString());
            return sCSV.ToString();
        }

        public Float llList2Float(list lSource, integer iIndex)
        {
            Float fResult;
            if (iIndex < 0)
            {
                iIndex = lSource.Count + iIndex;
            }
            fResult = iIndex >= lSource.Count || iIndex < 0 ? (Float)0.0 : (Float)lSource[iIndex].ToString();
            this.Verbose("llList2Float({0}, {1})={2}", lSource, iIndex, fResult);
            return fResult;
        }

        public integer llList2Integer(list lSrc, integer iIndex)
        {
            integer iResult;
            if (iIndex < 0)
            {
                iIndex = lSrc.Count + iIndex;
            }
            iResult = iIndex >= lSrc.Count || iIndex < 0 ? (integer)0 : (integer)lSrc[iIndex].ToString();
            this.Verbose("llList2Integer({0}, {1})={2}", lSrc, iIndex, iResult);
            return iResult;
        }

        public string llList2Json(string sType, list lValues)
        {
            // TODO determine return value
            const string sReturn = JSON_INVALID;
            this.Verbose(@"llList2Json({0}, {1})=""{2}""", sType, lValues, sReturn);
            return sReturn;
        }

        public key llList2Key(list lSource, integer iIndex)
        {
            key kResult;
            if (iIndex < 0)
            {
                iIndex = lSource.Count + iIndex;
            }
            kResult = iIndex >= lSource.Count || iIndex < 0 ? key.NULL_KEY : lSource[iIndex].ToString();
            this.Verbose("llList2Key({0}, {1})={2}", lSource, iIndex, kResult);
            return kResult;
        }

        public list llList2List(list lSource, integer iStart, integer iEnd)
        {
            var iLength = lSource.Count;

            int start = iStart;
            int end = iEnd;

            var lResult = new list();

            if (this.CorrectIt(iLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (var intI = start; intI <= end; intI++)
                    {
                        lResult.Add(lSource[intI]);
                    }
                }
                else
                { // excluding
                    for (var intI = 0; intI <= end; intI++)
                    {
                        lResult.Add(lSource[intI]);
                    }
                    for (var intI = start; intI < iLength; intI++)
                    {
                        lResult.Add(lSource[intI]);
                    }
                }
            }

            this.Verbose(string.Format("List2List({0}, {1}, {2})={3}", lSource, iStart, iEnd, lResult));
            return lResult;
        }

        public list llList2ListStrided(list lSource, integer iStart, integer iEnd, integer iStride)
        {
            var iLength = lSource.Count;

            int intStart = iStart;
            int intEnd = iEnd;

            var lTemp = new list();

            if (this.CorrectIt(iLength, ref intStart, ref intEnd))
            {
                if (intStart <= intEnd)
                {
                    for (var intI = intStart; intI <= intEnd; intI++)
                    {
                        lTemp.Add(lSource[intI]);
                    }
                }
                else
                {	// excluding
                    for (var intI = 0; intI <= intEnd; intI++)
                    {
                        lTemp.Add(lSource[intI]);
                    }
                    for (var intI = intStart; intI < iLength; intI++)
                    {
                        lTemp.Add(lSource[intI]);
                    }
                }
            }
            var lResult = new list();
            var sRemark = "";
            if (iStride <= 0)
            {
                sRemark = " ** stride must be > 0 **";
            }
            else
            {
                if (intStart == 0)
                {
                    for (var intI = 0; intI < lTemp.Count; intI += iStride)
                    {
                        lResult.Add(lTemp[intI]);
                    }
                }
                else
                {
                    for (int intI = iStride - 1; intI < lTemp.Count; intI += iStride)
                    {
                        lResult.Add(lTemp[intI]);
                    }
                }
            }
            this.Verbose("llList2ListStrided({0}, {1}, {2}, {3})={4}{5}", lSource, intStart, intEnd, iStride, lResult, sRemark);
            return lResult;
        }

        public rotation llList2Rot(list lSource, integer iIndex)
        {
            rotation rResult;
            if (iIndex < 0)
            {
                iIndex = lSource.Count + iIndex;
            }
            if (iIndex >= lSource.Count || iIndex < 0)
            {
                rResult = ZERO_ROTATION;
            }
            else if (lSource[iIndex] is rotation)
            {
                rResult = (rotation)lSource[iIndex];
            }
            else
            {
                rResult = ZERO_ROTATION;
            }
            this.Verbose("llList2Rot({0}, {1})={2}", lSource, iIndex, rResult);
            return rResult;
        }

        public String llList2String(list lSource, integer iIndex)
        {
            String sResult;
            if (iIndex < 0)
            {
                iIndex = lSource.Count + iIndex;
            }
            sResult = iIndex >= lSource.Count || iIndex < 0 ? (String)"" : (String)lSource[iIndex].ToString();
            this.Verbose(@"llList2String({0}, {1})=""{2}""", lSource, iIndex, sResult);
            return sResult;
        }

        public vector llList2Vector(list lSource, integer iIndex)
        {
            vector vResult;
            if (iIndex < 0)
            {
                iIndex = lSource.Count + iIndex;
            }
            if (iIndex >= lSource.Count || iIndex < 0)
            {
                vResult = ZERO_VECTOR;
            }
            else if (lSource[iIndex] is vector)
            {
                vResult = (vector)lSource[iIndex];
            }
            else
            {
                vResult = ZERO_VECTOR;
            }
            this.Verbose("llList2Vector({0}, {1})={2}", lSource, iIndex, vResult);
            return vResult;
        }

        public integer llListFindList(list lSource, list lMatch)
        {
            if (lSource.Count == 0)
            {
                return -1;
            }
            if (lMatch.Count == 0)
            {
                return 0;
            }
            if (lMatch.Count > lSource.Count)
            {
                return -1;
            }

            var iReturn = -1;
            for (var intI = 0; intI <= (lSource.Count - lMatch.Count); intI++)
            {
                if (lMatch[0].Equals(lSource[intI]))
                {
                    var blnOkay = true;
                    for (var intJ = 1; intJ < lMatch.Count; intJ++)
                    {
                        if (!lMatch[intJ].Equals(lSource[intI + intJ]))
                        {
                            blnOkay = false;
                            break;
                        }
                    }
                    if (blnOkay)
                    {
                        iReturn = intI;
                        break;
                    }
                }
            }
            this.Verbose("llListFindList({0}, {1}={2}", lSource, lMatch, iReturn);
            return iReturn;
        }

        public list llListInsertList(list lDestination, list lSource, integer iIndex)
        {
            var intLength = lDestination.Count;
            var lResult = new list();
            if (iIndex < 0)
            {
                iIndex = lDestination.Count + iIndex;
            }

            for (var intI = 0; intI < Math.Min(iIndex, intLength); intI++)
            {
                lResult.Add(lDestination[intI]);
            }

            lResult.AddRange(lSource);

            for (var intI = Math.Max(0, iIndex); intI < intLength; intI++)
            {
                lResult.Add(lDestination[intI]);
            }

            this.Verbose("llListInsertList({0}, {1}, {2})={3}", lDestination, lSource, iIndex, lResult);
            return lResult;
        }

        public list llListRandomize(list lSource, integer iStride)
        {
            list lResult;
            var buckets = this.List2Buckets(lSource, iStride);
            lResult = buckets == null ? new list(lSource) : this.Buckets2List(this.RandomShuffle(buckets), iStride);
            this.Verbose("llListRandomize({0}, {1})={2}", lSource, iStride, lResult);
            return lResult;
        }

        // TODO check this!!!
        public list llListReplaceList(list lDestination, list lSource, integer iStart, integer iEnd)
        {
            var intLength = lDestination.Count;

            int intStart = iStart;
            int intEnd = iEnd;
            this.CorrectIt(intLength, ref intStart, ref intEnd);

            var lResult = new list();
            if (intStart <= intEnd)
            {
                for (var intI = 0; intI < intStart; intI++)
                {
                    lResult.Add(lDestination[intI]);
                }
                lResult.AddRange(lSource);
                for (var intI = intEnd + 1; intI < intLength; intI++)
                {
                    lResult.Add(lDestination[intI]);
                }
            }
            else
            {
                // where to add src?????
                for (var intI = intEnd; intI <= intStart; intI++)
                {
                    lResult.Add(lDestination[intI]);
                }
            }
            this.Verbose("llListReplaceList({0}, {1}, {2}, {3}={4}", lDestination, lSource, intStart, intEnd, lResult);
            return lResult;
        }

        public list llListSort(list lSource, integer iStride, integer iAscending)
        {
            int intAscending = iAscending;
            int intStride = iStride;
            list lResult;
            var buckets = this.List2Buckets(lSource, intStride);
            if (buckets == null)
            {
                lResult = new list(lSource);
            }
            else
            {
                buckets.Sort(new BucketComparer(intAscending));
                lResult = this.Buckets2List(buckets, intStride);
            }
            this.Verbose("llListSort({0}, {1}, {2})={3}", lSource, iStride, iAscending, lResult);
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
            var dResult = 0.0;
            double rmin, rmax;
            int operation = iOperation;
            var input = this.GetListOfNumbers(lInput);
            if (input.Count > 0)
            {
                switch (operation)
                {
                    case LIST_STAT_RANGE:
                        rmin = double.MaxValue;
                        rmax = double.MinValue;
                        for (var intI = 0; intI < input.Count; intI++)
                        {
                            if (input[intI] < rmin)
                            {
                                rmin = input[intI];
                            }
                            if (input[intI] > rmax)
                            {
                                rmax = input[intI];
                            }
                        }
                        dResult = rmax - rmin;
                        break;
                    case LIST_STAT_MIN:
                        dResult = double.MaxValue;
                        for (var intI = 0; intI < input.Count; intI++)
                        {
                            if (input[intI] < dResult)
                            {
                                dResult = input[intI];
                            }
                        }
                        break;
                    case LIST_STAT_MAX:
                        dResult = double.MinValue;
                        for (var intI = 0; intI < input.Count; intI++)
                        {
                            if (input[intI] > dResult)
                            {
                                dResult = input[intI];
                            }
                        }
                        break;
                    case LIST_STAT_MEAN:
                        for (var intI = 0; intI < input.Count; intI++)
                        {
                            dResult += input[intI];
                        }
                        dResult /= input.Count;
                        break;
                    case LIST_STAT_MEDIAN:
                        input.Sort();
                        dResult = Math.Ceiling(input.Count * 0.5) == input.Count * 0.5
                            ? input[(int)((input.Count * 0.5) - 1)] + (input[(int)(input.Count * 0.5)] / 2)
                            : input[((int)(Math.Ceiling(input.Count * 0.5))) - 1];
                        break;
                    case LIST_STAT_STD_DEV:
                        dResult = this.GetStandardDeviation(input.ToArray());
                        break;
                    case LIST_STAT_SUM:
                        for (var intI = 0; intI < input.Count; intI++)
                        {
                            dResult += input[intI];
                        }
                        break;
                    case LIST_STAT_SUM_SQUARES:
                        for (var intI = 0; intI < input.Count; intI++)
                        {
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
                        for (var intI = 0; intI < input.Count; intI++)
                        {
                            input[intI] = Math.Log(input[intI]);
                        }
                        dResult = Math.Exp(this.GetAverage(input.ToArray()));
                        break;
                    default:
                        break;
                }
            }
            this.Verbose("llListStatistics({0}, {1})={2}", iOperation, lInput, dResult);
            return dResult;
        }

        public integer llListen(integer iChannel, String sName, key kID, String sText)
        {
            var intHandle = this.host.llListen(iChannel, sName, kID, sText);
            this.Verbose(@"llListen({0}, ""{1}"", {2}, {3})={4}", iChannel, sName, kID, sText, intHandle);
            return intHandle;
        }

        public void llListenControl(integer iHandle, integer iActive)
        {
            this.Verbose("llListenControl({0}, {1})", iHandle, iActive);
            this.host.llListenControl(iHandle, iActive);
        }

        public void llListenRemove(integer iHandle)
        {
            this.Verbose("llListenRemove({0})", iHandle);
            this.host.llListenRemove(iHandle);
        }

        public void llLoadURL(key kAvatarID, String sText, String sURL)
        {
            this.Verbose(@"llLoadURL({0}, ""{1}"", ""{2}"")", kAvatarID, sText, sURL);
            var strUrl = sURL.ToString();
            if (strUrl.StartsWith("http://"))
            {
                System.Diagnostics.Process.Start(strUrl);
            }
        }

        public Float llLog(Float fValue)
        {
            var dblA = 0.0;
            if (fValue > 0.0)
            {
                dblA = Math.Log(fValue);
            }
            this.Verbose("llLog({0})={1}", fValue, dblA);
            return dblA;
        }

        public Float llLog10(Float fValue)
        {
            var dblA = 0.0;
            if (fValue > 0.0)
            {
                dblA = Math.Log10(fValue);
            }
            this.Verbose("llLog10({0})={1}", fValue, dblA);
            return dblA;
        }

        public void llLookAt(vector vTarget, Float fStrength, Float fDamping)
        {
            this.Verbose("llLookAt({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", vTarget, fStrength, fDamping);
        }

        public void llLoopSound(String sSound, Float sVolume)
        {
            try
            {
                var sp = this.host.GetSoundPlayer(sSound);
                sp.PlayLooping();
            }
            catch
            {
            }
            this.Verbose("llLoopSound({0}, {1})", sSound, sVolume);
        }

        public void llLoopSoundMaster(String sSound, Float fVolume)
        {
            try
            {
                var sp = this.host.GetSoundPlayer(sSound);
                sp.PlayLooping();
            }
            catch
            {
            }
            this.Verbose("llLoopSoundMaster({0}, {1})", sSound, fVolume);
        }

        public void llLoopSoundSlave(String sSound, Float fVolume)
        {
            try
            {
                var sp = this.host.GetSoundPlayer(sSound);
                sp.PlayLooping();
            }
            catch
            {
            }
            this.Verbose("llLoopSoundSlave({0}, {1})", sSound, fVolume);
        }

        // ok
        public String llMD5String(String sSource, integer iNonce)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            var hash = md5.ComputeHash(Encoding.ASCII.GetBytes(sSource + ":" + iNonce.ToString()));
            var sbResult = new StringBuilder();
            foreach (var hex in hash)
            {
                sbResult.Append(hex.ToString("x2"));						// convert to standard MD5 form
            }
            this.Verbose("llMD5String({0}, {1})={2}", sSource, iNonce, sbResult);
            return sbResult.ToString();
        }

        public void llMakeExplosion(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
        {
            this.Verbose(@"llMakeExplosion({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
        }

        public void llMakeFire(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
        {
            this.Verbose(@"llMakeFire({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
        }

        public void llMakeFountain(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
        {
            this.Verbose(@"llMakeFountain({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
        }

        public void llMakeSmoke(integer iParticles, Float fScale, Float fVelocity, Float fLifetime, Float fArc, String sTexture, vector vOffset)
        {
            this.Verbose(@"llMakeSmoke({0}, {1}, {2}, {3}, {4}, ""{5}"", {6})", iParticles, fScale, fVelocity, fLifetime, fArc, sTexture, vOffset);
        }

        public void llManageEstateAccess(integer iAction, key kID)
        {
            this.Verbose("llManageEstateAccess({0}, {1}", iAction, kID);
        }

        public void llMapDestination(String sLandmark, vector vPosition, vector vLookat)
        {
            this.Verbose("llMapDestination({0}, {1}, {2})", sLandmark, vPosition, vLookat);
        }

        public void llMessageLinked(integer iLink, integer iValue, String sText, key kID)
        {
            this.Verbose(@"llMessageLinked({0}, {1}, ""{2}"", {3})", iLink, iValue, sText, kID);
            this.host.MessageLinked(iLink, iValue, sText, kID);
        }

        public void llMinEventDelay(Float fDelay)
        {
            this.Verbose("llMinEventDelay({0}", fDelay);
        }

        public integer llModPow(integer iValue1, integer iValue2, integer iModulus)
        {
            var iResult = this.ModPow2(iValue1, iValue2, iModulus);
            this.Verbose("llModPow({0}, {1}, {2})={3}", iValue1, iValue2, iModulus, iResult);
            return iResult;
        }

        public void llModifyLand(integer iAction, integer iSize)
        {
            this.Verbose("llModifyLand({0}, {1})", iAction, iSize);
        }

        public void llMoveToTarget(vector vTarget, Float fTau)
        {
            this.Verbose("llMoveToTarget({0}, {1})", vTarget, fTau);
        }

        public void llNavigateTo(vector vLocation, list lOptions)
        {
            this.Verbose("llNavigateTo({0}, {1})", vLocation, lOptions);
        }

        public void llOffsetTexture(Float fOffsetS, Float fOffsetT, integer iFace)
        {
            this.Verbose("llOffsetTexture({0}, {1}, {2})", fOffsetS, fOffsetT, iFace);
        }

        public void llOpenRemoteDataChannel()
        {
            this.host.llOpenRemoteDataChannel();
            this.Verbose("llOpenRemoteDataChannel()");
        }

        public integer llOverMyLand(key kID)
        {
            var iResult = integer.TRUE;
            this.Verbose("llOverMyLand({0})={1}", kID, iResult);
            return iResult;
        }

        public void llOwnerSay(String sText)
        {
            this.Chat(0, sText, CommunicationType.OwnerSay);
        }

        public void llParcelMediaCommandList(list lCommands)
        {
            this.Verbose("llParcelMediaCommandList({0})", lCommands);
        }

        public list llParcelMediaQuery(list lQuery)
        {
            var lResult = new list();
            this.Verbose("llParcelMediaQuery({0})={1})", lQuery, lResult);
            return lResult;
        }

        // 21 sep 2007, check this
        public list llParseString2List(String sSource, list lSseparators, list lSpacers)
        {
            var lResult = this.ParseString(sSource, lSseparators, lSpacers, false);
            this.Verbose(@"llParseString2List(""{0}"", {1}, {2})={3}", sSource, lSseparators, lSpacers, lResult);
            return lResult;
        }

        // 21 sep 2007, check this, first check 3 oct 2007, last element=="" is added also
        public list llParseStringKeepNulls(String sSource, list lSeparators, list lSpacers)
        {
            var lResult = this.ParseString(sSource, lSeparators, lSpacers, true);
            this.Verbose(@"llParseStringKeepNulls(""{0}"", {1}, {2})={3}", sSource, lSeparators, lSpacers, lResult);
            return lResult;
        }

        public void llParticleSystem(list lParameters)
        {
            this.Verbose("llParticleSystem({0})", lParameters);
        }

        public void llPassCollisions(integer iPassFlag)
        {
            this.Verbose("llPassCollisions({0})", iPassFlag);
        }

        public void llPassTouches(integer iPassFlag)
        {
            this.Verbose("llPassTouches({0})", iPassFlag);
        }

        public void llPatrolPoints(list lPoints, list lOptions)
        {
            this.Verbose("llPatrolPoints({0}, {1})", lPoints, lOptions);
        }

        public void llPlaySound(String sSoundName, Float fVolume)
        {
            try
            {
                var sp = this.host.GetSoundPlayer(sSoundName);
                sp.Play();
                this.Verbose("llPlaySound({0}, {1})", sSoundName, fVolume);
            }
            catch (Exception exception)
            {
                this.Verbose("llPlaySound({0}, {1}) **{2}", sSoundName, fVolume, exception.Message);
            }
        }

        public void llPlaySoundSlave(String sSoundName, Float fVolume)
        {
            try
            {
                var sp = this.host.GetSoundPlayer(sSoundName);
                sp.Play();
            }
            catch
            {
            }
            this.Verbose("llPlaySoundSlave({0}, {1})", sSoundName, fVolume);
        }

        public void llPointAt(vector vTarget)
        {
            this.Verbose("llPointAt({0})", vTarget);
        }

        public Float llPow(Float fBase, Float fExponent)
        {
            var dblA = Math.Pow(fBase, fExponent);
            this.Verbose("llPow({0}, {1})={2}", fBase, fExponent, dblA);
            return dblA;
        }

        public void llPreloadSound(String sSoundName)
        {
            this.Verbose("llPreloadSound({0})", sSoundName);
        }

        public void llPursue(key kTargetID, list lOptions)
        {
            this.Verbose("llPursue({0}, {1})", kTargetID, lOptions);
        }

        public void llPushObject(key kID, vector vImpulse, vector vAngularImpulse, integer iLocalFlag)
        {
            this.Verbose("llPushObject({0}, {1}, {2}, {3})", kID, vImpulse, vAngularImpulse, iLocalFlag);
        }

        public void llRegionSay(integer iChannel, String sText)
        {
            if (iChannel != 0)
            {
                this.Chat(iChannel, sText, CommunicationType.RegionSay);
            }
            this.Verbose(@"llRegionSay({0}, ""{1}"")", iChannel, sText);
        }

        public void llRegionSayTo(key kTargetID, integer iChannel, String sText)
        {
            this.Verbose(@"llRegionSayTo({0}, {1}, ""{2}"")", kTargetID, iChannel, sText);
        }

        public void llReleaseCamera(key kID)
        {
            this.Verbose("llReleaseCamera({0})", kID);
        }

        public void llReleaseControls()
        {
            this.Verbose("llReleaseControls()");
            this.host.ReleaseControls();
        }

        public void llReleaseControls(key kAvatarID)
        {
            this.Verbose("llReleaseControls({0})", kAvatarID);
        }

        public void llReleaseURL(string sURL)
        {
            this.Verbose(@"llReleaseURL(""{0}"")", sURL);
        }

        public void llRemoteDataReply(key kChannel, key kMessageID, String sData, integer iData)
        {
            this.host.llRemoteDataReply(kChannel, kMessageID, sData, iData);
            this.Verbose("llRemoteDataReply({0}, {1}, {2}, {3})", kChannel, kMessageID, sData, iData);
        }

        public void llRemoteDataSetRegion()
        {
            this.Verbose("llRemoteDataSetRegion()");
        }

        public void llRemoteLoadScript(key kTargetID, String sName, integer iRunningFlag, integer iStartParameter)
        {
            this.Verbose(@"llRemoteLoadScript({0}, ""{1}"", {2}, {3})", kTargetID, sName, iRunningFlag, iStartParameter);
        }

        public void llRemoteLoadScriptPin(key kTargetID, String sName, integer iPIN, integer iRunningFlag, integer iStartParameter)
        {
            this.Verbose(@"llRemoteLoadScriptPin({0}, ""{1}"", {2}, {3}, {4})", kTargetID, sName, iPIN, iRunningFlag, iStartParameter);
        }

        public void llRemoveFromLandBanList(key kAvatarID)
        {
            this.Verbose("llRemoveFromLandBanList({0})", kAvatarID);
            if (this.htLandBanList.ContainsKey(kAvatarID))
            {
                this.htLandBanList.Remove(kAvatarID);
            }
        }

        public void llRemoveFromLandPassList(key kAvatarID)
        {
            this.Verbose("llRemoveFromLandPassList({0})", kAvatarID);
            if (this.htLandPassList.ContainsKey(kAvatarID))
            {
                this.htLandPassList.Remove(kAvatarID);
            }
        }

        public void llRemoveInventory(String sInventoryItem)
        {
            this.host.RemoveInventory(sInventoryItem);
            this.Verbose("llRemoveInventory({0})", sInventoryItem);
        }

        public void llRemoveVehicleFlags(integer iFlags)
        {
            this.Verbose("llRemoveVehicleFlags({0})", iFlags);
        }

        public key llRequestAgentData(key kID, integer iDataConstant)
        {
            var kResult = new key(Guid.NewGuid());

            var strData = "***";
            switch (iDataConstant)
            {
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
            this.host.ExecuteSecondLife("dataserver", kResult, (SecondLife.String)strData);
            this.Verbose("llRemoveVehicleFlags({0}, {1})={2}", kID, iDataConstant, kResult);
            return kResult;
        }

        public key llRequestDisplayName(key kAvatarID)
        {
            var kID = new key(Guid.NewGuid());
            const string strData = "dummyDisplay Name";
            this.host.ExecuteSecondLife("dataserver", kID, (SecondLife.String)strData);
            this.Verbose("llRequestDisplayName({0})={1}", kAvatarID, kID);
            return kID;
        }

        public key llRequestInventoryData(String sName)
        {
            var kResult = new key(Guid.NewGuid());
            this.Verbose(@"llRequestInventoryData(""{0}"")={1}", sName, kResult);
            return kResult;
        }

        public void llRequestPermissions(key kAvatarID, integer iPermissionFlags)
        {
            this.Verbose("llRequestPermissions({0}, {1})", kAvatarID, iPermissionFlags);
            this.host.llRequestPermissions(kAvatarID, iPermissionFlags);
        }

        public key llRequestSecureURL()
        {
            var kResult = new key(Guid.NewGuid());
            this.Verbose("llRequestPermissions()={0}", kResult);
            return kResult;
        }

        public key llRequestSimulatorData(String sSimulator, integer iData)
        {
            var kResult = new key(Guid.NewGuid());
            this.Verbose(@"llRequestSimulatorData(""{0}"", {1})={2}", sSimulator, iData, kResult);
            return kResult;
        }

        public key llRequestURL()
        {
            var kResult = new key(Guid.NewGuid());
            this.Verbose("llRequestURL()={0}", kResult);
            return kResult;
        }

        public key llRequestUsername(key kAvatarID)
        {
            var kID = new key(Guid.NewGuid());
            const string strData = "dummyUser Name";
            this.Verbose("llRequestDisplayName({0})={1}", kAvatarID, kID);
            this.host.ExecuteSecondLife("dataserver", kID, (SecondLife.String)strData);
            return kID;
        }

        public void llResetAnimationOverride(String sAnimationState)
        {
            this.Verbose("llResetAnimationOverride({0})", sAnimationState);
        }

        public void llResetLandBanList()
        {
            this.htLandBanList = new Hashtable();
            this.Verbose("llResetLandBanList()");
        }

        public void llResetLandPassList()
        {
            this.htLandPassList = new Hashtable();
            this.Verbose("llResetLandPassList()");
        }

        public void llResetOtherScript(String sScriptName)
        {
            this.Verbose("llResetOtherScript({0})", sScriptName);
        }

        public void llResetScript()
        {
            this.Verbose("llResetScript()");
            this.host.Reset();
            System.Threading.Thread.Sleep(1000);
            System.Windows.Forms.MessageBox.Show("If you see this, something wrong in llResetScript()", "Oops...");
        }

        public void llResetTime()
        {
            this.Verbose("llResetTime()");
            this.dtDateTimeScriptStarted = DateTime.Now.ToUniversalTime();
        }

        public integer llReturnObjectsByID(list lObjects)
        {
            integer iReturned = ERR_GENERIC;
            this.Verbose("llReturnObjectsByID({0})={1}", lObjects, iReturned);
            return iReturned;
        }

        public integer llReturnObjectsByOwner(key kID, integer iScope)
        {
            integer iReturned = ERR_GENERIC;
            this.Verbose("llReturnObjectsByOwner({0}, {1})={2}", kID, iScope, iReturned);
            return iReturned;
        }

        public void llRezAtRoot(String sInventoryItem, vector vPosition, vector vVelocity, rotation rRotation, integer iParameter)
        {
            this.Verbose(@"llRezAtRoot(""{0}"", {1}, {2}, {3}, {4})", sInventoryItem, vPosition, vVelocity, rRotation, iParameter);
        }

        public void llRezObject(String sInventoryItem, vector vPosition, vector vVelocity, rotation rRotation, integer iParameter)
        {
            this.Verbose(@"llRezObject(""{0}"", {1}, {2}, {3}, {4})", sInventoryItem, vPosition, vVelocity, rRotation, iParameter);
            this.object_rez(new key(Guid.NewGuid()));
            this.on_rez(iParameter);
        }

        public Float llRot2Angle(rotation rRotation)
        {
            Float fAngle = 0.0F;
            this.Verbose("llRot2Angle({0})={1}", rRotation, fAngle);
            return fAngle;
        }

        public vector llRot2Axis(rotation rRotation)
        {
            var vAxis = vector.ZERO_VECTOR;
            this.Verbose("llRot2Axis({0})={1})", rRotation, vAxis);
            return vAxis;
        }

        public vector llRot2Euler(rotation rRotation)
        {
            // http://rpgstats.com/wiki/index.php?title=LibraryRotationFunctions
            var t = new rotation(rRotation.x * rRotation.x, rRotation.y * rRotation.y, rRotation.z * rRotation.z, rRotation.s * rRotation.s);
            double m = t.x + t.y + t.z + t.s;
            var vEuler = new vector(0, 0, 0);
            if (m != 0)
            {
                double n = 2 * ((rRotation.y * rRotation.s) + (rRotation.x * rRotation.z));
                var p = (m * m) - (n * n);
                if (p > 0)
                {
                    vEuler = new vector(
                        Math.Atan2(2.0 * ((rRotation.x * rRotation.s) - (rRotation.y * rRotation.z)), -t.x - t.y + t.z + t.s),
                        Math.Atan2(n, Math.Sqrt(p)),
                        Math.Atan2(2.0 * ((rRotation.z * rRotation.s) - (rRotation.x * rRotation.y)), t.x - t.y - t.z + t.s));
                }
                else if (n > 0)
                {
                    vEuler = new vector(0, PI_BY_TWO, Math.Atan2((rRotation.z * rRotation.s) + (rRotation.x * rRotation.y), 0.5 - t.x - t.z));
                }
                else
                {
                    vEuler = new vector(0, -PI_BY_TWO, Math.Atan2((rRotation.z * rRotation.s) + (rRotation.x * rRotation.y), 0.5 - t.x - t.z));
                }
            }
            this.Verbose("llRot2Euler({0})={1}", rRotation, vEuler);
            return vEuler;
        }

        public vector llRot2Fwd(rotation rRotation)
        {
            var v = new vector(1.0 / ((rRotation.x * rRotation.x) + (rRotation.y * rRotation.y) + (rRotation.z * rRotation.z) + (rRotation.s * rRotation.s)), 0, 0);
            var vResult = v * rRotation;
            this.Verbose("llRot2Fwd({0})={1}", rRotation, vResult);
            return vResult;
        }

        public vector llRot2Left(rotation rRotation)
        {
            var v = new vector(0, 1.0 / ((rRotation.x * rRotation.x) + (rRotation.y * rRotation.y) + (rRotation.z * rRotation.z) + (rRotation.s * rRotation.s)), 0);
            var vResult = v * rRotation;
            this.Verbose("llRot2Left({0})={1}", rRotation, vResult);
            return vResult;
        }

        public vector llRot2Up(rotation rRotation)
        {
            var v = new vector(0, 0, 1.0 / ((rRotation.x * rRotation.x) + (rRotation.y * rRotation.y) + (rRotation.z * rRotation.z) + (rRotation.s * rRotation.s)));
            var vResult = v * rRotation;
            this.Verbose("llRot2Left({0})={1}", rRotation, vResult);
            return vResult;
        }

        public void llRotateTexture(Float fRadians, integer iFace)
        {
            this.Verbose("llRotateTexture({0}, {1})", fRadians, iFace);
        }

        public rotation llRotBetween(vector vDirection1, vector vDirection2)
        {
            var rResult = rotation.ZERO_ROTATION;
            this.Verbose("llRotBetween({0}, {1})={2}", vDirection1, vDirection2, rResult);
            return rResult;
        }

        public void llRotLookAt(rotation rRotation, Float fStrength, Float fDamping)
        {
            this.Verbose("llRotLookAt({0}, {1}, {2})", rRotation, fStrength, fDamping);
        }

        public integer llRotTarget(rotation rRotation, Float fError)
        {
            integer iHandle = 0;
            this.Verbose("llRotTarget({0}, {1})={2})", rRotation, fError, iHandle);
            return iHandle;
        }

        public void llRotTargetRemove(integer iHandle)
        {
            this.Verbose("llRotTargetRemove({0})", iHandle);
        }

        public integer llRound(Float fValue)
        {
            var intA = (int)Math.Round(fValue);
            this.Verbose("llRound({0})={1})=", fValue, intA);
            return intA;
        }

        public integer llSameGroup(key kID)
        {
            integer iFlag = 0;
            this.Verbose("llSameGroup({0})={1})", kID, iFlag);
            return iFlag;
        }

        public void llSay(integer iChannel, String sText)
        {
            this.Chat(iChannel, sText, CommunicationType.Say);
        }

        public void llScaleTexture(Float fScaleS, Float fScaleT, integer iFace)
        {
            this.Verbose("llScaleTexture({0}, {1}, {2})", fScaleS, fScaleT, iFace);
        }

        public integer llScriptDanger(vector vPosition)
        {
            integer iFlag = 0;
            this.Verbose("llScriptDanger({0})={1}", vPosition, iFlag);
            return iFlag;
        }

        public void llScriptProfiler(integer iState)
        {
            this.Verbose("llScriptProfiler({0})", iState);
        }

        public key llSendRemoteData(key kChannel, String sDestination, integer iData, String sData)
        {
            var kID = this.host.llSendRemoteData(kChannel, sDestination, iData, sData);
            this.Verbose("llSendRemoteData({0}, {1}, {2}, {3})={4}", kChannel, sDestination, iData, sData, kID);
            return kID;
        }

        public void llSensor(String sName, key kID, integer iType, Float fRange, Float fArc)
        {
            this.Verbose(@"llSensor(""{0}"", {1}, {2}, {3}, {4})", sName, kID, iType, fRange, fArc);
            this.host.SensorTimer.Stop();
            integer iTotalNumber = 1;
            this.host.ExecuteSecondLife("sensor", iTotalNumber);
        }

        public void llSensorRemove()
        {
            this.Verbose("llSensorRemove()");
            this.host.SensorTimer.Stop();
        }

        public void llSensorRepeat(String sName, key kID, integer iType, Float fRange, Float fArc, Float fRate)
        {
            this.Verbose(@"llSensorRepeat(""{0}"", {1}, {2}, {3}, {4}, {5})", sName, kID, iType, fRange, fArc, fRate);
            this.host.SensorTimer.Stop();
            if (fRate > 0)
            {
                this.host.SensorTimer.Interval = (int)Math.Round(fRate * 1000);
                this.host.SensorTimer.Start();
            }
        }

        public void llSetAlpha(Float fOpacity, integer iFace)
        {
            this.Verbose("llSetAlpha({0}, {1})", fOpacity, iFace);
        }

        public void llSetAngularVelocity(vector vForce, integer iLocal)
        {
            this.Verbose("llSetAngularVelocity({0}, {1})", vForce, iLocal);
        }

        public void llSetAnimationOverride(String sAnimationState, String sAnimation)
        {
            this.Verbose("llSetAnimationOverride({0}, {1})", sAnimationState, sAnimation);
        }

        public void llSetBuoyancy(Float fBuoyancy)
        {
            this.Verbose("llSetBuoyancy({0})", fBuoyancy);
        }

        public void llSetCameraAtOffset(vector vOffset)
        {
            this.Verbose("llSetCameraAtOffset({0})", vOffset);
        }

        public void llSetCameraEyeOffset(vector vOffset)
        {
            this.Verbose("llSetCameraEyeOffset({0})", vOffset);
        }

        public void llSetCameraParams(list lRules)
        {
            this.Verbose("llSetCameraParams({0})", lRules);
        }

        public void llSetClickAction(integer iAction)
        {
            this.Verbose("llSetClickAction({0})", iAction);
        }

        public void llSetColor(vector vColour, integer iFace)
        {
            this.Verbose("llSetColor({0}, {1})", vColour, iFace);
        }

        public void llSetContentType(key kHTTPRequestID, integer iContentType)
        {
            this.Verbose("llSetContentType({0}, {1})", kHTTPRequestID, iContentType);
        }

        public void llSetDamage(Float fDamage)
        {
            this.Verbose("llSetDamage({0})", fDamage);
        }

        public void llSetForce(vector vForce, integer iLocal)
        {
            this.Verbose("llSetForce({0}, {1})", vForce, iLocal);
        }

        public void llSetForceAndTorque(vector vForce, vector vTorque, integer iLocal)
        {
            this.Verbose("llSetForceAndTorque({0}, {1}, {2})", vForce, vTorque, iLocal);
        }

        public void llSetHoverHeight(Float fHeight, Float fWater, Float fTau)
        {
            this.Verbose("llSetHoverHeight({0}, {1}, {2})", fHeight, fWater, fTau);
        }

        public void llSetInventoryPermMask(String sItem, integer iMask, integer iValue)
        {
            this.Verbose(@"llSetInventoryPermMask(""{0}"", {1}, {2})", sItem, iMask, iValue);
        }

        public void llSetKeyframedMotion(list lKeyframes, list lOptions)
        {
            this.Verbose("llSetKeyframedMotion({0}, {1})", lKeyframes, lOptions);
        }

        public void llSetLinkAlpha(integer iLinkIndex, Float fAlpha, integer iFace)
        {
            this.Verbose("llSetLinkAlpha({0}, {1}, {2})", iLinkIndex, fAlpha, iFace);
        }

        public void llSetLinkCamera(integer iLinkIndex, vector vEyeOffset, vector vLookOffset)
        {
            this.Verbose("llSetLinkCamera({0}, {1}, {2})", iLinkIndex, vEyeOffset, vLookOffset);
        }

        public void llSetLinkColor(integer iLinkIndex, vector vColour, integer iFace)
        {
            this.Verbose("llSetLinkColor({0}, {1}, {2})", iLinkIndex, vColour, iFace);
        }

        public integer llSetLinkMedia(integer iLinkIndex, integer iFace, list lParameters)
        {
            integer iResult = STATUS_OK;
            this.Verbose("llSetLinkMedia({0}, {1}, {2})={3}", iLinkIndex, iFace, lParameters, iResult);
            return iResult;
        }

        public void llSetLinkPrimitiveParams(integer iLinkIndex, list lRules)
        {
            this.Verbose("llSetLinkPrimitiveParams({0}, {1})", iLinkIndex, lRules);
        }

        public void llSetLinkPrimitiveParamsFast(integer iLinkIndex, list lRules)
        {
            this.Verbose("llSetLinkPrimitiveParamsFast({0}, {1})", iLinkIndex, lRules);
        }

        public void llSetLinkTexture(integer iLinkIndex, String sTexture, integer iFace)
        {
            this.Verbose(@"llSetLinkTexture({0}, ""{1}"", {2})", iLinkIndex, sTexture, iFace);
        }

        public void llSetLinkTextureAnim(integer iLinkIndex, integer iMode, integer iFace, integer iSizeX, integer iSizeY, Float fStart, Float fLength, Float fRate)
        {
            this.Verbose("llSetLinkTextureAnim({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", iLinkIndex, iMode, iFace, iSizeX, iSizeY, fStart, fLength, fRate);
        }

        public void llSetLocalRot(rotation rRotation)
        {
            this.rRotationlocal = rRotation;
            this.Verbose("llSetLocalRot({0})", rRotation);
        }

        public integer llSetMemoryLimit(integer iLimit)
        {
            integer iResult = true;
            this.Verbose("llSetMemoryLimit({0})=(1)", iLimit, iResult);
            return iResult;
        }

        public void llSetObjectDesc(String sDescription)
        {
            this.Verbose(@"llSetObjectDesc(""{0}"")", sDescription);
            this.host.SetObjectDescription(sDescription);
        }

        public void llSetObjectName(String sName)
        {
            this.Verbose(@"llSetObjectName(""{0}"")", sName);
            this.host.SetObjectName(sName);
        }

        public void llSetObjectPermMask(integer iMaskConstant, integer iValue)
        {
            this.Verbose("llSetObjectPermMask({0}, {1})", iMaskConstant, iValue);
        }

        public void llSetParcelMusicURL(String sURL)
        {
            this.Verbose(@"llSetParcelMusicURL(""{0}"")", sURL);
            this.sParcelMusicURL = sURL;
        }

        public void llSetPayPrice(integer iPrice, list lButtons)
        {
            this.Verbose("llSetPayPrice({0}, {1})", iPrice, lButtons);
        }

        public void llSetPhysicsMaterial(integer iMaterialBits, Float fGravityMultiplier, Float fRestitution, Float fFriction, Float fDensity)
        {
            this.Verbose("llSetPhysicsMaterial({0}, {1}, {2}, {3}, {4})", iMaterialBits, fGravityMultiplier, fRestitution, fFriction, fDensity);
        }

        public void llSetPos(vector vPosition)
        {
            this.Verbose("llSetPos({0})", vPosition);
            this.vPosition = vPosition;
        }

        public integer llSetPrimMediaParams(integer iFace, list lParameters)
        {
            integer iResult = 0;
            this.Verbose("llSetPrimMediaParams({0}, {1})={2}", iFace, lParameters, iResult);
            return iResult;
        }

        public void llSetPrimitiveParams(list lRule)
        {
            this.Verbose("llSetPrimitiveParams({0})", lRule);
        }

        public integer llSetRegionPos(vector vPosition)
        {
            integer iResult = true;
            this.Verbose("llSetRegionPos({0})={1}", vPosition, iResult);
            this.vPosition = vPosition;
            return iResult;
        }

        public void llSetRemoteScriptAccessPin(integer iPIN)
        {
            this.Verbose("llSetRemoteScriptAccessPin({0})", iPIN);
        }

        public void llSetRot(rotation rRotation)
        {
            this.Verbose("llSetRot({0})", rRotation);
            this.rRotation = rRotation;
        }

        public void llSetScale(vector vScale)
        {
            this.Verbose("llSetScale({0})", vScale);
            this.vScale = vScale;
        }

        public void llSetScriptState(String sName, integer iRunState)
        {
            this.Verbose(@"llSetScriptState(""{0}"", {1})", sName, iRunState);
        }

        public void llSetSitText(String sText)
        {
            this.Verbose(@"llSetSitText(""{0}"")", sText);
            this.sSitText = sText;
        }

        public void llSetSoundQueueing(integer iQueueFlag)
        {
            this.Verbose("llSetSoundQueueing({0})", iQueueFlag);
        }

        public void llSetSoundRadius(Float fRadius)
        {
            this.fSoundRadius = fRadius;
            this.Verbose("llSetSoundRadius({0})", fRadius);
        }

        public void llSetStatus(integer iStatusConstant, integer iValue)
        {
            this.Verbose("llSetStatus({0}, {1})", iStatusConstant, iValue);
        }

        public void llSetText(String sText, vector vColour, Float fOpacity)
        {
            this.Verbose(@"llSetText(""{0}"", {1}, {2})", sText, vColour, fOpacity);
        }

        public void llSetTexture(String sTextureName, integer iFace)
        {
            this.Verbose(@"llSetTexture(""{0}"", {1})", sTextureName, iFace);
        }

        public void llSetTextureAnim(integer iMode, integer iFace, integer iSizeX, integer iSizeY, Float fStart, Float fLength, Float fRate)
        {
            this.Verbose("llSetTextureAnim({0}, {1}, {2}, {3}, {4}, {5}, {6}, {7})", iMode, iFace, iSizeX, iSizeY, fStart, fLength, fRate);
        }

        public void llSetTimerEvent(Float fSeconds)
        {
            this.Verbose("llSetTimerEvent({0})", fSeconds);
            this.host.Timer.Stop();
            if (fSeconds > 0)
            {
                this.host.Timer.Interval = (int)Math.Round(fSeconds * 1000);
                this.host.Timer.Start();
            }
        }

        public void llSetTorque(vector vTorque, integer iLocal)
        {
            this.Verbose("llSetTorque({0}, {1})", vTorque, iLocal);
        }

        public void llSetTouchText(String sText)
        {
            this.Verbose(@"llSetTouchText(""{0}"")", sText);
        }

        public void llSetVehicleFlags(integer iVehicleFlagConstants)
        {
            this.Verbose("llSetVehicleFlags({0})", iVehicleFlagConstants);
        }

        public void llSetVehicleFloatParam(integer iParameterName, Float fParameterValue)
        {
            this.Verbose("llSetVehicledoubleParam({0}, {1})", iParameterName, fParameterValue);
        }

        public void llSetVehicleRotationParam(integer iParameterName, rotation fParameterValue)
        {
            this.Verbose("llSetVehicleRotationParam({0}, {1})", iParameterName, fParameterValue);
        }

        public void llSetVehicleType(integer iTypeConstant)
        {
            this.Verbose("llSetVehicleType({0})", iTypeConstant);
        }

        public void llSetVehicleVectorParam(integer iParameterName, vector vParameterValue)
        {
            this.Verbose("llSetVehicleVectorParam({0}, {1})", iParameterName, vParameterValue);
        }

        public void llSetVelocity(vector vForce, integer iLocal)
        {
            this.Verbose("llSetVelocity({0}, {1})", vForce, iLocal);
        }

        public String llSHA1String(String sSource)
        {
            var buffer = System.Text.Encoding.UTF8.GetBytes(sSource.ToString());
            var cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            var sHash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
            this.Verbose(@"llSHA1String(""{0"")=""{1}""", sSource, sHash);
            return sHash;
        }

        public void llShout(integer iChannel, String sText)
        {
            this.Chat(iChannel, sText, CommunicationType.Shout);
        }

        public Float llSin(Float fTheta)
        {
            var dblA = Math.Sin(fTheta);
            this.Verbose("llSin({0})={1}", fTheta, dblA);
            return dblA;
        }

        public void llSitTarget(vector vOffset, rotation rRotation)
        {
            this.Verbose("llSitTarget({0}, {1})", vOffset, rRotation);
        }

        public void llSleep(Float fSeconds)
        {
            this.Verbose("llSleep({0})", fSeconds);
            System.Threading.Thread.Sleep((int)Math.Round(fSeconds * 1000));
        }

        public Float llSqrt(Float fValue)
        {
            var dblA = Math.Sqrt(fValue);
            this.Verbose("llSqrt({0})={1}", fValue, dblA);
            return dblA;
        }

        public void llStartAnimation(String sAnimationName)
        {
            this.Verbose(@"llStartAnimation(""{0}"")", sAnimationName);
        }

        public void llStopAnimation(String sAnimationName)
        {
            this.Verbose(@"llStopAnimation(""{0}"")", sAnimationName);
        }

        public void llStopHover()
        {
            this.Verbose("llStopHover()");
        }

        public void llStopLookAt()
        {
            this.Verbose("llStopLookAt()");
        }

        public void llStopMoveToTarget()
        {
            this.Verbose("llStopMoveToTarget()");
        }

        public void llStopPointAt()
        {
            this.Verbose("llStopPointAt()");
        }

        public void llStopSound()
        {
            this.Verbose("llStopSound()");
        }

        public integer llStringLength(String sSource)
        {
            var intLength = ((string)sSource).Length;
            this.Verbose(@"llStringLength(""{0}"")={1}", sSource, intLength);
            return intLength;
        }

        public String llStringToBase64(String sText)
        {
            var sResult = this.StringToBase64(sText.ToString());
            this.Verbose(@"llStringToBase64(""{0}"")=""{1}""", sText, sResult);
            return sResult;
        }

        public String llStringTrim(String sText, integer iTrimType)
        {
            var strResult = sText.ToString();

            if ((iTrimType & STRING_TRIM_HEAD) != 0)
            {
                strResult = strResult.TrimStart();
            }

            if ((iTrimType & STRING_TRIM_TAIL) != 0)
            {
                strResult = strResult.TrimEnd();
            }

            this.Verbose(@"llStringTrim(""{0}"", {1})=""{2}""", sText, iTrimType, strResult);
            return strResult;
        }

        public integer llSubStringIndex(String sSource, String sPattern)
        {
            var intIndex = ((string)sSource).IndexOf(sPattern);
            this.Verbose("llSubStringIndex({0}, {1})={2}", sSource, sPattern, intIndex);
            return intIndex;
        }

        public void llTakeCamera(key kAvatarID)
        {
            this.Verbose("llTakeCamera({0})", kAvatarID);
        }

        public void llTakeControls(integer iControls, integer iAcceptFlag, integer iPassOnFlag)
        {
            this.Verbose("llTakeControls({0}, {1}), {2})", iControls, iAcceptFlag, iPassOnFlag);
            this.host.TakeControls(iControls, iAcceptFlag, iPassOnFlag);
        }

        public Float llTan(Float fTheta)
        {
            var dblA = Math.Tan(fTheta);
            this.Verbose("llTan({0})={1}", fTheta, dblA);
            return dblA;
        }

        public integer llTarget(vector vPosition, Float fRange)
        {
            integer iResult = 0;
            this.Verbose("llTarget({0}, {1})={2}", vPosition, fRange, iResult);
            return iResult;
        }

        public void llTargetOmega(vector vAxis, Float fSpinRate, Float fGain)
        {
            this.Verbose("llTargetOmega({0}, {1}, {2})", vAxis, fSpinRate, fGain);
        }

        public void llTargetRemove(integer iTargetHandle)
        {
            this.Verbose("llTargetRemove({0})", iTargetHandle);
        }

        public void llTeleportAgent(key kAvatarID, String sLandmarkName, vector vLandingPoint, vector vLookAtPoint)
        {
            this.Verbose(@"llTeleportAgentHome({0}, ""{1}"", {2}, {3})", kAvatarID, sLandmarkName, vLandingPoint, vLookAtPoint);
        }

        public void llTeleportAgentGlobalCoords(key kAvatarID, vector vGlobalPosition, vector vRegionPosition, vector iLookAtPoint)
        {
            this.Verbose("llTeleportAgentHome({0}, {1}, {2}, {3})", kAvatarID, vGlobalPosition, vRegionPosition, iLookAtPoint);
        }

        public void llTeleportAgentHome(key kAvatarID)
        {
            this.Verbose("llTeleportAgentHome({0})", kAvatarID);
        }

        public void llTextBox(key kAvatar, String sText, integer iChannel)
        {
            this.Verbose(@"llTextBox({0}, ""{1}"", {2})", kAvatar, sText, iChannel);
            this.host.llTextBox(kAvatar, sText, iChannel);
        }

        public String llToLower(String sText)
        {
            var strTemp = ((string)sText).ToLower();
            this.Verbose(@"llToLower(""{0}"")=""{1}""", sText, strTemp);
            return strTemp;
        }

        public String llToUpper(String sText)
        {
            var strTemp = ((string)sText).ToUpper();
            this.Verbose(@"llToUpper(""{0}"")=""{1}""", sText, strTemp);
            return strTemp;
        }

        public key llTransferLindenDollars(key kPayee, integer iAmount)
        {
            var kID = new key(Guid.NewGuid());
            var strData = kPayee.ToString() + "," + iAmount.ToString();
            this.Verbose("llTransferLindenDollars({0}, {1})", kPayee, iAmount);
            this.host.ExecuteSecondLife("transaction_result", kID, true, (SecondLife.String)strData);
            return kID;
        }

        public void llTriggerSound(String sSoundName, Float fVolume)
        {
            this.Verbose(@"llTriggerSound(""{0}"", {1})", sSoundName, fVolume);
        }

        public void llTriggerSoundLimited(String sSoundName, Float fVolume, vector vBoxNE, vector vBoxSW)
        {
            this.Verbose("llTriggerSoundLimited({0}, {1}, {2}, {3})", sSoundName, fVolume, vBoxNE, vBoxSW);
        }

        public String llUnescapeURL(String sURL)
        {
            var data = Encoding.UTF8.GetBytes(sURL.ToString());
            var list = new List<byte>();
            for (var intI = 0; intI < data.Length; intI++)
            {
                var chrC = data[intI];
                if (chrC == (byte)'%')
                {
                    if (intI < (data.Length - 2))
                    {
                        list.Add((byte)(this.HexToInt(data[intI + 1]) << 4 | this.HexToInt(data[intI + 2])));
                    }
                    intI += 2;
                }
                else
                {
                    list.Add(chrC);
                }
            }
            data = list.ToArray();
            var intLen = Array.IndexOf(data, (byte)0x0);
            if (intLen < 0)
            {
                intLen = data.Length;
            }
            var strTmp = Encoding.UTF8.GetString(data, 0, intLen);
            this.Verbose(string.Format(@"llUnescapeURL(""{0}"")=""{1}""", sURL, strTmp));
            return strTmp;
        }

        public void llUnSit(key kAvatarID)
        {
            this.Verbose("llUnSit({0})", kAvatarID);
        }

        public void llUpdateCharacter(list lOptions)
        {
            this.Verbose("llUpdateCharacter({0})", lOptions);
        }

        public Float llVecDist(vector vPostionA, vector vPositionB)
        {
            var vecValue = new vector(vPostionA.x - vPositionB.x, vPostionA.y - vPositionB.y, vPostionA.z - vPositionB.z);
            double dblMag = this.llVecMag(vecValue);
            this.Verbose("llVecDist({0}, {1})={2}", vPostionA, vPositionB, dblMag);
            return dblMag;
        }

        public Float llVecMag(vector vVector)
        {
            var dblValue = Math.Sqrt((vVector.x * vVector.x) + (vVector.y * vVector.y) + (vVector.z * vVector.z));
            this.Verbose("llVecMag({0})={1}", vVector, dblValue);
            return dblValue;
        }

        public vector llVecNorm(vector vVector)
        {
            double dblMag = this.llVecMag(vVector);
            var vecValue = new vector(vVector.x / dblMag, vVector.y / dblMag, vVector.z / dblMag);
            this.Verbose("llVecNorm({0})={1}", vVector, vecValue);
            return vecValue;
        }

        public void llVolumeDetect(integer iDetectFlag)
        {
            this.Verbose("llVolumeDetect({0})", iDetectFlag);
        }

        public void llWanderWithin(vector vOrigin, vector vDistance, list lOptions)
        {
            this.Verbose("llWanderWithin({0}, {1}, {2})", vOrigin, vDistance, lOptions);
        }

        public Float llWater(vector vOffset)
        {
            Float fWaterLevel = 0.0F;
            this.Verbose("llWater({0})={1}", vOffset, fWaterLevel);
            return fWaterLevel;
        }

        public void llWhisper(integer iChannel, String sText)
        {
            this.Chat(iChannel, sText, CommunicationType.Whisper);
        }

        public vector llWind(vector vOffset)
        {
            var vDirection = vector.ZERO_VECTOR;
            this.Verbose("llWind({0})={1}", vOffset, vDirection);
            return vDirection;
        }

        public String llXorBase64(String sText1, String sText2)
        {
            const string sResult = "";
            this.Verbose(@"llXorBase64(""{0}"",""{1}"")=""{2}""", sText1, sText2, sResult);
            return sResult;
        }

        public String llXorBase64Strings(String sText1, String sText2)
        {
            const string sResult = "";
            this.Verbose(@"llXorBase64Strings(""{0}"",""{1}"")=""{2}""", sText1, sText2, sResult);
            return sResult;
        }

        public String llXorBase64StringsCorrect(String sText1, String sText2)
        {
            var strS1 = this.Base64ToString(sText1.ToString());
            var strS2 = this.Base64ToString(sText2.ToString());
            var intLength = strS1.Length;
            if (strS2.Length == 0)
            {
                strS2 = " ";
            }
            while (strS2.Length < intLength)
            {
                strS2 += strS2;
            }
            strS2 = strS2.Substring(0, intLength);
            var sb = new StringBuilder();
            for (var intI = 0; intI < intLength; intI++)
            {
                sb.Append((char)(strS1[intI] ^ strS2[intI]));
            }
            var sResult = this.StringToBase64(sb.ToString());
            this.Verbose(@"llXorBase64StringsCorrect(""{0}"",""{1}"")=""{2}""", sText1, sText2, sResult);
            return sResult;
        }
    }
}
