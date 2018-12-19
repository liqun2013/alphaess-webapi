using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model
{
	public sealed class UserContext
	{
		/// <summary>
		/// 返回 用户类型
		/// </summary>
		public string UserType { get; set; }

		public Guid Token { get; set; }

		public List<string> UserTypes { get; set; }

		public System.Guid UserId { get; set; }

		public string Licno { get; set; }

		public string Username { get; set; }

		public string Userpwd { get; set; }

		public DateTime ExpiryTime { get; set; }
	}
}
