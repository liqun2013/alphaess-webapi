using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public sealed class ExternalGetProfitDataResponseModel : ExternalBaseResponseModel
	{
		public ProfitReportDataDto Result { get; set; }
	}
}
