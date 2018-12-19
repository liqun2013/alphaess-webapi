using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetPowerDataResponseModel : ExternalBaseResponseModel
	{
		public PowerReportDataDto Result { get; set; }
	}
}
