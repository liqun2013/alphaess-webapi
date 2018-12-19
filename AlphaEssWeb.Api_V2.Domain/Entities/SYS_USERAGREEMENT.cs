namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_USERAGREEMENT : IEntity<Guid>
	{
		[Key]
		//[Column(Order = 0)]
		//public Guid Agreement_Id { get; set; }
		[Column("Agreement_Id")]
		public Guid Key { get; set; }

		[StringLength(16)]
		public string Agreement_Language { get; set; }

		public string Agreement_Content { get; set; }

		public DateTime? CreateTime { get; set; }

		public DateTime? UpdateTime { get; set; }
		public Guid? CompanyId { get; set; }
	}
}
