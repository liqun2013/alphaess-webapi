namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sys_PurchasePrice : IEntity<Guid>
	{
		public Sys_PurchasePrice() {
		}
		[Key]
		[Column("PurchasePriceId")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(64)]
		public string PurchasePriceSn { get; set; }

		[Column(TypeName = "date")]
		public DateTime PurchasePriceDate { get; set; }
		public decimal? PurchasePriceMaxPrice { get; set; }
		public decimal? PurchasePriceMinPrice { get; set; }
		public DateTime? PurchasePriceCreateTime { get; set; }
		public int? PurchasePriceDeleteFlag { get; set; }
		public short? PurchasePriceEndTime { get; set; }
		public short? PurchasePriceBeginTime { get; set; }
		public decimal? PurchasePrice { get; set; }
	}
}
