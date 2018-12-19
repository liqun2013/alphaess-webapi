﻿using System;
using System.ComponentModel.DataAnnotations;

namespace AlphaEssWeb.Api_V2.Model.ExternalRequestModels
{
	public class ExternalMicrogridControlCommandRequestModel: ExternalBaseRequestModel
	{
		[Required]
		public Guid MicrogridId { get; set; }
		[Required]
		public int Command { get; set; }
	}
}
