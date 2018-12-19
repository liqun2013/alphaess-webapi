using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class Externalv2RemoteDispatchRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }

		[StringLength(64)]
		public string SN { get; set; }

		[Required]
		public int ActivePower { get; set; }

		[Required]
		public int ReactivePower { get; set; }

		[Required]
		public decimal SOC { get; set; }

		[Required]
		public int Status { get; set; }

		[Required]
		public int ControlMode { get; set; }

	}
}
