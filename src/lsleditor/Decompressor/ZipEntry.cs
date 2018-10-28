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

namespace LSLEditor.Decompressor
{

	public class ZipEntry
	{

		private const int ZipEntrySignature = 0x04034b50;
		private const int ZipEntryDataDescriptorSignature = 0x08074b50;

		private DateTime _LastModified;

		private string _FileName;

		private Int16 _VersionNeeded;

		private Int16 _BitField;

		private Int16 _CompressionMethod;

		private Int32 _CompressedSize;

		private Int32 _UncompressedSize;

		private Int32 _LastModDateTime;
		private Int32 _Crc32;
		private byte[] _Extra;

		private byte[] __filedata;
		private byte[] _FileData
		{
			get
			{
				if (__filedata == null)
				{
				}
				return __filedata;
			}
		}

		private System.IO.MemoryStream _UnderlyingMemoryStream;
		private System.IO.Compression.DeflateStream _CompressedStream;
		private System.IO.Compression.DeflateStream CompressedStream
		{
			get
			{
				if (_CompressedStream == null)
				{
					_UnderlyingMemoryStream = new System.IO.MemoryStream();
					bool LeaveUnderlyingStreamOpen = true;
					_CompressedStream = new System.IO.Compression.DeflateStream(_UnderlyingMemoryStream,
													System.IO.Compression.CompressionMode.Compress,
													LeaveUnderlyingStreamOpen);
				}
				return _CompressedStream;
			}
		}

		private static bool ReadHeader(System.IO.Stream s, ZipEntry ze)
		{
			int signature = Shared.ReadSignature(s);

			// return null if this is not a local file header signature
			if (SignatureIsNotValid(signature))
			{
				s.Seek(-4, System.IO.SeekOrigin.Current);
				return false;
			}

			byte[] block = new byte[26];
			int n = s.Read(block, 0, block.Length);
			if (n != block.Length) return false;

			int i = 0;
			ze._VersionNeeded = (short)(block[i++] + block[i++] * 256);
			ze._BitField = (short)(block[i++] + block[i++] * 256);
			ze._CompressionMethod = (short)(block[i++] + block[i++] * 256);
			ze._LastModDateTime = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;

			// the PKZIP spec says that if bit 3 is set (0x0008), then the CRC, Compressed size, and uncompressed size
			// come directly after the file data.  The only way to find it is to scan the zip archive for the signature of 
			// the Data Descriptor, and presume that that signature does not appear in the (compressed) data of the compressed file.  

			if ((ze._BitField & 0x0008) != 0x0008)
			{
				ze._Crc32 = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
				ze._CompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
				ze._UncompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
			}
			else
			{
				// the CRC, compressed size, and uncompressed size are stored later in the stream.
				// here, we advance the pointer.
				i += 12;
			}

			Int16 filenameLength = (short)(block[i++] + block[i++] * 256);
			Int16 extraFieldLength = (short)(block[i++] + block[i++] * 256);

			block = new byte[filenameLength];
			n = s.Read(block, 0, block.Length);
			ze._FileName = Shared.StringFromBuffer(block, 0, block.Length);

			ze._Extra = new byte[extraFieldLength];
			n = s.Read(ze._Extra, 0, ze._Extra.Length);

			// transform the time data into something usable
			ze._LastModified = Shared.PackedToDateTime(ze._LastModDateTime);

			// actually get the compressed size and CRC if necessary
			if ((ze._BitField & 0x0008) == 0x0008)
			{
				long posn = s.Position;
				long SizeOfDataRead = Shared.FindSignature(s, ZipEntryDataDescriptorSignature);
				if (SizeOfDataRead == -1) return false;

				// read 3x 4-byte fields (CRC, Compressed Size, Uncompressed Size)
				block = new byte[12];
				n = s.Read(block, 0, block.Length);
				if (n != 12) return false;
				i = 0;
				ze._Crc32 = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
				ze._CompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;
				ze._UncompressedSize = block[i++] + block[i++] * 256 + block[i++] * 256 * 256 + block[i++] * 256 * 256 * 256;

				if (SizeOfDataRead != ze._CompressedSize)
					throw new Exception("Data format error (bit 3 is set)");

				// seek back to previous position, to read file data
				s.Seek(posn, System.IO.SeekOrigin.Begin);
			}

			return true;
		}


		private static bool SignatureIsNotValid(int signature)
		{
			return (signature != ZipEntrySignature);
		}

		public static ZipEntry Read(System.IO.Stream s)
		{
			ZipEntry entry = new ZipEntry();
			if (!ReadHeader(s, entry)) return null;

			entry.__filedata = new byte[entry._CompressedSize];
			int n = s.Read(entry._FileData, 0, entry._FileData.Length);
			if (n != entry._FileData.Length)
			{
				throw new Exception("badly formatted zip file.");
			}
			// finally, seek past the (already read) Data descriptor if necessary
			if ((entry._BitField & 0x0008) == 0x0008)
			{
				s.Seek(16, System.IO.SeekOrigin.Current);
			}
			return entry;
		}


		public void Extract(System.IO.Stream s)
		{
			using (System.IO.MemoryStream memstream = new System.IO.MemoryStream(_FileData))
			{
				System.IO.Stream input = null;
				try
				{
					if (this._CompressedSize == this._UncompressedSize)
					{
						input = memstream;
					}
					else
					{
						input = new System.IO.Compression.DeflateStream(memstream, System.IO.Compression.CompressionMode.Decompress);
					}

					System.IO.Stream output = null;
					try
					{
						output = s;


						byte[] bytes = new byte[4096];
						int n;

						n = 1; // anything non-zero
						while (n != 0)
						{
							n = input.Read(bytes, 0, bytes.Length);
							if (n > 0)
							{
								output.Write(bytes, 0, n);
							}
						}
					}
					finally
					{
						// we only close the output stream if we opened it. 
						if (output != null)
						{
							output.Close();
							output.Dispose();
						}
					}
				}
				finally
				{
					// we only close the output stream if we opened it. 
					// we cannot use using() here because in some cases we do not want to Dispose the stream!
					if ((input != null) && (input != memstream))
					{
						input.Close();
						input.Dispose();
					}
				}
			}
		}

	}

	class Shared
	{
		protected internal static string StringFromBuffer(byte[] buf, int start, int maxlength)
		{
			int i;
			char[] c = new char[maxlength];
			for (i = 0; (i < maxlength) && (i < buf.Length) && (buf[i] != 0); i++)
			{
				c[i] = (char)buf[i]; // System.BitConverter.ToChar(buf, start+i*2);
			}
			string s = new System.String(c, 0, i);
			return s;
		}

		protected internal static int ReadSignature(System.IO.Stream s)
		{
			int n = 0;
			byte[] sig = new byte[4];
			n = s.Read(sig, 0, sig.Length);
			if (n != sig.Length) throw new Exception("Could not read signature - no data!");
			int signature = (((sig[3] * 256 + sig[2]) * 256) + sig[1]) * 256 + sig[0];
			return signature;
		}

		protected internal static long FindSignature(System.IO.Stream s, int SignatureToFind)
		{
			long startingPosition = s.Position;

			int BATCH_SIZE = 1024;
			byte[] targetBytes = new byte[4];
			targetBytes[0] = (byte)(SignatureToFind >> 24);
			targetBytes[1] = (byte)((SignatureToFind & 0x00FF0000) >> 16);
			targetBytes[2] = (byte)((SignatureToFind & 0x0000FF00) >> 8);
			targetBytes[3] = (byte)(SignatureToFind & 0x000000FF);
			byte[] batch = new byte[BATCH_SIZE];
			int n = 0;
			bool success = false;
			do
			{
				n = s.Read(batch, 0, batch.Length);
				if (n != 0)
				{
					for (int i = 0; i < n; i++)
					{
						if (batch[i] == targetBytes[3])
						{
							s.Seek(i - n, System.IO.SeekOrigin.Current);
							int sig = ReadSignature(s);
							success = (sig == SignatureToFind);
							if (!success) s.Seek(-3, System.IO.SeekOrigin.Current);
							break; // out of for loop
						}
					}
				}
				else break;
				if (success) break;
			} while (true);
			if (!success)
			{
				s.Seek(startingPosition, System.IO.SeekOrigin.Begin);
				return -1;  // or throw?
			}

			// subtract 4 for the signature.
			long bytesRead = (s.Position - startingPosition) - 4;
			// number of bytes read, should be the same as compressed size of file            
			return bytesRead;
		}
		protected internal static DateTime PackedToDateTime(Int32 packedDateTime)
		{
			Int16 packedTime = (Int16)(packedDateTime & 0x0000ffff);
			Int16 packedDate = (Int16)((packedDateTime & 0xffff0000) >> 16);

			int year = 1980 + ((packedDate & 0xFE00) >> 9);
			int month = (packedDate & 0x01E0) >> 5;
			int day = packedDate & 0x001F;


			int hour = (packedTime & 0xF800) >> 11;
			int minute = (packedTime & 0x07E0) >> 5;
			int second = packedTime & 0x001F;

			DateTime d = System.DateTime.Now;
			try { d = new System.DateTime(year, month, day, hour, minute, second, 0); }
			catch
			{
				Console.Write("\nInvalid date/time?:\nyear: {0} ", year);
				Console.Write("month: {0} ", month);
				Console.WriteLine("day: {0} ", day);
				Console.WriteLine("HH:MM:SS= {0}:{1}:{2}", hour, minute, second);
			}

			return d;
		}


		protected internal static Int32 DateTimeToPacked(DateTime time)
		{
			UInt16 packedDate = (UInt16)((time.Day & 0x0000001F) | ((time.Month << 5) & 0x000001E0) | (((time.Year - 1980) << 9) & 0x0000FE00));
			UInt16 packedTime = (UInt16)((time.Second & 0x0000001F) | ((time.Minute << 5) & 0x000007E0) | ((time.Hour << 11) & 0x0000F800));
			return (Int32)(((UInt32)(packedDate << 16)) | packedTime);
		}
	}

}

