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
using System.Net;
using System.IO;
using System.Text;
using System.Threading;

namespace LSLEditor
{
	public class RequestState
	{
		// This class stores the state of the request.
		public SecondLife secondlife;
		public SecondLife.key httpkey;
		public byte[] postData;
		const int BUFFER_SIZE = 1024;
		public StringBuilder requestData;
		public byte[] bufferRead;
		public WebRequest request;
		public WebResponse response;
		public Stream responseStream;
		public RequestState()
		{
			bufferRead = new byte[BUFFER_SIZE];
			requestData = new StringBuilder("");
			request = null;
			responseStream = null;
			postData = null;
		}
	}

	class WebRequestClass
	{
		public static ManualResetEvent allDone= new ManualResetEvent(false);
		const int BUFFER_SIZE = 1024;
		public WebRequestClass(WebProxy proxy, SecondLife secondlife, string strUrl, SecondLife.list parameters, string postData, SecondLife.key key)
		{
			try
			{
				// Create a new webrequest to the mentioned URL.   
				WebRequest myWebRequest = WebRequest.Create(strUrl);

				myWebRequest.Headers.Add("Cache-Control", "max-age=259200");
				//myWebRequest.Headers.Add("Connection", "keep-alive");
				myWebRequest.Headers.Add("Pragma", "no-cache");
				//myWebRequest.Headers.Add("Via", "1.1 sim3560.agni.lindenlab.com:3128 (squid/2.6.STABLE12)");
				//myWebRequest.Headers.Add("Content-Length", "3");
				//myWebRequest.Headers.Add("Content-Type", "text/plain;charset=utf-8");
				//myWebRequest.Headers.Add("Accept", "text/*");
				myWebRequest.Headers.Add("Accept-Charset", "utf-8;q=1.0, *;q=0.5");
				myWebRequest.Headers.Add("Accept-Encoding", "deflate, gzip");
				//myWebRequest.Headers.Add("Host", "www.lsleditor.org");
				//myWebRequest.Headers.Add("User-Agent", "LSLEditor 2.24 (http://www.lsleditor.org)");
				myWebRequest.Headers.Add("X-SecondLife-Shard", "Production");

				SecondLife.vector RegionCorner = secondlife.llGetRegionCorner();
				SecondLife.vector pos = secondlife.llGetPos();

				myWebRequest.Headers.Add("X-SecondLife-Object-Name", secondlife.host.GetObjectName());
				myWebRequest.Headers.Add("X-SecondLife-Object-Key", secondlife.host.GetKey().ToString());
				myWebRequest.Headers.Add("X-SecondLife-Region", Properties.Settings.Default.RegionName + " (" + (int)RegionCorner.x + ", " + (int)RegionCorner.y + ")");
				myWebRequest.Headers.Add("X-SecondLife-Local-Position", "("+pos.x+", "+pos.y+", "+pos.z+")");
				myWebRequest.Headers.Add("X-SecondLife-Local-Rotation", "(0.000000, 0.000000, 0.000000, 1.000000)");
				myWebRequest.Headers.Add("X-SecondLife-Local-Velocity", "(0.000000, 0.000000, 0.000000)");
				myWebRequest.Headers.Add("X-SecondLife-Owner-Name", Properties.Settings.Default.AvatarName);
				myWebRequest.Headers.Add("X-SecondLife-Owner-Key", Properties.Settings.Default.AvatarKey);
				myWebRequest.Headers.Add("X-Forwarded-For", "127.0.0.1");

				// Setting up paramters
				for (int intI = 0; intI < parameters.Count; intI += 2)
				{
					switch (int.Parse(parameters[intI].ToString()))
					{
						case 0:
							myWebRequest.Method = parameters[intI + 1].ToString();
							break;
						case 1:
							myWebRequest.ContentType = parameters[intI + 1].ToString();
							break;
						case 2:
							// HTTP_BODY_MAXLENGTH
							break;
						default:
							break;
					}
				}

				if (proxy != null)
					myWebRequest.Proxy = proxy;

				// Create a new instance of the RequestState.
				RequestState myRequestState = new RequestState();

				myRequestState.secondlife = secondlife;
				myRequestState.httpkey = key;
				myRequestState.postData = Encoding.UTF8.GetBytes(postData);

				// 19 sep 2007
				myWebRequest.ContentLength = myRequestState.postData.Length;

				// The 'WebRequest' object is associated to the 'RequestState' object.
				myRequestState.request = myWebRequest;

				// Start the Asynchronous call for response.
				IAsyncResult asyncResult;
				if (myWebRequest.Method == "POST" || myWebRequest.Method == "PUT")
					asyncResult = (IAsyncResult)myWebRequest.BeginGetRequestStream(new AsyncCallback(RespCallback), myRequestState);
				else
					asyncResult = (IAsyncResult)myWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
			}
			catch (WebException e)
			{
				secondlife.host.VerboseMessage(e.Message);
				secondlife.host.VerboseMessage(e.Status.ToString());
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception raised!");
				Console.WriteLine("Source : " + e.Source);
				Console.WriteLine("Message : " + e.Message);
				secondlife.host.VerboseMessage(e.Message);
			}
		}

		private static void RespCallback(IAsyncResult asynchronousResult)
		{  
			RequestState myRequestState=(RequestState) asynchronousResult.AsyncState;
			try
			{
				// Set the State of request to asynchronous.
				WebRequest request = myRequestState.request;

				if (request.Method == "POST" || request.Method == "PUT") // TODO check if this post works!!!!
				{
					// End the operation.
					Stream postStream = request.EndGetRequestStream(asynchronousResult);
					// Write to the request stream.
					postStream.Write(myRequestState.postData, 0, myRequestState.postData.Length);
					postStream.Close();

					myRequestState.response = (HttpWebResponse)request.GetResponse();
				}
				else
				{
					// End the Asynchronous response.
					myRequestState.response = request.EndGetResponse(asynchronousResult);
				}

				// Read the response into a 'Stream' object.
				Stream responseStream = myRequestState.response.GetResponseStream();
				myRequestState.responseStream = responseStream;
				// Begin the reading of the response
				IAsyncResult asynchronousResultRead = responseStream.BeginRead(myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
			}
			catch (WebException e)
			{
				myRequestState.secondlife.host.VerboseMessage(e.Message);
				myRequestState.secondlife.host.VerboseMessage(e.Status.ToString());
			}
			catch (Exception e)
			{
				Console.WriteLine("Exception raised!");
				Console.WriteLine("Source : " + e.Source);
				Console.WriteLine("Message : " + e.Message);
				myRequestState.secondlife.host.VerboseMessage(e.Message);
			}
		}

		private static void ReadCallBack(IAsyncResult asyncResult)
		{
			RequestState myRequestState = (RequestState)asyncResult.AsyncState;
			try
			{
				// Result state is set to AsyncState.
				Stream responseStream = myRequestState.responseStream;
				int read = responseStream.EndRead( asyncResult );
				// Read the contents of the HTML page and then print to the console.
				if (read > 0)
				{
					myRequestState.requestData.Append(Encoding.ASCII.GetString(myRequestState.bufferRead, 0, read));
					IAsyncResult asynchronousResult = responseStream.BeginRead( myRequestState.bufferRead, 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack), myRequestState);
				}
				else
				{
					responseStream.Close();
					if(myRequestState.requestData.Length>1)
					{
						myRequestState.secondlife.host.ExecuteSecondLife("http_response",myRequestState.httpkey, (SecondLife.integer)200, new SecondLife.list(), (SecondLife.String)myRequestState.requestData.ToString());
					}
				}
			}
			catch(WebException e)
			{
				myRequestState.secondlife.host.VerboseMessage(e.Message);
				myRequestState.secondlife.host.VerboseMessage(e.Status.ToString());
			} 
			catch(Exception e)
			{
				Console.WriteLine("Exception raised!");
				Console.WriteLine("Source : {0}" , e.Source);
				Console.WriteLine("Message : {0}" , e.Message);
				myRequestState.secondlife.host.VerboseMessage(e.Message);
			}

		}
	}
}

