using System.Collections.Generic;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class UserWithRoles
	{
		public SYS_USER User { get; set; }
		public IEnumerable<SYS_ROLE> Roles { get; set; }
	}

	public class UserWithRolesAndSystems: UserWithRoles
	{
		public IEnumerable<VT_SYSTEM> Systems { get; set; }
	}
}
