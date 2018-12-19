using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalRetrievePwdRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(50)]
		public string Username { get; set; }
		[Required]
		[EmailAddress]
		[StringLength(100)]
		public string Email { get; set; }
	}
}
