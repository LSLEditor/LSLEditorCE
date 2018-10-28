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
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

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
                this.version = new Version(ver);
                this.hash = "";
                this.uri = "";
            }

            public versionInfo(string ver, string md5)
            {
                this.version = new Version(ver);
                this.hash = md5;
                this.uri = "";
            }

            public versionInfo(string ver, string md5, string URI)
            {
                this.version = new Version(ver);
                this.hash = md5;
                this.uri = URI;
            }
        }

        public UpdateApplicationForm()
        {
            this.InitializeComponent();
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
            var strVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            var url = Path.GetFileNameWithoutExtension(Assembly.GetExecutingAssembly().Location).Contains("beta")
                ? new Uri(Properties.Settings.Default.UpdateManifest + "?beta-" + strVersion)
                : new Uri(Properties.Settings.Default.UpdateManifest + "?" + strVersion);
            this.manifest = new WebClient();
            this.manifest.DownloadStringCompleted += this.manifest_DownloadCompleted;
            this.manifest.DownloadStringAsync(url);
        }

        public void CheckForHelpFile()
        {
            this.blnOnlyHelpFile = true;
            this.StartDownloadinManifest();
        }

        public void CheckForUpdate(bool blnForce)
        {
            if (!blnForce)
            {
                if (Properties.Settings.Default.DeleteOldFiles)
                {
                    this.DeleteOldFile();
                }

                var dateTime = Properties.Settings.Default.CheckDate;
                if (Properties.Settings.Default.CheckEveryDay)
                {
                    var lastUpdate = DateTime.Now - dateTime;
                    if (lastUpdate.TotalDays >= 1.0)
                    {
                        blnForce = true;
                    }
                }
                else if (Properties.Settings.Default.CheckEveryWeek)
                {
                    var lastUpdate = DateTime.Now - dateTime;
                    if (lastUpdate.TotalDays >= 7.0)
                    {
                        blnForce = true;
                    }
                }
            }

            if (blnForce)
            {
                Properties.Settings.Default.CheckDate = DateTime.Now;
                Properties.Settings.Default.Save(); // save also all settings

                this.StartDownloadinManifest();
            }
        }

        private void manifest_DownloadCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                return;
            }

            var bzip = new versionInfo();
            var gzip = new versionInfo();
            var wzip = new versionInfo();

            var web = new versionInfo();

            var current = new versionInfo(Assembly.GetExecutingAssembly().GetName().Version.ToString())
            {
                hash = Decompressor.MD5Verify.ComputeHash(Assembly.GetExecutingAssembly().Location),
                uri = ""
            };

            var strHelpHashMe = "";
            var strHelpFile = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), Properties.Settings.Default.HelpOfflineFile);
            if (File.Exists(strHelpFile))
            {
                strHelpHashMe = Decompressor.MD5Verify.ComputeHash(strHelpFile);
            }
            else
            {
                // help file does not exist
                if (Properties.Settings.Default.HelpOffline || this.blnOnlyHelpFile)
                {
                    strHelpHashMe = "*"; // force new update
                }
                else
                {
                    strHelpHashMe = ""; // no update
                    this.labelHelpFile.Visible = false;
                    this.labelHelpversionString.Visible = false;
                }
            }

            var sr = new StringReader(e.Result);
            for (var intI = 0; intI < 255; intI++)
            {
                var strLine = sr.ReadLine();
                if (strLine == null)
                {
                    break;
                }

                var intSplit = strLine.IndexOf("=");
                if (intSplit < 0)
                {
                    continue;
                }

                var strName = strLine.Substring(0, intSplit);
                var strValue = strLine.Substring(intSplit + 1);

                //All hashes are of the uncompressed file. However, different archives may contain different versions.
                switch (strName)
                {
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
                        this.strHelpHashWeb = strValue;
                        break;
                    case "HelpUrl2":
                        this.strHelpUrl = strValue;
                        break;
                    case "HelpReferer":
                        this.strHelpReferer = strValue;
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
            this.strHashWeb = web.hash;

            this.labelOurVersionString.Text = current.version.ToString();
            this.labelLatestVersionString.Text = web.version.ToString();

            if (string.IsNullOrEmpty(web.uri) || (web.version.CompareTo(current.version) != 1))
            {
                return;
            }

            if (strHelpHashMe?.Length == 0)
            {
                strHelpHashMe = this.strHelpHashWeb;
            }

            if (strHelpHashMe == this.strHelpHashWeb)
            {
                this.labelHelpversionString.Text = "Up to date";
                this.strHelpUrl = null;
            }
            else
            {
                this.labelHelpversionString.Text = "Out of date";
            }

            this.strDownloadUrl = current.hash == web.hash ? null : web.uri;

            if (this.blnOnlyHelpFile)
            {
                this.strDownloadUrl = null;
                this.labelLatestVersion.Visible = false;
                this.labelLatestVersionString.Visible = false;
            }

            if (this.strHelpUrl != null || this.strDownloadUrl != null)
            {
                this.buttonUpdate.Enabled = true;

                OnUpdateAvailable?.Invoke(this, null);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            this.buttonUpdate.Enabled = false;
            this.Download();
        }

        private void Download()
        {
            if (this.strHelpUrl != null)
            {
                this.DownloadHelpFile(); // starts also DownloadProgram when finished
            }
            else
            {
                this.DownloadProgram();
            }
        }

        private void DownloadHelpFile()
        {
            if (this.strHelpUrl != null)
            {
                var url = new Uri(this.strHelpUrl);

                this.client = new WebClient();

                if (this.strHelpReferer != null)
                {
                    this.client.Headers.Add("Referer", this.strHelpReferer);
                }

                this.client.DownloadFileCompleted += this.client_DownloadHelpFileCompleted;
                this.client.DownloadProgressChanged += this.client_DownloadProgressChanged;

                var strCurrentFile = Assembly.GetExecutingAssembly().Location;
                var strDirectory = Path.GetDirectoryName(strCurrentFile);
                var strNewFile = Path.Combine(strDirectory, Properties.Settings.Default.HelpOfflineFile);

                if (File.Exists(strNewFile))
                {
                    File.Delete(strNewFile);
                }

                this.client.DownloadFileAsync(url, strNewFile);
            }
        }

        private void client_DownloadHelpFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw e.Error;
                }

                var strCurrentFile = Assembly.GetExecutingAssembly().Location;
                var strDirectory = Path.GetDirectoryName(strCurrentFile);
                var strNewFile = Path.Combine(strDirectory, Properties.Settings.Default.HelpOfflineFile);

                var strComputedHash = Decompressor.MD5Verify.ComputeHash(strNewFile);
                if (strComputedHash != this.strHelpHashWeb)
                {
                    this.buttonUpdate.Enabled = true;
                    throw new Exception("MD5 Hash of HelpFile not correct, try downloading again!");
                }
                if (this.strDownloadUrl != null)
                {
                    this.DownloadProgram();
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DownloadProgram()
        {
            if (this.strDownloadUrl != null)
            {
                var url = new Uri(this.strDownloadUrl);

                this.client = new WebClient();
                this.client.DownloadFileCompleted += this.client_DownloadFileCompleted;
                this.client.DownloadProgressChanged += this.client_DownloadProgressChanged;

                var strCurrentFile = Assembly.GetExecutingAssembly().Location;
                var strDirectory = Path.GetDirectoryName(strCurrentFile);
                var strNewFileName = Path.GetFileName(this.strDownloadUrl);
                var strNewFile = Path.Combine(strDirectory, strNewFileName);

                if (File.Exists(strNewFile))
                {
                    File.Delete(strNewFile);
                }

                this.client.DownloadFileAsync(url, strNewFile, strNewFileName);
            }
        }

        private void client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            try
            {
                if (e.Error != null)
                {
                    throw e.Error;
                }

                var strNewFileName = e.UserState.ToString();

                var strCurrentFile = Assembly.GetExecutingAssembly().Location;
                var strDirectory = Path.GetDirectoryName(strCurrentFile);
                var strZipFile = Path.Combine(strDirectory, strNewFileName);
                var strNewFile = Path.Combine(strDirectory, "LSLEditor.exe.new");

                var strOldFile = Path.Combine(strDirectory, "_LSLEditor.exe");

                switch (Path.GetExtension(strNewFileName))
                {
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
                var strComputedHash = Decompressor.MD5Verify.ComputeHash(strNewFile);
                if (strComputedHash == this.strHashWeb)
                {
                    if (File.Exists(strOldFile))
                    {
                        File.Delete(strOldFile);
                    }

                    File.Move(strCurrentFile, strOldFile);
                    File.Move(strNewFile, strCurrentFile);

                    if (File.Exists(strZipFile))
                    {
                        File.Delete(strZipFile);
                    }

                    // save all there is pending (if any)
                    Properties.Settings.Default.Save();

                    System.Diagnostics.Process.Start(strCurrentFile);

                    Environment.Exit(0);
                }
                else
                {
                    this.buttonUpdate.Enabled = true;
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
            if (this.client != null)
            {
                if (this.client.IsBusy)
                {
                    this.client.CancelAsync();
                }
                this.client.Dispose();
            }
            this.client = null;
            if (this.manifest != null)
            {
                if (this.manifest.IsBusy)
                {
                    this.manifest.CancelAsync();
                }
                this.manifest.Dispose();
            }
            this.manifest = null;
        }

        private void DeleteOldFile()
        {
            var strCurrentFile = Assembly.GetExecutingAssembly().Location;
            var strDirectory = Path.GetDirectoryName(strCurrentFile);
            var strOldFile = Path.Combine(strDirectory, "_LSLEditor.exe");
            if (File.Exists(strOldFile))
            {
                File.Delete(strOldFile);
            }
        }
    }
}
