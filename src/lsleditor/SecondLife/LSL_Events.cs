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
// LSL_Events.cs
//
// </summary>

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace LSLEditor
{
	/// <summary>
	/// This part of the SecondLife class contains the Event definitions.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1502:ElementMustNotBeOnSingleLine", Justification = "Reviewed.")]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "These are all LSL functions, the documentation is in the LSL Wiki.")]
	public partial class SecondLife
	{
		#region events as virtuals, if one is used, use override
		public virtual void at_rot_target(integer iHandle, rotation rTargetRotation, rotation rCurrentRotation) { }

		public virtual void at_target(integer iHandle, vector vTargetPosition, vector vCurrrentPosition) { }

		public virtual void attach(key kAttached) { }

		public virtual void changed(integer iChangedFlags) { }

		public virtual void collision(integer iCollisionCount) { }

		public virtual void collision_end(integer iCollisionCount) { }

		public virtual void collision_start(integer iCollisionCount) { }

		public virtual void control(key kID, integer iLevels, integer iEdges) { }

		public virtual void dataserver(key kRequestID, String sData) { }

		public virtual void email(String sTime, String sAddress, String sSubject, String sBody, integer iRemaining) { }

		public virtual void http_response(key kRequestID, integer iStatus, list lMetadata, String sBody) { }

		public virtual void http_request(key kRequestID, String sMethod, String sBody) { }

		public virtual void land_collision(vector vPosition) { }

		public virtual void land_collision_end(vector vPosition) { }

		public virtual void land_collision_start(vector vPosition) { }

		public virtual void link_message(integer iSenderLinkIndex, integer iNumber, String sText, key kID) { }

		public virtual void listen(integer iChannel, String sName, key kID, String sText) { }

		public virtual void money(key kPayerID, integer iAmount) { }

		public virtual void moving_end() { }

		public virtual void moving_start() { }

		public virtual void no_sensor() { }

		public virtual void not_at_rot_target() { }

		public virtual void not_at_target() { }

		public virtual void object_rez(key kID) { }

		public virtual void on_rez(integer iStartParameter) { }

		public virtual void path_update(integer iType, list lReserved) { }

		public virtual void remote_data(integer iEventType, key kChannelID, key kMessageID, String sSender, integer iData, String sData) { }

		public virtual void run_time_permissions(integer iPermissionsFlags) { }

		public virtual void sensor(integer iSensedCount) { }

		public virtual void state_entry() { }

		public virtual void state_exit() { }

		public virtual void timer() { }

		public virtual void touch(integer iTouchesCount) { }

		public virtual void touch_end(integer iTouchesCount) { }

		public virtual void touch_start(integer iTouchesCount) { }

		public virtual void transaction_result(key kID, integer iSuccess, String sMessage) { }
		#endregion
	}
}
