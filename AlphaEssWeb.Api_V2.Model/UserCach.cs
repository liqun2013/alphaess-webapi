using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlphaEssWeb.Api_V2.Model
{
	public sealed class UserCach
	{
		/// <summary>
		/// 返回 用户缓存类型
		/// </summary>

		public System.Guid UserId { get; set; }

		public DateTime ExpiryTime { get; set; }

		public string ipAddress { get; set; }
	}
}
