using AlphaEssWeb.Api_V2.Model.Dtos;

namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetUserAgreementByLanguageCodeResponseModel : ExternalBaseResponseModel
	{
		public SysUserAgreementDto Result { get; set; }
	}
}
