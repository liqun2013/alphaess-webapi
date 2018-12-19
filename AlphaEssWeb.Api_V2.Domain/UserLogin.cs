using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain
{
	public sealed class UserLogin
	{
		/// <summary>
		/// 返回 用户类型
		/// </summary>
		public string userType { get; set; }

		public Guid token { get; set; }

		public List<string> userTypes { get; set; }

		public System.Guid UserId { get; set; }

		public string Licno { get; set; }

		public string Username { get; set; }

		public string Userpwd { get; set; }

		public DateTime ExpiryTime { get; set; }
	}
}
