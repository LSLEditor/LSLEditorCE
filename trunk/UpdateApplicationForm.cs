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
using System.IO;
using System.Net;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

namespace LSLEditor
{
	public partial class UpdateApplicationForm : Form
	{
		private WebClient manifest;
		private WebClient client;

		private string strHashNew;
		private string strDownloadUrl;

		private string strHelpHashNew;
		private string strHelpUrl;
		private string strHelpReferer;

		private bool blnOnlyHelpFile;

		public UpdateApplicationForm()
		{
			InitializeComponent();
			this.strHashNew = "";
			this.strHelpHashNew = "";
			this.strDownloadUrl = null;
			this.strHelpUrl = null;
			this.strHelpReferer = null;
			this.button1.Enabled = false;
			this.blnOnlyHelpFile = false;
		}

		public event EventHandler OnUpdateAvailable;

		private void StartDownloadinManifest()
		{
			Uri url;
			string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			if (Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).Contains("beta"))
				url = new Uri(Properties.Settings.Default.UpdateManifest + "?beta-" + strVersion);
			else
				url = new Uri(Properties.Settings.Default.UpdateManifest + "?" + strVersion);

			manifest = new WebClient();
			manifest.DownloadStringCompleted += new DownloadStringCompletedEventHandler(manifest_DownloadCompleted);
			manifest.DownloadStringAsync(url);
		}

		public void CheckForHelpFile()
		{
			this.blnOnlyHelpFile = true;
			StartDownloadinManifest();
		}

		public void CheckForUpdate(bool blnForce)
		{
			if (!blnForce)
			{
				if (Properties.Settings.Default.DeleteOldFiles)
					DeleteOldFile();

				DateTime dateTime = Properties.Settings.Default.CheckDate;
				if (Properties.Settings.Default.CheckEveryDay)
				{
					TimeSpan lastUpdate = DateTime.Now - dateTime;
					if (lastUpdate.TotalDays >= 1.0)
						blnForce = true;
				}
				else if (Properties.Settings.Default.CheckEveryWeek)
				{
					TimeSpan lastUpdate = DateTime.Now - dateTime;
					if(lastUpdate.TotalDays >= 7.0)
						blnForce = true;
				}
			}

			if (blnForce)
			{
				Properties.Settings.Default.CheckDate = DateTime.Now;
				Properties.Settings.Default.Save(); // save also al settings

				StartDownloadinManifest();
			}
		}

		void manifest_DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null)
				return;

			string strHashOld = Decompressor.MD5Verify.ComputeHash(Assembly.GetExecutingAssembly().Location);
			string strVersionOld = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			string strVersionNew = strVersionOld;

			string strHelpHashOld = "";
			string strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Settings.Default.HelpOfflineFile);
			if (File.Exists(strHelpFile))
			{
				strHelpHashOld = Decompressor.MD5Verify.ComputeHash(strHelpFile);
			}
			else
			{
				// help file does not exist
				if (Properties.Settings.Default.HelpOffline || blnOnlyHelpFile)
				{
					strHelpHashOld = "*"; // force new update
				}
				else
				{
					strHelpHashOld = ""; // no update
					this.label5.Visible = false;
					this.label6.Visible = false;
				}
			}

			strHashNew = strHashOld;
			StringReader sr = new StringReader(e.Result);
			for (int intI = 0; intI < 255; intI++)
			{
				string strLine = sr.ReadLine();
				if (strLine == null)
					break;
				
                int intSplit = strLine.IndexOf("=");
				if (intSplit < 0)
					continue;
				
                string strName = strLine.Substring(0, intSplit);
				string strValue = strLine.Substring(intSplit + 1);

				switch (strName)
				{
					case "Version":
						strVersionNew = strValue;
						break;
					case "Hash":
						strHashNew = strValue;
						break;
					case "Url":
						strDownloadUrl = strValue;
						break;
                    case "GZipVersion":
						strVersionNew = strValue;
						break;
					case "GZipHash":
						strHashNew = strValue;
						break;
					case "GZipUrl":
						strDownloadUrl = strValue;
						break;
					case "ZipVersion":
						strVersionNew = strValue;
						break;
					case "ZipHash":
						strHashNew = strValue;
						break;
					case "ZipUrl":
						strDownloadUrl = strValue;
						break;
                    case "HelpHash":
						strHelpHashNew = strValue;
						break;
					case "HelpUrl2":
						strHelpUrl = strValue;
						break;
					case "HelpReferer":
						strHelpReferer = strValue;
						break;
					default:
						break;
				}
			}

			this.label3.Text = strVersionOld;
			this.label4.Text = strVersionNew;

			if (strHelpHashOld == "")
				strHelpHashOld = strHelpHashNew;

			if (strHelpHashOld == strHelpHashNew)
			{
				this.label6.Text = "Up to date";
				this.strHelpUrl = null;
			}
			else
			{
				this.label6.Text = "Out of date";
			}

			if (strHashOld == strHashNew)
			{
				this.strDownloadUrl = null;
			}

			if (this.blnOnlyHelpFile)
			{
				this.strDownloadUrl = null;
				this.label2.Visible = false;
				this.label4.Visible = false;
			}

			if (this.strHelpUrl != null || this.strDownloadUrl != null)
			{
				this.button1.Enabled = true;

				if (OnUpdateAvailable != null)
					OnUpdateAvailable(this, null);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.button1.Enabled = false;
			Download();
		}

		private void Download()
		{
			if (strHelpUrl != null)
				DownloadHelpFile(); // starts also DownloadProgram when finished
			else
				DownloadProgram();
		}

		private void DownloadHelpFile()
		{
			if (strHelpUrl == null)
				return;

			Uri url = new Uri(strHelpUrl);

			client = new WebClient();

			if(this.strHelpReferer != null)
				client.Headers.Add("Referer", strHelpReferer);

			client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadHelpFileCompleted);
			client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);

			string strCurrentFile = Assembly.GetExecutingAssembly().Location;
			string strDirectory = Path.GetDirectoryName(strCurrentFile);
			string strNewFile = Path.Combine(strDirectory, Properties.Settings.Default.HelpOfflineFile);

			if (File.Exists(strNewFile))
				File.Delete(strNewFile);

			client.DownloadFileAsync(url, strNewFile);
		}

		void client_DownloadHelpFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
					throw e.Error;

				string strCurrentFile = Assembly.GetExecutingAssembly().Location;
				string strDirectory = Path.GetDirectoryName(strCurrentFile);
				string strNewFile = Path.Combine(strDirectory, Properties.Settings.Default.HelpOfflineFile);

				string strComputedHash = Decompressor.MD5Verify.ComputeHash(strNewFile);
				if (strComputedHash != strHelpHashNew)
				{
					this.button1.Enabled = true;
					throw new Exception("MD5 Hash of HelpFile not correct, try downloading again!");
				}
				if (this.strDownloadUrl != null)
					DownloadProgram();
				else
					this.Close();
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DownloadProgram()
		{
			if (strDownloadUrl == null)
				return;

			Uri url = new Uri(strDownloadUrl);

			client = new WebClient();
			client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
			client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);

			string strCurrentFile = Assembly.GetExecutingAssembly().Location;
			string strDirectory = Path.GetDirectoryName(strCurrentFile);
			string strNewFileName = Path.GetFileName(strDownloadUrl);
			string strNewFile = Path.Combine(strDirectory, strNewFileName);

			if (File.Exists(strNewFile))
				File.Delete(strNewFile);

			client.DownloadFileAsync(url, strNewFile, strNewFileName);
		}

		void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
		}

		void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			try
			{
				if (e.Error != null)
					throw e.Error;

				string strNewFileName = e.UserState.ToString();

				string strCurrentFile = Assembly.GetExecutingAssembly().Location;
				string strDirectory = Path.GetDirectoryName(strCurrentFile);
				string strZipFile = Path.Combine(strDirectory, strNewFileName);
				string strNewFile = Path.Combine(strDirectory, "LSLEditor.exe.new");

				string strOldFile = Path.Combine(strDirectory, "_LSLEditor.exe");

				string strExtension = Path.GetExtension(strNewFileName);
				switch (strExtension)
				{
					case ".bz2":
						//BZip2Decompress.Decompressor.Decompress(File.OpenRead(strZipFile), File.Create(strNewFile));
						break;
					case ".gz":
					case ".gzip":
						Decompressor.Gzip.Decompress(File.OpenRead(strZipFile), File.Create(strNewFile));
						break;
					case ".zip":
						Decompressor.Zip.Decompress(File.OpenRead(strZipFile), File.Create(strNewFile));
						break;
					default:
						break;
				}
				string strComputedHash = Decompressor.MD5Verify.ComputeHash(strNewFile);
				if (strComputedHash == strHashNew)
				{
					if (File.Exists(strOldFile))
						File.Delete(strOldFile);

					File.Move(strCurrentFile, strOldFile);
					File.Move(strNewFile, strCurrentFile);

					if (File.Exists(strZipFile))
						File.Delete(strZipFile);

					// save all there is pending (if any)
					Properties.Settings.Default.Save();

					System.Diagnostics.Process.Start(strCurrentFile);

					Environment.Exit(0);
				}
				else
				{
					this.button1.Enabled = true;
					throw new Exception("MD5 Hash not correct, try downloading again!");
				}
			}
			catch (Exception exception)
			{
				MessageBox.Show(exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateApplicationForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (client != null)
			{
				if (client.IsBusy)
					client.CancelAsync();
				client.Dispose();
			}
			client = null;
			if (manifest != null)
			{
				if (manifest.IsBusy)
					manifest.CancelAsync();
				manifest.Dispose();
			}
			manifest = null;
		}

		private void DeleteOldFile()
		{
			string strCurrentFile = Assembly.GetExecutingAssembly().Location;
			string strDirectory = Path.GetDirectoryName(strCurrentFile);
			string strOldFile = Path.Combine(strDirectory, "_LSLEditor.exe");
			if (File.Exists(strOldFile))
				File.Delete(strOldFile);
		}

	}
}