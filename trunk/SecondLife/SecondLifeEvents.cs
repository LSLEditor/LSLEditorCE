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
		virtual public void at_rot_target(integer number, rotation target_rotation, rotation our_rotation) { }
		virtual public void at_target(integer tnum, vector targetpos, vector ourpos) { }
		virtual public void attach(key attached) { }
		virtual public void changed(integer changed) { }
		virtual public void collision(integer total_number) { }
		virtual public void collision_end(integer total_number) { }
		virtual public void collision_start(integer total_number) { }
		virtual public void control(key name, integer levels, integer edges) { }
		virtual public void dataserver(key requested, String data) { }
		virtual public void email(String time, String address, String subject, String body, integer remaining) { }
		virtual public void http_response(key request_id, integer status, list metadata, String body) { }
		virtual public void http_request(key request_id, String method, String body) { }
		virtual public void land_collision(vector position) { }
		virtual public void land_collision_end(vector position) { }
		virtual public void land_collision_start(vector position) { }
		virtual public void link_message(integer sender_number, integer number, String message, key id) { }
		virtual public void listen(integer channel, String name, key id, String message) { }
		virtual public void money(key giver, integer amount) { }
		virtual public void moving_end() { }
		virtual public void moving_start() { }
		virtual public void no_sensor() { }
		virtual public void not_at_rot_target() { }
		virtual public void not_at_target() { }
		virtual public void object_rez(key id) { }
		virtual public void on_rez(integer start_param) { }
		virtual public void path_update(integer iType, list lReserved) { }
		virtual public void remote_data(integer event_type, key channel, key message_id, String sender, integer idata, String sdata) { }
		virtual public void run_time_permissions(integer permissions) { }
		virtual public void sensor(integer total_number) { }
		virtual public void state_entry() { }
		virtual public void state_exit() { }
		virtual public void timer() { }
		virtual public void touch(integer total_number) { }
		virtual public void touch_end(integer total_number) { }
		virtual public void touch_start(integer total_number) { }
		virtual public void transaction_result(key kID, integer iSuccess, String sMessage) { }
		#endregion
	}
}
