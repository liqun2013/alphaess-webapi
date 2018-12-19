using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class DeviceInfoExtension
	{
		public static DeviceInfoDto ToDeviceInfoDto(this DeviceInfo ms)
		{
			return new DeviceInfoDto
			{
				Status = ms.Status,
				DeviceID = ms.DeviceID,
				TimeZone = ms.TimeZone,
				UpdateTime = ms.UpdateTime,
				Voltage = ms.Voltage
			};
		}
	}
}
