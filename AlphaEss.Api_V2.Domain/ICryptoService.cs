namespace AlphaEss.Api_V2.Infrastructure
{
	public interface ICryptoService
	{
		/// <summary>
		/// 密钥
		/// </summary>
		//string GetSecretKey(string api_Account);
		//string Encrypt(string plainText);
		//string Encrypt(string plainText, string secretKey);
		/// <summary>
		/// 生成md5hash值
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		string GenerateMD5Hash(string input);
		//string Decrypt(string cipherText, string secretKey);
		/// <summary>
		/// 加密
		/// </summary>
		/// <param name="plainText">要加密的字符串</param>
		/// <param name="secretKey">密钥</param>
		/// <returns></returns>
		string EncryptNew(string plainText, string secretKey);
		/// <summary>
		/// 解密
		/// </summary>
		/// <param name="cipherText">要解密的字符串</param>
		/// <param name="secretKey">密钥</param>
		/// <returns></returns>
		string DecryptNew(string cipherText, string secretKey);
	}
}
