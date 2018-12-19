namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalLoginUserResponseModel : ExternalBaseResponseModel
	{
		public string userType { get; set; }
		public string Token { get; set; }
	}
}
