using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetMicrogridSchedulingStrategyRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public System.Guid MicrogridId { get; set; }
	}
}
