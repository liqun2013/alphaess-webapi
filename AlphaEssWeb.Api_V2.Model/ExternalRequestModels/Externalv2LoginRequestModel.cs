using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class Externalv2LoginRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(50)]
		public string Username { get; set; }
		[Required]
		[StringLength(50)]
		public string Password { get; set; }
	}
}
