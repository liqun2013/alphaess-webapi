using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetPowerDataRequestModel : ExternalBaseRequestModel
	{
		[StringLength(50)]
		public string Sn { get; set; }
		[StringLength(50)]
		public string Username { get; set; }
		[Required]
		public string Date { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
