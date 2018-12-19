using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalGetMsgReauestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[Range(1, 20000)]
		public int PageIndex { get; set; }
		[Required]
		[Range(1, 200)]
		public int PageSize { get; set; }
		[Required]
		[Range(0, 1)]
		public int OnlyUnread { get; set; }
	}
}
