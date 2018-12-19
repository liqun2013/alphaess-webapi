namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	[Table("MicrogridAndSN")]
	public partial class MicrogridAndSN : IEntity<Guid>
	{
		[Key]
		[Column("Id")]
		public Guid Key { get; set; }

		public Guid MicrogridId { get; set; }

		[Required]
		[StringLength(32)]
		public string SysSn { get; set; }

		[StringLength(32)]
		public string SysType { get; set; }

		public int? MeterBoxBit { get; set; }

		public DateTime CreateTime { get; set; }

		public int? SubMicrogridId { get; set; }

		public int IsDelete { get; set; }
	}
}
