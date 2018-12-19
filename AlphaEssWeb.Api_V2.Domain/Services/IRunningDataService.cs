using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IRunningDataService
	{
		/// <summary>
		/// 获取最新的系统常规运行数据 
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="token">token</param>
		/// <returns></returns>
		OperationResult<PaginatedList<VT_COLDATA>> GetRunningNewData(string api_Account, long timeStamp, string sign, string sn, string token, string ipAddress);

		/// <summary>
		/// 获取历史的系统常规运行数据 
		/// </summary>
		/// <param name="sn"></param>
		/// <param name="token">token</param>
		/// <param name="starttime"></param>
		/// <param name="endtime"></param>
		/// <returns></returns>
		OperationResult<PaginatedList<VT_COLDATA>> GetHistoryRunningData(string api_Account, long timeStamp, string sign, string sn, string token, string starttime, string endtime, string ipAddress);
	}
}
