using System;
using System.Collections.Generic;
using System.Linq;

namespace AlphaEss.Api_V2.Infrastructure
{
	public class PaginatedList<T> : List<T>
	{
		public int PageIndex { get; private set; }
		public int PageSize { get; private set; }
		public int TotalCount { get; set; }
		public int TotalPageCount { get; set; }
		public PaginatedList(int pageIndex, int pageSize)
		{
			PageIndex = pageIndex;
			PageSize = pageSize;
			TotalCount = 0;
			TotalPageCount = 0;
		}
		public PaginatedList(int pageIndex, int pageSize, IQueryable<T> source)
		{
			if (source != null)
			{
				PageIndex = pageIndex;
				PageSize = pageSize;
				TotalCount = source.Count();
				TotalPageCount = (int)Math.Ceiling(TotalCount / (double)pageSize);
				AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize));
			}
			else
			{
				PageIndex = 1;
				PageSize = 10;
				TotalCount = 0;
				TotalPageCount = 0;
			}
		}
		public PaginatedList(int pageIndex, int pageSize, IList<T> source)
		{
			if (source != null)
			{
				PageIndex = pageIndex;
				PageSize = pageSize;
				TotalCount = source.Count();
				TotalPageCount = (int)Math.Ceiling(TotalCount / (double)pageSize);
				AddRange(source.Skip((pageIndex - 1) * pageSize).Take(pageSize));
			}
			else
			{
				PageIndex = 1;
				PageSize = 10;
				TotalCount = 0;
				TotalPageCount = 0;
			}
		}
		public PaginatedList(int pageIndex, int pageSize, int total, IList<T> source)
		{
			if (source != null)
			{
				PageIndex = pageIndex;
				PageSize = pageSize;
				TotalCount = total;
				TotalPageCount = (int)Math.Ceiling(TotalCount / (double)pageSize);
				AddRange(source);
			}
			else
			{
				PageIndex = 1;
				PageSize = 10;
				TotalCount = 0;
				TotalPageCount = 0;
			}
		}
		public bool HasPreviousPage
		{
			get
			{
				return (PageIndex > 1);
			}
		}
		public bool HasNextPage
		{
			get
			{
				return (PageIndex < TotalPageCount);
			}
		}
	}
}
