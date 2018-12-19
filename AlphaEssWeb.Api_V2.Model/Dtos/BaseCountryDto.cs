using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class BaseCountryDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string AreaCode { get; set; }
		public string AreaName { get; set; }
		public int ParentId{ get; set; }
		public int DeleteFlag { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public DateTime? UpdateDatetime { get; set; }
	}
}
