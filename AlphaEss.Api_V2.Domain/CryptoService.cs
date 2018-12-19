using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class CryptoService : ICryptoService
	{
		//public CryptoService()
		//{
		//}

		//public CryptoService(int exptime)
		//{
		//	ExpirationTime = exptime;
		//}
		///// <summary>
		///// 过期时间(单位:分钟) 配置在webconfig里
		///// </summary>
		//public int ExpirationTime { get; set; }

		//public string GetSecretKey(string api_Account)
		//{
		//	string SecretKey = WebCache.Get(api_Account.Trim().ToString());

		//	if (SecretKey != null)
		//	{
		//		return SecretKey;
		//	}
		//	string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["AlphaEssDbContext"].ToString();//定义数据库连接参数

		//	using (SqlConnection connection = new SqlConnection(connectionString))
		//	{
		//		DataSet ds = new DataSet();
		//		string SQLString = "SELECT Api_SecretKey FROM [dbo].[SYS_API] where Api_Account = '" + api_Account.Trim() + "'";
		//		try
		//		{
		//			connection.Open();
		//			SqlDataAdapter command = new SqlDataAdapter(SQLString, connection);
		//			command.Fill(ds, "ds");

		//			if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
		//			{
		//				SecretKey = ds.Tables[0].Rows[0][0].ToString();
		//			}
		//			else
		//			{
		//				SecretKey = "";
		//			}

		//			WebCache.Set(api_Account.Trim().ToString(), SecretKey, ExpirationTime);
		//		}
		//		catch (System.Data.SqlClient.SqlException ex)
		//		{
		//			throw new Exception(ex.Message);
		//		}
		//		finally
		//		{
		//			connection.Close();
		//		}
		//	}
		//	return SecretKey;
		//}

		public string EncryptNew(string plainText, string secretKey)
		{
			if (string.IsNullOrEmpty(plainText))
				throw new ArgumentNullException("plainText");

			string encrypted = string.Empty;
			byte[] clearBytes = Encoding.UTF8.GetBytes(plainText);
			using (Aes aesAlg = Aes.Create())
			{
				byte[] k;
				byte[] iv;
				GeneralKeyIV(secretKey, out k, out iv);
				aesAlg.Key = k;
				aesAlg.IV = iv;

				ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msEncrypt = new MemoryStream())
				{
					using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
					{
						csEncrypt.Write(clearBytes, 0, clearBytes.Length);
					}
					encrypted = Convert.ToBase64String(msEncrypt.ToArray());
				}
			}

			return encrypted;
		}

		public string DecryptNew(string cipherText, string secretKey)
		{
			// Check arguments.
			if (string.IsNullOrEmpty(cipherText))
				throw new ArgumentNullException("cipherText");

			// Declare the string used to hold the decrypted text.
			string plaintext = null;

			// Create an Aes object with the specified key and IV.
			using (Aes aesAlg = Aes.Create())
			{
				//Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(secretKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
				//aesAlg.Key = pdb.GetBytes(32);
				//aesAlg.IV = pdb.GetBytes(16);
				byte[] k;
				byte[] iv;
				GeneralKeyIV(secretKey, out k, out iv);
				aesAlg.Key = k;
				aesAlg.IV = iv;

				// Create a decrytor to perform the stream transform.
				ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

				using (MemoryStream msDecrypt = new MemoryStream())
				{
					try
					{
						using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Write))
						{
							var bytesToBeDecrypted = Convert.FromBase64String(cipherText);
							csDecrypt.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
						}
						plaintext = Encoding.Unicode.GetString(msDecrypt.ToArray());
					}
					catch (CryptographicException)
					{
						plaintext = string.Empty;
					}
				}
			}

			return plaintext;
		}

		public string GenerateMD5Hash(string input)
		{
			if (string.IsNullOrEmpty(input))
			{
				throw new ArgumentException("argument cannot be null", "input");
			}

			using (MD5 md5Hash = MD5.Create())
			{
				byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
				StringBuilder sb = new StringBuilder();

				for (int i = 0; i < data.Length; i++)
				{
					sb.Append(data[i].ToString("x2"));
				}

				return sb.ToString();
			}
		}

		private void GeneralKeyIV(string keyStr, out byte[] key, out byte[] iv)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(keyStr);
			key = SHA256.Create().ComputeHash(bytes);
			iv = MD5.Create().ComputeHash(bytes);
		}
	}
}
