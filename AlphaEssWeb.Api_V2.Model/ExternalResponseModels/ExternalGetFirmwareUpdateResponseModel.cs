namespace AlphaEssWeb.Api_V2.Model.ExternalResponseModels
{
	public class ExternalGetFirmwareUpdateResponseModel<T> : ExternalBaseResponseModel
	{
		public T Result { get; set; }
	}
}
