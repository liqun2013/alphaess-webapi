using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalInstallNewSystemRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		public string NewSN { get; set; }
		[Required]
		public string CheckCode { get; set; }
		[Required]
		public string License_no { get; set; }
		[Required]
		public DateTime InstallationDate { get; set; }
		[Required]
		public string CustomerName { get; set; }
		[Required]
		public string ContactNumber { get; set; }
		[Required]
		public string ContactAddress { get; set; }
	}
}
