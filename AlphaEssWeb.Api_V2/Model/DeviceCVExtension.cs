using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class DeviceCVExtension
	{
		public static DeviceCVDto ToDeviceCVDto(this DeviceCV ms)
		{
			return new DeviceCVDto
			{
				Time = ms.Time,
				CVTotal = ms.CVTotal,
				CV1 = ms.CV1,
				CV2 = ms.CV2,
				CV3 = ms.CV3
			};
		}
	}
}
