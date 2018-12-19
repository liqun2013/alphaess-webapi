using AlphaEssWeb.Api_V2.Model.Validation;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class Externalv2GetSystemListRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Minimum(1)]
		public int PageIndex { get; set; }
		[Minimum(1)]
		public int PageSize { get; set; }
	}
}
