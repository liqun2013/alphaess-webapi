namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("ComplaintsProcessingAdditionalInfo")]
	public partial class ComplaintsProcessingAdditionalInfo : IEntity<long>
	{
		[Key]
		[Column("Id")]
		public long Key { get; set; }

		public long ComplaintProcessingId { get; set; }

		[StringLength(4000)]
		public string AdditionalInfoContent { get; set; }

		[StringLength(400)]
		public string Attachment { get; set; }

		public DateTime? CreateTime { get; set; }

		public short? IsDelete { get; set; }
	}
}
