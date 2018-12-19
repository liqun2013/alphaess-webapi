using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class EncryptoService
	{
		private static readonly Byte[] DesKey = { 5, 7, 8, 9, 0, 2, 1, 6 };
		private static readonly Byte[] DesVi = { 6, 9, 8, 5, 1, 6, 2, 8 };

		/// <summary>
		/// 加密
		/// </summary>
		/// <param name="txt"></param>
		/// <returns></returns>
		public static string EncrypNew(string txt)
		{
			DESCryptoServiceProvider des = new DESCryptoServiceProvider();
			Encoding utf = new UTF8Encoding();
			ICryptoTransform encryptor = des.CreateEncryptor(DesKey, DesVi);

			byte[] bData = utf.GetBytes(txt);
			byte[] bEnc = encryptor.TransformFinalBlock(bData, 0, bData.Length);
			string cipherText = Convert.ToBase64String(bEnc);

			return cipherText;
		}


		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="cipherText"></param>
		/// <returns></returns>
		public static string Decrypt(string cipherText)
		{
			string txt = string.Empty;
			if (!string.IsNullOrEmpty(cipherText))
			{
				DESCryptoServiceProvider des = new DESCryptoServiceProvider();
				Encoding utf = new UTF8Encoding();
				ICryptoTransform decryptor = des.CreateDecryptor(DesKey, DesVi);

				byte[] bEnc = Convert.FromBase64String(cipherText);
				byte[] bDec = decryptor.TransformFinalBlock(bEnc, 0, bEnc.Length);

				txt = utf.GetString(bDec);		
			}
			return txt;
		}

	}
}
