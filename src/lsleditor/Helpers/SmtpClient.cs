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
using System.Text;
using System.Net.Sockets;
using System.Collections;

namespace LSLEditor
{
	public class MailAttachment
	{
		public string Filename;
		public MailAttachment(string strAttachmentFile)
		{
			this.Filename = strAttachmentFile;
		}
	}

	public class MailMessage
	{
		public string Body;
		public string From;
		public string To;
		public string Cc;
		public string Bcc;
		public string Subject;
		public Hashtable Headers;
		public ArrayList Attachments;

		public MailMessage()
		{
			this.Headers = new Hashtable();
			this.Attachments = new ArrayList();
		}
	}

	/// <summary>
	/// provides methods to send email via smtp direct to mail server
	/// http://www.ietf.org/rfc/rfc0821.txt
	/// </summary>
	public class SmtpClient
	{
		/// <summary>
		/// Get / Set the name of the SMTP mail server
		/// </summary>
		public string SmtpServer;
		private enum SMTPResponse: int
		{
			CONNECT_SUCCESS = 220,
			GENERIC_SUCCESS = 250,
			DATA_SUCCESS	= 354,
			QUIT_SUCCESS	= 221,

			AUTH_SUCCESS    = 334,
			AUTH_GRANTED    = 235
		}

		public string Send(MailMessage message)
		{
			string strResponse;
			byte[] data;
			IPHostEntry IPhst;
			try
			{
				IPhst = Dns.GetHostEntry(SmtpServer);
			}
			catch (Exception exception)
			{
				return "Email:Error on "+exception.Message;
			}
			IPEndPoint endPt = new IPEndPoint(IPhst.AddressList[0], 25);
			Socket s= new Socket(endPt.AddressFamily, SocketType.Stream,ProtocolType.Tcp);
			s.Connect(endPt);
    		
			if(!Check_Response(s, SMTPResponse.CONNECT_SUCCESS, out strResponse))
			{				
				s.Close();
				return "Email:Error on connection (" + strResponse + ")";
			}

			Senddata(s, string.Format("HELO {0}\r\n", Dns.GetHostName() ));
			if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, out strResponse))
			{
				s.Close();
				return "Email:Error on HELO (" + strResponse + ")";
			}

			// SMTP Authentication
			if (Properties.Settings.Default.SmtpUserid != "")
			{
				if (Properties.Settings.Default.SmtpAuth == "PLAIN")
				{
					Senddata(s, "AUTH PLAIN\r\n");
					if (!Check_Response(s, SMTPResponse.AUTH_SUCCESS, out strResponse))
					{
						s.Close();
						return "Email: Error on AUTH PLAIN (" + strResponse + ")";
					}
					data = Encoding.ASCII.GetBytes(string.Format("\0{0}\0{1}",
						Properties.Settings.Default.SmtpUserid,
						Properties.Settings.Default.SmtpPassword));
					Senddata(s, string.Format("{0}\r\n",Convert.ToBase64String(data)));
					if (!Check_Response(s, SMTPResponse.AUTH_GRANTED, out strResponse))
					{
						s.Close();
						return "Email: AUTH PLAIN not granted (" + strResponse + ")";
					}
				}
				if (Properties.Settings.Default.SmtpAuth == "LOGIN")
				{
					Senddata(s, "AUTH LOGIN\r\n");
					if (!Check_Response(s, SMTPResponse.AUTH_SUCCESS, out strResponse))
					{
						s.Close();
						return "Email: Error on AUTH LOGIN (" + strResponse + ")";
					}
					data = Encoding.ASCII.GetBytes(Properties.Settings.Default.SmtpUserid);
					Senddata(s, string.Format("{0}\r\n", Convert.ToBase64String(data)));
					if (!Check_Response(s, SMTPResponse.AUTH_SUCCESS, out strResponse))
					{
						s.Close();
						return "Email: AUTH LOGIN userid error (" + strResponse + ")";
					}
					data = Encoding.ASCII.GetBytes(Properties.Settings.Default.SmtpPassword);
					Senddata(s, string.Format("{0}\r\n", Convert.ToBase64String(data)));
					if (!Check_Response(s, SMTPResponse.AUTH_GRANTED, out strResponse))
					{
						s.Close();
						return "Email: AUTH LOGIN not granted (" + strResponse + ")";
					}
				}
				if (Properties.Settings.Default.SmtpAuth == "CRAM-MD5")
				{
					s.Close();
					return "Email: LSLEditor Not Implemented CRAM-MD5";

				}
				if (Properties.Settings.Default.SmtpAuth == "DIGEST-MD5")
				{
					s.Close();
					return "Email: LSLEditor Not Implemented DIGEST-MD5";
				}
				if (Properties.Settings.Default.SmtpAuth == "EXTERNAL")
				{
					s.Close();
					return "Email: LSLEditor Not Implemented EXTERNAL";
				}
				if (Properties.Settings.Default.SmtpAuth == "ANONYMOUS")
				{
					s.Close();
					return "Email: LSLEditor Not Implemented ANONYMOUS";
				}
			}

			Senddata(s, string.Format("MAIL From: {0}\r\n", message.From ));
			if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, out strResponse))
			{
				s.Close();
				return "Email: Error on MAIL From (" + strResponse + ")";
			}
  
			string _To = message.To;
			string[] Tos= _To.Split(new char[] {';'});
			foreach (string To in Tos)
			{
				Senddata(s, string.Format("RCPT TO: {0}\r\n", To));
				if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, out strResponse))
				{
					s.Close();
					return "Email: Error on RCPT TO (" + strResponse + ")";
				}
			}
    			
			if(message.Cc!=null)
			{
				Tos= message.Cc.Split(new char[] {';'});
				foreach (string To in Tos)
				{
					Senddata(s, string.Format("RCPT TO: {0}\r\n", To));
					if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, out strResponse))
					{					
						s.Close();
						return "Email: Error on (CC) RCPT TO (" + strResponse + ")";
					}
				}
			}
    			
			StringBuilder Header=new StringBuilder();
			Header.Append("From: " + message.From + "\r\n");
			Tos= message.To.Split(new char[] {';'});
			Header.Append("To: ");
			for( int i=0; i< Tos.Length; i++)
			{
				Header.Append( i > 0 ? "," : "" );
				Header.Append(Tos[i]);
			}
			Header.Append("\r\n");
			if(message.Cc!=null)
			{
				Tos= message.Cc.Split(new char[] {';'});
				Header.Append("Cc: ");
				for( int i=0; i< Tos.Length; i++)
				{
					Header.Append( i > 0 ? "," : "" );
					Header.Append(Tos[i]);
				}
				Header.Append("\r\n");
			}
			Header.Append("Date: " + DateTime.Now.ToString("R" ) + "\r\n");
			Header.Append("Subject: " + message.Subject+ "\r\n");
			Header.Append("X-Mailer: SMTPClient v2.36 (LSL-Editor)\r\n" );
			// escape . on newline

			string MsgBody = message.Body.Replace("\n.","\n..");
			if(!MsgBody.EndsWith("\r\n"))
				MsgBody+="\r\n";
			if(message.Attachments.Count>0)
			{
				Header.Append( "MIME-Version: 1.0\r\n" );
				Header.Append( "Content-Type: multipart/mixed; boundary=unique-boundary-1\r\n" );
				Header.Append("\r\n");
				Header.Append( "This is a multi-part message in MIME format.\r\n" );
				StringBuilder sb = new StringBuilder();
				sb.Append("--unique-boundary-1\r\n");
				sb.Append("Content-Type: text/plain\r\n");
				sb.Append("Content-Transfer-Encoding: 7Bit\r\n");
				sb.Append("\r\n");
				sb.Append(MsgBody + "\r\n");
				sb.Append("\r\n");
    				
				foreach(object o in message.Attachments)
				{
					MailAttachment a = o as MailAttachment;
					byte[] binaryData;
					if(a!=null)
					{
						FileInfo f = new FileInfo(a.Filename);
						sb.Append("--unique-boundary-1\r\n");
						sb.Append("Content-Type: application/octet-stream; file=" + f.Name + "\r\n");
						sb.Append("Content-Transfer-Encoding: base64\r\n");
						sb.Append("Content-Disposition: attachment; filename=" + f.Name + "\r\n");
						sb.Append("\r\n");
						FileStream fs = f.OpenRead();
						binaryData = new Byte[fs.Length];
						long bytesRead = fs.Read(binaryData, 0, (int)fs.Length);
						fs.Close();
						string base64String = System.Convert.ToBase64String(binaryData, 0,binaryData.Length);
    						
						for(int i=0; i< base64String.Length ; )
						{
							int nextchunk=100;
							if(base64String.Length - (i + nextchunk ) <0)
								nextchunk = base64String.Length -i;
							sb.Append(base64String.Substring(i, nextchunk));
							sb.Append("\r\n");
							i+=nextchunk;
    						
						}
						sb.Append("\r\n");
					}
				}
				MsgBody=sb.ToString();
			}
    			
			Senddata(s, ("DATA\r\n"));
			if (!Check_Response(s, SMTPResponse.DATA_SUCCESS, out strResponse))
			{
				s.Close();
				return "Email:Error on DATA (" + strResponse + ")";
			}
			Header.Append( "\r\n" );
			Header.Append( MsgBody);
			Header.Append("\r\n.\r\n");
			Senddata(s, Header.ToString());
			if (!Check_Response(s, SMTPResponse.GENERIC_SUCCESS, out strResponse))
			{
				s.Close();
				return "Email:Error on message body (" + strResponse + ")";
			}			
    			
			Senddata(s, "QUIT\r\n");
			if (!Check_Response(s, SMTPResponse.QUIT_SUCCESS, out strResponse))
			{
				s.Close();
				return "Email:Error on QUIT (" + strResponse + ")";
			}
			s.Close();    			
			return "Email: Succes :-)";
		}
		private void Senddata(Socket s, string msg)
		{	
			byte[] _msg = Encoding.ASCII.GetBytes(msg);
			s.Send(_msg , 0, _msg .Length, SocketFlags.None);
		}
		private bool Check_Response(Socket s, SMTPResponse response_expected, out string sResponse)
		{
			byte[] bytes = new byte[1024];
			while (s.Available==0)
			{
				System.Threading.Thread.Sleep(100);
			}

			int intCount = s.Receive(bytes, 0, s.Available, SocketFlags.None);
			sResponse = Encoding.ASCII.GetString(bytes,0,intCount);
			int response = Convert.ToInt32(sResponse.Substring(0,3));
			if(response != (int)response_expected)
				return false;
			return true;
		}
	}
}

