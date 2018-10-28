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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Script.Serialization;

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
                var k = new key(Guid.NewGuid());
                this.Verbose("osAgentSaveAppearance(\"{0}\", \"{1}\")=\"{2}\"", avatarId, notecard, k);
                return k;
            }
            this.Verbose("osAgentSaveAppearance(\"{0}\", \"{1}\")=\"{2}\"", avatarId, notecard, "");
            return key.NULL_KEY;
        }

        public String osAvatarName2Key(String firstname, String lastname)
        {
            if (Properties.Settings.Default.AvatarName == (firstname + " " + lastname))
            {
                this.Verbose("osAvatarName2Key(\"{0}\", \"{1}\")=\"{2}\"", firstname, lastname, Properties.Settings.Default.AvatarKey);
                return Properties.Settings.Default.AvatarKey;
            }

            if (!(firstname == "" && lastname == ""))
            {
                var k = new key(Guid.NewGuid());
                this.Verbose("osAvatarName2Key(\"{0}\", \"{1}\")=\"{2}\"", firstname, lastname, k);
                return k.ToString();
            }
            this.Verbose("osAvatarName2Key(\"{0}\", \"{1}\")=\"{2}\"", firstname, lastname, "");
            return key.NULL_KEY.ToString();
        }

        public void osAvatarPlayAnimation(key avatar, String animation)
        {
            this.Verbose("osAvatarPlayAnimation(\"{0}\", \"{1}\")", avatar, animation);
        }

        public void osAvatarStopAnimation(key avatar, String animation)
        {
            this.Verbose("osAvatarStopAnimation(\"{0}\", \"{1}\")", avatar, animation);
        }

        public void osCauseDamage(key avatar, Float damage)
        {
            this.Verbose("osCauseDamage(\"{0}\", {1})", avatar, damage);
        }

        public void osCauseHealing(key avatar, Float healing)
        {
            this.Verbose("osCauseHealing(\"{0}\", {1})", avatar, healing);
        }

        public void osDropAttachment()
        {
            this.Verbose("osDorpAttachment()");
        }

        public void osDropAttachmentAt(vector pos, rotation rot)
        {
            this.Verbose("osDorpAttachmentAt({0}, {1})", pos, rot);
        }

        public integer osEjectFromGroup(key user)
        {
            integer isEjected = 1;
            if (user == "")
            {
                isEjected = 0;
            }
            this.Verbose("osEjectFromGroup(\"{0}\")={1}", user, isEjected);
            return isEjected;
        }

        public void osForceAttachToAvatar(integer attachmentPoint)
        {
            this.Verbose("osForceAttachToAvatar({0})", attachmentPoint);
        }

        public void osForceAttachToAvatarFromInventory(String itemName, integer attachmentPoint)
        {
            this.Verbose("osForceAttachToAvatarFromInventory(\"{0}\", {1})", itemName, attachmentPoint);
        }

        public void osForceAttachToOtherAvatarFromInventory(String rawAvatarId, String itemName, integer attachmentPoint)
        {
            this.Verbose("osForceAttachToOtherAvatarFromInventory(\"{0}\", \"{1}\", {2})", rawAvatarId, itemName, attachmentPoint);
        }

        public void osForceDetachFromAvatar()
        {
            this.Verbose("osForceDetachFromAvatar()");
        }

        public void osForceDropAttachment()
        {
            this.Verbose("osForceDropAttachment()");
        }

        public void osForceDropAttachmentAt(vector pos, rotation rot)
        {
            this.Verbose("osForceDropAttachmentAt({0}, {1})", pos, rot);
        }

        public void osForceOtherSit(String avatar)
        {
            this.Verbose("osForceOtherSit(\"{0}\")", avatar);
        }

        public void osForceOtherSit(String avatar, String target)
        {
            this.Verbose("osForceOtherSit(\"{0}\", \"{1}\")", avatar, target);
        }

        public String osGetAgentIP(key uuid)
        {
            this.Verbose("osGetAgentIP(\"{0}\")=\"\"", uuid);
            return "";
        }

        public list osGetAgents()
        {
            var agents = new list();
            this.Verbose("osGetAgents()=" + agents.ToVerboseString());
            return agents;
        }

        public String osGetAvatarHomeURI(String uuid)
        {
            this.Verbose("osGetAvatarHomeURI(\"{0}\")=\"\"", uuid);
            return "";
        }

        public list osGetAvatarList()
        {
            var avatars = new list();
            this.Verbose("osGetAvatarList()=" + avatars.ToVerboseString());
            return avatars;
        }

        public String osGetGender(key id)
        {
            String gender = "unknown";
            this.Verbose("osGetGender(\"{0}\")=\"{1}\"", id, gender);
            return gender;
        }

        public Float osGetHealRate(key avatar)
        {
            Float defaultHealRate = 0.5F;
            if (avatar = "")
            {
                defaultHealRate = 0;
            }
            this.Verbose("osGetHealRate(\"{0}\")={1}", avatar, defaultHealRate);
            return defaultHealRate;
        }

        public Float osGetHealth(string avatar)
        {
            Float health = 100.0F;
            this.Verbose("osGetHealth(\"{0}\")={1}", avatar, health);
            return health;
        }

        public list osGetNumberOfAttachments(key avatar, list attachmentPoints)
        {
            var numberOfAttachments = new list();
            this.Verbose("osGetNumberOfAttachments(\"{0}\", {1})={2}", avatar, attachmentPoints.ToVerboseString(), numberOfAttachments.ToVerboseString());
            return numberOfAttachments;
        }

        public integer osInviteToGroup(key user)
        {
            if (user != "")
            {
                this.Verbose("osInviteToGroup(\"{0}\")={1}", user, 1);
                return 1;
            }
            this.Verbose("osInviteToGroup(\"{0}\")={1}", user, 0);
            return 0;
        }

        public void osKickAvatar(String FirstName, String SurName, String alert)
        {
            this.Verbose("osKickAvatar(\"{0}\", \"{1}\", \"{2}\")", FirstName, SurName, alert);
        }

        public key osOwnerSaveAppearance(String notecard)
        {
            var k = new key(Guid.NewGuid());
            this.Verbose("osOwnerSaveAppearance(\"{0}\")=\"{1}\"", notecard, k);
            return k;
        }

        public Float osSetHealRate(key avatar, Float healrate)
        {
            this.Verbose("osSetHealRate(\"{0}\", {1})={1}", avatar, healrate);
            return healrate;
        }

        public Float osSetHealth(String avatar, Float health)
        {
            this.Verbose("osSetHealth(\"{0}\", {1})={1}", avatar, health);
            return health;
        }

        public void osTeleportAgent(key agent, integer regionX, integer regionY, vector position, vector lookat)
        {
            this.Verbose("osTeleportAgent(\"{0}\", {1}, {2}, {3}, {4})", agent, regionX, regionY, position, lookat);
        }

        public void osTeleportAgent(key agent, String regionName, vector position, vector lookat)
        {
            this.Verbose("osTeleportAgent(\"{0}\", \"{1}\", {2}, {3})", agent, regionName, position, lookat);
        }

        public void osTeleportAgent(key agent, vector position, vector lookat)
        {
            this.Verbose("osTeleportAgent(\"{0}\", {1}, {2})", agent, position, lookat);
        }

        public void osTeleportOwner(integer regionX, integer regionY, vector position, vector lookat)
        {
            this.Verbose("osTeleportOwner({0}, {1}, {2}, {3})", regionX, regionY, position, lookat);
        }

        public void osTeleportOwner(String regionName, vector position, vector lookat)
        {
            this.Verbose("osTeleportOwner(\"{0}\", {1}, {2})", regionName, position, lookat);
        }

        public void osTeleportOwner(vector position, vector lookat)
        {
            this.Verbose("osTeleportOwner({0}, {1})", position, lookat);
        }

        public integer osIsNpc(key npc)
        {
            if (npc != "")
            {
                integer isNpc = this.rdmRandom.Next(0, 2);
                this.Verbose("osIsNpc(\"{0}\")={1}", npc, isNpc);
                return isNpc;
            }
            this.Verbose("osIsNpc(\"{0}\")=0", npc);
            return 0;
        }

        public key osNpcCreate(String firstname, String lastname, vector position, String cloneFrom, integer options)
        {
            var k = new key(Guid.NewGuid());
            this.Verbose("osNpcCreate(\"{0}\", \"{1}\", {2}, \"{3}\", {4})=\"{5}\"", firstname, lastname, position, cloneFrom, options, k);
            return k;
        }

        public key osNpcCreate(String firstname, String lastname, vector position, String cloneFrom)
        {
            var k = new key(Guid.NewGuid());
            this.Verbose("osNpcCreate(\"{0}\", \"{1}\", {2}, \"{3}\")=\"{4}\"", firstname, lastname, position, cloneFrom, k);
            return k;
        }

        public list osGetNpcList()
        {
            var npcs = new list();
            this.Verbose("osGetNpcList()=" + npcs.ToVerboseString());
            return npcs;
        }

        public vector osNpcGetPos(key npc)
        {
            var pos = new vector(this.rdmRandom.Next(0, 255), this.rdmRandom.Next(0, 255), this.rdmRandom.Next(0, 255));
            this.Verbose("osNpcGetPos({0})={1}", npc, pos);
            return pos;
        }

        public rotation osNpcGetRot(key npc)
        {
            var rot = new rotation(this.rdmRandom.Next(0, (int)(Math.PI * 2)), this.rdmRandom.Next(0, (int)(Math.PI * 2)),
                this.rdmRandom.Next(0, (int)(Math.PI * 2)), this.rdmRandom.Next(0, (int)(Math.PI * 2)));
            this.Verbose("osNpcGetRot(\"{0}\")={1}", npc, rot);
            return rot;
        }

        public key osNpcGetOwner(key npc)
        {
            var k = npc != "" ? new key(Guid.NewGuid()) : "";
            this.Verbose("osNpcGetOwner(\"{0}\")=\"{1}\"", npc, k);
            return k;
        }

        public void osNpcLoadAppearance(key npc, String notecard)
        {
            // Test if notecard exists
            this.host.GetNotecardLine(notecard, 1);
            this.Verbose("osNpcLoadAppearance(\"{0}\", \"{1}\")", npc, notecard);
        }

        public void osNpcMoveTo(key npc, vector position)
        {
            this.Verbose("osNpcMoveTo(\"{0}\", {1})", npc, position);
        }

        public void osNpcMoveToTarget(key npc, vector target, integer options)
        {
            this.Verbose("osNpcMoveToTarget(\"{0}\", {1}, {2})", npc, target, options);
        }

        public void osNpcPlayAnimation(key npc, String animation)
        {
            this.Verbose("osNpcPlayAnimation(\"{0}\", \"{1}\")", npc, animation);
        }

        public void osNpcRemove(key npc)
        {
            this.Verbose("osNpcRemove(\"{0}\")", npc);
        }

        public key osNpcSaveAppearance(key npc, String notecard)
        {
            var k = new key(Guid.NewGuid());
            this.Verbose("osNpcSaveAppearance(\"{0}\", \"{1}\")=\"{2}\"", npc, notecard, k);
            return k;
        }

        public void osNpcSay(key npc, String message)
        {
            this.Verbose("osNpcSay(\"{0}\", \"{1}\")", npc, message);
            this.Chat(0, message, CommunicationType.Say);
        }

        public void osNpcSay(key npc, integer channel, String message)
        {
            this.Verbose("osNpcSay(\"{0}\", {1}, \"{2}\")", npc, channel, message);
            this.Chat(channel, message, CommunicationType.Say);
        }

        public void osNpcSetProfileAbout(key npc, String about)
        {
            this.Verbose("osNpcSetProfileAbout(\"{0}\", \"{1}\")", npc, about);
        }

        public void osNpcSetProfileImage(key npc, String image)
        {
            this.Verbose("osNpcSetProfileImage(\"{0}\", \"{1}\")", npc, image);
        }

        public void osNpcSetRot(key npc, rotation rot)
        {
            this.Verbose("osNpcSetRot(\"{0}\", {1})", npc, rot);
        }

        public void osNpcShout(key npc, integer channel, String message)
        {
            this.Verbose("osNpcShout(\"{0}\", {1}, \"{2}\")", npc, channel, message);
            this.Chat(channel, message, CommunicationType.Shout);
        }

        public void osNpcSit(key npc, key target, integer options)
        {
            this.Verbose("osNpcSit(\"{0}\", \"{1}\", {2})", npc, target, options);
        }

        public void osNpcStand(key npc)
        {
            this.Verbose("osNpcStand(\"{0}\")", npc);
        }

        public void osNpcStopMoveToTarget(key npc)
        {
            this.Verbose("osNpcStopMoveToTarget(\"{0}\")", npc);
        }

        public void osNpcStopAnimation(key npc, String animation)
        {
            this.Verbose("osNpcStopAnimation(\"{0}\", \"{1}\")", npc, animation);
        }

        public void osNpcTouch(key npcKey, key objectKey, integer linkNum)
        {
            this.Verbose("osNpcTouch(\"{0}\", \"{1}\", {2})", npcKey, objectKey, linkNum);
        }

        public void osNpcWhisper(key npc, integer channel, String message)
        {
            this.Verbose("osNpcWhipser(\"{0}\", \"{1}\", {2})", npc, channel, message);
            this.llWhisper(channel, message);
        }

        public void osClearInertia()
        {
            this.Verbose("osClearInertia()");
        }

        public void osForceBreakAllLinks()
        {
            this.Verbose("osForceBreakAllLinks()");
        }

        public void osForceBreakLink(integer link)
        {
            this.Verbose("osForceBreakLink({0})", link);
        }

        public void osForceCreateLink(key target, integer parent)
        {
            this.Verbose("osForceCreateLink(\"{0}\", {1})", target, parent);
        }

        public list osGetInertiaData()
        {
            var l = new list();
            this.Verbose("osGetInertiaData()={0}", l.ToVerboseString());
            return l;
        }

        public String osGetInventoryDesc(String name)
        {
            this.Verbose("osGetInventoryDesc(\"{0}\")=\"\"", name);
            return "";
        }

        public integer osGetLinkNumber(String name)
        {
            if (name == "")
            {
                this.Verbose("osGetLinkNumber(\"{0}\")={1}", name, -1);
                return -1;
            }

            integer i = this.rdmRandom.Next(100);
            this.Verbose("osGetLinkNumber(\"{0}\")={1}", name, i);
            return i;
        }

        public list osGetLinkPrimitiveParams(integer linknumber, list rules)
        {
            var primParams = new list();
            this.Verbose("osGetLinkPrimitiveParams({0}, {1})={2}", linknumber, rules.ToVerboseString(), primParams.ToVerboseString());
            return primParams;
        }

        public list osGetPrimitiveParams(key prim, list rules)
        {
            var primParams = new list();
            this.Verbose("osGetPrimitiveParams(\"{0}\", {1})={2}", prim, rules.ToVerboseString(), primParams.ToVerboseString());
            return primParams;
        }

        public String osGetRezzingObject()
        {
            this.Verbose("osGetRezzingObject()=\"{0}\"", key.NULL_KEY.ToString());
            return key.NULL_KEY.ToString();
        }

        public integer osIsUUID(String thing)
        {
            try
            {
                new Guid(thing);
            }
            catch (FormatException)
            {
                this.Verbose("osIsUUID({0})=0", thing);
                return 0;
            }
            this.Verbose("osIsUUID({0})=1", thing);
            return 1;
        }

        public integer osListenRegex(int channelID, String name, String ID, String msg, int regexBitfield)
        {
            this.Verbose("osListenRegex({0}, \"{1}\", \"{2}\", \"{3}\", {4})=1", channelID, name, ID, msg, regexBitfield);
            return 1;
        }

        public void osMessageAttachments(key avatar, String message, list attachmentPoints, integer options)
        {
            this.Verbose("osMessageAttachments(\"{0}\", \"{1}\", {2}, {3})", avatar, message, attachmentPoints.ToVerboseString(), options);
        }

        public void osMessageObject(key objectUUID, String message)
        {
            this.Verbose("osMessageObject(\"{0}\", \"{1}\")", objectUUID, message);
        }

        public void osSetInertia(Float mass, vector centerOfMass, vector principalInertiaScaled, rotation InertiaRot)
        {
            this.Verbose("osSetInertia({0}, {1}, {2}, {3})", mass, centerOfMass, principalInertiaScaled, InertiaRot);
        }

        public void osSetInertiaAsBox(Float mass, vector boxSize, vector centerOfMass, rotation rot)
        {
            this.Verbose("osSetInertiaAsBox({0}, {1}, {2}, {3})", mass, boxSize, centerOfMass, rot);
        }

        public void osSetInertiaAsCylinder(Float mass, Float radius, Float length, vector centerOfMass, rotation rot)
        {
            this.Verbose("osSetInertiaAsCylinder({0}, {1}, {2}, {3}, {4})", mass, radius, length, centerOfMass, rot);
        }

        public void osSetInertiaAsSphere(Float mass, Float radius, vector centerOfMass)
        {
            this.Verbose("osSetInertiaAsSphere({0}, {1}, {2})", mass, radius, centerOfMass);
        }

        public void osSetPrimitiveParams(key prim, list rules)
        {
            this.Verbose("osSetPrimitiveParams(\"{0}\", {1})", prim, rules.ToVerboseString());
        }

        public void osSetProjectionParams(integer projection, key texture, Float fov, Float focus, Float amb)
        {
            this.Verbose("osSetProjectionParams({0}, \"{1}\", {2}, {3}, {4})", projection, texture, fov, focus, amb);
        }

        public void osSetProjectionParams(key prim, integer projection, key texture, Float fov, Float focus, Float amb)
        {
            this.Verbose("osSetProjectionParams(\"{0}\", {1}, \"{2}\", {3}, {4}, {5})", prim, projection, texture, fov, focus, amb);
        }

        public void osSetSpeed(String UUID, Float SpeedModifier)
        {
            this.Verbose("osSetSpeed(\"{0}\", {1})", UUID, SpeedModifier);
        }

        public integer osTeleportObject(key objectUUID, vector targetPos, rotation rot, integer flags)
        {
            this.Verbose("osTeleportObject(\"{0}\", {1}, {2}, {3})=1", objectUUID, targetPos, rot, flags);
            return 1;
        }

        public String osDrawEllipse(String drawList, integer width, integer height)
        {
            this.Verbose("osDrawEllipse(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawFilledEllipse(String drawList, integer width, integer height)
        {
            this.Verbose("osDrawFilledEllipse(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawFilledPolygon(String drawList, list xpoints, list ypoints)
        {
            this.Verbose("osDrawFilledPolygon(\"{0}\", {1}, {2})=\"\"", drawList, xpoints.ToVerboseString(), ypoints.ToVerboseString());
            return "";
        }

        public String osDrawFilledRectangle(String drawList, integer width, integer height)
        {
            this.Verbose("osDrawFilledRectangle(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawImage(String drawList, integer width, integer height, String imageUrl)
        {
            this.Verbose("osDrawImage(\"{0}\", {1}, {2}, \"{3}\")=\"\"", drawList, width, height, imageUrl);
            return "";
        }

        public String osDrawLine(String drawList, integer startX, integer startY, integer endX, integer endY)
        {
            this.Verbose("osDrawLine(\"{0}\", {1}, {2}, {3}, {4})=\"\"", drawList, startX, startY, endX, endY);
            return "";
        }

        public String osDrawLine(String drawList, integer endX, integer endY)
        {
            this.Verbose("osDrawLine(\"{0}\", {1}, {2}, {3}, {4})=\"\"", drawList, endX, endY);
            return "";
        }

        public String osDrawPolygon(String drawList, list xpoints, list ypoints)
        {
            this.Verbose("osDrawPolygon(\"{0}\", {1}, {2})=\"\"", drawList, xpoints.ToVerboseString(), ypoints.ToVerboseString());
            return "";
        }

        public String osDrawRectangle(String drawList, integer width, integer height)
        {
            this.Verbose("osDrawRectangle(\"{0}\", {1}, {2})=\"\"", drawList, width, height);
            return "";
        }

        public String osDrawResetTransform(String drawList)
        {
            this.Verbose("osDrawResetTransform(\"{0}\")=\"\"", drawList);
            return "";
        }

        public String osDrawRotationTransform(String drawList, Float x)
        {
            this.Verbose("osDrawRotationTransform(\"{0}\", {1})=\"\"", drawList, x);
            return "";
        }

        public String osDrawScaleTransform(String drawList, Float x, Float y)
        {
            this.Verbose("osDrawScaleTransform(\"{0}\", {1}, {2})=\"\"", drawList, x, y);
            return "";
        }

        public String osDrawText(String drawList, String text)
        {
            this.Verbose("osDrawText(\"{0}\", \"{1}\")=\"\"", drawList, text);
            return "";
        }

        public String osDrawTranslationTransform(String drawList, Float x, Float y)
        {
            this.Verbose("osDrawTranslationTransform(\"{0}\", {1}, {2})=\"\"", drawList, x, y);
            return "";
        }

        public vector osGetDrawStringSize(String contentType, String text, String fontName, integer fontSize)
        {
            var v = new vector();
            this.Verbose("osGetDrawStringSize(\"{0}\", \"{1}\", \"{2}\", {3})={4}", contentType, text, fontName, fontSize, v);
            return v;
        }

        public String osMovePen(String drawList, integer x, integer y)
        {
            this.Verbose("osMovePen(\"{0}\", {1}, {2})=\"\"", drawList, x, y);
            return "";
        }

        public String osSetFontName(String drawList, String fontName)
        {
            this.Verbose("osSetFontName(\"{0}\", \"{1}\")=\"\"", drawList, fontName);
            return "";
        }

        public String osSetFontSize(String drawList, integer fontSize)
        {
            this.Verbose("osSetFontSize(\"{0}\", {1})=\"\"", drawList, fontSize);
            return "";
        }

        public String osSetPenCap(String drawList, String direction, String type)
        {
            this.Verbose("osSetPenCap(\"{0}\", \"{1}\", \"{2}\")=\"\"", drawList, direction, type);
            return "";
        }

        public String osSetPenColor(String drawList, String color)
        {
            this.Verbose("osSetPenColor(\"{0}\", \"{1}\")=\"\"", drawList, color);
            return "";
        }

        public String osSetPenSize(String drawList, integer penSize)
        {
            this.Verbose("osSetPenSize(\"{0}\", {1})=\"\"", drawList, penSize);
            return "";
        }

        public String osSetDynamicTextureData(String dynamicID, String contentType, String data, String extraParams, integer timer)
        {
            this.Verbose("osSetDynamicTextureData(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4})=\"\"", dynamicID, contentType, data, extraParams, timer);
            return "";
        }

        public String osSetDynamicTextureDataBlend(String dynamicID, String contentType, String data, String extraParams, integer timer, integer alpha)
        {
            this.Verbose("osSetDynamicTextureDataBlend(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5})=\"\"",
                dynamicID, contentType, data, extraParams, timer, alpha);
            return "";
        }

        public String osSetDynamicTextureDataBlendFace(String dynamicID, String contentType, String data, String extraParams, integer blend, integer disp, integer timer, integer alpha, integer face)
        {
            this.Verbose("osSetDynamicTextureDataBlendFace(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5}, {6}, {7}, {8})=\"\"",
                dynamicID, contentType, data, extraParams, blend, disp, timer, alpha, face);
            return "";
        }

        public String osSetDynamicTextureDataFace(String dynamicID, String contentType, String data, String extraParams, integer timer, integer face)
        {
            this.Verbose("osSetDynamicTextureDataFace(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5})=\"\"",
                dynamicID, contentType, data, extraParams, timer, face);
            return "";
        }

        public String osSetDynamicTextureURL(String dynamicID, String contentType, String url, String extraParams, integer timer)
        {
            this.Verbose("osSetDynamicTextureURL(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4})=\"\"",
                dynamicID, contentType, url, extraParams, timer);
            return "";
        }

        public String osSetDynamicTextureURLBlend(String dynamicID, String contentType, String url, String extraParams, integer timer, integer alpha)
        {
            this.Verbose("osSetDynamicTextureURLBlend(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5})=\"\"",
                dynamicID, contentType, url, extraParams, timer, alpha);
            return "";
        }

        public String osSetDynamicTextureURLBlendFace(String dynamicID, String contentType, String url, String extraParams, integer blend, integer disp, integer timer, integer alpha, integer face)
        {
            this.Verbose("osSetDynamicTextureURLBlendFace(\"{0}\", \"{1}\", \"{2}\", \"{3}\", {4}, {5}, {6}, {7}, {8})=\"\"",
                dynamicID, contentType, url, extraParams, blend, disp, timer, alpha, face);
            return "";
        }

        public String osGetNotecard(String name)
        {
            var strPath = this.host.efMainForm.SolutionExplorer.GetPath(this.host.GUID, name);

            if (strPath?.Length == 0)
            {
                strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }

            if (!File.Exists(strPath))
            {
                this.host.VerboseMessage("Notecard: " + strPath + " not found");
                this.host.ExecuteSecondLife("llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
                this.Verbose("osGetNotecard(\"{0}\")=\"\"", name);
                return "";
            }

            var sr = new StreamReader(strPath);
            var result = new StringBuilder();

            while (!sr.EndOfStream)
            {
                result.Append(sr.ReadLine()).Append("\n");
            }
            sr.Close();

            this.Verbose("osGetNotecard(\"{0}\")=\"{1}\"", name, result.ToString());
            return result.ToString();
        }

        public String osGetNotecardLine(String name, integer line)
        {
            var strPath = this.host.efMainForm.SolutionExplorer.GetPath(this.host.GUID, name);

            if (strPath?.Length == 0)
            {
                strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }

            if (!File.Exists(strPath))
            {
                this.host.VerboseMessage("Notecard: " + strPath + " not found");
                this.host.ExecuteSecondLife("llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
                this.Verbose("osGetNotecardLine(\"{0}\", {1})=\"\"", name, line);
                return "";
            }

            var sr = new StreamReader(strPath);
            var result = "";
            var i = 1;

            while (!sr.EndOfStream)
            {
                if (i == line)
                {
                    result = sr.ReadLine();
                }
                else
                {
                    sr.ReadLine();
                }
                i++;
            }
            sr.Close();

            this.Verbose("osGetNotecardLine(\"{0}\", {1})=\"{2}\"", name, line, result);
            return result;
        }

        public integer osGetNumberOfNotecardLines(String name)
        {
            var strPath = this.host.efMainForm.SolutionExplorer.GetPath(this.host.GUID, name);

            if (strPath?.Length == 0)
            {
                strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
            }

            if (!File.Exists(strPath))
            {
                this.host.VerboseMessage("Notecard: " + strPath + " not found");
                this.host.ExecuteSecondLife("llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
                this.Verbose("osGetNumberOfNotecardLines(\"{0}\")=-1", name);
                return -1;
            }

            var sr = new StreamReader(strPath);
            var i = 0;

            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                i++;
            }
            sr.Close();

            this.Verbose("osGetNumberOfNotecardLines(\"{0}\")={1}", name, i);
            return i;
        }

        public void osMakeNotecard(String notecardName, list contents)
        {
            using (var sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), notecardName)))
            {
                foreach (String content in contents.ToArray())
                {
                    sw.WriteLine(content.ToString());
                }
            }

            this.Verbose("osMakeNotecard(\"{0}\", {1})", notecardName, contents.ToVerboseString());
        }

        public void osMakeNotecard(String notecardName, String contents)
        {
            using (var sw = new StreamWriter(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), notecardName)))
            {
                sw.Write(contents.ToString());
            }

            this.Verbose("osMakeNotecard(\"{0}\", \"{1}\")", notecardName, contents);
        }

        public void osRequestSecureURL(list options)
        {
            this.Verbose("osRequestSecureURL({0})", options.ToVerboseString());
        }

        public void osRequestURL(list options)
        {
            this.Verbose("osRequestURL({0})", options.ToVerboseString());
        }

        public void osParcelJoin(vector pos1, vector pos2)
        {
            this.Verbose("osParcelJoin({0}, {1})", pos1, pos2);
        }

        public void osParcelSubdivide(vector pos1, vector pos2)
        {
            this.Verbose("osParcelSubdivide({0}, {1})", pos1, pos2);
        }

        public void osSetParcelDetails(vector pos, list rules)
        {
            this.Verbose("osSetParcelDetails({0}, {1})", pos, rules.ToVerboseString());
        }

        public Float osGetTerrainHeight(integer x, integer y)
        {
            this.Verbose("osGetTerrainHeight({0}, {1})=1", x, y);
            return 1;
        }

        public integer osSetTerrainHeight(integer x, integer y, double value)
        {
            this.Verbose("osSetTerrainHeight({0}, {1}, {2})=1", x, y, value);
            return 1;
        }

        public void osSetTerrainTexture(integer level, key texture)
        {
            this.Verbose("osSetTerrainTexture({0}, \"{1}\")", level, texture);
        }

        public void osSetTerrainTextureHeight(integer corner, Float low, Float high)
        {
            this.Verbose("osSetTerrainTextureHeight({0}, {1}, {2})", corner, low, high);
        }

        public void osTerrainFlush()
        {
            this.Verbose("osTerrainFlush()");
        }

        public Float osGetCurrentSunHour()
        {
            this.Verbose("osGetCurrentSunHour()=0");
            return 0;
        }

        public Float osGetSunParam(String param)
        {
            this.Verbose("osGetSunParam(\"{0}\")=0", param);
            return 0;
        }

        public Float osGetWindParam(String plugin, String param)
        {
            this.Verbose("osGetWindParam(\"{0}\", \"{1}\")=0", plugin, param);
            return 0;
        }

        public void osSetEstateSunSettings(integer sunFixed, Float sunHour)
        {
            this.Verbose("osSetEstateSunSettings({0}, {1})", sunFixed, sunHour);
        }

        public void osSetRegionSunSettings(integer useEstateSun, integer sunFixed, Float sunHour)
        {
            this.Verbose("osSetRegionSunSettings({0}, {1}, {2})", useEstateSun, sunFixed, sunHour);
        }

        public void osSetRegionWaterHeight(Float height)
        {
            this.Verbose("osSetRegionWaterHeight({0})", height);
        }

        public void osSetSunParam(String param, Float value)
        {
            this.Verbose("osSetSunParam(\"{0}\", {1})", param, value);
        }

        public void osSetWindParam(String plugin, String param, Float value)
        {
            this.Verbose("osSetWindParam(\"{0}\", \"{1}\", {2})", plugin, param, value);
        }

        public String osWindActiveModelPluginName()
        {
            this.Verbose("osWindActiveModelPluginName()=\"\"");
            return "";
        }

        public integer osCheckODE()
        {
            this.Verbose("osCheckODE()=0");
            return 0;
        }

        public String osGetGridCustom(String key)
        {
            this.Verbose("osGetGridCustom(\"{0}\")=\"\"", key);
            return "";
        }

        public String osGetGridGatekeeperURI()
        {
            this.Verbose("osGetGridGatekeeperURI()=\"\"");
            return "";
        }

        public String osGetGridHomeURI()
        {
            this.Verbose("osGetGridHomeURI()=\"\"");
            return "";
        }

        public String osGetGridLoginURI()
        {
            this.Verbose("osGetGridLoginURI()=\"\"");
            return "";
        }

        public String osGetGridName()
        {
            this.Verbose("osGetGridName()=\"\"");
            return "";
        }

        public String osGetGridNick()
        {
            this.Verbose("osGetGridNick()=\"\"");
            return "";
        }

        public key osGetMapTexture()
        {
            var k = new key(new Guid());
            this.Verbose("osGetMapTexture()=\"{0}\"", k);
            return k;
        }

        public String osGetPhysicsEngineName()
        {
            this.Verbose("osGetPhysicsEngineName()=\"\"");
            return "";
        }

        public String osGetPhysicsEngineType()
        {
            this.Verbose("osGetPhysicsEngineType()=\"\"");
            return "";
        }

        public key osGetRegionMapTexture(String regionName)
        {
            var k = new key(new Guid());
            this.Verbose("osGetRegionMapTexture(\"{0}\")=\"{1}\"", regionName, k);
            return k;
        }

        public vector osGetRegionSize()
        {
            var v = new vector(256, 256, 0);
            this.Verbose("osGetRegionSize()={0}", v);
            return v;
        }

        public list osGetRegionStats()
        {
            var l = new list();
            this.Verbose("osGetRegionStats()={0}", l.ToVerboseString());
            return l;
        }

        public String osGetScriptEngineName()
        {
            this.Verbose("osGetScriptEngineName()=\"\"");
            return "";
        }

        public integer osGetSimulatorMemory()
        {
            this.Verbose("osGetSimulatorMemory()=0");
            return 0;
        }

        public String osGetSimulatorVersion()
        {
            this.Verbose("osGetSimulatorVersion()=\"\"");
            return "";
        }

        public String osLoadedCreationDate()
        {
            this.Verbose("osLoadedCreationDate()=\"\"");
            return "";
        }

        public String osLoadedCreationID()
        {
            this.Verbose("osLoadedCreationID()=\"\"");
            return "";
        }

        public String osLoadedCreationTime()
        {
            this.Verbose("osLoadedCreationTime()=\"\"");
            return "";
        }

        public integer osConsoleCommand(String command)
        {
            this.Verbose("osConsoleCommand(\"{0}\")=1", command);
            return 1;
        }

        public void osRegionNotice(String msg)
        {
            this.Verbose("osRegionNotice(\"{0}\")", msg);
        }

        public integer osRegionRestart(Float seconds)
        {
            this.Verbose("osRegionRestart({0})={1}", seconds, 1);
            return 1;
        }

        public void osSetParcelMediaURL(String url)
        {
            this.Verbose("osSetParcelMediaURL(\"{0}\")", url);
        }

        public void osSetParcelSIPAddress(String SIPAddress)
        {
            this.Verbose("osSetParcelSIPAddress(\"{0}\")", SIPAddress);
        }

        public void osSetPrimFloatOnWater(integer f)
        {
            this.Verbose("osSetPrimFloatOnWater({0})", f);
        }

        public void osGrantScriptPermissions(key allowed_key, String function)
        {
            this.Verbose("osGrantScriptPermissions(\"{0}\", \"{1}\")", allowed_key, function);
        }

        public void osRevokeScriptPermissions(key revoked_key, String function)
        {
            this.Verbose("osRevokeScriptPermissions(\"{0}\", \"{1}\")", revoked_key, function);
        }

        public void osCollisionSound(String impact_sound, double impact_volume)
        {
            this.Verbose("osCollisionSound(\"{0}\", {1})", impact_sound, impact_volume);
        }

        public Float osDie(key objectUUID)
        {
            this.Verbose("osDie(\"{0}\")=1", objectUUID);
            return 1;
        }

        public String osFormatString(String format, list parameters)
        {
            var formatted = string.Format(format, parameters.ToArray());

            this.Verbose("osFormatString(\"{0}\", {1})=\"{2}\"", format, parameters.ToVerboseString(), formatted);
            return formatted;
        }

        public String osKey2Name(key id)
        {
            this.Verbose("osKey2Name(\"{0}\")=\"\"", id);
            return "";
        }

        public Float osList2Double(list src, integer index)
        {
            if (index >= src.Count)
            {
                this.Verbose("osList2Double({0}, {1})={2}", src.ToVerboseString(), index, -1);
                return -1;
            }
            else if (src[index].GetType() == typeof(string))
            {
                this.Verbose("osList2Double({0}, {1})={2}", src.ToVerboseString(), index, float.Parse((string)src[index]));
                return float.Parse((string)src[index]);
            }
            else
            {
                Float f = float.Parse(src[index].ToString());
                this.Verbose("osList2Double({0}, {1})={2}", src.ToVerboseString(), index, src[index]);
                return f;
            }
        }

        public list osMatchString(string src, String pattern, integer start)
        {
            var result = new list();

            if (start < 0)
            {
                start = src.Length + start;
            }

            if (start < 0 || start >= src.Length)
            {
                this.Verbose("osMatchString(\"{0}\", \"{1}\", {2})={3}", src, pattern, start, result.ToVerboseString());
                return result;  // empty list 
            }

            // Find matches beginning at start position
            var matcher = new Regex(pattern);
            var match = matcher.Match(src, start);
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
            this.Verbose("osMatchString(\"{0}\", \"{1}\", {2})={3}", src, pattern, start, result.ToVerboseString());
            return result;
        }

        public Float osMax(double a, double b)
        {
            var max = (Float)Math.Max(a, b);

            this.Verbose("osMax({0}, {1})={2}", a, b, max);
            return max;
        }

        public Float osMin(double a, double b)
        {
            var min = (Float)Math.Min(a, b);

            this.Verbose("osMin({0}, {1})={2}", a, b, min);
            return min;
        }

        public Hashtable osParseJSON(String JSON)
        {
            var serializer = new JavaScriptSerializer()
            {
                MaxJsonLength = JSON.ToString().Length,
            };
            var decoded = serializer.DeserializeObject(JSON);
            if (decoded is Hashtable hashtable)
            {
                this.Verbose("osParseJSON(\"{0}\")={1}", JSON, hashtable);
                return hashtable;
            }
            else if (decoded is ArrayList arraies)
            {
                var decoded_list = arraies;
                var fakearray = new Hashtable();
                var i = 0;
                for (i = 0; i < decoded_list.Count; i++)
                {
                    fakearray.Add(i, decoded_list[i]);
                }

                this.Verbose("osParseJSON(\"{0}\")={1}", JSON, new list(decoded_list.ToArray()).ToVerboseString());
                return fakearray;
            }
            else
            {
                // It can be a recursive from Dictionary<string, object>.Value too
                this.Chat(0, "osParseJSON: unable to parse JSON string " + JSON, CommunicationType.Say);
                this.Verbose("osParseJSON(\"{0}\")={1}", JSON, new list().ToVerboseString());
                return null;
            }
        }

        public object osParseJSONNew(String JSON)
        {
            var serializer = new JavaScriptSerializer()
            {
                MaxJsonLength = JSON.ToString().Length,
            };
            var decoded = serializer.DeserializeObject(JSON);
            this.Verbose("osParseJSONNew(\"{0}\")={1}", JSON, decoded);
            return decoded;
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
                this.Verbose("osReplaceString(\"{0}\", \"{1}\", \"{2}\", {3}, {4})=\"{5}\"", src, pattern, replace, count, start, src);
                return src;
            }

            // Find matches beginning at start position
            var matcher = new Regex(pattern);
            var result = matcher.Replace(src, replace, count, start);
            this.Verbose("osReplaceString(\"{0}\", \"{1}\", \"{2}\", {3}, {4})=\"{5}\"", src, pattern, replace, count, start, result);

            return result;
        }

        public integer osRegexIsMatch(String input, String pattern)
        {
            integer result = Regex.IsMatch(input, pattern);
            this.Verbose("osRegexIsMatch(\"{0}\", \"{1}\")={2}", input, pattern, result);

            return result;
        }

        public void osSetContentType(key id, string type)
        {
            this.Verbose("osSetContentType(\"{0}\", \"{1}\")", id, type);
        }

        public void osSetStateEvents(integer events)
        {
            this.Verbose("osSetStateEvents({0})", events);
        }

        public string osUnixTimeToTimestamp(integer epoch)
        {
            const long baseTicks = 621355968000000000;
            const long tickResolution = 10000000;
            long epochTicks = (epoch * tickResolution) + baseTicks;
            var date = new DateTime(epochTicks);
            var result = date.ToString("yyyy-MM-ddTHH:mm:ss.fffffffZ");

            this.Verbose("osUnixTimeToTimestamp({0})=\"{1}\"", epoch, result);

            return result;
        }

        public void osVolumeDetect(integer detect)
        {
            this.Verbose("osVolumeDetect({0})", detect);
        }
    }
}
