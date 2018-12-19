using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalChangePwdRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(64)]
		public string Username { get; set; }
		[Required]
		[StringLength(64)]
		public string OldPwd { get; set; }
		[Required]
		[StringLength(64)]
		public string NewPwd { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
