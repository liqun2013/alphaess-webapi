using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalElectricDispatchingControlRecordRequestModel : ExternalBaseRequestModel
	{
		[Required]
		public Guid MicrogridId { get; set; }
		[Required]
		public string CmdIndex { get; set; }
		[Required]
		public decimal ControlPower { get; set; }
	}
}
