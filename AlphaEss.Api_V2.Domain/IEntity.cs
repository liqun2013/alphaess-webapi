using System;

namespace AlphaEss.Api_V2.Infrastructure
{
	public interface IEntity<T>
	{
		T Key { get; set; }
	}
}
