using AlphaEssWeb.Api_V2.Domain;
using AlphaEssWeb.Api_V2.Model.Dtos;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEssWeb.Api_V2.Model
{
	internal static class SystemStateExtensions
	{
		public static SystemStateDto ToSystemStateDto(this SystemState ss)
		{
			return new SystemStateDto
			{
				NetWorkStatus = ss.NetWorkStatus,
				Sn = ss.Sn,
				State = ss.State,
				Minv = ss.Minv
			};
		}

		public static IEnumerable<SystemStateDto> ToCollectionSystemStateDto(this IEnumerable<SystemState> systemStates)
		{
			return systemStates.Select(x => x.ToSystemStateDto());
		}
  }
}
