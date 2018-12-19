namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalQuerySystemByUserResponseModel<T> : ExternalBaseResponseModel
	{
		public T Result { get; set; }
	}
}
