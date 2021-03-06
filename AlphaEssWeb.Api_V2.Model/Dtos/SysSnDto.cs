﻿using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class SysSnDto : IDto<Guid>
	{
		public Guid Id { get; set; }
		public int TypeId { get; set; }
		public string SnNo { get; set; }
		public string Description { get; set; }
		public int DeleteFlag { get; set; }
		public string CreateAccount { get; set; }
		public DateTime? CreateDatetime { get; set; }
		public string UpdateAccount { get; set; }
		public DateTime? UpdateDatetime { get; set; }
	}
}
