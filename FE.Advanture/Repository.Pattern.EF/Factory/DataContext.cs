using Microsoft.EntityFrameworkCore;
using Repository.Pattern.EF.DataContext;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Pattern.EF.Factory
{
    public class DataContext : DbContext, IDataContextAsync
    {
        public DataContext(DbContextOptions context) : base(context) { }
        public int SaveChange() => base.SaveChanges();
        public Task<int> SaveChangesAsync() => base.SaveChangesAsync();
        public override void Dispose()
        {
            base.Dispose();
            GC.SuppressFinalize(this);
        }

    }
}
