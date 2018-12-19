using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IPowerDataService
	{
		/// <summary>
		/// 获取实时功率数据 
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="username">用户名</param>
		/// <returns></returns>
		OperationResult<PaginatedList<PowerData>> GetLastPowerData(string api_Account, long timeStamp, string sign, string sn, string token, string ipAddress);
	}
}
