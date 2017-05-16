using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Data;
using System.Linq.Expressions;

namespace DataModel
{
	/// <summary>
	/// Generic class for basic entity operations
	/// </summary>
	/// <typeparam name="TEntity"></typeparam>
	public class GenericRepository<TEntity> where TEntity : class
	{
		internal TaskControlEntities Context;
		internal DbSet<TEntity> dbSet;
		private readonly bool _lazyLoadingEnabled = true;

		public GenericRepository(TaskControlEntities context)
		{
			this.Context = context;
			this.Context.Configuration.LazyLoadingEnabled = _lazyLoadingEnabled;
			this.dbSet = context.Set<TEntity>();
		}

		#region public member methods
		public virtual IEnumerable<TEntity> Get()
		{
			IEnumerable<TEntity> query = dbSet;
			return query.ToList();
		}


		public virtual TEntity GetByID(long Id)
		{
			return dbSet.Find(Id);
		}

		public virtual void Insert(TEntity entity)
		{
			dbSet.Add(entity);
		}

		public virtual void Delete(long Id)
		{
			TEntity entityToDelete = dbSet.Find(Id);
			Delete(entityToDelete);
		}

		public virtual void Delete(TEntity entity)
		{
			if (Context.Entry(entity).State == EntityState.Detached)
			{
				dbSet.Attach(entity);
			}

			dbSet.Remove(entity);
		}

		public virtual void Update(TEntity entityToUpdate)
		{
			dbSet.Attach(entityToUpdate);
			Context.Entry(entityToUpdate).State = EntityState.Modified;
		}

		/// <summary>
		/// generic method that gets many records on the basis of condition
		/// </summary>
		/// <param name="where"></param>
		/// <returns></returns>
		public virtual IEnumerable<TEntity> GetMany(Func<TEntity, bool> where)
		{
			return dbSet.Where(where).ToList();
		}


		public virtual IEnumerable<TEntity> GetManyQueryable(Func<TEntity, bool> where)
		{
			return dbSet.Where(where).AsQueryable();
		}

		/// <summary>
		/// fetches data for the entities on the basis of condition
		/// </summary>
		/// <param name="where"></param>
		/// <returns></returns>
		public virtual IEnumerable<TEntity> Get(
					 Expression<Func<TEntity, bool>> filter = null,
					 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
					 string includeProperties = "")
		{
			IQueryable<TEntity> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			foreach (var includeProperty in includeProperties.Split
					(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
			{
				query = query.Include(includeProperty);
			}

			if (orderBy != null)
			{
				return orderBy(query).ToList();
			}
			else
			{
				return query.ToList();
			}
		}


		/// <summary>
		/// deletes data for the entities on the basis fo condition
		/// </summary>
		/// <param name="where"></param>
		public void Delete(Func<TEntity, Boolean> where)
		{
			IEnumerable<TEntity> objects = dbSet.Where<TEntity>(where).AsQueryable();
			foreach(TEntity obj in objects)
			{
				dbSet.Remove(obj);
			}

		}

		/// <summary>
		/// fetch all records from the db
		/// </summary>
		/// <returns></returns>

		public virtual IEnumerable<TEntity> GetAll()
		{
			return dbSet.ToList();
		}

		/// <summary>
		/// incldue multiple
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="include"></param>
		/// <returns></returns>
		//public IEnumerable<TEntity> GetWithInclude(System.Linq.Expressions.Expression<Func<TEntity, bool>> predicate, params string[] include)
		//{
		//	IEnumerable<TEntity> query = this.DbSet;
		//	query = include.Aggregate(query, (current, inc) => current.Include(inc));
		//	return query.Where(predicate);
		//}

		/// <summary>
		/// generic method to check if entity exists
		/// </summary>
		/// <param name="primaryKey"></param>
		/// <returns></returns>
		public bool Exists(long primaryKey)
		{
			return dbSet.Find(primaryKey) != null;
		}

		/// <summary>
		/// get a single record by the specified criteria (usually the unique identifier)
		/// </summary>
		/// <param name="predicate">Criteria to match on</param>
		/// <returns></returns>
		public TEntity GetSingle(Func<TEntity, bool> predicate)
		{
			return dbSet.Single<TEntity>(predicate);
		}


		/// <summary>
		/// The first record matching the specified criteria
		/// </summary>
		/// <param name="predicate">Criteria to match on</param>
		/// <returns></returns>

		public TEntity GetFirst(Func<TEntity, bool> predicate)
		{
			return dbSet.First<TEntity>(predicate);
		}

		#endregion
	}
}
