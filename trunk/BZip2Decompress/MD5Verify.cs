using System;
using System.Security.Cryptography;
using System.IO;

namespace LSLEditor.BZip2Decompress
{
	class MD5Verify
	{
		public static string ComputeHash(string strFile)
		{
			MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
			FileStream stream = File.OpenRead(strFile);
			byte[] hash = csp.ComputeHash(stream);
			stream.Close();
			return BitConverter.ToString(hash).Replace("-", "").ToLower();
		}	
	}
}
