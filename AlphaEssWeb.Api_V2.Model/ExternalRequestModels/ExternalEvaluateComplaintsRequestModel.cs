using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalEvaluateComplaintsRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		public long ComplaintsId { get; set; }
		[Required]
		public int Satisfaction { get; set; }
		[Required]
		public int Satisfaction1 { get; set; }
		[Required]
		public int Satisfaction2 { get; set; }
		public string Content { get; set; }
	}
}
