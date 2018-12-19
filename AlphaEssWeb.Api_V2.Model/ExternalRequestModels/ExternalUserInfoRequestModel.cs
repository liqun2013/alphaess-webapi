using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalUserInfoRequestModel : ExternalBaseRequestModel
	{
		[StringLength(64)]
		public string Username { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
