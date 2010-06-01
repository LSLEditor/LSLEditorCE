// /**
// ********
// *
// * ORIGINAL CODE BASE IS Copyright (C) 2006-2010 by Alphons van der Heijden
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
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LSLEditor
{
	public partial class SimulatorConsole : UserControl
	{
		public event SecondLifeHost.SecondLifeHostChatHandler OnChat;
		public event EventHandler OnControl;

		private List<string> History;
		private int intHistory;

		private Form[] Children;
		private Solution.SolutionExplorer solutionExplorer;

		public SimulatorConsole(Solution.SolutionExplorer solutionExplorer, Form[] Children)
		{
			InitializeComponent();
			this.solutionExplorer = solutionExplorer;
			this.Children = Children;

			this.textBox1.Focus();
			this.Dock = DockStyle.Fill;
			this.History = new List<string>();
			this.intHistory = 0;

			if (Properties.Settings.Default.SimulatorLocation != Point.Empty)
				this.Location = Properties.Settings.Default.SimulatorLocation;
			if (Properties.Settings.Default.SimulatorSize != Size.Empty)
				this.Size = Properties.Settings.Default.SimulatorSize;

			this.Clear();

			SecondLifeHost.SecondLifeHostChatHandler chathandler = new SecondLifeHost.SecondLifeHostChatHandler(SecondLifeHost_OnChat);
			SecondLifeHost.SecondLifeHostMessageLinkedHandler messagelinkedhandler = new SecondLifeHost.SecondLifeHostMessageLinkedHandler(SecondLifeHost_OnMessageLinked);

			this.OnChat += chathandler;
			this.OnControl += new EventHandler(SimulatorConsole_OnControl);

			this.LocationChanged += new EventHandler(SimulatorConsole_LocationChanged);

			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				editForm.ChatHandler = chathandler;
				editForm.MessageLinkedHandler = messagelinkedhandler;
				editForm.StartCompiler();
			}
		}

		public void Stop()
		{
			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;
				editForm.StopCompiler();
			}
		}

		private void SimulatorConsole_LocationChanged(object sender, EventArgs e)
		{
			Properties.Settings.Default.SimulatorLocation = this.Location;
		}

		private void SecondLifeHost_OnChat(object sender, SecondLifeHostChatEventArgs e)
		{
			this.Listen(e);

			// talk only to the owner
			if (e.how == CommunicationType.OwnerSay)
				return;

			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;

				if (editForm.runtime == null)
					continue;

				if (editForm.runtime.SecondLifeHost == null)
					continue;

				// prevent loops loops loops loops, dont talk to myself
				if (sender != editForm.runtime.SecondLifeHost)
					editForm.runtime.SecondLifeHost.Listen(e);
			}
		}

		private void SecondLifeHost_OnMessageLinked(object sender, SecondLifeHostMessageLinkedEventArgs e)
		{
			SecondLifeHost secondLifeHostSender = sender as SecondLifeHost;

			Guid ObjectGuid = this.solutionExplorer.GetParentGuid(secondLifeHostSender.guid);
			Guid RootObjectGuid = this.solutionExplorer.GetParentGuid(ObjectGuid);

			List<Guid> list;

			int intLinkNum = e.linknum;
			switch (intLinkNum)
			{
				case 1: // LINK_ROOT  , root prim in linked set (but not in a single prim, which is 0)
					list = this.solutionExplorer.GetScripts(RootObjectGuid, false);
					break;
				case -1: // LINK_SET  , all prims in object
					list = this.solutionExplorer.GetScripts(RootObjectGuid, true);
					break;
				case -2: // LINK_ALL_OTHERS  ,  all other prims in object besides prim function is in
					list = this.solutionExplorer.GetScripts(RootObjectGuid, true);
					// remove scripts in prim itself, and below
					foreach (Guid guid in this.solutionExplorer.GetScripts(ObjectGuid, true))
					{
						if (list.Contains(guid))
							list.Remove(guid);
					}
					break;
				case -3: // LINK_ALL_CHILDREN  , all child prims in object
					list = this.solutionExplorer.GetScripts(RootObjectGuid, true);
					// remove root itself
					foreach (Guid guid in this.solutionExplorer.GetScripts(RootObjectGuid, false))
					{
						if (list.Contains(guid))
							list.Remove(guid);
					}
					break;
				case -4: // LINK_THIS
					list = new List<Guid>();
					list.Add(secondLifeHostSender.guid); // 4 feb 2008
					//list = this.solutionExplorer.GetScripts(ObjectGuid, true);
					break;
				default: // Link number
					Guid ObjectNrGuid = this.solutionExplorer.GetGuidFromObjectNr(ObjectGuid, intLinkNum);
					list = this.solutionExplorer.GetScripts(ObjectNrGuid, true);
					break;
			}

			// only send message to running scripts in list
			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;

				if (editForm.runtime == null)
					continue;

				if (editForm.runtime.SecondLifeHost == null)
					continue;

				if(list.Contains(editForm.guid))
					editForm.runtime.SecondLifeHost.LinkMessage(e);
			}
		}

		private void SimulatorConsole_OnControl(object sender, EventArgs e)
		{
			foreach (Form form in this.Children)
			{
				EditForm editForm = form as EditForm;
				if (editForm == null || editForm.IsDisposed)
					continue;

				if (editForm.runtime == null)
					continue;

				if (editForm.runtime.SecondLifeHost == null)
					continue;

				editForm.runtime.SecondLifeHost.SendControl((Keys)sender);
			}
		}

		private delegate void AppendTextDelegate(string strLine);
		public void TalkToSimulatorConsole(string strLine)
		{
			if (this.textBox2.InvokeRequired)
			{
				this.textBox2.Invoke(new AppendTextDelegate(TalkToSimulatorConsole), new object[] { strLine });
			}
			else
			{
				this.textBox2.AppendText(strLine.Replace("\n", "\r\n") + "\r\n");
			}
		}

		private void Chat(int channel, string name, SecondLife.key id, string message, CommunicationType how)
		{
			if (OnChat != null)
				OnChat(this, new SecondLifeHostChatEventArgs(channel, name, id, message, how));
		}

		public void Listen(SecondLifeHostChatEventArgs e)
		{
			// Translate the incomming messages a bit so it looks like SL.
			string strHow = ": ";
			if (e.how == CommunicationType.Shout)
				strHow = " shout: ";

			if (e.how == CommunicationType.Whisper)
				strHow = " whispers: ";

			string strWho = e.name;
			string strMessage = e.message;

			if (e.name == Properties.Settings.Default.AvatarName)
				strWho = "You";

			if (e.message.ToString().StartsWith("/me"))
			{
				strWho = e.name;
				strHow = " ";
				strMessage = e.message.ToString().Substring(3).Trim();
			}

			if (e.channel == 0)
				TalkToSimulatorConsole(strWho + strHow + strMessage);
		}

		private void Speak(CommunicationType how)
		{
			int intChannel = 0;
			string strMessage = this.textBox1.Text.Trim();

			History.Add(strMessage);
			intHistory = History.Count;


			if (strMessage == "")
				return;

			if (strMessage[0] == '/')
			{
				if (strMessage.StartsWith("/me"))
				{
					// do nothing
				}
				else
				{
					string strChannel = "";
					for (int intI = 1; intI < strMessage.Length; intI++)
					{
						if (strMessage[intI] >= '0' && strMessage[intI] <= '9')
						{
							strChannel += strMessage[intI];
							if (intI < 10)
								continue;
						}
						try
						{
							intChannel = Convert.ToInt32(strChannel);
							strMessage = strMessage.Substring(intI).Trim();
						}
						catch
						{
						}
						break;
					}
				}
			}

			SecondLife.key id = new SecondLife.key(Properties.Settings.Default.AvatarKey);

			Chat(intChannel, Properties.Settings.Default.AvatarName, id, strMessage, how);

			this.textBox1.Clear();

			this.buttonSay.Enabled = false;
			this.buttonShout.Enabled = false;
		}

		private void buttonShout_Click(object sender, EventArgs e)
		{
			Speak(CommunicationType.Shout);
			this.textBox1.Focus();
		}


		private void ScrollChat(KeyEventArgs e)
		{
			e.SuppressKeyPress = true;
			if (e.KeyCode == Keys.Up)
				intHistory = Math.Max(0, intHistory - 1);
			if (e.KeyCode == Keys.Down)
				intHistory = Math.Min(History.Count, intHistory + 1);
			this.textBox1.Clear();
			if (intHistory != History.Count)
				this.textBox1.AppendText(History[intHistory]);
		}

		private void textBox1_KeyDown(object sender, KeyEventArgs e)
		{
			this.buttonSay.Enabled = true;
			this.buttonShout.Enabled = true;

			if (e.KeyCode == Keys.Return)
			{
				Speak(CommunicationType.Say);
				e.SuppressKeyPress = true;
			}
			if (e.Control && (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down))
			{
				ScrollChat(e);
			}
			else if (e.KeyCode == Keys.Down ||
				e.KeyCode == Keys.Left ||
				e.KeyCode == Keys.Right ||
				e.KeyCode == Keys.Up
				)
			{
				if (OnControl != null)
					OnControl(e.KeyCode, new EventArgs());
				e.SuppressKeyPress = true;
			}
		}

		private void Simulator_Load(object sender, EventArgs e)
		{
			this.textBox1.Focus();
		}

		public void Clear()
		{
			this.textBox1.Clear();
			this.textBox2.Clear();
			this.textBox1.Focus();
		}

		private void buttonSay_Click(object sender, EventArgs e)
		{
			Speak(CommunicationType.Say);
			this.textBox1.Focus();
		}

		private void textBox2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A)
				this.textBox2.SelectAll();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.textBox2.Focus();
			this.textBox2.Copy();
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.textBox2.Focus();
			this.textBox2.SelectAll();
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.textBox2.Focus();
			this.textBox2.Clear();
		}

	}
}
