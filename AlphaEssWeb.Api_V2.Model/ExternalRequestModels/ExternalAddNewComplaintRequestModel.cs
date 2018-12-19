using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalAddNewComplaintRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public string Token { get; set; }
		[Required]
		[StringLength(128)]
		public string Title { get; set; }
		[Required]
		[StringLength(1024)]
		public string Description { get; set; }
		[Required]
		public string ComplaintsType { get; set; }
		[Required]
		[StringLength(64)]
		public string SysSn { get; set; }
		[StringLength(64)]
		public string Email { get; set; }
		[StringLength(16)]
		public string ContactNumber { get; set; }
		public string Attachment1 { get; set; }
		public string Attachment2 { get; set; }
		public string Attachment3 { get; set; }
	}
}
