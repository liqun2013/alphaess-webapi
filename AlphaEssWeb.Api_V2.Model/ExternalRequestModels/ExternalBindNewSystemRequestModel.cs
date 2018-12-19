using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalBindNewSystemRequestModel : ExternalBaseRequestModel
	{
		[StringLength(64)]
		public string NewSn { get; set; }
		[Required]
		public string Token { get; set; }
		[Required]
		public string CheckCode { get; set; }
		[Required]
		public string Username { get; set; }
		[Required]
		public string License_no { get; set; }
	}
}
