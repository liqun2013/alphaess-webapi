using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetSystemStatusRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(500)]
		public string Sn { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
