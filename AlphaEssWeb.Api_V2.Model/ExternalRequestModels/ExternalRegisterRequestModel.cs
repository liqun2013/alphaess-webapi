using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalRegisterRequestModel : ExternalBaseRequestModel
	{
		public string Sn { get; set; }
		public string License_No { get; set; }
		[Required]
		[StringLength(64)]
		public string Username { get; set; }
		[Required]
		[StringLength(64)]
		public string Password { get; set; }
		[Required]
		[EmailAddress]
		[StringLength(64)]
		public string Email { get; set; }
		[Required]
		[StringLength(16)]
		public string PostCode { get; set; }
		public int Allow_AutoUpdate { get; set; }
		[Required]
		public string Country { get; set; }
	}
}
