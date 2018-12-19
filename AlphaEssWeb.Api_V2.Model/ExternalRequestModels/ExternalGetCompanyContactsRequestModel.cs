using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetCompanyContactsRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public int Flag { get; set; }
	}
}
