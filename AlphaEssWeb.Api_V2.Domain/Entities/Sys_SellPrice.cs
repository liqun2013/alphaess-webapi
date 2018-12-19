namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class Sys_SellPrice : IEntity<Guid>
	{
		[Key]
		[Column("SellPriceId")]
		public Guid Key { get; set; }

		[Required]
		[StringLength(64)]
		public string SellPriceSn { get; set; }

		[Column(TypeName = "date")]
		public DateTime SellPriceDate { get; set; }

		public decimal? SellPricePrice { get; set; }

		public DateTime? SellPriceCreateTime { get; set; }

		public int? SellPriceDeleteFlag { get; set; }
	}
}
