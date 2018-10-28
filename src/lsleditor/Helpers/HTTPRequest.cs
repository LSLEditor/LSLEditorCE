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
using System.IO.Compression;
using System.Net;
using System.Text;

namespace LSLEditor
{
    internal static class HTTPRequest
    {
        private class UserState
        {
            public SecondLife secondlife;
            public SecondLife.key httpkey;

            public UserState(SecondLife.key httpkey, SecondLife secondlife)
            {
                this.secondlife = secondlife;
                this.httpkey = httpkey;
            }
        }

        public static void Request(WebProxy proxy, SecondLife secondlife, string strUrl, SecondLife.list parameters, string postData, SecondLife.key key)
        {
            var strMethod = "GET";
            var strContentType = "text/plain; charset=utf-8";

            for (var intI = 0; intI < parameters.Count; intI += 2)
            {
                if (!int.TryParse(parameters[intI].ToString(), out var intKey))
                {
                    continue;
                }

                switch (intKey)
                {
                    case 0:
                        // get, post, put, delete
                        strMethod = parameters[intI + 1].ToString().ToUpper();
                        break;
                    case 1:
                        strContentType = parameters[intI + 1].ToString();
                        break;
                    case 2:
                        // HTTP_BODY_MAXLENGTH
                        break;
                    case 3:
                        // HTTP_VERIFY_CERT
                        break;
                    default:
                        break;
                }
            }

            var wc = new WebClient();

            wc.Headers.Add("Content-Type", strContentType);
            wc.Headers.Add("Accept", "text/*");
            wc.Headers.Add("Accept-Charset", "utf-8; q=1.0, *; q=0.5");
            wc.Headers.Add("Accept-Encoding", "deflate, gzip");
            wc.Headers.Add("User-Agent", "Second Life LSL/1.19.0(12345) (http://secondlife.com)");

            var point = Properties.Settings.Default.RegionCorner;
            var RegionCorner = new SecondLife.vector(point.X, point.Y, 0);

            var pos = secondlife.GetLocalPos;

            wc.Headers.Add("X-SecondLife-Shard", Properties.Settings.Default.XSecondLifeShard);
            wc.Headers.Add("X-SecondLife-Object-Name", secondlife.host.GetObjectName());
            wc.Headers.Add("X-SecondLife-Object-Key", secondlife.host.GetKey().ToString());
            wc.Headers.Add("X-SecondLife-Region", string.Format("{0} ({1}, {2})", Properties.Settings.Default.RegionName, (int)RegionCorner.x, (int)RegionCorner.y));
            wc.Headers.Add("X-SecondLife-Local-Position", string.Format("({0}, {1}, {2})", pos.x, pos.y, pos.z));
            wc.Headers.Add("X-SecondLife-Local-Rotation", "(0.000000, 0.000000, 0.000000, 1.000000)");
            wc.Headers.Add("X-SecondLife-Local-Velocity", "(0.000000, 0.000000, 0.000000)");
            wc.Headers.Add("X-SecondLife-Owner-Name", Properties.Settings.Default.AvatarName);
            wc.Headers.Add("X-SecondLife-Owner-Key", Properties.Settings.Default.AvatarKey);
            wc.Headers.Add("X-Forwarded-For", "127.0.0.1");

            if (proxy != null)
            {
                wc.Proxy = proxy;
            }

            var uri = new Uri(strUrl);

            // Basic Authentication scheme, added 28 mrt 2008
            if (uri.UserInfo != "")
            {
                var UserInfo = uri.UserInfo.Split(':');
                if (UserInfo.Length == 2)
                {
                    wc.Credentials = new CredentialCache
                    {
                        {
                            uri,
                            "Basic",
                            new NetworkCredential(UserInfo[0], UserInfo[1])
                        }
                    };
                }
            }

            var userState = new UserState(key, secondlife);

            if (strMethod == "POST" || strMethod == "PUT")
            {
                wc.UploadDataCompleted += wc_UploadDataCompleted;
                wc.UploadDataAsync(uri, strMethod, Encoding.UTF8.GetBytes(postData), userState);
            }
            else
            {
                wc.DownloadDataCompleted += wc_DownloadDataCompleted;
                wc.DownloadDataAsync(uri, userState);
            }
        }

        private static void wc_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            var intStatusCode = 200;
            var userState = (UserState)e.UserState;
            var strResult = "";

            if (e.Error != null)
            {
                var webException = e.Error as WebException;
                if (webException.Response is HttpWebResponse webResponse)
                {
                    intStatusCode = (int)webResponse.StatusCode;
                    var sr = new StreamReader(webResponse.GetResponseStream());
                    strResult = sr.ReadToEnd();
                }
                else
                {
                    intStatusCode = 0;
                    strResult = webException.Message;
                }
            }
            else
            {
                if (e.Result != null)
                {
                    strResult = Encoding.UTF8.GetString(e.Result);
                }
            }
            userState.secondlife.host.ExecuteSecondLife("http_response", userState.httpkey, (SecondLife.integer)intStatusCode, new SecondLife.list(), (SecondLife.String)strResult);
        }

        private static void wc_UploadDataCompleted(object sender, UploadDataCompletedEventArgs e)
        {
            var intStatusCode = 200;
            var userState = (UserState)e.UserState;
            var strResult = "";

            if (e.Error != null)
            {
                var webException = e.Error as WebException;
                var webResponse = webException.Response as HttpWebResponse;
                intStatusCode = (int)webResponse.StatusCode;
                var sr = new StreamReader(webResponse.GetResponseStream());
                strResult = sr.ReadToEnd();
            }
            else if (e.Result != null)
            {
                var strEncoding = ((WebClient)sender).ResponseHeaders["Content-Encoding"];
                if (strEncoding == "gzip")
                {
                    var tempE = new GZipStream(new MemoryStream(e.Result), CompressionMode.Decompress);

                    var sr = new StreamReader(tempE);
                    strResult = sr.ReadToEnd();
                }
                else
                {
                    strResult = Encoding.UTF8.GetString(e.Result);
                }
            }
            userState.secondlife.host.ExecuteSecondLife("http_response", userState.httpkey, (SecondLife.integer)intStatusCode, new SecondLife.list(), (SecondLife.String)strResult);
        }
    }
}
