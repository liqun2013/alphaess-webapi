using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public sealed class ExternalGetEnergeDataResponseModel : ExternalBaseResponseModel
	{
		public EnergyReportDataDto Result { get; set; }
	}
}
