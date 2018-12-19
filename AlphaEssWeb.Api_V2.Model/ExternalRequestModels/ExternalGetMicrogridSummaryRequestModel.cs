using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetMicrogridSummaryRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public Guid MicrogridId { get; set; }
	}
}
