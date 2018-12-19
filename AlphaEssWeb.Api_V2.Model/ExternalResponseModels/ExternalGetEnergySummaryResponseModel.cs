using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetEnergySummaryResponseModel : ExternalBaseResponseModel
	{
		public PaginatedDto<ReportEnergyDto> Result { get; set; }
	}
}
