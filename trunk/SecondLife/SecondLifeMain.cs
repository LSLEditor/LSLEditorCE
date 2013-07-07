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

        #region all constants
        public static readonly integer TRUE = 1;
        public static readonly integer FALSE = 0;

        public static readonly integer DENSITY = 1;
        public static readonly integer FRICTION = 2;
        public static readonly integer GRAVITY_MULTIPLIER = 8;
        public static readonly integer RESTITUTION = 4;

        public static readonly integer AGENT = 1;
        public static readonly integer ACTIVE = 2;
        public static readonly integer PASSIVE = 4;
        public static readonly integer SCRIPTED = 8;

        public static readonly integer ANIM_ON = 1;
        public static readonly integer LOOP = 2;
        public static readonly integer REVERSE = 4;
        public static readonly integer PING_PONG = 8;
        public static readonly integer SMOOTH = 16;
        public static readonly integer ROTATE = 32;
        public static readonly integer SCALE = 64;

        public static readonly Float PI = 3.1415926535897932384626;
        public static readonly Float TWO_PI = 2.0 * PI;
        public static readonly Float PI_BY_TWO = 1.570796;
        public static readonly Float DEG_TO_RAD = 0.017453;
        public static readonly Float RAD_TO_DEG = 57.295780;
        public static readonly Float SQRT2 = 1.414214;

        public static readonly key NULL_KEY = key.NULL_KEY;
        public static readonly rotation ZERO_ROTATION = rotation.ZERO_ROTATION;
        public static readonly vector ZERO_VECTOR = vector.ZERO_VECTOR;

        public static readonly integer ALL_SIDES = -1;

        public static readonly integer DEBUG_CHANNEL = 2147483647;
        public const string EOF = "\n\n\n";

        public static readonly integer AGENT_FLYING = 1;
        public static readonly integer AGENT_ATTACHMENTS = 2;
        public static readonly integer AGENT_SCRIPTED = 4;
        public static readonly integer AGENT_MOUSELOOK = 8;
        public static readonly integer AGENT_SITTING = 16;
        public static readonly integer AGENT_ON_OBJECT = 32;
        public static readonly integer AGENT_AWAY = 64;
        public static readonly integer AGENT_WALKING = 128;
        public static readonly integer AGENT_IN_AIR = 256;
        public static readonly integer AGENT_TYPING = 512;
        public static readonly integer AGENT_CROUCHING = 1024;
        public static readonly integer AGENT_BUSY = 2048;
        public static readonly integer AGENT_ALWAYS_RUN = 4096;
		public static readonly integer AGENT_AUTOPILOT = 0x2000;
		public static readonly integer AGENT_BY_LEGACY_NAME = 0x1;
		public static readonly integer AGENT_BY_USERNAME = 0x10;

		public static readonly integer AGENT_LIST_PARCEL = 0x01;
		public static readonly integer AGENT_LIST_PARCEL_OWNER = 0x02;
		public static readonly integer AGENT_LIST_REGION = 0x04;

        public static readonly integer ATTACH_AVATAR_CENTER = 40;
        public static readonly integer ATTACH_BACK = 9;
        public static readonly integer ATTACH_CHEST = 1;
        public static readonly integer ATTACH_CHIN = 12;
        public static readonly integer ATTACH_HEAD = 2;
        public static readonly integer ATTACH_LSHOULDER = 3;
        public static readonly integer ATTACH_RSHOULDER = 4;
        public static readonly integer ATTACH_LHAND = 5;
        public static readonly integer ATTACH_RHAND = 6;
        public static readonly integer ATTACH_LFOOT = 7;
        public static readonly integer ATTACH_RFOOT = 8;
        public static readonly integer ATTACH_PELVIS = 10;
        public static readonly integer ATTACH_MOUTH = 11;
        public static readonly integer ATTACH_NECK = 39;
        public static readonly integer ATTACH_LEAR = 13;
        public static readonly integer ATTACH_LEFT_PEC = 29;
        public static readonly integer ATTACH_REAR = 14;
        public static readonly integer ATTACH_LEYE = 15;
        public static readonly integer ATTACH_REYE = 16;
        public static readonly integer ATTACH_NOSE = 17;
        public static readonly integer ATTACH_RUARM = 18;
        public static readonly integer ATTACH_RLARM = 19;
        public static readonly integer ATTACH_LUARM = 20;
        public static readonly integer ATTACH_LLARM = 21;
        public static readonly integer ATTACH_RHIP = 22;
        public static readonly integer ATTACH_RIGHT_PEC = 30;
        public static readonly integer ATTACH_RULEG = 23;
        public static readonly integer ATTACH_RLLEG = 24;
        public static readonly integer ATTACH_LHIP = 25;
        public static readonly integer ATTACH_LULEG = 26;
        public static readonly integer ATTACH_LLLEG = 27;
        public static readonly integer ATTACH_BELLY = 28;
        public static readonly integer ATTACH_RPEC = 29;
        public static readonly integer ATTACH_LPEC = 30;

        public static readonly integer ATTACH_HUD_CENTER_2 = 31;
        public static readonly integer ATTACH_HUD_TOP_RIGHT = 32;
        public static readonly integer ATTACH_HUD_TOP_CENTER = 33;
        public static readonly integer ATTACH_HUD_TOP_LEFT = 34;
        public static readonly integer ATTACH_HUD_CENTER_1 = 35;
        public static readonly integer ATTACH_HUD_BOTTOM_LEFT = 36;
        public static readonly integer ATTACH_HUD_BOTTOM = 37;
        public static readonly integer ATTACH_HUD_BOTTOM_RIGHT = 38;

		public static readonly integer AVOID_CHARACTERS = 1;
		public static readonly integer AVOID_DYNAMIC_OBSTACLES = 2;

        public static readonly integer CAMERA_PITCH = 0;
        public static readonly integer CAMERA_FOCUS_OFFSET = 1;
        public static readonly integer CAMERA_POSITION_LAG = 5;
        public static readonly integer CAMERA_FOCUS_LAG = 6;
        public static readonly integer CAMERA_DISTANCE = 7;
        public static readonly integer CAMERA_BEHINDNESS_ANGLE = 8;
        public static readonly integer CAMERA_BEHINDNESS_LAG = 9;
        public static readonly integer CAMERA_POSITION_THRESHOLD = 10;
        public static readonly integer CAMERA_FOCUS_THRESHOLD = 11;
        public static readonly integer CAMERA_ACTIVE = 12;
        public static readonly integer CAMERA_POSITION = 13;
        public static readonly integer CAMERA_FOCUS = 17;
        public static readonly integer CAMERA_FOCUS_LOCKED = 22;
        public static readonly integer CAMERA_POSITION_LOCKED = 21;

        public static readonly integer CHANGED_INVENTORY = 1;
        public static readonly integer CHANGED_COLOR = 2;
        public static readonly integer CHANGED_SHAPE = 4;
        public static readonly integer CHANGED_SCALE = 8;
        public static readonly integer CHANGED_TEXTURE = 16;
        public static readonly integer CHANGED_LINK = 32;
        public static readonly integer CHANGED_ALLOWED_DROP = 64;
        public static readonly integer CHANGED_OWNER = 128;
        public static readonly integer CHANGED_REGION = 256;
        public static readonly integer CHANGED_TELEPORT = 512;
        public static readonly integer CHANGED_REGION_START = 1024;
		public static readonly integer CHANGED_MEDIA = 2048;

        public static readonly integer CHARACTER_ACCOUNT_FOR_SKIPPED_FRAMES = 14;
		public static readonly integer CHARACTER_AVOIDANCE_MODE = 5;
		public static readonly integer CHARACTER_CMD_JUMP = 0x01;
		public static readonly integer CHARACTER_CMD_STOP = 0x00;
		public static readonly integer CHARACTER_DESIRED_SPEED = 1;
		public static readonly integer CHARACTER_LENGTH = 3;
		public static readonly integer CHARACTER_TYPE = 6;
		public static readonly integer CHARACTER_MAX_ACCEL = 8;
		public static readonly integer CHARACTER_MAX_DECEL = 9;
        public static readonly integer CHARACTER_MAX_SPEED = 13;
        public static readonly integer CHARACTER_MAX_TURN_RADIUS = 10;
        public static readonly integer CHARACTER_ORIENTATION = 4;
        public static readonly integer CHARACTER_RADIUS = 2;
        public static readonly integer CHARACTER_STAY_WITHIN_PARCEL = 15;
		public static readonly integer CHARACTER_TYPE_A = 0;
		public static readonly integer CHARACTER_TYPE_B = 1;
		public static readonly integer CHARACTER_TYPE_C = 2;
		public static readonly integer CHARACTER_TYPE_D = 3;
		public static readonly integer CHARACTER_TYPE_NONE = 4;

        public static readonly integer CLICK_ACTION_NONE = 0;
        public static readonly integer CLICK_ACTION_TOUCH = 0;
        public static readonly integer CLICK_ACTION_SIT = 1;
        public static readonly integer CLICK_ACTION_BUY = 2;
        public static readonly integer CLICK_ACTION_PAY = 3;
        public static readonly integer CLICK_ACTION_OPEN = 4;
        public static readonly integer CLICK_ACTION_PLAY = 5;
        public static readonly integer CLICK_ACTION_OPEN_MEDIA = 6;

		public static readonly string CONTENT_TYPE_ATOM = "application/atom+xml";
		public static readonly string CONTENT_TYPE_FORM = "application/x-www-form-urlencoded";
        public static readonly string CONTENT_TYPE_HTML = "text/html";
		public static readonly string CONTENT_TYPE_JSON = "application/json";
		public static readonly string CONTENT_TYPE_LLSD = "application/llsd+xml";
		public static readonly string CONTENT_TYPE_RSS = "application/rss+xml";
		public static readonly string CONTENT_TYPE_TEXT = "text/plain";
		public static readonly string CONTENT_TYPE_XHTML = "application/xhtml+xml";
		public static readonly string CONTENT_TYPE_XML = "application/xml";

        public static readonly integer CONTROL_FWD = 1;
        public static readonly integer CONTROL_BACK = 2;
        public static readonly integer CONTROL_LEFT = 4;
        public static readonly integer CONTROL_RIGHT = 8;
        public static readonly integer CONTROL_ROT_LEFT = 256;
        public static readonly integer CONTROL_ROT_RIGHT = 512;
        public static readonly integer CONTROL_UP = 16;
        public static readonly integer CONTROL_DOWN = 32;
        public static readonly integer CONTROL_LBUTTON = 268435456;
        public static readonly integer CONTROL_ML_LBUTTON = 1073741824;

        public static readonly integer DATA_ONLINE = 1;
        public static readonly integer DATA_NAME = 2;
        public static readonly integer DATA_BORN = 3;
        public static readonly integer DATA_RATING = 4;

        public static readonly integer DATA_SIM_POS = 5;
        public static readonly integer DATA_SIM_STATUS = 6;
        public static readonly integer DATA_SIM_RATING = 7;

        public static readonly integer DATA_PAYINFO = 8;

        public static readonly integer ERR_GENERIC = -1;
        public static readonly integer ERR_MALFORMED_PARAMS = -3;
        public static readonly integer ERR_PARCEL_PERMISSIONS = -2;
        public static readonly integer ERR_RUNTIME_PERMISSIONS = -4;
        public static readonly integer ERR_THROTTLED = -5;

        public static readonly integer ESTATE_ACCESS_ALLOWED_AGENT_ADD = 4;
        public static readonly integer ESTATE_ACCESS_ALLOWED_AGENT_REMOVE = 8;
        public static readonly integer ESTATE_ACCESS_ALLOWED_GROUP_ADD = 16;
        public static readonly integer ESTATE_ACCESS_ALLOWED_GROUP_REMOVE = 32;
        public static readonly integer ESTATE_ACCESS_BANNED_AGENT_ADD = 64;
        public static readonly integer ESTATE_ACCESS_BANNED_AGENT_REMOVE = 128;

		public static readonly integer FORCE_DIRECT_PATH = 1;

        public static readonly integer HTTP_BODY_MAXLENGTH = 2;
        public static readonly integer HTTP_BODY_TRUNCATED = 0;
        public static readonly integer HTTP_METHOD = 0;
        public static readonly integer HTTP_MIMETYPE = 1;
        public static readonly integer HTTP_PRAGMA_NO_CACHE = 6;
        public static readonly integer HTTP_VERBOSE_THROTTLE = 4;
        public static readonly integer HTTP_VERIFY_CERT = 3;

        public static readonly integer HORIZONTAL = 1;

        public static readonly integer INVENTORY_ALL = -1;
        public static readonly integer INVENTORY_NONE = -1;
        public static readonly integer INVENTORY_TEXTURE = 0;
        public static readonly integer INVENTORY_SOUND = 1;
        public static readonly integer INVENTORY_LANDMARK = 3;
        public static readonly integer INVENTORY_CLOTHING = 5;
        public static readonly integer INVENTORY_OBJECT = 6;
        public static readonly integer INVENTORY_NOTECARD = 7;
        public static readonly integer INVENTORY_SCRIPT = 10;
        public static readonly integer INVENTORY_BODYPART = 13;
        public static readonly integer INVENTORY_ANIMATION = 20;
        public static readonly integer INVENTORY_GESTURE = 21;

        public static readonly string JSON_ARRAY    = "\uFDD2";
        public static readonly string JSON_FALSE    = "\uFDD7";
        public static readonly string JSON_INVALID  = "\uFDD0";
        public static readonly string JSON_NULL     = "\uFDD5";
        public static readonly string JSON_NUMBER   = "\uFDD3";
        public static readonly string JSON_OBJECT   = "\uFDD1";
        public static readonly string JSON_STRING   = "\uFDD4";
        public static readonly string JSON_TRUE     = "\uFDD6";

        public static readonly integer KFM_CMD_PAUSE = 2;
        public static readonly integer KFM_CMD_PLAY = 0;
        public static readonly integer KFM_CMD_STOP = 1;
        public static readonly integer KFM_COMMAND = 0;
        public static readonly integer KFM_DATA = 2;
        public static readonly integer KFM_FORWARD = 0;
        public static readonly integer KFM_LOOP = 1;
        public static readonly integer KFM_MODE = 1;
        public static readonly integer KFM_PING_PONG = 2;
        public static readonly integer KFM_REVERSE = 2;
        public static readonly integer KFM_ROTATION = 1;
        public static readonly integer KFM_TRANSLATION = 2;

        public static readonly integer LAND_LEVEL = 0;
        public static readonly integer LAND_RAISE = 1;
        public static readonly integer LAND_LOWER = 2;
        public static readonly integer LAND_SMOOTH = 3;
        public static readonly integer LAND_NOISE = 4;
        public static readonly integer LAND_REVERT = 5;

        public static readonly integer LAND_LARGE_BRUSH = 3;
        public static readonly integer LAND_MEDIUM_BRUSH = 2;
        public static readonly integer LAND_SMALL_BRUSH = 1;

        public static readonly integer LINK_ROOT = 1;
        public static readonly integer LINK_SET = -1;
        public static readonly integer LINK_ALL_OTHERS = -2;
        public static readonly integer LINK_ALL_CHILDREN = -3;
        public static readonly integer LINK_THIS = -4;

        public static readonly integer LIST_STAT_RANGE = 0;
        public static readonly integer LIST_STAT_MIN = 1;
        public static readonly integer LIST_STAT_MAX = 2;
        public static readonly integer LIST_STAT_MEAN = 3;
        public static readonly integer LIST_STAT_MEDIAN = 4;
        public static readonly integer LIST_STAT_STD_DEV = 5;
        public static readonly integer LIST_STAT_SUM = 6;
        public static readonly integer LIST_STAT_SUM_SQUARES = 7;
        public static readonly integer LIST_STAT_NUM_COUNT = 8;
        public static readonly integer LIST_STAT_GEOMETRIC_MEAN = 9;

        public static readonly integer MASK_BASE = 0;
        public static readonly integer MASK_EVERYONE = 3;
        public static readonly integer MASK_GROUP = 2;
        public static readonly integer MASK_NEXT = 4;
        public static readonly integer MASK_OWNER = 1;

        public static readonly integer OBJECT_ATTACHED_POINT = 19;
        public static readonly integer OBJECT_NAME = 1;
        public static readonly integer OBJECT_DESC = 2;
        public static readonly integer OBJECT_POS = 3;
        public static readonly integer OBJECT_ROOT = 18;
        public static readonly integer OBJECT_ROT = 4;
        public static readonly integer OBJECT_VELOCITY = 5;
        public static readonly integer OBJECT_OWNER = 6;
        public static readonly integer OBJECT_PATHFINDING_TYPE = 20;
        public static readonly integer OBJECT_GROUP = 7;
        public static readonly integer OBJECT_CREATOR = 8;
        public static readonly integer OBJECT_PHYSICS = 21;
        public static readonly integer OBJECT_PHANTOM = 22;
        public static readonly integer OBJECT_TEMP_ON_REZ = 23;

        public static readonly integer OBJECT_STREAMING_COST = 15;
        public static readonly integer OBJECT_PHYSICS_COST = 16;
        public static readonly integer OBJECT_SERVER_COST = 14;
        public static readonly integer OBJECT_PRIM_EQUIVALENCE = 13;

		public static readonly integer OBJECT_RUNNING_SCRIPT_COUNT = 9;
		public static readonly integer OBJECT_SCRIPT_MEMORY = 11;
		public static readonly integer OBJECT_SCRIPT_TIME = 12;
		public static readonly integer OBJECT_TOTAL_SCRIPT_COUNT = 10;
		public static readonly integer OBJECT_UNKNOWN_DETAIL = -1;

        public static readonly integer OPT_AVATAR = 1;
        public static readonly integer OPT_CHARACTER = 2;
        public static readonly integer OPT_EXCLUSION_VOLUME = 6;
        public static readonly integer OPT_LEGACY_LINKSET = 0;
        public static readonly integer OPT_MATERIAL_VOLUME = 5;
        public static readonly integer OPT_OTHER = -1;
        public static readonly integer OPT_STATIC_OBSTACLE = 4;
        public static readonly integer OPT_WALKABLE = 3;

        public static readonly integer OBJECT_RETURN_PARCEL = 1;
        public static readonly integer OBJECT_RETURN_PARCEL_OWNER = 2;
        public static readonly integer OBJECT_RETURN_REGION = 4;

        public static readonly integer PARCEL_COUNT_TOTAL = 0;
        public static readonly integer PARCEL_COUNT_OWNER = 1;
        public static readonly integer PARCEL_COUNT_GROUP = 2;
        public static readonly integer PARCEL_COUNT_OTHER = 3;
        public static readonly integer PARCEL_COUNT_SELECTED = 4;
        public static readonly integer PARCEL_COUNT_TEMP = 5;

        public static readonly integer PARCEL_DETAILS_AREA = 4;
        public static readonly integer PARCEL_DETAILS_DESC = 1;
        public static readonly integer PARCEL_DETAILS_GROUP = 3;
        public static readonly integer PARCEL_DETAILS_ID = 5;
        public static readonly integer PARCEL_DETAILS_NAME = 0;
        public static readonly integer PARCEL_DETAILS_OWNER = 2;
        public static readonly integer PARCEL_DETAILS_SEE_AVATARS = 6;

        public static readonly integer PARCEL_FLAG_ALLOW_FLY = 0x0000001;
        public static readonly integer PARCEL_FLAG_ALLOW_SCRIPTS = 0x0000002;
        public static readonly integer PARCEL_FLAG_ALLOW_LANDMARK = 0x0000008;
        public static readonly integer PARCEL_FLAG_ALLOW_TERRAFORM = 0x0000010;
        public static readonly integer PARCEL_FLAG_ALLOW_DAMAGE = 0x0000020;
        public static readonly integer PARCEL_FLAG_ALLOW_CREATE_OBJECTS = 0x0000040;
        public static readonly integer PARCEL_FLAG_USE_ACCESS_GROUP = 0x0000100;
        public static readonly integer PARCEL_FLAG_USE_ACCESS_LIST = 0x0000200;
        public static readonly integer PARCEL_FLAG_USE_BAN_LIST = 0x0000400;
        public static readonly integer PARCEL_FLAG_USE_LAND_PASS_LIST = 0x0000800;
        public static readonly integer PARCEL_FLAG_LOCAL_SOUND_ONLY = 0x0008000;
        public static readonly integer PARCEL_FLAG_RESTRICT_PUSHOBJECT = 0x0200000;
        public static readonly integer PARCEL_FLAG_ALLOW_GROUP_SCRIPTS = 0x2000000;

        public static readonly integer PARCEL_FLAG_ALLOW_CREATE_GROUP_OBJECTS = 0x4000000;
        public static readonly integer PARCEL_FLAG_ALLOW_ALL_OBJECT_ENTRY = 0x8000000;
        public static readonly integer PARCEL_FLAG_ALLOW_GROUP_OBJECT_ENTRY = 0x10000000;

        public static readonly integer PARCEL_MEDIA_COMMAND_STOP = 0;
        public static readonly integer PARCEL_MEDIA_COMMAND_PAUSE = 1;
        public static readonly integer PARCEL_MEDIA_COMMAND_PLAY = 2;
        public static readonly integer PARCEL_MEDIA_COMMAND_LOOP = 3;
        public static readonly integer PARCEL_MEDIA_COMMAND_TEXTURE = 4;
        public static readonly integer PARCEL_MEDIA_COMMAND_URL = 5;
        public static readonly integer PARCEL_MEDIA_COMMAND_TIME = 6;
        public static readonly integer PARCEL_MEDIA_COMMAND_AGENT = 7;
        public static readonly integer PARCEL_MEDIA_COMMAND_UNLOAD = 8;
        public static readonly integer PARCEL_MEDIA_COMMAND_AUTO_ALIGN = 9;

        public static readonly integer PARCEL_MEDIA_COMMAND_TYPE = 10;
        public static readonly integer PARCEL_MEDIA_COMMAND_SIZE = 11;
        public static readonly integer PARCEL_MEDIA_COMMAND_DESC = 12;
        public static readonly integer PARCEL_MEDIA_COMMAND_LOOP_SET = 13;

        public static readonly integer PAY_DEFAULT = -2;
        public static readonly integer PAY_HIDE = -1;

        public static readonly integer PAYMENT_INFO_ON_FILE = 1;
        public static readonly integer PAYMENT_INFO_USED = 2;

        public static readonly integer PERM_ALL = 2147483647;
        public static readonly integer PERM_COPY = 32768;
        public static readonly integer PERM_MODIFY = 16384;
        public static readonly integer PERM_MOVE = 524288;
        public static readonly integer PERM_TRANSFER = 8192;

        public static readonly integer PERMISSION_DEBIT = 2;
        public static readonly integer PERMISSION_TAKE_CONTROLS = 4;
        public static readonly integer PERMISSION_REMAP_CONTROLS = 8;
        public static readonly integer PERMISSION_TRIGGER_ANIMATION = 16;
        public static readonly integer PERMISSION_ATTACH = 32;
        public static readonly integer PERMISSION_RELEASE_OWNERSHIP = 64;
        public static readonly integer PERMISSION_CHANGE_LINKS = 128;
        public static readonly integer PERMISSION_CHANGE_JOINTS = 256;
        public static readonly integer PERMISSION_CHANGE_PERMISSIONS = 512;
        public static readonly integer PERMISSION_CONTROL_CAMERA = 2048;
        public static readonly integer PERMISSION_OVERRIDE_ANIMATIONS = 0x8000;
        public static readonly integer PERMISSION_TRACK_CAMERA = 1024;
        public static readonly integer PERMISSION_RETURN_OBJECTS = 65536;

        public static readonly integer PRIM_BUMP_BARK = 4;
        public static readonly integer PRIM_BUMP_BLOBS = 12;
        public static readonly integer PRIM_BUMP_BRICKS = 5;
        public static readonly integer PRIM_BUMP_BRIGHT = 1;
        public static readonly integer PRIM_BUMP_CHECKER = 6;
        public static readonly integer PRIM_BUMP_CONCRETE = 7;
        public static readonly integer PRIM_BUMP_DARK = 2;
        public static readonly integer PRIM_BUMP_DISKS = 10;
        public static readonly integer PRIM_BUMP_GRAVEL = 11;
        public static readonly integer PRIM_BUMP_LARGETILE = 14;
        public static readonly integer PRIM_BUMP_NONE = 0;
        public static readonly integer PRIM_BUMP_SHINY = 19;
        public static readonly integer PRIM_BUMP_SIDING = 13;
        public static readonly integer PRIM_BUMP_STONE = 9;
        public static readonly integer PRIM_BUMP_STUCCO = 15;
        public static readonly integer PRIM_BUMP_SUCTION = 16;
        public static readonly integer PRIM_BUMP_TILE = 8;
        public static readonly integer PRIM_BUMP_WEAVE = 17;
        public static readonly integer PRIM_BUMP_WOOD = 3;

        public static readonly integer PRIM_CAST_SHADOWS = 24;
        public static readonly integer PRIM_COLOR = 18;
        public static readonly integer PRIM_DESC = 28;
        public static readonly integer PRIM_FLEXIBLE = 21;
        public static readonly integer PRIM_FULLBRIGHT = 20;
        public static readonly integer PRIM_HOLE_CIRCLE = 16;
        public static readonly integer PRIM_HOLE_DEFAULT = 0;
        public static readonly integer PRIM_HOLE_SQUARE = 32;
        public static readonly integer PRIM_HOLE_TRIANGLE = 48;
		public static readonly integer PRIM_LINK_TARGET = 34;
        public static readonly integer PRIM_MATERIAL = 2;
        public static readonly integer PRIM_MATERIAL_FLESH = 4;
        public static readonly integer PRIM_MATERIAL_GLASS = 2;
        public static readonly integer PRIM_MATERIAL_LIGHT = 7;
        public static readonly integer PRIM_MATERIAL_METAL = 1;
        public static readonly integer PRIM_MATERIAL_PLASTIC = 5;
        public static readonly integer PRIM_MATERIAL_RUBBER = 6;
        public static readonly integer PRIM_MATERIAL_STONE = 0;
        public static readonly integer PRIM_MATERIAL_WOOD = 3;

        public static readonly integer PRIM_MEDIA_ALT_IMAGE_ENABLE = 0;
        public static readonly integer PRIM_MEDIA_AUTO_LOOP = 4;
        public static readonly integer PRIM_MEDIA_AUTO_PLAY = 5;
        public static readonly integer PRIM_MEDIA_AUTO_SCALE = 6;
        public static readonly integer PRIM_MEDIA_AUTO_ZOOM = 7;
        public static readonly integer PRIM_MEDIA_CONTROLS = 1;
        public static readonly integer PRIM_MEDIA_CONTROLS_MINI = 1;
        public static readonly integer PRIM_MEDIA_CONTROLS_STANDARD = 0;
        public static readonly integer PRIM_MEDIA_CURRENT_URL = 2;
        public static readonly integer PRIM_MEDIA_FIRST_CLICK_INTERACT = 8;
        public static readonly integer PRIM_MEDIA_HEIGHT_PIXELS = 10;
        public static readonly integer PRIM_MEDIA_HOME_URL = 3;
        public static readonly integer PRIM_MEDIA_PERM_ANYONE = 4;
        public static readonly integer PRIM_MEDIA_PERM_GROUP = 2;
        public static readonly integer PRIM_MEDIA_PERM_NONE = 0;
        public static readonly integer PRIM_MEDIA_PERM_OWNER = 1;
        public static readonly integer PRIM_MEDIA_PERMS_CONTROL = 14;
        public static readonly integer PRIM_MEDIA_PERMS_INTERACT = 13;
        public static readonly integer PRIM_MEDIA_WHITELIST = 12;
        public static readonly integer PRIM_MEDIA_WHITELIST_ENABLE = 11;
        public static readonly integer PRIM_MEDIA_WIDTH_PIXELS = 9;

        public static readonly integer PRIM_NAME = 27;
        public static readonly integer PRIM_OMEGA = 32;
        public static readonly integer PRIM_PHANTOM = 5;
        public static readonly integer PRIM_PHYSICS = 3;
        public static readonly integer PRIM_PHYSICS_MATERIAL = 31;
        public static readonly integer PRIM_PHYSICS_SHAPE_CONVEX = 2;
        public static readonly integer PRIM_PHYSICS_SHAPE_NONE = 1;
        public static readonly integer PRIM_PHYSICS_SHAPE_PRIM = 0;
        public static readonly integer PRIM_PHYSICS_SHAPE_TYPE = 30;
        public static readonly integer PRIM_POINT_LIGHT = 23;
        public static readonly integer PRIM_POSITION = 6;
        public static readonly integer PRIM_POS_LOCAL = 33;
        public static readonly integer PRIM_ROTATION = 8;
        public static readonly integer PRIM_ROT_LOCAL = 29;
        public static readonly integer PRIM_SCULPT_FLAG_INVERT = 64;
        public static readonly integer PRIM_SCULPT_FLAG_MIRROR = 128;
        public static readonly integer PRIM_SHINY_HIGH = 3;
        public static readonly integer PRIM_SHINY_LOW = 1;
        public static readonly integer PRIM_SHINY_MEDIUM = 2;
        public static readonly integer PRIM_SHINY_NONE = 0;
        public static readonly integer PRIM_SIZE = 7;
		public static readonly integer PRIM_SLICE = 35;
        public static readonly integer PRIM_TEMP_ON_REZ = 4;
        public static readonly integer PRIM_TEXGEN = 22;
        public static readonly integer PRIM_TEXGEN_DEFAULT = 0;
        public static readonly integer PRIM_TEXGEN_PLANAR = 1;
        public static readonly integer PRIM_TEXT = 26;
        public static readonly integer PRIM_TEXTURE = 17;
        public static readonly integer PRIM_TYPE = 9;

        public static readonly integer PRIM_TYPE_BOX = 0;
        public static readonly integer PRIM_TYPE_CYLINDER = 1;
        public static readonly integer PRIM_TYPE_PRISM = 2;
        public static readonly integer PRIM_TYPE_SPHERE = 3;
        public static readonly integer PRIM_TYPE_TORUS = 4;
        public static readonly integer PRIM_TYPE_TUBE = 5;
        public static readonly integer PRIM_TYPE_RING = 6;
        public static readonly integer PRIM_TYPE_SCULPT = 7;

        public static readonly integer PRIM_GLOW = 25;

		public static readonly integer PRIM_SCULPT_TYPE_MASK = 7;
        public static readonly integer PRIM_SCULPT_TYPE_SPHERE = 1;
        public static readonly integer PRIM_SCULPT_TYPE_TORUS = 2;
        public static readonly integer PRIM_SCULPT_TYPE_PLANE = 3;
        public static readonly integer PRIM_SCULPT_TYPE_CYLINDER = 4;

        public static readonly integer PROFILE_NONE = 0;
        public static readonly integer PROFILE_SCRIPT_MEMORY = 1;

        public static readonly integer PSYS_PART_FLAGS = 0;
        public static readonly integer PSYS_PART_INTERP_COLOR_MASK = 1;
        public static readonly integer PSYS_PART_INTERP_SCALE_MASK = 2;
        public static readonly integer PSYS_PART_BOUNCE_MASK = 4;
        public static readonly integer PSYS_PART_WIND_MASK = 8;
        public static readonly integer PSYS_PART_FOLLOW_SRC_MASK = 16;
        public static readonly integer PSYS_PART_FOLLOW_VELOCITY_MASK = 32;
        public static readonly integer PSYS_PART_TARGET_POS_MASK = 64;
        public static readonly integer PSYS_PART_TARGET_LINEAR_MASK = 128;
        public static readonly integer PSYS_PART_EMISSIVE_MASK = 256;

        public static readonly integer PSYS_PART_SRC_PATTERN_ANGLE = 0x04;
        public static readonly integer PSYS_PART_SRC_PATTERN_ANGLE_CONE = 0x08;
        public static readonly integer PSYS_PART_SRC_PATTERN_DROP = 0x01;
        public static readonly integer PSYS_PART_SRC_PATTERN_EXPLODE = 0x02;

        public static readonly integer PSYS_PART_START_COLOR = 1;
        public static readonly integer PSYS_PART_START_ALPHA = 2;
        public static readonly integer PSYS_PART_END_COLOR = 3;
        public static readonly integer PSYS_PART_END_ALPHA = 4;
        public static readonly integer PSYS_PART_START_SCALE = 5;
        public static readonly integer PSYS_PART_END_SCALE = 6;
        public static readonly integer PSYS_PART_MAX_AGE = 7;

        public static readonly integer PSYS_SRC_ACCEL = 8;
        public static readonly integer PSYS_SRC_PATTERN = 9;
        public static readonly integer PSYS_SRC_INNERANGLE = 10;
        public static readonly integer PSYS_SRC_OUTERANGLE = 11;
        public static readonly integer PSYS_SRC_TEXTURE = 12;
        public static readonly integer PSYS_SRC_BURST_RATE = 13;
        public static readonly integer PSYS_SRC_BURST_PART_COUNT = 15;
        public static readonly integer PSYS_SRC_BURST_RADIUS = 16;
        public static readonly integer PSYS_SRC_BURST_SPEED_MIN = 17;
        public static readonly integer PSYS_SRC_BURST_SPEED_MAX = 18;
        public static readonly integer PSYS_SRC_MAX_AGE = 19;
        public static readonly integer PSYS_SRC_TARGET_KEY = 20;
        public static readonly integer PSYS_SRC_OMEGA = 21;
        public static readonly integer PSYS_SRC_ANGLE_BEGIN = 22;
        public static readonly integer PSYS_SRC_ANGLE_END = 23;

        public static readonly integer PSYS_SRC_PATTERN_DROP = 1;
        public static readonly integer PSYS_SRC_PATTERN_EXPLODE = 2;
        public static readonly integer PSYS_SRC_PATTERN_ANGLE = 4;
        public static readonly integer PSYS_SRC_PATTERN_ANGLE_CONE = 8;
        public static readonly integer PSYS_SRC_PATTERN_ANGLE_CONE_EMPTY = 16;

		public static readonly integer PU_EVADE_HIDDEN = 0x07;
		public static readonly integer PU_EVADE_SPOTTED = 0x08;
		public static readonly integer PU_FAILURE_INVALID_GOAL = 0x03;
		public static readonly integer PU_FAILURE_INVALID_START = 0x02;
		public static readonly integer PU_FAILURE_NO_VALID_DESTINATION = 0x06;
		public static readonly integer PU_FAILURE_OTHER = 1000000;
		public static readonly integer PU_FAILURE_TARGET_GONE = 0x05;
		public static readonly integer PU_FAILURE_UNREACHABLE = 0x04;
		public static readonly integer PU_GOAL_REACHED = 0x01;
		public static readonly integer PU_SLOWDOWN_DISTANCE_REACHED = 0x00;

        public static readonly integer PUBLIC_CHANNEL = 0;

		public static readonly integer PURSUIT_FUZZ_FACTOR = 3;
		public static readonly integer PURSUIT_INTERCEPT = 4;
		public static readonly integer PURSUIT_OFFSET = 1;

		public static readonly integer RC_DATA_FLAGS = 2;
		public static readonly integer RC_DETECT_PHANTOM = 1;
		public static readonly integer RC_GET_LINK_NUM = 4;
		public static readonly integer RC_GET_NORMAL = 1;
		public static readonly integer RC_GET_ROOT_KEY = 2;
		public static readonly integer RC_MAX_HITS = 3;
		public static readonly integer RC_REJECT_AGENTS = 1;
		public static readonly integer RC_REJECT_LAND = 8;
		public static readonly integer RC_REJECT_NONPHYSICAL = 4;
		public static readonly integer RC_REJECT_PHYSICAL = 2;
		public static readonly integer RC_REJECT_TYPES = 2;
		public static readonly integer RCERR_CAST_TIME_EXCEEDED = -3;
		public static readonly integer RCERR_SIM_PERF_LOW = -2;
		public static readonly integer RCERR_UNKNOWN = -1;


        public static readonly integer REGION_FLAG_ALLOW_DAMAGE = 1;
        public static readonly integer REGION_FLAG_FIXED_SUN = 16;
        public static readonly integer REGION_FLAG_BLOCK_TERRAFORM = 64;
        public static readonly integer REGION_FLAG_SANDBOX = 256;
        public static readonly integer REGION_FLAG_DISABLE_COLLISIONS = 4096;
        public static readonly integer REGION_FLAG_DISABLE_PHYSICS = 16384;
        public static readonly integer REGION_FLAG_BLOCK_FLY = 524288;
        public static readonly integer REGION_FLAG_ALLOW_DIRECT_TELEPORT = 1048576;
        public static readonly integer REGION_FLAG_RESTRICT_PUSHOBJECT = 4194304;

        public static readonly integer REMOTE_DATA_CHANNEL = 1;
        public static readonly integer REMOTE_DATA_REQUEST = 2;
        public static readonly integer REMOTE_DATA_REPLY = 3;

		public static readonly integer REQUIRE_LINE_OF_SIGHT = 2;

        public static readonly integer SIM_STAT_PCT_CHARS_STEPPED = 0;

        public static readonly integer STATUS_PHYSICS = 1;
        public static readonly integer STATUS_ROTATE_X = 2;
        public static readonly integer STATUS_ROTATE_Y = 4;
        public static readonly integer STATUS_ROTATE_Z = 8;
        public static readonly integer STATUS_PHANTOM = 16;
        public static readonly integer STATUS_SANDBOX = 32;
        public static readonly integer STATUS_BLOCK_GRAB = 64;
        public static readonly integer STATUS_BLOCK_GRAB_OBJECT = 1024;
        public static readonly integer STATUS_DIE_AT_EDGE = 128;
        public static readonly integer STATUS_RETURN_AT_EDGE = 256;
        public static readonly integer STATUS_CAST_SHADOWS = 512;
        public static readonly integer STATUS_BOUNDS_ERROR = 1002;
        public static readonly integer STATUS_INTERNAL_ERROR = 1999;
        public static readonly integer STATUS_MALFORMED_PARAMS = 1000;
        public static readonly integer STATUS_NOT_FOUND = 1003;
        public static readonly integer STATUS_NOT_SUPPORTED = 1004;
        public static readonly integer STATUS_OK = 0;
        public static readonly integer STATUS_TYPE_MISMATCH = 1001;
        public static readonly integer STATUS_WHITELIST_FAILED = 2001;

        public static readonly integer STRING_TRIM_HEAD = 1;
        public static readonly integer STRING_TRIM_TAIL = 2;
        public static readonly integer STRING_TRIM = 3;

        public static readonly key TEXTURE_BLANK = "5748decc-f629-461c-9a36-a35a221fe21f";
        public static readonly key TEXTURE_DEFAULT = "89556747-24cb-43ed-920b-47caed15465f";
        public static readonly key TEXTURE_PLYWOOD = "89556747-24cb-43ed-920b-47caed15465f";
        public static readonly key TEXTURE_TRANSPARENT = "8dcd4a48-2d37-4909-9f78-f7a9eb4ef903";
        public static readonly key TEXTURE_MEDIA = "8b5fec65-8d8d-9dc5-cda8-8fdf2716e361";

		public static readonly integer TOUCH_INVALID_FACE = 0xFFFFFFFF;
        public static readonly vector TOUCH_INVALID_TEXCOORD = new vector(-1.0, -1.0, 0.0);
		public static readonly vector TOUCH_INVALID_VECTOR = new vector(0.0, 0.0, 0.0);

		public static readonly integer TRAVERSAL_TYPE = 7;
		public static readonly integer TRAVERSAL_TYPE_FAST = 1;
		public static readonly integer TRAVERSAL_TYPE_NONE = 2;
		public static readonly integer TRAVERSAL_TYPE_SLOW = 0;

        public static readonly integer TYPE_INTEGER = 1;
        public static readonly integer TYPE_FLOAT = 2;
        public static readonly integer TYPE_STRING = 3;
        public static readonly integer TYPE_KEY = 4;
        public static readonly integer TYPE_VECTOR = 5;
        public static readonly integer TYPE_ROTATION = 6;
        public static readonly integer TYPE_INVALID = 0;

        public static readonly String URL_REQUEST_GRANTED = "URL_REQUEST_GRANTED";
        public static readonly String URL_REQUEST_DENIED = "URL_REQUEST_DENIED";

        public static readonly integer VEHICLE_TYPE_NONE = 0;

        public static readonly integer VEHICLE_LINEAR_FRICTION_TIMESCALE = 16;
        public static readonly integer VEHICLE_ANGULAR_FRICTION_TIMESCALE = 17;
        public static readonly integer VEHICLE_LINEAR_MOTOR_DIRECTION = 18;
        public static readonly integer VEHICLE_ANGULAR_MOTOR_DIRECTION = 19;
        public static readonly integer VEHICLE_LINEAR_MOTOR_OFFSET = 20;
        public static readonly integer VEHICLE_HOVER_HEIGHT = 24;
        public static readonly integer VEHICLE_HOVER_EFFICIENCY = 25;
        public static readonly integer VEHICLE_HOVER_TIMESCALE = 26;
        public static readonly integer VEHICLE_BUOYANCY = 27;
        public static readonly integer VEHICLE_LINEAR_DEFLECTION_EFFICIENCY = 28;
        public static readonly integer VEHICLE_LINEAR_DEFLECTION_TIMESCALE = 29;
        public static readonly integer VEHICLE_LINEAR_MOTOR_TIMESCALE = 30;
        public static readonly integer VEHICLE_LINEAR_MOTOR_DECAY_TIMESCALE = 31;
        public static readonly integer VEHICLE_ANGULAR_DEFLECTION_EFFICIENCY = 32;
        public static readonly integer VEHICLE_ANGULAR_DEFLECTION_TIMESCALE = 33;
        public static readonly integer VEHICLE_ANGULAR_MOTOR_TIMESCALE = 34;
        public static readonly integer VEHICLE_ANGULAR_MOTOR_DECAY_TIMESCALE = 35;
        public static readonly integer VEHICLE_VERTICAL_ATTRACTION_EFFICIENCY = 36;
        public static readonly integer VEHICLE_VERTICAL_ATTRACTION_TIMESCALE = 37;
        public static readonly integer VEHICLE_BANKING_EFFICIENCY = 38;
        public static readonly integer VEHICLE_BANKING_MIX = 39;
        public static readonly integer VEHICLE_BANKING_TIMESCALE = 40;
        public static readonly integer VEHICLE_REFERENCE_FRAME = 44;

        // depricaded
        public static readonly integer VEHICLE_FLAG_NO_FLY_UP = 1;

        public static readonly integer VEHICLE_FLAG_NO_DEFLECTION_UP = 1;
        public static readonly integer VEHICLE_FLAG_LIMIT_ROLL_ONLY = 2;
        public static readonly integer VEHICLE_FLAG_HOVER_WATER_ONLY = 4;
        public static readonly integer VEHICLE_FLAG_HOVER_TERRAIN_ONLY = 8;
        public static readonly integer VEHICLE_FLAG_HOVER_GLOBAL_HEIGHT = 16;
        public static readonly integer VEHICLE_FLAG_HOVER_UP_ONLY = 32;
        public static readonly integer VEHICLE_FLAG_LIMIT_MOTOR_UP = 64;
        public static readonly integer VEHICLE_FLAG_MOUSELOOK_STEER = 128;
        public static readonly integer VEHICLE_FLAG_MOUSELOOK_BANK = 256;
        public static readonly integer VEHICLE_FLAG_CAMERA_DECOUPLED = 512;

        public static readonly integer VEHICLE_TYPE_SLED = 1;
        public static readonly integer VEHICLE_TYPE_CAR = 2;
        public static readonly integer VEHICLE_TYPE_BOAT = 3;
        public static readonly integer VEHICLE_TYPE_AIRPLANE = 4;
        public static readonly integer VEHICLE_TYPE_BALLOON = 5;

        public static readonly integer VERTICAL = 0;

        //public static readonly integer REGION_FLAG_RESTRICT_PUSHOBJECT=4194304;

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

        #region llFunctions

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
            if (m_LandBanList.ContainsKey(kID))
                m_LandBanList.Remove(kID);
            m_LandBanList.Add(kID, fHours);
        }

        public void llAddToLandPassList(key kID, Float fHours)
        {
            Verbose("llAddToLandPassList({0}, {1})", kID, fHours);
            if (m_LandPassList.ContainsKey(kID))
                m_LandPassList.Remove(kID);
            m_LandPassList.Add(kID, fHours);
        }

        public void llAdjustSoundVolume(Float fVolume)
        {
            Verbose("llAdjustSoundVolume({0}), fVolume");
            m_Volume = fVolume;
        }

        public void llAllowInventoryDrop(integer iAllowDrop)
        {
            Verbose("llAllowInventoryDrop({0})", iAllowDrop);
            m_AllowDrop = (Boolean)iAllowDrop;
        }

        public Float llAngleBetween(rotation a, rotation b)
        {
            rotation r = b / a;                                                 // calculate the rotation between the two arguments as quaternion
            double s2 = r.s * r.s;                                              // square of the s-element
            double v2 = r.x * r.x + r.y * r.y + r.z * r.z;                      // sum of the squares of the v-elements

            if (s2 < v2)                                                        // compare the s-component to the v-component
            {
                return 2.0 * Math.Acos(Math.Sqrt(s2 / (s2 + v2)));              // use arccos if the v-component is dominant
            }
            else if (v2 != 0)                                                   // make sure the v-component is non-zero
            {
                return 2.0 * Math.Asin(Math.Sqrt(v2 / (s2 + v2)));              // use arcsin if the s-component is dominant
            }
            return 0.0; // one or both arguments are scaled too small to be meaningful, or the values are the same, so return zero
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

		public key llAvatarOnLinkSitTarget(integer iLinkNumber)
		{
            key kLinkUUID = new key(Guid.NewGuid());
            Verbose("llAvatarOnLinkSitTarget({0}))={1}", iLinkNumber, kLinkUUID);
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

            Verbose("llAxisAngle2Rot({0},{1})={2}", vAxis, fAngle, rResult);
            return rResult;
        }

        public integer llBase64ToInteger(String sString)
        {
            int iResult;

            try
            {
                string s = sString;
                byte[] data = new byte[4];

                if (s.Length > 1)
                {
                    data[3] = (byte)(LookupBase64(s, 0) << 2);
                    data[3] |= (byte)(LookupBase64(s, 1) >> 4);
                }

                if (s.Length > 2)
                {
                    data[2] = (byte)((LookupBase64(s, 1) & 0xf) << 4);
                    data[2] |= (byte)(LookupBase64(s, 2) >> 2);
                }

                if (s.Length > 3)
                {
                    data[1] = (byte)((LookupBase64(s, 2) & 0x7) << 6);
                    data[1] |= (byte)(LookupBase64(s, 3));
                }

                if (s.Length > 5)
                {
                    data[0] = (byte)(LookupBase64(s, 4) << 2);
                    data[0] |= (byte)(LookupBase64(s, 5) >> 4);
                }

                iResult = BitConverter.ToInt32(data, 0);

                //0 12 34 56
                //1 78 12 34
                //2 56 78 12
                //3 34 56 78

                //4 12 34 56
                //5 78 12 34
                //6 56 78 12
                //7 34 56 78
            }
            catch
            {
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

        public void llBreakLink(integer iLinkNumber)
        {
            host.llBreakLink(iLinkNumber);
            Verbose("llBreakLink({0})", iLinkNumber);
        }

        public list llCSV2List(String sString)
        {
            string sSource = sString;
            list lResult = new list();
            StringBuilder sb = new StringBuilder();
            int WithinAngelBracket = 0;
            for (int intI = 0; intI < sSource.Length; intI++)
            {
                char chrC = sSource[intI];
                if (chrC == '<')
                    WithinAngelBracket++;
                else if (chrC == '>')
                    WithinAngelBracket--;

                if (WithinAngelBracket == 0 && chrC == ',')
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
            for (int intI = 0; intI < lResult.Count; intI++)
            {
                string strValue = lResult[intI].ToString();
                if (strValue == "")
                    continue;
                if (strValue[0] == ' ')
                    lResult[intI] = strValue.Substring(1);
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
            Verbose(@"llCollisionSound(""{0}"", {1})", sImpactSound, + fImpactVolume);
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

            if (CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (int intI = start; intI <= end; intI++)
                        src[intI] = null;
                }
                else // excluding
                {
                    for (int intI = 0; intI <= end; intI++)
                        src[intI] = null;
                    for (int intI = start; intI < intLength; intI++)
                        src[intI] = null;
                }
            }
            list lResult = new list();
            for (int intI = 0; intI < src.Count; intI++)
            {
                if (src[intI] != null)
                    lResult.Add(src[intI]);
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

            if (CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (int intI = start; intI <= end; intI++)
                        src[intI] = '\0';
                }
                else // excluding
                {
                    for (int intI = 0; intI <= end; intI++)
                        src[intI] = '\0';
                    for (int intI = start; intI < intLength; intI++)
                        src[intI] = '\0';
                }
            }
            StringBuilder result = new StringBuilder();
            for (int intI = 0; intI < src.Length; intI++)
            {
                if (src[intI] != '\0')
                    result.Append(src[intI]);
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

        public integer llDetectedLinkNumber(integer iLinkNumber)
        {
            integer iDetected = 0;
            Verbose("llDetectedLinkNumber({0})={1}", iLinkNumber, iDetected);
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
            for (int intI = 0; intI < lSource.Count; intI++)
            {
                if (intI > 0)
                    result.Append(sSeparator.ToString());
                result.Append(lSource[intI].ToString());
            }
            Verbose(@"DumpList2String({0},""{1}"")=""{2}""", lSource.ToVerboseString(), sSeparator, result.ToString());
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
            Verbose("@llEmail(""{0}"", ""{1}"", ""{2}"")", sAddress, sSubject, sMessage);
            host.Email(sAddress, sSubject, sMessage);
        }

        public String llEscapeURL(String sURL)
        {
            StringBuilder sb = new StringBuilder();
            byte[] data = Encoding.UTF8.GetBytes(sURL.ToString());
            for (int intI = 0; intI < data.Length; intI++)
            {
                byte chrC = data[intI];
                if ((chrC >= 'a' && chrC <= 'z') ||
                    (chrC >= 'A' && chrC <= 'Z') ||
                    (chrC >= '0' && chrC <= '9'))
                    sb.Append((char)chrC);
                else
                    sb.AppendFormat("%{0:X2}", (int)chrC);
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
            rotation rRotation = new rotation(aw * by * cz + ax * bw * cw,
                    aw * by * cw - ax * bw * cz,
                    aw * bw * cz + ax * by * cw,
                    aw * bw * cw - ax * by * cz);
            Verbose("llEuler2Rot({0})={1}", v, rRotation);
            return rRotation;
        }

		public void llEvade(key TargetID, list Options)
		{
			Verbose("llEvade({0}, {1})", TargetID, Options);
		}

		public void llExecCharacterCmd(integer Command, list Options)
		{
			Verbose("llExecCharacterCmd({0}, {1})", Command, Options);
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
            double dblValue = fMaximum * m_random.NextDouble();
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
            double dTime = llGetTime();
            Verbose("llGetAndResetTime()=" + dTime);
            // reset time
            llResetTime();
            return dTime;
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
            Verbose("llGetAnimationOverride(""{0}"")=""{1}""", sAnimationState, sAnimation);
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

        public vector llGetColor(integer face)
        {
            Verbose("llGetColor(" + face + ")");
            return vector.ZERO_VECTOR;
        }

        public key llGetCreator()
        {
            key result = Properties.Settings.Default.AvatarKey;
            Verbose("llGetCreator()={0}", result);
            return result;
        }

        public String llGetDate()
        {
            string result = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd");
            Verbose("llGetDate()=" + result);
            return result;
        }

        public string llGetDisplayName(key kAvatarID)
        {
            string result = "";
            Verbose("llGetDisplayName({0})={1}", kAvatarID, result);
            return result;
        }

        public Float llGetEnergy()
        {
            Float result = 1.23;
            Verbose("llGetEnergy()={0}", result);
            return result;
        }

        public string llGetEnv(string sDataRequest)
        {
            string result;
            switch (sDataRequest)
            {
                case "sim_channel":
                    result = "Second Life Server";
                    break;
                case "sim_version":
                    result = "11.11.09.244706";
                    break;
                default:
                    result = "";
                    break;
            }
            Verbose("llGetEnv({0})={1}", sDataRequest, result);
            return result;
        }

        public vector llGetForce()
        {
            Verbose("llGetForce()");
            return vector.ZERO_VECTOR;
        }

        public integer llGetFreeMemory()
        {
            Verbose("llGetFreeMemory()");
            return 16000;
        }

        public integer llGetFreeURLs()
        {
            return 0;
        }

        public Float llGetGMTclock()
        {
            Float result = DateTime.Now.ToUniversalTime().TimeOfDay.TotalSeconds;
            Verbose("llGetGMTclock()={0}", result);
            return result;
        }

        public vector llGetGeometricCenter()
        {
            vector result = ZERO_VECTOR;
            Verbose("llGetGeometricCenter()={0}", result);
            return result;
        }

        public String llGetHTTPHeader(key request_id, String header)
        {
            return "not-implemented";
        }

        public key llGetInventoryCreator(String item)
        {
            key result = Properties.Settings.Default.AvatarKey;
            Verbose(@"GetInventoryCreator(""{0}"")={1}", item, result);
            return result;
        }

        public key llGetInventoryKey(String name)
        {
            key result = host.GetInventoryKey(name);
            Verbose("llGetInventoryKey({0})={1}", name, result);
            return result;
        }

        public String llGetInventoryName(integer type, integer number)
        {
            string result = host.GetInventoryName(type, number);
            Verbose("llGetInventoryName({0},{1})={2}", type, number, result);
            return result;
        }

        public integer llGetInventoryNumber(integer type)
        {
            int result = host.GetInventoryNumber(type);
            Verbose("llGetInventoryNumber({0})={1}", type, result);
            return result;
        }

        public integer llGetInventoryPermMask(String item, integer mask)
        {
            Verbose("llGetInventoryPermMask(" + item + "," + mask + ")");
            return 0;
        }

        public integer llGetInventoryType(String name)
        {
            integer result = host.GetInventoryType(name);
            Verbose("llGetInventoryType({0})={1}", name, result);
            return result;
        }

        public key llGetKey()
        {
            key result = host.GetKey();
            Verbose("llGetKey()=" + result.ToString());
            return result;
        }

        public key llGetLandOwnerAt(vector pos)
        {
            Verbose("llGetLandOwnerAt(" + pos + ")");
            return new key(Guid.NewGuid());
        }

        public key llGetLinkKey(integer linknum)
        {
            Verbose("llGetLinkKey(" + linknum + ")");
            return new key(Guid.NewGuid());
        }

        public list llGetLinkMedia(integer iLinkNumber, integer iFace, list lParameters)
        {
            list result = new list();
            Verbose("llGetLinkMedia({0}, {1}, {2})={3}", iLinkNumber, iFace, lParameters.ToString(), result);
            return result;
        }

        public String llGetLinkName(integer linknum)
        {
            Verbose("llGetLinkName(" + linknum + ")");
            return "";
        }

        public integer llGetLinkNumber()
        {
            Verbose("llGetLinkNumber()");
            return 0;
        }

        public integer llGetLinkNumberOfSides(integer link)
        {
            integer result = 6;
            Verbose("llGetLinkNumberOfSides()={" + link + "}", result);
            return result;
        }

        public list llGetLinkPrimitiveParams(integer link, list myparams)
        {
            Verbose("llGetLinkPrimitiveParams(" + link + "," + myparams.ToString() + ")");
            return new list();
        }

        public integer llGetListEntryType(list src, integer index)
        {
            integer intReturn;

            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                intReturn = 0;
            else
            {
                switch (src[index].GetType().ToString().Replace("LSLEditor.SecondLife+", ""))
                {
                    case "System.Double":
                    case "Float":
                        intReturn = TYPE_FLOAT;
                        break;
                    case "System.String":
                    case "String":
                        intReturn = TYPE_STRING;
                        break;
                    case "System.Int32":
                    case "integer":
                        intReturn = TYPE_INTEGER;
                        break;
                    case "key":
                        intReturn = TYPE_KEY;
                        break;
                    case "vector":
                        intReturn = TYPE_VECTOR;
                        break;
                    case "rotation":
                        intReturn = TYPE_ROTATION;
                        break;
                    default:
                        intReturn = TYPE_INVALID;
                        break;
                }
            }
            Verbose("llGetListEntryType({0},{1})={2}", src.ToVerboseString(), index, intReturn);
            return intReturn;
        }

        public integer llGetListLength(list src)
        {
            integer result = src.Count;
            Verbose("llGetListLength({0})={1}", src.ToVerboseString(), result);
            return result;
        }

        public vector GetLocalPos()
        {
            // no verbose
            return m_pos;
        }

        public vector llGetLocalPos()
        {
            Verbose("llGetLocalPos()={0}", m_pos);
            return m_pos;
        }

        public rotation llGetLocalRot()
        {
            Verbose("llGetLocalRot()={0}", m_rotlocal);
            return m_rotlocal;
        }

        public Float llGetMass()
        {
            Float result = 1.23;
            Verbose("llGetMass()={0}", result);
            return result;
        }

        public Float llGetMassMKS()
        {
            Float result = 1.23;
            Verbose("llGetMassMKS()={0}", result);
            return result;
        }

        public integer llGetMemoryLimit()
        {
            return 65536;
        }

        public void llGetNextEmail(String address, String subject)
        {
            Verbose("llGetNextEmail(" + address + "," + subject + ")");
        }

        public key llGetNotecardLine(String name, integer line)
        {
            key k = host.GetNotecardLine(name, line);
            Verbose(@"GetNotecardLine(""{0}"",{1})={2}", name, line, k);
            return k;
        }

        public key llGetNumberOfNotecardLines(String name)
        {
            key k = host.GetNumberOfNotecardLines(name);
            Verbose(@"GetNumberOfNotecardLines(""{0}"")={1}", name, k);
            return k;
        }

        public integer llGetNumberOfPrims()
        {
            integer result = 10;
            Verbose("llGetNumberOfPrims()={0}", result);
            return result;
        }

        public integer llGetNumberOfSides()
        {
            integer result = 6;
            Verbose("llGetNumberOfSides()={0}", result);
            return result;
        }

        public String llGetObjectDesc()
        {
            string result = host.GetObjectDescription();
            Verbose("llGetObjectDesc()={0}", result);
            return result;
        }

        public list llGetObjectDetails(key id, list _params)
        {
            list result = new list();
            for (int intI = 0; intI < _params.Count; intI++)
            {
                if (!(_params[intI] is integer))
                    continue;
                switch ((int)(integer)_params[intI])
                {
                    case 1: //OBJECT_NAME:
                        result.Add((SecondLife.String)host.GetObjectName(new Guid(id.guid)));
                        break;
                    case 2: //OBJECT_DESC:
                        result.Add((SecondLife.String)host.GetObjectDescription(new Guid(id.guid)));
                        break;
                    case 3://OBJECT_POS:
                        break;
                    case 4://OBJECT_ROT:
                        break;
                    case 5://OBJECT_VELOCITY:
                        break;
                    case 6://OBJECT_OWNER:
                        break;
                    case 7://OBJECT_GROUP:
                        break;
                    case 8://OBJECT_CREATOR:
                        break;
                    default:
                        break;
                }
            }
            Verbose(@"llGetObjectDetails({0},{1})={2}", id, _params, result);
            return result;
        }

        public Float llGetObjectMass(key id)
        {
            Verbose("llGetObjectMass(" + id + ")");
            return 0.0F;
        }

        public String llGetObjectName()
        {
            string result = host.GetObjectName();
            Verbose("llGetObjectName()={0}", result);
            return result;
        }

        public integer llGetObjectPermMask(integer mask)
        {
            Verbose("llGetObjectPermMask(" + mask + ")");
            return 0;
        }

        // added 4 mei 2007
        public integer llGetObjectPrimCount(key id)
        {
            Verbose("llGetObjectPrimCount(" + id + ")");
            return 0;
        }

        public vector llGetOmega()
        {
            Verbose("llGetOmega()");
            return vector.ZERO_VECTOR;
        }

        public key llGetOwner()
        {
            key k = new key(Properties.Settings.Default.AvatarKey);
            Verbose("llGetOwner()=" + k);
            return k;
        }

        public key llGetOwnerKey(key id)
        {
            key k = llGetOwner();
            Verbose("llGetOwnerKey(" + id + ")=" + k);
            return k;
        }

        /*
        PARCEL_DETAILS_NAME  0  The name of the parcel.  63 Characters  string
        PARCEL_DETAILS_DESC  1  The description of the parcel.  127 Characters  string
        PARCEL_DETAILS_OWNER  2  The parcel owner's key.  (36 Characters)  key
        PARCEL_DETAILS_GROUP  3  The parcel group's key.  (36 Characters)  key
        PARCEL_DETAILS_AREA  4  The parcel's area, in sqm.  (5 Characters)  integer
        */
        public list llGetParcelDetails(vector pos, list details)
        {
            list result = new list();
            for (int intI = 0; intI < details.Count; intI++)
            {
                if (details[intI] is integer)
                {
                    switch ((int)(integer)details[intI])
                    {
                        case 0: // PARCEL_DETAILS_NAME:
                            result.Add(Properties.Settings.Default.ParcelName);
                            break;
                        case 1: // PARCEL_DETAILS_DESC:
                            result.Add(Properties.Settings.Default.ParcelDescription);
                            break;
                        case 2: //PARCEL_DETAILS_OWNER:
                            result.Add(new key(Properties.Settings.Default.ParcelOwner));
                            break;
                        case 3: //PARCEL_DETAILS_GROUP:
                            result.Add(new key(Properties.Settings.Default.ParcelGroup));
                            break;
                        case 4: // PARCEL_DETAILS_AREA:
                            result.Add(new integer(Properties.Settings.Default.ParcelArea));
                            break;
                        default:
                            break;
                    }
                }
            }
            Verbose("llGetParcelDetails({0},{1})={2}", pos, details.ToVerboseString(), result.ToVerboseString());
            return result;
        }

        public integer llGetParcelFlags(vector pos)
        {
            Verbose("llGetParcelFlags(" + pos + ")");
            return 0;
        }

        public integer llGetParcelMaxPrims(vector pos, integer sim_wide)
        {
            Verbose("llGetParcelMaxPrims(" + pos + "," + sim_wide + ")");
            return 0;
        }

		public string llGetParcelMusicURL()
		{
			Verbose("llGetParcelMaxPrims()={1}", m_ParcelMusicURL);
			return m_ParcelMusicURL;
		}

        public integer llGetParcelPrimCount(vector pos, integer category, integer sim_wide)
        {
            Verbose("llGetParcelPrimCount(" + pos + "," + category + "," + sim_wide + ")");
            return 0;
        }

        public list llGetParcelPrimOwners(vector pos)
        {
            list result = new list(new object[] { Properties.Settings.Default.AvatarKey, 10 });
            Verbose("llGetParcelPrimOwners({0})={1}", pos, result);
            return result;
        }

        public integer llGetPermissions()
        {
            integer perm =
                PERMISSION_DEBIT
            | PERMISSION_TAKE_CONTROLS
            | PERMISSION_TRIGGER_ANIMATION
            | PERMISSION_ATTACH
            | PERMISSION_CHANGE_LINKS
            | PERMISSION_TRACK_CAMERA
            | PERMISSION_CONTROL_CAMERA;

            Verbose("llGetPermissions()=" + perm);
            return perm;
        }

        public key llGetPermissionsKey()
        {
            Verbose("llGetPermissionsKey()");
            return new key(Guid.NewGuid());
        }

        public list llGetPhysicsMaterial()
        {
            Verbose("llGetPhysicalMaterial()");
            return new list();
        }

        public vector llGetPos()
        {
            Verbose("llGetPos()=" + m_pos);
            return m_pos;
        }

        public list llGetPrimMediaParams(integer face, list myparams)
        {
            Verbose("llGetPrimMediaParams(" + face + "," + myparams.ToString() + ")");
            return new list();
        }

        public list llGetPrimitiveParams(list myparams)
        {
            Verbose("llGetPrimitiveParams(" + myparams.ToString() + ")");
            return new list();
        }

        // 334
        public integer llGetRegionAgentCount()
        {
            Verbose("llGetRegionAgentCount()");
            return 0;
        }

        public vector llGetRegionCorner()
        {
            System.Drawing.Point point = Properties.Settings.Default.RegionCorner;
            vector RegionCorner = new vector(point.X, point.Y, 0);
            Verbose("llGetRegionCorner()" + RegionCorner);
            return RegionCorner;
        }

        public Float llGetRegionFPS()
        {
            Verbose("llGetRegionFPS()=" + Properties.Settings.Default.RegionFPS);
            return Properties.Settings.Default.RegionFPS;
        }

        public integer llGetRegionFlags()
        {
            Verbose("llGetRegionFlags()");
            return 0;
        }

        public String llGetRegionName()
        {
            Verbose("llGetRegionName()=" + Properties.Settings.Default.RegionName);
            return Properties.Settings.Default.RegionName;
        }

        public Float llGetRegionTimeDilation()
        {
            Verbose("llGetRegionTimeDilation()");
            return 0.9;
        }

        public vector llGetRootPosition()
        {
            Verbose("llGetRootPosition()");
            return vector.ZERO_VECTOR;
        }

        public rotation llGetRootRotation()
        {
            Verbose("llGetRootRotation()");
            return rotation.ZERO_ROTATION;
        }

        public rotation llGetRot()
        {
            Verbose("llGetRot()=" + m_rot);
            return m_rot;
        }

        public integer llGetSPMaxMemory()
        {
            return 65536;
        }

        public vector llGetScale()
        {
            Verbose("llGetScale()=" + m_scale);
            return m_scale;
        }

        public String llGetScriptName()
        {
            string result = host.GetScriptName();
            Verbose("llGetScriptName()=" + result);
            return result;
        }

        public integer llGetScriptState(String name)
        {
            Verbose("llGetScriptState(" + name + ")");
            return 0;
        }

        public float llGetSimStats(integer StatType)
        {
            Verbose("llGetSimStats(" + StatType + ")");
            return 0;
        }

        public String llGetSimulatorHostname()
        {
            Verbose("llGetSimulatorHostname()");
            return "";
        }

        public integer llGetStartParameter()
        {
            Verbose("llGetStartParameter()=" + m_start_parameter);
            return m_start_parameter;
        }

        public integer llGetStatus(integer status)
        {
            Verbose("llGetStatus(" + status + ")");
            return 0;
        }

        public String llGetSubString(String _src, integer _start, integer _end)
        {
            string src = _src;
            int start = _start;
            int end = _end;

            StringBuilder result = new StringBuilder();

            int intLength = src.Length;

            if (CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (int intI = start; intI <= end; intI++)
                        result.Append(src[intI]);
                }
                else // excluding
                {
                    for (int intI = 0; intI <= end; intI++)
                        result.Append(src[intI]);
                    for (int intI = start; intI < intLength; intI++)
                        result.Append(src[intI]);
                }
            }
            Verbose(string.Format(@"GetSubString(""{0}"",{1},{2})=""{3}""", _src, _start, _end, result));
            return result.ToString();
        }

        public vector llGetSunDirection()
        {
            Verbose("llGetSunDirection()");
            return vector.ZERO_VECTOR;
        }

        public String llGetTexture(integer face)
        {
            Verbose("llGetTexture(" + face + ")");
            return "";
        }

        public vector llGetTextureOffset(integer side)
        {
            Verbose("llGetTextureOffset(" + side + ")");
            return vector.ZERO_VECTOR;
        }

        public Float llGetTextureRot(integer side)
        {
            Verbose("llGetTextureRot(" + side + ")");
            return 0.0;
        }

        public vector llGetTextureScale(integer side)
        {
            Verbose("llGetTextureScale(" + side + ")");
            return vector.ZERO_VECTOR;
        }

        public Float llGetTime()
        {
            TimeSpan span = DateTime.Now.ToUniversalTime() - m_DateTimeScriptStarted;
            Verbose("llGetTime()=" + span.TotalSeconds);
            return span.TotalSeconds;
        }

        public Float llGetTimeOfDay()
        {
            Verbose("llGetTimeOfDay()");
            // dummy
            return llGetTime();
        }

        public string llGetTimestamp()
        {
            string strTimestamp = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ss.ffffffZ", System.Globalization.DateTimeFormatInfo.InvariantInfo);
            Verbose("llGetTimestamp()=" + strTimestamp);
            return strTimestamp;
        }

        public vector llGetTorque()
        {
            Verbose("llGetTorque()");
            return vector.ZERO_VECTOR;
        }

        public integer llGetUnixTime()
        {
            DateTime date_time_base = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            TimeSpan span = DateTime.Now.ToUniversalTime() - date_time_base;
            Verbose("llGetUnixTime()=" + span.TotalSeconds);
            return (int)span.TotalSeconds;
        }

        public integer llGetUsedMemory()
        {
            return 65536;
        }

        public string llGetUsername(key kAvatarID)
        {
            //TODO Find a dummy username.
            string result = "";
            Verbose("llGetUsername({0})={1}", kAvatarID, result);
            return result;
        }

        public vector llGetVel()
        {
            Verbose("llGetVel()");
            return vector.ZERO_VECTOR;
        }

        public Float llGetWallclock()
        {
            Float result = (int)DateTime.Now.AddHours(-9.0).TimeOfDay.TotalSeconds;
            Verbose("llGetWallclock()={0}", result);
            return result;
        }

        public void llGiveInventory(key destination, String inventory)
        {
            Verbose("llGiveInventory(" + destination + "," + inventory + ")");
        }

        public void llGiveInventoryList(key destination, String category, list inventory)
        {
            Verbose("llGiveInventoryList(" + destination + "," + category + "," + inventory.ToString() + ")");
        }

        public integer llGiveMoney(key destination, integer amount)
        {
            Verbose("llGiveMoney(" + destination + "," + amount + ")");
            return 0;
        }

        public Float llGround(vector offset)
        {
            Float ground = 25.0;
            Verbose("llGround(" + offset + ")=" + ground);
            return ground;
        }

        public vector llGroundContour(vector offset)
        {
            Verbose("llGroundContour(" + offset + ")");
            return vector.ZERO_VECTOR;
        }

        public vector llGroundNormal(vector offset)
        {
            vector GroundNormal = new vector(0, 0, 1);
            Verbose("llGroundNormal(" + offset + ")=" + GroundNormal);
            return GroundNormal;
        }

        public void llGroundRepel(Float height, integer water, Float tau)
        {
            Verbose("llGroundRepel(" + height + "," + water + "," + tau + ")");
        }

        public vector llGroundSlope(vector offset)
        {
            Verbose("llGroundSlope:" + offset);
            return vector.ZERO_VECTOR;
        }

        public key llHTTPRequest(String url, list parameters, String body)
        {
            key result = host.Http(url, parameters, body);
            Verbose(@"HTTPRequest(""{0}"",{1},""{2}"")=""{3}""", url, parameters.ToVerboseString(), body, result);
            return result;
        }

        //348
        public void llHTTPResponse(key request_id, integer status, String body)
        {
        }

        public String llInsertString(String _dst, integer _position, String _src)
        {
            string dst = _dst;
            string src = _src;
            int position = _position;
            string result;

            if (position < 0)
                position = 0;

            if (position < dst.Length)
                result = dst.Substring(0, position) + src + dst.Substring(position);
            else
                result = dst + src;
            Verbose(@"InsertString(""{0}"",{1},""{2}"")=""{3}""", dst, position, src, result);
            return result;
        }

        public void llInstantMessage(key user, String message)
        {
            Verbose("llInstantMessage(" + user + "," + message + ")");
        }

        public String llIntegerToBase64(integer number)
        {
            byte[] data = new byte[4];
            data[3] = (byte)(number & 0xff);
            data[2] = (byte)((number >> 8) & 0xff);
            data[1] = (byte)((number >> 16) & 0xff);
            data[0] = (byte)((number >> 24) & 0xff);
            string result = Convert.ToBase64String(data);
            Verbose(@"IntegerToBase64({0})=""{1}""", number, result);
            return result;
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

        public list llJson2List(string sJSON)
        {
            //TODO implement conversion to list
            list lJSON = new list();
            Verbose("llJson2List({0})={1}", sJSON, lJSON);
            return lJSON;
        }

        public string llJsonGetValue(string sJSON, list lSpecifiers)
        {
            //TODO determine return value from list
            string sReturn = JSON_INVALID;
            Verbose("llJsonGetValue({0}, {1})= {2}", sJSON, lSpecifiers, sReturn);
            return sReturn;
        }

        public string llJsonSetValue(string sJSON, list lSpecifiers, string sValue)
        {
            //TODO determine return value
            string sReturn = JSON_INVALID;
            Verbose("llJsonGetValue({0}, {1}, {2})= {3}", sJSON, lSpecifiers, sValue, sReturn);
            return sReturn;
        }

        public string llJsonValueType(string sJSON, list lSpecifiers)
        {
            //TODO determine return value
            string sReturn = JSON_INVALID;
            Verbose("llJsonGetValue({0}, {1})= {2}", sJSON, lSpecifiers, sReturn);
            return sReturn;
        }

        public String llKey2Name(key id)
        {
            string strName = "*unknown*";
            if (Properties.Settings.Default.AvatarKey == id)
                strName = Properties.Settings.Default.AvatarName;
            Verbose("llKey2Name(" + id + ")=" + strName);
            return strName;
        }

        public void llLinkParticleSystem(integer link, list parameters)
        {
            Verbose("llLinkParticleSystem(" + link + "," + parameters.ToString() + ")");
        }

        public void llLinkSitTarget(integer iLinkNumber, vector vOffset, rotation rRotation)
        {
            Verbose("llLinkSitTarget({0}, {1}, {2})", iLinkNumber, vOffset, rRotation);
        }

        public String llList2CSV(list src)
        {
            StringBuilder result = new StringBuilder();
            for (int intI = 0; intI < src.Count; intI++)
            {
                if (intI > 0)
                    result.Append(", ");
                result.Append(src[intI].ToString());
            }
            Verbose(@"List2CSV({0})=""{1}""", src.ToVerboseString(), result.ToString());
            return result.ToString();
        }

        public Float llList2Float(list src, integer index)
        {
            Float result;
            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                result = 0.0;
            else
                result = (Float)src[index].ToString();
            Verbose("llList2Float({0},{1})={2}", src.ToVerboseString(), index, result);
            return result;
        }

        public integer llList2Integer(list src, integer index)
        {
            integer result;
            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                result = 0;
            else
                result = (integer)src[index].ToString();
            Verbose("llList2Integer({0},{1})={2}", src.ToVerboseString(), index, result);
            return result;
        }

        public string llList2Json(string sType, list lValues)
        {
            //TODO determine return value
            string sReturn = JSON_INVALID;
            Verbose("llList2Json({0}, {1})= {2}", sType, lValues, sReturn);
            return sReturn;
        }

        public key llList2Key(list src, integer index)
        {
            key result;
            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                result = NULL_KEY;
            else
                result = (key)src[index].ToString();
            Verbose("llList2Key({0},{1})={2}", src.ToVerboseString(), index, result);
            return result;
        }

        // ok
        public list llList2List(list src, integer _start, integer _end)
        {
            int intLength = src.Count;

            int start = _start;
            int end = _end;

            list result = new list();

            if (CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (int intI = start; intI <= end; intI++)
                        result.Add(src[intI]);
                }
                else // excluding
                {
                    for (int intI = 0; intI <= end; intI++)
                        result.Add(src[intI]);
                    for (int intI = start; intI < intLength; intI++)
                        result.Add(src[intI]);
                }
            }

            Verbose(string.Format(@"List2List(""{0}"",{1},{2})=""{3}""", src, _start, _end, result));
            return result;
        }

        // ok
        public list llList2ListStrided(list src, integer _start, integer _end, integer stride)
        {
            int intLength = src.Count;

            int start = _start;
            int end = _end;

            list temp = new list();

            if (CorrectIt(intLength, ref start, ref end))
            {
                if (start <= end)
                {
                    for (int intI = start; intI <= end; intI++)
                        temp.Add(src[intI]);
                }
                else // excluding
                {
                    for (int intI = 0; intI <= end; intI++)
                        temp.Add(src[intI]);
                    for (int intI = start; intI < intLength; intI++)
                        temp.Add(src[intI]);
                }
            }
            list result = new list();
            string remark = "";
            if (stride <= 0)
            {
                remark = " ** stride must be > 0 **";
            }
            else
            {
                if (start == 0)
                    for (int intI = 0; intI < temp.Count; intI += stride)
                        result.Add(temp[intI]);
                else
                    for (int intI = stride - 1; intI < temp.Count; intI += stride)
                        result.Add(temp[intI]);
            }
            Verbose(@"List2ListStrided({0},{1},{2},{3})={4}{5}", src.ToVerboseString(), start, end, stride, result.ToVerboseString(), remark);
            return result;
        }

        public rotation llList2Rot(list src, integer index)
        {
            rotation result;
            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                result = ZERO_ROTATION;
            else
            {
                if (src[index] is rotation)
                    result = (rotation)src[index];
                else
                    result = ZERO_ROTATION;
            }
            Verbose("llList2Rot({0},{1})={2}", src.ToVerboseString(), index, result);
            return result;
        }

        public String llList2String(list src, integer index)
        {
            String result;
            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                result = "";
            else
                result = (String)src[index].ToString();
            Verbose("llList2String({0},{1})={2}", src.ToVerboseString(), index, result);
            return result;
        }

        public vector llList2Vector(list src, integer index)
        {
            vector result;
            if (index < 0)
                index = src.Count + index;
            if (index >= src.Count || index < 0)
                result = ZERO_VECTOR;
            else
            {
                if (src[index] is vector)
                    result = (vector)src[index];
                else
                    result = ZERO_VECTOR;
            }
            Verbose("llList2Vector({0},{1})={2}", src.ToVerboseString(), index, result);
            return result;
        }

        // ok, 27 mei 2007
        public integer llListFindList(list src, list test)
        {
            if (src.Count == 0)
                return -1;
            if (test.Count == 0)
                return 0;
            if (test.Count > src.Count)
                return -1;

            int intReturn = -1;
            for (int intI = 0; intI <= (src.Count - test.Count); intI++)
            {
                if (test[0].Equals(src[intI]))
                {
                    bool blnOkay = true;
                    for (int intJ = 1; intJ < test.Count; intJ++)
                    {
                        if (!test[intJ].Equals(src[intI + intJ]))
                        {
                            blnOkay = false;
                            break;
                        }
                    }
                    if (blnOkay)
                    {
                        intReturn = intI;
                        break;
                    }
                }
            }
            Verbose("llListFindList({0},{1}={2}", src.ToVerboseString(), test.ToVerboseString(), intReturn);
            return intReturn;
        }

        // ok
        public list llListInsertList(list dest, list src, int pos)
        {
            int intLength = dest.Count;
            list result = new list();
            if (pos < 0)
                pos = dest.Count + pos;

            for (int intI = 0; intI < Math.Min(pos, intLength); intI++)
                result.Add(dest[intI]);

            result.AddRange(src);

            for (int intI = Math.Max(0, pos); intI < intLength; intI++)
                result.Add(dest[intI]);

            Verbose("llListInsertList({0},{1},{2})={3}", dest.ToVerboseString(), src.ToVerboseString(), pos, result.ToVerboseString());
            return result;
        }

        // ok
        public list llListRandomize(list src, int stride)
        {
            list l;
            ArrayList buckets = List2Buckets(src, stride);
            if (buckets == null)
                l = new list(src);
            else
                l = Buckets2List(RandomShuffle(buckets), stride);
            Verbose("llListRandomize({0},{1})={2}", src.ToVerboseString(), stride, l.ToVerboseString());
            return l;
        }

        // TODO check this!!!
        public list llListReplaceList(list dest, list src, int start, int end)
        {
            int intLength = dest.Count;

            CorrectIt(intLength, ref start, ref end);

            list result = new list();
            if (start <= end)
            {
                for (int intI = 0; intI < start; intI++)
                    result.Add(dest[intI]);
                result.AddRange(src);
                for (int intI = end + 1; intI < intLength; intI++)
                    result.Add(dest[intI]);
            }
            else
            {
                // where to add src?????
                for (int intI = end; intI <= start; intI++)
                    result.Add(dest[intI]);
            }
            Verbose("llListReplaceList({0},{1},{2},{3}={4}", dest.ToVerboseString(), src.ToVerboseString(), start, end, result.ToVerboseString());
            return result;
        }

        // ok
        public list llListSort(list src, int stride, int ascending)
        {
            list result;
            ArrayList buckets = List2Buckets(src, stride);
            if (buckets == null)
                result = new list(src);
            else
            {
                buckets.Sort(new BucketComparer(ascending));
                result = Buckets2List(buckets, stride);
            }
            Verbose("llListSort({0},{1},{2})={3}", src.ToVerboseString(), stride, ascending, result.ToVerboseString());
            return result;
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
        public Float llListStatistics(integer _operation, list _input)
        {
            double result = 0.0;
            double rmin, rmax;
            int operation = _operation;
            List<double> input = GetListOfNumbers(_input);
            if (input.Count > 0)
            {
                switch (operation)
                {
                    case 0: // LIST_STAT_RANGE:
                        rmin = double.MaxValue;
                        rmax = double.MinValue;
                        for (int intI = 0; intI < input.Count; intI++)
                        {
                            if (input[intI] < rmin)
                                rmin = input[intI];
                            if (input[intI] > rmax)
                                rmax = input[intI];
                        }
                        result = rmax - rmin;
                        break;
                    case 1: //LIST_STAT_MIN:
                        result = double.MaxValue;
                        for (int intI = 0; intI < input.Count; intI++)
                            if (input[intI] < result)
                                result = input[intI];
                        break;
                    case 2: //LIST_STAT_MAX:
                        result = double.MinValue;
                        for (int intI = 0; intI < input.Count; intI++)
                            if (input[intI] > result)
                                result = input[intI];
                        break;
                    case 3: //LIST_STAT_MEAN:
                        for (int intI = 0; intI < input.Count; intI++)
                            result += input[intI];
                        result = result / input.Count;
                        break;
                    case 4: //LIST_STAT_MEDIAN:
                        input.Sort();
                        if (Math.Ceiling(input.Count * 0.5) == input.Count * 0.5)
                            result = (input[(int)(input.Count * 0.5 - 1)] + input[(int)(input.Count * 0.5)]) / 2;
                        else
                            result = input[((int)(Math.Ceiling(input.Count * 0.5))) - 1];
                        break;
                    case 5: //LIST_STAT_STD_DEV:
                        result = GetStandardDeviation(input.ToArray());
                        break;
                    case 6: //LIST_STAT_SUM:
                        for (int intI = 0; intI < input.Count; intI++)
                            result += input[intI];
                        break;
                    case 7: //LIST_STAT_SUM_SQUARES:
                        for (int intI = 0; intI < input.Count; intI++)
                            result += input[intI] * input[intI];
                        //double av = GetAverage(input.ToArray());
                        //for (int intI = 0; intI < input.Count; intI++)
                        //	result += (av - input[intI]) * (av - input[intI]);
                        break;
                    case 8: //LIST_STAT_NUM_COUNT:
                        result = input.Count;
                        break;
                    case 9: //LIST_STAT_GEOMETRIC_MEAN:
                        for (int intI = 0; intI < input.Count; intI++)
                            input[intI] = Math.Log(input[intI]);
                        result = Math.Exp(GetAverage(input.ToArray()));
                        break;
                    default:
                        break;
                }
            }
            Verbose("llListStatistics({0},{1})={2}", _operation, _input.ToString(), result);
            return result;
        }

        public integer llListen(integer channel, String name, key id, String msg)
        {
            int intHandle = host.llListen(channel, name, id, msg);
            Verbose(@"Listen(" + channel + @",""" + name + @"""," + id + @",""" + msg + @""")=" + intHandle);
            return intHandle;
        }

        public void llListenControl(integer number, integer active)
        {
            Verbose("llListenControl(" + number + "," + active + ")");
            host.llListenControl(number, active);
        }

        public void llListenRemove(integer number)
        {
            Verbose("llListenRemove(" + number + ")");
            host.llListenRemove(number);
        }

        public void llLoadURL(key avatar_id, String message, String url)
        {
            Verbose("llLoadURL(" + avatar_id + "," + message + "," + url + ")");
            string strUrl = url.ToString();
            if (strUrl.StartsWith("http://"))
                System.Diagnostics.Process.Start(strUrl);
        }

        public Float llLog(Float val)
        {
            double dblA = 0.0;
            if (val > 0.0)
                dblA = Math.Log(val);
            Verbose("llLog({0})={1}", val, dblA);
            return dblA;
        }

        public Float llLog10(Float val)
        {
            double dblA = 0.0;
            if (val > 0.0)
                dblA = Math.Log10(val);
            Verbose("llLog10({0})={1}", val, dblA);
            return dblA;
        }

        public void llLookAt(vector target, Float strength, Float damping)
        {
            Verbose("llLookAt(" + target + "," + strength + "," + damping + ")");
        }

        public void llLoopSound(String sound, Float volume)
        {
            try
            {
                System.Media.SoundPlayer sp = host.GetSoundPlayer(sound);
                sp.PlayLooping();
            }
            catch
            {
            }
            Verbose("llLoopSound(" + sound + "," + volume + ")");
        }

        public void llLoopSoundMaster(String sound, Float volume)
        {
            try
            {
                System.Media.SoundPlayer sp = host.GetSoundPlayer(sound);
                sp.PlayLooping();
            }
            catch
            {
            }
            Verbose("llLoopSoundMaster(" + sound + "," + volume + ")");
        }

        public void llLoopSoundSlave(String sound, Float volume)
        {
            try
            {
                System.Media.SoundPlayer sp = host.GetSoundPlayer(sound);
                sp.PlayLooping();
            }
            catch
            {
            }
            Verbose("llLoopSoundSlave(" + sound + "," + volume + ")");
        }

        // ok
        public String llMD5String(String src, integer nonce)
        {
            System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(Encoding.ASCII.GetBytes(src + ":" + nonce.ToString()));
            StringBuilder sb = new StringBuilder();
            foreach (byte hex in hash)
                sb.Append(hex.ToString("x2")); //convert to standard MD5 form
            Verbose("llMD5String({0},{1})={2}", src, nonce, sb);
            return sb.ToString();
        }

        public void llMakeExplosion(integer particles, Float scale, Float velocity, Float lifetime, Float arc, String texture, vector offset)
        {
            Verbose("llMakeExplosion(" + particles + "," + scale + "," + velocity + "," + lifetime + "," + arc + "," + texture + "," + offset + ")");
        }

        public void llMakeFire(integer particles, Float scale, Float velocity, Float lifetime, Float arc, String texture, vector offset)
        {
            Verbose("llMakeFire(" + particles + "," + scale + "," + velocity + "," + lifetime + "," + arc + "," + texture + "," + offset + ")");
        }

        public void llMakeFountain(integer particles, Float scale, Float velocity, Float lifetime, Float arc, String texture, vector offset)
        {
            Verbose("llMakeFountain(" + particles + "," + scale + "," + velocity + "," + lifetime + "," + arc + "," + texture + "," + offset + ")");
        }

        public void llMakeSmoke(integer particles, Float scale, Float velocity, Float lifetime, Float arc, String texture, vector offset)
        {
            Verbose("llMakeSmoke(" + particles + "," + scale + "," + velocity + "," + lifetime + "," + arc + "," + texture + "," + offset + ")");
        }

        public void llManageEstateAccess(integer iAction, key kID)
        {
            Verbose("llManageEstateAccess({0}, {1}", iAction, kID);
        }

        public void llMapDestination(String simname, vector position, vector lookat)
        {
            Verbose("llMapDestination(" + simname + "," + position + "," + lookat + ")");
        }

        public void llMessageLinked(integer linknum, integer num, String str, key id)
        {
            Verbose("llMessageLinked(" + linknum + "," + num + "," + str + "," + id + ")");
            host.MessageLinked(linknum, num, str, id);
        }

        public void llMinEventDelay(Float delay)
        {
            Verbose("llMinEventDelay(" + delay + ")");
        }

        public integer llModPow(integer x, integer y, integer m)
        {
            integer result = ModPow2(x, y, m);
            Verbose("llModPow({0},{1},{2})={3}", x, y, m, result);
            return result;
        }

        public void llModifyLand(integer action, integer size)
        {
            Verbose("llModifyLand(" + action + "," + size + ")");
        }

        public void llMoveToTarget(vector target, Float tau)
        {
            Verbose("llMoveToTarget(" + target + "," + tau + ")");
        }

		public void llNavigateTo(vector Location, list Options)
		{
			Verbose("llNavigateTo({0}, {1})", Location, Options);
		}

        public void llOffsetTexture(Float offset_s, Float offset_t, integer face)
        {
            Verbose("llOffsetTexture(" + offset_s + "," + offset_t + "," + face + ")");
        }

        public void llOpenRemoteDataChannel()
        {
            host.llOpenRemoteDataChannel();
            Verbose("llOpenRemoteDataChannel()");
        }

        public integer llOverMyLand(key id)
        {
            Verbose("llOverMyLand(" + id + ")");
            return integer.TRUE;
        }

        public void llOwnerSay(String message)
        {
            Chat(0, message, CommunicationType.OwnerSay);
        }

        public void llParcelMediaCommandList(list command_list)
        {
            Verbose("llParcelMediaCommandList(" + command_list.ToString() + ")");
        }

        public list llParcelMediaQuery(list query_list)
        {
            Verbose("llParcelMediaQuery(" + query_list.ToString() + ")");
            return new list();
        }

        // 21 sep 2007, check this
        public list llParseString2List(String src, list separators, list spacers)
        {
            list result = ParseString(src, separators, spacers, false);
            Verbose("llParseString2List({0},{1},{2})={3}", src, separators.ToVerboseString(), spacers.ToVerboseString(), result.ToVerboseString());
            return result;
        }

        // 21 sep 2007, check this, first check 3 oct 2007, last element=="" is added also
        public list llParseStringKeepNulls(String src, list separators, list spacers)
        {
            list result = ParseString(src, separators, spacers, true);
            Verbose("llParseStringKeepNulls({0},{1},{2})={3}", src, separators.ToVerboseString(), spacers.ToVerboseString(), result.ToVerboseString());
            return result;
        }

        public void llParticleSystem(list parameters)
        {
            Verbose("llParticleSystem(" + parameters.ToString() + ")");
        }

        public void llPassCollisions(integer pass)
        {
            Verbose("llPassCollisions(" + pass + ")");
        }

        public void llPassTouches(integer pass)
        {
            Verbose("llPassTouches(" + pass + ")");
        }

		public void llPatrolPoints(list Points, list Options)
		{
			Verbose("llPatrolPoints({0}, {1})", Points, Options);
		}

        public void llPlaySound(String sound, Float volume)
        {
            try
            {
                System.Media.SoundPlayer sp = host.GetSoundPlayer(sound);
                sp.Play();
                Verbose("llPlaySound(" + sound + "," + volume + ")");
            }
            catch (Exception exception)
            {
                Verbose("llPlaySound(" + sound + "," + volume + ") **" + exception.Message);
            }
        }

        public void llPlaySoundSlave(String sound, Float volume)
        {
            try
            {
                System.Media.SoundPlayer sp = host.GetSoundPlayer(sound);
                sp.Play();
            }
            catch
            {
            }
            Verbose("llPlaySoundSlave(" + sound + "," + volume + ")");
        }

        public void llPointAt(vector pos)
        {
            Verbose("llPointAt(" + pos + ")");
        }

        public Float llPow(Float baze, Float exp)
        {
            double dblA = Math.Pow(baze, exp);
            Verbose("llPow(" + baze + "," + exp + ")=" + dblA);
            return dblA;
        }

        public void llPreloadSound(String sound)
        {
            Verbose("llPreloadSound(" + sound + ")");
        }

		public void llPursue(key TargetID, list Options)
		{
			Verbose("llPursue({0}, {1})", TargetID, Options);
		}

        public void llPushObject(key id, vector impulse, vector angular_impulse, integer local)
        {
            Verbose("llPushObject(" + id + "," + impulse + "," + angular_impulse + "," + local + ")");
        }

        public void llRegionSay(integer channel, String text)
        {
            if (channel != 0)
                Chat(channel, text, CommunicationType.RegionSay);
        }

        public void llRegionSayTo(key kTargetID, integer iChannel, string iText)
        {
            Verbose("llRegionSayTo({0}, {1}, {2})", kTargetID, iChannel, iText);
        }

        public void llReleaseCamera(key agent)
        {
            Verbose("llReleaseCamera(" + agent + ")");
        }

        public void llReleaseControls()
        {
            Verbose("llReleaseControls()");
            this.host.ReleaseControls();
        }

        public void llReleaseControls(key avatar)
        {
            Verbose("llReleaseControls(" + avatar + ")");
        }

        //347
        public void llReleaseURL(string url)
        {
        }

        public void llRemoteDataReply(key channel, key message_id, String sdata, integer idata)
        {
            host.llRemoteDataReply(channel, message_id, sdata, idata);
            Verbose("llRemoteDataReply({0},{1},{2},{3})", channel, message_id, sdata, idata);
        }

        public void llRemoteDataSetRegion()
        {
            Verbose("llRemoteDataSetRegion()");
        }

        public void llRemoteLoadScript(key target, String name, integer running, integer param)
        {
            Verbose("llRemoteLoadScript(" + target + "," + name + "," + running + "," + param + ")");
        }

        public void llRemoteLoadScriptPin(key target, String name, integer pin, integer running, integer start_param)
        {
            Verbose("llRemoteLoadScriptPin(" + target + "," + name + "," + pin + "," + running + "," + start_param + ")");
        }

        public void llRemoveFromLandBanList(key avatar)
        {
            Verbose("llRemoveFromLandBanList(" + avatar + ")");
            if (m_LandBanList.ContainsKey(avatar))
                m_LandBanList.Remove(avatar);
        }

        public void llRemoveFromLandPassList(key avatar)
        {
            Verbose("llRemoveFromLandPassList(" + avatar + ")");
            if (m_LandPassList.ContainsKey(avatar))
                m_LandPassList.Remove(avatar);
        }

        public void llRemoveInventory(String inventory)
        {
            host.RemoveInventory(inventory);
            Verbose("llRemoveInventory(" + inventory + ")");
        }

        public void llRemoveVehicleFlags(integer flags)
        {
            Verbose("llRemoveVehicleFlags(" + flags + ")");
        }

        public key llRequestAgentData(key id, integer data)
        {
            key k = new key(Guid.NewGuid());

            string strData = "***";
            switch ((int)data)
            {
                case 1: // DATA_ONLINE
                    break;
                case 2: // DATA_NAME
                    strData = Properties.Settings.Default.AvatarName;
                    break;
                case 3: // DATA_BORN
                    strData = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case 4: // DATA_RATING
                    break;
                case 8: // DATA_PAYINFO
                    break;
                default:
                    break;
            }
            host.ExecuteSecondLife("dataserver", k, (SecondLife.String)strData);
            return k;
        }

        public key llRequestDisplayName(key kAvatarID)
        {
            key kID = new key(Guid.NewGuid());
            string strData = "dummyDisplay Name";
            Verbose("llRequestDisplayName({0})={1}", kAvatarID, kID);
            host.ExecuteSecondLife("dataserver", kID, (SecondLife.String)strData);
            return kID;
        }

        public key llRequestInventoryData(String name)
        {
            Verbose("llRequestInventoryData(" + name + ")");
            return new key(Guid.NewGuid());
        }

        public void llRequestPermissions(key avatar, integer perm)
        {
            Verbose("llRequestPermissions(" + avatar + "," + perm + ")");
            this.host.llRequestPermissions(avatar, perm);
        }

        //346
        public key llRequestSecureURL()
        {
            return new key();
        }

        public key llRequestSimulatorData(String simulator, integer data)
        {
            Verbose("llRequestSimulatorData(" + simulator + "," + data + ")");
            return NULL_KEY;
        }

        //345
        public key llRequestURL()
        {
            return new key();
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
            m_LandBanList = new Hashtable();
            Verbose("llResetLandBanList()");
        }

        public void llResetLandPassList()
        {
            m_LandPassList = new Hashtable();
            Verbose("llResetLandPassList()");
        }

        public void llResetOtherScript(String name)
        {
            Verbose("llResetOtherScript(" + name + ")");
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
            m_DateTimeScriptStarted = DateTime.Now.ToUniversalTime();
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

        public void llRezAtRoot(String inventory, vector pos, vector vel, rotation rot, integer param)
        {
            Verbose("llRezAtRoot(" + inventory + "," + pos + "," + vel + "," + rot + "," + param + ")");
        }

        public void llRezObject(String inventory, vector pos, vector vel, rotation rot, integer param)
        {
            Verbose("llRezObject(" + inventory + "," + pos + "," + vel + "," + rot + "," + param + ")");
            object_rez(new key(Guid.NewGuid()));
            on_rez(param);
        }

        public Float llRot2Angle(rotation rot)
        {
            Verbose("llRot2Angle(" + rot + ")");
            return 0F;
        }

        public vector llRot2Axis(rotation rot)
        {
            Verbose("llRot2Axis(" + rot + ")");
            return ZERO_VECTOR;
        }

        public vector llRot2Euler(rotation r)
        {
            // http://rpgstats.com/wiki/index.php?title=LibraryRotationFunctions
            rotation t = new rotation(r.x * r.x, r.y * r.y, r.z * r.z, r.s * r.s);
            double m = (t.x + t.y + t.z + t.s);
            vector v = new vector(0, 0, 0);
            if (m != 0)
            {
                double n = 2 * (r.y * r.s + r.x * r.z);
                double p = m * m - n * n;
                if (p > 0)
                    v = new vector(Math.Atan2(2.0 * (r.x * r.s - r.y * r.z), (-t.x - t.y + t.z + t.s)),
                    Math.Atan2(n, Math.Sqrt(p)), Math.Atan2(2.0 * (r.z * r.s - r.x * r.y), (t.x - t.y - t.z + t.s)));
                else if (n > 0)
                    v = new vector(0, PI_BY_TWO, Math.Atan2((r.z * r.s + r.x * r.y), 0.5 - t.x - t.z));
                else
                    v = new vector(0, -PI_BY_TWO, Math.Atan2((r.z * r.s + r.x * r.y), 0.5 - t.x - t.z));
            }
            Verbose("llRot2Euler(" + r + ")=" + v);
            return v;
        }

        public vector llRot2Fwd(rotation rot)
        {
            vector v = new vector(1.0 / (rot.x * rot.x + rot.y * rot.y + rot.z * rot.z + rot.s * rot.s), 0, 0);
            vector result = v * rot;
            Verbose("llRot2Fwd({0})={1}", rot, result);
            return result;
        }

        public vector llRot2Left(rotation rot)
        {
            vector v = new vector(0, 1.0 / (rot.x * rot.x + rot.y * rot.y + rot.z * rot.z + rot.s * rot.s), 0);
            vector result = v * rot;
            Verbose("llRot2Left({0})={1}", rot, result);
            return result;
        }

        public vector llRot2Up(rotation rot)
        {
            vector v = new vector(0, 0, 1.0 / (rot.x * rot.x + rot.y * rot.y + rot.z * rot.z + rot.s * rot.s));
            vector result = v * rot;
            Verbose("llRot2Left({0})={1}", rot, result);
            return result;
        }

        public void llRotateTexture(Float radians, integer face)
        {
            Verbose("llRotateTexture(" + radians + "," + face + ")");
        }

        public rotation llRotBetween(vector a, vector b)
        {
            Verbose("llRotBetween(" + a + "," + b + ")");
            return ZERO_ROTATION;
        }

        public void llRotLookAt(rotation rot, Float strength, Float damping)
        {
            Verbose("llRotLookAt(" + rot + "," + strength + "," + damping + ")");
        }

        public integer llRotTarget(rotation rot, Float error)
        {
            Verbose("llRotTarget(" + rot + "," + error + ")");
            return 0;
        }

        public void llRotTargetRemove(integer number)
        {
            Verbose("llRotTargetRemove(" + number + ")");
        }

        public integer llRound(Float val)
        {
            int intA = (int)Math.Round(val);
            Verbose("llRound(" + val + ")=" + intA);
            return intA;
        }

        public integer llSameGroup(key id)
        {
            Verbose("llSameGroup(" + id + ")");
            return 0;
        }

        public void llSay(integer channel, String text)
        {
            Chat(channel, text, CommunicationType.Say);
        }

        public void llScaleTexture(Float scale_s, Float scale_t, integer face)
        {
            Verbose("llScaleTexture(" + scale_s + "," + scale_t + "," + face + ")");
        }

        public integer llScriptDanger(vector pos)
        {
            Verbose("llScriptDanger(" + pos + ")");
            return 0;
        }

        public void llScriptProfiler(integer iState)
        {
            Verbose("llScriptProfiler(" + iState + ")");
        }

        public key llSendRemoteData(key channel, String dest, integer idata, String sdata)
        {
            key k = host.llSendRemoteData(channel, dest, idata, sdata);
            Verbose("llSendRemoteData({0},{1},{2},{3})={4}", channel, dest, idata, sdata, k);
            return k;
        }

        public void llSensor(String name, key id, integer type, Float range, Float arc)
        {
            Verbose("llSensor()");
            host.sensor_timer.Stop();
            integer total_number = 1;
            host.ExecuteSecondLife("sensor", total_number);
        }

        public void llSensorRemove()
        {
            Verbose("llSensorRemove()");
            host.sensor_timer.Stop();
        }

        public void llSensorRepeat(String name, key id, integer type, Float range, Float arc, Float rate)
        {
            Verbose("llSensorRepeat(" + name + "," + id + "," + type + "," + range + "," + arc + "," + rate + ")");
            host.sensor_timer.Stop();
            if (rate > 0)
            {
                host.sensor_timer.Interval = (int)Math.Round(rate * 1000);
                host.sensor_timer.Start();
            }
        }

        public void llSetAlpha(Float alpha, integer face)
        {
            Verbose("llSetAlpha(" + alpha + "," + face + ")");
        }

        public void llSetAngularVelocity(vector vForce, integer iLocal)
        {
            Verbose("llSetAngularVelocity(" + vForce + "," + iLocal + ")");
        }

        public void llSetAnimationOverride(String sAnimationState, String sAnimation)
        {
            Verbose("llSetAnimationOverride({0}, {1})", sAnimationState, sAnimation);
        }

        public void llSetBuoyancy(Float buoyancy)
        {
            Verbose("llSetBuoyancy(" + buoyancy + ")");
        }

        public void llSetCameraAtOffset(vector offset)
        {
            Verbose("llSetCameraAtOffset(" + offset + ")");
        }

        public void llSetCameraEyeOffset(vector offset)
        {
            Verbose("llSetCameraEyeOffset(" + offset + ")");
        }

        public void llSetCameraParams(list rules)
        {
            Verbose("llSetCameraParams(" + rules.ToString() + ")");
        }

        public void llSetClickAction(integer action)
        {
            Verbose("llSetClickAction({0})", action);
        }

        public void llSetColor(vector color, integer face)
        {
            Verbose("llSetColor(" + color + "," + face + ")");
        }

		public void llSetContentType(key HTTPRequestID, integer ContentType)
		{
			Verbose("llSetContentType(" + HTTPRequestID + "," + ContentType + ")");
		}

        public void llSetDamage(Float damage)
        {
            Verbose("llSetDamage(" + damage + ")");
        }

        public void llSetForce(vector force, integer local)
        {
            Verbose("llSetForce(" + force + "," + local + ")");
        }

        public void llSetForceAndTorque(vector force, vector torque, integer local)
        {
            Verbose("llSetForceAndTorque(" + force + "," + torque + "," + local + ")");
        }

        public void llSetHoverHeight(Float height, Float water, Float tau)
        {
            Verbose("llSetHoverHeight(" + height + "," + water + "," + tau + ")");
        }

        public void llSetInventoryPermMask(String item, integer mask, integer value)
        {
            Verbose("llSetInventoryPermMask(" + item + "," + mask + "," + value + ")");
        }

        public void llSetKeyframedMotion(list lKeyframes, list lOptions)
        {
            Verbose("llSetKeyframedMotion({0}, {1})", lKeyframes, lOptions.ToString());
        }

        public void llSetLinkAlpha(integer linknumber, Float alpha, integer face)
        {
            Verbose("llSetLinkAlpha(" + linknumber + "," + alpha + "," + face + ")");
        }

		public void llSetLinkCamera(integer LinkNumber, vector EyeOffset, vector LookOffset)
		{
			Verbose("llSetLinkCamera(" + LinkNumber + "," + EyeOffset + "," + LookOffset + ")");
		}

        public void llSetLinkColor(integer linknumber, vector color, integer face)
        {
            Verbose("llSetLinkColor(" + linknumber + "," + color + "," + face + ")");
        }

        public integer llSetLinkMedia(integer iLink, integer iFace, list lParams)
        {
            Verbose("llSetLinkMedia(" + iLink + "," + iFace + "," + lParams.ToString() + ")");
            return STATUS_OK;
        }

        public void llSetLinkPrimitiveParams(integer linknumber, list rules)
        {
            Verbose("llSetLinkPrimitiveParams({0},[{1}])", linknumber, rules);
        }

        public void llSetLinkPrimitiveParamsFast(integer linknumber, list rules)
        {
            Verbose("llSetLinkPrimitiveParamsFast({0},[{1}])", linknumber, rules);
        }

        public void llSetLinkTexture(integer linknumber, String texture, integer face)
        {
            Verbose(@"llSetLinkTexture({0},""{1}"",{2})", linknumber, texture, face);
        }

        public void llSetLinkTextureAnim(integer link, integer mode, integer face, integer sizex, integer sizey, Float start, Float length, Float rate)
        {
            Verbose("llSetLinkTextureAnim(" + link + "," + mode + "," + face + "," + sizex + "," + sizey + "," + start + "," + length + "," + rate + ")");
        }

        public void llSetLocalRot(rotation rot)
        {
            this.m_rotlocal = rot;
            Verbose("llSetLocalRot(" + rot + ")");
        }

        public integer llSetMemoryLimit(integer iLimit)
        {
            Verbose("llSetMemoryLimit(" + iLimit + ")");
            return true;
        }

        public void llSetObjectDesc(String description)
        {
            Verbose("llSetObjectDesc({0})", description);
            host.SetObjectDescription(description);
        }

        public void llSetObjectName(String name)
        {
            Verbose("llSetObjectName({0})", name);
            host.SetObjectName(name);
        }

        public void llSetObjectPermMask(integer mask, integer value)
        {
            Verbose("llSetObjectPermMask(" + mask + "," + value + ")");
        }

        public void llSetParcelMusicURL(String url)
        {
            Verbose("llSetParcelMusicURL(" + url + ")");
			m_ParcelMusicURL = url;
        }

        public void llSetPayPrice(integer price, list quick_pay_buttons)
        {
            Verbose("llSetPayPrice(" + price + "," + quick_pay_buttons.ToString() + ")");
        }

        public void llSetPhysicsMaterial(integer material_bits, Float gravity_multiplier, Float restitution, Float friction, Float density)
        {
            Verbose("llSetPhysicsMaterial(" + material_bits + "," + gravity_multiplier + "," + restitution + "," + friction + "," + density + ")");
        }

        public void llSetPos(vector pos)
        {
            Verbose("llSetPos(" + pos + ")");
            m_pos = pos;
        }

        public integer llSetPrimMediaParams(integer face, list myparams)
        {
            Verbose("llSetPrimMediaParams(" + face + "," + myparams.ToString() + ")");
            return 0;
        }

        public void llSetPrimitiveParams(list rule)
        {
            Verbose("llSetPrimitiveParams(" + rule.ToString() + ")");
        }

		public integer llSetRegionPos(vector Position)
		{
			Verbose("llSetRegionPos(" + Position + ")");
			m_pos = Position;
			return true;
		}

        public void llSetRemoteScriptAccessPin(integer pin)
        {
            Verbose("llSetRemoteScriptAccessPin(" + pin + ")");
        }

        public void llSetRot(rotation rot)
        {
            Verbose("llSetRot(" + rot + ")");
            m_rot = rot;
        }

        public void llSetScale(vector scale)
        {
            Verbose("llSetScale(" + scale + ")");
            m_scale = scale;
        }

        public void llSetScriptState(String name, integer run)
        {
            Verbose("llSetScriptState(" + name + "," + run + ")");
        }

        public void llSetSitText(String text)
        {
            Verbose("llSetSitText(" + text + ")");
            m_SitText = text;
        }

        public void llSetSoundQueueing(integer queue)
        {
            Verbose("llSetSoundQueueing(" + queue + ")");
        }

        public void llSetSoundRadius(Float radius)
        {
            m_SoundRadius = radius;
            Verbose("llSetSoundRadius(" + m_SoundRadius + ")");
        }

        public void llSetStatus(integer status, integer value)
        {
            Verbose("llSetStatus(" + status + "," + value + ")");
        }

        public void llSetText(String text, vector color, Float alpha)
        {
            Verbose("llSetText(" + text + "," + color + "," + alpha + ")");
        }

        public void llSetTexture(String texture, integer face)
        {
            Verbose("llSetTexture(" + texture + "," + face + ")");
        }

        public void llSetTextureAnim(integer mode, integer face, integer sizex, integer sizey, Float start, Float length, Float rate)
        {
            Verbose("llSetTextureAnim(" + mode + "," + face + "," + sizex + "," + sizey + "," + start + "," + length + "," + rate + ")");
        }

        public void llSetTimerEvent(Float sec)
        {
            Verbose("llSetTimerEvent(" + sec + ")");
            host.timer.Stop();
            if (sec > 0)
            {
                host.timer.Interval = (int)Math.Round(sec * 1000);
                host.timer.Start();
            }
        }

        public void llSetTorque(vector torque, integer local)
        {
            Verbose("llSetTorque(" + torque + "," + local + ")");
        }

        public void llSetTouchText(String text)
        {
            Verbose("llSetTouchText(" + text + ")");
        }

        public void llSetVehicleFlags(integer flags)
        {
            Verbose("llSetVehicleFlags(" + flags + ")");
        }

        public void llSetVehicleFloatParam(integer param_name, Float param_value)
        {
            Verbose("llSetVehicledoubleParam(" + param_name + "," + param_value + ")");
        }

        public void llSetVehicleRotationParam(integer param_name, rotation param_value)
        {
            Verbose("llSetVehicleRotationParam(" + param_name + "," + param_value + ")");
        }

        public void llSetVehicleType(integer type)
        {
            Verbose("llSetVehicleType(" + type + ")");
        }

        public void llSetVehicleVectorParam(integer param_name, vector param_value)
        {
            Verbose("llSetVehicleVectorParam(" + param_name + "," + param_value + ")");
        }

        public void llSetVelocity(vector vForce, integer iLocal)
        {
            Verbose("llSetVelocity({0}, {1})", vForce, iLocal);
        }

        // 343
        public String llSHA1String(String src)
        {
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(src.ToString());
            System.Security.Cryptography.SHA1CryptoServiceProvider cryptoTransformSHA1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            string hash = BitConverter.ToString(cryptoTransformSHA1.ComputeHash(buffer)).Replace("-", "");
            Verbose("llSHA1String(\"{0}\")=\"{1}\"", src, hash);
            return hash;
        }

        public void llShout(integer channel, String text)
        {
            Chat(channel, text, CommunicationType.Shout);
        }

        public Float llSin(Float theta)
        {
            double dblA = Math.Sin(theta);
            Verbose("llSin(" + theta + ")=" + dblA);
            return dblA;
        }

        public void llSitTarget(vector offset, rotation rot)
        {
            Verbose("llSitTarget(" + offset + "," + rot + ")");
        }

        public void llSleep(Float sec)
        {
            Verbose("llSleep(" + sec + ")");
            System.Threading.Thread.Sleep((int)Math.Round(sec * 1000));
        }

        public Float llSqrt(Float val)
        {
            double dblA = Math.Sqrt(val);
            Verbose("llSqrt(" + val + ")=" + dblA);
            return dblA;
        }

        public void llStartAnimation(String anim)
        {
            Verbose("llStartAnimation(" + anim + ")");
        }

        public void llStopAnimation(String anim)
        {
            Verbose("llStopAnimation(" + anim + ")");
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

        public integer llStringLength(String src)
        {
            int intLength = ((string)src).Length;
            Verbose(@"StringLength(""{0}"")={1}", src, intLength);
            return intLength;
        }

        public String llStringToBase64(String str)
        {
            string result = StringToBase64(str.ToString());
            Verbose(@"StringToBase64(""{0}"")=""{1}""", str, result);
            return result;
        }

        public String llStringTrim(String text, integer trim_type)
        {
            string strResult = text.ToString();

            if ((trim_type & STRING_TRIM_HEAD) != 0)
                strResult = strResult.TrimStart();

            if ((trim_type & STRING_TRIM_TAIL) != 0)
                strResult = strResult.TrimEnd();

            Verbose(@"llStringTrim(""{0}"",{1})=""{2}""", text, trim_type, strResult);
            return strResult;
        }

        public integer llSubStringIndex(String source, String pattern)
        {
            int intIndexOf = ((string)source).IndexOf((string)pattern);
            Verbose("llSubStringIndex({0},{1})={2}", source, pattern, intIndexOf);
            return intIndexOf;
        }

        public void llTakeCamera(key avatar)
        {
            Verbose("llTakeCamera(" + avatar + ")");
        }

        public void llTakeControls(integer controls, integer accept, integer pass_on)
        {
            Verbose("llTakeControls(" + controls + "," + accept + "," + pass_on + ")");
            this.host.TakeControls(controls, accept, pass_on);
        }

        public Float llTan(Float theta)
        {
            double dblA = Math.Tan(theta);
            Verbose("llTan(" + theta + ")=" + dblA);
            return dblA;
        }

        public integer llTarget(vector position, Float range)
        {
            Verbose("llTarget(" + position + "," + range + ")");
            return 0;
        }

        public void llTargetOmega(vector axis, Float spinrate, Float gain)
        {
            Verbose("llTargetOmega(" + axis + "," + spinrate + "," + gain + ")");
        }

        public void llTargetRemove(integer tnumber)
        {
            Verbose("llTargetRemove(" + tnumber + ")");
        }

        public void llTeleportAgent(key AvatarID, string LandmarkName, vector LandingPoint, vector LookAtPoint)
        {
            Verbose("llTeleportAgentHome({0}, \"{1}\", {2}, {3})", AvatarID, LandmarkName, LandingPoint, LookAtPoint);
        }

        public void llTeleportAgentGlobalCoords(key AvatarID, vector GlobalPosition, vector RegionPosition, vector LookAtPoint)
        {
            Verbose("llTeleportAgentHome({0}, {1}, {2}, {3})", AvatarID, GlobalPosition, RegionPosition, LookAtPoint);
        }

        public void llTeleportAgentHome(key AvatarID)
        {
            Verbose("llTeleportAgentHome({0})", AvatarID);
        }

        // 335
        public void llTextBox(key avatar, String message, integer chat_channel)
        {
            Verbose("llTextBox({0}, \"{1}\", {2})", avatar, message, chat_channel);
            host.llTextBox(avatar, message, chat_channel);
        }

        public String llToLower(String src)
        {
            string strTemp = ((string)src).ToLower();
            Verbose("llToLower(" + src + ")=" + strTemp);
            return strTemp;
        }

        public String llToUpper(String src)
        {
            string strTemp = ((string)src).ToUpper();
            Verbose("llToUpper(" + src + ")=" + strTemp);
            return strTemp;
        }

        public key llTransferLindenDollars(key kPayee, integer iAmount)
        {
            key kID = new key(Guid.NewGuid());
            string strData = kPayee.ToString() + "," + iAmount.ToString();
            Verbose("llTransferLindenDollars(" + kPayee + "," + iAmount + ")");
            host.ExecuteSecondLife("transaction_result", kID, true, (SecondLife.String)strData);
            return kID;
        }

        public void llTriggerSound(String sound, Float volume)
        {
            Verbose("llTriggerSound(" + sound + "," + volume + ")");
        }

        public void llTriggerSoundLimited(String sound, Float volume, vector tne, vector bsw)
        {
            Verbose("llTriggerSoundLimited(" + sound + "," + volume + "," + tne + "," + bsw + ")");
        }

        public String llUnescapeURL(String url)
        {
            byte[] data = Encoding.UTF8.GetBytes(url.ToString());
            List<byte> list = new List<byte>();
            for (int intI = 0; intI < data.Length; intI++)
            {
                byte chrC = data[intI];
                if (chrC == (byte)'%')
                {
                    if (intI < (data.Length - 2))
                        list.Add((byte)
                            (HexToInt(data[intI + 1]) << 4
                            | HexToInt(data[intI + 2])));
                    intI += 2;
                }
                else
                {
                    list.Add(chrC);
                }
            }
            data = list.ToArray();
            int intLen = Array.IndexOf(data, (byte)0x0);
            if (intLen < 0)
                intLen = data.Length;
            string strTmp = Encoding.UTF8.GetString(data, 0, intLen);
            Verbose(string.Format(@"llUnescapeURL(""{0}"")=""{1}""", url, strTmp));
            return strTmp;
        }

        public void llUnSit(key id)
        {
            Verbose("llUnSit(" + id + ")");
        }

		public void llUpdateCharacter(list Options)
		{
			Verbose("llUpdateCharacter({0})", Options);
		}

        public Float llVecDist(vector a, vector b)
        {
            vector vecValue = new vector(a.x - b.x, a.y - b.y, a.z - b.z);
            double dblMag = llVecMag(vecValue);
            Verbose("llVecDist(" + a + "," + b + ")=" + dblMag);
            return dblMag;
        }

        public Float llVecMag(vector vec)
        {
            double dblValue = Math.Sqrt(vec.x * vec.x + vec.y * vec.y + vec.z * vec.z);
            Verbose("llVecMag(" + vec + ")=" + dblValue);
            return dblValue;
        }

        public vector llVecNorm(vector vec)
        {
            double dblMag = llVecMag(vec);
            vector vecValue = new vector(vec.x / dblMag, vec.y / dblMag, vec.z / dblMag);
            Verbose("llVecNorm(" + vec + ")=" + vecValue);
            return vecValue;
        }

        public void llVolumeDetect(integer detect)
        {
            Verbose("llVolumeDetect(" + detect + ")");
        }

		public void llWanderWithin(vector Origin, vector Distance, list Options)
		{
			Verbose("llWanderWithin({0}, {1}, {2})", Origin, Distance, Options);
		}

        public Float llWater(vector offset)
        {
            Verbose("llWater(" + offset + ")");
            return 0F;
        }

        public void llWhisper(integer channel, String text)
        {
            Chat(channel, text, CommunicationType.Whisper);
        }

        public vector llWind(vector offset)
        {
            Verbose("llWind(" + offset + ")");
            return vector.ZERO_VECTOR;
        }

        public String llXorBase64StringsCorrect(String s1, String s2)
        {
            string S1 = Base64ToString(s1.ToString());
            string S2 = Base64ToString(s2.ToString());
            int intLength = S1.Length;
            if (S2.Length == 0)
                S2 = " ";
            while (S2.Length < intLength)
                S2 += S2;
            S2 = S2.Substring(0, intLength);
            StringBuilder sb = new StringBuilder();
            for (int intI = 0; intI < intLength; intI++)
                sb.Append((char)(S1[intI] ^ S2[intI]));
            string result = StringToBase64(sb.ToString());
            Verbose(@"XorBase64StringsCorrect(""{0}"",""{1}"")=""{2}""", s1, s2, result);
            return result;
        }
        #endregion
    }
}
