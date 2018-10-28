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
using System.Xml;
using System.Net;
using System.Threading;

namespace LSLEditor.Helpers
{
	class XmlRpcRequestEventArgs : EventArgs
	{
		public SecondLife.key channel;
		public SecondLife.key message_id;
		public SecondLife.String sData;
		public SecondLife.integer iData;
		public SecondLife.String sender;
	}

	class XMLRPC
	{
		private HttpListener listener;
		private Thread thread;
		private bool blnRunning;
		private WebRequest request;

		public SecondLife.key guid;
		public HttpListenerContext context;
		public delegate void RequestEventHandler(object sender, XmlRpcRequestEventArgs e);
		public event RequestEventHandler OnReply;
		public event RequestEventHandler OnRequest;

		public string Prefix;
		public XMLRPC()
		{
			this.guid = SecondLife.NULL_KEY;
		}

		public SecondLife.key OpenChannel(int intChannel)
		{
			if (!HttpListener.IsSupported)
				return this.guid;

			// Yes, it works
			this.guid = new SecondLife.key(Guid.NewGuid());


			// Create a listener.
			listener = new HttpListener();

			// Add the prefix.
			int intPort = 50888 + intChannel;
			Prefix = "http://localhost:" + intPort + "/";
			listener.Prefixes.Add(Prefix);

			listener.Start();

			blnRunning = true;
			thread = new Thread(new ThreadStart(Worker));
			thread.Name = "Worker";
			thread.Start();

			return this.guid;
		}

		private XmlRpcRequestEventArgs DecodeRequest(System.IO.Stream stream)
		{
			XmlRpcRequestEventArgs e = new XmlRpcRequestEventArgs();
			e.sender = "";
			e.message_id = SecondLife.NULL_KEY;
			/*
<?xml version="1.0"?>
 <methodCall>
     <methodName>llRemoteData</methodName>
     <params>
         <param>
             <value>
                 <struct>
                     <member>
                         <name>Channel</name>
                         <value><string>4a250e12-c02e-94fb-6d2f-13529cbaad63</string></value>
                     </member>
                     <member>
                         <name>IntValue</name>
                         <value><int>0</int></value>
                     </member>
                     <member>
                         <name>StringValue</name>
                         <value><string>test</string></value>
                     </member>
                 </struct>
             </value>
         </param>
     </params>
 </methodCall>
*/
			XmlDocument xml = new XmlDocument();
			xml.Load(stream);

			XmlNode methodCall = xml.SelectSingleNode("/methodCall");

			if (methodCall == null)
				return e;

			XmlNode methodName = methodCall.SelectSingleNode("./methodName");
			if (methodName == null)
				return e;

			if (methodName.InnerText != "llRemoteData")
				return e;

			foreach (XmlNode xmlMember in methodCall.SelectNodes("./params/param/value/struct/member"))
			{
				string strName = xmlMember.SelectSingleNode("./name").InnerText;
				string strValue = xmlMember.SelectSingleNode("./value").InnerText;
				switch (strName)
				{
					case "Channel":
						e.channel = new SecondLife.key(strValue);
						break;
					case "StringValue":
						e.sData = strValue;
						break;
					case "IntValue":
						int iData;
						int.TryParse(strValue, out iData);
						e.iData = iData;
						break;
					default:
						break;
				}
			}
			return e;
		}

		private void Worker()
		{
			XmlRpcRequestEventArgs e;
			while (blnRunning)
			{
				// Note: The GetContext method blocks while waiting for a request. 
				try
				{
					context = listener.GetContext();
					e = DecodeRequest(context.Request.InputStream);

					if (OnRequest != null)
						OnRequest(this, e);
				}
				catch (HttpListenerException)
				{
				}
				catch (ThreadAbortException)
				{
				}
				catch (Exception exception)
				{
					System.Windows.Forms.MessageBox.Show("RPC Error:" + exception.Message, "Oops...");
				}
			}
		}

		public void RemoteDataReply(SecondLife.key channel, SecondLife.key message_id, string sData, int iData)
		{
			// Obtain a response object.
			HttpListenerResponse response = context.Response;

			// Construct a response.
			string responseString = string.Format(@"<?xml version=""1.0""?>
 <methodResponse>
     <params>
         <param>
             <value>
                 <struct>
                     <member>
                         <name>Channel</name>
                         <value><string>{0}</string></value>
                     </member>
                     <member>
                         <name>StringValue</name>
                         <value><string>{1}</string></value>
                     </member>
                     <member>
                         <name>IntValue</name>
                         <value><int>{2}</int></value>
                     </member>
                 </struct>
             </value>
         </param>
     </params>
 </methodResponse>", channel.ToString(),sData,iData);
			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

			// Get a response stream and write the response to it.
			response.ContentLength64 = buffer.Length;
			response.ContentType = "text/xml";
			System.IO.Stream output = response.OutputStream;
			output.Write(buffer, 0, buffer.Length);

			// You must close the output stream.
			output.Close();
		}

		public void CloseChannel()
		{
			blnRunning = false;
			if(listener!=null)
				listener.Stop();
			if (thread != null)
			{
				thread.Abort();
				bool succes = thread.Join(1000);
			}
			thread = null;
			listener = null;			
		}

		public SecondLife.key SendRemoteData(SecondLife.key channel, string dest, int iData, string sData)
		{
			this.guid = new SecondLife.key(Guid.NewGuid());

			// Construct a request.
			string requestString = string.Format(@"<?xml version=""1.0""?>
 <methodCall>
     <methodName>llRemoteData</methodName>
     <params>
         <param>
             <value>
                 <struct>
                     <member>
                         <name>Channel</name>
                         <value><string>{0}</string></value>
                     </member>
                     <member>
                         <name>IntValue</name>
                         <value><int>{1}</int></value>
                     </member>
                     <member>
                         <name>StringValue</name>
                         <value><string>{2}</string></value>
                     </member>
                 </struct>
             </value>
         </param>
     </params>
 </methodCall>", channel.ToString(), iData, sData);

			request = WebRequest.Create(dest);

			byte[] buffer = System.Text.Encoding.UTF8.GetBytes(requestString);

			// Get a response stream and write the response to it.
			request.ContentLength = buffer.Length;
			request.ContentType = "text/xml";
			System.IO.Stream requestStream = request.GetRequestStream();
			requestStream.Write(buffer, 0, buffer.Length);

			// You must close the request stream.
			requestStream.Close();

			thread = new Thread(new ThreadStart(WaitOnResponse));
			thread.Name = "WaitOnResponse";
			thread.Start();

			return this.guid;
		}

		private void WaitOnResponse()
		{
			Thread.Sleep(100);
			WebResponse response = request.GetResponse();

			XmlRpcRequestEventArgs e = DecodeRequest(response.GetResponseStream());

			// yes!!
			e.message_id = this.guid;

			if (OnReply != null)
				OnReply(this, e);
		}
	}
}
