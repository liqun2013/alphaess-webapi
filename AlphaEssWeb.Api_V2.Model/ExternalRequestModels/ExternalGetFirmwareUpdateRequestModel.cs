using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetFirmwareUpdateRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[MaxLength(64)]
		public string Sn { get; set; }
	}
}
