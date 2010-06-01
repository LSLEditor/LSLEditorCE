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
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Windows.Forms;

namespace NativeHelper
{
	public class NativeWIN32
	{
		public const ushort KEYEVENTF_KEYUP = 0x0002;

		public struct KEYBDINPUT
		{
			public ushort wVk;
			public ushort wScan;
			public uint dwFlags;
			public long time;
			public uint dwExtraInfo;
		}

		[StructLayout(LayoutKind.Explicit, Size = 28)]
		public struct INPUT
		{
			[FieldOffset(0)]
			public uint type;
			[FieldOffset(4)]
			public KEYBDINPUT ki;
		}

		[DllImport("user32.dll")]
		public static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetForegroundWindow(IntPtr hWnd);
	}

	public class SendMyKeys
	{
		public static void PasteTextToApp(string strText,string strAppName, string strTitle)
		{
			foreach (Process p in Process.GetProcessesByName(strAppName))
			{
				if(strTitle!=null)
					if (p.MainWindowTitle.IndexOf(strTitle) < 0)
						continue;
				NativeWIN32.SetForegroundWindow(p.MainWindowHandle);
				Thread.Sleep(5000);
				SendMyKeys.SendString(strText);
				return;
			}
		}

		public static void ClipBoardToApp(string strAppName, string strTitle)
		{
			foreach (Process p in Process.GetProcessesByName(strAppName))
			{
				if (strTitle != null)
					if (p.MainWindowTitle.IndexOf(strTitle) < 0)
						continue;
				NativeWIN32.SetForegroundWindow(p.MainWindowHandle);
				Thread.Sleep(1000);
				SendMyKeys.SendChar((ushort)Keys.V,false,true);
				return;
			}
		}

		public static void SendChar(ushort wVk, bool ShiftKey, bool ControlKey)
		{
			uint uintReturn;

			NativeWIN32.INPUT structInput = new NativeWIN32.INPUT();
			structInput.type = (uint)1;
			structInput.ki.wScan = 0;
			structInput.ki.time = 0;
			structInput.ki.dwFlags = 0;
			structInput.ki.dwExtraInfo = 0;

			if (ControlKey)
			{
				structInput.ki.wVk = (ushort)Keys.ControlKey;
				uintReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}

			if (ShiftKey)
			{
				structInput.ki.wVk = (ushort)Keys.ShiftKey;
				uintReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}

			structInput.ki.wVk = wVk;
			uintReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));

			structInput.ki.dwFlags = NativeWIN32.KEYEVENTF_KEYUP;
			structInput.ki.wVk = wVk;
			uintReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));

			if (ShiftKey)
			{
				structInput.ki.dwFlags = NativeWIN32.KEYEVENTF_KEYUP;
				structInput.ki.wVk = (ushort)Keys.ShiftKey;
				uintReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}

			if (ControlKey)
			{
				structInput.ki.dwFlags = NativeWIN32.KEYEVENTF_KEYUP;
				structInput.ki.wVk = (ushort)Keys.ControlKey;
				uintReturn = NativeWIN32.SendInput((uint)1, ref structInput, Marshal.SizeOf(structInput));
			}
		}

		public static void SendString(string s)
		{
			ushort wVk;
			bool ShiftKey;
			foreach (Char c in s.ToCharArray())
			{
				if (Char.IsUpper(c))
					ShiftKey = true;
				else
					ShiftKey = false;

				wVk = (ushort)Char.ToUpper(c);

				string special = ")!@#$%^&*(";

				int intDigit = special.IndexOf(c);
				if (intDigit >= 0)
				{
					ShiftKey = true;
					wVk = (ushort)('0' + intDigit);
				}
				else
				{
					switch (c)
					{
						case ':':
							ShiftKey = true;
							wVk = (ushort)Keys.Oem1;
							break;
						case ';':
							wVk = (ushort)Keys.Oem1;
							break;
						case '-':
							wVk = (ushort)Keys.OemMinus;
							break;
						case '_':
							ShiftKey = true;
							wVk = (ushort)Keys.OemMinus;
							break;
						case '+':
							ShiftKey = true;
							wVk = (ushort)Keys.Oemplus;
							break;
						case '=':
							wVk = (ushort)Keys.Oemplus;
							break;
						case '/':
							wVk = (ushort)Keys.Oem2;
							break;
						case '?':
							ShiftKey = true;
							wVk = (ushort)Keys.OemQuestion;
							break;
						case '.':
							wVk = (ushort)Keys.OemPeriod;
							break;
						case '>':
							ShiftKey = true;
							wVk = (ushort)Keys.OemPeriod;
							break;
						case ',':
							wVk = (ushort)Keys.Oemcomma;
							break;
						case '<':
							ShiftKey = true;
							wVk = (ushort)Keys.Oemcomma;
							break;
						case '`':
							wVk = (ushort)Keys.Oemtilde;
							break;
						case '~':
							ShiftKey = true;
							wVk = (ushort)Keys.Oemtilde;
							break;
						case '|':
							ShiftKey = true;
							wVk = (ushort)Keys.Oem5;
							break;
						case '\\':
							wVk = (ushort)Keys.Oem5;
							break;
						case '[':
							wVk = (ushort)Keys.OemOpenBrackets;
							break;
						case '{':
							ShiftKey = true;
							wVk = (ushort)Keys.OemOpenBrackets;
							break;
						case ']':
							wVk = (ushort)Keys.Oem6;
							break;
						case '}':
							ShiftKey = true;
							wVk = (ushort)Keys.Oem6;
							break;
						case '\'':
							wVk = (ushort)Keys.Oem7;
							break;
						case '"':
							ShiftKey = true;
							wVk = (ushort)Keys.Oem7;
							break;
						default:
							break;
					}
				}

				SendChar(wVk, ShiftKey, false);

			}
		}
	}

}
