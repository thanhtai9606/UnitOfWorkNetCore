using Repository.Pattern.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Repository.Pattern.EF.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        int SaveChanges();
        void Dispose(bool disposing);
        IRepository<TEntity> Repository<TEntity>() where TEntity : class;
        void SyncObjectState<TEntity>(TEntity entity) where TEntity : class;
        void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Unspecified);
        bool Commit();
        void Rollback();
    }
}
