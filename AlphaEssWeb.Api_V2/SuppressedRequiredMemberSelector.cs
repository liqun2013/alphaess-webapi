using System.Net.Http.Formatting;
using System.Reflection;

namespace AlphaEssWeb.Api_V2
{
	public class SuppressedRequiredMemberSelector : IRequiredMemberSelector
	{
		public bool IsRequiredMember(MemberInfo member)
		{
			return false;
		}
	}
}
