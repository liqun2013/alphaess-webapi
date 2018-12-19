using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IDevDataService
	{
		/// <summary>
		/// 获取 智能电表 读数
		/// </summary>
		/// <param name="deviceID">设备编号</param>
		/// <param name="localDate">需要获取数据的日期，日期的格式为：yyyy-MM-dd</param>
		/// <returns></returns>
		OperationResult<DeviceCV> GetDeviceCV(string api_account, long timeStamp, string sign, string deviceID, string localDate);
	}
}
