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
using System.Net;
using System.Reflection;
using System.ComponentModel;
using System.Windows.Forms;

using LSLEditor.Decompressor;

namespace LSLEditor
{
	public partial class UpdateApplicationForm : Form
	{
		private WebClient manifest;
		private WebClient client;

		private string strHashWeb;
		private string strDownloadUrl;

		private string strHelpHashWeb;
		private string strHelpUrl;
		private string strHelpReferer;

		private bool blnOnlyHelpFile;

		private struct versionInfo
		{
			public Version version;
			public string hash;
			public string uri;

			public versionInfo(string ver)
			{
				version = new Version(ver);
				hash = "";
				uri = "";
			}

			public versionInfo(string ver, string md5)
			{
				version = new Version(ver);
				hash = md5;
				uri = "";
			}

			public versionInfo(string ver, string md5, string URI)
			{
				version = new Version(ver);
				hash = md5;
				uri = URI;
			}
		}

		public UpdateApplicationForm()
		{
			InitializeComponent();
			this.strHashWeb = "";
			this.strHelpHashWeb = "";
			this.strDownloadUrl = null;
			this.strHelpUrl = null;
			this.strHelpReferer = null;
			this.buttonUpdate.Enabled = false;
			this.blnOnlyHelpFile = false;
		}

		public event EventHandler OnUpdateAvailable;

		private void StartDownloadinManifest()
		{
			Uri url;
			string strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
			if (Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).Contains("beta")) {
				url = new Uri(Properties.Settings.Default.UpdateManifest + "?beta-" + strVersion);
			} else {
				url = new Uri(Properties.Settings.Default.UpdateManifest + "?" + strVersion);
			}

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
			if (!blnForce) {
				if (Properties.Settings.Default.DeleteOldFiles) {
					DeleteOldFile();
				}

				DateTime dateTime = Properties.Settings.Default.CheckDate;
				if (Properties.Settings.Default.CheckEveryDay) {
					TimeSpan lastUpdate = DateTime.Now - dateTime;
					if (lastUpdate.TotalDays >= 1.0) {
						blnForce = true;
					}
				} else if (Properties.Settings.Default.CheckEveryWeek) {
					TimeSpan lastUpdate = DateTime.Now - dateTime;
					if (lastUpdate.TotalDays >= 7.0) {
						blnForce = true;
					}
				}
			}

			if (blnForce) {
				Properties.Settings.Default.CheckDate = DateTime.Now;
				Properties.Settings.Default.Save(); // save also all settings

				StartDownloadinManifest();
			}
		}

		void manifest_DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
		{
			if (e.Error != null)
				return;

			versionInfo bzip = new versionInfo();
			versionInfo gzip = new versionInfo();
			versionInfo wzip = new versionInfo();

			versionInfo web = new versionInfo();

			versionInfo current = new versionInfo(Assembly.GetExecutingAssembly().GetName().Version.ToString());
			current.hash = Decompressor.MD5Verify.ComputeHash(Assembly.GetExecutingAssembly().Location);
			current.uri = "";

			string strHelpHashMe = "";
			string strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Settings.Default.HelpOfflineFile);
			if (File.Exists(strHelpFile)) {
				strHelpHashMe = Decompressor.MD5Verify.ComputeHash(strHelpFile);
			} else {
				// help file does not exist
				if (Properties.Settings.Default.HelpOffline || blnOnlyHelpFile) {
					strHelpHashMe = "*"; // force new update
				} else {
					strHelpHashMe = ""; // no update
					this.labelHelpFile.Visible = false;
					this.labelHelpversionString.Visible = false;
				}
			}

			StringReader sr = new StringReader(e.Result);
			for (int intI = 0; intI < 255; intI++) {
				string strLine = sr.ReadLine();
				if (strLine == null) {
					break;
				}

				int intSplit = strLine.IndexOf("=");
				if (intSplit < 0) {
					continue;
				}

				string strName = strLine.Substring(0, intSplit);
				string strValue = strLine.Substring(intSplit + 1);

				//All hashes are of the uncompressed file. However, different archives may contain different versions.
				switch (strName) {
					case "Version":
					case "BZipVersion":
						bzip = new versionInfo(strValue);
						break;
					case "Hash":
					case "BZipHash":
						bzip.hash = strValue;
						break;
					case "Url":
					case "BZipUrl":
						bzip.uri = strValue;
						break;
					case "GZipVersion":
						gzip = new versionInfo(strValue);
						break;
					case "GZipHash":
						gzip.hash = strValue;
						break;
					case "GZipUrl":
						gzip.uri = strValue;
						break;
					case "ZipVersion":
						wzip = new versionInfo(strValue);
						break;
					case "ZipHash":
						wzip.hash = strValue;
						break;
					case "ZipUrl":
						wzip.uri = strValue;
						break;
					case "HelpHash":
						strHelpHashWeb = strValue;
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

			web = bzip;
			/*
			if (!String.IsNullOrEmpty(gzip.uri) && (gzip.compare(web) == 1))
			{
				web = gzip;
			}


			if (!String.IsNullOrEmpty(wzip.uri) && (wzip.compare(web) == 1))
			{
				web = wzip;
			}
			*/
			strHashWeb = web.hash;

			this.labelOurVersionString.Text = current.version.ToString();
			this.labelLatestVersionString.Text = web.version.ToString();

			if (String.IsNullOrEmpty(web.uri) || (web.version.CompareTo(current.version) != 1)) {
				return;
			}

			if (strHelpHashMe == "") {
				strHelpHashMe = strHelpHashWeb;
			}

			if (strHelpHashMe == strHelpHashWeb) {
				this.labelHelpversionString.Text = "Up to date";
				this.strHelpUrl = null;
			} else {
				this.labelHelpversionString.Text = "Out of date";
			}

			if (current.hash == web.hash) {
				this.strDownloadUrl = null;
			} else {
				this.strDownloadUrl = web.uri;
			}

			if (this.blnOnlyHelpFile) {
				this.strDownloadUrl = null;
				this.labelLatestVersion.Visible = false;
				this.labelLatestVersionString.Visible = false;
			}

			if (this.strHelpUrl != null || this.strDownloadUrl != null) {
				this.buttonUpdate.Enabled = true;

				if (OnUpdateAvailable != null) {
					OnUpdateAvailable(this, null);
				}
			}
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void buttonUpdate_Click(object sender, EventArgs e)
		{
			this.buttonUpdate.Enabled = false;
			Download();
		}

		private void Download()
		{
			if (strHelpUrl != null) {
				DownloadHelpFile(); // starts also DownloadProgram when finished
			} else {
				DownloadProgram();
			}
		}

		private void DownloadHelpFile()
		{
			if (strHelpUrl != null) {
				Uri url = new Uri(strHelpUrl);

				client = new WebClient();

				if (this.strHelpReferer != null) {
					client.Headers.Add("Referer", strHelpReferer);
				}

				client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadHelpFileCompleted);
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);

				string strCurrentFile = Assembly.GetExecutingAssembly().Location;
				string strDirectory = Path.GetDirectoryName(strCurrentFile);
				string strNewFile = Path.Combine(strDirectory, Properties.Settings.Default.HelpOfflineFile);

				if (File.Exists(strNewFile)) {
					File.Delete(strNewFile);
				}

				client.DownloadFileAsync(url, strNewFile);
			}
		}

		void client_DownloadHelpFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			try {
				if (e.Error != null) {
					throw e.Error;
				}

				string strCurrentFile = Assembly.GetExecutingAssembly().Location;
				string strDirectory = Path.GetDirectoryName(strCurrentFile);
				string strNewFile = Path.Combine(strDirectory, Properties.Settings.Default.HelpOfflineFile);

				string strComputedHash = Decompressor.MD5Verify.ComputeHash(strNewFile);
				if (strComputedHash != strHelpHashWeb) {
					this.buttonUpdate.Enabled = true;
					throw new Exception("MD5 Hash of HelpFile not correct, try downloading again!");
				}
				if (this.strDownloadUrl != null) {
					DownloadProgram();
				} else {
					this.Close();
				}
			} catch (Exception exception) {
				MessageBox.Show(exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void DownloadProgram()
		{
			if (strDownloadUrl != null) {
				Uri url = new Uri(strDownloadUrl);

				client = new WebClient();
				client.DownloadFileCompleted += new AsyncCompletedEventHandler(client_DownloadFileCompleted);
				client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(client_DownloadProgressChanged);

				string strCurrentFile = Assembly.GetExecutingAssembly().Location;
				string strDirectory = Path.GetDirectoryName(strCurrentFile);
				string strNewFileName = Path.GetFileName(strDownloadUrl);
				string strNewFile = Path.Combine(strDirectory, strNewFileName);

				if (File.Exists(strNewFile)) {
					File.Delete(strNewFile);
				}

				client.DownloadFileAsync(url, strNewFile, strNewFileName);
			}
		}

		void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
		{
			this.progressBar1.Value = e.ProgressPercentage;
		}

		void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
		{
			try {
				if (e.Error != null) {
					throw e.Error;
				}

				string strNewFileName = e.UserState.ToString();

				string strCurrentFile = Assembly.GetExecutingAssembly().Location;
				string strDirectory = Path.GetDirectoryName(strCurrentFile);
				string strZipFile = Path.Combine(strDirectory, strNewFileName);
				string strNewFile = Path.Combine(strDirectory, "LSLEditor.exe.new");

				string strOldFile = Path.Combine(strDirectory, "_LSLEditor.exe");

				string strExtension = Path.GetExtension(strNewFileName);
				switch (strExtension) {
					case ".bz2":
						Decompressor.BZip2.Decompress(File.OpenRead(strZipFile), File.Create(strNewFile));
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
				if (strComputedHash == strHashWeb) {
					if (File.Exists(strOldFile))
						File.Delete(strOldFile);

					File.Move(strCurrentFile, strOldFile);
					File.Move(strNewFile, strCurrentFile);

					if (File.Exists(strZipFile)) {
						File.Delete(strZipFile);
					}

					// save all there is pending (if any)
					Properties.Settings.Default.Save();

					System.Diagnostics.Process.Start(strCurrentFile);

					Environment.Exit(0);
				} else {
					this.buttonUpdate.Enabled = true;
					throw new Exception("MD5 Hash not correct, try downloading again!");
				}
			} catch (Exception exception) {
				MessageBox.Show(exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void UpdateApplicationForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (client != null) {
				if (client.IsBusy) {
					client.CancelAsync();
				}
				client.Dispose();
			}
			client = null;
			if (manifest != null) {
				if (manifest.IsBusy) {
					manifest.CancelAsync();
				}
				manifest.Dispose();
			}
			manifest = null;
		}

		private void DeleteOldFile()
		{
			string strCurrentFile = Assembly.GetExecutingAssembly().Location;
			string strDirectory = Path.GetDirectoryName(strCurrentFile);
			string strOldFile = Path.Combine(strDirectory, "_LSLEditor.exe");
			if (File.Exists(strOldFile)) {
				File.Delete(strOldFile);
			}
		}

	}
}
