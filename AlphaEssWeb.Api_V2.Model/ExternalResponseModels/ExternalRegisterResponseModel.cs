namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalRegisterResponseModel : ExternalBaseResponseModel
	{

	}

	public class ExternalRegisterResponseModel<T> : ExternalBaseResponseModel
	{
		public T Result { get; set; }
	}
}
