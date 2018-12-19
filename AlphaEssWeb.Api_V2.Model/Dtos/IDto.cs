using System;

namespace AlphaEssWeb.Api_V2.Model.Dtos
{
	public interface IDto<T>
	{
		T Id { get; set; }
	}
}
