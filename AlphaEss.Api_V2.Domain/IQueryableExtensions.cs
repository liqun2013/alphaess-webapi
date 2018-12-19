using System.Collections.Generic;
using System.Linq;

namespace AlphaEss.Api_V2.Infrastructure
{
	public static class IQueryableExtensions
	{
		/// <summary>
		///  converts a collection of objects to a PaginatedList object
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="query"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public static PaginatedList<T> ToPaginatedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
		{
			//var collection = query.Skip((pageIndex - 1) * pageSize).Take(pageSize);

			return new PaginatedList<T>(pageIndex, pageSize, query);
		}

		public static PaginatedList<T> ToPaginatedList<T>(this IList<T> list, int pageIndex, int pageSize)
		{
			return new PaginatedList<T>(pageIndex, pageSize, list);
		}

		public static PaginatedList<T> ToPaginatedList<T>(this IList<T> list, int pageIndex, int pageSize, int total)
		{
			return new PaginatedList<T>(pageIndex, pageSize, total, list);
		}
	}
}
