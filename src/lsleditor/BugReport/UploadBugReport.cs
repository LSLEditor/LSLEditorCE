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
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace LSLEditor.BugReport
{
	internal class UploadBugReport
	{
		public struct FileToUpload
		{
			public string FileName;
			public string Body;
			public string Path;

			public FileToUpload(string FileName, string Body)
			{
				this.FileName = FileName;
				this.Body = Body;
				this.Path = null;
			}
		}

		public class UploadCompleteEventArgs : EventArgs
		{
			public int TotalBytes;

			public UploadCompleteEventArgs(int intTotal)
			{
				this.TotalBytes = intTotal;
			}
		}

		public bool blnRunning;

		public delegate void UploadCompleteHandler(object sender, UploadCompleteEventArgs e);
		public event UploadCompleteHandler OnComplete;

		private Thread thread;
		private ProgressBar progressbar;
		private List<FileToUpload> list;

		public void UploadAsync(List<FileToUpload> list, ProgressBar progressbar)
		{
			this.list = list;
			this.progressbar = progressbar;

			this.blnRunning = true;

			this.thread = new Thread(new ThreadStart(this.Worker)) {
				IsBackground = true,
				Name = "Worker"
			};
			this.thread.Start();
		}

		public void Stop()
		{
			this.blnRunning = false;
			if (this.thread != null) {
				this.thread.Join(1000);
				this.thread = null;
			}
		}

		private void Worker()
		{
			var intTotal = 0;
			try {
				var webservice = new org.lsleditor.www.Service1();

				var Handle = webservice.Open();
				if (Handle == null) {
					MessageBox.Show("Can't get an upload handle", "Oops...");
					this.blnRunning = false;
					return;
				}

				if (Properties.Settings.Default.Bugreports == null) {
					Properties.Settings.Default.Bugreports = new StringCollection();
				}

				Properties.Settings.Default.Bugreports.Add(Handle);

				// Properties.Settings.Default.Save();

				var intNumber = 0;
				foreach (var file in this.list) {
					var strFileName = string.Format("{0}-{1}", intNumber, file.FileName);
					if (file.Path == null) {
						intTotal += this.Upload(Handle, strFileName, Encoding.ASCII.GetBytes(file.Body));
					} else {
						intTotal += this.Upload(Handle, strFileName, file.Path);
					}
					intNumber++;
				}
			} catch {
				intTotal = -1;
			}
			if (this.blnRunning) {
				OnComplete?.Invoke(this, new UploadCompleteEventArgs(intTotal));
			}

			this.blnRunning = false;
		}

		private delegate void SetValueDelegate(int intValue);

		private void SetMaximum(int intMaximum)
		{
			if (this.progressbar.InvokeRequired) {
				this.progressbar.Invoke(new SetValueDelegate(this.SetMaximum), new object[] { intMaximum });
			} else {
				this.progressbar.Maximum = intMaximum;
			}
		}

		private void SetValue(int intValue)
		{
			if (this.progressbar.InvokeRequired) {
				this.progressbar.Invoke(new SetValueDelegate(this.SetValue), new object[] { intValue });
			} else {
				this.progressbar.Value = intValue;
			}
		}

		private int Upload(string Handle, string FileName, byte[] buffer)
		{
			var webservice = new org.lsleditor.www.Service1();
			if (Handle == null) {
				return 0;
			}

			this.SetMaximum(buffer.Length);
			var intOffset = 0;
			var intTotal = 0;
			var smallbuffer = new byte[1024];
			while (this.blnRunning) {
				var intLength = Math.Min(smallbuffer.Length, buffer.Length - intOffset);
				if (intLength <= 0) {
					break;
				}

				Array.Copy(buffer, intOffset, smallbuffer, 0, intLength);
				intOffset += intLength;
				var strError = webservice.Write(Handle, FileName, smallbuffer, intLength);
				if (strError != null) {
					MessageBox.Show("Error:" + strError, "Oops...", MessageBoxButtons.OK, MessageBoxIcon.Error);
					break;
				}
				intTotal += intLength;
				this.SetValue(intTotal);
			}
			return intTotal;
		}

		private int Upload(string Handle, string FileName, string strPath)
		{
			var fs = new FileStream(strPath, FileMode.Open, FileAccess.Read);
			var buffer = new byte[fs.Length];
			fs.Read(buffer, 0, (int)fs.Length);
			fs.Close();
			return this.Upload(Handle, FileName, buffer);
		}
	}
}
