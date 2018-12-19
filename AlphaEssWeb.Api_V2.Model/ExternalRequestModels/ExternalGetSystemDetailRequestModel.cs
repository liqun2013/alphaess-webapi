using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetSystemDetailRequestModel : ExternalBaseRequestModel
	{
		[StringLength(64)]
		public string Sn { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
