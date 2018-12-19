using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetUserAgreementRequestModel : ExternalBaseRequestModel
	{
		[StringLength(10)]
		public string Language_Code { get; set; }
	}
}
