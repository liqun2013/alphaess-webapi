namespace AlphaEssWeb.Api_V2.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using AlphaEss.Api_V2.Infrastructure;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class CustomerReviews : IEntity<long>
	{
		[Key]
		[Column("Id")]
		public long Key { get; set; }

		public long ComplaintId { get; set; }

		public int Satisfaction { get; set; }
		public int? Satisfaction1 { get; set; }
		public int? Satisfaction2 { get; set; }
		public int? Satisfaction3 { get; set; }
		public int? Satisfaction4 { get; set; }

		[StringLength(1024)]
		public string Content { get; set; }

		public int IsDelete { get; set; }

		//[Column(TypeName = "datetime2")]
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CreateTime { get; set; }

		//[Column(TypeName = "datetime2")]
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? UpdateTime { get; set; }

		public virtual Complaints Complaints { get; set; }
	}
}
