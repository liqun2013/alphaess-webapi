using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain.Services
{
	public interface IDevInfoService
	{
		/// <summary>
		/// 获取 设备信息
		/// </summary>
		/// <param name="deviceID">设备编号</param>
		/// <returns></returns>
		OperationResult<DeviceInfo> GetDeviceInfo(string api_account, long timeStamp, string sign, string deviceID);

	}
}
