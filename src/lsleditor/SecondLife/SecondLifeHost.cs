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
// SecondLifeHost.cs
//
// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using LSLEditor.Helpers;

[module: SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Reviewed.")]

namespace LSLEditor
{
	/// <summary>
	/// Represents an event with a single string argument.
	/// </summary>
	public class SecondLifeHostEventArgs : EventArgs
	{
		/// <summary>
		/// Stores the text of the message.
		/// </summary>
		public string Message;

		/// <summary>
		/// Initialises a new instance of the <see cref="SecondLifeHostEventArgs"/> class.
		/// </summary>
		/// <param name="strMessage">String Text.</param>
		public SecondLifeHostEventArgs(string strMessage)
		{
			this.Message = strMessage;
		}
	}

	/// <summary>
	/// Represents a linked message event.
	/// </summary>
	public class SecondLifeHostMessageLinkedEventArgs : EventArgs
	{
		/// <summary>
		/// Stores the index of the sending link.
		/// </summary>
		public SecondLife.integer LinkIndex;

		/// <summary>
		/// Stores a 32 bit numerical value.
		/// </summary>
		public SecondLife.integer Number;

		/// <summary>
		/// Stores a text string.
		/// </summary>
		public SecondLife.String Text;

		/// <summary>
		/// Stores a key.
		/// </summary>
		public SecondLife.key ID;

		/// <summary>
		/// Initialises a new instance of the <see cref="SecondLifeHostMessageLinkedEventArgs"/> class.
		/// </summary>
		/// <param name="iLinkIndex"></param>
		/// <param name="iNumber"></param>
		/// <param name="sText"></param>
		/// <param name="kID"></param>
		public SecondLifeHostMessageLinkedEventArgs(SecondLife.integer iLinkIndex, SecondLife.integer iNumber, SecondLife.String sText, SecondLife.key kID)
		{
			this.LinkIndex = iLinkIndex;
			this.Number = iNumber;
			this.Text = sText;
			this.ID = kID;
		}
	}

	/// <summary>
	/// Represents a chat event.
	/// </summary>
	public class SecondLifeHostChatEventArgs : EventArgs
	{
		/// <summary>
		/// Stores the 32-bit number of the channel.
		/// </summary>
		public SecondLife.integer Channel;

		/// <summary>
		/// Stores the name of the object/avatar.
		/// </summary>
		public SecondLife.String Name;

		/// <summary>
		/// Stores the key of the objewct/avatar.
		/// </summary>
		public SecondLife.key ID;

		/// <summary>
		/// Stores the text of the message.
		/// </summary>
		public SecondLife.String Message;

		/// <summary>
		/// Stores the type of communication the event represents.
		/// </summary>
		public CommunicationType How;

		/// <summary>
		/// Initialises a new instance of the <see cref="SecondLifeHostChatEventArgs"/> class.
		/// </summary>
		/// <param name="channel">Channel to communicate on. Some communication types have channel limitations.</param>
		/// <param name="name">Name of object/avatar.</param>
		/// <param name="id">UUID of object/avatar.</param>
		/// <param name="message">Text of message.</param>
		/// <param name="how">Type of communication (CommunicationType enum).</param>
		public SecondLifeHostChatEventArgs(SecondLife.integer channel, SecondLife.String name, SecondLife.key id, SecondLife.String message, CommunicationType how)
		{
			this.Channel = channel;
			this.Name = name;
			this.ID = id;
			this.Message = message;
			this.How = how;
		}
	}

	/// <summary>
	/// SecondLifeHost class.
	/// </summary>
	public class SecondLifeHost : IDisposable
	{
		/// <summary>
		/// Stores a list of ListenFilters
		/// </summary>
		private List<ListenFilter> lstListenFilter;

		/// <summary>
		/// Stores a list of the links in an object.
		/// </summary>
		private List<Link> lstLinks;

		/// <summary>
		/// Stores the SecondLife object representing the script.
		/// </summary>
		private SecondLife slSecondLife;

		/// <summary>
		/// not sure?
		/// </summary>
		private TaskQueue tqTaskQueue;

		/// <summary>
		/// Flag indicating an event has occurred.
		/// </summary>
		private readonly AutoResetEvent areStateChanged;

		/// <summary>
		/// Stores and controls a thread?.
		/// </summary>
		private readonly Thread tStateWatcher;

		/// <summary>
		/// An editor form?
		/// </summary>
		public LSLEditorForm efMainForm;

		/// <summary>
		/// A compiled assembly of the LSL script.
		/// </summary>
		private readonly Assembly assCompiledAssembly;

		/// <summary>
		/// Stores the path to the script (including script name).
		/// </summary>
		public string FullPath;

		/// <summary>
		/// Stores globally unique ID.
		/// </summary>
		public Guid GUID;

		/// <summary>
		/// Stores SecondLifeHostMessageHandler function.
		/// </summary>
		/// <param name="sender">Sender of message.</param>
		/// <param name="e">SecondLifeHostEventArgs object.</param>
		public delegate void SecondLifeHostMessageHandler(object sender, SecondLifeHostEventArgs e);

		/// <summary>
		/// OnVerboseMessage event handler.
		/// </summary>
		public event SecondLifeHostMessageHandler OnVerboseMessage;

		/// <summary>
		/// OnStateChange event handler.
		/// </summary>
		public event SecondLifeHostMessageHandler OnStateChange;

		/// <summary>
		/// Stores SecondLifeHostChatHandler function.
		/// </summary>
		/// <param name="sender">The sender.</param>
		/// <param name="e">SecondLifeHostChatEventArgs object.</param>
		public delegate void SecondLifeHostChatHandler(object sender, SecondLifeHostChatEventArgs e);

		/// <summary>
		/// OnChat event handler.
		/// </summary>
		public event SecondLifeHostChatHandler OnChat;

		/// <summary>
		/// Stores SecondLifeHostMessageLinkedHandler function.
		/// </summary>
		/// <param name="sender">The caller.</param>
		/// <param name="e">SecondLifeHostMessageLinkedEventArgs object.</param>
		public delegate void SecondLifeHostMessageLinkedHandler(object sender, SecondLifeHostMessageLinkedEventArgs e);

		/// <summary>
		/// OnMessageLinked event handler.
		/// </summary>
		public event SecondLifeHostMessageLinkedHandler OnMessageLinked;

		/// <summary>
		/// OnDie event handler.
		/// </summary>
		public event EventHandler OnDie;

		/// <summary>
		/// OnReset event handler.
		/// </summary>
		public event EventHandler OnReset;

		/// <summary>
		/// OnListenChannelsChanged event handler.
		/// </summary>
		public event EventHandler OnListenChannelsChanged;

		/// <summary>
		/// Stores a timer.
		/// </summary>
		public System.Timers.Timer Timer;

		/// <summary>
		/// Stores the timer for a Sensor event.
		/// </summary>
		public System.Timers.Timer SensorTimer;

		/// <summary>
		/// Name of currently active state.
		/// </summary>
		public string CurrentStateName;

		/// <summary>
		/// Name of state to switch to.
		/// </summary>
		private string strNewStateName;

		/// <summary>
		/// Name of the containing object/prim.
		/// </summary>
		private string strObjectName;

		/// <summary>
		/// Description of the containing object/prim.
		/// </summary>
		private string strObjectDescription;

		/// <summary>
		/// Initialises a new instance of the <see cref="SecondLifeHost"/> class.
		/// </summary>
		/// <param name="mainForm">Editor form this host is atached to.</param>
		/// <param name="assCompiledAssembly">Assembly of the compiled script.</param>
		/// <param name="strFullPath">Full path (including file name) to the script.</param>
		/// <param name="guid">UUID of the containing object?</param>
		public SecondLifeHost(LSLEditorForm mainForm, Assembly assCompiledAssembly, string strFullPath, Guid guid)
		{
			this.lstListenFilter = null;
			this.lstLinks = null;
			this.slSecondLife = null;
			this.tqTaskQueue = new TaskQueue();
			this.areStateChanged = new AutoResetEvent(false);
			this.tStateWatcher = new Thread(new ThreadStart(this.StateWatch)) {
				Name = "StateWatch",
				IsBackground = true
			};
			this.tStateWatcher.Start();

			this.efMainForm = mainForm;
			this.assCompiledAssembly = assCompiledAssembly;
			this.FullPath = strFullPath;
			this.GUID = guid;

			this.strObjectName = Path.GetFileNameWithoutExtension(this.FullPath);
			this.strObjectDescription = "";

			this.Timer = new System.Timers.Timer {
				AutoReset = true
			};
			this.Timer.Elapsed += this.timer_Elapsed;

			this.SensorTimer = new System.Timers.Timer {
				AutoReset = true
			};
			this.SensorTimer.Elapsed += this.sensor_timer_Elapsed;

			this.strNewStateName = "default";
			this.CurrentStateName = "";
		}

		/// <summary>
		/// Performs the timer event?
		/// </summary>
		/// <param name="sender">The caller.</param>
		/// <param name="e">ElapsedEventArgs object.</param>
		private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			this.ExecuteSecondLife("timer");
		}

		/// <summary>
		/// Watches for the flag to switch state, and enacts the change when needed.
		/// </summary>
		private void StateWatch()
		{
			while (true) {
				this.areStateChanged.WaitOne();
				this.tqTaskQueue.Start(); // is implicit Stop() old Queue
				if (this.CurrentStateName != this.strNewStateName) {
					this.CurrentStateName = this.strNewStateName;
					this.ExecuteSecondLife("state_exit");
					// TODO: EXECUTE OPENSIM FUNCTION HERE

					// Changing to CurrentStateName on this thread! (not ExecuteSecondLife)
					this.tqTaskQueue.Invoke(this, "SetState");
				}
			}
		}

		/// <summary>
		/// Raises the flag for switching state. If the Force argument is true it ensures a state change.
		/// </summary>
		/// <param name="strStateName"></param>
		/// <param name="blnForce"></param>
		public void State(string strStateName, bool blnForce)
		{
			if (this.assCompiledAssembly != null) {
				if (blnForce) {
					this.CurrentStateName = "";
				}
				this.strNewStateName = strStateName;
				this.areStateChanged.Set();
			}
		}

		/// <summary>
		/// Initialises a new state.
		/// </summary>
		private void SetState()
		{
			if (this.assCompiledAssembly != null) {
				this.slSecondLife = this.assCompiledAssembly.CreateInstance("LSLEditor.State_" + this.CurrentStateName) as SecondLife;

				if (this.slSecondLife == null) {
					MessageBox.Show("State " + this.CurrentStateName + " does not exist!");
					return;
				}

				this.lstListenFilter = new List<ListenFilter>();

				this.lstLinks = new List<Link>();

				// Make friends
				this.slSecondLife.host = this;

				// Update runtime userinterface by calling event handler
				OnStateChange?.Invoke(this, new SecondLifeHostEventArgs(this.CurrentStateName));

				this.ExecuteSecondLife("state_entry");
			}
		}

		/// <summary>
		/// Gets a methods arguments using reflection.
		/// </summary>
		/// <param name="strName"></param>
		/// <returns></returns>
		public string GetArgumentsFromMethod(string strName)
		{
			var strArgs = "";
			if (this.slSecondLife != null) {
				var mi = this.slSecondLife.GetType().GetMethod(strName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly);
				if (mi != null) {
					var intI = 0;
					foreach (var pi in mi.GetParameters()) {
						if (intI > 0) {
							strArgs += ",";
						}
						strArgs += pi.ParameterType.ToString() + " " + pi.Name;
						intI++;
					}
				}
			}
			return strArgs;
		}

		/// <summary>
		/// Runtime output of LSL event info.
		/// </summary>
		/// <param name="strEventName"></param>
		/// <param name="args"></param>
		public void VerboseEvent(string strEventName, object[] args)
		{
			var sb = new StringBuilder();
			sb.Append("*** ");
			sb.Append(strEventName);
			sb.Append('(');
			for (var intI = 0; intI < args.Length; intI++) {
				if (intI > 0) {
					sb.Append(',');
				}
				sb.Append(args[intI].ToString());
			}
			sb.Append(")");
			this.VerboseMessage(sb.ToString());
		}

		/// <summary>
		/// Queue the method for execution.
		/// </summary>
		/// <param name="strName">Some method (is it event or state?).</param>
		/// <param name="args">Array of arguments for the method.</param>
		public void ExecuteSecondLife(string strName, params object[] args)
		{
			if (this.slSecondLife != null) {
				this.VerboseEvent(strName, args);

				this.tqTaskQueue.Invoke(this.slSecondLife, strName, args);
			}
		}

		/// <summary>
		/// Fetches the names of all events (limited to current state?) in the script.
		/// </summary>
		/// <returns>List of events.</returns>
		public ArrayList GetEvents()
		{
			var ar = new ArrayList();
			if (this.slSecondLife != null) {
				foreach (var mi in this.slSecondLife.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)) {
					ar.Add(mi.Name);
				}
			}
			ar.Sort();
			return ar;
		}

		/// <summary>
		/// Reset the script?
		/// </summary>
		public void Reset()
		{
			OnReset?.Invoke(this, EventArgs.Empty);
		}

		/// <summary>
		/// Stop executing the script. Resets the queue, timer etc. first.
		/// </summary>
		public void Die()
		{
			OnDie?.Invoke(this, EventArgs.Empty);

			if (this.slSecondLife != null) {
				// stop all timers
				this.Timer.Stop();
				this.SensorTimer.Stop();

				this.tqTaskQueue.Stop();
				this.tqTaskQueue.Dispose();
				this.tqTaskQueue = null;

				this.slSecondLife = null;
			}
		}

		/// <summary>
		/// Disposal method. Resets the Queue, Sensor, Timer, XMLRPC, etc.
		/// </summary>
		public void Dispose()
		{
			if (this.tqTaskQueue != null) {
				this.tqTaskQueue.Stop();
				this.tqTaskQueue.Dispose();
				this.tqTaskQueue = null;
			}
			if (this.listXmlRpc != null) {
				foreach (var xmlRpc in this.listXmlRpc) {
					xmlRpc.CloseChannel();
				}
			}
			if (this.slSecondLife != null) {
				this.Timer.Stop();
				this.SensorTimer.Stop();
				this.efMainForm = null;
				this.slSecondLife = null;
			}
		}

		#region Link functions
		/// <summary>
		/// The Link structure holds data used in Link Messaging.
		/// </summary>
		private struct Link
		{
			/// <summary>
			/// Data - 32 bit integer.
			/// </summary>
			public int Number;

			/// <summary>
			/// Data - string.
			/// </summary>
			public string Text;

			/// <summary>
			/// Data - SL key.
			/// </summary>
			public SecondLife.key ID;

			/// <summary>
			/// Destination for the data.
			/// </summary>
			public SecondLife.key Target;

			/// <summary>
			/// Initialises a new instance of the <see cref="Link"/> type.
			/// </summary>
			/// <param name="number">32 bit integer data.</param>
			/// <param name="name">string data.</param>
			/// <param name="id">SL key data.</param>
			/// <param name="target">Destination for the message.</param>
			public Link(int number, string name, SecondLife.key id, SecondLife.key target)
			{
				this.Number = number;
				this.Text = name;
				this.ID = id;
				this.Target = target;
			}
		}

		/// <summary>
		/// Resets the link list, "breaking" them.
		/// </summary>
		public void llBreakAllLinks()
		{
			this.lstLinks = new List<Link>();
		}

		/// <summary>
		/// Removes the specified Link from the list.
		/// </summary>
		/// <param name="iLinkIndex">The index number of the link to remove.</param>
		public void llBreakLink(int iLinkIndex)
		{
			foreach (var link in this.lstLinks) {
				if (link.Number == iLinkIndex) {
					this.lstLinks.Remove(link);
					break;
				}
			}
		}

		#endregion

		#region Listen functions

		/// <summary>
		/// Fetches the names of the ListenFilters.
		/// </summary>
		/// <returns>Array of ListenFilter names.</returns>
		public string[] GetListenChannels() // for GroupboxEvent
		{
			var list = new List<string>();
			foreach (var lf in this.lstListenFilter) {
				list.Add(lf.Channel.ToString());
			}
			return list.ToArray();
		}

		/// <summary>
		/// ListenFilter type structure.
		/// </summary>
		private struct ListenFilter
		{
			/// <summary>
			/// Channel to listen on.
			/// </summary>
			public int Channel;

			/// <summary>
			/// Name of object/avatar to listen for.
			/// </summary>
			public string Name;

			/// <summary>
			/// Key of object/avatar to listen for.
			/// </summary>
			public SecondLife.key ID;

			/// <summary>
			/// Text from object/avatar to listen for.
			/// </summary>
			public string Message;

			/// <summary>
			/// Flag indicating whether this filter is enabled or not.
			/// </summary>
			public bool Active;

			/// <summary>
			/// Initialises a new instance of the <see cref="ListenFilter"/> type.
			/// </summary>
			/// <param name="channel">Channel to listen to (required).</param>
			/// <param name="name">Name to listen for (can be empty).</param>
			/// <param name="id">UUID to listen for (can be empty/null).</param>
			/// <param name="message">Text to listen for (can be empty).</param>
			public ListenFilter(int channel, string name, SecondLife.key id, string message)
			{
				this.Channel = channel;
				this.Name = name;
				this.ID = id;
				this.Message = message;
				this.Active = true;
			}
		}

		/// <summary>
		/// Control for a ListenFilter
		/// </summary>
		/// <param name="number">32 bit integer handle of the ListenFilter.</param>
		/// <param name="active">Flag indicating whether to enable or disable.</param>
		public void llListenControl(int number, int active)
		{
			for (var intI = 0; intI < this.lstListenFilter.Count; intI++) {
				var lf = this.lstListenFilter[intI];
				if (lf.GetHashCode() == number) {
					lf.Active = active == 1;
					this.lstListenFilter[intI] = lf;
					break;
				}
			}
		}

		/// <summary>
		/// Removes a ListenFilter from the list.
		/// </summary>
		/// <param name="intHandle">32 bit integer handle of the ListenFilter.</param>
		public void llListenRemove(int intHandle)
		{
			for (var intI = 0; intI < this.lstListenFilter.Count; intI++) {
				var lf = this.lstListenFilter[intI];
				if (lf.GetHashCode() == intHandle) {
					this.lstListenFilter.RemoveAt(intI);
					break;
				}
			}
		}

		/// <summary>
		/// Creates a ListenFilter from the llListen paramters.
		/// </summary>
		/// <param name="channel">Channel to listen ot.</param>
		/// <param name="name">Name of object/avatar to listen for.</param>
		/// <param name="id">Key of object/avatar to listen for.</param>
		/// <param name="message">Text from object/avatar to listen for.</param>
		/// <returns>32 bit integer handle.</returns>
		public int llListen(int channel, string name, SecondLife.key id, string message)
		{
			if (this.lstListenFilter.Count >= 64) {
				this.Chat(this, 0, "LSLEditor", SecondLife.NULL_KEY, "Maximum of 64 listens!!!", CommunicationType.Shout);
				return 0;
			}
			var lf = new ListenFilter(channel, name, id, message);
			this.lstListenFilter.Add(lf);
			OnListenChannelsChanged?.Invoke(this, null);
			return lf.GetHashCode();
		}

		/// <summary>
		/// Determines whether paremeters have a matching ListenFilter entry.
		/// </summary>
		/// <param name="channel"></param>
		/// <param name="name"></param>
		/// <param name="id"></param>
		/// <param name="message"></param>
		/// <returns>True if a matche is found, otherwise false.</returns>
		private bool CheckListenFilter(int channel, string name, SecondLife.key id, string message)
		{
			var lfToCheck = new ListenFilter(channel, name, id, message);

			foreach (var lf in this.lstListenFilter) {
				if (!lf.Active) {
					continue;
				}
				if (lf.Channel != lfToCheck.Channel) {
					continue;
				}
				if (lf.Name != "" && lf.Name != lfToCheck.Name) {
					continue;
				}
				if (lf.ID != Guid.Empty.ToString() && lf.ID != "" && lf.ID != lfToCheck.ID) {
					continue;
				}
				if (lf.Message != "" && lf.Message != lfToCheck.Message) {
					continue;
				}
				return true;
			}
			return false;
		}

		/// <summary>
		/// // sink listen
		/// </summary>
		/// <param name="e">SecondLifeHostChatEventArgs object.</param>
		public void Listen(SecondLifeHostChatEventArgs e)
		{
			if (this.slSecondLife != null) {
				if (this.CheckListenFilter(e.Channel, e.Name, e.ID, e.Message)) {
					this.ExecuteSecondLife("listen", e.Channel, e.Name, e.ID, e.Message);
				}
			}
		}

		#endregion

		/// <summary>
		/// Raise a chat event.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="channel">Channel to send message on.</param>
		/// <param name="name">Name of sender.</param>
		/// <param name="id">Key of sender.</param>
		/// <param name="message">Message to send.</param>
		/// <param name="how">CommunicatioType enumerator value.</param>
		public void Chat(object sender, int channel, string name, SecondLife.key id, string message, CommunicationType how)
		{
			OnChat?.Invoke(sender, new SecondLifeHostChatEventArgs(channel, name, id, message, how));
		}

		/// <summary>
		/// Raise a linked message event.
		/// </summary>
		/// <param name="iLlinkIndex"></param>
		/// <param name="iNumber"></param>
		/// <param name="sText"></param>
		/// <param name="kID"></param>
		public void MessageLinked(SecondLife.integer iLlinkIndex, SecondLife.integer iNumber, SecondLife.String sText, SecondLife.key kID)
		{
			OnMessageLinked?.Invoke(this, new SecondLifeHostMessageLinkedEventArgs(iLlinkIndex, iNumber, sText, kID));
		}

		/// <summary>
		/// // sink
		/// </summary>
		/// <param name="e"></param>
		public void LinkMessage(SecondLifeHostMessageLinkedEventArgs e)
		{
			this.ExecuteSecondLife("link_message", e.LinkIndex, e.Number, e.Text, e.ID);
		}

		public SecondLife.key Http(string strURL, SecondLife.list lParameters, string strBody)
		{
			var kID = SecondLife.NULL_KEY;
			if (this.slSecondLife != null) {
				System.Net.WebProxy proxy = null;
				if (Properties.Settings.Default.ProxyServer != "") {
					proxy = new System.Net.WebProxy(Properties.Settings.Default.ProxyServer.Replace("http://", ""));
				}

				if (Properties.Settings.Default.ProxyUserid != "" && proxy != null) {
					proxy.Credentials = new System.Net.NetworkCredential(Properties.Settings.Default.ProxyUserid, Properties.Settings.Default.ProxyPassword);
				}

				kID = new SecondLife.key(Guid.NewGuid());
				////WebRequestClass a = new WebRequestClass(proxy, secondLife, Url, Parameters, Body, Key);
				try {
					HTTPRequest.Request(proxy, this.slSecondLife, strURL, lParameters, strBody, kID);
				} catch (Exception exception) {
					this.VerboseMessage(exception.Message);
				}
			}
			return kID;
		}

		public void Email(string strRecipient, string strSubject, string strBody)
		{
			if (this.slSecondLife != null) {
				var client = new SmtpClient {
					SmtpServer = Properties.Settings.Default.EmailServer
				};

				var strName = this.GetObjectName();
				var strObjectName = string.Format("Object-Name: {0}", strName);

				var vRegionCorner = this.slSecondLife.llGetRegionCorner();
				string strRegionName = this.slSecondLife.llGetRegionName();
				var strRegion = string.Format("Region: {0} ({1},{2})", strRegionName, vRegionCorner.x, vRegionCorner.y);

				var pos = this.slSecondLife.llGetPos();
				var strPosition = string.Format("Local-Position: ({0},{1},{2})", (int)pos.x, (int)pos.y, (int)pos.z);

				var strPrefix = strObjectName + "\r\n";
				strPrefix += strRegion + "\r\n";
				strPrefix += strPosition + "\r\n\r\n";

				var msg = new MailMessage {
					To = strRecipient,
					Subject = strSubject,
					Body = strPrefix + strBody,
					From = Properties.Settings.Default.EmailAddress
				};
				msg.Headers.Add("Reply-to", msg.From);

				////MailAttachment myAttachment = new MailAttachment(strAttachmentFile);
				////msg.Attachments.Add(myAttachment);

				this.VerboseMessage(client.Send(msg));
			}
		}

		public void VerboseMessage(string strMessage)
		{
			if (!string.IsNullOrEmpty(strMessage)) {
				var sb = new StringBuilder(strMessage);
				sb.Replace("\0", "\\0");
				sb.Replace("\a", "\\a");
				sb.Replace("\b", "\\b");
				sb.Replace("\f", "\\f");
				sb.Replace("\n", "\\n");
				sb.Replace("\r", "\\r");
				sb.Replace("\t", "\\t");
				sb.Replace("\v", "\\v");
				strMessage = sb.ToString();
			}

			OnVerboseMessage?.Invoke(this, new SecondLifeHostEventArgs(strMessage));
		}

		public delegate void ShowDialogDelegate(
			SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.String message,
			SecondLife.list buttons,
			SecondLife.integer channel);

		private void Dialog(
			SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.String message,
			SecondLife.list buttons,
			SecondLife.integer channel)
		{
			var lldfDialogForm = new llDialogForm(host, objectName, k, name, message, buttons, channel);
			lldfDialogForm.Left = this.efMainForm.Right - lldfDialogForm.Width - 5;
			lldfDialogForm.Top = this.efMainForm.Top + 30;
			lldfDialogForm.Show(this.efMainForm);
			this.efMainForm.llDialogForms.Add(lldfDialogForm);
		}

		public void llDialog(SecondLife.key avatar, SecondLife.String message, SecondLife.list buttons, SecondLife.integer channel)
		{
			if (message.ToString().Length >= 512) {
				this.VerboseMessage("llDialog: message too long, must be less than 512 characters");
				return;
			}
			if (message.ToString().Length == 0) {
				this.VerboseMessage("llDialog: must supply a message");
				return;
			}
			for (var intI = 0; intI < buttons.Count; intI++) {
				if (buttons[intI].ToString()?.Length == 0) {
					this.VerboseMessage("llDialog: all buttons must have label strings");
					return;
				}
				if (buttons[intI].ToString().Length > 24) {
					this.VerboseMessage("llDialog:Button Labels can not have more than 24 characters");
					return;
				}
			}

			if (buttons.Count == 0) {
				buttons = new SecondLife.list(new string[] { "OK" });
			}

			this.efMainForm.Invoke(new ShowDialogDelegate(this.Dialog), this, (SecondLife.String)this.GetObjectName(), this.slSecondLife.llGetOwner(), (SecondLife.String)Properties.Settings.Default.AvatarName, message, buttons, channel);
		}

		public delegate void ShowTextBoxDelegate(
			SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.String message,
			SecondLife.integer channel);

		private void TextBox(
			SecondLifeHost host,
			SecondLife.String objectName,
			SecondLife.key k,
			SecondLife.String name,
			SecondLife.String message,
			SecondLife.integer channel)
		{
			var tbfTextBoxForm = new llTextBoxForm(host, objectName, k, name, message, channel);
			tbfTextBoxForm.Left = this.efMainForm.Left + (this.efMainForm.Width / 2) - (tbfTextBoxForm.Width / 2);
			tbfTextBoxForm.Top = this.efMainForm.Top + (this.efMainForm.Height / 2) - (tbfTextBoxForm.Height / 2);
			tbfTextBoxForm.Show(this.efMainForm);
			this.efMainForm.llTextBoxForms.Add(tbfTextBoxForm);
		}

		public void llTextBox(SecondLife.key avatar, SecondLife.String message, SecondLife.integer channel)
		{
			if (message.ToString().Length >= 512) {
				this.VerboseMessage("llTextBox: message too long, must be less than 512 characters");
				return;
			}
			if (message.ToString().Length == 0) {
				this.VerboseMessage("llTextBos: must supply a message");
				return;
			}
			this.efMainForm.Invoke(new ShowTextBoxDelegate(this.TextBox), this, (SecondLife.String)this.GetObjectName(), this.slSecondLife.llGetOwner(), (SecondLife.String)Properties.Settings.Default.AvatarName, message, channel);
		}

		public void SetPermissions(SecondLife.integer intPermissions)
		{
			this.ExecuteSecondLife("run_time_permissions", intPermissions);
		}

		private delegate void RequestPermissionsDelegate(
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
			var pfPermissionForm = new PermissionsForm(this, this.GetObjectName(), this.slSecondLife.llGetOwner(), Properties.Settings.Default.AvatarName, agent, intPermissions);
			pfPermissionForm.Left = this.efMainForm.Right - pfPermissionForm.Width - 5;
			pfPermissionForm.Top = this.efMainForm.Top + 30;
			pfPermissionForm.Show(this.efMainForm);
			this.efMainForm.PermissionForms.Add(pfPermissionForm);
		}

		public void llRequestPermissions(SecondLife.key agent, SecondLife.integer intPermissions)
		{
			this.efMainForm.Invoke(
				new RequestPermissionsDelegate(this.RequestPermissions),
				this,
				(SecondLife.String)this.GetObjectName(),
				this.slSecondLife.llGetOwner(),
				(SecondLife.String)Properties.Settings.Default.AvatarName,
				agent,
				intPermissions);
		}

		private int intControls = -1;

		public void SendControl(Keys keys)
		{
			if (this.intControls >= 0 || this.slSecondLife != null) {
				// check againt m_intControls TODO!!!!!
				var held = 0;
				const int change = 0;

				if ((keys & Keys.Up) == Keys.Up) {
					held |= SecondLife.CONTROL_UP;
				}
				if ((keys & Keys.Down) == Keys.Down) {
					held |= SecondLife.CONTROL_DOWN;
				}
				if ((keys & Keys.Left) == Keys.Left) {
					held |= SecondLife.CONTROL_LEFT;
				}
				if ((keys & Keys.Right) == Keys.Right) {
					held |= SecondLife.CONTROL_RIGHT;
				}

				this.ExecuteSecondLife("control", (SecondLife.key)Properties.Settings.Default.AvatarKey, (SecondLife.integer)held, (SecondLife.integer)change);
			}
		}

		public void TakeControls(int intControls, int accept, int pass_on)
		{
			this.intControls = intControls;
		}

		public void ReleaseControls()
		{
			this.intControls = -1;
		}

		#region Notecards
		private void GetNotecardLineWorker(SecondLife.key k, string strPath, int line)
		{
			var sr = new StreamReader(strPath);
			var intI = 0;
			var strData = SecondLife.EOF;
			while (!sr.EndOfStream) {
				var strLine = sr.ReadLine();
				if (intI == line) {
					strData = strLine;
					break;
				}
				intI++;
			}
			sr.Close();
			this.ExecuteSecondLife("dataserver", k, (SecondLife.String)strData);
		}

		public SecondLife.key GetNotecardLine(string name, int line)
		{
			var strPath = this.efMainForm.SolutionExplorer.GetPath(this.GUID, name);
			if (strPath?.Length == 0) {
				strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
			}
			if (!File.Exists(strPath)) {
				this.VerboseMessage("Notecard: " + strPath + " not found");
				this.tqTaskQueue.Invoke(this.slSecondLife, "llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
				return SecondLife.NULL_KEY;
			}
			var k = new SecondLife.key(Guid.NewGuid());
			this.tqTaskQueue.Invoke(this, "GetNotecardLineWorker", k, strPath, line);
			return k;
		}

		private void GetNumberOfNotecardLinesWorker(SecondLife.key k, string strPath)
		{
			var sr = new StreamReader(strPath);
			var intI = 0;
			while (!sr.EndOfStream) {
				var strLine = sr.ReadLine();
				intI++;
			}
			sr.Close();
			this.ExecuteSecondLife("dataserver", k, (SecondLife.String)intI.ToString());
		}

		public SecondLife.key GetNumberOfNotecardLines(string name)
		{
			var strPath = this.efMainForm.SolutionExplorer.GetPath(this.GUID, name);
			if (strPath?.Length == 0) {
				strPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), name);
			}

			if (!File.Exists(strPath)) {
				this.VerboseMessage("Notecard: " + strPath + " not found");
				this.tqTaskQueue.Invoke(this.slSecondLife, "llSay", (SecondLife.integer)0, (SecondLife.String)("Couldn't find notecard " + name));
				return SecondLife.NULL_KEY;
			}
			var k = new SecondLife.key(Guid.NewGuid());
			this.tqTaskQueue.Invoke(this, "GetNumberOfNotecardLinesWorker", k, strPath);
			return k;
		}
		#endregion

		#region XML-RPC

		private List<XMLRPC> listXmlRpc;

		public void llOpenRemoteDataChannel()
		{
			if (this.listXmlRpc == null) {
				this.listXmlRpc = new List<XMLRPC>();
			}
			var xmlRpc = new XMLRPC();
			xmlRpc.OnRequest += this.xmlRpc_OnRequest;
			xmlRpc.OpenChannel(this.listXmlRpc.Count);
			this.listXmlRpc.Add(xmlRpc);
			this.ExecuteSecondLife(
				"remote_data",
				SecondLife.REMOTE_DATA_CHANNEL,
				xmlRpc.guid,
				new SecondLife.key(Guid.NewGuid()),
				(SecondLife.String)"LSLEditor",
				(SecondLife.integer)0,
				(SecondLife.String)("Listening on " + xmlRpc.Prefix));
		}

		private void xmlRpc_OnRequest(object sender, XmlRpcRequestEventArgs e)
		{
			var xmlRpc = sender as XMLRPC;

			this.ExecuteSecondLife(
				"remote_data",
				SecondLife.REMOTE_DATA_REQUEST,
				e.channel,
				e.message_id,
				e.sender,
				e.iData,
				e.sData);
		}

		public void llCloseRemoteDataChannel(SecondLife.key channel)
		{
			if (this.listXmlRpc != null) {
				foreach (var xmlRpc in this.listXmlRpc) {
					if (xmlRpc.guid == channel.guid) {
						xmlRpc.CloseChannel();
						break;
					}
				}
			}
		}

		public void llRemoteDataReply(SecondLife.key channel, SecondLife.key message_id, string sdata, int idata)
		{
			if (this.listXmlRpc != null) {
				foreach (var xmlRpc in this.listXmlRpc) {
					if (xmlRpc.guid == channel.guid) {
						xmlRpc.RemoteDataReply(channel.guid, message_id.guid, sdata, idata);
						break;
					}
				}
			}
		}

		/// <summary>
		/// // Wiki sais this is not working in InWorld
		/// </summary>
		/// <param name="kChannelID"></param>
		/// <param name="dest"></param>
		/// <param name="idata"></param>
		/// <param name="sdata"></param>
		/// <returns></returns>
		public SecondLife.key llSendRemoteData(SecondLife.key kChannelID, string dest, int idata, string sdata)
		{
			var xmlRpc = new XMLRPC();
			xmlRpc.OnReply += this.xmlRpc_OnReply;
			return xmlRpc.SendRemoteData(kChannelID, dest, idata, sdata);
		}

		/// <summary>
		/// // Wiki sais currently disabled
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void xmlRpc_OnReply(object sender, XmlRpcRequestEventArgs e)
		{
			this.ExecuteSecondLife(
				"remote_data",
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
			var strObjectName = this.efMainForm.SolutionExplorer.GetObjectName(guid);
			return strObjectName != string.Empty ? strObjectName : this.strObjectName;
		}

		public string GetObjectName()
		{
			return this.GetObjectName(this.GUID);
		}

		public void SetObjectName(string name)
		{
			if (!this.efMainForm.SolutionExplorer.SetObjectName(this.GUID, name)) {
				this.strObjectName = name;
			}
		}

		public string GetObjectDescription(Guid guid)
		{
			var strObjectDescription = this.efMainForm.SolutionExplorer.GetObjectDescription(guid);
			if (strObjectDescription != string.Empty) {
				return strObjectDescription;
			} else {
				return this.strObjectDescription;
			}
		}

		public string GetObjectDescription()
		{
			return this.GetObjectDescription(this.GUID);
		}

		public void SetObjectDescription(string description)
		{
			if (!this.efMainForm.SolutionExplorer.SetObjectDescription(this.GUID, description)) {
				this.strObjectDescription = description;
			}
		}

		public string GetScriptName()
		{
			var strScriptName = this.efMainForm.SolutionExplorer.GetScriptName(this.GUID);
			if (strScriptName?.Length == 0) {
				strScriptName = this.FullPath;
			}
			return Properties.Settings.Default.llGetScriptName
				? Path.GetFileNameWithoutExtension(strScriptName)
				: Path.GetFileName(strScriptName);
		}

		public SecondLife.key GetKey()
		{
			var strGuid = this.efMainForm.SolutionExplorer.GetKey(this.GUID);
			if (strGuid?.Length == 0) {
				return new SecondLife.key(this.GUID);
			}
			return new SecondLife.key(strGuid);
		}

		public SecondLife.String GetInventoryName(SecondLife.integer type, SecondLife.integer number)
		{
			var strInventoryName = this.efMainForm.SolutionExplorer.GetInventoryName(this.GUID, type, number);
			if (strInventoryName?.Length == 0) {
				return "**GetInventoryName only works in SolutionExplorer**";
			}
			return strInventoryName;
		}

		public SecondLife.key GetInventoryKey(SecondLife.String name)
		{
			var strInventoryKey = this.efMainForm.SolutionExplorer.GetInventoryKey(this.GUID, name);
			if (strInventoryKey?.Length == 0) {
				return new SecondLife.key(Guid.Empty);
			}
			return new SecondLife.key(strInventoryKey);
		}

		public SecondLife.integer GetInventoryNumber(SecondLife.integer type)
		{
			return this.efMainForm.SolutionExplorer.GetInventoryNumber(this.GUID, type);
		}

		public SecondLife.integer GetInventoryType(SecondLife.String name)
		{
			return this.efMainForm.SolutionExplorer.GetInventoryType(this.GUID, name);
		}

		public void RemoveInventory(SecondLife.String name)
		{
			this.efMainForm.SolutionExplorer.RemoveInventory(this.GUID, name);
		}

		public System.Media.SoundPlayer GetSoundPlayer(string sound)
		{
			var strPath = this.efMainForm.SolutionExplorer.GetPath(this.GUID, sound);
			if (strPath?.Length == 0) {
				strPath = sound;
			}
			return new System.Media.SoundPlayer(strPath);
		}

		private void sensor_timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			SecondLife.integer total_number = 1;
			this.ExecuteSecondLife("sensor", total_number);
		}
	}
}
