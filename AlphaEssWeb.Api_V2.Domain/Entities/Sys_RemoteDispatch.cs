namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;


	public partial class Sys_RemoteDispatch : IEntity<Guid>
	{
		[Key]
		[Column("Dispatch_ID")]
		public Guid Key { get; set; }

		[StringLength(50)]
		public string UserName { get; set; }

		[StringLength(64)]
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
