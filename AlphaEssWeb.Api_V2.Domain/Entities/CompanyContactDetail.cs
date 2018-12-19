using AlphaEss.Api_V2.Infrastructure;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AlphaEssWeb.Api_V2.Domain
{
	[Table("CompanyContactDetail")]
	public partial class CompanyContactDetail : IEntity<long>
	{
		[Key]
		[Column("Id")]
		public long Key { get; set; }
		[StringLength(128)]
		public string CompanyName { get; set; }
		[StringLength(32)]
		public string ContactNumber1 { get; set; }
		[StringLength(32)]
		public string ContactNumber2 { get; set; }
		[StringLength(64)]
		public string Email { get; set; }
		[StringLength(512)]
		public string ContactAddress { get; set; }
		[StringLength(64)]
		public string WebSite { get; set; }
		[StringLength(512)]
		public string Remark { get; set; }
		public int? DisplayOrder { get; set; }
	}
}
