using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetSystemSummaryStatisticsDataRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
	}
}
