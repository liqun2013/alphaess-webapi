using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public sealed class ExternalGetMicrogridSummaryResponseModel : ExternalBaseResponseModel
	{
		public MicrogridSummaryDto Result { get; set; }
	}
}
