using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetDeviceCVRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(50)]
		public string DeviceID { get; set; }
		[Required]
		[StringLength(50)]
		public string LocalDate { get; set; }
	}
}
