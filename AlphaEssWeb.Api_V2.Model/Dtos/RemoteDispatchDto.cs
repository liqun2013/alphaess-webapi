using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class RemoteDispatchDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string UserName { get; set; }
		public string SN { get; set; }
		public int ActivePower { get; set; }
		public int ReactivePower { get; set; }
		public decimal SOC { get; set; }
		public int Status { get; set; }
		public int ControlMode { get; set; }
		public int DELETE_FLAG { get; set; }
		public DateTime? CreateTime { get; set; }
		public DateTime? UpdateTime { get; set; }
	}
}
