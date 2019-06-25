using CommonServiceLocator;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Repository.Pattern.EF.DataContext;
using Repository.Pattern.EF.Repositories;
using Repository.Pattern.EF.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Pattern.EF.Factory
{
    public class UnitOfWork : IUnitOfWorkAsync
    {
        #region Private Fields
        protected readonly DataContext _dataContext;
        private bool _disposed;
        private Dictionary<string, dynamic> _repositories;
        private IsolationLevel? _isolationLevel;
        private IDbContextTransaction _transaction;
        #endregion Private Fields

        #region Constuctor/Dispose
        public UnitOfWork(IDataContextAsync dataContext)
        {
            _dataContext = dataContext as DataContext;
            _repositories = new Dictionary<string, dynamic>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                try
                {
                    var connection = _dataContext.Database.GetDbConnection();
                    if (connection != null && connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                catch (ObjectDisposedException)
                {
                    // 
                }

                if (_dataContext != null)
                {
                    _dataContext.Dispose();
                }
            }
            _disposed = true;
        }

        #endregion Constuctor/Dispose

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepository<TEntity>>();
            }

            return RepositoryAsync<TEntity>();
        }

        public IRepositoryAsync<TEntity> RepositoryAsync<TEntity>() where TEntity : class
        {
            if (ServiceLocator.IsLocationProviderSet)
            {
                return ServiceLocator.Current.GetInstance<IRepositoryAsync<TEntity>>();
            }

            if (_repositories == null)
            {
                _repositories = new Dictionary<string, dynamic>();
            }

            var type = typeof(TEntity).Name;

            if (_repositories.ContainsKey(type))
            {
                return (IRepositoryAsync<TEntity>)_repositories[type];
            }

            var repositoryType = typeof(Repository<>);

            _repositories.Add(type, Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _dataContext, this));

            return _repositories[type];
        }

        public int SaveChanges() => _dataContext.SaveChanges();
        public Task<int> SaveChangesAsync()=> _dataContext.SaveChangesAsync();      

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken)=> _dataContext.SaveChangesAsync(cancellationToken);
        #region Unit of Work Transactions

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified)
        {
            if (_transaction == null)
            {
                if (_isolationLevel.HasValue)
                    _transaction = _dataContext.Database.BeginTransaction(_isolationLevel.GetValueOrDefault());
                else
                    _transaction = _dataContext.Database.BeginTransaction();
            }
        }

        public bool Commit()
        {
            _transaction.Commit();
            return true;
        }

        public void Rollback()
        {
            if (_transaction == null) return;

            _transaction.Rollback();

            _transaction.Dispose();
            _transaction = null;
        }

        public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class
        {
            _dataContext.Entry(entity).State = StateHelper.ConvertState(_dataContext.Entry(entity).State);
        }

        #endregion

    }
}
