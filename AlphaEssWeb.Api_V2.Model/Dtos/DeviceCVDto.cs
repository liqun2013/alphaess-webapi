using System;
using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public sealed class DeviceCVDto 
	{
		public string[] Time { get; set; }
		public decimal[] CVTotal { get; set; }
		public decimal[] CV1 { get; set; }
		public decimal[] CV2 { get; set; }
		public decimal[] CV3 { get; set; }

	}
}
