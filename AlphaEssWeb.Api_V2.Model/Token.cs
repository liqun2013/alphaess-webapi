using System;

namespace AlphaEssWeb.Api_V2.Model
{
	public sealed class Token
	{
		public Token(Guid uid, DateTime et, string[] uts)
		{
			UserId = uid;
			ExpirationTime = et;
			UserTypes = uts;
		}
		/// <summary>
		/// 返回 用户缓存类型
		/// </summary>

		public Guid UserId { get; set; }

		public DateTime ExpirationTime { get; set; }
		public string[] UserTypes { get; set; }

		//public string ipAddress { get; set; }
	}
}
