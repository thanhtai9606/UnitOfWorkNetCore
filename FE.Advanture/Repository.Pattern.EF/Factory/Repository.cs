using Microsoft.EntityFrameworkCore;
using Repository.Pattern.EF.DataContext;
using Repository.Pattern.EF.Repositories;
using Repository.Pattern.EF.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Pattern.EF.Factory
{
    public class Repository<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        #region Private Fields

        private readonly DataContext _context;
        private readonly DbSet<TEntity> _dbSet;
        private readonly IUnitOfWorkAsync _unitOfWork;

        #endregion Private Fields
        public Repository(IDataContextAsync context, IUnitOfWorkAsync unitOfWork)
        {
            _context = context as DataContext;
            _unitOfWork = unitOfWork;
            if (_context != null)
            {
                _dbSet = _context.Set<TEntity>();
            }
        }
        public virtual void Add(TEntity entity)
        {
            _dbSet.Add(entity);
            _unitOfWork.SyncObjectState(entity);
        }
        public virtual void AddRange(IEnumerable<TEntity> entities)=> _dbSet.AddRange(entities);
        public virtual void Delete(object id)
        {
            var entity = _dbSet.Find(id);
            Delete(entity);
        }
        public virtual void Delete(TEntity entity)=> Remove(entity);
        public virtual async Task<bool> DeleteAsync(params object[] keyValues) => await DeleteAsync(CancellationToken.None, keyValues);
        public virtual async Task<bool> DeleteAsync(CancellationToken cancellationToken, params object[] keyValues)
        {
            var entity = await FindAsync(cancellationToken, keyValues);
            if (entity == null)
                return false;
            else
            {
                Remove(entity);
                return true;
            }           
        }
        public TEntity Find(params object[] keyValues)=> _dbSet.Find(keyValues);
        public virtual async Task<TEntity> FindAsync(params object[] keyValues)=> await _dbSet.FindAsync(keyValues);
        public virtual async Task<TEntity> FindAsync(CancellationToken cancellationToken, params object[] keyValues)=> await _dbSet.FindAsync(cancellationToken, keyValues);
        public virtual IEnumerable<TEntity> FindBy(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _dbSet.Where(predicate);
            }
            catch (Exception e)
            {
                //Loger.Error(e);
                throw new Exception(e.Message);
            }
        }
        public virtual IRepository<T> GetRepository<T>() where T : class=> _unitOfWork.Repository<T>();
        public virtual IQueryFluent<TEntity> Query(IQueryObject<TEntity> queryObject)=> new QueryFluent<TEntity>(this, queryObject);
        public virtual IQueryFluent<TEntity> Query(Expression<Func<TEntity, bool>> query)=> new QueryFluent<TEntity>(this, query);
        public virtual IQueryFluent<TEntity> Query() => new QueryFluent<TEntity>(this);
        public virtual IQueryable<TEntity> Queryable() => _dbSet;
        public virtual void Remove(TEntity entity)
        {
            _dbSet.Remove(entity);
            _unitOfWork.SyncObjectState(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                Remove(entity);
            }
        }

        public virtual IQueryable<TEntity> SelectQuery(string query, params object[] parameters) => _dbSet.FromSql(query, parameters).AsQueryable();
        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
            _unitOfWork.SyncObjectState(entity);
        }

        internal IQueryable<TEntity> Select(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          List<Expression<Func<TEntity, object>>> includes = null,
          int? page = null,
          int? pageSize = null)
        {
            IQueryable<TEntity> query = _dbSet;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.AsQueryable().Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }

        internal async Task<IEnumerable<TEntity>> SelectAsync(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null)
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }
    }
}
