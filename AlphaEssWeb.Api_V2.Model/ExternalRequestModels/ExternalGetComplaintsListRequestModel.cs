using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetComplaintsListRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[Range(1, 10000)]
		public int PageIndex { get; set; }
		[Required]
		[Range(1, 100)]
		public int PageSize { get; set; }
	}
}
