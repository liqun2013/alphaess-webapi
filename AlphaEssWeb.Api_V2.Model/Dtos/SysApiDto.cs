using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysApiDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string ApiAccount { get; set; }
		public string ApiSecretKey { get; set; }
		public string ApiDescription { get; set; }
		public DateTime? ApiCreateTime { get; set; }
		public DateTime? ApiUpdateTime { get; set; }
		public int ApiAvailable { get; set; }
		public int ApiDeleted { get; set; }
		//public string ApiAttr1 { get; set; }
		//public string ApiAttr2 { get; set; }
		//public string ApiAttr3 { get; set; }
		//public string ApiAttr4 { get; set; }
	}
}
