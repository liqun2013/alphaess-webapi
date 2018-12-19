using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public abstract class ExternalBaseRequestModel
	{
		[Required]
		[StringLength(64)]
		public string Api_Account { get; set; }
		[Required]
		public long TimeStamp { get; set; }
		[Required]
		public string Sign { get; set; }
	}
}
