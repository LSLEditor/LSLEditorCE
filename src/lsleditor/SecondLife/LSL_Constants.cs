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
// LSL_Constants.cs
//
// </summary>

using System;
using System.Diagnostics.CodeAnalysis;

namespace LSLEditor
{
	/// <summary>
	/// This part of the SecondLife class initialises the constants for LSL.
	/// </summary>
	[SuppressMessage("StyleCop.CSharp.NamingRules", "SA1310:FieldNamesMustNotContainUnderscore", Justification = "Reviewed.")]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", Justification = "Reviewed.")]
	public partial class SecondLife
	{
		public const int TRUE = 1;
		public const int FALSE = 0;

		public const int DENSITY = 1;
		public const int FRICTION = 2;
		public const int GRAVITY_MULTIPLIER = 8;
		public const int RESTITUTION = 4;

		public const int AGENT = 1;
		public const int ACTIVE = 2;
		public const int PASSIVE = 4;
		public const int SCRIPTED = 8;

		public const int ANIM_ON = 1;
		public const int LOOP = 2;
		public const int REVERSE = 4;
		public const int PING_PONG = 8;
		public const int SMOOTH = 16;
		public const int ROTATE = 32;
		public const int SCALE = 64;

		public const int ALL_SIDES = -1;

		public const int DEBUG_CHANNEL = 2147483647;
		public const string EOF = "\n\n\n";

		public const int AGENT_FLYING = 1;
		public const int AGENT_ATTACHMENTS = 2;
		public const int AGENT_SCRIPTED = 4;
		public const int AGENT_MOUSELOOK = 8;
		public const int AGENT_SITTING = 16;
		public const int AGENT_ON_OBJECT = 32;
		public const int AGENT_AWAY = 64;
		public const int AGENT_WALKING = 128;
		public const int AGENT_IN_AIR = 256;
		public const int AGENT_TYPING = 512;
		public const int AGENT_CROUCHING = 1024;
		public const int AGENT_BUSY = 2048;
		public const int AGENT_ALWAYS_RUN = 4096;
		public const int AGENT_AUTOPILOT = 0x2000;
		public const int AGENT_BY_LEGACY_NAME = 0x1;
		public const int AGENT_BY_USERNAME = 0x10;

		public const int AGENT_LIST_PARCEL = 0x01;
		public const int AGENT_LIST_PARCEL_OWNER = 0x02;
		public const int AGENT_LIST_REGION = 0x04;

		public const int ATTACH_AVATAR_CENTER = 40;
		public const int ATTACH_BACK = 9;
		public const int ATTACH_CHEST = 1;
		public const int ATTACH_CHIN = 12;
		public const int ATTACH_HEAD = 2;
		public const int ATTACH_LSHOULDER = 3;
		public const int ATTACH_RSHOULDER = 4;
		public const int ATTACH_LHAND = 5;
		public const int ATTACH_RHAND = 6;
		public const int ATTACH_LFOOT = 7;
		public const int ATTACH_RFOOT = 8;
		public const int ATTACH_PELVIS = 10;
		public const int ATTACH_MOUTH = 11;
		public const int ATTACH_NECK = 39;
		public const int ATTACH_LEAR = 13;
		public const int ATTACH_LEFT_PEC = 29;
		public const int ATTACH_REAR = 14;
		public const int ATTACH_LEYE = 15;
		public const int ATTACH_REYE = 16;
		public const int ATTACH_NOSE = 17;
		public const int ATTACH_RUARM = 18;
		public const int ATTACH_RLARM = 19;
		public const int ATTACH_LUARM = 20;
		public const int ATTACH_LLARM = 21;
		public const int ATTACH_RHIP = 22;
		public const int ATTACH_RIGHT_PEC = 30;
		public const int ATTACH_RULEG = 23;
		public const int ATTACH_RLLEG = 24;
		public const int ATTACH_LHIP = 25;
		public const int ATTACH_LULEG = 26;
		public const int ATTACH_LLLEG = 27;
		public const int ATTACH_BELLY = 28;
		public const int ATTACH_RPEC = 29;
		public const int ATTACH_LPEC = 30;

		public const int ATTACH_HUD_CENTER_2 = 31;
		public const int ATTACH_HUD_TOP_RIGHT = 32;
		public const int ATTACH_HUD_TOP_CENTER = 33;
		public const int ATTACH_HUD_TOP_LEFT = 34;
		public const int ATTACH_HUD_CENTER_1 = 35;
		public const int ATTACH_HUD_BOTTOM_LEFT = 36;
		public const int ATTACH_HUD_BOTTOM = 37;
		public const int ATTACH_HUD_BOTTOM_RIGHT = 38;

		public const int AVOID_NONE = 0;
		public const int AVOID_CHARACTERS = 1;
		public const int AVOID_DYNAMIC_OBSTACLES = 2;

		public const int CAMERA_PITCH = 0;
		public const int CAMERA_FOCUS_OFFSET = 1;
		public const int CAMERA_POSITION_LAG = 5;
		public const int CAMERA_FOCUS_LAG = 6;
		public const int CAMERA_DISTANCE = 7;
		public const int CAMERA_BEHINDNESS_ANGLE = 8;
		public const int CAMERA_BEHINDNESS_LAG = 9;
		public const int CAMERA_POSITION_THRESHOLD = 10;
		public const int CAMERA_FOCUS_THRESHOLD = 11;
		public const int CAMERA_ACTIVE = 12;
		public const int CAMERA_POSITION = 13;
		public const int CAMERA_FOCUS = 17;
		public const int CAMERA_FOCUS_LOCKED = 22;
		public const int CAMERA_POSITION_LOCKED = 21;

		public const int CHANGED_INVENTORY = 1;
		public const int CHANGED_COLOR = 2;
		public const int CHANGED_SHAPE = 4;
		public const int CHANGED_SCALE = 8;
		public const int CHANGED_TEXTURE = 16;
		public const int CHANGED_LINK = 32;
		public const int CHANGED_ALLOWED_DROP = 64;
		public const int CHANGED_OWNER = 128;
		public const int CHANGED_REGION = 256;
		public const int CHANGED_TELEPORT = 512;
		public const int CHANGED_REGION_START = 1024;
		public const int CHANGED_MEDIA = 2048;

		public const int CHARACTER_ACCOUNT_FOR_SKIPPED_FRAMES = 14;
		public const int CHARACTER_AVOIDANCE_MODE = 5;
		public const int CHARACTER_CMD_JUMP = 0x01;
		public const int CHARACTER_CMD_STOP = 0x00;
		public const int CHARACTER_DESIRED_SPEED = 1;
		public const int CHARACTER_LENGTH = 3;
		public const int CHARACTER_TYPE = 6;
		public const int CHARACTER_MAX_ACCEL = 8;
		public const int CHARACTER_MAX_DECEL = 9;
		public const int CHARACTER_MAX_SPEED = 13;
		public const int CHARACTER_MAX_TURN_RADIUS = 10;
		public const int CHARACTER_ORIENTATION = 4;
		public const int CHARACTER_RADIUS = 2;
		public const int CHARACTER_STAY_WITHIN_PARCEL = 15;
		public const int CHARACTER_TYPE_A = 0;
		public const int CHARACTER_TYPE_B = 1;
		public const int CHARACTER_TYPE_C = 2;
		public const int CHARACTER_TYPE_D = 3;
		public const int CHARACTER_TYPE_NONE = 4;

		public const int CLICK_ACTION_NONE = 0;
		public const int CLICK_ACTION_TOUCH = 0;
		public const int CLICK_ACTION_SIT = 1;
		public const int CLICK_ACTION_BUY = 2;
		public const int CLICK_ACTION_PAY = 3;
		public const int CLICK_ACTION_OPEN = 4;
		public const int CLICK_ACTION_PLAY = 5;
		public const int CLICK_ACTION_OPEN_MEDIA = 6;

		public const string CONTENT_TYPE_ATOM = "application/atom+xml";
		public const string CONTENT_TYPE_FORM = "application/x-www-form-urlencoded";
		public const string CONTENT_TYPE_HTML = "text/html";
		public const string CONTENT_TYPE_JSON = "application/json";
		public const string CONTENT_TYPE_LLSD = "application/llsd+xml";
		public const string CONTENT_TYPE_RSS = "application/rss+xml";
		public const string CONTENT_TYPE_TEXT = "text/plain";
		public const string CONTENT_TYPE_XHTML = "application/xhtml+xml";
		public const string CONTENT_TYPE_XML = "application/xml";

		public const int CONTROL_FWD = 1;
		public const int CONTROL_BACK = 2;
		public const int CONTROL_LEFT = 4;
		public const int CONTROL_RIGHT = 8;
		public const int CONTROL_ROT_LEFT = 256;
		public const int CONTROL_ROT_RIGHT = 512;
		public const int CONTROL_UP = 16;
		public const int CONTROL_DOWN = 32;
		public const int CONTROL_LBUTTON = 268435456;
		public const int CONTROL_ML_LBUTTON = 1073741824;

		public const int DATA_ONLINE = 1;
		public const int DATA_NAME = 2;
		public const int DATA_BORN = 3;
		public const int DATA_RATING = 4;

		public const int DATA_SIM_POS = 5;
		public const int DATA_SIM_STATUS = 6;
		public const int DATA_SIM_RATING = 7;

		public const int DATA_PAYINFO = 8;

		public const int ERR_GENERIC = -1;
		public const int ERR_MALFORMED_PARAMS = -3;
		public const int ERR_PARCEL_PERMISSIONS = -2;
		public const int ERR_RUNTIME_PERMISSIONS = -4;
		public const int ERR_THROTTLED = -5;

		public const int ESTATE_ACCESS_ALLOWED_AGENT_ADD = 4;
		public const int ESTATE_ACCESS_ALLOWED_AGENT_REMOVE = 8;
		public const int ESTATE_ACCESS_ALLOWED_GROUP_ADD = 16;
		public const int ESTATE_ACCESS_ALLOWED_GROUP_REMOVE = 32;
		public const int ESTATE_ACCESS_BANNED_AGENT_ADD = 64;
		public const int ESTATE_ACCESS_BANNED_AGENT_REMOVE = 128;

		public const int FORCE_DIRECT_PATH = 1;

		public const int HTTP_BODY_MAXLENGTH = 2;
		public const int HTTP_BODY_TRUNCATED = 0;
		public const int HTTP_METHOD = 0;
		public const int HTTP_MIMETYPE = 1;
		public const int HTTP_PRAGMA_NO_CACHE = 6;
		public const int HTTP_VERBOSE_THROTTLE = 4;
		public const int HTTP_VERIFY_CERT = 3;

		public const int HORIZONTAL = 1;

		public const int INVENTORY_ALL = -1;
		public const int INVENTORY_NONE = -1;
		public const int INVENTORY_TEXTURE = 0;
		public const int INVENTORY_SOUND = 1;
		public const int INVENTORY_LANDMARK = 3;
		public const int INVENTORY_CLOTHING = 5;
		public const int INVENTORY_OBJECT = 6;
		public const int INVENTORY_NOTECARD = 7;
		public const int INVENTORY_SCRIPT = 10;
		public const int INVENTORY_BODYPART = 13;
		public const int INVENTORY_ANIMATION = 20;
		public const int INVENTORY_GESTURE = 21;

		public const int JSON_APPEND = -1;
		public const string JSON_ARRAY = "\uFDD2";
		public const string JSON_DELETE = "\uFDD8";
		public const string JSON_FALSE = "\uFDD7";
		public const string JSON_INVALID = "\uFDD0";
		public const string JSON_NULL = "\uFDD5";
		public const string JSON_NUMBER = "\uFDD3";
		public const string JSON_OBJECT = "\uFDD1";
		public const string JSON_STRING = "\uFDD4";
		public const string JSON_TRUE = "\uFDD6";

		public const int KFM_CMD_PAUSE = 2;
		public const int KFM_CMD_PLAY = 0;
		public const int KFM_CMD_STOP = 1;
		public const int KFM_COMMAND = 0;
		public const int KFM_DATA = 2;
		public const int KFM_FORWARD = 0;
		public const int KFM_LOOP = 1;
		public const int KFM_MODE = 1;
		public const int KFM_PING_PONG = 2;
		public const int KFM_REVERSE = 2;
		public const int KFM_ROTATION = 1;
		public const int KFM_TRANSLATION = 2;

		public const int LAND_LEVEL = 0;
		public const int LAND_RAISE = 1;
		public const int LAND_LOWER = 2;
		public const int LAND_SMOOTH = 3;
		public const int LAND_NOISE = 4;
		public const int LAND_REVERT = 5;

		public const int LAND_LARGE_BRUSH = 3;
		public const int LAND_MEDIUM_BRUSH = 2;
		public const int LAND_SMALL_BRUSH = 1;

		public const int LINK_ROOT = 1;
		public const int LINK_SET = -1;
		public const int LINK_ALL_OTHERS = -2;
		public const int LINK_ALL_CHILDREN = -3;
		public const int LINK_THIS = -4;

		public const int LIST_STAT_RANGE = 0;
		public const int LIST_STAT_MIN = 1;
		public const int LIST_STAT_MAX = 2;
		public const int LIST_STAT_MEAN = 3;
		public const int LIST_STAT_MEDIAN = 4;
		public const int LIST_STAT_STD_DEV = 5;
		public const int LIST_STAT_SUM = 6;
		public const int LIST_STAT_SUM_SQUARES = 7;
		public const int LIST_STAT_NUM_COUNT = 8;
		public const int LIST_STAT_GEOMETRIC_MEAN = 9;

		public const int MASK_BASE = 0;
		public const int MASK_EVERYONE = 3;
		public const int MASK_GROUP = 2;
		public const int MASK_NEXT = 4;
		public const int MASK_OWNER = 1;

		public const int OBJECT_ATTACHED_POINT = 19;
		public const int OBJECT_CHARACTER_TIME = 17;
		public const int OBJECT_NAME = 1;
		public const int OBJECT_DESC = 2;
		public const int OBJECT_POS = 3;
		public const int OBJECT_ROOT = 18;
		public const int OBJECT_ROT = 4;
		public const int OBJECT_VELOCITY = 5;
		public const int OBJECT_OWNER = 6;
		public const int OBJECT_PATHFINDING_TYPE = 20;
		public const int OBJECT_GROUP = 7;
		public const int OBJECT_CREATOR = 8;
		public const int OBJECT_PHYSICS = 21;
		public const int OBJECT_PHANTOM = 22;
		public const int OBJECT_TEMP_ON_REZ = 23;

		public const int OBJECT_STREAMING_COST = 15;
		public const int OBJECT_PHYSICS_COST = 16;
		public const int OBJECT_SERVER_COST = 14;
		public const int OBJECT_PRIM_EQUIVALENCE = 13;

		public const int OBJECT_RUNNING_SCRIPT_COUNT = 9;
		public const int OBJECT_SCRIPT_MEMORY = 11;
		public const int OBJECT_SCRIPT_TIME = 12;
		public const int OBJECT_TOTAL_SCRIPT_COUNT = 10;
		public const int OBJECT_UNKNOWN_DETAIL = -1;

		public const int OPT_AVATAR = 1;
		public const int OPT_CHARACTER = 2;
		public const int OPT_EXCLUSION_VOLUME = 6;
		public const int OPT_LEGACY_LINKSET = 0;
		public const int OPT_MATERIAL_VOLUME = 5;
		public const int OPT_OTHER = -1;
		public const int OPT_STATIC_OBSTACLE = 4;
		public const int OPT_WALKABLE = 3;

		public const int OBJECT_RETURN_PARCEL = 1;
		public const int OBJECT_RETURN_PARCEL_OWNER = 2;
		public const int OBJECT_RETURN_REGION = 4;

		public const int PARCEL_COUNT_TOTAL = 0;
		public const int PARCEL_COUNT_OWNER = 1;
		public const int PARCEL_COUNT_GROUP = 2;
		public const int PARCEL_COUNT_OTHER = 3;
		public const int PARCEL_COUNT_SELECTED = 4;
		public const int PARCEL_COUNT_TEMP = 5;

		public const int PARCEL_DETAILS_AREA = 4;
		public const int PARCEL_DETAILS_DESC = 1;
		public const int PARCEL_DETAILS_GROUP = 3;
		public const int PARCEL_DETAILS_ID = 5;
		public const int PARCEL_DETAILS_NAME = 0;
		public const int PARCEL_DETAILS_OWNER = 2;
		public const int PARCEL_DETAILS_SEE_AVATARS = 6;

		public const int PARCEL_FLAG_ALLOW_FLY = 0x0000001;
		public const int PARCEL_FLAG_ALLOW_SCRIPTS = 0x0000002;
		public const int PARCEL_FLAG_ALLOW_LANDMARK = 0x0000008;
		public const int PARCEL_FLAG_ALLOW_TERRAFORM = 0x0000010;
		public const int PARCEL_FLAG_ALLOW_DAMAGE = 0x0000020;
		public const int PARCEL_FLAG_ALLOW_CREATE_OBJECTS = 0x0000040;
		public const int PARCEL_FLAG_USE_ACCESS_GROUP = 0x0000100;
		public const int PARCEL_FLAG_USE_ACCESS_LIST = 0x0000200;
		public const int PARCEL_FLAG_USE_BAN_LIST = 0x0000400;
		public const int PARCEL_FLAG_USE_LAND_PASS_LIST = 0x0000800;
		public const int PARCEL_FLAG_LOCAL_SOUND_ONLY = 0x0008000;
		public const int PARCEL_FLAG_RESTRICT_PUSHOBJECT = 0x0200000;
		public const int PARCEL_FLAG_ALLOW_GROUP_SCRIPTS = 0x2000000;

		public const int PARCEL_FLAG_ALLOW_CREATE_GROUP_OBJECTS = 0x4000000;
		public const int PARCEL_FLAG_ALLOW_ALL_OBJECT_ENTRY = 0x8000000;
		public const int PARCEL_FLAG_ALLOW_GROUP_OBJECT_ENTRY = 0x10000000;

		public const int PARCEL_MEDIA_COMMAND_STOP = 0;
		public const int PARCEL_MEDIA_COMMAND_PAUSE = 1;
		public const int PARCEL_MEDIA_COMMAND_PLAY = 2;
		public const int PARCEL_MEDIA_COMMAND_LOOP = 3;
		public const int PARCEL_MEDIA_COMMAND_TEXTURE = 4;
		public const int PARCEL_MEDIA_COMMAND_URL = 5;
		public const int PARCEL_MEDIA_COMMAND_TIME = 6;
		public const int PARCEL_MEDIA_COMMAND_AGENT = 7;
		public const int PARCEL_MEDIA_COMMAND_UNLOAD = 8;
		public const int PARCEL_MEDIA_COMMAND_AUTO_ALIGN = 9;

		public const int PARCEL_MEDIA_COMMAND_TYPE = 10;
		public const int PARCEL_MEDIA_COMMAND_SIZE = 11;
		public const int PARCEL_MEDIA_COMMAND_DESC = 12;
		public const int PARCEL_MEDIA_COMMAND_LOOP_SET = 13;

		public const int PAY_DEFAULT = -2;
		public const int PAY_HIDE = -1;

		public const int PAYMENT_INFO_ON_FILE = 1;
		public const int PAYMENT_INFO_USED = 2;

		public const int PERM_ALL = 2147483647;
		public const int PERM_COPY = 32768;
		public const int PERM_MODIFY = 16384;
		public const int PERM_MOVE = 524288;
		public const int PERM_TRANSFER = 8192;

		public const int PERMISSION_DEBIT = 2;
		public const int PERMISSION_TAKE_CONTROLS = 4;
		public const int PERMISSION_REMAP_CONTROLS = 8;
		public const int PERMISSION_TRIGGER_ANIMATION = 16;
		public const int PERMISSION_ATTACH = 32;
		public const int PERMISSION_RELEASE_OWNERSHIP = 64;
		public const int PERMISSION_CHANGE_LINKS = 128;
		public const int PERMISSION_CHANGE_JOINTS = 256;
		public const int PERMISSION_CHANGE_PERMISSIONS = 512;
		public const int PERMISSION_CONTROL_CAMERA = 2048;
		public const int PERMISSION_OVERRIDE_ANIMATIONS = 0x8000;
		public const int PERMISSION_TRACK_CAMERA = 1024;
		public const int PERMISSION_RETURN_OBJECTS = 65536;

		public const int PRIM_BUMP_BARK = 4;
		public const int PRIM_BUMP_BLOBS = 12;
		public const int PRIM_BUMP_BRICKS = 5;
		public const int PRIM_BUMP_BRIGHT = 1;
		public const int PRIM_BUMP_CHECKER = 6;
		public const int PRIM_BUMP_CONCRETE = 7;
		public const int PRIM_BUMP_DARK = 2;
		public const int PRIM_BUMP_DISKS = 10;
		public const int PRIM_BUMP_GRAVEL = 11;
		public const int PRIM_BUMP_LARGETILE = 14;
		public const int PRIM_BUMP_NONE = 0;
		public const int PRIM_BUMP_SHINY = 19;
		public const int PRIM_BUMP_SIDING = 13;
		public const int PRIM_BUMP_STONE = 9;
		public const int PRIM_BUMP_STUCCO = 15;
		public const int PRIM_BUMP_SUCTION = 16;
		public const int PRIM_BUMP_TILE = 8;
		public const int PRIM_BUMP_WEAVE = 17;
		public const int PRIM_BUMP_WOOD = 3;

		public const int PRIM_CAST_SHADOWS = 24;
		public const int PRIM_COLOR = 18;
		public const int PRIM_DESC = 28;
		public const int PRIM_FLEXIBLE = 21;
		public const int PRIM_FULLBRIGHT = 20;
		public const int PRIM_HOLE_CIRCLE = 16;
		public const int PRIM_HOLE_DEFAULT = 0;
		public const int PRIM_HOLE_SQUARE = 32;
		public const int PRIM_HOLE_TRIANGLE = 48;
		public const int PRIM_LINK_TARGET = 34;
		public const int PRIM_MATERIAL = 2;
		public const int PRIM_MATERIAL_FLESH = 4;
		public const int PRIM_MATERIAL_GLASS = 2;
		public const int PRIM_MATERIAL_LIGHT = 7;
		public const int PRIM_MATERIAL_METAL = 1;
		public const int PRIM_MATERIAL_PLASTIC = 5;
		public const int PRIM_MATERIAL_RUBBER = 6;
		public const int PRIM_MATERIAL_STONE = 0;
		public const int PRIM_MATERIAL_WOOD = 3;

		public const int PRIM_MEDIA_ALT_IMAGE_ENABLE = 0;
		public const int PRIM_MEDIA_AUTO_LOOP = 4;
		public const int PRIM_MEDIA_AUTO_PLAY = 5;
		public const int PRIM_MEDIA_AUTO_SCALE = 6;
		public const int PRIM_MEDIA_AUTO_ZOOM = 7;
		public const int PRIM_MEDIA_CONTROLS = 1;
		public const int PRIM_MEDIA_CONTROLS_MINI = 1;
		public const int PRIM_MEDIA_CONTROLS_STANDARD = 0;
		public const int PRIM_MEDIA_CURRENT_URL = 2;
		public const int PRIM_MEDIA_FIRST_CLICK_INTERACT = 8;
		public const int PRIM_MEDIA_HEIGHT_PIXELS = 10;
		public const int PRIM_MEDIA_HOME_URL = 3;
		public const int PRIM_MEDIA_PERM_ANYONE = 4;
		public const int PRIM_MEDIA_PERM_GROUP = 2;
		public const int PRIM_MEDIA_PERM_NONE = 0;
		public const int PRIM_MEDIA_PERM_OWNER = 1;
		public const int PRIM_MEDIA_PERMS_CONTROL = 14;
		public const int PRIM_MEDIA_PERMS_INTERACT = 13;
		public const int PRIM_MEDIA_WHITELIST = 12;
		public const int PRIM_MEDIA_WHITELIST_ENABLE = 11;
		public const int PRIM_MEDIA_WIDTH_PIXELS = 9;

		public const int PRIM_NAME = 27;
		public const int PRIM_OMEGA = 32;
		public const int PRIM_PHANTOM = 5;
		public const int PRIM_PHYSICS = 3;
		public const int PRIM_PHYSICS_MATERIAL = 31;
		public const int PRIM_PHYSICS_SHAPE_CONVEX = 2;
		public const int PRIM_PHYSICS_SHAPE_NONE = 1;
		public const int PRIM_PHYSICS_SHAPE_PRIM = 0;
		public const int PRIM_PHYSICS_SHAPE_TYPE = 30;
		public const int PRIM_POINT_LIGHT = 23;
		public const int PRIM_POSITION = 6;
		public const int PRIM_POS_LOCAL = 33;
		public const int PRIM_ROTATION = 8;
		public const int PRIM_ROT_LOCAL = 29;
		public const int PRIM_SCULPT_FLAG_INVERT = 64;
		public const int PRIM_SCULPT_FLAG_MIRROR = 128;
		public const int PRIM_SHINY_HIGH = 3;
		public const int PRIM_SHINY_LOW = 1;
		public const int PRIM_SHINY_MEDIUM = 2;
		public const int PRIM_SHINY_NONE = 0;
		public const int PRIM_SIZE = 7;
		public const int PRIM_SLICE = 35;
		public const int PRIM_TEMP_ON_REZ = 4;
		public const int PRIM_TEXGEN = 22;
		public const int PRIM_TEXGEN_DEFAULT = 0;
		public const int PRIM_TEXGEN_PLANAR = 1;
		public const int PRIM_TEXT = 26;
		public const int PRIM_TEXTURE = 17;
		public const int PRIM_TYPE = 9;

		public const int PRIM_TYPE_BOX = 0;
		public const int PRIM_TYPE_CYLINDER = 1;
		public const int PRIM_TYPE_PRISM = 2;
		public const int PRIM_TYPE_SPHERE = 3;
		public const int PRIM_TYPE_TORUS = 4;
		public const int PRIM_TYPE_TUBE = 5;
		public const int PRIM_TYPE_RING = 6;
		public const int PRIM_TYPE_SCULPT = 7;

		public const int PRIM_GLOW = 25;

		public const int PRIM_SCULPT_TYPE_MASK = 7;
		public const int PRIM_SCULPT_TYPE_SPHERE = 1;
		public const int PRIM_SCULPT_TYPE_TORUS = 2;
		public const int PRIM_SCULPT_TYPE_PLANE = 3;
		public const int PRIM_SCULPT_TYPE_CYLINDER = 4;

		public const int PROFILE_NONE = 0;
		public const int PROFILE_SCRIPT_MEMORY = 1;

		public const int PSYS_PART_FLAGS = 0;
		public const int PSYS_PART_INTERP_COLOR_MASK = 1;
		public const int PSYS_PART_INTERP_SCALE_MASK = 2;
		public const int PSYS_PART_BOUNCE_MASK = 4;
		public const int PSYS_PART_WIND_MASK = 8;
		public const int PSYS_PART_FOLLOW_SRC_MASK = 16;
		public const int PSYS_PART_FOLLOW_VELOCITY_MASK = 32;
		public const int PSYS_PART_TARGET_POS_MASK = 64;
		public const int PSYS_PART_TARGET_LINEAR_MASK = 128;
		public const int PSYS_PART_EMISSIVE_MASK = 256;

		public const int PSYS_PART_SRC_PATTERN_ANGLE = 0x04;
		public const int PSYS_PART_SRC_PATTERN_ANGLE_CONE = 0x08;
		public const int PSYS_PART_SRC_PATTERN_DROP = 0x01;
		public const int PSYS_PART_SRC_PATTERN_EXPLODE = 0x02;

		public const int PSYS_PART_START_COLOR = 1;
		public const int PSYS_PART_START_ALPHA = 2;
		public const int PSYS_PART_END_COLOR = 3;
		public const int PSYS_PART_END_ALPHA = 4;
		public const int PSYS_PART_START_SCALE = 5;
		public const int PSYS_PART_END_SCALE = 6;
		public const int PSYS_PART_MAX_AGE = 7;

		public const int PSYS_SRC_ACCEL = 8;
		public const int PSYS_SRC_PATTERN = 9;
		public const int PSYS_SRC_INNERANGLE = 10;
		public const int PSYS_SRC_OUTERANGLE = 11;
		public const int PSYS_SRC_TEXTURE = 12;
		public const int PSYS_SRC_BURST_RATE = 13;
		public const int PSYS_SRC_BURST_PART_COUNT = 15;
		public const int PSYS_SRC_BURST_RADIUS = 16;
		public const int PSYS_SRC_BURST_SPEED_MIN = 17;
		public const int PSYS_SRC_BURST_SPEED_MAX = 18;
		public const int PSYS_SRC_MAX_AGE = 19;
		public const int PSYS_SRC_TARGET_KEY = 20;
		public const int PSYS_SRC_OMEGA = 21;
		public const int PSYS_SRC_ANGLE_BEGIN = 22;
		public const int PSYS_SRC_ANGLE_END = 23;

		public const int PSYS_SRC_PATTERN_DROP = 1;
		public const int PSYS_SRC_PATTERN_EXPLODE = 2;
		public const int PSYS_SRC_PATTERN_ANGLE = 4;
		public const int PSYS_SRC_PATTERN_ANGLE_CONE = 8;
		public const int PSYS_SRC_PATTERN_ANGLE_CONE_EMPTY = 16;

		public const int PU_EVADE_HIDDEN = 0x07;
		public const int PU_EVADE_SPOTTED = 0x08;
		public const int PU_FAILURE_DYNAMIC_PATHFINDING_DISABLED = 10;
		public const int PU_FAILURE_PARCEL_UNREACHABLE = 11;
		public const int PU_FAILURE_INVALID_GOAL = 0x03;
		public const int PU_FAILURE_INVALID_START = 0x02;
		public const int PU_FAILURE_NO_VALID_DESTINATION = 0x06;
		public const int PU_FAILURE_OTHER = 1000000;
		public const int PU_FAILURE_TARGET_GONE = 0x05;
		public const int PU_FAILURE_UNREACHABLE = 0x04;
		public const int PU_GOAL_REACHED = 0x01;
		public const int PU_SLOWDOWN_DISTANCE_REACHED = 0x00;

		public const int PUBLIC_CHANNEL = 0;

		public const int PURSUIT_FUZZ_FACTOR = 3;
		public const int PURSUIT_INTERCEPT = 4;
		public const int PURSUIT_OFFSET = 1;

		public const int RC_DATA_FLAGS = 2;
		public const int RC_DETECT_PHANTOM = 1;
		public const int RC_GET_LINK_NUM = 4;
		public const int RC_GET_NORMAL = 1;
		public const int RC_GET_ROOT_KEY = 2;
		public const int RC_MAX_HITS = 3;
		public const int RC_REJECT_AGENTS = 1;
		public const int RC_REJECT_LAND = 8;
		public const int RC_REJECT_NONPHYSICAL = 4;
		public const int RC_REJECT_PHYSICAL = 2;
		public const int RC_REJECT_TYPES = 2;
		public const int RCERR_CAST_TIME_EXCEEDED = -3;
		public const int RCERR_SIM_PERF_LOW = -2;
		public const int RCERR_UNKNOWN = -1;

		public const int REGION_FLAG_ALLOW_DAMAGE = 1;
		public const int REGION_FLAG_FIXED_SUN = 16;
		public const int REGION_FLAG_BLOCK_TERRAFORM = 64;
		public const int REGION_FLAG_SANDBOX = 256;
		public const int REGION_FLAG_DISABLE_COLLISIONS = 4096;
		public const int REGION_FLAG_DISABLE_PHYSICS = 16384;
		public const int REGION_FLAG_BLOCK_FLY = 524288;
		public const int REGION_FLAG_ALLOW_DIRECT_TELEPORT = 1048576;
		public const int REGION_FLAG_RESTRICT_PUSHOBJECT = 4194304;

		public const int REMOTE_DATA_CHANNEL = 1;
		public const int REMOTE_DATA_REQUEST = 2;
		public const int REMOTE_DATA_REPLY = 3;

		public const int REQUIRE_LINE_OF_SIGHT = 2;

		public const int SIM_STAT_PCT_CHARS_STEPPED = 0;

		public const int STATUS_PHYSICS = 1;
		public const int STATUS_ROTATE_X = 2;
		public const int STATUS_ROTATE_Y = 4;
		public const int STATUS_ROTATE_Z = 8;
		public const int STATUS_PHANTOM = 16;
		public const int STATUS_SANDBOX = 32;
		public const int STATUS_BLOCK_GRAB = 64;
		public const int STATUS_BLOCK_GRAB_OBJECT = 1024;
		public const int STATUS_DIE_AT_EDGE = 128;
		public const int STATUS_RETURN_AT_EDGE = 256;
		public const int STATUS_CAST_SHADOWS = 512;
		public const int STATUS_BOUNDS_ERROR = 1002;
		public const int STATUS_INTERNAL_ERROR = 1999;
		public const int STATUS_MALFORMED_PARAMS = 1000;
		public const int STATUS_NOT_FOUND = 1003;
		public const int STATUS_NOT_SUPPORTED = 1004;
		public const int STATUS_OK = 0;
		public const int STATUS_TYPE_MISMATCH = 1001;
		public const int STATUS_WHITELIST_FAILED = 2001;

		public const int STRING_TRIM_HEAD = 1;
		public const int STRING_TRIM_TAIL = 2;
		public const int STRING_TRIM = 3;

		public const string TEXTURE_BLANK = "5748decc-f629-461c-9a36-a35a221fe21f";
		public const string TEXTURE_DEFAULT = "89556747-24cb-43ed-920b-47caed15465f";
		public const string TEXTURE_PLYWOOD = "89556747-24cb-43ed-920b-47caed15465f";
		public const string TEXTURE_TRANSPARENT = "8dcd4a48-2d37-4909-9f78-f7a9eb4ef903";
		public const string TEXTURE_MEDIA = "8b5fec65-8d8d-9dc5-cda8-8fdf2716e361";

		public const int TOUCH_INVALID_FACE = -1;

		public const int TRAVERSAL_TYPE = 7;
		public const int TRAVERSAL_TYPE_FAST = 1;
		public const int TRAVERSAL_TYPE_NONE = 2;
		public const int TRAVERSAL_TYPE_SLOW = 0;

		public const int TYPE_INTEGER = 1;
		public const int TYPE_FLOAT = 2;
		public const int TYPE_STRING = 3;
		public const int TYPE_KEY = 4;
		public const int TYPE_VECTOR = 5;
		public const int TYPE_ROTATION = 6;
		public const int TYPE_INVALID = 0;

		public const string URL_REQUEST_GRANTED = "URL_REQUEST_GRANTED";
		public const string URL_REQUEST_DENIED = "URL_REQUEST_DENIED";

		public const int VEHICLE_TYPE_NONE = 0;

		public const int VEHICLE_LINEAR_FRICTION_TIMESCALE = 16;
		public const int VEHICLE_ANGULAR_FRICTION_TIMESCALE = 17;
		public const int VEHICLE_LINEAR_MOTOR_DIRECTION = 18;
		public const int VEHICLE_ANGULAR_MOTOR_DIRECTION = 19;
		public const int VEHICLE_LINEAR_MOTOR_OFFSET = 20;
		public const int VEHICLE_HOVER_HEIGHT = 24;
		public const int VEHICLE_HOVER_EFFICIENCY = 25;
		public const int VEHICLE_HOVER_TIMESCALE = 26;
		public const int VEHICLE_BUOYANCY = 27;
		public const int VEHICLE_LINEAR_DEFLECTION_EFFICIENCY = 28;
		public const int VEHICLE_LINEAR_DEFLECTION_TIMESCALE = 29;
		public const int VEHICLE_LINEAR_MOTOR_TIMESCALE = 30;
		public const int VEHICLE_LINEAR_MOTOR_DECAY_TIMESCALE = 31;
		public const int VEHICLE_ANGULAR_DEFLECTION_EFFICIENCY = 32;
		public const int VEHICLE_ANGULAR_DEFLECTION_TIMESCALE = 33;
		public const int VEHICLE_ANGULAR_MOTOR_TIMESCALE = 34;
		public const int VEHICLE_ANGULAR_MOTOR_DECAY_TIMESCALE = 35;
		public const int VEHICLE_VERTICAL_ATTRACTION_EFFICIENCY = 36;
		public const int VEHICLE_VERTICAL_ATTRACTION_TIMESCALE = 37;
		public const int VEHICLE_BANKING_EFFICIENCY = 38;
		public const int VEHICLE_BANKING_MIX = 39;
		public const int VEHICLE_BANKING_TIMESCALE = 40;
		public const int VEHICLE_REFERENCE_FRAME = 44;

		public const int VEHICLE_FLAG_NO_FLY_UP = 1;

		public const int VEHICLE_FLAG_NO_DEFLECTION_UP = 1;
		public const int VEHICLE_FLAG_LIMIT_ROLL_ONLY = 2;
		public const int VEHICLE_FLAG_HOVER_WATER_ONLY = 4;
		public const int VEHICLE_FLAG_HOVER_TERRAIN_ONLY = 8;
		public const int VEHICLE_FLAG_HOVER_GLOBAL_HEIGHT = 16;
		public const int VEHICLE_FLAG_HOVER_UP_ONLY = 32;
		public const int VEHICLE_FLAG_LIMIT_MOTOR_UP = 64;
		public const int VEHICLE_FLAG_MOUSELOOK_STEER = 128;
		public const int VEHICLE_FLAG_MOUSELOOK_BANK = 256;
		public const int VEHICLE_FLAG_CAMERA_DECOUPLED = 512;

		public const int VEHICLE_TYPE_SLED = 1;
		public const int VEHICLE_TYPE_CAR = 2;
		public const int VEHICLE_TYPE_BOAT = 3;
		public const int VEHICLE_TYPE_AIRPLANE = 4;
		public const int VEHICLE_TYPE_BALLOON = 5;

		public const int VERTICAL = 0;

		public static readonly Float PI = 3.1415926535897932384626;
		public static readonly Float TWO_PI = 2.0 * PI;
		public static readonly Float PI_BY_TWO = 1.570796;
		public static readonly Float DEG_TO_RAD = 0.017453;
		public static readonly Float RAD_TO_DEG = 57.295780;
		public static readonly Float SQRT2 = 1.414214;

		public static readonly key NULL_KEY = key.NULL_KEY;
		public static readonly vector TOUCH_INVALID_TEXCOORD = new vector(-1.0, -1.0, 0.0);
		public static readonly vector TOUCH_INVALID_VECTOR = new vector(0.0, 0.0, 0.0);
		public static readonly rotation ZERO_ROTATION = rotation.ZERO_ROTATION;
		public static readonly vector ZERO_VECTOR = vector.ZERO_VECTOR;
	}
}
