using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class BaseErrorCode : IDto<Guid>
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string Description { get; set; }
		public int DeleteFlag { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public DateTime? UpdateDatetime { get; set; }
	}
}
