namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("SYS_Country_Region")]
	public partial class SYS_Country_Region : IEntity<Guid>
	{
		[Key]
		[Column("Area_ID")]
		public System.Guid Key { get; set; }
		[StringLength(50)]
		[Column("Area_EnglishName")]
		public string AreaEnglishName { get; set; }
		[StringLength(50)]
		[Column("Area_FirstName")]
		public string AreaFirstName { get; set; }
		[Column("Area_Grade")]
		public int AreaGrade { get; set; }
		[StringLength(64)]
		[Column("Area_UpperLevelId")]
		public string AreaUpperLevelId { get; set; }
		[Column("DELETE_FLAG")]
		public int Delete_Flag { get; set; }
		[Column("CREATE_DATETIME")]
		public DateTime? CreateDatetime { get; set; }
		[Column("UPDATE_DATETIME")]
		public DateTime? UpdateDatetime { get; set; }
		[NotMapped]
		public int? baseCode { get; set; }
	}
}
