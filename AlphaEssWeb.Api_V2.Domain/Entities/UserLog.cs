namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class UserLog : IEntity<Guid>
	{
		[Key]
		[Column("LogId")]
		public Guid Key { get; set; }

        public Guid? CompanyId { get; set; }
        [StringLength(64)]
        public string SysSn { get; set; }
        [StringLength(1024)]
        public string LogContent { get; set; }
        public DateTime? CreateTime { get; set; }
        [StringLength(128)]
        public string LogFrom { get; set; }
        [StringLength(64)]
        public string LogType { get; set; }
        [StringLength(64)]
        public string Operator { get; set; }
        
	}
}
