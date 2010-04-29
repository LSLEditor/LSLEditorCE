// /**
// ********
// *
// * ORIGIONAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
// * The code was donated on 4/28/2010 by Alphons van der Heijden
// * To Brandon'Dimentox Travanti' Husbands & Malcolm J. Kudra which in turn Liscense under the GPLv2.
// * In agreement to Alphons van der Heijden wishes.
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
using System.IO;
using System.Text;
using System.Threading;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using System.Windows.Forms;

using LSLEditor.Helpers;

namespace LSLEditor
{
	public class SecondLifeHostEventArgs : EventArgs
	{
		public string Message;
		public SecondLifeHostEventArgs(string Message)
		{
			this.Message = Message;
		}
	}

	public class SecondLifeHostMessageLinkedEventArgs : EventArgs
	{
		public SecondLife.integer linknum;
		public SecondLife.integer num;
		public SecondLife.String str;
		public SecondLife.key id;

		public SecondLifeHostMessageLinkedEventArgs(SecondLife.integer linknum, SecondLife.integer num, SecondLife.String str, SecondLife.key id)
		{
			this.linknum = linknum;
			this.num = num;
			this.str = str;
			this.id = id;
		}
	}

	public class SecondLifeHostChatEventArgs : EventArgs
	{
		public SecondLife.integer channel;
		public SecondLife.String name;
		public SecondLife.key id;
		public SecondLife.String message;
		public CommunicationType how;

		public SecondLifeHostChatEventArgs(SecondLife.integer channel, SecondLife.String name, SecondLife.key id, SecondLife.String message, CommunicationType how)
		{
			this.channel = channel;
			this.name = name;
			this.id = id;
			this.message = message;
			this.how = how;
		}
	}

	public class SecondLifeHost : IDisposable
	{
		private List<ListenFilter> ListenFilterList;

		private List<Link> LinkList;

		private SecondLife secondLife;
		private TaskQueue taskQueue;
		private AutoResetEvent StateChanged;
		private Thread StateWatcher;

		private LSLEditorForm mainForm;
		private Assembly CompiledAssembly;

		public string FullPath;
		public Guid guid;

		public delegate void SecondLifeHostMessageHandler(object sender, SecondLifeHostEventArgs e);
		public event SecondLifeHostMessageHandler OnVerboseMessage;
		public event SecondLifeHostMessageHandler OnStateChange;

		public delegate void SecondLifeHostChatHandler(object sender, SecondLifeHostChatEventArgs e);
		public event SecondLifeHostChatHandler OnChat;

		public delegate void SecondLifeHostMessageLinkedHandler(object sender, SecondLifeHostMessageLinkedEventArgs e);
		public event SecondLifeHostMessageLinkedHandler OnMessageLinked;

		public event EventHandler OnDie;
		public event EventHandler OnReset;

		public event EventHandler OnListenChannelsChanged;

		public System.Timers.Timer timer;

		public System.Timers.Timer sensor_timer;

		public string CurrentStateName;
		private string NewStateName;

		private string ObjectName;
		private string ObjectDescription;

		public SecondLifeHost(LSLEditorForm mainForm, Assembly CompiledAssembly, string FullPath, Guid guid)
		{
			this.ListenFilterList = null;
			this.LinkList = null;
			this.secondLife = null;
			this.taskQueue = new TaskQueue();
			this.StateChanged = new AutoResetEvent(false);
			this.StateWatcher = new Thread(new ThreadStart(StateWatch));
			this.StateWatcher.Name = "StateWatch";
			this.StateWatcher.IsBackground = true;
			this.StateWatcher.Start();

			this.mainForm = mainForm;
			this.CompiledAssembly = CompiledAssembly;
			this.FullPath = FullPath;
			this.guid = guid;

			this.ObjectName = Path.GetFileNameWithoutExtension(this.FullPath);
			this.ObjectDescription = "";

			this.timer = new System.Timers.Timer();
			this.timer.AutoReset = true;
			this.timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

			this.sensor_timer = new System.Timers.Timer();
			this.sensor_timer.AutoReset = true;
			this.sensor_timer.Elapsed += new System.Timers.ElapsedEventHandler(sensor_timer_Elapsed);

			this.NewStateName = "default";
			this.CurrentStateName = "";
		}

		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			ExecuteSecondLife("timer");
		}

		private void StateWatch()
		{
			while (true)
			{
				this.StateChanged.WaitOne();
				this.taskQueue.Start(); // is implicit Stop() old Queue
				if (this.CurrentStateName != this.NewStateName)
				{
					this.CurrentStateName = this.NewStateName;
					ExecuteSecondLife("state_exit");

					// Changing to CurrentStateName on this thread! (not ExecuteSecondLife)
					this.taskQueue.Invoke(this, "SetState");
				}
			}
		}

		public void State(string strStateName, bool blnForce)
		{
			if (this.CompiledAssembly == null)
				return;
			if (blnForce)
				this.CurrentStateName = "";
			this.NewStateName = strStateName;
			this.StateChanged.Set();
		}

		private void SetState()
		{
			if (CompiledAssembly == null)
				return;
			secondLife = CompiledAssembly.CreateInstance("LSLEditor.State_" + CurrentStateName) as SecondLife;

			if (secondLife == null)
			{
				MessageBox.Show("State " + CurrentStateName+" does not exist!");
				return;
			}

			ListenFilterList = new List<ListenFilter>();

			LinkList = new List<Link>();

			// Make friends
			secondLife.host = this;

			// Update runtime userinterface by calling event handler
			if (OnStateChange != null)
				OnStateChange(this, new SecondLifeHostEventArgs(CurrentStateName));

			ExecuteSecondLife("state_entry");
		}

		public string GetArgumentsFromMethod(string strName)
		{
			if (this.secondLife == null)
				return "";
			MethodInfo mi = secondLife.GetType().GetMethod(strName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
			if (mi == null)
				return "";

			int intI = 0;
			string strArgs = "";
			foreach (ParameterInfo pi in mi.GetParameters())
			{
				if (intI > 0)
					strArgs += ",";
				strArgs += pi.ParameterType.ToString() + " " + pi.Name;
				intI++;
			}
			return strArgs;
		}

		public void VerboseEvent(string strEventName, object[] args)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("*** ");
			sb.Append(strEventName);
			sb.Append('(');
			for (int intI = 0; intI < args.Length; intI++)
			{
				if (intI > 0)
					sb.Append(',');
				sb.Append(args[intI].ToString());
			}
			sb.Append(")");
			VerboseMessage(sb.ToString());
		}

		public void ExecuteSecondLife(string strName, params object[] args)
		{
			if (secondLife == null)
				return;

			VerboseEvent(strName, args);

			this.taskQueue.Invoke(secondLife, strName, args);
		}

		public ArrayList GetEvents()
		{
			ArrayList ar = new ArrayList();
			if (secondLife != null)
			{
				foreach (MethodInfo mi in secondLife.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly))
				{
					ar.Add(mi.Name);
				}
			}
			ar.Sort();
			return ar;
		}

		public void Reset()
		{
			if (OnReset != null)
				OnReset(this, new EventArgs());
		}

		public void Die()
		{
			if (OnDie != null)
				OnDie(this, new EventArgs());

			if (secondLife != null)
			{
				// stop all timers
				this.timer.Stop();
				this.sensor_timer.Stop();

				this.taskQueue.Stop();
				this.taskQueue.Dispose();
				this.taskQueue = null;

				this.secondLife = null;
			}

		}

		public void Dispose()
		{
			if (taskQueue != null)
			{
				this.taskQueue.Stop();
				this.taskQueue.Dispose();
				this.taskQueue = null;
			}
			if (listXmlRpc != null)
			{
				foreach (XMLRPC xmlRpc in listXmlRpc)
				{
					xmlRpc.CloseChannel();
				}
			}
			if (secondLife != null)
			{
				this.timer.Stop();
				this.sensor_timer.Stop();
				this.mainForm = null;
				this.secondLife = null;
			}
		}


		#region Link functions
		private struct Link
		{
			public int number;
			public string name;
			public SecondLife.key id;
			public SecondLife.key target;
			public Link(int number, string name, SecondLife.key id, SecondLife.key target)
			{
				this.number = number;
				this.name = name;
				this.id = id;
				this.target = target;
			}
		}

		public void llBreakAllLinks()
		{
			LinkList = new List<Link>();
		}

		public void llBreakLink(int linknum)
		{
			foreach (Link link in this.LinkList)
			{
				if (link.number == linknum)
				{
					this.LinkList.Remove(link);
					break;
				}
			}
		}

		#endregion

		#region Listen functions

		public string[] GetListenChannels() // for GroupboxEvent
		{
			List<string> list = new List<string>();
			foreach (ListenFilter lf in ListenFilterList)
			{
				list.Add(lf.channel.ToString());
			}
			return list.ToArray();
		}

		private struct ListenFilter
		{
			public int channel;
			public string name;
			public SecondLife.key id;
			public string message;
			public bool active;
			public ListenFilter(int channel, string name, SecondLife.key id, string message)
			{
				this.channel = channel;
				this.name = name;
				this.id = id;
				this.message = message;
				this.active = true;
			}
		}

		public void llListenControl(int number, int active)
		{
			for (int intI = 0; intI < ListenFilterList.Count; intI++)
			{
				ListenFilter lf = ListenFilterList[intI];
				if (lf.GetHashCode() == number)
				{
					lf.active = (active == 1);
					ListenFilterList[intI] = lf;
					break;
				}
			}
		}

		public void llListenRemove(int intHandle)
		{
			for (int intI = 0; intI < ListenFilterList.Count; intI++)
			{
				ListenFilter lf = ListenFilterList[intI];
				if (lf.GetHashCode() == intHandle)
				{
					ListenFilterList.RemoveAt(intI);
					break;
				}
			}
		}

		public int llListen(int channel, string name, SecondLife.key id, string message)
		{
			if (ListenFilterList.Count >= 64)
			{
				Chat(this, 0, "LSLEditor", SecondLife.NULL_KEY, "Maximum of 64 listens!!!", CommunicationType.Shout);
				return 0;
			}
			ListenFilter lf = new ListenFilter(channel, name, id, message);
			ListenFilterList.Add(lf);
			if (OnListenChannelsChanged != null)
				OnListenChannelsChanged(this, null);
			return lf.GetHashCode();
		}

		private bool CheckListenFilter(int channel, string name, SecondLife.key id, string message)
		{
			ListenFilter lfToCheck = new ListenFilter(channel, name, id, message);

			foreach (ListenFilter lf in ListenFilterList)
			{
				if (!lf.active)
					continue;
				if (lf.channel != lfToCheck.channel)
					continue;
				if (lf.name != "" && lf.name != lfToCheck.name)
					continue;
				if (lf.id != Guid.Empty.ToString() && lf.id!="" && lf.id != lfToCheck.id)
					continue;
				if (lf.message != "" && lf.message != lfToCheck.message)
					continue;
				return true;
			}
			return false;
		}

		// sink listen
		public void Listen(SecondLifeHostChatEventArgs e)
		{
			if (secondLife == null)
				return;
			if (CheckListenFilter(e.channel, e.name, e.id, e.message))
				ExecuteSecondLife("listen", e.channel, e.name, e.id, e.message);
		}

		#endregion

		// raise
		public void Chat(object sender, int channel, string name, SecondLife.key id, string message, CommunicationType how)
		{
			if (OnChat != null)
				OnChat(sender, new SecondLifeHostChatEventArgs(channel, name, id, message, how));
		}

		// raise
		public void MessageLinked(SecondLife.integer linknum, SecondLife.integer num, SecondLife.String str, SecondLife.key id)
		{
			if (OnMessageLinked != null)
				OnMessageLinked(this, new SecondLifeHostMessageLinkedEventArgs(linknum, num, str, id));
		}

		// sink
		public void LinkMessage(SecondLifeHostMessageLinkedEventArgs e)
		{
			ExecuteSecondLife("link_message", e.linknum, e.num, e.str, e.id);
		}


		public SecondLife.key Http(string Url, SecondLife.list Parameters, string Body)
		{
			if (secondLife == null)
				return SecondLife.NULL_KEY;

			System.Net.WebProxy proxy = null;
			if (Properties.Settings.Default.ProxyServer != "")
				proxy = new System.Net.WebProxy(Properties.Settings.Default.ProxyServer.Replace("http://", ""));

			if (Properties.Settings.Default.ProxyUserid != "" && proxy != null)
				proxy.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.ProxyUserid, Properties.Settings.Default.ProxyPassword);

			SecondLife.key Key = new SecondLife.key(Guid.NewGuid());
			//WebRequestClass a = new WebRequestClass(proxy, secondLife, Url, Parameters, Body, Key);
			try
			{
				HTTPRequest.Request(proxy, secondLife, Url, Parameters, Body, Key);
			}
			catch(Exception exception)
			{
				VerboseMessage(exception.Message);
			}
			return Key;
		}

		public void Email(string To, string Subject, string Body)
		{
			if (secondLife == null)
				return;

			SmtpClient client = new SmtpClient();
			client.SmtpServer = Properties.Settings.Default.EmailServer;

			string strName = GetObjectName();
			string strObjectName = string.Format("Object-Name: {0}", strName);

			SecondLife.vector RegionCorner = secondLife.llGetRegionCorner();
			string strRegionName = secondLife.llGetRegionName();
			string strRegion = string.Format("Region: {0} ({1},{2})", strRegionName, RegionCorner.x, RegionCorner.y);

			SecondLife.vector pos = secondLife.llGetPos();
			string strPosition = string.Format("Local-Position: ({0},{1},{2})", (int)pos.x, (int)pos.y, (int)pos.z);

			string strPrefix = strObjectName + "\r\n";
			strPrefix += strRegion + "\r\n";
			strPrefix += strPosition + "\r\n\r\n";

			MailMessage msg = new MailMessage();
			msg.To = To;
			msg.Subject = Subject;
			msg.Body = strPrefix + Body;
			msg.From = Properties.Settings.Default.EmailAddress;
			msg.Headers.Add("Reply-to", msg.From);

			//MailAttachment myAttachment = new MailAttachment(strAttachmentFile);
			//msg.Attachments.Add(myAttachment);

			VerboseMessage(client.Send(msg));
		}

		public void VerboseMessage(string Message)
		{
			if (OnVerboseMessage != null)
				OnVerboseMessage(this, new SecondLifeHostEventArgs(Message));
		}

		delegate void ShowDialogDelegate(SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.String message, 
			SecondLife.list buttons,
			SecondLife.integer channel);
		private void Dialog(SecondLifeHost host, 
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.String message,
			SecondLife.list buttons,
			SecondLife.integer channel)
		{
			llDialogForm DialogForm = new llDialogForm(host, objectName, k, name, message, buttons, channel);
			DialogForm.Left = this.mainForm.Right - DialogForm.Width - 5;
			DialogForm.Top = this.mainForm.Top + 30;
			DialogForm.Show(this.mainForm);
			this.mainForm.llDialogForms.Add(DialogForm);
		}

		public void llDialog(SecondLife.key avatar, SecondLife.String message, SecondLife.list buttons, SecondLife.integer channel)
		{
			if (message.ToString().Length >= 512)
			{
				VerboseMessage("llDialog: message too long, must be less than 512 characters");
				return;
			}
			if (message.ToString().Length == 0)
			{
				VerboseMessage("llDialog: must supply a message");
				return;
			}
			for (int intI = 0; intI < buttons.Count; intI++)
			{
				if (buttons[intI].ToString() == "")
				{
					VerboseMessage("llDialog: all buttons must have label strings");
					return;
				}
				if (buttons[intI].ToString().Length > 24)
				{
					VerboseMessage("llDialog:Button Labels can not have more than 24 characters");
					return;
				}
			}

			if (buttons.Count == 0)
				buttons = new SecondLife.list(new string[] { "OK" });

			this.mainForm.Invoke(new ShowDialogDelegate(Dialog), this, (SecondLife.String)GetObjectName(), secondLife.llGetOwner(), (SecondLife.String)Properties.Settings.Default.AvatarName, message, buttons, channel);
		}

		public void SetPermissions(SecondLife.integer intPermissions)
		{
			ExecuteSecondLife("run_time_permissions", intPermissions);
		}

		delegate void RequestPermissionsDelegate(
			SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.key agent,
			SecondLife.integer intPermissions);
		private void RequestPermissions(
			SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.key agent,
			SecondLife.integer intPermissions)
		{
			PermissionsForm PermissionForm = new PermissionsForm(this, GetObjectName(), secondLife.llGetOwner(), Properties.Settings.Default.AvatarName, agent, intPermissions);
			PermissionForm.Left = this.mainForm.Right - PermissionForm.Width - 5;
			PermissionForm.Top = this.mainForm.Top + 30;
			PermissionForm.Show(this.mainForm);
			this.mainForm.PermissionForms.Add(PermissionForm);
		}

		public void llRequestPermissions(SecondLife.key agent, SecondLife.integer intPermissions)
		{
			this.mainForm.Invoke(new RequestPermissionsDelegate(RequestPermissions), 
				this,
				(SecondLife.String)GetObjectName(), 
				secondLife.llGetOwner(), 
				(SecondLife.String)Properties.Settings.Default.AvatarName, 
				agent, 
				intPermissions);
		}

		private int m_intControls = -1;

		public void SendControl(Keys keys)
		{
			if (m_intControls < 0)
				return;
			if (this.secondLife == null)
				return;

			// check againt m_intControls TODO!!!!!

			int held = 0;
			int change = 0;

			if ((keys & Keys.Up) == Keys.Up)
				held |= SecondLife.CONTROL_UP;
			if ((keys & Keys.Down) == Keys.Down)
				held |= SecondLife.CONTROL_DOWN;
			if ((keys & Keys.Left) == Keys.Left)
				held |= SecondLife.CONTROL_LEFT;
			if ((keys & Keys.Right) == Keys.Right)
				held |= SecondLife.CONTROL_RIGHT;

			ExecuteSecondLife("control", (SecondLife.key)Properties.Settings.Default.AvatarKey, (SecondLife.integer)held, (SecondLife.integer)change);
		}

		public void TakeControls(int intControls, int accept, int pass_on)
		{
			this.m_intControls = intControls;
		}

		public void ReleaseControls()
		{
			this.m_intControls = -1;
		}

		#region Notecards
		private void GetNotecardLineWorker(SecondLife.key k, string strPath, int line)
		{
			StreamReader sr = new StreamReader(strPath);
			int intI = 0;
			string strData = SecondLife.EOF;
			while (!sr.EndOfStream)
			{
				string strLine = sr.ReadLine();
				if (intI == line)
				{
					strData = strLine;
					break;
				}
				intI++;
			}
			sr.Close();
			ExecuteSecondLife("dataserver", k, (SecondLife.String)strData);
		}

		public SecondLife.key GetNotecardLine(string name, int line)
		{
			string strPath = mainForm.SolutionExplorer.GetPath(this.guid, name);
			if(strPath == string.Empty)
				strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
			if (!File.Exists(strPath))
			{
				VerboseMessage("Notecard: " + strPath + " not found");
				taskQueue.Invoke(secondLife, "llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
				return SecondLife.NULL_KEY;
			}
			SecondLife.key k = new SecondLife.key(Guid.NewGuid());
			taskQueue.Invoke(this, "GetNotecardLineWorker", k, strPath, line);
			return k;
		}


		private void GetNumberOfNotecardLinesWorker(SecondLife.key k, string strPath)
		{
			StreamReader sr = new StreamReader(strPath);
			int intI = 0;
			while (!sr.EndOfStream)
			{
				string strLine = sr.ReadLine();
				intI++;
			}
			sr.Close();
			ExecuteSecondLife("dataserver", k, (SecondLife.String)intI.ToString());
		}


		public SecondLife.key GetNumberOfNotecardLines(string name)
		{
			string strPath = mainForm.SolutionExplorer.GetPath(this.guid, name);
			if (strPath == string.Empty)
				strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);

			if (!File.Exists(strPath))
			{
				VerboseMessage("Notecard: " + strPath + " not found");
				taskQueue.Invoke(secondLife, "llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
				return SecondLife.NULL_KEY;
			}
			SecondLife.key k = new SecondLife.key(Guid.NewGuid());
			taskQueue.Invoke(this, "GetNumberOfNotecardLinesWorker", k, strPath);
			return k;
		}
		#endregion

		#region XML-RPC

		private List<XMLRPC> listXmlRpc;
		public void llOpenRemoteDataChannel()
		{
			if (listXmlRpc == null)
				listXmlRpc = new List<XMLRPC>();
			XMLRPC xmlRpc = new XMLRPC();
			xmlRpc.OnRequest += new XMLRPC.RequestEventHandler(xmlRpc_OnRequest);
			xmlRpc.OpenChannel(listXmlRpc.Count);
			listXmlRpc.Add(xmlRpc);
			ExecuteSecondLife("remote_data",
				SecondLife.REMOTE_DATA_CHANNEL,
				xmlRpc.guid,
				new SecondLife.key(Guid.NewGuid()),
				(SecondLife.String)("LSLEditor"),
				(SecondLife.integer)(0),
				(SecondLife.String)("Listening on " + xmlRpc.Prefix));
		}

		void xmlRpc_OnRequest(object sender, XmlRpcRequestEventArgs e)
		{
			XMLRPC xmlRpc = sender as XMLRPC;

			ExecuteSecondLife("remote_data",
				SecondLife.REMOTE_DATA_REQUEST,
				e.channel,
				e.message_id,
				e.sender,
				e.iData,
				e.sData);
		}

		public void llCloseRemoteDataChannel(SecondLife.key channel)
		{
			if (listXmlRpc == null)
				return;
			foreach (XMLRPC xmlRpc in listXmlRpc)
			{
				if (xmlRpc.guid == channel.guid)
				{
					xmlRpc.CloseChannel();
					break;
				}
			}
		}
		public void llRemoteDataReply(SecondLife.key channel, SecondLife.key message_id, string sdata, int idata)
		{
			if (listXmlRpc == null)
				return;
			foreach (XMLRPC xmlRpc in listXmlRpc)
			{
				if (xmlRpc.guid == channel.guid)
				{
					xmlRpc.RemoteDataReply(channel.guid, message_id.guid, sdata, idata);
					break;
				}
			}
		}

		// Wiki sais this is not working in InWorld
		public SecondLife.key llSendRemoteData(SecondLife.key channel, string dest, int idata, string sdata)
		{
			XMLRPC xmlRpc = new XMLRPC();
			xmlRpc.OnReply += new XMLRPC.RequestEventHandler(xmlRpc_OnReply);
			SecondLife.key message_id = xmlRpc.SendRemoteData(channel, dest, idata, sdata);
			return message_id;
		}

		// Wiki sais currently disabled
		void xmlRpc_OnReply(object sender, XmlRpcRequestEventArgs e)
		{
			ExecuteSecondLife("remote_data",
				SecondLife.REMOTE_DATA_REPLY,
				e.channel,
				e.message_id,
				(SecondLife.String)"", // Wiki
				e.iData,
				e.sData);
		}
		#endregion

		public string GetObjectName(Guid guid)
		{
			string strObjectName = mainForm.SolutionExplorer.GetObjectName(guid);
			if (strObjectName != string.Empty)
				return strObjectName;
			else
				return this.ObjectName;
		}

		public string GetObjectName()
		{
			return GetObjectName(this.guid);
		}

		public void SetObjectName(string name)
		{
			if (!mainForm.SolutionExplorer.SetObjectName(this.guid, name))
				ObjectName = name;
		}

		public string GetObjectDescription(Guid guid)
		{
			string strObjectDescription = mainForm.SolutionExplorer.GetObjectDescription(guid);
			if (strObjectDescription != string.Empty)
				return strObjectDescription;
			else
				return this.ObjectDescription;
		}

		public string GetObjectDescription()
		{
			return GetObjectDescription(this.guid);
		}

		public void SetObjectDescription(string description)
		{
			if (!mainForm.SolutionExplorer.SetObjectDescription(this.guid, description))
				this.ObjectDescription = description;
		}

		public string GetScriptName()
		{
			string strScriptName = mainForm.SolutionExplorer.GetScriptName(this.guid);
			if (strScriptName == string.Empty)
				strScriptName = this.FullPath;
			if (Properties.Settings.Default.llGetScriptName)
				strScriptName = Path.GetFileNameWithoutExtension(strScriptName);
			else
				strScriptName = Path.GetFileName(strScriptName);
			return strScriptName;
		}

		public SecondLife.key GetKey()
		{
			string strGuid = mainForm.SolutionExplorer.GetKey(this.guid);
			if (strGuid == string.Empty)
				return new SecondLife.key(this.guid);
			return new SecondLife.key(strGuid);
		}

		public SecondLife.String GetInventoryName(SecondLife.integer type, SecondLife.integer number)
		{
			string strInventoryName = mainForm.SolutionExplorer.GetInventoryName(this.guid, type, number);
			if (strInventoryName == string.Empty)
				return "**GetInventoryName only works in SolutionExplorer**";
			return strInventoryName;
		}

		public SecondLife.key GetInventoryKey(SecondLife.String name)
		{
			string strInventoryKey = mainForm.SolutionExplorer.GetInventoryKey(this.guid, name);
			if (strInventoryKey == string.Empty)
				return new SecondLife.key(Guid.Empty);
			return new SecondLife.key(strInventoryKey);
		}

		public SecondLife.integer GetInventoryNumber(SecondLife.integer type)
		{
			return mainForm.SolutionExplorer.GetInventoryNumber(this.guid, type);
		}

		public SecondLife.integer GetInventoryType(SecondLife.String name)
		{
			return mainForm.SolutionExplorer.GetInventoryType(this.guid, name);
		}

		public System.Media.SoundPlayer GetSoundPlayer(string sound)
		{
			string strPath = mainForm.SolutionExplorer.GetPath(this.guid, sound);
			if (strPath == string.Empty)
				strPath = sound;
			return new System.Media.SoundPlayer(strPath);
		}

		private void sensor_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			SecondLife.integer total_number = 1;
			ExecuteSecondLife("sensor", total_number);
		}

	}
}
