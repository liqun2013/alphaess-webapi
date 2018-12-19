using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetEnergeDataRequestModel : ExternalBaseRequestModel
	{
		[StringLength(64)]
		public string Sn { get; set; }
		[StringLength(64)]
		public string Username { get; set; }
		[StringLength(10)]
		public string Date_Start { get; set; }
		[StringLength(10)]
		public string Date_End { get; set; }
		[Required]
		public string Token { get; set; }
		public string StatisticsBy { get; set; }
	}
}
