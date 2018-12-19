using AlphaEss.Api_V2.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;

namespace AlphaEssWeb.Api_V2.Domain
{
	public class EntityRepository<T, S> : IEntityRepository<T, S> where T : class, IEntity<S>, new()
	{
		protected readonly AlphaEssDbContext _entitiesContext;
		public EntityRepository(AlphaEssDbContext entitiesContext)
		{
			if (entitiesContext == null)
			{
				throw new ArgumentNullException("entitiesContext");
			}
			_entitiesContext = entitiesContext;
			_entitiesContext.Database.CommandTimeout = 300;

		}
		public virtual IQueryable<T> GetAll()
		{
			return _entitiesContext.Set<T>();
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
			IQueryable<T> query = _entitiesContext.Set<T>();
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
			return query;
		}
		//public T GetSingle(S key)
		//{
		//	return GetAll().FirstOrDefault(x => x.Key == key);
		//}
		public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			return _entitiesContext.Set<T>().Where(predicate);
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
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			_entitiesContext.Set<T>().Add(entity);
		}
		public virtual void Edit(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}
		public virtual void Save()
		{
			_entitiesContext.SaveChanges();
		}

		public virtual List<T> ExecSql(string sql)
		{
			var data = _entitiesContext.Database.SqlQuery<T>(sql);
			if (data != null && data.Any())
				return data.ToList<T>();
			else
				return null;
		}

		public void AddBulk(IEnumerable<T> collection)
		{
			_entitiesContext.Set<T>().AddRange(collection);
		}

		//public virtual AlphaEssDbContext AlphaEssDbContext
		//{
		//	get
		//	{
		//		return (AlphaEssDbContext)_entitiesContext;
		//	}
		//}
	}


	//public class EntityMeterRepository<T, S> : IEntityRepository<T, S> where T : class, IEntity<S>, new()
	//{
	//	readonly RemoteMeterContext _entitiesContext;
	//	public EntityMeterRepository(RemoteMeterContext entitiesContext)
	//	{
	//		if (entitiesContext == null)
	//		{
	//			throw new ArgumentNullException("entitiesContext");
	//		}
	//		_entitiesContext = entitiesContext;
	//		_entitiesContext.Database.CommandTimeout = 20;

	//	}
	//	public virtual IQueryable<T> GetAll()
	//	{
	//		return _entitiesContext.Set<T>();
	//	}
	//	public virtual IQueryable<T> All
	//	{
	//		get
	//		{
	//			return GetAll();
	//		}
	//	}
	//	public virtual IQueryable<T> AllIncluding(params Expression<Func<T, object>>[] includeProperties)
	//	{
	//		IQueryable<T> query = _entitiesContext.Set<T>();
	//		foreach (var includeProperty in includeProperties)
	//		{
	//			query = query.Include(includeProperty);
	//		}
	//		return query;
	//	}
	//	//public T GetSingle(S key)
	//	//{
	//	//	return GetAll().FirstOrDefault(x => x.Key == key);
	//	//}
	//	public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
	//	{
	//		return _entitiesContext.Set<T>().Where(predicate);
	//	}
	//	public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector)
	//	{
	//		return Paginate(pageIndex, pageSize, keySelector, null);
	//	}
	//	public virtual PaginatedList<T> Paginate<TKey>(int pageIndex, int pageSize, Expression<Func<T, TKey>> keySelector,
	//		Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
	//	{
	//		IQueryable<T> query =
	//		AllIncluding(includeProperties).OrderBy(keySelector);
	//		query = (predicate == null)
	//		? query
	//		: query.Where(predicate);
	//		return query.ToPaginatedList(pageIndex, pageSize);
	//	}
	//	public virtual void Add(T entity)
	//	{
	//		DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
	//		_entitiesContext.Set<T>().Add(entity);
	//	}
	//	public virtual void Edit(T entity)
	//	{
	//		DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
	//		dbEntityEntry.State = EntityState.Modified;
	//	}
	//	public virtual void Delete(T entity)
	//	{
	//		DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
	//		dbEntityEntry.State = EntityState.Deleted;
	//	}
	//	public virtual void Save()
	//	{
	//		_entitiesContext.SaveChanges();
	//	}

	//	public virtual List<T> ExecSql(string sql)
	//	{
	//		var data = _entitiesContext.Database.SqlQuery<T>(sql);
	//		if (data != null && data.Any())
	//			return data.ToList<T>();
	//		else
	//			return null;
	//	}

	//	public void AddBulk(IEnumerable<T> collection)
	//	{
	//		_entitiesContext.Set<T>().AddRange(collection);
	//	}

	//	//public virtual AlphaEssDbContext AlphaEssDbContext
	//	//{
	//	//	get
	//	//	{
	//	//		return (AlphaEssDbContext)_entitiesContext;
	//	//	}
	//	//}
	//}

	public class EntityPowerDataDbRepository<T, S> : IEntityRepository<T, S> where T : class, IEntity<S>, new()
	{
		readonly AlphaEssDb_PowerDataDbContext _entitiesContext;
		public EntityPowerDataDbRepository(AlphaEssDb_PowerDataDbContext entitiesContext)
		{
			if (entitiesContext == null)
			{
				throw new ArgumentNullException("entitiesContext");
			}
			_entitiesContext = entitiesContext;
			_entitiesContext.Database.CommandTimeout = 300;

		}
		public virtual IQueryable<T> GetAll()
		{
			return _entitiesContext.Set<T>();
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
			IQueryable<T> query = _entitiesContext.Set<T>();
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
			return _entitiesContext.Set<T>().Where(predicate);
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
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			_entitiesContext.Set<T>().Add(entity);
		}
		public virtual void Edit(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}
		public virtual void Save()
		{
			_entitiesContext.SaveChanges();
		}

		public void AddBulk(IEnumerable<T> collection)
		{
			_entitiesContext.Set<T>().AddRange(collection);
		}

		public virtual List<T> ExecSql(string sql)
		{
			var data = _entitiesContext.Database.SqlQuery<T>(sql);
			if (data != null && data.Any())
				return data.ToList<T>();
			else
				return null;
		}

		//public virtual AlphaEssDb_PowerDataDbContext AlphaEssDbContext
		//{
		//	get
		//	{
		//		return (AlphaEssDb_PowerDataDbContext)_entitiesContext;
		//	}
		//}
	}

	public class EntityLogDbRepository<T, S> : IEntityRepository<T, S> where T : class, IEntity<S>, new()
	{
		readonly AlphaEssDb_LogDbContext _entitiesContext;
		public EntityLogDbRepository(AlphaEssDb_LogDbContext entitiesContext)
		{
			if (entitiesContext == null)
			{
				throw new ArgumentNullException("entitiesContext");
			}
			_entitiesContext = entitiesContext;
			_entitiesContext.Database.CommandTimeout = 300;

		}
		public virtual IQueryable<T> GetAll()
		{
			return _entitiesContext.Set<T>();
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
			IQueryable<T> query = _entitiesContext.Set<T>();
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
			return _entitiesContext.Set<T>().Where(predicate);
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
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			_entitiesContext.Set<T>().Add(entity);
		}
		public virtual void Edit(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}
		public virtual void Save()
		{
			_entitiesContext.SaveChanges();
		}

		public void AddBulk(IEnumerable<T> collection)
		{
			_entitiesContext.Set<T>().AddRange(collection);
		}

		public virtual List<T> ExecSql(string sql)
		{
			var data = _entitiesContext.Database.SqlQuery<T>(sql);
			if (data != null && data.Any())
				return data.ToList<T>();
			else
				return null;
		}

		//public virtual AlphaEssDb_PowerDataDbContext AlphaEssDbContext
		//{
		//	get
		//	{
		//		return (AlphaEssDb_PowerDataDbContext)_entitiesContext;
		//	}
		//}
	}

	public class ComplaintsProcessingEntityRepository<T, S> : IEntityRepository<T, S> where T : class, IEntity<S>, new()
	{
		protected readonly AlphaComplaintsProcessingDbContext _entitiesContext;
		public ComplaintsProcessingEntityRepository(AlphaComplaintsProcessingDbContext entitiesContext)
		{
			if (entitiesContext == null)
			{
				throw new ArgumentNullException("entitiesContext");
			}
			_entitiesContext = entitiesContext;
			_entitiesContext.Database.CommandTimeout = 300;

		}
		public virtual IQueryable<T> GetAll()
		{
			return _entitiesContext.Set<T>();
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
			IQueryable<T> query = _entitiesContext.Set<T>();
			foreach (var includeProperty in includeProperties)
			{
				query = query.Include(includeProperty);
			}
			return query;
		}
		//public T GetSingle(S key)
		//{
		//	return GetAll().FirstOrDefault(x => x.Key == key);
		//}
		public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			return _entitiesContext.Set<T>().Where(predicate);
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
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			_entitiesContext.Set<T>().Add(entity);
		}
		public virtual void Edit(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Modified;
		}
		public virtual void Delete(T entity)
		{
			DbEntityEntry dbEntityEntry = _entitiesContext.Entry<T>(entity);
			dbEntityEntry.State = EntityState.Deleted;
		}
		public virtual void Save()
		{
			_entitiesContext.SaveChanges();
		}

		public virtual List<T> ExecSql(string sql)
		{
			var data = _entitiesContext.Database.SqlQuery<T>(sql);
			if (data != null && data.Any())
				return data.ToList<T>();
			else
				return null;
		}

		public void AddBulk(IEnumerable<T> collection)
		{
			_entitiesContext.Set<T>().AddRange(collection);
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
