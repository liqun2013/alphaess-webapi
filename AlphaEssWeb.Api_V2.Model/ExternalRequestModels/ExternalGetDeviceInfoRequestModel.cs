using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetDeviceInfoRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(50)]
		public string DeviceID { get; set; }
	}
}
