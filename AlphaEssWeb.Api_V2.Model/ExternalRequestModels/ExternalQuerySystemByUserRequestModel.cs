using AlphaEssWeb.Api_V2.Model.Validation;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalQuerySystemByUserRequestModel : ExternalBaseRequestModel
	{
		[Minimum(1)]
		public int PageIndex { get; set; }
		[Minimum(1)]
		public int PageSize { get; set; }

		[Required]
		[StringLength(50)]
		public string Username { get; set; }
	}
}
