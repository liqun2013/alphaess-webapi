namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetMsgResponseModel<T> : ExternalBaseResponseModel
	{
		public T Result { get; set; }
	}
}
