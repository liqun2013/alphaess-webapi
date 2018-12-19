namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("ElectricDispatchingControlRecord")]
	public partial class ElectricDispatchingControlRecord : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

		public Guid MicrogridId { get; set; }

		[StringLength(32)]
		public string SysSn { get; set; }

		[StringLength(64)]
		public string CommandId { get; set; }

		public decimal? ControlPower { get; set; }

		public DateTime? SendTime { get; set; }

		public DateTime CreateTime { get; set; }
	}
}
