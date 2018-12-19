using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalUpdateSystemFirmwareRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[MaxLength(64)]
		public string Sn { get; set; }
		[MaxLength(64)]
		public string Category { get; set; }
	}
}
