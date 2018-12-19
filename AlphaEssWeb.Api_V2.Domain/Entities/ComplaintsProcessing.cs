namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("ComplaintsProcessing")]
	public partial class ComplaintsProcessing : IEntity<long>
	{
		[Key]
		[Column("Id")]
		public long Key { get; set; }

		public long ComplaintId { get; set; }

		//[Column(TypeName = "datetime2")]
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime ProcessingTime { get; set; }

		[StringLength(64)]
		public string OriginalStatus { get; set; }

		[StringLength(64)]
		public string CurrentStatus { get; set; }

		[StringLength(2048)]
		public string Reason { get; set; }

		[StringLength(2048)]
		public string Solution { get; set; }

		[StringLength(64)]
		public string Processor { get; set; }

		//[Column(TypeName = "datetime2")]
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime? ProcessingEndTime { get; set; }

		[StringLength(64)]
		public string ComplaintType { get; set; }

		public int? ProcessingPriority { get; set; }

		public int? ProcessFlowNumber { get; set; }

		[StringLength(400)]
		public string Attachment { get; set; }
		[StringLength(400)]
		public string Attachment2 { get; set; }
		[StringLength(400)]
		public string Attachment3 { get; set; }
		[StringLength(64)]
		public string OnsiteHandler { get; set; }
		public short? AllowToGoToClientsHome { get; set; }
		public virtual Complaints Complaints { get; set; }
	}
}
