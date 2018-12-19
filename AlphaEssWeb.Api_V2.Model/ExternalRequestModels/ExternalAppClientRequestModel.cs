using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalTheLastAppClientVersionRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(160)]
		public string AppType { get; set; }
	}
}
