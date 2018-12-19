using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetDeviceCVResponseModel : ExternalBaseResponseModel
	{
		public string Status { get; set; }
		public DeviceCVDto Result { get; set; }
	}
}
