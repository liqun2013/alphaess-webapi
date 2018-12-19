using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetDeviceInfoResponseModel : ExternalBaseResponseModel
	{
		//public DeviceInfoDto Result { get; set; }
		public string Status { get; set; }
		public string DeviceID { get; set; }
		public decimal Voltage { get; set; }
		public string TimeZone { get; set; }
		public string UpdateTime { get; set; }
	}
}
