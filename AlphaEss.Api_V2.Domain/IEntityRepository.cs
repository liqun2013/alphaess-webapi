using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace AlphaEss.Api_V2.Infrastructure
{
	public interface IEntityRepository<T, S> where T : class, IEntity<S>, new()
	{
		IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties);
		IQueryable<T> All { get; }
		IQueryable<T> GetAll();
		//T GetSingle(S key);
		IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
		PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector);
		PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector,
			Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties);

		void Add(T entity);
		void AddBulk(IEnumerable<T> entity);
		void Delete(T entity);
		void Edit(T entity);
		void Save();
		List<T> ExecSql(string sql);
	}
}
