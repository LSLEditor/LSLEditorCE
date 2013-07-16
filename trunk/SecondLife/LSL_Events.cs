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
using System.Collections.Generic;
using System.Text;

namespace LSLEditor
{
	public partial class SecondLife
	{
		#region events as virtuals, if one is used, use override
		virtual public void at_rot_target(integer iHandle, rotation rTargetRotation, rotation rCurrentRotation) { }
		virtual public void at_target(integer iHandle, vector vTargetPosition, vector vCurrrentPosition) { }
		virtual public void attach(key kAttached) { }
		virtual public void changed(integer iChangedFlags) { }
		virtual public void collision(integer iCollisionCount) { }
		virtual public void collision_end(integer iCollisionCount) { }
		virtual public void collision_start(integer iCollisionCount) { }
		virtual public void control(key kID, integer iLevels, integer iEdges) { }
		virtual public void dataserver(key kRequestID, String sData) { }
		virtual public void email(String sTime, String sAddress, String sSubject, String sBody, integer iRemaining) { }
		virtual public void http_response(key kRequestID, integer iStatus, list lMetadata, String sBody) { }
		virtual public void http_request(key kRequestID, String sMethod, String sBody) { }
		virtual public void land_collision(vector vPosition) { }
		virtual public void land_collision_end(vector vPosition) { }
		virtual public void land_collision_start(vector vPosition) { }
		virtual public void link_message(integer iSenderLinkIndex, integer iNumber, String sText, key kID) { }
		virtual public void listen(integer iChannel, String sName, key kID, String sText) { }
		virtual public void money(key kPayerID, integer iAmount) { }
		virtual public void moving_end() { }
		virtual public void moving_start() { }
		virtual public void no_sensor() { }
		virtual public void not_at_rot_target() { }
		virtual public void not_at_target() { }
		virtual public void object_rez(key kID) { }
		virtual public void on_rez(integer iStartParameter) { }
		virtual public void path_update(integer iType, list lReserved) { }
		virtual public void remote_data(integer iEventType, key kChannelID, key kMessageID, String sSender, integer iData, String sData) { }
		virtual public void run_time_permissions(integer iPermissionsFlags) { }
		virtual public void sensor(integer iSensedCount) { }
		virtual public void state_entry() { }
		virtual public void state_exit() { }
		virtual public void timer() { }
		virtual public void touch(integer iTouchesCount) { }
		virtual public void touch_end(integer iTouchesCount) { }
		virtual public void touch_start(integer iTouchesCount) { }
		virtual public void transaction_result(key kID, integer iSuccess, String sMessage) { }
		#endregion
	}
}
