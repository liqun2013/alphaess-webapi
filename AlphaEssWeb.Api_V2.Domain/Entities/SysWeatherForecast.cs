namespace AlphaEssWeb.Api_V2.Domain
{
	using AlphaEss.Api_V2.Infrastructure;
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("SYS_WeatherForecast")]
	public partial class SysWeatherForecast : IEntity<Guid>
	{
		[Key]
		[Column("Weather_ID")]
		public Guid Key { get; set; }
		[StringLength(64)]
		public string SYS_SN { get; set; }
		public decimal? temp_min { get; set; }
		public decimal? temp_max { get; set; }
		public decimal? pressure { get; set; }
		public decimal? humidity { get; set; }
		[StringLength(50)]
		public string weather { get; set; }
		public decimal? clouds { get; set; }
		public decimal? wind_speed { get; set; }
		public DateTime? DATETIME { get; set; }
		[Column(TypeName = "ntext")]
		public string remark { get; set; }
		public DateTime? CREATEDATE { get; set; }
		public System.Guid CompanyId { get; set; }
		public decimal? wind_deg { get; set; }

		public string Latitude { get; set; }
		public string Longitude { get; set; }
	}
}

      