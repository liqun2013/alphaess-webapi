using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public sealed class ExternalGetMicrogridSchedulingStrategyResponseModel : ExternalBaseResponseModel
	{
		public SchedulingStrategyDto Result { get; set; }
	}
}
