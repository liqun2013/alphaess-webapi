using System.Security.Principal;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class ValidUserContext
	{
		public IPrincipal Principal { get; set; }
		public UserWithRoles User { get; set; }

		public bool IsValid()
		{
			return Principal != null;
		}
	}
}
