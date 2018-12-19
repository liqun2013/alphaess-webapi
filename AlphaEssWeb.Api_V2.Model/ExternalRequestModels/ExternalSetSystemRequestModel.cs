using AlphaEssWeb.Api_V2.Model.Dtos;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalSetSystemRequestModel : VTSYSTEMDto
	{
		//[StringLength(64)]
		//public string Sn { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
