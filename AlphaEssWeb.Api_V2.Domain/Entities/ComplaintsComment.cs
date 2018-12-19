namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("ComplaintsComment")]
	public partial class ComplaintsComment : IEntity<long>
	{
		[Key]
		[Column("Id")]
		public long Key { get; set; }
		public long ComplaintId { get; set; }
		[StringLength(512)]
		public string CommentContent { get; set; }
		//[Column(TypeName = "datetime2")]
		//[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
		public DateTime CommentTime { get; set; }
		public string CommentCreator { get; set; }
		public long ReplyFrom { get; set; }
		public int IsDelete { get; set; }
		[StringLength(4000)]
		public string Attachment { get; set; }
		public virtual Complaints Complaints { get; set; }
	}
}
