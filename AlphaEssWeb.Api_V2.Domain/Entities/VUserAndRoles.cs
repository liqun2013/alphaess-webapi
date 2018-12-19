namespace AlphaEssWeb.Api_V2.Domain
{
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	public partial class VUserAndRoles
	{
		[StringLength(50)]
		public string ROLENAME { get; set; }

		[Key]
		[Column(Order = 0)]
		public Guid ROLE_ID { get; set; }

		[Key]
		[Column(Order = 1)]
		public Guid USER_ID { get; set; }

		[StringLength(50)]
		public string USERTYPE { get; set; }

		[StringLength(50)]
		public string LICNO { get; set; }

		[StringLength(50)]
		public string USERNAME { get; set; }

		[StringLength(50)]
		public string REALNAME { get; set; }

		[StringLength(50)]
		public string USERPWD { get; set; }

		[StringLength(100)]
		public string EMAIL { get; set; }

		[StringLength(50)]
		public string COUNTRYCODE { get; set; }

		[StringLength(50)]
		public string STATECODE { get; set; }

		[StringLength(50)]
		public string CITYCODE { get; set; }

		[StringLength(50)]
		public string ADDRESS { get; set; }

		[StringLength(50)]
		public string POSTCODE { get; set; }

		[StringLength(50)]
		public string TIMEZONECODE { get; set; }

		[StringLength(50)]
		public string LINKMAN { get; set; }

		[StringLength(50)]
		public string CELLPHONE { get; set; }

		[StringLength(50)]
		public string FAX { get; set; }

		[Key]
		[Column(Order = 2)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int DELETE_FLAG { get; set; }

		[StringLength(100)]
		public string CREATE_ACCOUNT { get; set; }

		public DateTime? CREATE_DATETIME { get; set; }

		[StringLength(100)]
		public string LASTUPDATE_ACCOUNT { get; set; }

		public DateTime? LASTUPDATE_DATETIME { get; set; }

		[StringLength(64)]
		public string Inviter { get; set; }

		public int? Validity { get; set; }

		public DateTime? ShareTime { get; set; }

		[StringLength(256)]
		public string HeaderPath { get; set; }

		[Key]
		[Column(Order = 3)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int SERVICEAREA_FLAG { get; set; }

		[StringLength(50)]
		public string LANGUAGECODE { get; set; }

		[Key]
		[Column(Order = 4)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int LICINVALID_FLAG { get; set; }

		public Guid? CompanyId { get; set; }

		public int? IsInterAlpha { get; set; }

		[StringLength(96)]
		public string WXOpenId { get; set; }

		[StringLength(128)]
		public string WXUserNickName { get; set; }
	}
}
