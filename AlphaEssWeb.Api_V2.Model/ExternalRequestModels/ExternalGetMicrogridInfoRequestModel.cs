using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetMicrogridInfoRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(50)]
		public string Username { get; set; }
	}
}
