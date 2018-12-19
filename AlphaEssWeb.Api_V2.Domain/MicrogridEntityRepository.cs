using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Data.SqlClient;
using AlphaEss.Api_V2.Infrastructure;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class MicrogridEntityRepository<T, S> : IEntityRepository<T, S> where T : class, IEntity<S>, new()
	{
		readonly AlphaMicrogridDbContext _microgridEntitiesContext;
		public MicrogridEntityRepository(AlphaMicrogridDbContext entitiesContext)
		{
			if (entitiesContext == null)
			{
				throw new ArgumentNullException("entitiesContext");
			}
			_microgridEntitiesContext = entitiesContext;
			_microgridEntitiesContext.Database.CommandTimeout = 20;
			
		}
		public virtual IQueryable<T> GetAll()
		{
			return _microgridEntitiesContext.Set<T>();
		}
		public virtual IQueryable<T> All
		{
			get
			{
				return GetAll();
			}
		}
		public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query = _microgridEntitiesContext.Set<T>();
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
			return query;
		}
		//public T GetSingle(Guid key)
		//{
		//	return GetAll().FirstOrDefault(x => x.Key == key);
		//}
		public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			return _microgridEntitiesContext.Set<T>().Where(predicate);
		}
		public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
		{
			return Paginate(pageIndex, pageSize, keySelector, null);
		}
		public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector,
			Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
		{
			IQueryable<T> query =
			AllIncluding(includeProperties).OrderBy(keySelector);
			query = (predicate == null)
			? query
			: query.Where(predicate);
			return query.ToPaginatedList(pageIndex, pageSize);
		}
		public virtual void Add(T entity)
		{
			DbEntityEntry dbEntityEntry = _microgridEntitiesContext.Entry<T>(entity);
			_microgridEntitiesContext.Set<T>().Add(entity);
		}
		public virtual void Edit(T entity)
		{
			DbEntityEntry dbEntityEntry = _microgridEntitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = _microgridEntitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}
		public virtual void Save()
		{
			_microgridEntitiesContext.SaveChanges();
		}

		public List<T> ExecSql(string sql)
		{
			throw new NotImplementedException();
		}

		public void AddBulk(IEnumerable<T> entity)
		{
			throw new NotImplementedException();
		}

		//public virtual AlphaEssDbContext AlphaEssDbContext
		//{
		//	get
		//	{
		//		return (AlphaEssDbContext)_entitiesContext;
		//	}
		//}
	}
}
