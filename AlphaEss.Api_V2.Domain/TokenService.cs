using AlphaEssWeb.Api_V2.Model;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Helpers;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class TokenService
	{

		/// <summary>
		/// 过期时间 配置在webconfig里
		/// </summary>
		public static int ExpiryTime
		{
			get
			{
				var expiryTime = System.Configuration.ConfigurationManager.AppSettings["expiryTime"];
				int time = 90;
				int.TryParse(expiryTime, out time);

				if (int.TryParse(expiryTime, out time))
				{
					return time;
				}
				else {
					time = 90;
					return time;
				}
			}
		}

		/// <summary>
		/// 清理缓存
		/// </summary>
		public static void ClearCache(Guid token)
		{
			string key = token.ToString();
			WebCache.Remove(key);
		}

		/// <summary>
		/// 设定缓存
		/// </summary>
		/// <param name="token"></param>
		public static void SetCache(Guid token, Guid userId, DateTime expiryTime, string ip)
		{
			Token user = WebCache.Get(token.ToString()) as Token;
			if (user != null)
			{
				ClearCache(token);
			}

			Token newUser = new Token();
			newUser.UserId = userId;
			newUser.ExpirationTime = expiryTime;
			newUser.ipAddress = ip;

            WebCache.Set(token.ToString(), newUser, ExpiryTime);
		}

		/// <summary>
		/// 获取缓存
		/// </summary>
		/// <param name="token"></param>
		/// <returns></returns>
		public static Token GetCache(Guid token)
		{
			var user = WebCache.Get(token.ToString()) as Token;

			if (user != null)
			{
				Token newUser = new Token();
				newUser.UserId = user.UserId;
				newUser.ExpirationTime = DateTime.Now.AddMinutes(ExpiryTime);
				newUser.ipAddress = user.ipAddress;

				ClearCache(token);
				WebCache.Set(token.ToString(), newUser, ExpiryTime);

				return newUser;
			}
			return user;
		}
	}
}
