namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;
	using System.Data.Entity.Spatial;

	public partial class Complaints : IEntity<long>
	{
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Complaints()
		{
			ComplaintsProcessing = new HashSet<ComplaintsProcessing>();
			CustomerReviews = new HashSet<CustomerReviews>();
		}

		[Key]
		[Column("Id")]
		public long Key { get; set; }

		[Required]
		[StringLength(128)]
		public string Title { get; set; }

		[StringLength(1024)]
		public string Description { get; set; }

		//[Column(TypeName = "datetime2")]
		public DateTime CreateTime { get; set; }

		//[Column(TypeName = "datetime2")]
		public DateTime? UpdateTime { get; set; }

		[Required]
		[StringLength(64)]
		public string Creator { get; set; }

		public int Status { get; set; }

		[StringLength(64)]
		public string Recipient { get; set; }

		//[Column(TypeName = "datetime2")]
		public DateTime? ReceiveTime { get; set; }

		[StringLength(64)]
		public string CurrentProcessor { get; set; }

		[StringLength(64)]
		public string Area { get; set; }

		[StringLength(400)]
		public string Attachment { get; set; }
		[StringLength(400)]
		public string Attachment2 { get; set; }
		[StringLength(400)]
		public string Attachment3 { get; set; }

		public int IsDelete { get; set; }

		[StringLength(64)]
		public string SysSn { get; set; }
		[StringLength(64)]
		public string OnsiteHandler { get; set; }
		[StringLength(96)]
		public string SystemLicense { get; set; }
		[StringLength(16)]
		public string SystemPostcode { get; set; }
		[StringLength(32)]
		public string SystemMinv { get; set; }
		[StringLength(64)]
		public string ComplaintsType { get; set; }
		[StringLength(64)]
		public string Email { get; set; }
		[StringLength(16)]
		public string ContactNumber { get; set; }
		public int? ProcessingPriority { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ComplaintsProcessing> ComplaintsProcessing { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<CustomerReviews> CustomerReviews { get; set; }
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<ComplaintsComment> ComplaintsComment { get; set; }

		[NotMapped]
		public string StrStatus { get; set; }
		[NotMapped]
		public string StrComplaintsType { get; set; }
	}
}
