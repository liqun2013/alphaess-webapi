namespace AlphaEssWeb.Api_V2.Domain
{
	using System;
	using AlphaEss.Api_V2.Infrastructure;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("CustomerInfo")]
	public partial class CustomerInfo : IEntity<long>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public CustomerInfo()
		{
		}

		[Key]
		[Column("Id")]
		public long Key { get; set; }

		public long ComplaintId { get; set; }

		[Required]
		[StringLength(64)]
		public string CustomerName { get; set; }

		[StringLength(256)]
		public string CustomerAddress { get; set; }

		[StringLength(64)]
		public string CustomerContactNumber { get; set; }

		[StringLength(16)]
		public string CustomerPostcode { get; set; }

		[StringLength(64)]
		public string CustomerEmail { get; set; }

		[StringLength(40)]
		public string CustomerCountry { get; set; }

		public DateTime CreateTime { get; set; }

		public DateTime? LastUpdateTime { get; set; }

		[StringLength(64)]
		public string LastUpdateUser { get; set; }

		public short IsDelete { get; set; }

		[StringLength(64)]
		public string SysSn { get; set; }
		[StringLength(128)]
		public string ContactName { get; set; }
	}
}
