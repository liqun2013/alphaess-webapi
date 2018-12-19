using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public sealed class ExternalGetMicrogridInfoResponseModel : ExternalBaseResponseModel
	{
		public MicrogridInfoDto Result { get; set; }
	}
}
