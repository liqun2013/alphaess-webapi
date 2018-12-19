namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SysUser : IEntity<Guid>
	{
		[Key]
		[Column("ID")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(20)]
		public string LogUser { get; set; }

		[Required]
		[StringLength(20)]
		public string LogPwd { get; set; }

		[StringLength(10)]
		public string Role { get; set; }

		[StringLength(50)]
		public string Email { get; set; }

		[StringLength(20)]
		public string PhoneNo { get; set; }

		[StringLength(20)]
		public string RealName { get; set; }

		[StringLength(50)]
		public string Address { get; set; }

		public DateTime? CreateTime { get; set; }

		public int? Delete_Flag { get; set; }
	}
}
