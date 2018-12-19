namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalSystemListResponseModel<T> : ExternalBaseResponseModel
	{
		public T Result { get; set; }
	}
}
