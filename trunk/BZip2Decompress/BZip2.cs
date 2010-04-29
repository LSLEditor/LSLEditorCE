using System;
using System.IO;

// 	BZip2.Decompress(File.OpenRead("in"), File.Create("out"));


namespace LSLEditor.BZip2Decompress
{
	public sealed class Decompressor
	{
		public static void Decompress(Stream inStream, Stream outStream) 
		{
			using ( outStream ) {
				using ( BZip2InputStream bzis = new BZip2InputStream(inStream) ) {
					int ch = bzis.ReadByte();
					while (ch != -1) {
						outStream.WriteByte((byte)ch);
						ch = bzis.ReadByte();
					}
				}
			}
		}
	}
}
