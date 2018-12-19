using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalUpdateMsgFlagRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[Range(1, 2)]
		public int Flag { get; set; }
		[Required]
		public string MsgId { get; set; }
	}
}
