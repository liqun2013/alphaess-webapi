namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class SYS_APPVERSIONDETAIL : IEntity<Guid>
	{
		//[Key]
		//public Guid APPVERSIONDETAIL_ID { get; set; }
		[Key]
		[Column("APPVERSIONDETAIL_ID")]
		public Guid Key { get; set; }
		
		public Guid? APPVERSIONREC_ID { get; set; }

		public int? UPDATE_FLAG { get; set; }

		//[StringLength(250)]
		//public string ATTR1 { get; set; }

		//[StringLength(250)]
		//public string ATTR2 { get; set; }

		//[StringLength(250)]
		//public string ATTR3 { get; set; }

		//[StringLength(250)]
		//public string ATTR4 { get; set; }

		//[StringLength(250)]
		//public string ATTR5 { get; set; }

		[StringLength(64)]
		public string SN { get; set; }
		public Guid? CompanyId { get; set; }
	}
}
