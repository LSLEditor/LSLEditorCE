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
// <summary>
// All OSSL functions implemented.
// </summary>


using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace LSLEditor
{
    /// <summary>
	/// This part of the SecondLife class contains the OSSL function definitions.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "These are all OSSL functions, the documentation is in the OSSL Wiki.")]
    public partial class SecondLife
    {
        public key osAgentSaveAppearance(key avatarId, String notecard)
        {
            if (avatarId != "")
            {
                key k = new key(Guid.NewGuid());
                Verbose("osAgentSaveAppearance(\"{0}\", \"{1}\")=\"{2}\"", avatarId, notecard, k);
                return k;
            }
            Verbose("osAgentSaveAppearance(\"{0}\", \"{1}\")=\"{2}\"", avatarId, notecard, "");
            return key.NULL_KEY;
        }

        public String osAvatarName2Key(String firstname, String lastname)
        {
            if(Properties.Settings.Default.AvatarName == (firstname + " " + lastname))
            {
                Verbose("osAvatarName2Key(\"{0}\", \"{1}\")=\"{2}\"", firstname, lastname, Properties.Settings.Default.AvatarKey.ToString());
                return Properties.Settings.Default.AvatarKey.ToString();
            }

            if (!(firstname == "" && lastname == ""))
            {
                key k = new key(Guid.NewGuid());
                Verbose("osAvatarName2Key(\"{0}\", \"{1}\")=\"{2}\"", firstname, lastname, k);
                return k.ToString();
            }
            Verbose("osAvatarName2Key(\"{0}\", \"{1}\")=\"{2}\"", firstname, lastname, "");
            return key.NULL_KEY.ToString();
        }

        public void osAvatarPlayAnimation(key avatar, String animation)
        {
            Verbose("osAvatarPlayAnimation(\"{0}\", \"{1}\")", avatar, animation);
        }

        public void osAvatarStopAnimation(key avatar, String animation)
        {
            Verbose("osAvatarStopAnimation(\"{0}\", \"{1}\")", avatar, animation);
        }

        public void osCauseDamage(key avatar, Float damage)
        {
            Verbose("osCauseDamage(\"{0}\", {1})", avatar, damage);
        }

        public void osCauseHealing(key avatar, Float healing)
        {
            Verbose("osCauseHealing(\"{0}\", {1})", avatar, healing);
        }

        public void osDropAttachment()
        {
            Verbose("osDorpAttachment()");
        }

        public void osDropAttachmentAt(vector pos, rotation rot)
        {
            Verbose("osDorpAttachmentAt({0}, {1})", pos, rot);
        }

        public integer osEjectFromGroup(key user)
        {
            integer isEjected = 1;
            if (user == "")
            {
                isEjected = 0;
            }
            Verbose("osEjectFromGroup(\"{0}\")={1}", user, isEjected);
            return isEjected;
        }

        public void osForceAttachToAvatar(integer attachmentPoint)
        {
            Verbose("osForceAttachToAvatar({0})", attachmentPoint);
        }

        public void osForceAttachToAvatarFromInventory(String itemName, integer attachmentPoint)
        {
            Verbose("osForceAttachToAvatarFromInventory(\"{0}\", {1})", itemName, attachmentPoint);
        }

        public void osForceAttachToOtherAvatarFromInventory(String rawAvatarId, String itemName, integer attachmentPoint)
        {
            Verbose("osForceAttachToOtherAvatarFromInventory(\"{0}\", \"{1}\", {2})", rawAvatarId, itemName, attachmentPoint);
        }

        public void osForceDetachFromAvatar()
        {
            Verbose("osForceDetachFromAvatar()");
        }

        public void osForceDropAttachment()
        {
            Verbose("osForceDropAttachment()");
        }

        public void osForceDropAttachmentAt(vector pos, rotation rot)
        {
            Verbose("osForceDropAttachmentAt({0}, {1})", pos, rot);
        }

        public void osForceOtherSit(String avatar)
        {
            Verbose("osForceOtherSit(\"{0}\")", avatar);
        }

        public void osForceOtherSit(String avatar, String target)
        {
            Verbose("osForceOtherSit(\"{0}\", \"{1}\")", avatar, target);
        }

        public String osGetAgentIP(key uuid)
        {
            Verbose("osGetAgentIP(\"{0}\")=\"\"", uuid);
            return "";
        }

        public list osGetAgents()
        {
            list agents = new list();
            Verbose("osGetAgents()=" + agents.ToVerboseString());
            return agents;
        }

        public String osGetAvatarHomeURI(String uuid)
        {
            Verbose("osGetAvatarHomeURI(\"{0}\")=\"\"", uuid);
            return "";
        }

        public list osGetAvatarList()
        {
            list avatars = new list();
            Verbose("osGetAvatarList()=" + avatars.ToVerboseString());
            return avatars;
        }

        public String osGetGender(key id)
        {
            String gender = "unknown";
            Verbose("osGetGender(\"{0}\")=\"{1}\"", id, gender);
            return gender;
        }

        public Float osGetHealRate(key avatar)
        {
            Float defaultHealRate = 0.5F;
            if (avatar = "")
            {
                defaultHealRate = 0;
            }
            Verbose("osGetHealRate(\"{0}\")={1}", avatar, defaultHealRate);
            return defaultHealRate;
        }

        public Float osGetHealth(string avatar)
        {
            Float health = 100.0F;
            Verbose("osGetHealth(\"{0}\")={1}", avatar, health);
            return health;
        }
        
        public list osGetNumberOfAttachments(key avatar, list attachmentPoints)
        {
            list numberOfAttachments = new list();
            Verbose("osGetNumberOfAttachments(\"{0}\", {1})={2}", avatar, attachmentPoints.ToVerboseString(), numberOfAttachments.ToVerboseString());
            return numberOfAttachments;
        }

        public integer osInviteToGroup(key user)
        {
            if (user != "")
            {
                Verbose("osInviteToGroup(\"{0}\")={1}", user, 1);
                return 1;
            }
            Verbose("osInviteToGroup(\"{0}\")={1}", user, 0);
            return 0;
        }

        public void osKickAvatar(String FirstName, String SurName, String alert)
        {
            Verbose("osKickAvatar(\"{0}\", \"{1}\", \"{2}\")", FirstName, SurName, alert);
        }

        public key osOwnerSaveAppearance(String notecard)
        {
            key k = new key(Guid.NewGuid());
            Verbose("osOwnerSaveAppearance(\"{0}\")=\"{1}\"", notecard, k);
            return k;
        }

        public Float osSetHealRate(key avatar, Float healrate)
        {
            Verbose("osSetHealRate(\"{0}\", {1})={1}", avatar, healrate);
            return healrate;
        }

        public Float osSetHealth(String avatar, Float health)
        {
            Verbose("osSetHealth(\"{0}\", {1})={1}", avatar, health);
            return health;
        }

        public void osTeleportAgent(key agent, integer regionX, integer regionY, vector position, vector lookat)
        {
            Verbose("osTeleportAgent(\"{0}\", {1}, {2}, {3}, {4})", agent, regionX, regionY, position, lookat);
        }

        public void osTeleportAgent(key agent, String regionName, vector position, vector lookat)
        {
            Verbose("osTeleportAgent(\"{0}\", \"{1}\", {2}, {3})", agent, regionName, position, lookat);
        }

        public void osTeleportAgent(key agent, vector position, vector lookat)
        {
            Verbose("osTeleportAgent(\"{0}\", {1}, {2})", agent, position, lookat);
        }

        public void osTeleportOwner(integer regionX, integer regionY, vector position, vector lookat)
        {
            Verbose("osTeleportOwner({0}, {1}, {2}, {3})", regionX, regionY, position, lookat);
        }

        public void osTeleportOwner(String regionName, vector position, vector lookat)
        {
            Verbose("osTeleportOwner(\"{0}\", {1}, {2})", regionName, position, lookat);
        }

        public void osTeleportOwner(vector position, vector lookat)
        {
            Verbose("osTeleportOwner({0}, {1})", position, lookat);
        }

        public integer osIsNpc(key npc)
        {
            if (npc != "")
            {
                integer isNpc = rdmRandom.Next(0, 2);
                Verbose("osIsNpc(\"{0}\")={1}", npc, isNpc);
                return isNpc;
            }
            Verbose("osIsNpc(\"{0}\")=0", npc);
            return 0;
        }

        public key osNpcCreate(String firstname, String lastname, vector position, String cloneFrom, integer options)
        {
            key k = new key(Guid.NewGuid());
            Verbose("osNpcCreate(\"{0}\", \"{1}\", {2}, \"{3}\", {4})=\"{5}\"", firstname, lastname, position, cloneFrom, options, k);
            return k;
        }

        public key osNpcCreate(String firstname, String lastname, vector position, String cloneFrom)
        {
            key k = new key(Guid.NewGuid());
            Verbose("osNpcCreate(\"{0}\", \"{1}\", {2}, \"{3}\")=\"{4}\"", firstname, lastname, position, cloneFrom, k);
            return k;
        }

        public list osGetNpcList()
        {
            list npcs = new list();
            Verbose("osGetNpcList()=" + npcs.ToVerboseString());
            return npcs;
        }

        public vector osNpcGetPos(key npc)
        {
            vector pos = new vector(this.rdmRandom.Next(0, 255), this.rdmRandom.Next(0, 255), this.rdmRandom.Next(0, 255));
            Verbose("osNpcGetPos({0})={1}", npc, pos);
            return pos;
        }

        public rotation osNpcGetRot(key npc)
        {
            rotation rot = new rotation(this.rdmRandom.Next(0, (int)(Math.PI * 2)), this.rdmRandom.Next(0, (int)(Math.PI * 2)),
                this.rdmRandom.Next(0, (int)(Math.PI * 2)), this.rdmRandom.Next(0, (int)(Math.PI * 2)));
            Verbose("osNpcGetRot(\"{0}\")={1}", npc, rot);
            return rot;
        }

        public key osNpcGetOwner(key npc)
        {
            key k = npc != "" ? new key(Guid.NewGuid()) : "";
            Verbose("osNpcGetOwner(\"{0}\")=\"{1}\"", npc, k);
            return k;
        }

        public void osNpcLoadAppearance(key npc, String notecard)
        {
            // Test if notecard exists
            host.GetNotecardLine(notecard, 1);
            Verbose("osNpcLoadAppearance(\"{0}\", \"{1}\")", npc, notecard);
        }

        public void osNpcMoveTo(key npc, vector position)
        {
            Verbose("osNpcMoveTo(\"{0}\", {1})", npc, position);
        }

        public void osNpcMoveToTarget(key npc, vector target, integer options)
        {
            Verbose("osNpcMoveToTarget(\"{0}\", {1}, {2})", npc, target, options);
        }

        public void osNpcPlayAnimation(key npc, String animation)
        {
            Verbose("osNpcPlayAnimation(\"{0}\", \"{1}\")", npc, animation);
        }

        public void osNpcRemove(key npc)
        {
            Verbose("osNpcRemove(\"{0}\")", npc);
        }

        public key osNpcSaveAppearance(key npc, String notecard)
        {
            key k = new key(Guid.NewGuid());
            Verbose("osNpcSaveAppearance(\"{0}\", \"{1}\")=\"{2}\"", npc, notecard, k);
            return k;
        }

        public void osNpcSay(key npc, String message)
        {
            Verbose("osNpcSay(\"{0}\", \"{1}\")", npc, message);
            Chat(0, message, CommunicationType.Say);
        }

        public void osNpcSay(key npc, integer channel, String message)
        {
            Verbose("osNpcSay(\"{0}\", {1}, \"{2}\")", npc, channel, message);
            Chat(channel, message, CommunicationType.Say);
        }

        public void osNpcSetProfileAbout(key npc, String about)
        {
            Verbose("osNpcSetProfileAbout(\"{0}\", \"{1}\")", npc, about);
        }

        public void osNpcSetProfileImage(key npc, String image)
        {
            Verbose("osNpcSetProfileImage(\"{0}\", \"{1}\")", npc, image);
        }

        public void osNpcSetRot(key npc, rotation rot)
        {
            Verbose("osNpcSetRot(\"{0}\", {1})", npc, rot);
        }

        public void osNpcShout(key npc, integer channel, String message)
        {
            Verbose("osNpcShout(\"{0}\", {1}, \"{2}\")", npc, channel, message);
            Chat(channel, message, CommunicationType.Shout);
        }

        public void osNpcSit(key npc, key target, integer options)
        {
            Verbose("osNpcSit(\"{0}\", \"{1}\", {2})", npc, target, options);
        }

        public void osNpcStand(key npc)
        {
            Verbose("osNpcStand(\"{0}\")", npc);
        }

        public void osNpcStopMoveToTarget(key npc)
        {
            Verbose("osNpcStopMoveToTarget(\"{0}\")", npc);
        }

        public void osNpcStopAnimation(key npc, String animation)
        {
            Verbose("osNpcStopAnimation(\"{0}\", \"{1}\")", npc, animation);
        }

        public void osNpcTouch(key npcKey, key objectKey, integer linkNum)
        {
            Verbose("osNpcTouch(\"{0}\", \"{1}\", {2})", npcKey, objectKey, linkNum);
        }

        public void osNpcWhisper(key npc, integer channel, String message)
        {
            Verbose("osNpcWhipser(\"{0}\", \"{1}\", {2})", npc, channel, message);
            llWhisper(channel, message);
        }

        public void osClearInertia()
        {
            Verbose("osClearInertia()");
        }

        public void osForceBreakAllLinks()
        {
            Verbose("osForceBreakAllLinks()");
        }

        public void osForceBreakLink(integer link)
        {
            Verbose("osForceBreakLink({0})", link);
        }

        public void osForceCreateLink(key target, integer parent)
        {
            Verbose("osForceCreateLink(\"{0}\", {1})", target, parent);
        }

        public list osGetInertiaData()
        {
            list l = new list();
            Verbose("osGetInertiaData()={0}", l.ToVerboseString());
            return l;
        }

        public String osGetInventoryDesc(String name)
        {
            Verbose("osGetInventoryDesc(\"{0}\")=\"\"", name);
            return "";
        }

        public integer osGetLinkNumber(String name)
        {
            if (name == "")
            {
                Verbose("osGetLinkNumber(\"{0}\")={1}", name, -1);
                return -1;
            }

            integer i = rdmRandom.Next(100);
            Verbose("osGetLinkNumber(\"{0}\")={1}", name, i);
            return i;
        }

        public list osGetLinkPrimitiveParams(integer linknumber, list rules)
        {
            list primParams = new list();
            Verbose("osGetLinkPrimitiveParams({0}, {1})={2}", linknumber, rules.ToVerboseString(), primParams.ToVerboseString());
            return primParams;
        }

        public list osGetPrimitiveParams(key prim, list rules)
        {
            list primParams = new list();
            Verbose("osGetPrimitiveParams(\"{0}\", {1})={2}", prim, rules.ToVerboseString(), primParams.ToVerboseString());
            return primParams;
        }

        public String osGetRezzingObject()
        {
            Verbose("osGetRezzingObject()=\"{0}\"", key.NULL_KEY.ToString());
            return key.NULL_KEY.ToString();
        }

        public integer osIsUUID(String thing)
        {
            try
            {
                new Guid(thing);
            } catch (FormatException e)
            {
                Verbose("osIsUUID({0})=0", thing);
                return 0;
            }
            Verbose("osIsUUID({0})=1", thing);
            return 1;
        }

        public integer osListenRegex(Int32 channelID, String name, String ID, String msg, Int32 regexBitfield)
        {
            Verbose("osListenRegex({0}, \"{1}\", \"{2}\", \"{3}\", {4})=1", channelID, name, ID, msg, regexBitfield);
            return 1;
        }

        public void osMessageAttachments(key avatar, String message, list attachmentPoints, integer options)
        {
            Verbose("osMessageAttachments(\"{0}\", \"{1}\", {2}, {3})", avatar, message, attachmentPoints.ToVerboseString(), options);
        }

        public void osMessageObject(key objectUUID, String message)
        {
            Verbose("osMessageObject(\"{0}\", \"{1}\")", objectUUID, message);
        }

        public void osSetInertia(Float mass, vector centerOfMass, vector principalInertiaScaled, rotation InertiaRot)
        {
            Verbose("osSetInertia({0}, {1}, {2}, {3})", mass, centerOfMass, principalInertiaScaled, InertiaRot);
        }

        public void osSetInertiaAsBox(Float mass, vector boxSize, vector centerOfMass, rotation rot)
        {
            Verbose("osSetInertiaAsBox({0}, {1}, {2}, {3})", mass, boxSize, centerOfMass, rot);
        }

        public void osSetInertiaAsCylinder(Float mass, Float radius, Float length, vector centerOfMass, rotation rot)
        {
            Verbose("osSetInertiaAsCylinder({0}, {1}, {2}, {3}, {4})", mass, radius, length, centerOfMass, rot);
        }

        public void osSetInertiaAsSphere(Float mass, Float radius, vector centerOfMass)
        {
            Verbose("osSetInertiaAsSphere({0}, {1}, {2})", mass, radius, centerOfMass);
        }

        public void osSetPrimitiveParams(key prim, list rules)
        {
            Verbose("osSetPrimitiveParams(\"{0}\", {1})", prim, rules.ToVerboseString());
        }

        public void osSetProjectionParams(integer projection, key texture, Float fov, Float focus, Float amb)
        {
            Verbose("osSetProjectionParams({0}, \"{1}\", {2}, {3}, {4})", projection, texture, fov, focus, amb);
        }

        public void osSetProjectionParams(key prim, integer projection, key texture, Float fov, Float focus, Float amb)
        {
            Verbose("osSetProjectionParams(\"{0}\", {1}, \"{2}\", {3}, {4}, {5})", prim, projection, texture, fov, focus, amb);
        }

        public void osSetSpeed(String UUID, Float SpeedModifier)
        {
            Verbose("osSetSpeed(\"{0}\", {1})", UUID, SpeedModifier);
        }

        public integer osTeleportObject(key objectUUID, vector targetPos, rotation rot, integer flags)
        {
            Verbose("osTeleportObject(\"{0}\", {1}, {2}, {3})=1", objectUUID, targetPos, rot, flags);
            return 1;
        }

        public String osDrawEllipse(String drawList, integer width, integer height)
        {
            Verbose("osDrawEllipse(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawFilledEllipse(String drawList, integer width, integer height)
        {
            Verbose("osDrawFilledEllipse(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawFilledPolygon(String drawList, list xpoints, list ypoints)
        {
            Verbose("osDrawFilledPolygon(\"{0}\", {1}, {2})=\"\"", drawList, xpoints.ToVerboseString(), ypoints.ToVerboseString());
            return "";
        }

        public String osDrawFilledRectangle(String drawList, integer width, integer height)
        {
            Verbose("osDrawFilledRectangle(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawImage(String drawList, integer width, integer height, String imageUrl)
        {
            Verbose("osDrawImage(\"{0}\", {1}, {2}, \"{3}\")=\"\"", drawList, width, height, imageUrl);
            return "";
        }

        public String osDrawLine(String drawList, integer startX, integer startY, integer endX, integer endY)
        {
            Verbose("osDrawLine(\"{0}\", {1}, {2}, {3}, {4})=\"\"", drawList, startX, startY, endX, endY);
            return "";
        }

        public String osDrawLine(String drawList, integer endX, integer endY)
        {
            Verbose("osDrawLine(\"{0}\", {1}, {2}, {3}, {4})=\"\"", drawList, endX, endY);
            return "";
        }

        public String osDrawPolygon(String drawList, list xpoints, list ypoints)
        {
            Verbose("osDrawPolygon(\"{0}\", {1}, {2})=\"\"", drawList, xpoints.ToVerboseString(), ypoints.ToVerboseString());
            return "";
        }

        public String osDrawRectangle(String drawList, integer width, integer height)
        {
            Verbose("osDrawRectangle(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawResetTransform(String drawList)
        {
            Verbose("osDrawResetTransform(\"{0}\")=\"\"", drawList);
            return "";
        }

        public String osDrawRotationTransform(String drawList, Float x)
        {
            Verbose("osDrawRotationTransform(\"{0}\", {1})=\"\"", drawList, x);
            return "";
        }

        public String osDrawScaleTransform(String drawList, Float x, Float y)
        {
            Verbose("osDrawScaleTransform(\"{0}\", {1}, {2})=\"\"", drawList, x, y);
            return "";
        }

        public String osDrawText(String drawList, String text)
        {
            Verbose("osDrawText(\"{0}\", \"{1}\")=\"\"", drawList, text);
            return "";
        }

        public String osDrawTranslationTransform(String drawList, Float x, Float y)
        {
            Verbose("osDrawTranslationTransform(\"{0}\", {1}, {2})=\"\"", drawList, x, y);
            return "";
        }

        public vector osGetDrawStringSize(String contentType, String text, String fontName, integer fontSize)
        {
            vector v = new vector();
            Verbose("osGetDrawStringSize(\"{0}\", \"{1}\", \"{2}\", {3})={4}", contentType, text, fontName, fontSize, v);
            return v;
        }

        public String osMovePen(String drawList, integer x, integer y)
        {
            Verbose("osMovePen(\"{0}\", {1}, {2})=\"\"", drawList, x, y);
            return "";
        }

        public String osSetFontName(String drawList, String fontName)
        {
            Verbose("osSetFontName(\"{0}\", \"{1}\")=\"\"", drawList, fontName);
            return "";
        }

        public String osSetFontSize(String drawList, integer fontSize)
        {
            Verbose("osSetFontSize(\"{0}\", {1})=\"\"", drawList, fontSize);
            return "";
        }

        public String osSetPenCap(String drawList, String direction, String type)
        {
            Verbose("osSetPenCap(\"{0}\", \"{1}\", \"{2}\")=\"\"", drawList, direction, type);
            return "";
        }

        public String osSetPenColor(String drawList, String color)
        {
            Verbose("osSetPenColor(\"{0}\", \"{1}\")=\"\"", drawList, color);
            return "";
        }

        public String osSetPenSize(String drawList, integer penSize)
        {
            Verbose("osSetPenSize(\"{0}\", {1})=\"\"", drawList, penSize);
            return "";
        }

        public String osSetDynamicTextureData(String dynamicID, String contentType, String data, String extraParams, integer timer)
        {
            Verbose("osSetDynamicTextureData(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4})=\"\"", dynamicID, contentType, data, extraParams, timer);
            return "";
        }

        public String osSetDynamicTextureDataBlend(String dynamicID, String contentType, String data, String extraParams, integer timer, integer alpha)
        {
            Verbose("osSetDynamicTextureDataBlend(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5})=\"\"", 
                dynamicID, contentType, data, extraParams, timer, alpha);
            return "";
        }

        public String osSetDynamicTextureDataBlendFace(String dynamicID, String contentType, String data, String extraParams, integer blend, integer disp, integer timer, integer alpha, integer face)
        {
            Verbose("osSetDynamicTextureDataBlendFace(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5}, {6}, {7}, {8})=\"\"",
                dynamicID, contentType, data, extraParams, blend, disp, timer, alpha, face);
            return "";
        }

        public String osSetDynamicTextureDataFace(String dynamicID, String contentType, String data, String extraParams, integer timer, integer face)
        {
            Verbose("osSetDynamicTextureDataFace(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5})=\"\"",
                dynamicID, contentType, data, extraParams, timer, face);
            return "";
        }

        public String osSetDynamicTextureURL(String dynamicID, String contentType, String url, String extraParams, integer timer)
        {
            Verbose("osSetDynamicTextureURL(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4})=\"\"",
                dynamicID, contentType, url, extraParams, timer);
            return "";
        }

        public String osSetDynamicTextureURLBlend(String dynamicID, String contentType, String url, String extraParams, integer timer, integer alpha)
        {
            Verbose("osSetDynamicTextureURLBlend(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5})=\"\"",
                dynamicID, contentType, url, extraParams, timer, alpha);
            return "";
        }

        public String osSetDynamicTextureURLBlendFace(String dynamicID, String contentType, String url, String extraParams, integer blend, integer disp, integer timer, integer alpha, integer face)
        {
            Verbose("osSetDynamicTextureURLBlendFace(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5}, {6}, {7}, {8})=\"\"",
                dynamicID, contentType, url, extraParams, blend, disp, timer, alpha, face);
            return "";
        }

        public String osGetNotecard(String name)
        {
            string strPath = host.efMainForm.SolutionExplorer.GetPath(host.GUID, name);

            if (strPath == string.Empty)
            {
                strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }

            if (!File.Exists(strPath))
            {
                host.VerboseMessage("Notecard: " + strPath + " not found");
                host.ExecuteSecondLife("llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
                Verbose("osGetNotecard(\"{0}\")=\"\"", name);
                return "";
            }

            StreamReader sr = new StreamReader(strPath);
            StringBuilder result = new StringBuilder();

            while (!sr.EndOfStream)
            {
                result.Append(sr.ReadLine() + "\n");
                
            }
            sr.Close();
            
            Verbose("osGetNotecard(\"{0}\")=\"{1}\"", name, result.ToString());
            return result.ToString();
        }

        public String osGetNotecardLine(String name, integer line)
        {
            string strPath = host.efMainForm.SolutionExplorer.GetPath(host.GUID, name);

            if (strPath == string.Empty)
            {
                strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }

            if (!File.Exists(strPath))
            {
                host.VerboseMessage("Notecard: " + strPath + " not found");
                host.ExecuteSecondLife("llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
                Verbose("osGetNotecardLine(\"{0}\", {1})=\"\"", name, line);
                return "";
            }

            StreamReader sr = new StreamReader(strPath);
            string result = "";
            int i = 1;

            while (!sr.EndOfStream)
            {
                if(i == line)
                {
                    result = sr.ReadLine();
                } else
                {
                    sr.ReadLine();
                }
                i++;
            }
            sr.Close();

            Verbose("osGetNotecardLine(\"{0}\", {1})=\"{2}\"", name, line, result);
            return result;
        }

        public integer osGetNumberOfNotecardLines(String name)
        {
            string strPath = host.efMainForm.SolutionExplorer.GetPath(host.GUID, name);

            if (strPath == string.Empty)
            {
                strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }

            if (!File.Exists(strPath))
            {
                host.VerboseMessage("Notecard: " + strPath + " not found");
                host.ExecuteSecondLife("llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
                Verbose("osGetNumberOfNotecardLines(\"{0}\")=-1", name);
                return -1;
            }

            StreamReader sr = new StreamReader(strPath);
            int i = 0;

            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                i++;
            }
            sr.Close();

            Verbose("osGetNumberOfNotecardLines(\"{0}\")={1}", name, i);
            return i;
        }

        public void osMakeNotecard(String notecardName, list contents)
        {
            using(StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), notecardName)))
            {
                foreach(String content in contents.ToArray())
                {
                    sw.WriteLine(content.ToString());
                }
            }

            Verbose("osMakeNotecard(\"{0}\", {1})", notecardName, contents.ToVerboseString());
        }

        public void osMakeNotecard(String notecardName, String contents)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), notecardName)))
            {
                sw.Write(contents.ToString());
            }

            Verbose("osMakeNotecard(\"{0}\", \"{1}\")", notecardName, contents);
        }

        public void osRequestSecureURL(list options)
        {
            Verbose("osRequestSecureURL({0})", options.ToVerboseString());
        }

        public void osRequestURL(list options)
        {
            Verbose("osRequestURL({0})", options.ToVerboseString());
        }

        public void osParcelJoin(vector pos1, vector pos2)
        {
            Verbose("osParcelJoin({0}, {1})", pos1, pos2);
        }

        public void osParcelSubdivide(vector pos1, vector pos2)
        {
            Verbose("osParcelSubdivide({0}, {1})", pos1, pos2);
        }

        public void osSetParcelDetails(vector pos, list rules)
        {
            Verbose("osSetParcelDetails({0}, {1})", pos, rules.ToVerboseString());
        }

        public Float osGetTerrainHeight(integer x, integer y)
        {
            Verbose("osGetTerrainHeight({0}, {1})=1", x, y);
            return 1;
        }
        
        public integer osSetTerrainHeight(integer x, integer y, double value)
        {
            Verbose("osSetTerrainHeight({0}, {1}, {2})=1", x, y, value);
            return 1;
        }

        public void osSetTerrainTexture(integer level, key texture)
        {
            Verbose("osSetTerrainTexture({0}, \"{1}\")", level, texture);
        }

        public void osSetTerrainTextureHeight(integer corner, Float low, Float high)
        {
            Verbose("osSetTerrainTextureHeight({0}, {1}, {2})", corner, low, high);
        }

        public void osTerrainFlush()
        {
            Verbose("osTerrainFlush()");
        }

        public Float osGetCurrentSunHour()
        {
            Verbose("osGetCurrentSunHour()=0");
            return 0;
        }

        public Float osGetSunParam(String param)
        {
            Verbose("osGetSunParam(\"{0}\")=0", param);
            return 0;
        }

        public Float osGetWindParam(String plugin, String param)
        {
            Verbose("osGetWindParam(\"{0}\", \"{1}\")=0", plugin, param);
            return 0;
        }

        public void osSetEstateSunSettings(integer sunFixed, Float sunHour)
        {
            Verbose("osSetEstateSunSettings({0}, {1})", sunFixed, sunHour);
        }

        public void osSetRegionSunSettings(integer useEstateSun, integer sunFixed, Float sunHour)
        {
            Verbose("osSetRegionSunSettings({0}, {1}, {2})", useEstateSun, sunFixed, sunHour);
        }

        public void osSetRegionWaterHeight(Float height)
        {
            Verbose("osSetRegionWaterHeight({0})", height);
        }

        public void osSetSunParam(String param, Float value)
        {
            Verbose("osSetSunParam(\"{0}\", {1})", param, value);
        }

        public void osSetWindParam(String plugin, String param, Float value)
        {
            Verbose("osSetWindParam(\"{0}\", \"{1}\", {2})", plugin, param, value);
        }

        public String osWindActiveModelPluginName()
        {
            Verbose("osWindActiveModelPluginName()=\"\"");
            return "";
        }

        public integer osCheckODE()
        {
            Verbose("osCheckODE()=0");
            return 0;
        }

        public String osGetGridCustom(String key)
        {
            Verbose("osGetGridCustom(\"{0}\")=\"\"");
            return "";
        }

        public String osGetGridGatekeeperURI()
        {
            Verbose("osGetGridGatekeeperURI()=\"\"");
            return "";
        }

        public String osGetGridHomeURI()
        {
            Verbose("osGetGridHomeURI()=\"\"");
            return "";
        }

        public String osGetGridLoginURI()
        {
            Verbose("osGetGridLoginURI()=\"\"");
            return "";
        }

        public String osGetGridName()
        {
            Verbose("osGetGridName()=\"\"");
            return "";
        }

        public String osGetGridNick()
        {
            Verbose("osGetGridNick()=\"\"");
            return "";
        }

        public key osGetMapTexture()
        {
            key k = new key(new Guid());
            Verbose("osGetMapTexture()=\"{0}\"", k);
            return k;
        }

        public String osGetPhysicsEngineName()
        {
            Verbose("osGetPhysicsEngineName()=\"\"");
            return "";
        }

        public String osGetPhysicsEngineType()
        {
            Verbose("osGetPhysicsEngineType()=\"\"");
            return "";
        }

        public key osGetRegionMapTexture(String regionName)
        {
            key k = new key(new Guid());
            Verbose("osGetRegionMapTexture(\"{0}\")=\"{1}\"", regionName, k);
            return k;
        }

        public vector osGetRegionSize()
        {
            vector v = new vector(256, 256, 0);
            Verbose("osGetRegionSize()={0}", v);
            return v;
        }

        public list osGetRegionStats()
        {
            list l = new list();
            Verbose("osGetRegionStats()={0}", l.ToVerboseString());
            return l;
        }

        public String osGetScriptEngineName()
        {
            Verbose("osGetScriptEngineName()=\"\"");
            return "";
        }

        public integer osGetSimulatorMemory()
        {
            Verbose("osGetSimulatorMemory()=0");
            return 0;
        }

        public String osGetSimulatorVersion()
        {
            Verbose("osGetSimulatorVersion()=\"\"");
            return "";
        }

        public String osLoadedCreationDate()
        {
            Verbose("osLoadedCreationDate()=\"\"");
            return "";
        }

        public String osLoadedCreationID()
        {
            Verbose("osLoadedCreationID()=\"\"");
            return "";
        }

        public String osLoadedCreationTime()
        {
            Verbose("osLoadedCreationTime()=\"\"");
            return "";
        }

        public integer osConsoleCommand(String command)
        {
            Verbose("osConsoleCommand(\"{0}\")=1");
            return 1;
        }

        public void osRegionNotice(String msg)
        {
            Verbose("osRegionNotice(\"{0}\")", msg);
        }

        public integer osRegionRestart(Float seconds)
        {
            Verbose("osRegionRestart({0})={1}", seconds, 1);
            return 1;
        }

        public void osSetParcelMediaURL(String url)
        {
            Verbose("osSetParcelMediaURL(\"{0}\")", url);
        }

        public void osSetParcelSIPAddress(String SIPAddress)
        {
            Verbose("osSetParcelSIPAddress(\"{0}\")", SIPAddress);
        }

        public void osSetPrimFloatOnWater(integer f)
        {
            Verbose("osSetPrimFloatOnWater({0})", f);
        }

        public void osGrantScriptPermissions(key allowed_key, String function)
        {
            Verbose("osGrantScriptPermissions(\"{0}\", \"{1}\")", allowed_key, function);
        }

        public void osRevokeScriptPermissions(key revoked_key, String function)
        {
            Verbose("osRevokeScriptPermissions(\"{0}\", \"{1}\")", revoked_key, function);
        }

        public void osCollisionSound(String impact_sound, double impact_volume)
        {
            Verbose("osCollisionSound(\"{0}\", {1})", impact_sound, impact_volume);
        }

        public Float osDie(key objectUUID)
        {
            Verbose("osDie(\"{0}\")=1", objectUUID);
            return 1;
        }

        public String osFormatString(String format, list parameters)
        {
            string formatted = string.Format(format, parameters.ToArray());

            Verbose("osFormatString(\"{0}\", {1})=\"{2}\"", format, parameters.ToVerboseString(), formatted);
            return formatted;
        }

        public String osKey2Name(key id)
        {
            Verbose("osKey2Name(\"{0}\")=\"\"");
            return "";
        }

        public Float osList2Double(list src, integer index)
        {
            if (index >= src.Count)
            {
                Verbose("osList2Double({0}, {1})={2}", src.ToVerboseString(), index, -1);
                return -1;
            }
            else if (src[index].GetType() == typeof(string))
            {
                Verbose("osList2Double({0}, {1})={2}", src.ToVerboseString(), index, float.Parse((string)src[index]));
                return (Float)float.Parse((string)src[index]);
            }
            else
            {
                Float f = float.Parse(src[index].ToString());
                Verbose("osList2Double({0}, {1})={2}", src.ToVerboseString(), index, src[index]);
                return f;
            }
        }

        public list osMatchString(string src, String pattern, integer start)
        {
            list result = new list();

            if (start < 0)
            {
                start = src.Length + start;
            }

            if (start < 0 || start >= src.Length)
            {
                Verbose("osMatchString(\"{0}\", \"{1}\", {2})={3}", src, pattern, start, result.ToVerboseString());
                return result;  // empty list 
            }

            // Find matches beginning at start position
            Regex matcher = new Regex(pattern);
            Match match = matcher.Match(src, start);
            while (match.Success)
            {
                foreach (System.Text.RegularExpressions.Group g in match.Groups)
                {
                    if (g.Success)
                    {
                        result.Add(new String(g.Value));
                        result.Add(new integer(g.Index));
                    }
                }

                match = match.NextMatch();
            }
            Verbose("osMatchString(\"{0}\", \"{1}\", {2})={3}", src, pattern, start, result.ToVerboseString());
            return result;
        }

        public Float osMax(Double a, Double b)
        {
            Float max = (Float)Math.Max(a, b);

            Verbose("osMax({0}, {1})={2}", a, b, max);
            return max;
        }

        public Float osMin(Double a, Double b)
        {
            Float min = (Float)Math.Min(a, b);

            Verbose("osMin({0}, {1})={2}", a, b, min);
            return min;
        }
        
        public Hashtable osParseJSON(String JSON)
        {
            Object decoded = JsonConvert.DeserializeObject(JSON);

            if (decoded is Hashtable)
            {
                Verbose("osParseJSON(\"{0}\")={1}", JSON, (Hashtable)decoded);
                return (Hashtable) decoded;
            }
            else if (decoded is ArrayList)
            {
                ArrayList decoded_list = (ArrayList)decoded;
                Hashtable fakearray = new Hashtable();
                int i = 0;
                for (i = 0; i < decoded_list.Count; i++)
                {
                    fakearray.Add(i, decoded_list[i]);
                }
                
                Verbose("osParseJSON(\"{0}\")={1}", JSON, new list(decoded_list.ToArray()).ToVerboseString());
                return fakearray;
            }
            else
            {
                Chat(0, "osParseJSON: unable to parse JSON string " + JSON, CommunicationType.Say);
                Verbose("osParseJSON(\"{0}\")={1}", JSON, new list().ToVerboseString());
                return null;
            }
        }

        public Object osParseJSONNew(String JSON)
        {
            Object result = JsonConvert.DeserializeObject(JSON);
            Verbose("osParseJSONNew(\"{0}\")={1}", JSON, result);
            return result;
        }

        public string osReplaceString(string src, String pattern, String replace, int count, int start)
        {
            // Normalize indices (if negative).
            // After normalization they may still be
            // negative, but that is now relative to
            // the start, rather than the end, of the
            // sequence.
            if (start < 0)
            {
                start = src.Length + start;
            }

            if (start < 0 || start >= src.Length)
            {
                Verbose("osReplaceString(\"{0}\", \"{1}\", \"{2}\", {3}, {4})=\"{5}\"", src, pattern, replace, count, start, src);
                return src;
            }

            // Find matches beginning at start position
            Regex matcher = new Regex(pattern);
            string result = matcher.Replace(src, replace, count, start);
            Verbose("osReplaceString(\"{0}\", \"{1}\", \"{2}\", {3}, {4})=\"{5}\"", src, pattern, replace, count, start, result);

            return result;
        }

        public integer osRegexIsMatch(String input, String pattern)
        {
            integer result = Regex.IsMatch(input, pattern);
            Verbose("osRegexIsMatch(\"{0}\", \"{1}\")={2}", input, pattern, result);

            return result;
        }

        public void osSetContentType(key id, string type)
        {
            Verbose("osSetContentType(\"{0}\", \"{1}\")", id, type);
        }

        public void osSetStateEvents(integer events)
        {
            Verbose("osSetStateEvents({0})", events);
        }

        public string osUnixTimeToTimestamp(integer epoch)
        {
            long baseTicks = 621355968000000000;
            long tickResolution = 10000000;
            long epochTicks = (epoch * tickResolution) + baseTicks;
            DateTime date = new DateTime(epochTicks);
            string result = date.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");

            Verbose("osUnixTimeToTimestamp({0})=\"{1}\"", epoch, result);

            return result;
        }

        public void osVolumeDetect(integer detect)
        {
            Verbose("osVolumeDetect({0})", detect);
        }
    }
}
