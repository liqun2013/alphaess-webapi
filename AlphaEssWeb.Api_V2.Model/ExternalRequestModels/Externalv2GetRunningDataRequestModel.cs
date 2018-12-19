using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class Externalv2GetRunningDataRequestModel : ExternalBaseRequestModel
	{
		[StringLength(50)]
		public string Sn { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
