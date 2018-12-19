using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalChangeUserInfoRequestModel : ExternalBaseRequestModel
	{
		[Required]
		[StringLength(64)]
		public string Username { get; set; }
		[Required]
		[StringLength(50)]
		public string Language_Code { get; set; }
		[Required]
		[StringLength(50)]
		public string Country { get; set; }
		public string State { get; set; }
		public string City { get; set; }
		[Required]
		[StringLength(20)]
		public string Zipcode { get; set; }
		[Required]
		[StringLength(50)]
		public string Contact_User { get; set; }
		[Required]
		[StringLength(100)]
		[EmailAddress]
		public string Email { get; set; }
		[Required]
		public int Allow_AutoUpdate { get; set; }
		[Required]
		[StringLength(20)]
		public string CellPhone { get; set; }
		[StringLength(50)]
		public string Address { get; set; }
		[Required]
		public string Token { get; set; }
	}
}
