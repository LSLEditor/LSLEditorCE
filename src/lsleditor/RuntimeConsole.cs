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
//
//
// </summary>

using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

using System.Threading;
using LSLEditor.Helpers;

namespace LSLEditor
{
	public partial class RuntimeConsole : UserControl
	{
		private LSLEditorForm mainForm;

		public RuntimeConsole(LSLEditorForm mainForm)
		{
			InitializeComponent();
			this.mainForm = mainForm;
			this.Dock = DockStyle.Fill;
		}

		private Thread ResetScriptWatcher;
		private AutoResetEvent ResetScriptEvent;
		public SecondLifeHost SecondLifeHost;

		private string CSharpCode;
		private EditForm editForm;

        private bool GetNewHost()
		{
			bool blnResult = false;
            Assembly assembly = null;
            assembly = CompilerHelper.CompileCSharp(editForm, CSharpCode);

            if (assembly != null) {
				if (SecondLifeHost != null) {
					SecondLifeHost.Dispose();
				}
				SecondLifeHost = null;

				SecondLifeHost = new SecondLifeHost(this.mainForm, assembly, editForm.FullPathName, editForm.guid);
				SecondLifeHost.OnChat += editForm.ChatHandler;
				SecondLifeHost.OnMessageLinked += editForm.MessageLinkedHandler;
				SecondLifeHost.OnDie += new EventHandler(host_OnDie);
				SecondLifeHost.OnReset += new EventHandler(SecondLifeHost_OnReset);
				SecondLifeHost.OnListenChannelsChanged += new EventHandler(SecondLifeHost_OnListenChannelsChanged);

				SecondLifeHost.OnVerboseMessage += new SecondLifeHost.SecondLifeHostMessageHandler(host_OnVerboseMessage);
				SecondLifeHost.OnStateChange += new SecondLifeHost.SecondLifeHostMessageHandler(host_OnStateChange);

				SecondLifeHost.State("default", true);

				blnResult = true;
			}
			return blnResult;
		}

        /// <summary>
        /// Converts this script (when it's LSLI) to expanded lsl and writes it.
        /// </summary>
        /// <returns></returns>
        private string ConvertLSLI()
        {
            LSLIConverter lsliConverter = new LSLIConverter();
            string lsl = lsliConverter.ExpandToLSL(editForm);
            string nameExpanded = LSLIPathHelper.CreateExpandedScriptName(editForm.FullPathName);
            string path = LSLIPathHelper.CreateExpandedPathAndScriptName(editForm.FullPathName);

            LSLIPathHelper.DeleteFile(path);

            using (StreamWriter sw = new StreamWriter(path))
            {
                sw.Write(lsl);
            }

            LSLIPathHelper.HideFile(path);
            return lsl;
        }

		public bool Compile(EditForm editForm)
		{
			this.editForm = editForm;

            ResetScriptEvent = new AutoResetEvent(false);
			ResetScriptWatcher = new Thread(new ThreadStart(ResetScriptWatch));
			ResetScriptWatcher.Name = "ResetScriptWatch";
			ResetScriptWatcher.IsBackground = true;
			ResetScriptWatcher.Start();

            string lsl = editForm.SourceCode;

            // If not hidden and not readonly
            if (!editForm.IsHidden && !this.mainForm.IsReadOnly(editForm))
            {
                if (LSLIPathHelper.IsLSLI(editForm.ScriptName)) // Expand LSLI to LSL
                {
                    lsl = ConvertLSLI();
                }
            } else
            {
                this.editForm.StopCompiler();
                return false;
            }

			CSharpCode = MakeSharp(editForm.ConfLSL, lsl);

			return GetNewHost();
		}

		private void SecondLifeHost_OnReset(object sender, EventArgs e)
		{
			this.ResetScriptEvent.Set();
		}

		private void ResetScriptWatch()
		{
			while (true) {
				this.ResetScriptEvent.WaitOne();

				SecondLifeHost.Dispose();
				GetNewHost();
			}
		}

		private delegate void AddListenChannelsDelegate(ComboBox comboBox, string[] channels);
		private void AddListenChannelsToComboxBox(ComboBox comboBox, string[] channels)
		{
			if (comboBox.InvokeRequired) {
				comboBox.Invoke(new AddListenChannelsDelegate(AddListenChannelsToComboxBox),
					new object[] { comboBox, channels });
			} else {
				comboBox.Items.Clear();
				comboBox.Items.AddRange(channels);
				comboBox.SelectedIndex = 0;
			}
		}
		private void SecondLifeHost_OnListenChannelsChanged(object sender, EventArgs e)
		{
			foreach (Control control in this.panel4.Controls) {
				GroupboxEvent gbe = control as GroupboxEvent;
				if (gbe == null) {
					continue;
				}
				foreach (Control control1 in gbe.Controls) {
					GroupBox gb = control1 as GroupBox;
					if (gb == null) {
						continue;
					}
					if (gb.Name != "listen_0") {
						continue;
					}
					ComboBox comboBox = gb.Controls[0] as ComboBox;
					AddListenChannelsToComboxBox(comboBox, SecondLifeHost.GetListenChannels());
				}
			}
		}

		void host_OnDie(object sender, EventArgs e)
		{
			if (this.panel1.InvokeRequired) {
				// using Evenhandler definition as a delegate
				this.panel1.Invoke(new EventHandler(host_OnDie));
			} else {
				this.panel1.Enabled = false;
			}
		}

		private void host_OnStateChange(object sender, SecondLifeHostEventArgs e)
		{
			ShowLifeEvents();
			SetStateCombobox(e.Message);
		}

		private delegate void SetStateComboboxDelegate(string strName);
		public void SetStateCombobox(string strName)
		{
			if (this.comboBox1.InvokeRequired) {
				this.comboBox1.Invoke(new SetStateComboboxDelegate(SetStateCombobox), new object[] { strName });
			} else {
				for (int intI = 0; intI < this.comboBox1.Items.Count; intI++) {
					if (this.comboBox1.Items[intI].ToString() == strName) {
						this.comboBox1.SelectedIndex = intI;
						break;
					}
				}
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, System.EventArgs e)
		{
			if (SecondLifeHost != null) {
				string strStateName = (string)this.comboBox1.Items[this.comboBox1.SelectedIndex];
				if (strStateName != "") {
					if (SecondLifeHost.CurrentStateName != strStateName) {
						SecondLifeHost.State(strStateName, true);
					}
				}
			}
		}

		private void host_OnVerboseMessage(object sender, SecondLifeHostEventArgs e)
		{
			VerboseConsole(e.Message);
		}

		private string MakeSharp(XmlDocument xml, string strC)
		{
			this.comboBox1.Items.Clear();
			this.comboBox1.Items.Add("");
			this.comboBox1.SelectedIndex = 0;

			// xml is needed for making events buttons
			LSL2CSharp translator = new LSL2CSharp(xml);

			strC = translator.Parse(strC);

			foreach (string strState in translator.States) {
				this.comboBox1.Items.Add(strState);
			}
			return strC;
		}

		private delegate void ShowLifeEventsDelegate();
		private void ShowLifeEvents()
		{
			if (this.panel4.InvokeRequired) {
				this.panel4.Invoke(new ShowLifeEventsDelegate(ShowLifeEvents), null);
			} else {
				int intX = 8;
				int intY = 0;

				this.panel4.Controls.Clear();
				foreach (string strEventName in SecondLifeHost.GetEvents()) {
					string strArgs = SecondLifeHost.GetArgumentsFromMethod(strEventName);
					GroupboxEvent ge = new GroupboxEvent(new Point(intX, intY), strEventName, strArgs, new System.EventHandler(this.buttonEvent_Click));
					this.panel4.Controls.Add(ge);
					intY += ge.Height;
				}
			}
		}

		private delegate void AppendTextDelegate(string strLine);

		// Verbose
		public void VerboseConsole(string strLine)
		{
			if (!this.textBox2.IsDisposed) {
				if (this.textBox2.InvokeRequired) {
					this.textBox2.Invoke(new AppendTextDelegate(VerboseConsole), new object[] { strLine });
				} else {
					this.textBox2.AppendText(strLine.Replace("\n", "\r\n") + "\r\n");
				}
			}
		}

		private string GetArgumentValue(string strName)
		{
			foreach (Control parent in this.panel4.Controls) {
				if (parent.Name == "GroupboxTextbox") {
					foreach (Control c in parent.Controls) {
						if (c.Name == strName) {
							return c.Controls[0].Text;
						}
					}
				}
			}
			return "";
		}

		private object[] GetArguments(string strName, string strArgs)
		{
			object[] objResult = new object[0];
			if (strArgs != "") {
				try {
					string[] args = strArgs.Trim().Split(new char[] { ',' });
					object[] argobjects = new object[args.Length];
					for (int intI = 0; intI < argobjects.Length; intI++) {
						string[] argument = args[intI].Trim().Split(new char[] { ' ' });
						if (argument.Length == 2) {
							string[] arArgs;
							string strArgumentValue = GetArgumentValue(strName + "_" + intI);
							string strArgumentName = argument[1];
							string strArgumentType = argument[0];
							switch (strArgumentType) {
								case "System.String":
									argobjects[intI] = strArgumentValue;
									break;
								case "System.Int32":
									argobjects[intI] = int.Parse(strArgumentValue);
									break;
								case "LSLEditor.SecondLife+Float":
									argobjects[intI] = new SecondLife.Float(strArgumentValue);
									break;
								case "LSLEditor.SecondLife+integer":
									argobjects[intI] = new SecondLife.integer(int.Parse(strArgumentValue));
									break;
								case "LSLEditor.SecondLife+String":
									argobjects[intI] = new SecondLife.String(strArgumentValue);
									break;
								case "LSLEditor.SecondLife+key":
									argobjects[intI] = new SecondLife.key(strArgumentValue);
									break;
								case "LSLEditor.SecondLife+list":
									argobjects[intI] = new SecondLife.list(new string[] { strArgumentValue });
									break;
								case "LSLEditor.SecondLife+rotation":
									arArgs = strArgumentValue.Replace("<", "").Replace(">", "").Replace(" ", "").Split(new char[] { ',' });
									argobjects[intI] = new SecondLife.rotation(double.Parse(arArgs[0]), double.Parse(arArgs[1]), double.Parse(arArgs[2]), double.Parse(arArgs[3]));
									break;
								case "LSLEditor.SecondLife+vector":
									arArgs = strArgumentValue.Replace("<", "").Replace(">", "").Replace(" ", "").Split(new char[] { ',' });
									argobjects[intI] = new SecondLife.vector(double.Parse(arArgs[0]), double.Parse(arArgs[1]), double.Parse(arArgs[2]));
									break;
								default:
									MessageBox.Show("Compiler->GetArguments->[" + strArgumentType + "][" + strArgumentName + "]");
									argobjects[intI] = null;
									break;
							}
						} else {
							MessageBox.Show("Argument must be 'type name' [" + args[intI] + "]");
							break;
						}
					}
					objResult = argobjects;
				} catch { }
			}
			return objResult;
		}

		private void buttonEvent_Click(object sender, System.EventArgs e)
		{
			Button button = (Button)sender;
			string strName = button.Text;
			string strArgs = SecondLifeHost.GetArgumentsFromMethod(strName);
			object[] args = GetArguments(strName, strArgs);
			//SecondLifeHost.VerboseEvent(strName, args);
			SecondLifeHost.ExecuteSecondLife(strName, args);
		}

		private void Die()
		{
			if (this.SecondLifeHost != null) {
				this.SecondLifeHost.Die();
			}
		}

		// Die
		private void button1_Click(object sender, EventArgs e)
		{
			Die();
		}

		private void textBox2_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.KeyCode == Keys.A) {
				this.textBox2.SelectAll();
			}
		}

		private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.textBox2.Focus();
			this.textBox2.SelectAll();
		}

		private void copyToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.textBox2.Focus();
			this.textBox2.Copy();
		}

		private void clearToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.textBox2.Focus();
			this.textBox2.Clear();
		}

	}
}
