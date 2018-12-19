using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IReportInfoService
	{
		/// <summary>
		/// 获取系统能量信息
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="theDate">日期</param>
		/// <param name="token">token</param>
		/// <returns></returns>
		OperationResult<PaginatedList<Report_Energy>> GetEnergySummary(string api_Account, long timeStamp, string sign, string sn, string theDate, string token, string ipAddress);

	}
}
