namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalLoginResponseModel: ExternalBaseResponseModel
	{
		public string UserType { get; set; }
		public string Token { get; set; }
	}
}
