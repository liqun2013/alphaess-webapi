using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IAlphaESSV2Service
	{

		/// <summary>
		/// 用户登录
		/// </summary>
		/// <param name="username">用户名</param>
		/// <param name="password">密码</param>
		/// <returns></returns>
		OperationResult<UserLogin> LoginForUser(string api_Account, long timeStamp, string sign, string username, string password, string ipAddress);

		/// <summary>
		/// 根据用户获取储能列表
		/// </summary>
		/// <param name="pageIndex">页索引</param>
		/// <param name="pageSize">每页记录数</param>
		/// <returns></returns>
		OperationResult<PaginatedList<VT_SYSTEM>> GetSystemListForUser(string api_Account, long timeStamp, string sign, string token, int? pageIndex, int? pageSize, string ipAddress);
	}
}
